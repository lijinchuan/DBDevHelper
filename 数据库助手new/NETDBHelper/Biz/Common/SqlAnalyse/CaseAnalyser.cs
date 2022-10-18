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

        protected override AnalyseAccept AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (hasEnd)
            {
                return AnalyseAccept.Reject;
            }
            if (sqlExpress.ExpressType == SqlExpressType.End)
            {
                hasEnd = true;
            }
            return base.AcceptInnerKey(sqlProcessor, sqlExpress);
        }

        protected override AnalyseAccept Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (hasEnd)
            {
                return AnalyseAccept.Reject;
            }
            if (sqlExpress.AnalyseType == AnalyseType.UnKnown && sqlExpress.ExpressType == SqlExpressType.Token)
            {
                sqlExpress.AnalyseType = AnalyseType.Column;
                colums.Add(sqlExpress);
            }
            return AnalyseAccept.Accept;
        }
    }
}
