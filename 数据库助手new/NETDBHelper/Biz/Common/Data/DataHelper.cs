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
            if (!new Regex(@"using\s+LJC.FrameWork.Data.QuickDataBase").IsMatch(code))
            {
                code = "using LJC.FrameWork.Data.QuickDataBase;\r\n" + code;
            }

            if (!new Regex(@"using\s+Entity").IsMatch(code))
            {
                code = "using Entity;\r\n" + code;
            }

            if (!new Regex(@"using\s+System").IsMatch(code))
            {
                code = "using System;\r\n" + code;
            }

            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            CompilerParameters paramerters = new CompilerParameters();
            paramerters.ReferencedAssemblies.Add("System.Core.dll");
            paramerters.ReferencedAssemblies.Add("Entity.dll");
            paramerters.ReferencedAssemblies.Add("LJC.FrameWork.dll");
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
                    var fieldAttr=(DBFieldMapAttribute)proper.GetCustomAttributes(typeof(DBFieldMapAttribute), true).FirstOrDefault();
                    if (fieldAttr != null)
                    {
                        if (!string.IsNullOrWhiteSpace(fieldAttr.FieldName))
                        {
                            row["colname"] = fieldAttr.FieldName;
                        }
                        if (!string.IsNullOrWhiteSpace(fieldAttr.DBType))
                        {
                            row["coltype"] = fieldAttr.DBType;
                        }
                        else
                        {
                            row["coltype"] = Common.NetTypeToDBType(proper.PropertyType, fieldAttr.Len);
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
                        row["coltype"] = Common.NetTypeToDBType(proper.PropertyType, 50);
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
            sb.AppendFormat("USE [{0}]", dbName);
            sb.AppendLine();
            //sb.AppendLine("GO");
            sb.AppendLine("SET ANSI_NULLS ON");
            //sb.AppendLine("GO");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            //sb.AppendLine("GO");
            sb.AppendFormat("CREATE TABLE [{0}](", tb.TableName);
            foreach (DataRow row in tb.Rows)
            {
                sb.AppendLine();
                sb.AppendFormat("[{0}] {1} {2} {3} NULL,", row["colname"].ToString(), row["coltype"].ToString(), (bool)row["isidentity"] ? "IDENTITY(1,1)" : "", !(bool)row["nullable"] || (bool)row["iskey"] || (bool)row["isidentity"] ? "NOT" : "");
            }
            string keyCols = string.Join(",", from x in tb.AsEnumerable()
                                              where x["iskey"].Equals(true)
                                              select x["colname"] + " ASC");
            if (string.IsNullOrWhiteSpace(keyCols))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(keyCols))
            {
                sb.AppendFormat("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ", tb.TableName);
                sb.AppendLine();
                sb.AppendLine("(");
                sb.AppendLine(keyCols);
                sb.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
            }
            sb.AppendLine(") ON [PRIMARY]");
            //sb.AppendLine("GO");
            sb.AppendLine("SET ANSI_PADDING OFF");
            //sb.AppendLine("GO");

            var descCols = from x in tb.AsEnumerable()
                           where x["desc"] != DBNull.Value
                           select x;
            foreach (DataRow row in descCols)
            {
                sb.AppendFormat("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}', @level2type=N'COLUMN',@level2name=N'{2}'",
                   row["desc"].ToString(), tb.TableName, row["colname"].ToString());
                sb.AppendLine();
                //sb.AppendLine("GO");
            }
            var dftCols = from x in tb.AsEnumerable()
                          where x["defaultvalue"] != DBNull.Value
                          select x;
            foreach (DataRow row in dftCols)
            {
                sb.AppendFormat("ALTER TABLE [dbo].[{0}] ADD  DEFAULT ({1}) FOR [{2}]", tb.TableName, row["defaultvalue"], row["colname"].ToString());
                sb.AppendLine();
                //sb.AppendLine("GO");
            }
            return sb.ToString();
        }


        public static string GetBatInsertProcSql(DBSource dbSource, string dbName, string tbid, string tbName)
        {
            var cols = SQLHelper.GetColumns(dbSource, dbName, tbid, tbName).ToList();

            string idcolname = string.Empty;
            var idcol=cols.Find(p => p.IsID);
            if (idcol != null)
            {
                idcolname = idcol.Name;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------创建用户自定义类型作为批量插入存储过程的输入参数----------");

            string midTableName="Mulitinsertmidtable$"+tbName;
            sb.AppendLine(string.Format("USE [{0}]", dbName));
            sb.AppendLine("GO");
            sb.AppendLine(string.Format("CREATE TYPE [dbo].[{0}] AS TABLE(",midTableName));
            for (int i = 0; i < cols.Count(); i++)
            {
                var x = cols[i];
                sb.AppendLine(string.Format("[{0}] {1} {2},", x.Name, Common.GetDBType(x), x.IsNullAble ? "null" : "not null"));
            }
            sb.AppendLine(string.Format("{0} {1} {2}","[state]","int","not null"));
            sb.AppendLine(")");
            sb.AppendLine("GO");
            sb.AppendLine("-----------------------下面创建存储过程----------------------------------");


            sb.AppendLine(string.Format("USE [{0}]", dbName));
            sb.AppendLine("GO");
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("GO");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine("GO");
            sb.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_mulitinsert]", tbName));
            sb.AppendLine(string.Format("@dt  {0} readonly",midTableName));
            sb.AppendLine("AS");
            sb.AppendLine("Begin");
            sb.AppendLine("SET NOCOUNT ON");
            sb.AppendLine("-----------------------插入----------------------------------");
            sb.AppendFormat("insert {0}({1}) select {2} from @dt t2 where [state]=1", tbName, string.Join(",\r\n", cols.Where(p => !p.IsID).Select(p => "["+p.Name+"]")),
                string.Join(",\r\n",cols.Where(p=>!p.IsID).Select(p=>"t2."+p.Name)));
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("-----------------------更新----------------------------------");
            if (!string.IsNullOrEmpty(idcolname))
            {
                sb.AppendFormat(@"  update t1 set {0} from {1} t1 inner join @dt t2 on t1.{2}=t2.{2} where t2.[State]=2",
                    string.Join(",\r\n", cols.Where(p => !p.IsID).Select(p => "t1." + p.Name + "=t2." + p.Name)),
                    tbName,
                    idcolname
                    );
            }
            else
            {
                sb.AppendLine("--缺少自增主键");
            }
            sb.AppendLine();
            sb.AppendLine("-----------------------删除----------------------------------");
            if (!string.IsNullOrEmpty(idcolname))
            {
                sb.AppendFormat(@"delete [{0}] where [{1}] in( select [{1}] from @dt t2 where t2.[state]=3)", tbName, idcolname);
            }
            else
            {
                sb.AppendLine("--缺少自增主键");
            }
            sb.AppendLine();
            sb.AppendLine("End");
            return sb.ToString();
        }

        public static string GetDeleteSql(DBSource dbSource, string dbName, string tbid, string tbName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SET LOCK_TIMEOUT 1000");
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine(string.Format("USE [{0}]", dbName));
            sb.AppendLine("GO");
            sb.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_{1}_delete]", dbName, tbName));
            var cols = SQLHelper.GetColumns(dbSource, dbName, tbid, tbName).Where(p => p.TypeName.IndexOf("timestamp", StringComparison.OrdinalIgnoreCase) == -1).ToList();
            
            var idcol = cols.Find(p => p.IsID);
            var keys = cols.Where(p => p.IsKey && !p.IsID).ToList();

            if (idcol == null && keys.Count == 0)
            {
                throw new Exception("无法生成删除存储过程，未定义自增主键或者其它主键！");
            }

            if (idcol != null)
            {
                sb.AppendLine(string.Format("@{0} {1}", idcol.Name, Common.GetDBType(idcol)));
                
            }
            else
            {
                sb.AppendLine(string.Join(",\r\n",keys.Select(p=>string.Format("@{0} {1}",p.Name,Common.GetDBType(p)))));
                
            }

            sb.AppendLine("AS");
            sb.AppendLine("DECLARE @retcode int,");
            sb.AppendLine("@rowcount int");

            if (idcol != null)
            {
                sb.AppendLine(string.Format("delete [{0}] where [{1}]=@{1}", tbName,
                    idcol.Name));
            }
            else
            {
                sb.AppendLine(string.Format("delete [{0}] where {1}", tbName,
                     string.Join(" and ", keys.Select(p => string.Format("[{0}]=@{0}", p.Name)))));
            }

            sb.AppendLine("SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT");
            sb.AppendLine("IF @retcode = 0 AND @rowcount = 0");
            sb.AppendLine("RETURN 100");
            sb.AppendLine("ELSE");
            sb.AppendLine("RETURN @retcode");
            return sb.ToString();
        }

        public static string GetUpsertProcsql(DBSource dbSource, string dbName, string tbid, string tbName)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SET LOCK_TIMEOUT 1000");
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine(string.Format("USE [{0}]", dbName));
            sb.AppendLine("GO");
            sb.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_{1}_upsert]", dbName, tbName));
            var cols = SQLHelper.GetColumns(dbSource, dbName, tbid, tbName).Where(p => p.TypeName.IndexOf("timestamp", StringComparison.OrdinalIgnoreCase) == -1).ToList();
            var idcol = cols.Find(p => p.IsID);
            var keys = cols.Where(p => p.IsKey && !p.IsID).ToList();

            for (int i = 0; i < cols.Count(); i++)
            {
                var x = cols[i];
                if (i < cols.Count() - 1)
                {
                    sb.AppendLine(string.Format("@{0} {1} {2},", x.Name, Common.GetDBType(x), (x.IsID ? "output" : (x.IsNullAble ? "= null" : ""))));
                }
                else
                {
                    sb.AppendLine(string.Format("@{0} {1} {2}", x.Name, Common.GetDBType(x), (x.IsID ? "output" : (x.IsNullAble ? "= null" : ""))));
                }
            }
            sb.AppendLine("AS");
            sb.AppendLine("DECLARE @retcode int,");
            sb.AppendLine("@rowcount int,");
            sb.AppendLine("@updated bit");
            sb.AppendLine("set @updated=0");

            if (idcol != null)
            {
                sb.AppendLine(string.Format("if @{0}>0",idcol.Name));
                sb.AppendLine(" begin");

                sb.AppendLine(string.Format("  update [{0}] set {1} where [{2}]=@{2}", tbName,
                    string.Join(",", cols.Where(p => !p.IsKey && !p.IsID).Select(p => string.Format("[{0}]=isnull(@{0},[{0}])", p.Name))),
                    idcol.Name));
                sb.AppendLine("set @updated=1");
                sb.AppendLine(" end");
            }

            if (keys.Count > 0)
            {
                sb.AppendLine(string.Format("if @updated=0 and exists(select 1 from {0} where {1})", tbName, string.Join(" and ", keys.Select(p => string.Format("{0}=@{0}", p.Name)))));
                sb.AppendLine(" begin");
                sb.AppendLine(string.Format("  update [{0}] set {1} where {2}", tbName,
                    string.Join(",", cols.Where(p => !p.IsKey && !p.IsID).Select(p => string.Format("[{0}]=isnull(@{0},[{0}])", p.Name))),
                    string.Join(" and ", keys.Select(p => string.Format("{0}=@{0}", p.Name)))));
                sb.AppendLine("  set @updated=1");
                sb.AppendLine(" end");
                sb.AppendLine("");
            }

            sb.AppendLine("if @updated=0");
            sb.AppendLine("begin");
            sb.AppendLine(string.Format(" INSERT INTO {0}({1})", tbName, string.Join(",", cols.Where(p => !p.IsID).Select(p => p.Name))));
            sb.AppendLine(string.Format(" VALUES({0})", string.Join(",", cols.Where(p => !p.IsID).Select(p => string.Concat("@", p.Name)))));
            sb.AppendLine("end");

            if (idcol != null)
            {
                sb.AppendFormat("if @{0}=0",idcol.Name);
                sb.AppendLine();
                sb.AppendLine(string.Format(" SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT,@{0}=@@IDENTITY", idcol.Name));
                sb.AppendLine("else");
                sb.AppendLine(" SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT");
            }
            else
                sb.AppendLine("SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT");
            sb.AppendLine("IF @retcode = 0 AND @rowcount = 0");
            sb.AppendLine("RETURN 100");
            sb.AppendLine("ELSE");
            sb.AppendLine("RETURN @retcode");
            return sb.ToString();
        }

        public static bool OrderColumn(bool up,string tbid,string tbName,string colname)
        {
             
            return false;
        }

        public static string GetUpdateProcSql(DBSource dbSource, string dbName, string tbid, string tbName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SET LOCK_TIMEOUT 1000");
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine(string.Format("USE [{0}]", dbName));
            sb.AppendLine("GO");
            sb.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_{1}_update]", dbName, tbName));
            var cols = SQLHelper.GetColumns(dbSource, dbName, tbid, tbName).Where(p=>p.TypeName.IndexOf("timestamp",StringComparison.OrdinalIgnoreCase)==-1).ToList();
            //foreach(TBColumn x in cols)
            for (int i = 0; i < cols.Count(); i++)
            {
                var x = cols[i];
                if (i < cols.Count() - 1)
                {
                    sb.AppendLine(string.Format("@{0} {1} {2},", x.Name, Common.GetDBType(x), (x.IsNullAble ? "= null" : "")));
                }
                else
                {
                    sb.AppendLine(string.Format("@{0} {1} {2}", x.Name, Common.GetDBType(x), (x.IsNullAble ? "= null" : "")));
                }
            }

            var idcol = cols.Find(p => p.IsID);
            var keys = cols.Where(p => p.IsKey && !p.IsID).ToList();

            if (idcol == null && keys.Count == 0)
            {
                throw new Exception("无法生成更新存储过程，未定义自增主键或者其它主键！");
            }

            sb.AppendLine("AS");
            sb.AppendLine("DECLARE @retcode int,");
            sb.AppendLine("@rowcount int");

            if (idcol != null)
            {
                sb.AppendLine(string.Format("update [{0}] set {1} where [{2}]=@{2}", tbName,
                    string.Join(",", cols.Where(p => !p.IsKey && !p.IsID).Select(p =>string.Format("[{0}]=isnull(@{0},[{0}])",p.Name))),
                    idcol.Name));
                
            }
            else
            {
                sb.AppendLine(string.Format("update [{0}] set {1} where {2}", tbName,
                    string.Join(",", cols.Where(p => !p.IsKey && !p.IsID).Select(p => string.Format("[{0}]=isnull(@{0},[{0}])", p.Name))),
                    string.Join(" and ", keys.Select(p => string.Format("{0}=@{0}",p.Name)))));
            }

            sb.AppendLine("SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT");
            sb.AppendLine("IF @retcode = 0 AND @rowcount = 0");
            sb.AppendLine("RETURN 100");
            sb.AppendLine("ELSE");
            sb.AppendLine("RETURN @retcode");
            return sb.ToString();
        }

        public static string GetInsertProcSql(DBSource dbSource, string dbName, string tbid, string tbName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SET LOCK_TIMEOUT 1000");
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine(string.Format("USE [{0}]",dbName));
            sb.AppendLine("GO");
            sb.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_{1}_insert]",dbName,tbName));
            var cols=SQLHelper.GetColumns(dbSource, dbName, tbid, tbName).ToList();
            //foreach(TBColumn x in cols)
            for (int i = 0; i < cols.Count();i++ )
            {
                var x = cols[i];
                if (i < cols.Count() - 1)
                {
                    sb.AppendLine(string.Format("@{0} {1} {2},", x.Name, Common.GetDBType(x), (x.IsID ? "output" : (x.IsNullAble ? "= null" : ""))));
                }
                else
                {
                    sb.AppendLine(string.Format("@{0} {1} {2}", x.Name, Common.GetDBType(x), (x.IsID ? "output" : (x.IsNullAble ? "= null" : ""))));
                }
            }
            
            sb.AppendLine("AS");
            sb.AppendLine("DECLARE @retcode int,");
            sb.AppendLine("@rowcount int");
            
            sb.AppendLine(string.Format("INSERT INTO {0}({1})",tbName, string.Join(",", cols.Where(p => !p.IsID).Select(p => p.Name))));
            sb.AppendLine(string.Format("VALUES({0})",string.Join(",",cols.Where(p=>!p.IsID).Select(p=>string.Concat("@",p.Name)))));
            var idCol = cols.FirstOrDefault(p => p.IsID);
            if (idCol != null)
                sb.AppendLine(string.Format("SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT,@{0}=@@IDENTITY", idCol.Name));
            else
                sb.AppendLine("SELECT @retcode = @@ERROR, @rowcount = @@ROWCOUNT");
            sb.AppendLine("IF @retcode = 0 AND @rowcount = 0");
            sb.AppendLine("RETURN 100");
            sb.AppendLine("ELSE");
            sb.AppendLine("RETURN @retcode");
            return sb.ToString();
        }

        public static string CreateTableEntity(DBSource dbsource, string dbname, string tbname, string tid,string classnamespace,bool isview, bool isSupportProtobuf,
            bool isSupportDBMapperAttr, bool isSupportJsonproterty, bool isSupportMvcDisplay,Func<string,string> getDesc, out bool hasKey)
        {
            hasKey = false;
            DataTable tbDesc = null;
            if (isview)
            {
                tbDesc = new DataTable();
                tbDesc.Columns.Add("ColumnName");
                tbDesc.Columns.Add("Description");
            }
            else
            {
                tbDesc = SQLHelper.GetTableColsDescription(dbsource, dbname, tbname);
            }

            Regex rg = new Regex(@"(\w+)\s*\((\w+)\)");
            string format = @"        {4}public {0} {1}
        {{
            get
            {{
                return {2};
            }}
            set
            {{
                {3}=value;
            }}
        }}";


            StringBuilder sb = new StringBuilder(string.Format("namespace {0}\r\n", classnamespace));
            sb.AppendLine("{");
            if (isSupportProtobuf)
                sb.AppendLine("    [ProtoContract]");
            if (isSupportDBMapperAttr)
                sb.AppendLine("    [DataBaseMapperAttr(TableName=\"" + tbname + "\")]");
            sb.Append("    public class ");
            sb.Append(Biz.Common.StringHelper.FirstToUpper(tbname));
            sb.Append("Entity");
            sb.Append("\r\n    {\r\n");

            if (isSupportDBMapperAttr)
            {
                sb.AppendLine("        //表名");
                sb.AppendLine(string.Format("        public const string TbName=\"{0}.{1}\";", dbname, tbname));
            }

            var cols =isview? SQLHelper.GetViews(dbsource,dbname,tbname).First().Value: SQLHelper.GetColumns(dbsource, dbname, tid, tbname);
            
            //TreeNode selNode = tv_DBServers.SelectedNode;
            int idx = 1;
            //foreach (TreeNode column in selNode.Nodes)
            foreach (var column in cols)
            {
                sb.AppendLine();

                var y = (from x in tbDesc.AsEnumerable()
                         where string.Equals((string)x["ColumnName"], column.Name, StringComparison.OrdinalIgnoreCase)
                         select x["Description"]).FirstOrDefault();

                string desc = y == DBNull.Value ? getDesc(column.Name) : (string)y;

                string privateAttr = string.Concat("_" + Biz.Common.StringHelper.FirstToLower(column.Name));
                sb.AppendFormat("        private {0} {1};", Biz.Common.Data.Common.DbTypeToNetType(column.TypeName,column.IsNullAble), privateAttr);
                sb.AppendLine();

                sb.AppendLine(@"        /// <summary>");
                sb.AppendLine($@"        /// {desc}");
                sb.AppendLine(@"        /// </summary>");

                if (isSupportProtobuf)
                {
                    sb.AppendLine(string.Format("        [ProtoMember({0})]", idx++));
                }

                bool iskey = column.IsKey;

                if (isSupportDBMapperAttr)
                {
                    if (iskey)
                    {
                        sb.AppendLine("        [DataBaseMapperAttr(Column=\"" + column.Name + "\",isKey=true)]");
                        hasKey = true;
                    }
                    else
                    {
                        sb.AppendLine("        [DataBaseMapperAttr(Column=\"" + column.Name + "\")]");
                    }
                }

                if (isSupportJsonproterty)
                {
                    sb.AppendLine("        [JsonProperty(\"" + column.Name.ToLower() + "\")]");
                    sb.AppendLine("        [PropertyDescriptionAttr(\"" + desc + "\")]");
                }

                if (iskey)
                {
                    sb.AppendLine("        [Key]");
                }
                
                sb.AppendFormat(format, Biz.Common.Data.Common.DbTypeToNetType(column.TypeName,column.IsNullAble), Biz.Common.StringHelper.FirstToUpper(column.Name),
                    privateAttr, privateAttr, isSupportMvcDisplay ? string.Format("[Display(Name = \"{0}\")]\r\n        ", string.IsNullOrWhiteSpace(desc) ? column.Name : desc) : string.Empty);
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string CreateSelectSql(string dbname,string tbname,string editer,string spabout,List<TBColumn> cols,List<TBColumn> conditioncols, List<TBColumn> outputcols)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("USE [{0}]", dbname);
            sb.AppendLine();
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("-- =============================================");
            sb.AppendLine(string.Format("-- Author:	     {0}", editer));
            sb.AppendLine(string.Format("-- Create date: {0}", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")));
            sb.AppendLine(string.Format("-- Description: {0}", spabout.Replace("\r\n", "\r\n--")));
            sb.AppendLine("-- =============================================");
            sb.AppendFormat("create PROCEDURE [dbo].[{0}List] ", tbname);
            sb.AppendLine();
            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendFormat("@{0} {1}=NULL, --{2}", col.Name, col.TypeToString(), col.Description);
                sb.AppendLine();
            }
            sb.AppendLine("@OrderBy varchar(200)=NULL,");
            sb.AppendLine("@pageSize int=20,	--每页显示记录数");
            sb.AppendLine("@pageIndex	int=1, --第几页");
            sb.AppendLine("@recordCount    int output --记录总数");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("	SET NOCOUNT ON;");
            sb.AppendLine();
            sb.AppendLine("	-- 拼接sql语句");
            sb.AppendLine("	declare @sql nvarchar(4000)");
            sb.AppendLine("	declare @where nvarchar(4000)");
            sb.AppendLine("	set @where=' where 1=1 '");
            sb.AppendLine();

            string orderBy = string.Join(",", cols.Where(p => p.IsKey).Select(p => p.Name + " DESC"));
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = cols.Where(p => p.IsID).Select(p => p.Name + " DESC").FirstOrDefault();
            }
            sb.AppendLine("   if @OrderBy is null");
            sb.AppendLine("   begin");
            sb.AppendLine(string.Format("      set @OrderBy='{0}'", orderBy));
            sb.AppendLine("   end");
            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendLine(string.Format("--{0}", col.Description));
                sb.AppendLine(string.Format("	if @{0}  is not null ", col.Name));
                sb.AppendLine("	begin");
                sb.AppendLine();
                if (col.IsString())
                {
                    sb.AppendLine(string.Format("	    set @{0} ='%'+@{0} +'%'", col.Name));
                    sb.AppendLine(string.Format("	    set @where=@where+' and [{0}] like @{0}  '", col.Name));
                }
                else if (col.IsNumber() || col.IsBoolean())
                {
                    sb.AppendLine(string.Format("       set @where=@where+' and [{0}]=@{0}  '", col.Name));
                }
                else if (col.IsDateTime())
                {
                    sb.AppendLine(string.Format("       set @{0} ='%'+@{0} +'%'", col.Name));
                    sb.AppendLine(string.Format("       set @where=@where+' and [{0}]=@{0}  '", col.Name));
                }
                sb.AppendLine(" end");
                sb.AppendLine();
            }

            sb.AppendLine(string.Format("   set @sql='select @recordCount = count(1) From {0}(nolock) '+@where", tbname));

            sb.AppendLine(" exec sp_executesql @sql,");
            sb.AppendLine();
            sb.Append("N'");
            //foreach (var col in cols)
            foreach (var col in conditioncols)
            {
                sb.AppendFormat("@{0} {1},", col.Name, col.TypeToString());
            }
            sb.Append("@OrderBy varchar(200),@recordCount int output");
            sb.AppendLine("',");
            //sb.Append(string.Join(",",cols.Select(p=>"@"+p.Name)));
            sb.Append(string.Join(",", conditioncols.Select(p => "@" + p.Name)));
            sb.Append(",@OrderBy,@recordCount output");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(" set @sql = '");
            //sb.AppendLine("	Select a.* From (");
            sb.AppendLine(string.Format("	Select {0} From (", string.Join(",", outputcols.Select(p => "[a].[" + p.Name + "]"))));
            sb.AppendLine("     Select row_number() over(order by '+@OrderBy+') as rowID,");
            //sb.AppendLine("		"+string.Join(",",cols.Select(p=>"["+p.Name+"]")));
            sb.AppendLine("		" + string.Join(",", outputcols.Select(p => "[" + p.Name + "]")));
            sb.AppendLine(string.Format("	FROM [{0}](nolock)  ' + @where", tbname));
            sb.AppendLine("    + ' ) a Where rowID > @pageSize*(@pageIndex-1) and rowID<=@pageSize*@pageIndex'  ");
            sb.AppendLine();
            sb.AppendLine(" exec sp_executesql @sql,");
            //sb.AppendLine(string.Format("		N'{0},@OrderBy varchar(200),@pageSize int,@pageIndex int',",
            //    string.Join(",",cols.Select(p=>string.Format("@{0} {1}",p.Name,p.TypeToString())))));
            sb.AppendLine(string.Format("		N'{0},@OrderBy varchar(200),@pageSize int,@pageIndex int',",
                string.Join(",", conditioncols.Select(p => string.Format("@{0} {1}", p.Name, p.TypeToString())))));
            //sb.AppendLine(string.Format("		{0},@OrderBy,@pageSize,@pageIndex", string.Join(",", cols.Select(p => "@" + p.Name))));
            sb.AppendLine(string.Format("		{0},@OrderBy,@pageSize,@pageIndex", string.Join(",", conditioncols.Select(p => "@" + p.Name))));
            sb.AppendLine("");
            sb.AppendLine("");

            //给ef生成查询结果用的，生成后删除
            sb.AppendLine("--下面语句给EF生成调用使用的");
            sb.AppendFormat("--select top 1 {0} from {1}(nolock)", string.Join(",", outputcols.Select(p => "[" + p.Name + "]")), tbname);
            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("END");
            sb.AppendLine("GO");

            return sb.ToString();
        }
    }
}
