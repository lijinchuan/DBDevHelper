using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class DefaultAnalyser : SqlAnalyser
    {
        private static HashSet<string> keys = new HashSet<string>();
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return "defaultKey";
        }

        protected override bool Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (sqlExpress.AnalyseType == AnalyseType.UnKnown && sqlExpress.ExpressType == SqlExpressType.Token)
            {
                sqlExpress.AnalyseType = AnalyseType.Column;
                colums.Add(sqlExpress);
            }
            return true;
        }
    }
}
