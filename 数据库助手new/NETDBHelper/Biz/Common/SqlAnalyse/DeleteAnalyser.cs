using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class DeleteAnalyser : SqlAnalyser
    {

        private HashSet<string> keys = new HashSet<string> { keyDelete, keyFrom, keyWhere };
        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyDelete;
        }

        protected override bool Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            var preExpress = PreAcceptExpress(acceptedSqlExpresses, 0);
            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyDelete || lastKey == keyFrom)
                {
                    tables.Add(sqlExpress);
                    sqlExpress.AnalyseType = AnalyseType.Table;
                }
            }

            return true;
        }
    }
}
