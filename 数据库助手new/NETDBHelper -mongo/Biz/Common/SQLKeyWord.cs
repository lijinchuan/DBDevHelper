using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Biz.Common
{
    public struct SQLKeyWord : IEqualityComparer<SQLKeyWord>
    {
        public string KeyWord
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public Color HighColor
        {
            get;
            internal set;
        }


        public bool Equals(SQLKeyWord x, SQLKeyWord y)
        {
            if (x.KeyWord == null && y.KeyWord == null)
            {
                return true;
            }

            if (x.KeyWord == null || y.KeyWord == null)
            {
                return false;
            }

            var ret = x.KeyWord.ToUpper() == y.KeyWord.ToUpper();
            if (ret)
            {
                if (string.IsNullOrEmpty(x.Desc))
                {
                    x.Desc = y.Desc;
                }
                else if (string.IsNullOrEmpty(y.Desc))
                {
                    y.Desc = x.Desc;
                }
            }
            return ret;
        }

        public int GetHashCode(SQLKeyWord obj)
        {
            if (obj.KeyWord == null)
            {
                return 0;
            }
            return obj.KeyWord.GetHashCode();
        }
    }

    public static class SQLKeyWordHelper
    {
        private static List<SQLKeyWord> KeyWordDic = new List<SQLKeyWord>();

        static SQLKeyWordHelper()
        {

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "*",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = ",",
                Desc = ""
            });
            
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "[",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "]",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "@@version",
                Desc = "版本",
                HighColor=Color.Pink
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "add",
                Desc = "添加"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "all",
                Desc = "所有,不去重,全"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "alter",
                Desc = "更改"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "and",
                Desc = "与,并且"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "ansi_nulls",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "as",
                Desc = "别名"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "asc",
                Desc = "正序"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "avg",
                Desc = "平均"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "begin",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "between",
                Desc = "范围"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "bigint",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "binary",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "bit",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "by",
                Desc = "根据"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "case",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "cast",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "char",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "char(n)",
                Desc = "字符"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "character",
                Desc = "字符集"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "charindex",
                Desc = "查找"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "close",
                Desc = "游标关闭"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "coalesce",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "column->>path",
                Desc = "json_unquote(column -> path)的简洁写法"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "column->path",
                Desc = "json_extract的简洁写法，MySQL 5.7.9开始支持"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "commit",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "read committed",
                Desc = "隔离级别:已提交读"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "continue",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "convert",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "count",
                Desc = "总计"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "create",
                Desc = "创建"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "cursor",
                Desc = "游标"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "date",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datename",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datepart",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datetime",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datetime2",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datetimeoffset",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "day",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "deallocate",
                Desc = "游标释放"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "decimal",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "declare",
                Desc = "申明"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "delete",
                Desc = "删除"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "desc",
                Desc = "倒序"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "difference",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "delimiter",
                Desc = "定界符"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "distinct",
                Desc = "去重"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "drop",
                Desc = "移除"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "else",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "end",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "except",
                Desc = "差集"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "exec",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "execute",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "exists",
                Desc = "存在"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "fetch",
                Desc = "游标获取"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "float",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "float(n)",
                Desc = "精度至少位n位的浮点数"
            });
            
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "for",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "foreign key(C) references T",
                Desc = "外键，括号中为外键，references后为外键的表"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "found",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "from",
                Desc = "从"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "getdate",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "group",
                Desc = "分组"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "handler",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "hash",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "having",
                Desc = "对group by产生的分组进行筛选，可以使用聚集函数"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "if",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "isolation",
                Desc = "隔离"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "inner",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "inner",
                Desc = "内联"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "insert",
                Desc = "插入"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "int",
                Desc = "整数类型"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "intersect",
                Desc = "相交"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "into",
                Desc = "到"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "is",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "isdate",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "isnull",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "join",
                Desc = "关联"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_append",
                Desc = "废弃，MySQL 5.7.9开始改名为json_array_append"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_array",
                Desc = "创建json数组"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_array_append",
                Desc = "末尾添加数组元素，如果原有值是数值或json对象，则转成数组后，再添加元素"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_array_insert",
                Desc = "插入数组元素"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_contains",
                Desc = "判断是否包含某个json值"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_contains_path",
                Desc = "判断某个路径下是否包json值"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_depth",
                Desc = "返回json文档的最大深度"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_extract",
                Desc = "提取json值"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_insert",
                Desc = "插入值（插入新值，但不替换已经存在的旧值）"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_keys",
                Desc = "提取json中的键值为json数组"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_length",
                Desc = "返回json文档的长度"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_merge",
                Desc = "合并json数组或对象"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_object",
                Desc = "创建json对象"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_quote",
                Desc = "将json转成json字符串类型"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_remove",
                Desc = "删除json数据"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_replace",
                Desc = "替换值（只替换已经存在的旧值）"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_search",
                Desc = "按给定字符串关键字搜索json，返回匹配的路径"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_set",
                Desc = "设置值（替换旧值，并插入不存在的新值）"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_type",
                Desc = "返回json值得类型"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_unquote",
                Desc = "去除json字符串的引号，将值转成string类型"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "json_valid",
                Desc = "判断是否为合法json文档"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "leave",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "left",
                Desc = "左联"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "len",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "level",
                Desc = "级别"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "like",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "limit",
                Desc = "分页"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "loop",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "lower",
                Desc = "小写"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "ltrim",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "max",
                Desc = "最大"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "mid",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "min",
                Desc = "最小"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "money",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "month",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nchar",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nocount",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nolock",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "not",
                Desc = "非,不"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "ntext",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "null",
                Desc = "空"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "numeric",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "numeric(p,d)",
                Desc = "定点数，精度由用户指定"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nvarchar",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "on",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "open",
                Desc = "游标打开"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "option",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "or",
                Desc = "或者"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "order",
                Desc = "排序"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "output",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "parsename",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "patindex",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "primary key(KEY)",
                Desc = "主键,后面括号中是作为主键的属性"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "print",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "proc",
                Desc = "存储过程"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "procedure",
                Desc = "存储过程"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "quoted_identifier",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "quotename",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "read",
                Desc = "读"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "real",
                Desc = "浮点数和双精度浮点数"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "recompile",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "repeat",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "repeatable read",
                Desc = "隔离级别:可重复读"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "replace",
                Desc = "替换"
            });
            
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "replicate",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "return",
                Desc = "返回"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "reverse",
                Desc = "反转"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "right",
                Desc = "右联"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "rollback",
                Desc = "回滚"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "round",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "rtrim",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "select",
                Desc = "查询"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "Serializable",
                Desc = "隔离级别:可串行化"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "session",
                Desc = "会话"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "set",
                Desc = "设置"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "show",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "smalldatetime",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "smallint",
                Desc = "小整数类型"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "smallmoney",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "soundex",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "sp_executesql",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "space",
                Desc = "空格"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "sql_variant",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "start",
                Desc = "开始"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "stuff",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "sum",
                Desc = "求和"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "table",
                Desc = "表"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "text",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "then",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "time",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "timestamp",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "tinyint",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "transaction",
                Desc = "事务"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "read uncommitted",
                Desc = "隔离级别:未提交读"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "union",
                Desc = "合并"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "uniqueidentifier",
                Desc = "全局唯一的标识,NewID()"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "until",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "update",
                Desc = "更新"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "upper",
                Desc = "大写"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "use",
                Desc = "使用"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "using",
                Desc = "使用"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "utf8",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "values",
                Desc = "插入值"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "values",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "varbinary",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "varchar",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "varchar(n)",
                Desc = "可变字符"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "variables",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "view",
                Desc = "视图"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "when",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "where",
                Desc = "条件"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "while",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "with",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "xml",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "year",
                Desc = ""
            });
        }

        private static void Deal()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var kw in KeyWordDic.Distinct().OrderBy(p => p.KeyWord))
            {
                sb.AppendLine(@"KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = """ + kw.KeyWord + @""",Desc = """ + kw.Desc + @"""
            });");
            }

            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }

        public static List<SQLKeyWord> GetKeyWordList()
        {
            return KeyWordDic;
        }

    }
}
