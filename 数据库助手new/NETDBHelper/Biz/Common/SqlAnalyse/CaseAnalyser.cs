using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class CaseAnalyser : SqlAnalyser
    {
        private static HashSet<string> keys = new HashSet<string> { keyCase, keyWhen, keyThen, keyElse, keyEnd };
        private bool hasEnd = false;
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyCase;
        }

        protected override bool AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (hasEnd)
            {
                return false;
            }
            if (sqlExpress.ExpressType == SqlExpressType.End)
            {
                hasEnd = true;
            }
            return base.AcceptInnerKey(sqlProcessor, sqlExpress);
        }

        protected override bool Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (hasEnd)
            {
                return false;
            }
            if (sqlExpress.AnalyseType == AnalyseType.UnKnown && sqlExpress.ExpressType == SqlExpressType.Token)
            {
                sqlExpress.AnalyseType = AnalyseType.Column;
                colums.Add(sqlExpress);
            }
            return true;
        }
    }
}
