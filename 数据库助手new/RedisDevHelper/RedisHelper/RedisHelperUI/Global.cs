using LJC.FrameWork.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisHelperUI
{
    public class Global
    {
        public const string TBName_RedisServer = "RedisServers";
        public const string TBName_SearchLog = "SearchLog";

        public static void Init()
        {
            EntityTableEngine.LocalEngine.CreateTable(TBName_RedisServer, "ServerName", typeof(RedisHelper.Model.RedisServerEntity));
            EntityTableEngine.LocalEngine.CreateTable(TBName_SearchLog, "Key", typeof(SearchLog));
        }
    }
}
