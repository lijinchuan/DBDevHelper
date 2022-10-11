using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SelectAnalyser : SqlAnalyser
    {

        private readonly HashSet<string> keys = new HashSet<string> { keySelect, keyDistinct, keyAll, keyCount, keyTop, keyInto, keyFrom, keyAs, keyWhere, keyBetween, keyLike, keyAnd, keyOr, keyIn, keyLeft, keyRight, keyInner, keyFull, keyJoin, keyOn, keyGroup, keyOrder, keyBy, keyHaving, keyAsc, keyDesc, keyWith, keyNolock, keyCase, keyWhen, keyThen, keyElse, keyEnd };

        public SelectAnalyser()
        {
        }

        public override string GetPrimaryKey()
        {
            return keySelect;
        }

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        protected override bool AcceptDeeper(ISqlExpress sqlExpress,bool isOuterkey)
        {
            if (!isOuterkey)
            {
                var lastLastKey = PreAcceptKeys(acceptKeys, 1);
                var lastKey = PreAcceptKeys(acceptKeys, 0);
                var isInnerKey= keys.Contains(sqlExpress.Val);
                if ((lastLastKey == keyCount && lastKey == keyDistinct) || lastKey == keyCount)
                {
                    if (sqlExpress.Val == keyDistinct)
                    {
                        sqlExpress.AnalyseType = AnalyseType.Key;
                    }
                    else if (!isInnerKey && sqlExpress.ExpressType == SqlExpressType.Token)
                    {
                        sqlExpress.AnalyseType = AnalyseType.Column;
                        colums.Add(sqlExpress);
                    }

                    return true;
                }
                //else if (PreAcceptKeysNot(acceptKeys, 0, new HashSet<string> { keyAnd, keyOr, keyNot }) == keyWhere && sqlExpress.Deep > Deep)
                //{
                //    if (!isInnerKey && sqlExpress.ExpressType == SqlExpressType.Token)
                //    {
                //        sqlExpress.AnalyseType = AnalyseType.Column;
                //        colums.Add(sqlExpress);
                //    }
                //    return true;
                //}
            }
            
            return base.AcceptDeeper(sqlExpress,isOuterkey);
        }

        protected override bool Accept(ISqlExpress sqlExpress)
        {
            var lastLastKey = PreAcceptKeys(acceptKeys, 1);
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            var preExpress = PreAcceptExpress(acceptedSqlExpresses, 0);
            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keyAs && preExpress?.AnalyseType == AnalyseType.Column)
                {
                    //别名
                    sqlExpress.AnalyseType = AnalyseType.ColumnAlas;
                }
                else if (lastKey == keySelect || (lastKey == keyDistinct || lastKey == keyAll) || lastKey == keyTop)
                {
                    if (preExpress?.AnalyseType == AnalyseType.Column)
                    {
                        //别名
                        sqlExpress.AnalyseType = AnalyseType.ColumnAlas;
                        sqlExpress.Tag = preExpress;
                    }
                    else
                    {
                        sqlExpress.AnalyseType = AnalyseType.Column;
                        colums.Add(sqlExpress);
                    }
                }
                else if (lastKey == keyAs && preExpress?.ExpressType == SqlExpressType.Comma && PreAcceptKeysNot(acceptKeys, 1, new HashSet<string> { keyAs, keyDistinct, keyAll }) == keySelect)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
                else if ((PreAcceptKeysNot(acceptKeys, 1, new HashSet<string> { keyAnd, keyOr }) == keyWhere && (lastKey == keyAnd || lastKey == keyOr)) || lastKey == keyWhere)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
                else if (lastLastKey == keyGroup && lastKey == keyBy)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
                else if ((lastLastKey == keyOrder && lastKey == keyBy) || lastKey == keyDesc || lastKey == keyAsc)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
                else if (PreAcceptKeysNot(acceptKeys, 1, new HashSet<string> { keyOn, keyAnd, keyOr }) == keyJoin && (lastKey == keyOn || lastKey == keyAnd || lastKey == keyOr))
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
                else if (lastKey == keyCase || lastKey == keyWhen || lastKey == keyThen || lastKey == keyElse)
                {
                    sqlExpress.AnalyseType = AnalyseType.Column;
                    colums.Add(sqlExpress);
                }
                else if (lastKey == keyAs && (preExpress?.AnalyseType == AnalyseType.Table || lastLastKey == keyFrom || lastLastKey == keyJoin))
                {
                    sqlExpress.AnalyseType = AnalyseType.TableAlias;
                    if (preExpress?.AnalyseType == AnalyseType.Table)
                    {
                        sqlExpress.Tag = preExpress;
                    }
                    aliasTables.Add(sqlExpress);
                }
                else if (lastKey == keyFrom || lastKey == keyJoin)
                {
                    if (preExpress?.AnalyseType == AnalyseType.Table)
                    {
                        sqlExpress.AnalyseType = AnalyseType.TableAlias;
                        sqlExpress.Tag = preExpress;
                        aliasTables.Add(sqlExpress);
                    }
                    else
                    {
                        sqlExpress.AnalyseType = AnalyseType.Table;
                        tables.Add(sqlExpress);
                    }
                }
            }

            return true;
        }
    }
}
