using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class AlterAnalyser : SqlAnalyser
    {
        private HashSet<string> keys = new HashSet<string> { keyAlter, keyTable, keyDrop, keyColumn, keyConstraint, keyModify };

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyAlter;
        }

        protected override AnalyseAccept Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);

            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyTable)
                {
                    sqlExpress.AnalyseType = AnalyseType.Table;
                    tables.Add(sqlExpress);
                }
                else if (lastKey == keyColumn || lastKey == keyModify)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
            }

            return AnalyseAccept.Accept;
        }

        protected override AnalyseAccept AcceptInnerKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (sqlExpress.Val == keyDrop)
            {
                if (lastLastKey == keyAlter && lastKey == keyTable)
                {
                    return AnalyseAccept.Accept;
                }
                else
                {
                    return AnalyseAccept.Reject;
                }
            }
            return base.AcceptInnerKey(sqlProcessor, sqlExpress);
        }

        protected override AnalyseAccept AcceptOuterKey(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastLastKey == keyAlter && lastKey == keyTable)
            {
                return AnalyseAccept.Accept;
            }
            return base.AcceptOuterKey(sqlProcessor,sqlExpress);
        }
    }
}
