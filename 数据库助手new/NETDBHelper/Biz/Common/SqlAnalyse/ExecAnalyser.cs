using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class ExecAnalyser : SqlAnalyser
    {
        private static HashSet<string> keys = new HashSet<string> { keyExec, keyExecute, keyOutput };

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyExec;
        }

        protected override AnalyseAccept Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {

            return AnalyseAccept.Accept;
        }
    }
}
