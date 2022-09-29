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
        private string keySelect = "select";
        private string keyDistinct = "distinct";
        private string keyFrom = "from";
        private string keyInto = "into";
        private string keyAs = "as";
        private string keyWhere = "where";
        private string keyTop = "top";
        private string keyJoin = "join";
        private string keyJoinOn = "on";

        private bool isAcceptSelect = false;

        private string lastError = string.Empty;
        private HashSet<string> tables = new HashSet<string>();
        private HashSet<string> colums = new HashSet<string>();

        private HashSet<string> keys = new HashSet<string> { "select", "distinct", "top", "into", "from", "as", "where", "between", "like","and", "in", "left", "right", "inner", "full", "join", "on", "group", "order", "by", "with", "nolock" };
        private List<string> acceptKeys = new List<string>();

        private readonly List<ISqlExpress> AcceptedSqlExpresses = new List<ISqlExpress>();

        public SelectAnalyser()
        {
        }

        private string PreAcceptKeys(int preIndex)
        {
            var idx = acceptKeys.Count - 1 - preIndex;
            if (idx < 0)
            {
                return null;
            }

            return acceptKeys[idx];
        }

        private string PreAcceptKeysNot(int preIndex, HashSet<string> keysIgnore)
        {
            var idx = acceptKeys.Count - 1 - preIndex;
            while (idx >= 0)
            {
                var key = acceptKeys[idx];
                if (!keysIgnore.Contains(key))
                {
                    return key;
                }
                idx--;
            }
            return null;
        }

        private ISqlExpress PreAcceptExpress(int preIndex)
        {
            var idx = AcceptedSqlExpresses.Count - 1 - preIndex;
            if (idx < 0)
            {
                return null;
            }
            return AcceptedSqlExpresses[idx];
        }

        public override bool Accept(ISqlExpress sqlExpress, bool isKey)
        {

            if (sqlExpress.Deep != this.Deep)
            {
                return false;
            }

            //分隔多个SELECT
            if (sqlExpress.Val == keySelect && isAcceptSelect)
            {
                return false;
            }

            var isAccept = false;

            if (sqlExpress.AnalyseType != AnalyseType.Key || keys.Contains(sqlExpress.Val))
            {
                if (sqlExpress.ExpressType != SqlExpressType.Annotation)
                {
                    if (keys.Contains(sqlExpress.Val))
                    {
                        acceptKeys.Add(sqlExpress.Val);
                        sqlExpress.AnalyseType = AnalyseType.Key;

                        if (sqlExpress.Val == keySelect)
                        {
                            isAcceptSelect = true;
                        }
                    }
                    else
                    {
                        var lastKey = PreAcceptKeys(0);
                        var preExpress = PreAcceptExpress(0);
                        if (sqlExpress.ExpressType == SqlExpressType.Token)
                        {
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
                            else if (lastKey == keyAs && preExpress.ExpressType == SqlExpressType.Comma && PreAcceptKeysNot(1, new HashSet<string> { keyAs, keyDistinct }) == keySelect)
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

                    }
                    AcceptedSqlExpresses.Add(sqlExpress);
                }

                isAccept = true;
            }

            return isAccept;
        }

        public override void Print(string sql)
        {
            if (AcceptedSqlExpresses.Any())
            {
                var perfx = "|-";
                for (var i = 0; i < this.Deep; i++)
                {
                    perfx += "-";
                }
                var start = AcceptedSqlExpresses.First().StartIndex;
                var end = AcceptedSqlExpresses.Last().EndIndex;

                Trace.WriteLine(perfx + sql.Substring(start, end - start + 1));
                Trace.WriteLine("tables:" + string.Join("、", tables) + ",columns:" + string.Join("、", colums));

                if (NestAnalyser != null)
                {
                    foreach (var nest in NestAnalyser)
                    {
                        nest.Print(sql);
                    }
                }
            }
        }
    }
}
