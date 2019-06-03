using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Entity;
using System.Data;

namespace Biz.Common.Data
{
    public static class SQLHelper
    {
        public static Exception LastException;
        public static bool CheckSQLConn(DBSource dbSource)
        {
            if (dbSource == null)
                return false;
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnstringFromDBSource(dbSource,"master")))
                {
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

        public static DataTable GetDBs(DBSource dbSource)
        {
            return ExecuteDBTable(dbSource, "master", SQLHelperConsts.GetDBs, null);
        }

        public static DataTable GetTBs(DBSource dbSource,string dbName)
        {
            return ExecuteDBTable(dbSource, dbName, SQLHelperConsts.GetTBs, null);
        }

        public static DataTable GetKeys(DBSource dbSource, string dbName, string tbName)
        {
            return ExecuteDBTable(dbSource, dbName, SQLHelperConsts.GetKeyColumn, new SqlParameter("@TABLE_NAME", tbName));
        }

        public static string GetIdColumName(DBSource dbSource, string dbName, string tbName)
        {
            var tb= ExecuteDBTable(dbSource, dbName, SQLHelperConsts.GetIdColumn, new SqlParameter("@TABLE_NAME", tbName));
            if (tb.Rows.Count == 0)
                return string.Empty;
            return tb.Rows[0][0].ToString();
        }

        /// <summary>
        /// 取表设计时字段的描述值
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="dbName"></param>
        /// <returns>
        /// [TableName]，[ColumnName]，[Description]
        /// </returns>
        public static DataTable GetTableColsDescription(DBSource dbSource, string dbName,string tbName=null)
        {
            return ExecuteDBTable(dbSource, dbName, SQLHelperConsts.GetTableColsDescription, tbName == null ? null : new SqlParameter("@TbName", tbName));
        }

        public static IEnumerable<TBColumn> GetColumns(DBSource dbSource, string dbName, string tbid,string tbName)
        {
            var tb= ExecuteDBTable(dbSource, dbName, SQLHelperConsts.GetColumns, new SqlParameter("@id", tbid));
            //查主键
            var tb2 = GetKeys(dbSource, dbName, tbName);
            //查自增键
            string idColumnName = GetIdColumName(dbSource, dbName, tbName);
            //描述
            DataTable tbDesc = GetTableColsDescription(dbSource, dbName,tbName);
            
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                var y=(from x in tbDesc.AsEnumerable()
                      where string.Equals((string)x["ColumnName"],(string)tb.Rows[i]["name"],StringComparison.OrdinalIgnoreCase)
                      select x["Description"]).FirstOrDefault();
                yield return new TBColumn
                {
                    IsKey = (tb2.AsEnumerable()).FirstOrDefault(p=>p[0].ToString().Equals(tb.Rows[i]["name"].ToString(),StringComparison.OrdinalIgnoreCase))!=null,
                    Length=int.Parse(tb.Rows[i]["length"].ToString()),
                    Name=tb.Rows[i]["name"].ToString(),
                    TypeName = tb.Rows[i]["type"].ToString(),
                    IsID=string.Equals(idColumnName,tb.Rows[i]["name"].ToString()),
                    IsNullAble = tb.Rows[i]["isnullable"].ToString().Equals("1"),
                    prec=NumberHelper.CovertToInt(tb.Rows[i]["prec"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["scale"]),
                    Description=y==null?"":y.ToString(),
                };
            }
        }

        public static DataTable ExecuteDBTable(DBSource dbSource, string connDB, string sql, params SqlParameter[] sqlParams)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(GetConnstringFromDBSource(dbSource, connDB));
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 180;
            if (sqlParams != null)
            {
               cmd.Parameters.AddRange(sqlParams);
            }
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            DataTable tb=new DataTable();
            ada.Fill(tb);
            return tb;
        }

        public static void ExecuteNoQuery(DBSource dbSource, string connDB, string sql, params SqlParameter[] sqlParams)
        {
            var conn= new SqlConnection(GetConnstringFromDBSource(dbSource, connDB));
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (sqlParams != null)
            {
                cmd.Parameters.AddRange(sqlParams);
            }
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="dbName"></param>
        /// <param name="tabName"></param>
        public static void DeleteTable(DBSource dbSource, string dbName, string tabName)
        {
            if (dbSource == null)
                return;
            //string delSql = "use ["+dbName+"]\r\n";
            string delSql = "drop table ["+tabName+"]";
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }
        /// <summary>
        /// 修改表名
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="dbName"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void ReNameTableName(DBSource dbSource, string dbName, string oldName, string newName)
        {
            if (dbSource == null)
                return;

            string sql = "EXEC sp_rename @oldname,@newname";
            ExecuteNoQuery(dbSource, dbName, sql, new SqlParameter("@oldname", oldName),
                new SqlParameter("@newname", newName));
        }
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="dbName"></param>
        public static void DeleteDataBase(DBSource dbSource, string dbName)
        {
            if (dbSource == null)
                return;
            string delSql = "alter database "+dbName+" set single_user with rollback immediate \r\ndrop database [" + dbName + "]";
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }

        public static void CreateDataBase(DBSource dbSource, string dbName,string newDBName)
        {
            if (dbSource == null)
                return;
            string sql = "create database ["+newDBName+"]";
            ExecuteNoQuery(dbSource, dbName, sql, null);
        }

        public static string GetConnstringFromDBSource(DBSource dbSource,string connDB)
        {
            if (dbSource == null)
                return null;
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = dbSource.ServerName;
            sb.InitialCatalog = connDB;
            sb.Pooling = true;
            sb.MaxPoolSize = 20;
            sb.MinPoolSize = 10;
            sb.ConnectTimeout = 30;
            if (dbSource.IDType == IDType.sqlserver)
            {
                sb.UserID = dbSource.LoginName;
                sb.Password = dbSource.LoginPassword;
                return sb.ConnectionString;
            }
            else
            {
                sb.Add("Trusted_Connection", "SSPI");
                return sb.ConnectionString;
            }
        }

        public static bool DeleteItem(DBSource dbSource, string connDB, string tableName, List<KeyValuePair<string, object>> delKeys)
        {
            string delSql = string.Format("delete from {0} where ",tableName);
            delSql += string.Join(" AND ",delKeys.Select(p => p.Key +"=@" + p.Key));
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddRange(delKeys.Select(p =>new SqlParameter("@"+p.Key,p.Value)));
            ExecuteNoQuery(dbSource, connDB, delSql, parameters.ToArray());
            return true;
        }

        public static IEnumerable<string> GetProcedures(DBSource dbSource, string dbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, "select name from dbo.sysobjects where OBJECTPROPERTY(id, N'IsProcedure') = 1 order by name");

            var y = from x in tb.AsEnumerable()
                    select x["name"] as string;

            return y;
        }

        public static string GetProcedureBody(DBSource dbSource, string dbName, string procedure)
        {
            //show create {procedure|function} sp_name
            string sql = string.Format("sp_helptext  '{0}'", procedure);

            var tb = ExecuteDBTable(dbSource, dbName, sql);
            StringBuilder sb = new StringBuilder();

            foreach(DataRow row in tb.Rows)
            {
                sb.AppendLine((string)row["Text"]);
            }
            

            return sb.ToString();
        }

        public static List<IndexEntry> GetIndexs(DBSource dbSource, string dbName, string tabname)
        {
            var indexs = new List<IndexEntry>();
            string sql = string.Format(@"SELECT  a.name Key_name,d.name Column_name,b.keyno Seq_in_index from
sysindexes a 
left join sysindexkeys  b  on a.id=b.id 
left JOIN  sysobjects  c  ON  b.id = c.id
left JOIN  syscolumns  d  ON  b.id = d.id 
WHERE  a.indid=b.indid and b.colid = d.colid and  c.xtype = 'U'
AND  c.name = '{0}' ", tabname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            var x = from row in tb.AsEnumerable()
                    group row by row.Field<string>("Key_name") into pp
                    select new IndexEntry
                    {
                        IndexName = pp.Key,
                        Cols = pp.OrderBy(c => c.Field<object>("Seq_in_index")).Select(c => c.Field<string>("Column_name")).ToArray()
                    };

            return x.ToList();
        }
    }
}
