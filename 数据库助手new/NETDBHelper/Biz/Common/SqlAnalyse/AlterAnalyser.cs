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

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);

            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyTable)
                {
                    sqlExpress.AnalyseType = AnalyseType.Table;
                    tables.Add(sqlExpress.Val);
                }
                else if (lastKey == keyColumn || lastKey == keyModify)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress.Val);
                }
            }

            return true;
        }

        protected override bool AcceptInnerKey(ISqlExpress sqlExpress)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (sqlExpress.Val == keyDrop)
            {
                if (lastLastKey == keyAlter && lastKey == keyTable)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return base.AcceptInnerKey(sqlExpress);
        }

        protected override bool AcceptOuterKey(ISqlExpress sqlExpress)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastLastKey == keyAlter && lastKey == keyTable)
            {
                return true;
            }
            return base.AcceptOuterKey(sqlExpress);
        }
    }
}
