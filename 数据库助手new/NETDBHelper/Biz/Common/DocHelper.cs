using Biz.Common.Data;
using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biz.Common
{
    public static class DocHelper
    {
        /// <summary>
        /// 生成数据字典
        /// </summary>
        /// <param name="server"></param>
        /// <param name="tableinfo"></param>
        /// <returns></returns>
        public static string GetDataDoc(DBSource server, TableInfo tableinfo)
        {
            //库名
            string tbname = string.Format("[{0}].[{1}]", tableinfo.DBName, tableinfo.TBName);

            var tbclumns = SQLHelper.GetColumns(server, tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, tableinfo.Schema).ToList();

            var tbmark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                             [] { tableinfo.DBName.ToUpper(), tableinfo.TBName.ToUpper(), string.Empty }).FirstOrDefault();
            var tbdesc = tbmark == null ? (tableinfo.Desc ?? tableinfo.TBName) : tbmark.MarkInfo;

            DataTable resulttb = new DataTable();
            resulttb.Columns.AddRange(new string[][] {
                    new []{"line","行号"},
                    new []{"name","列名"},
                    new []{"iskey","是否主键"},
                    new []{"null","可空"},
                    new []{"type","类型"},
                    new []{"defaultvalue","默认值"},
                    new []{"len","长度"},
                    new []{"desc","说明"} }.Select(s => new DataColumn
                    {
                        ColumnName = s[0],
                        Caption = s[1],
                    }).ToArray());

            var tbDesc = Biz.Common.Data.SQLHelper.GetTableColsDescription(server, tableinfo.DBName, tableinfo.TBName);

            int idx = 1;
            foreach (var col in tbclumns)
            {
                var newrow = resulttb.NewRow();
                newrow["line"] = idx++;

                object y = null;
                var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                        [] { tableinfo.DBName.ToUpper(), tableinfo.TBName.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();
                if (mark != null && !string.IsNullOrWhiteSpace(mark.MarkInfo))
                {
                    y = mark.MarkInfo;
                }
                else
                {
                    y = (from x in tbDesc.AsEnumerable()
                         where string.Equals((string)x["ColumnName"], col.Name, StringComparison.OrdinalIgnoreCase)
                         select x["Description"]).FirstOrDefault();
                }

                //字段描述
                string desc = y == DBNull.Value ? "&nbsp;" : (string)y;
                newrow["desc"] = desc;
                newrow["name"] = col.Name;
                newrow["type"] = col.TypeName;

                bool iskey = col.IsKey;

                newrow["iskey"] = iskey ? "√" : "✕";


                newrow["len"] = col.Length == -1 ? "max" : col.Length.ToString();
                newrow["null"] = col.IsNullAble ? "√" : "✕";
                newrow["defaultvalue"] = col.DefaultValue == null ? "" : DataHelper.TrimDefaultValue(col.DefaultValue.ToString());


                resulttb.Rows.Add(newrow);

            }

            //生成HTML
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<html><head><title>数据字典-{0}</title><style>
p{{font-size:11px;}}
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style></head><body><p>表名：{0}</p><p>表说明：{1}</p><table cellpadding='0' cellspacing='0' border='1'>", tbname, tbdesc);
            sb.Append("<tr>");
            foreach (DataColumn col in resulttb.Columns)
            {
                sb.AppendFormat("<th>{0}</th>", col.Caption);
            }
            sb.Append("</tr>");

            foreach (DataRow row in resulttb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn col in resulttb.Columns)
                {
                    sb.AppendFormat("<td>{0}</td>", row[col.ColumnName]);
                }
                sb.Append("</tr>");
            }

            sb.Append("</table></body></html>");


            return sb.ToString();
        }

        /// <summary>
        /// 生成数据字典
        /// </summary>
        /// <param name="server"></param>
        /// <param name="tableinfo"></param>
        /// <returns></returns>
        public static void CreateDataDoc(DBSource server, TableInfo tableinfo)
        {
            var dir = Application.StartupPath + "\\temp\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            dir = Path.Combine(dir, "dataDoc");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            dir = Path.Combine(dir, server.ServerName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            dir = Path.Combine(dir, tableinfo.DBName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var html = GetDataDoc(server, tableinfo);
            var path = Path.Combine(dir, $"{tableinfo.TBName}.html");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, html, Encoding.UTF8);
        }
    }
}
