using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace APIHelper.UC
{
    [ComVisible(true)]
    public class DocPage : WebTab
    {

        public void InitDoc(APIUrl apiurl,string title="文档")
        {
            if (apiurl == null)
            {
                return;
            }

            StringBuilder sbdoc = new StringBuilder();
            sbdoc.AppendLine("**简要描述：** ");
            sbdoc.AppendLine();
            sbdoc.AppendLine(apiurl.Desc);
            sbdoc.AppendLine();
            sbdoc.AppendLine("**请求URL：** ");
            sbdoc.AppendLine($"- ` {apiurl.Path} `");
            sbdoc.AppendLine();
            sbdoc.AppendLine("**请求方式：**");
            sbdoc.AppendLine($"- {apiurl.APIMethod.ToString()}");
            sbdoc.AppendLine();
            sbdoc.AppendLine("**请求参数说明：** ");
            sbdoc.AppendLine();
            sbdoc.AppendLine("|参数名|必选|类型|说明|");
            sbdoc.AppendLine("|:----    |:---|:----- |-----   |");
            var aPIParams = BigEntityTableEngine.LocalEngine.Find<APIParam>(nameof(APIParam), "APIId", new object[] { apiurl.Id }).ToList();
            foreach (var item in aPIParams.Where(p => p.Type == 0).OrderBy(p => p.Sort))
            {
                sbdoc.AppendLine($"{item.Name}|{(item.IsRequried ? "T" : "F")}|{item.TypeName}|{item.Desc}|");
            }
            sbdoc.AppendLine();

            sbdoc.AppendLine(" **返回参数说明：** ");
            sbdoc.AppendLine();
            sbdoc.AppendLine("|参数名|类型|说明|");
            sbdoc.AppendLine("|:-----  |:-----|-----                           |");
            foreach (var item in aPIParams.Where(p => p.Type == 1).OrderBy(p => p.Sort))
            {
                sbdoc.AppendLine($"{item.Name}|{item.TypeName}|{item.Desc}|");
            }
            sbdoc.AppendLine();

            var examplelist = BigEntityTableEngine.LocalEngine.Find<APIDocExample>(nameof(APIDocExample), "ApiId", new object[] { apiurl.Id });
            sbdoc.AppendLine("**示例：** ");
            sbdoc.AppendLine();
            foreach (var examle in examplelist)
            {
                sbdoc.AppendLine($"- 场景:{examle.Name}");
                sbdoc.AppendLine();
                sbdoc.AppendLine(" **请求示例**");
                sbdoc.AppendLine("");
                sbdoc.AppendLine("``` ");
                sbdoc.AppendLine(examle.Req);
                sbdoc.AppendLine("``` ");
                sbdoc.AppendLine();

                sbdoc.AppendLine();
                sbdoc.AppendLine(" **返回示例**");
                sbdoc.AppendLine("");
                sbdoc.AppendLine("``` ");
                sbdoc.AppendLine(examle.Resp);
                sbdoc.AppendLine("``` ");
                sbdoc.AppendLine();
            }

            sbdoc.AppendLine("**备注** ");
            sbdoc.AppendLine("> ");
            //sbdoc.Replace("\"", "\\\"");
            //sbdoc.Replace("\r", "\\\r");
            if (!string.IsNullOrEmpty(title))
            {
                this.Text = title;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.Append(System.IO.File.ReadAllText("marked.min.js"));
            sb.AppendLine("</script>");
            sb.AppendLine("<script type=\"text/javascript\">");

            sbdoc.Replace("\r", "\\n\\");
            //sbdoc.Replace("\n", "\\\n");
            sb.AppendLine($"var data='{sbdoc.ToString().Replace("'", "\\'")}'");
            sb.Append($"document.body.innerHTML=marked(data)");
            sb.AppendLine("</script>");
            this.SetBody($"<style type=\"text/css\">{System.IO.File.ReadAllText("ApiDoc.css")}</style>", sb.ToString());
        }
    }
}
