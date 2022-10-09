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
        public static readonly string keyDatabase = "database";
        public static readonly string keyTable = "table";
        public static readonly string keyIndex = "index";
        public static readonly string keyView = "view";
        public static readonly string keyUnique = "unique";
        public static readonly string keyClustered = "clustered";
        public static readonly string keyNonclustered = "nonclustered";
        public static readonly string keyInto = "into";

        

        public static readonly string keySelect = "select";
        public static readonly string keyDistinct = "distinct";
        public static readonly string keyAll = "all";
        public static readonly string keyFrom = "from";
        public static readonly string keyAs = "as";
        public static readonly string keyWhere = "where";
        public static readonly string keyTop = "top";
        public static readonly string keyJoin = "join";
        public static readonly string keyJoinOn = "on";
        public static readonly string keyCount = "count";

        public static readonly string keyTruncate = "truncate";

        public static readonly string keyInsert = "insert";
        public static readonly string keyValues = "values";

        public static readonly string keyBetween = "between";
        public static readonly string keyLike = "like";
        public static readonly string keyAnd = "and";
        public static readonly string keyOr = "or";

        public static readonly string keyIn = "in";
        public static readonly string keyLeft = "left";
        public static readonly string keyRight = "right";
        public static readonly string keyInner = "inner";
        public static readonly string keyFull = "full";
        public static readonly string keyOn = "on";
        public static readonly string keyGroup = "group";
        public static readonly string keyOrder = "order";
        public static readonly string keyBy = "by";
        public static readonly string keyHaving = "having";
        public static readonly string keyAsc = "asc";
        public static readonly string keyDesc = "desc";
        public static readonly string keyWith = "with";
        public static readonly string keyNolock = "nolock";

        public static readonly string keyUpdate = "update";
        public static readonly string keySet = "set";

        public static readonly string keyExec = "exec";
        public static readonly string keyExecute = "execute";
        public static readonly string keyOutput = "output";
        public static readonly string keyDelete = "delete";

        public static readonly string keyNull = "null";

        public static readonly string keyCreate = "create";

        public static readonly string keyChar = "char";
        public static readonly string keyNChar= "nchar";
        public static readonly string keyVarChar = "varchar";
        public static readonly string keyNVarChar = "nvarchar";
        public static readonly string keyInt = "int";
        public static readonly string keyNumeric = "numeric";
        public static readonly string keyBigint = "bigint";
        public static readonly string keyBinary = "binary";
        public static readonly string keyBit = "bit";
        public static readonly string keyDate = "date";
        public static readonly string keyDatetime = "datetime";
        public static readonly string keyDatetime2 = "datetime2";
        public static readonly string keyDatetimeoffset = "datetimeoffset";
        public static readonly string keyDecimal="decimal";
        public static readonly string keyFloat = "float";
        public static readonly string keyGeography = "geography";
        public static readonly string keyGeometry = "geometry";
        public static readonly string keyHierarchyid = "hierarchyid";
        public static readonly string keyImage = "image";
        public static readonly string keyMoney = "money";
        public static readonly string keyNtext = "ntext";
        public static readonly string keyReal = "real";
        public static readonly string keySmalldatetime = "smalldatetime";
        public static readonly string keySmallint = "smallint";
        public static readonly string keySmallmoney = "smallmoney";
        public static readonly string keySql_variant = "sql_variant";
        public static readonly string keyText = "text";
        public static readonly string keyTime = "time";
        public static readonly string keyTimestamp = "timestamp";
        public static readonly string keyTinyint = "tinyint";
        public static readonly string keyUniqueidentifier = "uniqueidentifier";
        public static readonly string keyVarbinary = "varbinary";
        public static readonly string keyXml = "xml";

        public static readonly string keyDefault = "default";

        public static readonly string keyAlter = "alter";
        public static readonly string keyDrop = "drop";
        public static readonly string keyColumn = "column";
        /// <summary>
        /// 约束
        /// </summary>
        public static readonly string keyConstraint = "constraint";
        public static readonly string keyModify = "modify";

        public static readonly string keyFileName = "filename";

        public static readonly string keyLog = "log";

        public static readonly string keyCase = "case";
        public static readonly string keyWhen = "when";
        public static readonly string keyThen = "then";
        public static readonly string keyElse = "else";
        public static readonly string keyEnd = "end";

        protected static readonly HashSet<string> commonKeys = new HashSet<string> { keyNull, keyChar, keyNChar, keyVarChar, keyNVarChar, keyInt, keyNumeric, keyBigint, keyBinary, keyBit, keyDate, keyDatetime, keyDatetime2, keyDatetimeoffset, keyDecimal, keyFloat, keyGeography, keyGeometry, keyHierarchyid, keyImage, keyMoney, keyNtext, keyReal, keySmalldatetime, keySmallint, keySmallmoney, keySql_variant, keyText, keyTime, keyTimestamp, keyTinyint, keyUniqueidentifier, keyVarbinary, keyXml };

        //https://web.baimiaoapp.com/
        protected static readonly HashSet<string> functions = new HashSet<string>
        {
            "avg","binary_checksum","checksum","checksum_agg","count","count_big","grouping","grouping_id","max","min","stdev","stdevp","sum","var","varp",
            "connectionproperty",
            "current_timestamp","oppeated","datediff","datename","datepart","day","getdate","getutcdate","isdate","month","sysdatetime","sysdatetimeoffset","sysutcdatetime","switchoffset","todatetimeoffset","year",
            "abs","acos","asin","atan","atn2","ceiling","cos","cot","degrees","exp","floor","log","log10","pi","power","radians","rand","round","sign","sin","sqrt","square","tan",
            "col_length","col_name","columnproperty","databaseproperty","databasepropertyex","opiqa","db_name","file_id","file_name","filegroup_id","filegroup_name","filegroupproperty","fileproperty","::fn_listextendedproperty","fulltextcatalogproperty","fulltextserviceproperty","index_col","indexkey_property","indexproperty","object_ld","object_name","objectproperty","objectpropertyex","@@procid","sql_variant_property","typeproperty","change_tracking_current_version","change_tracking_ls_column_in_mask","change_tracking_cleanup_version",
            "app_name","cast","coalesce","collationproperty","columns_updated","convert","current_user", "datalength","fn_helpcollations","fn_indexinfo","fn_servershareddrives","::fn_virtualservernodes","formatmessage","getansinullo", "host_id","host_name","ident_current","ident_incr","ident_seed","identity","isnullo","isnumeric","newid","nullifo","parsename","permissions","rowcount_big","scope_identity","serverproperty","sessionproperty","session_user","stats_date","system_user","update","user_name",
            "ascii","char","charindex","difference","left","len","lower","ltrim","nchar","patindex","quotename","replace","replicate","reverse","right","rtrim","soundex","space","str","stuff","substring","unicode","upper",
            "patindex","textptr","textvalid",
            "raiserror"
        };

        protected string lastError = string.Empty;
        protected readonly HashSet<ISqlExpress> tables = new HashSet<ISqlExpress>();
        protected readonly HashSet<ISqlExpress> aliasTables = new HashSet<ISqlExpress>();
        protected readonly HashSet<ISqlExpress> colums = new HashSet<ISqlExpress>();

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

        protected bool IsKey(string token)
        {
            return commonKeys.Contains(token) || GetKeys().Contains(token);
        }

        public List<ISqlAnalyser> NestAnalyser
        {
            get;
            set;
        } = new List<ISqlAnalyser>();

        protected virtual bool AcceptDeeper(ISqlExpress sqlExpress,bool isKey)
        {
            return false;
        }

        protected abstract bool Accept(ISqlExpress sqlExpress);

        /// <summary>
        /// 是否解受内部key，一般true，比如insert不能同时有values和select关键字
        /// </summary>
        /// <param name="sqlExpress"></param>
        /// <returns></returns>
        protected virtual bool AcceptInnerKey(ISqlExpress sqlExpress)
        {
            return true;
        }

        /// <summary>
        /// 是否接受外部key，一般false
        /// </summary>
        /// <param name="sqlExpress"></param>
        /// <returns></returns>
        protected virtual bool AcceptOuterKey(ISqlExpress sqlExpress)
        {
            return false;
        }

        public virtual bool Accept(ISqlExpress sqlExpress, bool isKey)
        {
            var primaryKey = GetPrimaryKey();
            var isInKeys = IsKey(sqlExpress.Val);
            if (sqlExpress.Deep != Deep)
            {
                if (AcceptDeeper(sqlExpress,isInKeys))
                {
                    if (isInKeys)
                    {
                        sqlExpress.AnalyseType = AnalyseType.Key;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //分隔多个SELECT
            if (sqlExpress.Val == primaryKey && isAcceptPrimaryKey && !AcceptOuterKey(sqlExpress))
            {
                return false;
            }

            var isAccept = false;

            if (!isKey || isInKeys)
            {
                if (sqlExpress.ExpressType != SqlExpressType.Annotation)
                {
                    if (isInKeys)
                    {
                        sqlExpress.AnalyseType = AnalyseType.Key;
                        if (AcceptInnerKey(sqlExpress))
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
            else if (isKey && AcceptOuterKey(sqlExpress))
            {
                acceptKeys.Add(sqlExpress.Val);
                AcceptedSqlExpresses.Add(sqlExpress);
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
                Trace.WriteLine("tables:" + string.Join("、", tables.Select(p => p.Val)) + ",columns:" + string.Join("、", colums.Select(p => p.Val)) + ",aliastable:" + string.Join("、", aliasTables.Select(p => p.Val)));

                if (NestAnalyser != null)
                {
                    foreach (var nest in NestAnalyser)
                    {
                        nest.Print(sql);
                    }
                }
            }
        }

        public HashSet<ISqlExpress> GetTables()
        {
            return tables;
        }

        public HashSet<ISqlExpress> GetColumns()
        {
            return colums;
        }

        public List<string> FindTables(ISqlExpress sqlExpress)
        {
            List<string> ret = new List<string>();


            return ret;
        }

        public ISqlExpress FindByPos(int pos)
        {
            if (!AcceptedSqlExpresses.Any() || AcceptedSqlExpresses.First().StartIndex > pos || AcceptedSqlExpresses.Last().EndIndex < pos)
            {
                return null;
            }

            foreach(var sqlexpress in NestAnalyser)
            {
                var item = sqlexpress.FindByPos(pos);
                if (item != null)
                {
                    return item;
                }
            }

            return AcceptedSqlExpresses.FirstOrDefault(p => p.StartIndex <= pos && p.EndIndex >= pos);
        }

        public HashSet<ISqlExpress> GetAliasTables()
        {
            return aliasTables;
        }
    }
}
