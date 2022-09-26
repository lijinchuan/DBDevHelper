using System;
using System.Collections.Generic;
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

        public SelectAnalyser()
        {
        }

        public override bool Accept(ISqlExpress sqlExpress, bool isKey)
        {
            if (sqlExpress.Deep != this.Deep)
            {
                return false;
            }
            if (keySelect.Equals(sqlExpress.Val))
            {
                if (!isAcceptSelect&&string.IsNullOrWhiteSpace(preExpress))
                {
                    isAcceptSelect = true;
                    preExpress = keySelect;
                    return true;
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
                    return true;
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
                    return true;
                }
                else
                {
                    lastError = $"more '{keyFrom}'";
                }
            }
            else if (keyJoin.Equals(sqlExpress.Val))
            {
                preExpress = keyJoin;
                return true;
            }
            else if (keyJoinOn.Equals(sqlExpress.Val))
            {
                preExpress = keyJoinOn;
                return true;
            }
            else if (keyWhere.Equals(sqlExpress.Val))
            {
                if (!isAcceptWhere)
                {
                    isAcceptWhere = true;
                    preExpress = keyWhere;
                    return true;
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
                    return true;
                }
            }

            return !isKey;
        }
    }
}
