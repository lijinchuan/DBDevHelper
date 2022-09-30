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
        private readonly HashSet<string> keys = new HashSet<string> { keyInsert, keyTable, keyInto, keySelect, keyDistinct, keyTop, keyFrom, keyAs, keyWhere, keyBetween, keyLike, keyAnd, keyIn, keyLeft, keyRight, keyInner, keyFull, keyJoin, keyOn, keyGroup, keyOrder, keyBy, keyWith, keyNolock };

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyInsert;
        }

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            var preExpress = PreAcceptExpress(AcceptedSqlExpresses, 0);
            if (lastKey == keyInto)
            {
                sqlExpress.AnalyseType = AnalyseType.Table;
                tables.Add(sqlExpress.Val);
            }
            if (lastKey == keySelect || lastKey == keyDistinct || lastKey == keyTop)
            {
                if (preExpress.AnalyseType == AnalyseType.Column || preExpress.Val == keyAs)
                {
                    //别名
                    sqlExpress.AnalyseType = AnalyseType.ColumnAlas;
                }
                else
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress.Val);
                }
            }
            else if (lastKey == keyAs && preExpress.ExpressType == SqlExpressType.Comma && PreAcceptKeysNot(acceptKeys, 1, new HashSet<string> { keyAs, keyDistinct }) == keySelect)
            {
                sqlExpress.AnalyseType = AnalyseType.Column;
                colums.Add(sqlExpress.Val);
            }
            else if (lastKey == keyFrom || lastKey == keyJoin)
            {
                if (preExpress.AnalyseType == AnalyseType.Table || preExpress.Val == keyAs)
                {
                    sqlExpress.AnalyseType = AnalyseType.TableAlias;
                }
                else
                {
                    sqlExpress.AnalyseType = AnalyseType.Table;
                    tables.Add(sqlExpress.Val);
                }
            }

            return true;
        }
    }
}
