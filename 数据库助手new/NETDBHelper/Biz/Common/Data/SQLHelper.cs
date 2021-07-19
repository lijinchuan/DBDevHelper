using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Entity;
using System.Data;
using static Entity.IndexEntry;
using System.Text.RegularExpressions;

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

        public static DataTable GetTBsDesc(DBSource dbSource, string dbName,string tablename)
        {
            object name = tablename;
            if (name == null)
                name = DBNull.Value;
            return ExecuteDBTable(dbSource, dbName, SQLHelperConsts.SQL_GetTBsDesc, new SqlParameter("@tablename", name));
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

        /// <summary>
        /// 取表说明
        /// </summary>
        /// <param name="dBSource">name,desc</param>
        /// <param name="tbName">空取所有的表说明，不空取单个表说明</param>
        /// <returns></returns>
        public static DataTable GetTableDescription(DBSource dBSource, string dbName, string tbName)
        {
            if (string.IsNullOrWhiteSpace(tbName))
            {
                return ExecuteDBTable(dBSource, dbName, SQLHelperConsts.GetTablesDescription, null);
            }
            else
            {
                return ExecuteDBTable(dBSource, dbName, SQLHelperConsts.GetTableDescription, new SqlParameter("@name", tbName));
            }
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

                var prec = NumberHelper.CovertToInt(tb.Rows[i]["prec"]);
                var len = int.Parse(tb.Rows[i]["length"].ToString());
                if (len > prec && prec > 0)
                {
                    len = prec;
                }
                yield return new TBColumn
                {
                    IsKey = (tb2.AsEnumerable()).FirstOrDefault(p=>p[0].ToString().Equals(tb.Rows[i]["name"].ToString(),StringComparison.OrdinalIgnoreCase))!=null,
                    Length= len,
                    Name=tb.Rows[i]["name"].ToString(),
                    TypeName = tb.Rows[i]["type"].ToString(),
                    IsID=string.Equals(idColumnName,tb.Rows[i]["name"].ToString()),
                    IsNullAble = tb.Rows[i]["isnullable"].ToString().Equals("1"),
                    prec=NumberHelper.CovertToInt(tb.Rows[i]["prec"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["scale"]),
                    Description=y==null?"":y.ToString(),
                    TBName=tbName,
                    DBName=dbName
                };
            }
        }

        public static IEnumerable<TBColumn> GetColumns(DBSource dbSource, string dbName, string tbName)
        {
            var tb = ExecuteDBTable(dbSource, dbName, SQLHelperConsts.GetColumnsByTableName, new SqlParameter("@name", tbName));
            //查主键
            var tb2 = GetKeys(dbSource, dbName, tbName);
            //查自增键
            string idColumnName = GetIdColumName(dbSource, dbName, tbName);
            //描述
            DataTable tbDesc = GetTableColsDescription(dbSource, dbName, tbName);

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                var y = (from x in tbDesc.AsEnumerable()
                         where string.Equals((string)x["ColumnName"], (string)tb.Rows[i]["name"], StringComparison.OrdinalIgnoreCase)
                         select x["Description"]).FirstOrDefault();
                yield return new TBColumn
                {
                    IsKey = (tb2.AsEnumerable()).FirstOrDefault(p => p[0].ToString().Equals(tb.Rows[i]["name"].ToString(), StringComparison.OrdinalIgnoreCase)) != null,
                    Length = int.Parse(tb.Rows[i]["length"].ToString()),
                    Name = tb.Rows[i]["name"].ToString(),
                    TypeName = tb.Rows[i]["type"].ToString(),
                    IsID = string.Equals(idColumnName, tb.Rows[i]["name"].ToString()),
                    IsNullAble = tb.Rows[i]["isnullable"].ToString().Equals("1"),
                    prec = NumberHelper.CovertToInt(tb.Rows[i]["prec"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["scale"]),
                    Description = y == null ? "" : y.ToString(),
                    TBName=tbName,
                    DBName=dbName
                };
            }
        }

        public static DataTable ExecuteDBTable(DBSource dbSource, string connDB, string sql, params SqlParameter[] sqlParams)
        {
            using (var conn = new SqlConnection(GetConnstringFromDBSource(dbSource, connDB))) {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 180;
                    if (sqlParams != null)
                    {
                        cmd.Parameters.AddRange(sqlParams);
                    }
                    try
                    {
                        SqlDataAdapter ada = new SqlDataAdapter(cmd);
                        DataTable tb = new DataTable();
                        ada.Fill(tb);
                        return tb;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString());
                        throw ex;
                    }
                }
            }
        }

        public static DataSet ExecuteDataSet(DBSource dbSource, string connDB, string sql, SqlInfoMessageEventHandler onmsg , params SqlParameter[] sqlParams)
        {
            using (var conn = new SqlConnection(GetConnstringFromDBSource(dbSource, connDB)))
            {
                conn.InfoMessage += onmsg;
                
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 180;
                    if (sqlParams != null)
                    {
                        cmd.Parameters.AddRange(sqlParams);
                    }
                    SqlDataAdapter ada = new SqlDataAdapter(cmd);
                    DataSet ts = new DataSet();
                    ada.Fill(ts);
                    
                    return ts;
                }
            }
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

        public static object ExecuteScalar(DBSource dbSource, string connDB, string sql, params SqlParameter[] sqlParams)
        {
            var conn = new SqlConnection(GetConnstringFromDBSource(dbSource, connDB));
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
                return cmd.ExecuteScalar();
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
            string delSql = "USE MASTER alter database [" + dbName+"] set single_user with rollback immediate \r\ndrop database [" + dbName + "]";
            ExecuteNoQuery(dbSource, dbName, delSql, null);
        }

        public static void CreateDataBase(DBSource dbSource, string dbName, string newDBName)
        {
            if (dbSource == null)
                return;
            string sql = "create database [" + newDBName + "]";
            ExecuteNoQuery(dbSource, dbName ?? string.Empty, sql, null);
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

        public static DataTable GetProceduresWithParams(DBSource dbSource, string dbName)
        {
            string sql = @"select a.name,b.name pname,c.name tpname,b.length,b.isnullable,b.isoutparam from dbo.sysobjects a
                           left join syscolumns b
                           on a.id=b.id
                           left join systypes c
                           on b.xusertype=c.xusertype
                           where (c.name<>'sysname' or c.name is null) and OBJECTPROPERTY(a.id, N'IsProcedure') = 1 order by a.name";
            var tb = ExecuteDBTable(dbSource, dbName, sql);

            return tb;

        }

        public static string GetProcedureBody(DBSource dbSource, string dbName, string procedure)
        {
            //show create {procedure|function} sp_name
            string sql = string.Format("sp_helptext  '{0}'", procedure);

            var tb = ExecuteDBTable(dbSource, dbName, sql);
            StringBuilder sb = new StringBuilder();

            foreach(DataRow row in tb.Rows)
            {
                //sb.Append(Regex.Replace((string)row["Text"],"\n{1,}","\n").Replace("\t","    "));
                sb.Append(((string)row["Text"]).TrimStart('\n').Replace("\t", "    "));
            }
            

            return sb.ToString();
        }

        public static DataTable GetFunctions(DBSource dbSource, string dbName)
        {
            var sql = @"SELECT OBJECTPROPERTY(ao.id, N'IsScalarFunction') IsScalarFunction,
                        OBJECTPROPERTY(ao.id, N'IsTableFunction') IsTableFunction,ao.name,
                        ao.id
                        FROM sys.sysobjects ao WHERE ao.type IN('RF', 'PC', 'FN', 'IF', 'TF', 'FS', 'FT') 
                        --AND ao.schema_id = SCHEMA_ID(N'dbo')
                        --AND ao.name = N'gets'
                        ORDER BY ao.name";

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

            var sql = @"select b.*,
						OBJECTPROPERTY(a.id, N'IsScalarFunction') IsScalarFunction,
                        OBJECTPROPERTY(a.id, N'IsTableFunction') IsTableFunction
						
						from sys.sysobjects a
					    left join(
						SELECT ao.object_id,ao.name, 1 AS number, p.parameter_id, p.name AS pname, SCHEMA_NAME(t.schema_id) AS typeschema, t.name AS tpname, p.max_length length, p.precision, p.scale, p.is_cursor_ref, p.is_output isoutparam, p.is_readonly, p.has_default_value, p.default_value 
                        FROM sys.parameters p
                        INNER JOIN sys.types t ON p.user_type_id = t.user_type_id
                        INNER JOIN sys.objects ao ON p.object_id = ao.object_id WHERE ao.type IN('RF', 'PC', 'FN', 'IF', 'TF', 'FS', 'FT') 
                        --AND ao.schema_id = SCHEMA_ID(N'dbo')
                        --AND ao.name = N'gets'
                        UNION ALL SELECT ao.object_id,ao.name, np.procedure_number AS number, np.parameter_id, np.name AS pname, SCHEMA_NAME(t.schema_id) AS typeschema, t.name AS tpname, np.max_length , np.precision, np.scale, np.is_cursor_ref, np.is_output isoutparam, 0 AS is_readonly, 0 AS has_default_value, NULL AS default_value
                        FROM sys.numbered_procedure_parameters np INNER JOIN sys.types t ON np.user_type_id = t.user_type_id INNER JOIN sys.all_objects ao ON np.object_id = ao.object_id WHERE 1=1
                        --and ao.schema_id = SCHEMA_ID(N'dbo')
                        --AND ao.name = N'gets'
						)b on a.id=b.object_id
						where a.type IN('RF', 'PC', 'FN', 'IF', 'TF', 'FS', 'FT') 
                        ORDER BY b.name, b.number, parameter_id";

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            return tb;
        }

        public static string GetFunctionBody(DBSource dbSource, string dbName, string functionname)
        {
            string sql = string.Format("sp_helptext  '{0}'", functionname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in tb.Rows)
            {
                //sb.Append(Regex.Replace((string)row["Text"],"\n{1,}","\n").Replace("\t","    "));
                sb.Append(((string)row["Text"]).TrimStart('\n').Replace("\t", "    "));
            }


            return sb.ToString();
        }

        public static string GetTriggerBody(DBSource dbSource, string dbName, string functionname)
        {
            string sql = string.Format("sp_helptext  '{0}'", functionname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in tb.Rows)
            {
                //sb.Append(Regex.Replace((string)row["Text"],"\n{1,}","\n").Replace("\t","    "));
                sb.Append(((string)row["Text"]).TrimStart('\n').Replace("\t", "    "));
            }


            return sb.ToString();
        }

        public static List<IndexEntry> GetIndexs(DBSource dbSource, string dbName, string tabname)
        {
            var indexs = new List<IndexEntry>();
            //            string sql = string.Format(@"SELECT  a.name Key_name,d.name Column_name,b.keyno Seq_in_index from
            //sysindexes a 
            //left join sysindexkeys  b  on a.id=b.id 
            //left JOIN  sysobjects  c  ON  b.id = c.id
            //left JOIN  syscolumns  d  ON  b.id = d.id 
            //WHERE  a.indid=b.indid and b.colid = d.colid and  c.xtype = 'U'
            //AND  c.name = '{0}' ", tabname);

            string sql = string.Format(SQLHelperConsts.SQL_GETINDEXLIST, tabname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);

            var x = from row in tb.AsEnumerable()
                    group row by row.Field<string>("Key_name") into pp
                    select new IndexEntry
                    {
                        IndexName = pp.Key,
                        IsPri= pp.First().Field<bool>("is_primary_key"),//
                        IsClustered =pp.First().Field<string>("ix_type_desc").Equals("CLUSTERED", StringComparison.OrdinalIgnoreCase),
                        Cols = pp.OrderBy(c => c.Field<object>("Seq_in_index")).Select(c =>new IndexCol
                        {
                            Col= c.Field<string>("Column_name"),
                            IsDesc=c.Field<bool>("is_descending_key"),
                            IsInclude= c.Field<bool>("is_included_column")
                        }).ToArray()
                    };

            return x.ToList();
        }

        public static List<TriggerEntity> GetTriggers(DBSource dbSource, string dbName, string tbname)
        {

            string sql = @"SELECT po.name,OBJECTPROPERTY(ao.id, N'ExecIsInsertTrigger') ExecIsInsertTrigger,
                        OBJECTPROPERTY(ao.id, N'ExecIsTriggerDisabled') ExecIsTriggerDisabled,
						OBJECTPROPERTY(ao.id, N'ExecIsUpdateTrigger') ExecIsUpdateTrigger,
						OBJECTPROPERTY(ao.id, N'ExecIsDeleteTrigger') ExecIsDeleteTrigger,
						ao.name triggername,
                        ao.id
                        FROM sys.sysobjects ao
						join sys.sysobjects po on po.id=ao.parent_obj
						 WHERE ao.type IN('tr', 'ta') and po.name=@name
                        ORDER BY ao.name";

            var tb = ExecuteDBTable(dbSource, dbName, sql,new SqlParameter("@name",tbname));

            var x = from row in tb.AsEnumerable()
                    select new TriggerEntity
                    {
                        TriggerName = row.Field<string>("triggername"),
                        ExecIsInsertTrigger = row.Field<int>("ExecIsInsertTrigger")==1,//
                        ExecIsTriggerDisabled = row.Field<int>("ExecIsTriggerDisabled") == 1,
                        ExecIsUpdateTrigger = row.Field<int>("ExecIsUpdateTrigger") == 1,
                        ExecIsDeleteTrigger = row.Field<int>("ExecIsDeleteTrigger") == 1
                    };

            return x.ToList();
        }

        public static List<KeyValuePair<string,List<Entity.ViewColumn>>> GetViews(DBSource dbSource,string dbname)
        {
            string sql = @"SELECT a.TABLE_NAME,b.COLUMN_NAME,B.IS_NULLABLE,B.DATA_TYPE,isnull(B.CHARACTER_MAXIMUM_LENGTH,-1) CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.VIEWS a,INFORMATION_SCHEMA.COLUMNS b
where a.TABLE_NAME=b.TABLE_NAME ORDER BY A.TABLE_NAME,B.ORDINAL_POSITION";

            var tb = ExecuteDBTable(dbSource, dbname, sql);


            return tb.AsEnumerable().GroupBy(p => p.Field<string>("TABLE_NAME")).
                Select(p => new KeyValuePair<string, List<ViewColumn>>(p.Key, p.Select(q => new ViewColumn
                {
                    Name=q.Field<string>("COLUMN_NAME"),
                    TypeName=q.Field<string>("DATA_TYPE"),
                    Length=q.Field<int>("CHARACTER_MAXIMUM_LENGTH"),
                    IsNullAble=q.Field<string>("IS_NULLABLE").Equals("YES")
                }).ToList())).ToList();
        }

        public static List<KeyValuePair<string, List<Entity.TBColumn>>> GetViews(DBSource dbSource, string dbname,string viewname)
        {
            string sql = @"SELECT a.TABLE_NAME,b.COLUMN_NAME,B.IS_NULLABLE,B.DATA_TYPE,isnull(B.CHARACTER_MAXIMUM_LENGTH,-1) CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.VIEWS a,INFORMATION_SCHEMA.COLUMNS b
where a.Table_NAME='"+viewname+"' and a.TABLE_NAME=b.TABLE_NAME ORDER BY A.TABLE_NAME,B.ORDINAL_POSITION";

            var tb = ExecuteDBTable(dbSource, dbname, sql);


            return tb.AsEnumerable().GroupBy(p => p.Field<string>("TABLE_NAME")).
                Select(p => new KeyValuePair<string, List<TBColumn>>(p.Key, p.Select(q => new TBColumn
                {
                    Name = q.Field<string>("COLUMN_NAME"),
                    TypeName = q.Field<string>("DATA_TYPE"),
                    Length = q.Field<int>("CHARACTER_MAXIMUM_LENGTH"),
                    IsNullAble = q.Field<string>("IS_NULLABLE").Equals("YES")
                }).ToList())).ToList();
        }

        public static string GetViewCreateSql(DBSource dbSource, string dbName, string viewname)
        {
            //show create {procedure|function} sp_name
            string sql = string.Format("SELECT VIEW_DEFINITION as Text FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME='{0}'", viewname);

            var tb = ExecuteDBTable(dbSource, dbName, sql);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in tb.Rows)
            {
                sb.AppendLine((string)row["Text"]);
            }


            return sb.ToString();
        }

        public static void CreateIndex(DBSource dbSource, string dbName, string tbname, string indexname, bool unique, bool primarykey, bool autoIncr, bool isclustered, List<IndexTBColumn> cols)
        {
            if (!cols.Any(p => !p.Include))
            {
                throw new Exception("缺少列");
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("create {0} {1} index {2}", unique ? "unique" : "", isclustered ? "clustered" : "nonclustered", indexname);
            sb.AppendLine();
            sb.AppendFormat("on {0}({1})", tbname, string.Join(",", cols.Where(p => !p.Include).Select(p => p.Name+ (p.Order==-1?" desc":""))));
            sb.AppendLine();
            if (cols.Any(p => p.Include))
            {
                sb.AppendFormat("include ({0})", string.Join(",", cols.Where(p => p.Include).Select(p => p.Name)));
            }

            var sql = sb.ToString();

            ExecuteNoQuery(dbSource, dbName, sql);
        }

        public static void DropIndex(DBSource dbSource, string dbName, string tbName, bool primarykey, string indexName)
        {
            string sql = string.Format("drop index {0} on [{1}]", indexName, tbName);

            ExecuteNoQuery(dbSource, dbName, sql);
        }

        public static string ExportData(List<TBColumn> columns, bool notExportId, DBSource dbSource, TableInfo tableinfo, int topNum)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                IEnumerable<TBColumn> cols = columns;
                if (!cols.ToList().Exists(p => p.IsID))
                {
                    notExportId = true;
                }
                if (notExportId)
                {
                    cols = cols.Where(p => !p.IsID);
                }
                cols = cols.OrderBy(p => p.IsID ? 0 : 1);

                if (!cols.Any())
                {
                    return "---------no columns----------";
                }

                string sqltext = string.Format("select top {2} {0} from [{3}].{1} with(nolock)", string.Join(",", cols.Select(p => GetConverType(p))), string.Concat("[", tableinfo.TBName, "]"), topNum, tableinfo.Schema);
                var datas = Biz.Common.Data.SQLHelper.ExecuteDBTable(dbSource, tableinfo.DBName, sqltext, null);

                if (!notExportId)
                {
                    sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} ON", string.Concat("[", tableinfo.TBName, "]")));
                    sb.AppendLine("GO");
                    sb.AppendLine(string.Format("delete from {0}", string.Concat("[", tableinfo.TBName, "]")));
                    sb.AppendLine(string.Format("DBCC CHECKIDENT({0},RESEED,0)", string.Concat("[", tableinfo.TBName, "]")));
                    sb.AppendLine("GO");
                }

                int idx = 0;
                foreach (DataRow row in datas.Rows)
                {
                    if ((++idx) == 1)
                    {
                        sb.AppendFormat("Insert into {0} ({1})  ", string.Concat("[", tableinfo.TBName, "]"), string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))));
                    }
                    StringBuilder sb1 = new StringBuilder(idx > 1 ? " union select " : "select ");
                    foreach (var column in cols)
                    {
                        try
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
                                    //|| column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                                    || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                                )
                                {
                                    sb1.AppendFormat("{0},", data);
                                }
                                else if (column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.Equals("binary", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.Equals("varbinary", StringComparison.OrdinalIgnoreCase))
                                {
                                    sb1.AppendFormat("{0},", data);
                                }
                                else if (column.TypeName.Equals("uniqueidentifier", StringComparison.OrdinalIgnoreCase))
                                {
                                    sb1.AppendFormat("'{0}',", data);
                                }
                                else if (column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase))
                                {
                                    sb1.AppendFormat("{0},", (bool)data ? 1 : 0);
                                }
                                else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.Equals("date", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.Equals("smalldatetime", StringComparison.OrdinalIgnoreCase)
                                    || column.TypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
                                {
                                    sb1.AppendFormat("'{0}',", ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else if (column.TypeName.Equals("sql_variant", StringComparison.OrdinalIgnoreCase))
                                {
                                    sb1.AppendFormat("'{0}',", data);
                                }
                                else
                                {
                                    sb1.Append(string.Concat("'", string.IsNullOrEmpty((string)data) ? string.Empty : data.ToString().Replace("'", "''"), "',"));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    if (sb1.Length > 0)
                        sb1.Remove(sb1.Length - 1, 1);
                    sb.AppendLine();
                    sb.AppendFormat("{0}", sb1.ToString());
                }

                if (idx == 0)
                {
                    sb.AppendLine("--------------no data--------------------");
                }

                if (!notExportId)
                {
                    sb.AppendLine();
                    sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} OFF", string.Concat("[", tableinfo.TBName, "]")));
                    sb.AppendLine("GO");
                }


            }
            catch (Exception)
            {
                throw;

            }

            return sb.ToString();

            string GetConverType(TBColumn column)
            {
                if (column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format("CAST([{0}] AS VARBINARY(8)) as [{0}]", column.Name);
                }
                else if (column.TypeName.Equals("binary", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("varbinary", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format("CAST([{0}] AS VARBINARY({1})) as [{0}]", column.Name, column.Length == -1 ? "max" : column.Length.ToString());
                }
                return string.Format("[{0}]", column.Name);
            }
        }
    }
}
