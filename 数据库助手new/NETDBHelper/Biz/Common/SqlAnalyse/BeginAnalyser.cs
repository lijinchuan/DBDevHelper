using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class BeginAnalyser : SqlAnalyser
    {
        private HashSet<string> keys = new HashSet<string> { keyBegin, keyTransaction };
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyBegin;
        }

        protected override AnalyseAccept AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            //
            if (sqlExpress.Val == keyBegin && sqlProcessor.GetNext()?.Val == keyTransaction)
            {
                return AnalyseAccept.Accept;
            }
            return AnalyseAccept.Reject;
        }

        protected override AnalyseAccept Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            return AnalyseAccept.Reject;
        }
    }
}
