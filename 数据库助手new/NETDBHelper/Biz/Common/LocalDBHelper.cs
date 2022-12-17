using Entity;
using LJC.FrameWorkV3.Comm;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common
{
    public static class LocalDBHelper
    {
        public static List<MarkObjectInfo> GetAllMarkObjectInfoFromCach()
        {
            var key = "GetAllMarkObjectInfoFromCach";
            var list=LocalCacheManager<List<MarkObjectInfo>>.FindCache(key,
                ()=> BigEntityTableRemotingEngine.List<MarkObjectInfo>("MarkObjectInfo", 1, int.MaxValue).ToList(),
                3);
            return list;
        }

        public static List<SPInfo> GetAllSPInfoFromCach()
        {
            var key = "GetAllSPInfoFromCach";
            var list = LocalCacheManager<List<SPInfo>>.FindCache(key,
                () => BigEntityTableRemotingEngine.List<SPInfo>("SPInfo", 1, int.MaxValue).ToList(),
                3);
            return list;
        }
    }
}
