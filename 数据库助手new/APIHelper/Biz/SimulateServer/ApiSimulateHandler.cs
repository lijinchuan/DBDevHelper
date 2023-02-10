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
            
            var simulateResponse=BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.Url), new object[] { url }).FirstOrDefault();
            if (simulateResponse != null)
            {
                response.ContentType = string.Format("{0}; charset={1}", simulateResponse.ContentType, simulateResponse.Charset);
                if (simulateResponse.Headers?.Any() == true)
                {
                    foreach (var pa in simulateResponse.Headers)
                    {
                        response.Header.Add(pa.Name, pa.Value);
                    }
                }

                //response.ReturnCode = 200;
                if (!string.IsNullOrWhiteSpace(simulateResponse.ResponseBody))
                {
                    response.Content = simulateResponse.ResponseBody;
                }

                return true;
            }
            else
            {
                response.ContentType = string.Format("{0}; charset={1}", "text/html", "utf-8");
                //response.ReturnCode = 404;
                response.Content = request.Url+" 不存在！";

                return true;
            }
        }
    }
}
