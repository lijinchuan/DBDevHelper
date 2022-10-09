using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class TruncateAnalyser : SqlAnalyser
    {
        private readonly HashSet<string> keys = new HashSet<string> { keyTruncate, keyTable };

        public override string GetPrimaryKey()
        {
            return keyTruncate;
        }

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (sqlExpress.ExpressType == SqlExpressType.Token && lastKey == keyTable)
            {
                sqlExpress.AnalyseType = AnalyseType.Table;
                tables.Add(sqlExpress);
            }

            return true;
        }
    }
}
