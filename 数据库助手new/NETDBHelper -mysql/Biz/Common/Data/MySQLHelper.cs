using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Entity;
using System.Text.RegularExpressions;
using static Entity.IndexEntry;
using System.Data.SqlClient;

namespace Biz.Common.Data
{
    public class MySQLHelper
    {
        public static Exception LastException;
        public static bool CheckSQLConn(DBSource dbSource)
        {
            if (dbSource == null)
                return false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GetConnstringFromDBSource(dbSource, "")))
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
            var dt = ExecuteDBTable(dbSource, null, MySqlHelperConsts.GetDBs, null);

            dt.Columns[0].ColumnName = "name";
            dt = dt.AsEnumerable().Where(p => p["name"].ToString() != "information_schema" && p["name"].ToString() != "mysql" && p["name"].ToString() != "performance_schema").CopyToDataTable();
            return dt;
        }

        /// <summary>
        /// 取表设计时字段的描述值
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="dbName"></param>
        /// <returns>
        /// [TableName]，[ColumnName]，[Description]
        /// </returns>
        public static DataTable GetTableColsDescription(DBSource dbSource, string dbName, string tbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, MySqlHelperConsts.GetTableColsDescription, new MySqlParameter("@db", dbName), new MySqlParameter("@tb", tbName));
            tb.Columns["column_name"].ColumnName = "ColumnName";
            tb.Columns["column_comment"].ColumnName = "Description";
            return tb;
        }

        public static DataTable GetTBs(DBSource dbSource, string dbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, string.Format(MySqlHelperConsts.GetTBs, dbName), null);
            tb.Columns[0].ColumnName = "name";
            return tb;
        }

        public static IEnumerable<TBColumn> GetColumns(DBSource dbSource, string dbName, string tbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, MySqlHelperConsts.GetColumns, new MySqlParameter("@db", dbName), new MySqlParameter("@tb", tbName));
            var idColumnName = GetAutoIncrementColName(dbSource, dbName, tbName);
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                long longlen = long.Parse(string.IsNullOrEmpty(tb.Rows[i]["character_maximum_length"].ToString()) ? "0" : tb.Rows[i]["character_maximum_length"].ToString());
                if (longlen > int.MaxValue)
                {
                    longlen = -1;
                }
                yield return new TBColumn
                {
                    IsKey = string.Equals((string)tb.Rows[i]["column_key"], "pri", StringComparison.OrdinalIgnoreCase),
                    Length = (int)longlen,
                    Name = tb.Rows[i]["column_name"].ToString(),
                    TypeName = tb.Rows[i]["data_type"].ToString(),
                    IsID = string.Equals(idColumnName, tb.Rows[i]["column_name"].ToString()),
                    IsNullAble = tb.Rows[i]["is_nullable"].ToString().Equals("yes", StringComparison.OrdinalIgnoreCase),
                    prec = NumberHelper.CovertToInt(tb.Rows[i]["numeric_precision"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["numeric_scale"]),
                    Description = tb.Rows[i]["column_comment"].ToString(),
                    DBName = dbName,
                    TBName = tbName
                };
            }
        }

        public static IEnumerable<string> GetProcedures(DBSource dbSource, string dbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, string.Format("select name from mysql.proc where db='{0}' AND `type` = 'PROCEDURE' ", dbName));

            var y = from x in tb.AsEnumerable()
                    select x["name"] as string;

            return y;
        }

        public static string GetProcedureBody(DBSource dbSource, string dbName, string procedure)
        {
            //show create {procedure|function} sp_name
            string sql = string.Format("show create procedure {0}", procedure);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            if (tb.Rows.Count > 0)
            {
                var body = (string)tb.Rows[0]["Create Procedure"];
                body = Regex.Replace(body, @"\n", "\r\n");
                body = Regex.Replace(body, "(?!\n);", ";\r\n");

                return body;
            }

            return string.Empty;
        }

        public static string GetCreateTableSQL(DBSource dbSource, string dbName, string tbName)
        {
            //show create {procedure|function} sp_name
            string sql = string.Format("show create table {0}", tbName);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            if (tb.Rows.Count > 0)
            {
                var body = (string)tb.Rows[0]["Create Table"];
                body = Regex.Replace(body, @"\n", "\r\n");
                body = Regex.Replace(body, "(?!\n);", "\r\n");

                return body;
            }

            return string.Empty;
        }

        public static string GetCreateViewSQL(DBSource dbSource, string dbName, string viewName)
        {
            //show create {procedure|function} sp_name
            string sql = string.Format("show create view `{0}`.`{1}`", dbName, viewName);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            if (tb.Rows.Count > 0)
            {
                var body = (string)tb.Rows[0]["Create View"];
                body = Regex.Replace(body, @"\n", "\r\n");
                body = Regex.Replace(body, "(?!\n);", "\r\n");

                return body;
            }

            return string.Empty;
        }

        [Obsolete("取不到参数")]
        public static string GetProcedureBody2(DBSource dbSource, string dbName, string procedure)
        {
            string sql = string.Format("select body from mysql.proc where name='{0}' AND `type` = 'PROCEDURE';", procedure);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            if (tb.Rows.Count > 0)
            {
                return Encoding.UTF8.GetString((byte[])tb.Rows[0][0]);
            }

            return string.Empty;
        }

        public static DataTable ExecuteDBTable(DBSource dbSource, string connDB, string sql, params MySqlParameter[] sqlParams)
        {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = new MySqlConnection(GetConnstringFromDBSource(dbSource, connDB));
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (sqlParams != null)
            {
                cmd.Parameters.AddRange(sqlParams);
            }
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataTable tb = new DataTable();
            ada.Fill(tb);
            return tb;
        }

        public static DataSet ExecuteDataSet(DBSource dbSource, string connDB, string sql, MySqlInfoMessageEventHandler onmsg, params MySqlParameter[] sqlParams)
        {
            using (var conn = new MySqlConnection(GetConnstringFromDBSource(dbSource, connDB)))
            {
                conn.InfoMessage += onmsg;

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 180;
                    if (sqlParams != null)
                    {
                        cmd.Parameters.AddRange(sqlParams);
                    }
                    MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                    DataSet ts = new DataSet();
                    ada.Fill(ts);

                    return ts;
                }
            }
        }

        public static string GetConnstringFromDBSource(DBSource dbSource, string connDB, bool? sqlServerMode = true, bool? allowBatch = true, bool? allowLoadLocalInfile = false)
        {
            if (dbSource == null)
                return null;
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = dbSource.ServerName;
            sb.Port = (uint)dbSource.Port;
            sb.Database = connDB;
            sb.Pooling = true;
            sb.ConnectionTimeout = 30;
            if (sqlServerMode.HasValue)
            {
                sb.SqlServerMode = sqlServerMode.Value;
            }
            if (allowBatch.HasValue)
            {
                sb.AllowBatch = allowBatch.Value;
            }
            sb.MinimumPoolSize = 1;
            sb.MaximumPoolSize = 5;
            sb.AllowUserVariables = true;
            if (allowLoadLocalInfile.HasValue)
            {
                sb.AllowLoadLocalInfile = allowLoadLocalInfile.Value;
            }

            if (dbSource.IDType == IDType.uidpwd)
            {
                sb.UserID = dbSource.LoginName;
                sb.Password = dbSource.LoginPassword;
                return sb.ConnectionString;
            }
            else
            {
                throw new Exception("仅支持用户名密码登录模式");
            }
        }

        public static void ExecuteNoQuery(DBSource dbSource, string connDB, string sql, params MySqlParameter[] sqlParams)
        {
            var conn = new MySqlConnection(GetConnstringFromDBSource(dbSource, connDB) + ";allowuservariables=True;");
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (sqlParams != null && sqlParams.Count() > 0)
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

        public static object ExecuteScalar(DBSource dbSource, string connDB, string sql, params MySqlParameter[] sqlParams)
        {
            var conn = new MySqlConnection(GetConnstringFromDBSource(dbSource, connDB) + ";allowuservariables=True;");
            MySqlCommand cmd = conn.CreateCommand();
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

        public static void CreateDataBase(DBSource dbSource, string dbName, string newDBName)
        {
            if (dbSource == null)
                return;
            string sql = "create database [" + newDBName + "]";
            ExecuteNoQuery(dbSource, dbName, sql, null);
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

            string sql = string.Format("alter table {0} rename {1}", oldName, newName);
            ExecuteNoQuery(dbSource, dbName, sql);
        }

        public static void DeleteTable(DBSource dbSource, string dbName, string tabName)
        {
            if (dbSource == null)
                return;

            string delSql = "drop table " + tabName + "";
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }

        public static void DeleteDataBase(DBSource dBSource, string database)
        {
            if (dBSource == null)
                return;

            string delSql = "drop database " + database;
            ExecuteNoQuery(dBSource, database, delSql);
        }

        public static DataTable GetKeys(DBSource dBSource, string dbname, string tbname)
        {
            var list = GetColumns(dBSource, dbname, tbname);

            DataTable tb = new DataTable("tbkeys_" + dbname + "_" + tbname);

            tb.Columns.AddRange(new DataColumn[] { new DataColumn("COLUMN_NAME") });

            var key = list.FirstOrDefault(p => p.IsKey);
            if (key != null)
            {
                var newrow = tb.NewRow();
                newrow["COLUMN_NAME"] = key.Name;
                tb.Rows.Add(newrow);
            }
            else
            {
                foreach (var item in list.Where(p => p.IsKey))
                {
                    var newrow = tb.NewRow();
                    newrow["COLUMN_NAME"] = item.Name;
                    tb.Rows.Add(newrow);
                }
            }

            return tb;
        }

        public static bool DeleteItem(DBSource dbSource, string connDB, string tableName, List<KeyValuePair<string, object>> delKeys)
        {
            string delSql = string.Format("delete from {0} where ", tableName);
            delSql += string.Join(" AND ", delKeys.Select(p => p.Key + "=@" + p.Key));
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.AddRange(delKeys.Select(p => new MySqlParameter("@" + p.Key, p.Value)));
            ExecuteNoQuery(dbSource, connDB, delSql, parameters.ToArray());
            return true;
        }

        public static string CreateSelectSql(string dbname, string tbname, string editer, string spabout, List<TBColumn> cols, List<TBColumn> conditioncols, List<TBColumn> outputcols)
        {
            string spname = tbname + "_list"; ;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("USE {0};", dbname);
            sb.AppendLine();
            sb.AppendLine(string.Format("drop PROCEDURE if exists {0};", spname));
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/* =============================================");
            sb.AppendLine(string.Format("-- Author:	     {0}", editer));
            sb.AppendLine(string.Format("-- Create date: {0}", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")));
            sb.AppendLine(string.Format("-- Description: {0}", spabout.Replace("\r\n", "\r\n--")));
            sb.AppendLine("=============================================*/");
            sb.AppendLine("DELIMITER $$");
            sb.AppendFormat(string.Format("CREATE PROCEDURE {0}(", spname));
            sb.AppendLine();
            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendFormat("IN {0} {1}, {2}", col.Name, col.TypeToString(), string.IsNullOrEmpty(col.Description) ? "" : "/*" + col.Description + "*/");
                sb.AppendLine();
            }
            sb.AppendLine("in OrderBy varchar(200),");
            sb.AppendLine("in pageSize int,	/*每页显示记录数*/");
            sb.AppendLine("in pageIndex	int, /*第几页*/");
            sb.AppendLine("out recordCount int /*记录总数*/");
            sb.AppendLine(")");
            sb.AppendLine("BEGIN");
            sb.AppendLine();

            sb.Append("set @OrderBy=OrderBy,@pageSize=ifnull(pageSize,20),@pageIndex=ifnull(pageIndex,1),");
            sb.Append(string.Join(",", conditioncols.Select(p => "@" + p.Name + "=" + p.Name)));
            sb.AppendLine(";");

            sb.AppendLine(" /*拼接sql语句*/");
            //sb.AppendLine("	declare @sql nvarchar(4000)");
            //sb.AppendLine("	declare @where nvarchar(4000)");
            sb.AppendLine("	set @where=' where 1=1 ';");
            sb.AppendLine();

            string orderBy = string.Join(",", cols.Where(p => p.IsKey).Select(p => p.Name + " DESC"));
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = cols.Where(p => p.IsID).Select(p => p.Name + " DESC").FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                sb.AppendLine("   if @OrderBy is null then");
                sb.AppendLine("   begin");
                sb.AppendLine(string.Format("      set @OrderBy=' order by {0}';", orderBy));
                sb.AppendLine("   end;");
                sb.AppendLine("else");
                sb.AppendLine("set @OrderBy =concat(' order by ',@OrderBy);");
                sb.AppendLine("   end if;");
            }

            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendLine(string.Format("/*{0}*/", string.IsNullOrWhiteSpace(col.Description) ? col.Name : col.Description));
                sb.AppendLine(string.Format("	if @{0} is not null then", col.Name));
                sb.AppendLine("	begin");
                sb.AppendLine();
                if (col.IsString())
                {
                    sb.AppendLine(string.Format("	    set @{0} =concat('%',{0},'%');", col.Name));
                    sb.AppendLine(string.Format("	    set @where=concat(@where,' and `{0}` like ?  ');", col.Name));
                }
                else if (col.IsNumber() || col.IsBoolean())
                {
                    sb.AppendLine(string.Format("       set @where=concat(@where,' and `{0}`=?  ');", col.Name));
                }
                else if (col.IsDateTime())
                {
                    //sb.AppendLine(string.Format("       set @{0} =concat(''',{0} ,''');", col.Name));
                    sb.AppendLine(string.Format("       set @where=concat(@where,' and `{0}`=?  ');", col.Name));
                }
                else if (col.IsEnum())
                {
                    sb.AppendLine(string.Format("       set @where=concat(@where,' and `{0}`=?  ');", col.Name));
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

        public static void CreateIndex(DBSource dbSource, string dbName, string tabname, string indexname, bool unique, bool primarykey, bool autoIncr, List<IndexTBColumn> cols)
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
                    sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD PRIMARY KEY ({2}) ", dbName, tabname, string.Join(",", cols.Select(p => "`" + p.Name + "`")));
                    ExecuteNoQuery(dbSource, dbName, sql, null);
                }

                sql = string.Format("alter table `{0}`.`{1}` modify `{2}` {3} auto_increment;", dbName, tabname, cols.First().Name, cols.First().TypeName);
                ExecuteNoQuery(dbSource, dbName, sql, null);

                return;
            }
            else
            {
                if (primarykey)
                {
                    sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD PRIMARY KEY ({2}) ", dbName, tabname, string.Join(",", cols.Select(p => "`" + p.Name + "`")));
                }
                else if (unique)
                {
                    sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD UNIQUE ({2})", dbName, tabname, string.Join(",", cols.Select(p => "`" + p.Name + "`")));
                }
                else
                {
                    sql = string.Format("ALTER TABLE `{0}`.`{1}` ADD INDEX {2}({3}) ", dbName, tabname, indexname, string.Join(",", cols.Select(p => "`" + p.Name + "`" + (p.Order == -1 ? " DESC" : ""))));
                }

                ExecuteNoQuery(dbSource, dbName, sql, null);
            }


        }

        public static string GetAutoIncrementColName(DBSource dbSource, string dbName, string tabname)
        {
            //string sql=string.Format("select * from information_schema.`TABLES` where table_name='{0}' and TABLE_SCHEMA='{1}'",tabname,dbName);

            //            string sql = string.Format(@"SELECT
            //  TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME
            //FROM
            //  information_schema.KEY_COLUMN_USAGE", dbName, tabname);

            string sql = string.Format("select COLUMN_NAME FROM information_schema.COLUMNS where TABLE_SCHEMA='{0}' and TABLE_NAME='{1}' and EXTRA='auto_increment'", dbName, tabname);

            var tb = ExecuteDBTable(dbSource, dbName, sql, null);

            if (tb.Rows.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return tb.Rows[0][0].ToString();
            }
        }

        public static DataTable GetFunctions(DBSource dbSource, string dbName)
        {
            var sql = $@"select `name` from mysql.proc where db = '{dbName}' and `type` = 'FUNCTION' order by `name`";

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            return tb;
        }

        public static DataTable GetFunctionsWithParams(DBSource dbSource, string dbName)
        {
            //var sql = @"SELECT ao.name, 1 AS number, p.parameter_id, p.name AS paramname, SCHEMA_NAME(t.schema_id) AS typeschema, t.name AS typename, p.max_length, p.precision, p.scale, p.is_cursor_ref, p.is_output, p.is_readonly, p.has_default_value, p.default_value 
            //            FROM sys.all_parameters p
            //            INNER JOIN sys.types t ON p.user_type_id = t.user_type_id
            //            INNER JOIN sys.all_objects ao ON p.object_id = ao.object_id WHERE ao.type IN('P', 'RF', 'PC', 'FN', 'IF', 'TF', 'FS', 'FT') 
            //            --AND ao.schema_id = SCHEMA_ID(N'dbo')
            //            --AND ao.name = N'gets'
            //            UNION ALL SELECT ao.name, np.procedure_number AS number, np.parameter_id, np.name AS paramname, SCHEMA_NAME(t.schema_id) AS typeschema, t.name AS typename, np.max_length, np.precision, np.scale, np.is_cursor_ref, np.is_output, 0 AS is_readonly, 0 AS has_default_value, NULL AS default_value
            //            FROM sys.numbered_procedure_parameters np INNER JOIN sys.types t ON np.user_type_id = t.user_type_id INNER JOIN sys.all_objects ao ON np.object_id = ao.object_id WHERE 
            //            --ao.schema_id = SCHEMA_ID(N'dbo')
            //            --AND ao.name = N'gets'
            //            ORDER BY ao.name, number, parameter_id";

            var sql = $@"select `name`,`param_list`,`body_utf8` from mysql.proc where db = '{dbName}' and `type` = 'FUNCTION' order by `name`";

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            return tb;
        }

        public static string GetFunctionBody(DBSource dbSource, string dbName, string functionname)
        {
            string sql = string.Format("show create function `{0}`", functionname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in tb.Rows)
            {
                //sb.Append(Regex.Replace((string)row["Text"],"\n{1,}","\n").Replace("\t","    "));
                sb.Append(((string)row["Create Function"]).TrimStart('\n').Replace("\t", "    "));
            }


            return sb.ToString();
        }


        public static List<IndexEntry> GetIndexs(DBSource dbSource, string dbName, string tabname)
        {
            var indexs = new List<IndexEntry>();
            string sql = string.Format("show index from `{0}`", tabname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            var x = from row in tb.AsEnumerable()
                    group row by row.Field<string>("Key_name") into pp
                    select new IndexEntry
                    {
                        IndexName = pp.Key,
                        Cols = pp.OrderBy(c => c.Field<object>("Seq_in_index")).Select(c => new IndexCol
                        {
                            Col = c.Field<string>("Column_name"),
                            IsDesc = c.Field<string>("Collation") != "A"
                        }).ToArray()
                    };

            return x.ToList();
        }

        public static List<IndexDDL> GetIndexDDL(DBSource dbSource, string dbName, string tabname)
        {
            var indexs = new List<IndexEntry>();

            var tb = ExecuteDBTable(dbSource, dbName, MySqlHelperConsts.GetIndexDDL, new MySqlParameter[]{
                    new MySqlParameter("@TABLE_SCHEMA",dbName),
                    new MySqlParameter("@TABLE_NAME",tabname)
                });

            var x = from row in tb.AsEnumerable().Select(p => new IndexDDL
            {
                DBName = dbName,
                TableName = tabname,
                IndexName = p.Field<string>("INDEX_NAME"),
                DDL = p.Field<string>("Show_Add_Indexes")
            }
            )
                    select row;

            return x.ToList();
        }

        public static void DropIndex(DBSource dbSource, string dbName, string tbName, bool primarykey, string indexName)
        {
            string sql = null;
            if (primarykey)
            {
                sql = string.Format("ALTER TABLE `{0}` DROP PRIMARY KEY", tbName);
            }
            else
            {
                sql = string.Format("ALTER TABLE `{0}` DROP INDEX {1}", tbName, indexName);
            }

            ExecuteNoQuery(dbSource, dbName, sql, null);
        }

        public static string GetTriggerBody(DBSource dbSource, string dbName, string functionname)
        {
            string sql = string.Format("SELECT * FROM INFORMATION_SCHEMA.triggers WHERE TRIGGER_SCHEMA=\"{0}\" and trigger_name=\"{1}\"", dbName, functionname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            if (tb.Rows.Count == 0)
            {
                return string.Empty;
            }

            return $@"CREATE trigger {tb.Rows[0].Field<string>("TRIGGER_NAME")} {tb.Rows[0].Field<string>("ACTION_TIMING")} {tb.Rows[0].Field<string>("EVENT_MANIPULATION")}
ON {tb.Rows[0].Field<string>("EVENT_OBJECT_TABLE")} FOR EACH Row {tb.Rows[0].Field<string>("ACTION_STATEMENT")}";
        }
        public static List<TriggerEntity> GetTriggers(DBSource dbSource, string dbName, string tbname)
        {

            string sql = @"SHOW TRIGGERS";

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            var x = from row in tb.AsEnumerable()
                    where row.Field<string>("Table").Equals(tbname, StringComparison.OrdinalIgnoreCase)
                    select new TriggerEntity
                    {
                        TriggerName = row.Field<string>("Trigger"),
                        ExecIsInsertTrigger = row.Field<string>("Event").Equals("insert", StringComparison.OrdinalIgnoreCase),//
                        ExecIsTriggerDisabled = false,
                        ExecIsUpdateTrigger = row.Field<string>("Event").Equals("update", StringComparison.OrdinalIgnoreCase),
                        ExecIsDeleteTrigger = row.Field<string>("Event").Equals("delete", StringComparison.OrdinalIgnoreCase)
                    };

            return x.ToList();
        }


        public static IEnumerable<string> ExportData(List<TBColumn> columns, bool notExportId, DBSource dbSource, TableInfo tableinfo, int topNum)
        {
            StringBuilder sb = new StringBuilder();
            var cols = columns.Where(p => !p.IsID).ToList();
            if (!cols.Any())
            {
                yield return "---------no columns----------";
                yield break;
            }

            string sqltext = string.Format("select {0} from {1}", string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))), string.Concat("[", tableinfo.TBName, "]"));
            var datas = ExecuteDBTable(dbSource, tableinfo.DBName, sqltext, null);
            int idx = 0;
            foreach (DataRow row in datas.Rows)
            {
                if ((++idx) == 1)
                {
                    sb.AppendFormat(string.Format("Insert into {0} ({1}) values", string.Concat("`", tableinfo.TBName, "`"), string.Join(",", cols.Select(p => string.Concat("`", p.Name, "`")))));
                }

                StringBuilder sb1 = new StringBuilder();
                foreach (var column in cols)
                {
                    object data = row[column.Name];
                    if (data == DBNull.Value)
                    {
                        sb1.Append("NULL,");
                    }
                    else
                    {
                        if (column.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("float", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                        )
                        {
                            sb1.AppendFormat("{0},", data);
                        }
                        else if (column.TypeName.Equals("boolean", StringComparison.OrdinalIgnoreCase)
                               || column.TypeName.Equals("bool", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("{0},", data.Equals(true) ? 1 : 0);
                        }
                        else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("'{0}',", ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            sb1.Append(string.Concat("'", data, "',"));
                        }
                    }
                }
                if (sb1.Length > 0)
                    sb1.Remove(sb1.Length - 1, 1);
                sb.AppendFormat("({0}),", sb1.ToString());

                if (idx > 10000)
                {
                    yield return sb.ToString();
                    sb.Clear();
                    idx = 0;
                }
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            if (idx == 0)
            {
                sb.AppendLine("--------------no data--------------------");
            }


            yield return sb.ToString();
        }
        private static List<ViewColumn> GetViewColums(DBSource dbSource, string dbname, string viewName)
        {
            var sql = "SHOW FULL COLUMNS FROM " + viewName + ";";

            var tb = ExecuteDBTable(dbSource, dbname, sql);

            var list = tb.AsEnumerable().Select(p =>
            {
                var typeWithLen = p.Field<string>("Type");
                var g = Regex.Match(typeWithLen, @"\((\d{1,})");
                return new ViewColumn
                {
                    DBName = dbname,
                    IsNullAble = "YES".Equals(p.Field<string>("Null"), StringComparison.OrdinalIgnoreCase),
                    TypeName = typeWithLen.Split('(').First(),
                    Length = g.Success ? int.Parse(g.Groups[1].Value) : -1,
                    Name = p.Field<string>("Field"),
                    TBName = viewName
                };
            }).ToList();

            return list;
        }

        public static List<KeyValuePair<string, List<ViewColumn>>> GetViews(DBSource dbSource, string dbname)
        {
            string sql = @"select TABLE_NAME from  information_schema.views where TABLE_SCHEMA=@dbname";

            var tb = ExecuteDBTable(dbSource, dbname, sql, new MySqlParameter("@dbname", dbname));

            return tb.AsEnumerable().Select(p =>
            {
                var name = p.Field<string>("TABLE_NAME");

                return new KeyValuePair<string, List<ViewColumn>>(name, GetViewColums(dbSource, dbname, name));
            }).ToList();
        }

        public static IEnumerable<DataTableObject> ExportData2(List<TBColumn> columns, DataTableObject dataTableObject, DBSource dbSource, TableInfo tableinfo, int topNum, Func<bool> checkCancel)
        {

            IEnumerable<TBColumn> cols = columns.Where(p => !p.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase));
            //cols = cols.OrderBy(p => p.IsID ? 0 : 1);

            if (!cols.Any())
            {
                yield break;
            }

            var idColumns = columns.Where(p => p.IsID).ToList();
            if (!idColumns.Any())
            {
                idColumns = columns.Where(p => p.IsKey).ToList();
            }

            var idColumn = idColumns.Count == 1 ? idColumns.First() : null;
            if (dataTableObject == null || idColumn == null)
            {
                dataTableObject = new DataTableObject()
                {
                    DBName = tableinfo.DBName,
                    TableName = tableinfo.TBName
                };
            }

            object maxId = dataTableObject.Key;
            var pagesize = Math.Min(topNum, 10000);
            var total = dataTableObject.TotalCount;
            var maxsize = 1000000;

            var totalsize = dataTableObject.Size;

            while (true)
            {
                if (checkCancel?.Invoke() == true)
                {
                    yield break;
                }

                string sqltext = null;
                DataTable datas = null;
                bool isFinished = false;
                if (idColumn == null)
                {
                    sqltext = string.Format("select {0} from `{3}`.`{1}` limit 0,{2}", string.Join(",", columns.Select(p => GetConverType(p))), tableinfo.TBName, topNum, tableinfo.DBName);
                    datas = ExecuteDBTable(dbSource, tableinfo.DBName, sqltext, null);
                    isFinished = true;
                }
                else
                {
                    MySqlParameter[] sqlParameters = null;
                    if (maxId != null)
                    {
                        sqltext = string.Format("select {0} from `{3}`.`{1}` where `{4}`>@{4} order by `{4}` ASC limit 0,{2}", string.Join(",", columns.Select(p => GetConverType(p))), tableinfo.TBName, pagesize, tableinfo.DBName, idColumn.Name);
                        sqlParameters = new[] { new MySqlParameter($"@{idColumn.Name}", maxId) };
                    }
                    else
                    {
                        sqltext = string.Format("select {0} from `{3}`.`{1}` order by `{4}` ASC limit 0,{2}", string.Join(",", columns.Select(p => GetConverType(p))), tableinfo.TBName, pagesize, tableinfo.DBName, idColumn.Name);
                    }
                    datas = ExecuteDBTable(dbSource, tableinfo.DBName, sqltext, sqlParameters);
                    if (datas.Rows.Count > 0)
                    {
                        maxId = datas.Rows[datas.Rows.Count - 1][idColumn.Name];
                    }
                    total += datas.Rows.Count;
                    if (datas.Rows.Count < pagesize)
                    {
                        isFinished = true;
                    }
                }

                if (dataTableObject.Columns.Count == 0)
                {
                    foreach (DataColumn col in datas.Columns)
                    {
                        dataTableObject.Columns.Add(new DataTableColumn
                        {
                            ColumnName = col.ColumnName,
                            ColumnType = col.DataType.FullName
                        });
                    }
                }

                foreach (DataRow row in datas.Rows)
                {
                    var datarow = new DataTableRow();

                    foreach (var cell in row.ItemArray)
                    {
                        var datacell = new DataTableCell();
                        if (cell == DBNull.Value)
                        {
                            datacell.IsDBNull = true;
                            totalsize += 1;
                        }
                        else
                        {
                            if (cell.GetType() == typeof(byte[]))
                            {
                                datacell.ByteValue = (byte[])cell;
                                totalsize += datacell.ByteValue.Length;
                            }
                            else
                            {
                                datacell.StringValue = cell.ToString();
                                totalsize += datacell.StringValue.Length * 2;
                            }
                        }
                        datarow.Cells.Add(datacell);
                    }

                    dataTableObject.Rows.Add(datarow);
                    if (totalsize >= 1000 * 1000 * 1000)
                    {
                        yield return dataTableObject;
                        dataTableObject.Rows.Clear();
                        totalsize = 0;
                    }
                }

                if (isFinished)
                {
                    break;
                }

                if (dataTableObject.Rows.Count >= maxsize)
                {
                    dataTableObject.Key = maxId?.ToString();
                    dataTableObject.TotalCount = total;
                    dataTableObject.Size = dataTableObject.Rows.Count;
                    yield return dataTableObject;
                    dataTableObject.Rows.Clear();
                }
            }

            if (dataTableObject.Rows.Count > 0)
            {
                dataTableObject.Key = maxId?.ToString();
                dataTableObject.TotalCount = total;
                dataTableObject.Size = dataTableObject.Rows.Count;
                yield return dataTableObject;
            }

            string GetConverType(TBColumn column)
            {
                if (column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
                {
                    //return string.Format("cast('' as xml).value('xs:base64Binary(sql:column(\"{0}\"))', 'varchar(max)') as [{0}]", column.Name);
                    return string.Format(" `{0}`", column.Name);
                }
                //else if (column.TypeName.Equals("binary", StringComparison.OrdinalIgnoreCase)
                //    || column.TypeName.Equals("varbinary", StringComparison.OrdinalIgnoreCase))
                //{
                //    return string.Format("cast('' as xml).value('xs:base64Binary(sql:column(\"{0}\"))', 'varchar(max)') as [{0}]", column.Name);
                //}
                //else if (column.TypeName.Equals("image", StringComparison.OrdinalIgnoreCase))
                //{
                //    return string.Format("convert(varbinary(max),[{0}]) as [{0}]", column.Name);
                //}
                return string.Format("`{0}`", column.Name);
            }
        }

        public static IEnumerable<DataTableObject> ExportData2(List<TBColumn> columns, bool notExportId, DBSource dbSource, TableInfo tableinfo, int topNum, Func<bool> checkCancel, CopyDBTask copyDBTask)
        {

            IEnumerable<TBColumn> cols = columns.Where(p => !p.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase));
            //cols = cols.OrderBy(p => p.IsID ? 0 : 1);

            if (!cols.Any())
            {
                yield break;
            }

            DataTableObject dataTableObject = new DataTableObject()
            {
                DBName = tableinfo.DBName,
                TableName = tableinfo.TBName
            };

            var idColumns = columns.Where(p => p.IsID).ToList();
            if (!idColumns.Any())
            {
                idColumns = columns.Where(p => p.IsKey).ToList();
            }

            var idColumn = idColumns.Count == 1 ? idColumns.First() : null;
            object maxId = null;
            var pagesize = Math.Min(topNum, 10000);
            var total = 0;
            var maxsize = 1000000;

            var lastTask = copyDBTask.CopyTBDataTasks.Where(p => p.DB == tableinfo.DBName && p.TB == tableinfo.TBName).OrderByDescending(p => p.TotalCount).FirstOrDefault();
            if (lastTask != null)
            {
                if (lastTask.Key == null)
                {
                    yield break;
                }
                //要转换下类型
                var sqltext = string.Format("select {0} from `{2}`.`{1}` limit 0,1", idColumn.Name, tableinfo.TBName, tableinfo.DBName);
                var data = ExecuteDBTable(dbSource, tableinfo.DBName, sqltext);
                maxId = DataHelper.ConvertDBType(lastTask.Key, data.Columns[0].DataType);
                total = lastTask.TotalCount;
            }

            var totalsize = 0;
            while (true)
            {
                if (checkCancel?.Invoke() == true)
                {
                    yield break;
                }

                string sqltext = null;
                DataTable datas = null;
                bool isFinished = false;
                if (idColumn == null)
                {
                    sqltext = string.Format("select  {0} from `{3}`.`{1}` limit 0,{2}", string.Join(",", columns.Select(p => GetConverType(p))), tableinfo.TBName, topNum, tableinfo.DBName);
                    datas = ExecuteDBTable(dbSource, tableinfo.DBName, sqltext, null);
                    isFinished = true;
                }
                else
                {
                    MySqlParameter[] sqlParameters = null;
                    if (maxId != null)
                    {
                        sqltext = string.Format("select {0} from `{3}`.`{1}` where `{4}`<@{4} order by `{4}` desc  limit 0,{2}", string.Join(",", columns.Select(p => GetConverType(p))), tableinfo.TBName, pagesize, tableinfo.DBName, idColumn.Name);
                        sqlParameters = new[] { new MySqlParameter($"@{idColumn.Name}", maxId) };
                    }
                    else
                    {
                        sqltext = string.Format("select {0} from `{3}`.`{1}` order by `{4}` desc  limit 0,{2}", string.Join(",", columns.Select(p => GetConverType(p))), tableinfo.TBName, pagesize, tableinfo.DBName, idColumn.Name);
                    }
                    datas = ExecuteDBTable(dbSource, tableinfo.DBName, sqltext, sqlParameters);
                    if (datas.Rows.Count > 0)
                    {
                        maxId = datas.Rows[datas.Rows.Count - 1][idColumn.Name];
                    }
                    total += datas.Rows.Count;
                    if (datas.Rows.Count < pagesize || total >= topNum)
                    {
                        isFinished = true;
                    }
                }


                if (dataTableObject.Columns.Count == 0)
                {
                    foreach (DataColumn col in datas.Columns)
                    {
                        dataTableObject.Columns.Add(new DataTableColumn
                        {
                            ColumnName = col.ColumnName,
                            ColumnType = col.DataType.FullName
                        });
                    }
                }

                foreach (DataRow row in datas.Rows)
                {
                    var datarow = new DataTableRow();

                    foreach (var cell in row.ItemArray)
                    {
                        var datacell = new DataTableCell();
                        if (cell == DBNull.Value)
                        {
                            datacell.IsDBNull = true;
                            totalsize += 1;
                        }
                        else
                        {
                            if (cell.GetType() == typeof(byte[]))
                            {
                                datacell.ByteValue = (byte[])cell;
                                totalsize += datacell.ByteValue.Length;
                            }
                            else
                            {
                                datacell.StringValue = cell.ToString();
                                totalsize += datacell.StringValue.Length * 2;
                            }
                        }
                        datarow.Cells.Add(datacell);
                    }

                    dataTableObject.Rows.Add(datarow);
                    if (totalsize >= 1000 * 1000 * 1000)
                    {
                        yield return dataTableObject;
                        dataTableObject.Rows.Clear();
                        totalsize = 0;
                    }
                }

                if (isFinished)
                {
                    break;
                }

                if (dataTableObject.Rows.Count >= maxsize)
                {
                    dataTableObject.Key = maxId?.ToString();
                    dataTableObject.TotalCount = total;
                    dataTableObject.Size = dataTableObject.Rows.Count;
                    yield return dataTableObject;
                    dataTableObject.Rows.Clear();
                }
            }

            if (dataTableObject.Rows.Count > 0)
            {
                dataTableObject.Key = maxId?.ToString();
                dataTableObject.TotalCount = total;
                dataTableObject.Size = dataTableObject.Rows.Count;
                yield return dataTableObject;
            }

            string GetConverType(TBColumn column)
            {
                if (column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
                {
                    //return string.Format("cast('' as xml).value('xs:base64Binary(sql:column(\"{0}\"))', 'varchar(max)') as [{0}]", column.Name);
                    return string.Format("`{0}`", column.Name);
                }
                //else if (column.TypeName.Equals("binary", StringComparison.OrdinalIgnoreCase)
                //    || column.TypeName.Equals("varbinary", StringComparison.OrdinalIgnoreCase))
                //{
                //    return string.Format("cast('' as xml).value('xs:base64Binary(sql:column(\"{0}\"))', 'varchar(max)') as [{0}]", column.Name);
                //}
                //else if (column.TypeName.Equals("image", StringComparison.OrdinalIgnoreCase))
                //{
                //    return string.Format("convert(varbinary(max),[{0}]) as [{0}]", column.Name);
                //}
                return string.Format("`{0}`", column.Name);
            }
        }

        public static void SqlBulkCopy(DBSource dbSource, string connDB, int timeOut, string destTable, DataTable copytable)
        {
            if (copytable.Rows.Count > 0)
            {
                using (var conn = new MySqlConnector.MySqlConnection(GetConnstringFromDBSource(dbSource, connDB, null, null, true)))
                {
                    conn.Open();
                    var copytask = new MySqlConnector.MySqlBulkCopy(conn);
                    if (timeOut > 0)
                    {
                        copytask.BulkCopyTimeout = timeOut / 1000;
                    }

                    copytask.DestinationTableName = destTable;
                    copytask.WriteToServer(copytable);
                }
            }
        }
    }
}
