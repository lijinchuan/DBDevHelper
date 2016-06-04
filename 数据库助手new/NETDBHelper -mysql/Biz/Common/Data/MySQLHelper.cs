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
    }
}
