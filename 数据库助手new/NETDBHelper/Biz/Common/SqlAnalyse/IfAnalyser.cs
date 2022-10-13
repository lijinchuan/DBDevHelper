using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class IfAnalyser : SqlAnalyser
    {
        private static HashSet<string> keys = new HashSet<string> { keyIf, keyElse, keyBegin, keyEnd };
        private static Stack<string> keyBegins = new Stack<string>();
        
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyIf;
        }

        protected override bool AcceptOuterKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastKey == keyElse)
            {
                return true;
            }
            return base.AcceptOuterKey(sqlProcessor,sqlExpress);
        }

        protected override bool Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            return true;
        }

        protected override bool AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            if (sqlExpress.Val == keyEnd)
            {
                if (keyBegins.Count == 0)
                {
                    return false;
                }
                else
                {
                    keyBegins.Pop();
                }
            }
            else if (sqlExpress.Val == keyBegin)
            {
                keyBegins.Push(keyBegin);
            }
            return base.AcceptInnerKey(sqlProcessor,sqlExpress);
        }
    }
}
