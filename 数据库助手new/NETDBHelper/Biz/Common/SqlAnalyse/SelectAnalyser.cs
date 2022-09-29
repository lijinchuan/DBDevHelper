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

        private ISqlExpress preExpress = null;

        private string lastError = string.Empty;
        private HashSet<string> tables = new HashSet<string>();
        private HashSet<string> colums = new HashSet<string>();

        private HashSet<string> keys = new HashSet<string> { "select", "distinct", "top", "into", "from", "as", "where", "left", "right", "full", "join", "on", "group", "order", "by", "with", "nolock" };
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
            var isAccept = false;

            if (sqlExpress.ExpressType != SqlExpressType.Annotation)
            {
                if (keys.Contains(sqlExpress.Val))
                {
                    acceptKeys.Add(sqlExpress.Val);
                    sqlExpress.AnalyseType = AnalyseType.Key;
                    isAccept = true;
                }
                else if (sqlExpress.AnalyseType != AnalyseType.Key)
                {
                    var lastKey = PreAcceptKeys(0);
                    var lastExpress = PreAcceptExpress(0);
                    //if (lastExpress?.AnalyseType != AnalyseType.Key)
                    {
                        if (lastKey == keySelect || lastKey == keyDistinct || lastKey == keyTop)
                        {
                            if (sqlExpress.ExpressType == SqlExpressType.Token)
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
                        }
                        else if (lastKey == keyFrom || lastKey == keyJoin)
                        {
                            if (sqlExpress.ExpressType == SqlExpressType.Token)
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
                    isAccept = true;
                }
            }

            if (isAccept && sqlExpress.ExpressType != SqlExpressType.Annotation)
            {
                AcceptedSqlExpresses.Add(sqlExpress);
                preExpress = sqlExpress;
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
