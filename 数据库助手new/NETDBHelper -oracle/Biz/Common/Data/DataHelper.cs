using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.CodeDom.Compiler;
using System.Reflection;
using Entity;
using System.Text.RegularExpressions;

namespace Biz.Common.Data
{
    public static class DataHelper
    {
        /// <summary>
        /// 创建一个表格
        /// </summary>
        /// <param name="colsName">表格字段，可以加//注释</param>
        /// <returns></returns>
        public static DataTable CreateTable(params string[] colsName)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < colsName.Length; i++)
            {
                dt.Columns.Add(colsName[i].Split(new string[] { "//" }, StringSplitOptions.None)[0].Trim());
            }

            return dt;
        }

        /// <summary>
        /// 创建一个表格
        /// </summary>
        /// <param name="colsName">表格字段,可以加//注释</param>
        /// <returns></returns>
        public static DataTable CreateFatTable(params string[] colsName)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < colsName.Length; i++)
            {
                dt.Columns.Add(colsName[i].Split(new string[] { "//" }, StringSplitOptions.None)[0].Trim(), typeof(object));
            }

            return dt;
        }

        public static DataRow TableAddRow(DataTable table, params string[] colValues)
        {
            if (table == null)
                return null;

            var newRow = table.NewRow();
            if (colValues.Length != table.Columns.Count)
                throw new DataException();
            for (int i = 0; i < colValues.Length; i++)
            {
                newRow[i] = colValues[i];
            }
            table.Rows.Add(newRow);
            return newRow;
        }

        public static List<DataTable> GetEntityFieldTable(string code)
        {
            //if (!new Regex(@"using\s+LJC.FrameWork.Data.QuickDataBase").IsMatch(code))
            //{
            //    code = "using LJC.FrameWork.Data.QuickDataBase;\r\n" + code;
            //}

            if (!new Regex(@"using\s+Entity").IsMatch(code))
            {
                code = "using Entity;\r\n" + code;
            }

            if (!new Regex(@"using\s+System").IsMatch(code))
            {
                code = "using System;\r\n" + code;
            }

            code = code.Replace("[JsonProperty", "//[JsonPorperty")
                .Replace("[JsonIgnore]", "//[JsonIgnore]");

            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            CompilerParameters paramerters = new CompilerParameters();
            paramerters.ReferencedAssemblies.Add("System.Core.dll");
            paramerters.ReferencedAssemblies.Add("Entity.dll");
            paramerters.GenerateExecutable = false;
            paramerters.GenerateInMemory = true;
            //parameters.OutputAssembly = @"person.dll";
            var result = provider.CompileAssemblyFromSource(paramerters, code);
            if (result.Errors.HasErrors)
            {
                throw new DBHelperException(result.Errors, result.Errors[0].ErrorText);
            }
            List<DataTable> ret = new List<DataTable>();
            var types = result.CompiledAssembly.GetTypes();
            foreach (Type type in types)
            {
                DataTable tb = CreateFatTable("colname","coltype","isidentity","iskey","defaultvalue","nullable","desc");
                var tbAttr=(DBMapAttribute)type.GetCustomAttributes(typeof(DBMapAttribute), true).FirstOrDefault();
                tb.TableName = type.Name;
                if (tb.TableName.EndsWith("Entity", StringComparison.OrdinalIgnoreCase))
                {
                    tb.TableName = tb.TableName.Substring(0, tb.TableName.Length - "Entity".Length);
                }
                if (tbAttr != null)
                {
                    if (!string.IsNullOrWhiteSpace(tbAttr.DBName))
                        tb.TableName = tbAttr.DBName;
                }
                
                foreach (PropertyInfo proper in type.GetProperties())
                {
                    var row = tb.NewRow();
                    row["colname"] = proper.Name;
                    row["isidentity"] = false;
                    row["iskey"] = false;
                    row["nullable"] = true;
                    var fieldAttr=(DataBaseMapperAttr)proper.GetCustomAttributes(typeof(DataBaseMapperAttr), true).FirstOrDefault();
                    if (fieldAttr != null)
                    {
                        if (!string.IsNullOrWhiteSpace(fieldAttr.Column))
                        {
                            row["colname"] = fieldAttr.Column;
                        }
                        if (!string.IsNullOrWhiteSpace(fieldAttr.DBType))
                        {
                            row["coltype"] = fieldAttr.DBType;
                        }
                        else
                        {
                            row["coltype"] = Common.NetTypeToOracleDBType(proper.PropertyType, fieldAttr.Len);
                        }
                        row["isidentity"] = fieldAttr.IsIdentity;
                        row["iskey"] = fieldAttr.IsKey;
                        row["nullable"] = fieldAttr.Nullable;
                        row["desc"] = fieldAttr.Description;
                        if (fieldAttr.DefaultValue!=null)
                        {
                            row["defaultvalue"] = fieldAttr.DefaultValue;
                        }
                    }
                    else
                    {
                        row["coltype"] = Common.NetTypeToOracleDBType(proper.PropertyType, 5);
                    }
                    tb.Rows.Add(row);
                }
                ret.Add(tb);
            }
            return ret;
        }

        public static string GetCreateTableSQL(string dbName,DataTable structTable)
        {
            DataTable tb = structTable;
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("GO");
            //sb.AppendLine();
            //sb.AppendLine("SET ANSI_NULLS ON");
            //sb.AppendLine("GO");
            //sb.AppendLine();
            //sb.AppendLine("SET QUOTED_IDENTIFIER ON");
           // sb.AppendLine("GO");

            sb.AppendFormat("CREATE TABLE {0}(", tb.TableName);
            foreach (DataRow row in tb.Rows)
            {
                sb.AppendLine();
                sb.AppendFormat("{0} {1} {2},", row["colname"].ToString(), row["coltype"].ToString(), (bool)row["isidentity"] ? "primary key" : "");
            }
            string keyCols = string.Join(",", from x in tb.AsEnumerable()
                                              where x["iskey"].Equals(true)
                                              select "`" + x["colname"] + "`");
            if (string.IsNullOrWhiteSpace(keyCols))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine(")");
            //sb.AppendLine("GO");
            //sb.AppendLine("SET ANSI_PADDING OFF");
            //sb.AppendLine("GO");

            
            
            return sb.ToString();
        }

        public static string GetInsertProcSql(DBSource dbSource, string dbName, string tbid, string tbName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("create procedure SP_{0}_I(", tbName));
            var cols=OracleHelper.GetColumns(dbSource, dbName, tbName);
            int i = 0;
            foreach (TBColumn x in cols)
            {
                i++;
                sb.AppendLine(string.Format("{0} {1} {2},", x.Name,(x.IsID ? "OUT" : ("IN")), x.TypeName));
            }
            sb.AppendLine(" P_ROWS OUT NUMBER");
            sb.Append(")");
            sb.AppendLine("as");

            sb.AppendLine("BEGIN");

            sb.AppendLine(string.Format("INSERT INTO {0}({1})",tbName, string.Join(",", cols.Where(p => !p.IsID).Select(p => p.Name))));
            sb.AppendLine(string.Format("VALUES({0});",string.Join(",",cols.Where(p=>!p.IsID).Select(p=>p.Name))));
            var idCol = cols.FirstOrDefault(p => p.IsID);

            sb.AppendLine("P_ROWS := SQL%ROWCOUNT;");

            sb.AppendLine("COMMIT;");
            sb.AppendLine("END;");

            return sb.ToString();
        }

        public static string GetDeleteProcSql(DBSource dbSource, string dbName, string tbid, string tbName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" CREATE PROCEDURE SP_{0}_D(", tbName));
            var cols = OracleHelper.GetColumns(dbSource, dbName, tbName);

            if (!cols.Any(p => p.IsKey))
            {
                throw new Exception("失败，没有指定主键");
            }

            cols = cols.Where(p => p.IsKey);
            int i = 0;
            foreach (TBColumn x in cols)
            {
                i++;
                sb.AppendLine(string.Format("{0} {1} {2},",x.Name, "IN", x.TypeName));
            }
            sb.AppendLine("P_ROWS OUT NUMBER");
            sb.Append(")");
            sb.AppendLine("as");

            sb.AppendLine("BEGIN");
            sb.AppendLine(string.Format("delete from {0} where {1};", tbName, string.Join(" and ",cols.Select(p => p.Name+"="+p.Name))));
            sb.AppendLine("P_ROWS := SQL%ROWCOUNT;");

            sb.AppendLine("COMMIT;");
            sb.AppendLine("END;");

            return sb.ToString();
        }

        public static List<GlobalStatusInfo> GetMysqlGlobalStatus(DBSource dbSource)
        {
            List<GlobalStatusInfo> list = new List<GlobalStatusInfo>();
            var dataTable = MySQLHelper.ExecuteDBTable(dbSource, "", "show global Status;");
            var dt = DateTime.Now;
            foreach(DataRow row in dataTable.AsEnumerable())
            {
                GlobalStatusInfo info = new GlobalStatusInfo
                {
                    Timestamp=dt,
                    Variable_name = (string)row["Variable_name"],
                    Val=(string)row["Value"],
                };

                list.Add(info);
            }

            return list;
        }

        public static void KillProcess(DBSource dbSource,int process)
        {
            MySQLHelper.ExecuteNoQuery(dbSource, "", "kill " + process);
        }

        public static string GetMysqlInnoDBStatus(DBSource dbSource)
        {
            var dataTable = MySQLHelper.ExecuteDBTable(dbSource, "", "Show Engine INNODB  Status;");

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["Status"].ToString();
            }

            return string.Empty;
        }

        public static List<ProcessListInfo> GetMysqlProcessList(DBSource dbSource)
        {
            var dataTable = MySQLHelper.ExecuteDBTable(dbSource, "", "SELECT * FROM `information_schema`.`PROCESSLIST`;");

            var dt = DateTime.Now;

            var list = new List<ProcessListInfo>();

            foreach(DataRow row in dataTable.AsEnumerable())
            {
                var info = new ProcessListInfo
                {
                    Cmd = (string)(row["COMMAND"] == DBNull.Value ? string.Empty : row["COMMAND"]),
                    DB = (string)(row["DB"] == DBNull.Value ? string.Empty : row["DB"]),
                    Host = (string)(row["HOST"] == DBNull.Value ? string.Empty : row["HOST"]),
                    ID=(long)row["ID"],
                    User = (string)(row["USER"] == DBNull.Value ? string.Empty : row["USER"]),
                    Info = (string)(row["INFO"] == DBNull.Value ? string.Empty : row["INFO"]),
                    State = (string)(row["STATE"] == DBNull.Value ? string.Empty : row["STATE"]),
                    Time=(int)row["Time"],
                    Timestamp=dt,
                };

                list.Add(info);
            }

            return list;
        }
    }
}
