using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class InsertAnalyser : SqlAnalyser
    {
        private bool isAcceptSelect = false;
        private readonly HashSet<string> keys = new HashSet<string> { keyInsert, keyValues, keyTable, keyInto, keySelect };

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyInsert;
        }

        protected override AnalyseAccept Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            //var preExpress = PreAcceptExpress(acceptedSqlExpresses, 0);
            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyInto)
                {
                    sqlExpress.AnalyseType = AnalyseType.Table;
                    tables.Add(sqlExpress);
                }
            }

            return AnalyseAccept.Accept;
        }

        protected override AnalyseAccept AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (sqlExpress.Val == keySelect)
            {
                if (isAcceptSelect|| lastKey == keyValues)
                {
                    return AnalyseAccept.Reject;
                }
                isAcceptSelect = true;
                return AnalyseAccept.AcceptDeeper;
            }

            return AnalyseAccept.Accept;
        }

        protected override AnalyseAccept AcceptDeeper(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress,bool isOuterkey)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastKey == keyInsert || (lastLastKey == keyInsert && lastKey == keyInto))
            {
                sqlExpress.AnalyseType = AnalyseType.Column;
                colums.Add(sqlExpress);
                return AnalyseAccept.Accept;
            }
            return base.AcceptDeeper(sqlProcessor,sqlExpress, isOuterkey);
        }
    }
}
