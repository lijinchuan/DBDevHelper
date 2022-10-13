using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class UpdateAnalyser : SqlAnalyser
    {
        private static readonly HashSet<string> keys = new HashSet<string> { keyUpdate, keySet, keyFrom, keyWhere, keyJoin, keyOn };

        public override HashSet<string> GetKeys()
        {
            return keys;
        }

        public override string GetPrimaryKey()
        {
            return keyUpdate;
        }

        protected override bool Accept(ISqlProcessor sqlProcessor, ISqlExpress sqlExpress)
        {
            var lastKey = PreAcceptKeys(acceptKeys, 0);
            var preExpress = PreAcceptExpress(acceptedSqlExpresses, 0);
            if (sqlExpress.ExpressType == SqlExpressType.Token)
            {
                if (lastKey == keySet)
                {
                    if (preExpress?.AnalyseType == AnalyseType.Column || preExpress?.Val == keyAs)
                    {
                        //别名
                        sqlExpress.AnalyseType = AnalyseType.ColumnAlas;
                    }
                    else
                    {
                        sqlExpress.AnalyseType = AnalyseType.Column;
                        colums.Add(sqlExpress);
                    }
                }
                else if (lastKey == keyFrom || lastKey == keyJoin || lastKey == keyUpdate)
                {
                    if (preExpress?.AnalyseType == AnalyseType.Table || preExpress?.Val == keyAs)
                    {
                        sqlExpress.AnalyseType = AnalyseType.TableAlias;
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
