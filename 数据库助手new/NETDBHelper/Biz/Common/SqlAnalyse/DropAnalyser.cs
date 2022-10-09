using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class DropAnalyser : SqlAnalyser
    {
        private static HashSet<string> keys = new HashSet<string> { keyDrop, keyDatabase, keyTable,keyIndex };
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyDrop;
        }

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastKey == keyTable)
            {
                sqlExpress.AnalyseType = AnalyseType.Table;
                tables.Add(sqlExpress);
            }
            return true;
        }
    }
}
