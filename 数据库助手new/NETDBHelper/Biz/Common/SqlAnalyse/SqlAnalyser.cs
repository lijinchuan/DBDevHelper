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
        public static readonly string keyCount = "count";

        public static readonly string keyTruncate = "truncate";

        public static readonly string keyInsert = "insert";
        public static readonly string keyValues = "values";

        public static readonly string keyBetween = "between";
        public static readonly string keyLike = "like";
        public static readonly string keyAnd = "and";
        public static readonly string keyNot = "not";
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

        public static readonly string keyIf = "if";

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

        protected readonly List<ISqlExpress> acceptedSqlExpresses = new List<ISqlExpress>();

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
        public ISqlAnalyser ParentAnalyser
        {
            get;
            set;
        }

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
                        AddAcceptKey(sqlExpress.Val);
                    }
                    AddAcceptSqlExpress(sqlExpress);
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
                            AddAcceptKey(sqlExpress.Val);
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
                        if (functions.Contains(sqlExpress.Val))
                        {
                            sqlExpress.AnalyseType = AnalyseType.Function;
                            sqlExpress.ExpressType = SqlExpressType.Function;
                        }

                        if (!Accept(sqlExpress))
                        {
                            return false;
                        }

                    }
                    AddAcceptSqlExpress(sqlExpress);
                }

                isAccept = true;
            }
            else if (isKey && AcceptOuterKey(sqlExpress))
            {
                AddAcceptKey(sqlExpress.Val);
                AddAcceptSqlExpress(sqlExpress);
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
            for(var i = acceptedSqlExpresses.Count - 1; i >= 0; i--)
            {
                var current = acceptedSqlExpresses[i];
                if (current.AnalyseType == AnalyseType.Key)
                {
                    continue;
                }
                if (preIndex == 0)
                {
                    return current;
                }
                preIndex--;
            }

            return null;
        }

        public virtual void Print(string sql)
        {
            if (acceptedSqlExpresses.Any())
            {
                var perfx = "|-";
                for (var i = 0; i < this.Deep; i++)
                {
                    perfx += "-";
                }
                var start = acceptedSqlExpresses.First().StartIndex;
                var end = acceptedSqlExpresses.Last().EndIndex;

                Trace.WriteLine(perfx + sql.Substring(start, end - start + 1));
                Trace.WriteLine(GetStartPos() + "->" + GetEndPos() + "," + "tables:" + string.Join("、", tables.Select(p => p.Val)) + ",columns:" + string.Join("、", colums.Select(p => p.Val)) + ",aliastable:" + string.Join("、", aliasTables.Select(p => p.Val)));

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

        private ISqlAnalyser FindAnalyser(ISqlExpress sqlExpress)
        {
            foreach(var nest in NestAnalyser)
            {
                var analyser = (nest as SqlAnalyser).FindAnalyser(sqlExpress);
                if (analyser != null)
                {
                    return analyser;
                }
            }

            if( GetStartPos() <= sqlExpress.StartIndex && GetEndPos() >= sqlExpress.EndIndex)
            {
                return this;
            }

            return null;
        }

        public List<string> FindTables(ISqlExpress sqlExpress)
        {
            List<string> ret = new List<string>();

            if (sqlExpress.AnalyseType == AnalyseType.Column || sqlExpress.AnalyseType == AnalyseType.ColumnAlas)
            {
                var val = sqlExpress.Val;
                var colname = val;
                var tbname = string.Empty;
                var colnames=val.Split('.');
                if (colnames.Length > 1)
                {
                    tbname = colnames[colnames.Length - 2];
                }
                colname = colnames.Last();

                var analyser = FindAnalyser(sqlExpress);
                if (analyser == null)
                {
                    
                    return ret;
                }

                if (!string.IsNullOrWhiteSpace(tbname))
                {
                    var aliasTable = analyser.GetAliasTables().FirstOrDefault(p => p.Val == tbname);
                    if (aliasTable != null)
                    {
                        if (aliasTable.Tag is ISqlExpress && (aliasTable.Tag as ISqlExpress).AnalyseType == AnalyseType.Table)
                        {
                            tbname = (aliasTable.Tag as ISqlExpress).Val;
                        }
                        else
                        {
                            //
                        }
                    }
                    else
                    {
                        ret.Add(tbname);
                    }
                }
                else
                {
                    ret.AddRange(analyser.GetTables().Select(p => p.Val));
                }
            }

            return ret;
        }

        public ISqlExpress FindByPos(int pos)
        {
            if (!acceptedSqlExpresses.Any() || acceptedSqlExpresses.First().StartIndex > pos || acceptedSqlExpresses.Last().EndIndex < pos)
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

            return acceptedSqlExpresses.FirstOrDefault(p => p.StartIndex <= pos && p.EndIndex >= pos);
        }

        public HashSet<ISqlExpress> GetAliasTables()
        {
            return aliasTables;
        }

        public virtual void AddAcceptKey(string key)
        {
            acceptKeys.Add(key);
        }

        public virtual void AddAcceptSqlExpress(ISqlExpress sqlExpress)
        {
            acceptedSqlExpresses.Add(sqlExpress);
        }

        public int GetStartPos()
        {
            return acceptedSqlExpresses.FirstOrDefault()?.StartIndex??0;
        }

        public int GetEndPos()
        {
            return acceptedSqlExpresses.LastOrDefault()?.EndIndex ?? 0;
        }
    }
}
