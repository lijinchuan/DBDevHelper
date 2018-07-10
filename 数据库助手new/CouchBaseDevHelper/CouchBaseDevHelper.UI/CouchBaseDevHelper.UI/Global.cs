using LJC.FrameWork.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchBaseDevHelper.UI
{
    public class Global
    {
        public const string TBName_RedisServer = "CouchBaseServers";
        public const string TBName_SearchLog = "SearchLog";

        public static void Init()
        {
            EntityTableEngine.LocalEngine.CreateTable(TBName_RedisServer, "ServerName", typeof(CouchBaseServerEntity));
            EntityTableEngine.LocalEngine.CreateTable(TBName_SearchLog, "Key", typeof(SearchLog));
        }
    }
}
