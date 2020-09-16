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

        public static DataSet ExecuteDataSet(DBSource dbSource, string connDB, string sql, OracleInfoMessageEventHandler onmsg, params OracleParameter[] sqlParams)
        {
            using (var conn = new OracleConnection(GetConnstringFromDBSource(dbSource, connDB)))
            {
                conn.InfoMessage += onmsg;

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 180;
                    if (sqlParams != null)
                    {
                        cmd.Parameters.AddRange(sqlParams);
                    }
                    OracleDataAdapter ada = new OracleDataAdapter(cmd);
                    DataSet ts = new DataSet();
                    ada.Fill(ts);

                    return ts;
                }
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
                var column=tb.Rows[i]["COLUMN_NAME"].ToString();
                dic.TryGetValue(column, out desc);
                yield return new TBColumn
                {
                    IsKey = keys.Contains(column),
                    Length = int.Parse(string.IsNullOrEmpty(tb.Rows[i]["DATA_LENGTH"].ToString()) ? "0" : tb.Rows[i]["CHAR_LENGTH"].ToString()),
                    Name = column,
                    TypeName = tb.Rows[i]["DATA_TYPE"].ToString(),
                    //IsID = string.Equals(idColumnName, tb.Rows[i]["column_name"].ToString()),
                    IsNullAble = tb.Rows[i]["NULLABLE"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase),
                    prec = NumberHelper.CovertToInt(tb.Rows[i]["DATA_PRECISION"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["DATA_SCALE"]),
                    Description = desc,
                    DBName = dbName,
                    TBName = tbName
                };
            }
        }

        public static int ExecuteNoQuery(DBSource dbSource, string connDB, string sql, params OracleParameter[] sqlParams)
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
                    return cmd.ExecuteNonQuery();

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

        public static List<string> GetProcList(DBSource dbSource)
        {
            var result = new List<string>();
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetProcListSql, null);

            foreach (DataRow row in tb.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static string GetProcedureBody(DBSource dbSource, string procedure)
        {
            
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetProcBodySql, new OracleParameter(":name", procedure));
            StringBuilder sb = new StringBuilder();

            foreach (DataRow item in tb.AsEnumerable())
            {
                sb.AppendLine(item[0].ToString());
            }

            return sb.ToString();
        }

        public static List<string> GetTriggers(DBSource dbSource, string dbName, string tabname)
        {
            var result = new List<string>();
            //string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetTriggerSql, new OracleParameter(":tb", tabname));

            foreach (DataRow row in tb.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static string GetTriggerDetail(DBSource dbSource, string triggername)
        {
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetTriggerDetailSql, new OracleParameter(":name", triggername));
            StringBuilder sb = new StringBuilder();

            foreach (DataRow item in tb.AsEnumerable())
            {
                sb.AppendLine(item[0].ToString());
            }

            return sb.ToString();
        }

        public static List<string> GetViews(DBSource dbSource, string dbName, string tabname)
        {
            var result = new List<string>();
            //string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetViewListSql, null);

            foreach (DataRow row in tb.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static string GetViewDetail(DBSource dbSource, string viewname)
        {
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetViewBodySql, new OracleParameter(":name", viewname));
            StringBuilder sb = new StringBuilder();

            foreach (DataRow item in tb.AsEnumerable())
            {
                sb.AppendLine(item[0].ToString());
            }

            return sb.ToString();
        }

        public static List<string> GetMViews(DBSource dbSource, string dbName, string tabname)
        {
            var result = new List<string>();
            //string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, dbName, OracleHelperConsts.GetMViewListSql, null);

            foreach (DataRow row in tb.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static string GetMViewDetail(DBSource dbSource, string viewname)
        {
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetMViewBodySql, new OracleParameter(":name", viewname));
            StringBuilder sb = new StringBuilder();

            foreach (DataRow item in tb.AsEnumerable())
            {
                sb.AppendLine(item[0].ToString());
            }

            return sb.ToString();
        }

        public static List<string> GetJobs(DBSource dbSource)
        {
            var result = new List<string>();
            //string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetJOBListSql, null);

            foreach (DataRow row in tb.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static string GetJobDetail(DBSource dbSource, string jobname)
        {
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetJOBBodySql, new OracleParameter(":name", jobname));
            StringBuilder sb = new StringBuilder();

            foreach (DataRow item in tb.AsEnumerable())
            {
                sb.AppendLine(item[0].ToString());
            }

            return sb.ToString();
        }

        public static List<string> GetSeqs(DBSource dbSource)
        {
            var result = new List<string>();
            //string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetUserSeqListSql,null);

            foreach (DataRow row in tb.Rows)
            {
                result.Add(row[0].ToString());
            }

            return result;
        }

        public static string GetSeqBody(DBSource dbSource,string seqname)
        {
            
            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetSeqBodySql, new OracleParameter(":name",seqname));

            if (tb.Rows.Count > 0)
            {
                return tb.Rows[0][0].ToString();
            }

            return string.Empty;
        }

        public static void DeleteTable(DBSource dbSource, string dbName, string tabName)
        {
            if (dbSource == null)
                return;

            string delSql = "drop table " + tabName;
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }

        public static string GetCreateTableSql(DBSource dbSource, string tabName)
        {
            if (dbSource == null)
            {
                return string.Empty;
            }

            var tb = ExecuteDBTable(dbSource, string.Empty, OracleHelperConsts.GetCreateTableSql,
                new OracleParameter(":tb", tabName));

            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in tb.AsEnumerable())
            {
                sb.AppendLine(row[0].ToString());
            }

            return sb.ToString();
        }

        public static bool DeleteItem(DBSource dbSource, string connDB, string tableName, List<KeyValuePair<string, object>> delKeys)
        {
            string delSql = string.Format("delete from {0} where ", tableName);
            delSql += string.Join(" AND ", delKeys.Select(p => p.Key + "=:" + p.Key));
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.AddRange(delKeys.Select(p => new OracleParameter(":" + p.Key, p.Value)));
            ExecuteNoQuery(dbSource, connDB, delSql, parameters.ToArray());
            return true;
        }

        public static void CreateIndex(DBSource dbSource, string dbName, string tabname, string indexname, bool unique, bool primarykey, bool autoIncr, List<TBColumnIndex> cols)
        {
            string sql = string.Empty;

            if (autoIncr)
            {
                if (cols.Count != 1)
                {
                    throw new Exception("只能有一个自增长键");
                }

                if (primarykey)
                {
                    //sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD PRIMARY KEY ({2}) ", dbName, tabname, string.Join(",", cols.Select(p => "`" + p.Name + "`")));

                    sql = string.Format("alter table {0} add constraint pk_id primary key({1})", tabname, cols[0].Name);
                    ExecuteNoQuery(dbSource, dbName, sql, null);
                }

                //sql = string.Format("alter table `{0}`.`{1}` modify `{2}` {3} auto_increment;", dbName, tabname, cols.First().Name, cols.First().TypeName);
                sql = string.Format(@"create sequence seq_{0} 
                                      start with 1    
                                      increment by 1
                                      NOCYCLE -- 一直累加，不循环
                                      nocache --不建缓冲区", tabname);


                ExecuteNoQuery(dbSource, dbName, sql, null);

                sql = string.Format(@"create or replace TRIGGER TRIG_{0}
                        before insert on {0}
                          for each row
                               begin
                                  select seq_{0}.nextval into:new.{1} from dual;
                               end;",tabname,cols[0].Name);
                ExecuteNoQuery(dbSource, dbName, sql, null);
                return;
            }
            else
            {
                if (primarykey)
                {
                    //sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD PRIMARY KEY ({2}) ", dbName, tabname, string.Join(",", cols.Select(p => "`" + p.Name + "`")));
                    sql = string.Format("alter table {0} add constraint pk_id primary key({1})", tabname, string.Join(",", cols.Select(p => p.Name)));
                    ExecuteNoQuery(dbSource, dbName, sql, null);
                }
                else if (unique)
                {
                    sql = string.Format("create UNIQUE index {0} on {1}({2})", indexname, tabname, string.Join(",", cols.Select(p => p.Name + " " + (p.Direction == 1 ? "ASC" : "DESC"))));
                }
                else
                {
                    //sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD INDEX {2}({3}) ", dbName, tabname, indexname, string.Join(",", cols.Select(p => "`" + p.Name + "`")));

                    sql = string.Format("create index {0} on {1}({2})", indexname, tabname, string.Join(",", cols.Select(p => p.Name + " " + (p.Direction == 1 ? "ASC" : "DESC"))));
                }

                //System.Windows.Forms.MessageBox.Show(sql);
                ExecuteNoQuery(dbSource, dbName, sql, null);
            }


        }

        public static void DropIndex(DBSource dbSource, string dbName, string tbName, bool primarykey, string indexName)
        {
            string sql = null;
            if (primarykey)
            {
                sql = string.Format("ALTER TABLE {0} DROP PRIMARY KEY", tbName);
            }
            else
            {
                sql = string.Format("drop index {0}", indexName);
            }

            ExecuteNoQuery(dbSource, dbName, sql, null);
        }

        public static void ReNameTableName(DBSource dbSource, string dbName, string oldName, string newName)
        {
            if (dbSource == null)
                return;

            string sql = string.Format("alter table {0} rename to {1}", oldName, newName);
            ExecuteNoQuery(dbSource, dbName, sql);
        }

        public static bool DropSeq(DBSource dbSource, string seqname)
        {
            return ExecuteNoQuery(dbSource, string.Empty, OracleHelperConsts.DropSeqSql.Replace(":name", seqname),null) > 0;
        }

        public static bool DropTrigger(DBSource dbSource, string triggername)
        {
            return ExecuteNoQuery(dbSource, string.Empty, OracleHelperConsts.DropTriggerSql.Replace(":name", triggername),null) > 0;
        }

        public static string CreateSelectSql(string dbname, string tbname, string editer, string spabout, List<TBColumn> cols, List<TBColumn> conditioncols, List<TBColumn> outputcols)
        {
            string spname = tbname + "_list"; ;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("/* =============================================");
            sb.AppendLine(string.Format("-- Author:	     {0}", editer));
            sb.AppendLine(string.Format("-- Create date: {0}", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")));
            sb.AppendLine(string.Format("-- Description: {0}", spabout.Replace("\r\n", "\r\n--")));
            sb.AppendLine("=============================================*/");
            sb.AppendFormat(string.Format("CREATE PROCEDURE {0}(", spname));
            sb.AppendLine();
            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendFormat("{0} IN {1}, {2}", col.Name, col.TypeName, string.IsNullOrEmpty(col.Description) ? "" : "/*" + col.Description + "*/");
                sb.AppendLine();
            }
            sb.AppendLine("in OrderBy varchar2 DEFALUT NULL,");
            sb.AppendLine("in pageSize number DEFAULT 10,	/*每页显示记录数*/");
            sb.AppendLine("in pageIndex	number DEFAULT 1, /*第几页*/");
            sb.AppendLine("out number recordCount /*记录总数*/");
            sb.AppendLine(") as ");
            sb.AppendLine("var_where long;");
            sb.AppendLine("var_orderBy varchar2(4000);");

            foreach (var col in conditioncols)
            {
                sb.AppendLine(string.Format("var_{0} varchar2(200);"));
            }

            sb.AppendLine("BEGIN");
            sb.AppendLine();

            sb.AppendLine(" /*拼接sql语句*/");
            //sb.AppendLine("	declare @sql nvarchar(4000)");
            //sb.AppendLine("	declare @where nvarchar(4000)");
            sb.AppendLine("	var_where:=' where 1=1 ';");
            sb.AppendLine();

            string orderBy = string.Join(",", cols.Where(p => p.IsKey).Select(p => p.Name + " DESC"));
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = cols.Where(p => p.IsID).Select(p => p.Name + " DESC").FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                sb.AppendLine("var_orderBy:=nvl(OrderBy,var_orderBy);");
                sb.AppendLine("var_orderBy: =' order by '||var_orderBy;");
            }
            else
            {
                sb.AppendLine("case when OrderBy is not null then var_orderBy:=' order by '||OrderBy else var_orderBy:=' ' end");
            }

            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendLine(string.Format("/*{0}*/", string.IsNullOrWhiteSpace(col.Description) ? col.Name : col.Description));
                sb.AppendLine(string.Format("	if {0} is not null then", col.Name));
                sb.AppendLine("	begin");
                sb.AppendLine();
                if (col.IsString())
                {
                    sb.AppendLine(string.Format("	    var_{0} :='%'||{0}||'%');", col.Name));
                    sb.AppendLine(string.Format("	    var_where:=var_where||' and {0} like var_{0}  ';", col.Name));
                }
                else if (col.IsNumber() || col.IsBoolean())
                {
                    sb.AppendLine(string.Format("       var_where:=var_where||' and {0}={0}  ';", col.Name));
                }
                else if (col.IsDateTime())
                {
                    //sb.AppendLine(string.Format("       set @{0} =concat(''',{0} ,''');", col.Name));
                    sb.AppendLine(string.Format("       var_where:=var_where||' and {0}={0}  ';", col.Name));
                }
                else
                {
                    sb.AppendLine("       /*add code*/");
                }

                sb.AppendLine(" end;");
                sb.AppendLine(" else");
                sb.AppendLine(" begin");
                sb.AppendLine(string.Format("       set @{0}=true;", col.Name));
                sb.AppendLine("       set @where=concat(@where,' and ?');");
                sb.AppendLine(" end;");
                sb.AppendLine(" end if;");
                sb.AppendLine();
            }
            sb.AppendLine("set @recordCount=0;");
            sb.AppendLine(string.Format("   set @sql=concat('select count(1) into @recordCount From {0} ',@where);", tbname));

            sb.AppendLine("prepare stmt from @sql;");
            sb.AppendLine(string.Format("execute stmt using {0};", string.Join(",", conditioncols.Select(p => "@" + p.Name))));
            //sb.AppendLine("OrderBy;");
            sb.AppendLine("DEALLOCATE PREPARE stmt;");
            sb.AppendLine();
            //foreach (var col in cols)

            sb.AppendLine("set recordCount=ifnull(@recordCount,0);");

            sb.AppendLine();
            sb.AppendLine("set @limit=(@pageIndex-1)*@pageSize;");
            sb.Append(" set @sql = concat('");
            sb.Append(string.Format("	Select {0} From ", string.Join(",", outputcols.Select(p => "`" + p.Name + "`"))));
            sb.Append(string.Format("	`{0}` ',@where", tbname));
            sb.AppendLine(", @OrderBy,' limit ?,?');");
            sb.AppendLine();
            sb.AppendLine("prepare stmt from @sql;");

            sb.Append(string.Format("execute stmt using {0},", string.Join(",", conditioncols.Select(p => "@" + p.Name))));
            sb.AppendLine("@limit,@pageSize;");
            sb.AppendLine("DEALLOCATE PREPARE stmt;");
            sb.AppendLine("");
            sb.AppendLine("");

            sb.AppendLine("END$$");
            sb.AppendLine("DELIMITER ;");

            return sb.ToString();
        }

        public static IEnumerable<string> GetUsers(DBSource dbsource)
        {
            DataTable tb = ExecuteDBTable(dbsource, string.Empty, OracleHelperConsts.GetUserSql, null);

            foreach (DataRow row in tb.Rows)
            {
                yield return row[0].ToString();
            }
        }

        public static bool AuthAllUser(DBSource dbsource, string tbname,string user)
        {
            string sql = string.Format("grant all on {0} to {1}",tbname,user);

            return ExecuteNoQuery(dbsource, string.Empty, sql, null)>0;
        }

        public static bool ReAuthUser(DBSource dbsource,string tbname,string user, bool select, bool update, bool insert, bool delete)
        {
            List<string> auth = new List<string>();
            if (select)
            {
                auth.Add("select");
            }
            if (update)
            {
                auth.Add("update");
            }
            if (insert)
            {
                auth.Add("insert");
            }
            if (delete)
            {
                auth.Add("delete");
            }
            if (auth.Count > 0)
            {
                var sql=string.Format("grant {2} on {0} to {1}",tbname,user,string.Join(",",auth));
                ExecuteNoQuery(dbsource, string.Empty, sql, null);
            }

            auth.Clear();
            if (!select)
            {
                auth.Add("select");
            }
            if (!update)
            {
                auth.Add("update");
            }
            if (!insert)
            {
                auth.Add("insert");
            }
            if (!delete)
            {
                auth.Add("delete");
            }
            if (auth.Count > 0)
            {
                var sql = string.Format("revoke {2} on {0} from {1}", tbname, user, string.Join(",", auth));
                ExecuteNoQuery(dbsource, string.Empty, sql, null);
            }

            return true;
        }

        public static bool ModifyUserPassword(DBSource dbsource, string user, string newpassword)
        {
            string sql = string.Format("alter user {0} identified by {1}",user,newpassword);

            return ExecuteNoQuery(dbsource, string.Empty, sql, null)>0;
        }

        public static void CreateDataBase(DBSource dbSource, string dbName, string newDBName)
        {
            if (dbSource == null)
                return;
            string sql = "create database " + newDBName;
            ExecuteNoQuery(dbSource, dbName, sql, null);
        }

        public static object ExecuteScalar(DBSource dbSource, string connDB, string sql, params OracleParameter[] sqlParams)
        {
            var conn = new OracleConnection(GetConnstringFromDBSource(dbSource, connDB));
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (sqlParams != null && sqlParams.Count() > 0)
            {
                cmd.Parameters.AddRange(sqlParams);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

        }
    }
}
