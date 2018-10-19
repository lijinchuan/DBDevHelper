using Entity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Biz.Common.Data
{
    public class OracleHelper
    {
        public static string GetConnstringFromDBSource(DBSource dbSource, string connDB)
        {
            //2.5.194.3
            if (dbSource.IDType == IDType.uidpwd)
            {
                var connstr = string.Format(@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = {2})
    ));User Id={3};Password={4};",
                    dbSource.ServerName, dbSource.Port,dbSource.DBName, dbSource.LoginName, dbSource.LoginPassword);

                return connstr;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        //public static string GetConnstringFromDBSource(DBSource dbSource, string connDB)
        //{
        //    if (dbSource == null)
        //        return null;

        //    OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();
        //    sb.PersistSecurityInfo = true;
        //    sb["Data Source"] = string.Format("(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))", dbSource.ServerName, dbSource.Port);
        //    sb.Pooling = true;
        //    sb.LoadBalanceTimeout = 30;
        //    sb.OmitOracleConnectionName = true;
            
        //    sb.MinPoolSize = 1;
        //    sb.MaxPoolSize = 5;

        //    if (dbSource.IDType == IDType.uidpwd)
        //    {
        //        sb.UserID = dbSource.LoginName;
        //        sb.Password = dbSource.LoginPassword;
        //        return sb.ConnectionString.Replace("\"","");
        //    }
        //    else
        //    {
        //        sb.IntegratedSecurity = true;
        //        return sb.ConnectionString.Replace("\"", "");
        //    }
        //}

        public static Exception LastException;
        public static bool CheckSQLConn(DBSource dbSource)
        {
            if (dbSource == null)
                return false;
            try
            {

                using (var conn = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection())
                {

                    conn.ConnectionString = GetConnstringFromDBSource(dbSource, "");
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
                return false;
            }
        }

        public static DataTable ExecuteDBTable(DBSource dbSource, string connDB, string sql, params Oracle.ManagedDataAccess.Client.OracleParameter[] sqlParams)
        {
            using (Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand())
            {
                DataTable tb = new DataTable();

                //MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = new Oracle.ManagedDataAccess.Client.OracleConnection(GetConnstringFromDBSource(dbSource, connDB));
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                if (sqlParams != null)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                Oracle.ManagedDataAccess.Client.OracleDataAdapter ada = new Oracle.ManagedDataAccess.Client.OracleDataAdapter(cmd);

                ada.Fill(tb);

                return tb;
            }
        }

        public static DataTable GetDBs(DBSource dbSource)
        {
            var dt = ExecuteDBTable(dbSource, null, OracleHelperConsts.GetDBs, null);
            if (dt.Columns.Count > 0)
            {
                dt.Columns[0].ColumnName = "name";
                dt = dt.AsEnumerable().CopyToDataTable();
            }
            return dt;
        }

        public static DataTable GetTBs(DBSource dbSource, string dbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, string.Format(OracleHelperConsts.GetTBs, dbName), new OracleParameter(":u",dbSource.LoginName));
            tb.Columns[0].ColumnName = "name";
            return tb;
        }

        public static IEnumerable<TBColumn> GetColumns(DBSource dbSource, string dbName, string tbName)
        {
            //TABLE_NAME	COLUMN_NAME	DATA_TYPE	DATA_TYPE_MOD	DATA_TYPE_OWNER	DATA_LENGTH	DATA_PRECISION	DATA_SCALE	NULLABLE	COLUMN_ID	DEFAULT_LENGTH	DATA_DEFAULT	NUM_DISTINCT
		//LOW_VALUE	HIGH_VALUE	DENSITY	NUM_NULLS	NUM_BUCKETS	LAST_ANALYZED	SAMPLE_SIZE	CHARACTER_SET_NAME	CHAR_COL_DECL_LENGTH	GLOBAL_STATS	USER_STATS	
		//AVG_COL_LEN	CHAR_LENGTH	CHAR_USED	V80_FMT_IMAGE	DATA_UPGRADED	HISTOGRAM
            var tb = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetColumns, new OracleParameter(":tb", tbName));

            var tb2 = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetTableColsDescription, new OracleParameter(":tb", tbName));
            var dic = new Dictionary<string, string>();
            foreach (DataRow row in tb2.Rows)
            {
                dic.Add(row[0].ToString(), row[1].ToString());
            }

            var tb3 = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetKeyCols, new OracleParameter(":tb", tbName));
            var keys =new HashSet<string>();
            foreach (DataRow row in tb3.Rows)
            {
                var keycolname = row["COLUMN_NAME"].ToString();
                if (!keys.Contains(keycolname))
                {
                    keys.Add(keycolname);
                }
                keys.Add(keycolname);
            }
            //var idColumnName = GetAutoIncrementColName(dbSource, dbName, tbName);


            for (int i = 0; i < tb.Rows.Count; i++)
            {
                var desc = string.Empty;
                dic.TryGetValue(tb.Rows[i]["COLUMN_NAME"].ToString(), out desc);
                yield return new TBColumn
                {
                    IsKey = keys.Contains(tb.Rows[i]["COLUMN_NAME"].ToString()),
                    Length = int.Parse(string.IsNullOrEmpty(tb.Rows[i]["DATA_LENGTH"].ToString()) ? "0" : tb.Rows[i]["CHAR_LENGTH"].ToString()),
                    Name = tb.Rows[i]["COLUMN_NAME"].ToString(),
                    TypeName = tb.Rows[i]["DATA_TYPE"].ToString(),
                    //IsID = string.Equals(idColumnName, tb.Rows[i]["column_name"].ToString()),
                    IsNullAble = tb.Rows[i]["NULLABLE"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase),
                    prec = NumberHelper.CovertToInt(tb.Rows[i]["DATA_PRECISION"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["DATA_SCALE"]),
                    Description = desc,
                };
            }
        }

        public static void ExecuteNoQuery(DBSource dbSource, string connDB, string sql, params OracleParameter[] sqlParams)
        {
            using (var conn = new OracleConnection(GetConnstringFromDBSource(dbSource, connDB)))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    DataTable tb = new DataTable();

                    //MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    if (sqlParams != null)
                    {
                        cmd.Parameters.AddRange(sqlParams);
                    }
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                }
            }

        }

        public static List<IndexEntry> GetIndexs(DBSource dbSource, string dbName, string tabname)
        {
            var indexs = new List<IndexEntry>();
            //string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetIndexs,new OracleParameter(":tb",tabname));

            foreach (DataRow row in tb.Rows)
            {
                var indexname = row["INDEX_NAME"].ToString();
                var tbcols = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetIndexCols, new OracleParameter(":tb", tabname), new OracleParameter(":idxname", indexname));
                if (tbcols.Rows.Count > 0)
                {
                    indexs.Add(new IndexEntry
                    {
                        IndexName = indexname,
                        Cols=tbcols.AsEnumerable().Select(p=>p["COLUMN_NAME"].ToString()).ToArray()
                    });
                }
            }

            //var x = from row in tb.AsEnumerable()
            //        group row by row.Field<string>("Key_name") into pp
            //        select new IndexEntry
            //        {
            //            IndexName = pp.Key,
            //            Cols = pp.OrderBy(c => c.Field<object>("Seq_in_index")).Select(c => c.Field<string>("Column_name")).ToArray()
            //        };

            //return x.ToList();

            return indexs;
        }

        public static void DeleteTable(DBSource dbSource, string dbName, string tabName)
        {
            if (dbSource == null)
                return;

            string delSql = "drop table " + tabName;
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }
    }
}
