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
        private string keyFrom = "from";
        private string keyInto = "into";
        private string keyAs = "as";
        private string keyWhere = "where";
        private string keyTop = "top";
        private string keyJoin = "join";
        private string keyJoinOn = "on";

        private bool isAcceptSelect = false;
        private bool isAcceptFrom = false;
        private bool isAcceptInto = false;
        private bool isAcceptJoin = false;
        private bool isExpectOn = false;
        private bool isAcceptWhere = false;
        private bool isAcceptTop = false;
        private bool isAcceptTopN = false;
        private ISqlExpress preExpress = null;

        private string lastError = string.Empty;
        private HashSet<string> acceptAndIgnore = new HashSet<string> { "distinct", "with", "nolock"};
        private HashSet<string> tables = new HashSet<string>();
        private HashSet<string> colums = new HashSet<string>();

        private readonly List<ISqlExpress> AcceptedSqlExpresses = new List<ISqlExpress>();

        public SelectAnalyser()
        {
        }

        public override bool Accept(ISqlExpress sqlExpress, bool isKey)
        {
            if (sqlExpress.Deep != this.Deep)
            {
                return false;
            }
            var isAccept = false;
            if (keySelect.Equals(sqlExpress.Val))
            {
                if (!isAcceptSelect && preExpress == null)
                {
                    isAcceptSelect = true;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keySelect}'";
                }
            }
            else if (keyTop.Equals(sqlExpress.Val))
            {
                if (!isAcceptTop)
                {
                    isAcceptTop = true;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyTop}'";
                }
            }
            else if (keyInto.Equals(sqlExpress.Val))
            {
                if (!isAcceptInto)
                {
                    isAcceptInto = true;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyInto}'";
                }
            }
            else if (keyFrom.Equals(sqlExpress.Val))
            {
                if (!isAcceptFrom)
                {
                    isAcceptFrom = true;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyFrom}'";
                }
            }
            else if (keyJoin.Equals(sqlExpress.Val))
            {
                isAcceptJoin = true;
                isExpectOn = true;
                isAccept = true;
            }
            else if (keyJoinOn.Equals(sqlExpress.Val))
            {
                isExpectOn = false;
                isAccept = true;
            }
            else if (keyAs.Equals(sqlExpress.Val))
            {
                isAccept = true;
            }
            else if (keyWhere.Equals(sqlExpress.Val))
            {
                if (!isAcceptWhere)
                {
                    isAcceptWhere = true;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyWhere}'";
                }
            }
            else if (sqlExpress.ExpressType != SqlExpressType.Annotation)
            {
                if (acceptAndIgnore.Contains(sqlExpress.Val))
                {
                    isAccept = true;
                }
                else if (!isKey)
                {
                    if (isAcceptSelect && !isAcceptInto && !isAcceptFrom)
                    {
                        if (isAcceptTop && !isAcceptTopN)
                        {
                            if (sqlExpress.ExpressType == SqlExpressType.Numric || sqlExpress.ExpressType == SqlExpressType.Var)
                            {
                                isAcceptTopN = true;
                                isAccept = true;
                            }

                        }
                        else if (!isAcceptTop || isAcceptTopN)
                        {
                            if (sqlExpress.ExpressType == SqlExpressType.Token)
                            {
                                if (preExpress.ExpressType == SqlExpressType.Comma || (preExpress.Val == keySelect || preExpress.ExpressType != SqlExpressType.Token))
                                {
                                    colums.Add(sqlExpress.Val);
                                    isAccept = true;
                                }
                                else if (preExpress.Val == keyAs || preExpress.ExpressType == SqlExpressType.Token)
                                {
                                    //别名
                                    isAccept = true;
                                }
                            }

                        }
                    }
                    else if (isAcceptJoin && isExpectOn)
                    {
                        if (preExpress.Val == keyJoin)
                        {
                            tables.Add(sqlExpress.Val);
                            isAccept = true;
                        }
                        else
                        {
                            //别名
                        }
                    }
                    else if (isAcceptFrom && (!isAcceptJoin || !isAcceptWhere))
                    {
                        if (preExpress.Val == keyFrom || preExpress.ExpressType == SqlExpressType.Comma)
                        {
                            tables.Add(sqlExpress.Val);
                            isAccept = true;
                        }
                        else
                        {
                            //别名
                            isAccept = true;
                        }
                    }
                }

            }

            isAccept = isAccept || !isKey;

            if (isAccept && sqlExpress.ExpressType != SqlExpressType.Annotation && !acceptAndIgnore.Contains(sqlExpress.Val))
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
