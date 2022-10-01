using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Biz.Common
{
    public struct SQLKeyWord:IEqualityComparer<SQLKeyWord>
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
            if (x.KeyWord==null && y.KeyWord == null)
            {
                return true;
            }

            if (x.KeyWord == null || y.KeyWord == null)
            {
                return false;
            }

            var ret = x.KeyWord.ToUpper() == y.KeyWord.ToUpper();
            if(ret)
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
                KeyWord = "@@error",
                Desc = "全局错误"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "@@fetch_status",
                Desc = "游标状态"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "@@rowcount",
                Desc = "行数"
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
                KeyWord= "column",
                Desc="列"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "commit",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "convert",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "constraint",
                Desc="约束"
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
                KeyWord = "cross apply",
                Desc = @" CROSS APPLY 的意思是“交叉应用”，在查询时首先查询左表，然后右表的每一条记录跟左表的当前记录进行匹配。
                匹配成功则将左表与右表的记录合并为一条记录输出；匹配失败则抛弃左表与右表的记录。（与 INNER JOIN 类似）"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "outer apply",
                Desc = @"  OUTER APPLY 的意思是“外部应用”，与 CROSS APPLY 的原理一致，只是在匹配失败时，
                        左表与右表也将合并为一条记录输出，不过右表的输出字段为 null。（与 LEFT OUTER JOIN 类似）"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "cursor",
                Desc = "游标"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datalength",
                Desc = @"datalength(expression)
其中expression可以是任何类型的表达式，表示该表达式所占用的字节数，返回值类型为int
例：
在 sql server中 select datalength('中国'); 返回值是 4.
select datalength('zhongguo') 返回值是 8."
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "date",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "dateadd",
                Desc = @"返回已添加指定时间间隔的日期。
DateAdd(interval, number, date)
参数
interval
必选项。字符串表达式，表示要添加的时间间隔。有关数值，请参阅“设置”部分。
number
必选项。数值表达式，表示要添加的时间间隔的个数。数值表达式可以是正数（得到未来的日期）或负数（得到过去的日期）。
date
必选项。Variant 或要添加 interval 的表示日期的文字。

interval 参数可以有以下值：
设置  描述
yyyy  年
q  季度
m  月
y  一年的日数
d  日
w  一周的日数
ww  周
h  小时
n  分钟
s  秒"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "datediff",
                Desc = "计算时间差，DATEDIFF(year|quarter|month|week|day|hour|minute|second|millisecond,开始时间,结束时间)",
                HighColor=Color.Red
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
                KeyWord = "holdlock",
                Desc = @"将共享锁保留到事务完成，而不是在相应的表、行或数据页不再需要时就立即释放锁。
HOLDLOCK等同于SERIALIZABLE。"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "hour",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "identity",
                Desc = @"identity(m,n)
m表示的是初始值，n表示的是每次自动增加的值
如果m和n的值都没有指定，默认为（1,1）"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "if",
                Desc = ""
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
                KeyWord = "like",
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
                KeyWord = "millisecond",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "min",
                Desc = "最小"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "minute",
                Desc = "分钟"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "money",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "month",
                Desc = "月"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nchar",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "newid",
                Desc = "生成GUID"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nocount",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "nolock",
                Desc = @"不要发出共享锁，并且不要提供排它锁。
当此选项生效时，可能会读取未提交的事务或一组在读取中间回滚的页面。
有可能发生脏读。仅应用于SELECT语句。"
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
                KeyWord = "paglock",
                Desc = "在通常使用单个表锁的地方采用页锁。"
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
                KeyWord = "quarter",
                Desc = ""
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
                KeyWord = "readcommitted",
                Desc = @"用与运行在提交读隔离级别的事务相同的锁语义执行扫描。
默认情况下，SQL Server2000在此隔离级别上操作。"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "readpast",
                Desc = @"跳过锁定行。此选项导致事务跳过由其它事务锁定的行（这些行平常会显示在结果集内），而不是阻塞该事务，
使其等待其它事务释放在这些行上的锁。READPAST 锁提示仅适用于运行在提交读隔离级别的事务，并且只在行级锁之后读取。
仅适用于SELECT语句。"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "readuncommitted",
                Desc = @"仅适用于SELECT语句。
READUNCOMMITTED 等同于NOLOCK。"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "real",
                Desc = ""
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
                KeyWord = "replace",
                Desc = "替换"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "repeatableread",
                Desc = "用与运行在可重复读隔离级别的事务相同的锁语义执行扫描。"
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
                KeyWord = "rowlock",
                Desc = "使用行级锁，而不使用粒度更粗的页级锁和表级锁。"
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
                KeyWord = "second",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "select",
                Desc = "查询"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "serializable",
                Desc = "用与运行在可串行读隔离级别的事务相同的锁语义执行扫描。等同于HOLDLOCK。"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "set",
                Desc = "设置"
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
                KeyWord = "sp_addextendedproperty",
                Desc = "备注,示例：EXEC sp_addextendedproperty N'MS_Description', N'备注内容', N'SCHEMA', N'dbo',N'TABLE', N'表名', N'COLUMN', N'字段名'"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "sp_executesql",
                Desc = "执行SQL语句"
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
                KeyWord = "stuff",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "substring",
                Desc = "substring(int beginIndex)"
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
                KeyWord = "tablock",
                Desc = @"使用表锁代替粒度更细的行级锁或页级锁。在语句结束前，SQL Server一直持有该锁。
但是，如果同时指定HOLDLOCK，那么在事务结束之前，锁将被一直持有。  "
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "tablockx",
                Desc = @"使用表的排它锁。该锁可以防止其它事务读取或更新表，并在语句或事务结束前一直持有。"
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
                KeyWord = "top",
                Desc = ""
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "transaction",
                Desc = "事务"
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
                KeyWord = "update",
                Desc = "更新"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "updlock",
                Desc = @"读取表时使用更新锁，而不使用共享锁，并将锁一直保留到语句或事务的结束。
UPDLOCK的优点是允许您读取数据（不阻塞其它事务）并在以后更新数据，同时确保自从上次读取数据后数据没有被更改。"
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
                KeyWord = "view",
                Desc = "视图"
            });
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "week",
                Desc = ""
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
                KeyWord = "xlock",
                Desc = @"使用排它锁并一直保持到由语句处理的所有数据上的事务结束时。
可以使用PAGLOCK或TABLOCK指定该锁，这种情况下排它锁适用于适当级别的粒度"
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

        public static List<SqlKeyword> GetSqlKeywordList()
        {
            var list = BigEntityTableEngine.LocalEngine.List<SqlKeyword>(nameof(SqlKeyword), 1, int.MaxValue).ToList();
            return list;
        }

        private static void Deal()
        {
            StringBuilder sb = new StringBuilder();

            foreach(var kw in KeyWordDic.Distinct().OrderBy(p=>p.KeyWord))
            {
                sb.AppendLine(@"KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = """+kw.KeyWord+@""",Desc = """+kw.Desc+@"""
            });");
            }

            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }

        public static Dictionary<string,Color> GetKeyWordColor()
        {
            Dictionary<string, Color> dic = new Dictionary<string, Color>();

            dic.Add("use", Color.Blue);
            dic.Add("set", Color.Blue);
            dic.Add("table", Color.Blue);
            dic.Add("tablock", Color.Blue);
            dic.Add("tablockx", Color.Blue);
            dic.Add("transaction", Color.Blue);
            dic.Add("rollback", Color.Blue);
            dic.Add("return", Color.Blue);
            dic.Add("on", Color.Blue);
            dic.Add("view", Color.Blue);
            dic.Add("ansi_nulls", Color.DeepPink);
            dic.Add("quoted_identifier", Color.DeepPink);
            dic.Add("create", Color.Blue);
            dic.Add("cross apply", Color.Blue);
            dic.Add("out", Color.Blue);
            dic.Add("outer apply", Color.Blue);
            dic.Add("@@error", Color.DeepPink);
            dic.Add("@@rowcount", Color.DeepPink);
            dic.Add("@@fetch_status", Color.DeepPink);
            dic.Add("@@TRANCOUNT", Color.DeepPink);
            dic.Add("select", Color.Blue);
            dic.Add("*", Color.Gray);
            dic.Add("from", Color.Blue);
            dic.Add("delete", Color.Blue);
            dic.Add("update", Color.Blue);
            dic.Add("updlock", Color.Blue);
            dic.Add("insert", Color.Blue);
            dic.Add("into", Color.Blue);
            dic.Add("values", Color.Blue);
            dic.Add("where", Color.Blue);
            dic.Add("distinct", Color.Blue);
            dic.Add("top", Color.Blue);
            dic.Add("nolock", Color.Blue);
            dic.Add("with", Color.Blue);
            dic.Add("like", Color.Blue);
            dic.Add("order", Color.Blue);
            dic.Add("by", Color.Blue);
            dic.Add("desc", Color.Blue);
            dic.Add("asc", Color.Blue);
            dic.Add("between", Color.Blue);
            dic.Add("and", Color.Blue);
            dic.Add("or", Color.Blue);
            dic.Add("not", Color.Blue);
            dic.Add("null", Color.Gray);
            dic.Add("identity", Color.Blue);
            dic.Add("isnull", Color.Red);
            dic.Add("getdate", Color.Red);
            dic.Add("goto", Color.Blue);
            dic.Add("holdlock", Color.Blue);
            dic.Add("year", Color.Red);
            dic.Add("month", Color.Red);
            dic.Add("day", Color.Red);
            dic.Add("cast", Color.Red);
            dic.Add("as", Color.Blue);
            dic.Add("convert", Color.Red);
            dic.Add("case", Color.Blue);
            dic.Add("when", Color.Blue);
            dic.Add("then", Color.Blue);
            dic.Add("else", Color.Blue);
            dic.Add("end", Color.Blue);
            dic.Add("if", Color.Blue);
            dic.Add("is", Color.Blue);
            dic.Add("begin", Color.Blue);
            dic.Add("exec", Color.Blue);
            dic.Add("execute", Color.Blue);
            dic.Add("sp_executesql", Color.Blue);
            dic.Add("proc", Color.Blue);
            dic.Add("procedure", Color.Blue);
            dic.Add("declare", Color.Blue);
            dic.Add("while", Color.Blue);
            dic.Add("join", Color.Blue);
            dic.Add("key", Color.Blue);
            dic.Add("for", Color.Blue);
            dic.Add("full", Color.Blue);
            dic.Add("image", Color.Green);
            dic.Add("in", Color.Blue);
            dic.Add("inner", Color.Blue);
            dic.Add("outer", Color.Blue);
            dic.Add("hash", Color.Blue);
            dic.Add("group", Color.Blue);
            dic.Add("output", Color.Blue);
            dic.Add("option", Color.Blue);
            dic.Add("recompile", Color.Blue);
            dic.Add("commit", Color.Blue);
            dic.Add("nocount", Color.Blue);

            dic.Add("count", Color.Red);
            dic.Add("substring", Color.Red);
            dic.Add("sum", Color.Red);
            dic.Add("max", Color.Red);
            dic.Add("min", Color.Red);
            dic.Add("avg", Color.Red);
            dic.Add("exists", Color.Red);
            dic.Add("having", Color.Red);
            dic.Add("mid", Color.Red);
            dic.Add(",", Color.Gray);
            dic.Add("[", Color.Gray);
            dic.Add("]", Color.Gray);
            dic.Add("primary", Color.Blue);
            dic.Add("print", Color.Red);
            dic.Add("charindex", Color.Red);
            dic.Add("left", Color.Red);
            dic.Add("right", Color.Red);
            dic.Add("stuff", Color.Red);
            dic.Add("len", Color.Red);
            dic.Add("round", Color.Red);
            dic.Add("rowlock", Color.Blue);
            dic.Add("difference", Color.Red);
            dic.Add("soundex", Color.Red);
            dic.Add("lower", Color.Red);
            dic.Add("upper", Color.Red);
            dic.Add("ltrim", Color.Red);
            dic.Add("rtrim", Color.Red);
            dic.Add("repeatableread", Color.Blue);
            dic.Add("replace", Color.Red);
            dic.Add("replicate", Color.Red);
            dic.Add("serializable", Color.Blue);
            dic.Add("space", Color.Red);
            dic.Add("reverse", Color.Red);
            dic.Add("quotename", Color.Red);
            dic.Add("paglock", Color.Blue);
            dic.Add("patindex", Color.Red);
            dic.Add("parsename", Color.Red);
            dic.Add("isdate", Color.Red);
            dic.Add("dateadd", Color.Red);
            dic.Add("datename", Color.Red);
            dic.Add("datepart", Color.Red);
            dic.Add("datalength", Color.Red);
            dic.Add("coalesce", Color.Red);
            dic.Add("open", Color.Red);
            dic.Add("fetch", Color.Red);
            dic.Add("close", Color.Red);
            dic.Add("deallocate", Color.Red);

            dic.Add("char", Color.Green);
            dic.Add("nchar", Color.Green);
            dic.Add("newid", Color.Red);
            dic.Add("varchar", Color.Green);
            dic.Add("nvarchar", Color.Green);
            dic.Add("datetime", Color.Green);
            dic.Add("float", Color.Green);
            dic.Add("text", Color.Green);
            dic.Add("truncate", Color.Blue);
            dic.Add("ntext", Color.Green);
            dic.Add("bit", Color.Green);
            dic.Add("binary", Color.Green);
            dic.Add("varbinary", Color.Green);
            dic.Add("int", Color.Green);
            dic.Add("tinyint", Color.Green);
            dic.Add("smallint", Color.Green);
            dic.Add("bigint", Color.Green);
            dic.Add("decimal", Color.Green);
            dic.Add("numeric", Color.Green);
            dic.Add("smallmoney", Color.Green);
            dic.Add("money", Color.Green);
            dic.Add("readcommitted", Color.Blue);
            dic.Add("readpast", Color.Blue);
            dic.Add("readuncommitted", Color.Blue);
            dic.Add("real", Color.Green);
            dic.Add("datetime2", Color.Green);
            dic.Add("smalldatetime", Color.Green);
            dic.Add("date", Color.Green);
            dic.Add("time", Color.Green);
            dic.Add("datetimeoffset", Color.Green);
            dic.Add("timestamp", Color.Green);
            dic.Add("sql_variant", Color.Green);
            dic.Add("uniqueidentifier", Color.Green);
            dic.Add("xlock", Color.Blue);
            dic.Add("xml", Color.Green);
            dic.Add("cursor", Color.Green);

            return dic;
        }

        public static List<SQLKeyWord> GetKeyWordList()
        {
            return KeyWordDic;
        }

        public static Color GetColorByType(SqlKeyWordType type)
        {
            if (type == SqlKeyWordType.KeyWord)
            {
                return Color.Blue;
            }
            else if (type == SqlKeyWordType.Function)
            {
                return Color.Red;
            }
            else if (type == SqlKeyWordType.DataType)
            {
                return Color.Green;
            }
            else if (type == SqlKeyWordType.GlobVar)
            {
                return Color.DeepPink;
            }
            else if (type == SqlKeyWordType.Other)
            {
                return Color.Gray;
            }

            return Color.Blue;
        }

        public static SqlKeyWordType GetTypeByColor(Color color)
        {
            if (color == Color.Blue)
            {
                return SqlKeyWordType.KeyWord;
            }
            else if (color == Color.Red)
            {
                return SqlKeyWordType.Function;
            }
            else if (color == Color.Green)
            {
                return SqlKeyWordType.DataType;
            }
            else if (color == Color.DeepPink)
            {
                return SqlKeyWordType.GlobVar;
            }
            else if (color == Color.Gray)
            {
                return SqlKeyWordType.Other;
            }

            return SqlKeyWordType.KeyWord;
        }

        public static void WriteDB()
        {
            var checkList = BigEntityTableEngine.LocalEngine.List<SqlKeyword>(nameof(SqlKeyword), 1, int.MaxValue);
            if (checkList.Any())
            {
                return;
            }
            List<SqlKeyword> list = new List<SqlKeyword>();
            var keylist = GetKeyWordList();
            var keyWordColor = GetKeyWordColor();
            foreach (var k in keyWordColor)
            {
                var item = keylist.FirstOrDefault(p => p.KeyWord.Equals(k.Key, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(item.KeyWord))
                {
                    list.Add(new SqlKeyword
                    {
                        KeyWord = item.KeyWord,
                        HighColor = k.Value.Name,
                        Desc = item.Desc,
                        IsProtect = true,
                        SqlKeyWordType = GetTypeByColor(k.Value)
                    });
                }
                else
                {
                    list.Add(new SqlKeyword
                    {
                        KeyWord = k.Key,
                        HighColor = k.Value.Name,
                        IsProtect = true,
                        SqlKeyWordType = GetTypeByColor(k.Value)
                    });
                }
            }

            foreach (var item in keylist)
            {
                var color = keyWordColor.FirstOrDefault(p => p.Key.Equals(item.KeyWord, StringComparison.OrdinalIgnoreCase));
                if (string.IsNullOrWhiteSpace(color.Key))
                {
                    list.Add(new SqlKeyword
                    {
                        KeyWord = item.KeyWord,
                        HighColor = Color.Blue.Name,
                        Desc = item.Desc,
                        IsProtect = true,
                        SqlKeyWordType = SqlKeyWordType.KeyWord
                    });
                }
            }

            BigEntityTableEngine.LocalEngine.InsertBatch(nameof(SqlKeyword), list);
        }
    }
}
