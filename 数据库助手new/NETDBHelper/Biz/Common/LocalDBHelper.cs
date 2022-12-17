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

        public static List<MarkObjectInfo> GetMarkObjectInfoFromCach(string servername,string dbname,string tbname)
        {
            var key = "GetAllMarkObjectInfoDicFromCach";
            var dic = LocalCacheManager<Dictionary<string, List<MarkObjectInfo>>>.Find(key, () =>
            {
                var list = GetAllMarkObjectInfoFromCach();

                return list.GroupBy(p => $"{p.Servername.ToUpper()}_{p.DBName.ToUpper()}_{p.TBName.ToUpper()}").ToDictionary(p => p.Key, q => q.ToList());
            }, 1);

            var datakey = $"{servername.ToUpper()}_{dbname.ToUpper()}_{tbname.ToUpper()}";

            if (dic.ContainsKey(datakey))
            {
                return dic[datakey];
            }

            return new List<MarkObjectInfo>();
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
