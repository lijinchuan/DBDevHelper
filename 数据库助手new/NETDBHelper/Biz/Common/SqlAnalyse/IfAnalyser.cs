using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class IfAnalyser : SqlAnalyser
    {
        private static HashSet<string> keys = new HashSet<string> { keyIf, keyElse };
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyIf;
        }

        protected override bool AcceptOuterKey(ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastKey == keyElse)
            {
                return true;
            }
            return base.AcceptOuterKey(sqlExpress);
        }

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            return true;
        }
    }
}
