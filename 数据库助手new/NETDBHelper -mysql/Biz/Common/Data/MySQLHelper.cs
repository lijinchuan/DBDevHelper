using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Entity;

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
            var dt= ExecuteDBTable(dbSource, null, MySqlHelperConsts.GetDBs, null);
            
            dt.Columns[0].ColumnName = "name";
            dt=dt.AsEnumerable().Where(p => p["name"].ToString() != "information_schema" && p["name"].ToString() != "mysql" && p["name"].ToString() != "performance_schema").CopyToDataTable();
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
            var tb= ExecuteDBTable(dbSource, dbName, MySqlHelperConsts.GetTableColsDescription,new MySqlParameter("@db", dbName),new MySqlParameter("@tb",tbName));
            tb.Columns["column_name"].ColumnName = "ColumnName";
            tb.Columns["column_comment"].ColumnName = "Description";
            return tb;
        }

        public static DataTable GetTBs(DBSource dbSource, string dbName)
        {
            var tb= ExecuteDBTable(dbSource, dbName, string.Format(MySqlHelperConsts.GetTBs,dbName), null);
            tb.Columns[0].ColumnName = "name";
            return tb;
        }

        public static IEnumerable<TBColumn> GetColumns(DBSource dbSource, string dbName,string tbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, MySqlHelperConsts.GetColumns, new MySqlParameter("@db", dbName),new MySqlParameter("@tb",tbName));

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                yield return new TBColumn
                {
                    IsKey = string.Equals((string)tb.Rows[i]["column_key"], "pri", StringComparison.OrdinalIgnoreCase),
                    Length = int.Parse(string.IsNullOrEmpty(tb.Rows[i]["character_maximum_length"].ToString()) ? "0" : tb.Rows[i]["character_maximum_length"].ToString()),
                    Name = tb.Rows[i]["column_name"].ToString(),
                    TypeName = tb.Rows[i]["data_type"].ToString(),
                    //IsID = string.Equals(idColumnName, tb.Rows[i]["name"].ToString()),
                    IsNullAble = tb.Rows[i]["is_nullable"].ToString().Equals("yes", StringComparison.OrdinalIgnoreCase),
                    prec = NumberHelper.CovertToInt(tb.Rows[i]["numeric_precision"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["numeric_scale"]),
                    Description = tb.Rows[i]["column_comment"].ToString(),
                };
            }
        }

        public static IEnumerable<string> GetProcedures(DBSource dbSource,string dbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, string.Format("select name from mysql.proc where db='{0}'", dbName));

            var y = from x in tb.AsEnumerable()
                    select x["name"] as string;

            return y;
        }

        public static string GetProcedureBody(DBSource dbSource, string dbName, string procedure)
        {
            string sql = string.Format("select body from mysql.proc where name='{0}';", procedure);

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

        public static string GetConnstringFromDBSource(DBSource dbSource, string connDB)
        {
            if (dbSource == null)
                return null;
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = dbSource.ServerName;
            sb.Database = connDB;
            sb.Pooling = true;
            sb.ConnectionTimeout = 30;
            sb.SqlServerMode = true;
            sb.AllowBatch = true;
            sb.MinimumPoolSize = 1;
            sb.MaximumPoolSize = 5;
            
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
            var conn = new MySqlConnection(GetConnstringFromDBSource(dbSource, connDB));
            MySqlCommand cmd = conn.CreateCommand();
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

            string delSql = "drop table [" + tabName + "]";
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }

        public static void DeleteDataBase(DBSource dBSource, string database)
        {
            if (dBSource == null)
                return;

            string delSql = "drop database "+database;
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
                var newrow=tb.NewRow();
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
            sb.AppendLine(string.Format("drop PROCEDURE if exists {0};",spname));
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
            sb.AppendLine("   if @OrderBy is null then");
            sb.AppendLine("   begin");
            sb.AppendLine(string.Format("      set @OrderBy='{0}';", orderBy));
            sb.AppendLine("   end;");
            sb.AppendLine("   end if;");
            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendLine(string.Format("/*{0}*/",string.IsNullOrWhiteSpace(col.Description)?col.Name:col.Description));
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
                sb.AppendLine(" end;");
                sb.AppendLine(" else");
                sb.AppendLine(" begin");
                sb.AppendLine(string.Format("       set @{0}=true;",col.Name));
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
            sb.Append(string.Format("	Select {0} From ", string.Join(",", outputcols.Select(p => "a.`" + p.Name + "`"))));
            sb.Append(string.Format("	`{0}` as a ',@where", tbname));
            sb.AppendLine(", ' order by ? limit ?,?');");
            sb.AppendLine();
            sb.AppendLine("prepare stmt from @sql;");

            sb.Append(string.Format("execute stmt using {0},", string.Join(",", conditioncols.Select(p => "@" + p.Name))));
            sb.AppendLine("@OrderBy,@limit,@pageSize;");
            sb.AppendLine("DEALLOCATE PREPARE stmt;");
            sb.AppendLine("");
            sb.AppendLine("");

            sb.AppendLine("END$$");
            sb.AppendLine("DELIMITER ;");

            return sb.ToString();
        }
    }
}
