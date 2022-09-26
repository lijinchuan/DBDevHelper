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
        private string keyWhere = "where";
        private string keyTop = "top";
        private string keyJoin = "join";
        private string keyJoinOn = "on";

        private bool isAcceptSelect = false;
        private bool isAcceptFrom = false;
        private bool isAcceptWhere = false;
        private bool isAcceptTop = false;
        private string preExpress = string.Empty;

        private string lastError = string.Empty;
        private HashSet<string> acceptAndIgnore = new HashSet<string> { "distinct" };
        private HashSet<string> tables = new HashSet<string>();

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
                if (!isAcceptSelect && string.IsNullOrWhiteSpace(preExpress))
                {
                    isAcceptSelect = true;
                    preExpress = keySelect;
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
                    preExpress = keyTop;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyTop}'";
                }
            }
            else if (keyFrom.Equals(sqlExpress.Val))
            {
                if (!isAcceptFrom)
                {
                    isAcceptFrom = true;
                    preExpress = keyFrom;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyFrom}'";
                }
            }
            else if (keyJoin.Equals(sqlExpress.Val))
            {
                preExpress = keyJoin;
                isAccept = true;
            }
            else if (keyJoinOn.Equals(sqlExpress.Val))
            {
                preExpress = keyJoinOn;
                isAccept = true;
            }
            else if (keyWhere.Equals(sqlExpress.Val))
            {
                if (!isAcceptWhere)
                {
                    isAcceptWhere = true;
                    preExpress = keyWhere;
                    isAccept = true;
                }
                else
                {
                    lastError = $"more '{keyWhere}'";
                }
            }
            else
            {
                if (acceptAndIgnore.Contains(sqlExpress.Val))
                {
                    isAccept = true;
                }
                
            }

            isAccept = isAccept || !isKey;

            if (isAccept && sqlExpress.ExpressType != SqlExpressType.Annotation)
            {
                AcceptedSqlExpresses.Add(sqlExpress);
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
