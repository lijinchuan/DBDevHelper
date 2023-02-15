using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using LJC.FrameWorkV3.Net.HTTP.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.SimulateServer
{
    public class ApiSimulateHandler : IHttpHandler
    {
        public bool Process(HttpServer server, HttpRequest request, HttpResponse response)
        {
            var url = request.Url.ToLower();
            if (url.StartsWith("http"))
            {
                var sqlArray = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (sqlArray.Length > 2)
                {
                    url = string.Join("/", sqlArray.Skip(2).ToArray());
                }
            }
            else
            {
                url = string.Join("/", url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            }
            
            var simulateResponseList=BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.Url), new object[] { url }).ToList();
            APISimulateResponse simulateResponse = null;
            if (simulateResponseList.Any())
            {
                simulateResponse = simulateResponseList.FirstOrDefault(p => p.Def);
                if (simulateResponse == null)
                {
                    simulateResponse = simulateResponseList.First();
                }
            }
            if (simulateResponse != null)
            {
                if ("OPTIONS".Equals(request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    response.Header.Add("Access-Control-Allow-Origin", "*");
                    return true;
                }
                var api = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), simulateResponse.APIId);
                if (!string.Equals(api?.APIMethod.ToString(),request.Method,StringComparison.OrdinalIgnoreCase))
                {
                    response.ReturnCode = 400;
                    response.ContentType = string.Format("{0}; charset={1}", "text/html", "utf-8");
                    response.Content = "不允许" + request.Method + "方式的请求！";
                    return true;
                }
                response.ContentType = string.Format("{0}; charset={1}", simulateResponse.ContentType, simulateResponse.Charset);
                if (simulateResponse.Headers?.Any() == true)
                {
                    foreach (var pa in simulateResponse.Headers)
                    {
                        response.Header.Add(pa.Name, pa.Value);
                    }
                }

                if (simulateResponse.ResponseCode != 200)
                {
                    response.ReturnCode = simulateResponse.ResponseCode;
                }
                if (simulateResponse.ResponseType == "文本")
                {
                    response.Content = simulateResponse.ResponseBody;
                }
                else
                {
                    var resource = BigEntityTableEngine.LocalEngine.Find<APIResource>(nameof(APIResource), simulateResponse.ReponseResourceId);
                    response.RawContent = resource.ResourceData;
                }

                return true;
            }
            else
            {
                response.ContentType = string.Format("{0}; charset={1}", "text/html", "utf-8");
                response.ReturnCode = 404;
                response.Content = request.Url+" 不存在！";

                return true;
            }
        }
    }
}
