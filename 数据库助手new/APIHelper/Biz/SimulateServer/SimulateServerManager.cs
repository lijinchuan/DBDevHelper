using Entity;
using LJC.FrameWorkV3.Net.HTTP.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.SimulateServer
{
    public static class SimulateServerManager
    {
        static HttpServer manhttpserver = null;

        private static void InitServer()
        {
            manhttpserver.Handlers.Add(new RESTfulApiHandlerBase(LJC.FrameWorkV3.Net.HTTP.Server.HMethod.GET, "/esb/index", new List<string>() { }, new DefaultHander()));
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool StartServer(int port)
        {
            if (manhttpserver == null)
            {
                manhttpserver = new HttpServer(new Server(port));
                InitServer();
                return true;
            }
            else if (manhttpserver.Server.Port != port)
            {
                manhttpserver.Server.Close();

                manhttpserver = new HttpServer(new Server(port));

                InitServer();
                return true;
            }

            return false;
        }
    }
}
