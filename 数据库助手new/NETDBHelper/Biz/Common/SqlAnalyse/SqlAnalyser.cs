using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public abstract class SqlAnalyser : ISqlAnalyser
    {
        public static readonly string keyTable = "table";
        public static readonly string keyInto = "into";

        public static readonly string keySelect = "select";
        public static readonly string keyDistinct = "distinct";
        public static readonly string keyFrom = "from";
        public static readonly string keyAs = "as";
        public static readonly string keyWhere = "where";
        public static readonly string keyTop = "top";
        public static readonly string keyJoin = "join";
        public static readonly string keyJoinOn = "on";

        public static readonly string keyTruncate = "truncate";

        public static readonly string keyInsert = "insert";

        public static readonly string keyBetween = "between";
        public static readonly string keyLike = "like";
        public static readonly string keyAnd = "and";

        public static readonly string keyIn = "in";
        public static readonly string keyLeft = "left";
        public static readonly string keyRight = "right";
        public static readonly string keyInner = "inner";
        public static readonly string keyFull = "full";
        public static readonly string keyOn = "on";
        public static readonly string keyGroup = "group";
        public static readonly string keyOrder = "order";
        public static readonly string keyBy = "by";
        public static readonly string keyWith = "with";
        public static readonly string keyNolock = "nolock";

        public static readonly string keyUpdate = "update";
        public static readonly string keySet = "set";

        public static readonly string keyExec = "exec";

        public static readonly string keyDelete = "delete";


        protected string lastError = string.Empty;
        protected readonly HashSet<string> tables = new HashSet<string>();
        protected readonly HashSet<string> colums = new HashSet<string>();

        protected readonly List<string> acceptKeys = new List<string>();

        protected readonly List<ISqlExpress> AcceptedSqlExpresses = new List<ISqlExpress>();

        public int Deep
        {
            get;
            set;
        }

        public abstract string GetPrimaryKey();

        public abstract HashSet<string> GetKeys();

        private bool isAcceptPrimaryKey = false;


        public List<ISqlAnalyser> NestAnalyser
        {
            get;
            set;
        } = new List<ISqlAnalyser>();

        protected abstract bool Accept(ISqlExpress sqlExpress);

        protected abstract bool AcceptKey(ISqlExpress sqlExpress);

        public virtual bool Accept(ISqlExpress sqlExpress, bool isKey)
        {
            var primaryKey = GetPrimaryKey();
            if (sqlExpress.Deep != this.Deep)
            {
                return false;
            }

            //分隔多个SELECT
            if (sqlExpress.Val == primaryKey && isAcceptPrimaryKey)
            {
                return false;
            }

            var keys = GetKeys();
            var isAccept = false;

            if (sqlExpress.AnalyseType != AnalyseType.Key || keys.Contains(sqlExpress.Val))
            {
                if (sqlExpress.ExpressType != SqlExpressType.Annotation)
                {
                    if (keys.Contains(sqlExpress.Val))
                    {
                        sqlExpress.AnalyseType = AnalyseType.Key;
                        if (AcceptKey(sqlExpress))
                        {
                            acceptKeys.Add(sqlExpress.Val);
                            if (sqlExpress.Val == primaryKey)
                            {
                                isAcceptPrimaryKey = true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!Accept(sqlExpress))
                        {
                            return false;
                        }

                    }
                    AcceptedSqlExpresses.Add(sqlExpress);
                }

                isAccept = true;
            }

            return isAccept;

        }

        protected string PreAcceptKeys(List<string> acceptKeys, int preIndex)
        {
            var idx = acceptKeys.Count - 1 - preIndex;
            if (idx < 0)
            {
                return null;
            }

            return acceptKeys[idx];
        }

        protected string PreAcceptKeysNot(List<string> acceptKeys, int preIndex, HashSet<string> keysIgnore)
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

        protected ISqlExpress PreAcceptExpress(List<ISqlExpress> acceptedSqlExpresses, int preIndex)
        {
            var idx = acceptedSqlExpresses.Count - 1 - preIndex;
            if (idx < 0)
            {
                return null;
            }
            return acceptedSqlExpresses[idx];
        }

        public virtual void Print(string sql)
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
