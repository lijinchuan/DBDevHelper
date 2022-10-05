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
        private readonly HashSet<string> keys = new HashSet<string> { keyInsert, keyValues, keyTable, keyInto, keySelect, keyDistinct,keyAll, keyTop, keyFrom, keyAs, keyWhere, keyBetween, keyLike, keyAnd, keyIn, keyLeft, keyRight, keyInner, keyFull, keyJoin, keyOn, keyGroup, keyOrder, keyBy,keyAsc,keyDesc, keyWith, keyNolock };

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
            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyInto)
                {
                    sqlExpress.AnalyseType = AnalyseType.Table;
                    tables.Add(sqlExpress.Val);
                }
                if (lastKey == keySelect || (lastKey == keyDistinct||lastKey==keyAll) || lastKey == keyTop)
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
                else if (lastKey == keyAs && preExpress.ExpressType == SqlExpressType.Comma && PreAcceptKeysNot(acceptKeys, 1, new HashSet<string> { keyAs, keyDistinct,keyAll }) == keySelect)
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
            }

            return true;
        }

        protected override bool AcceptInnerKey(ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (sqlExpress.Val == keySelect)
            {
                if (isAcceptSelect|| lastKey == keyValues)
                {
                    return false;
                }
                isAcceptSelect = true;
            }

            return true;
        }

        protected override bool AcceptDeeper(ISqlExpress sqlExpress,bool isKey)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            if (lastKey == keyInsert || (lastLastKey == keyInsert && lastKey == keyInto))
            {
                sqlExpress.AnalyseType = AnalyseType.Column;
                colums.Add(sqlExpress.Val);
                return true;
            }
            return base.AcceptDeeper(sqlExpress,isKey);
        }
    }
}
