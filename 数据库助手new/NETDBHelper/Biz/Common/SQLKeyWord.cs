using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common
{
    public struct SQLKeyWord
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
    }

    public static class SQLKeyWordHelper
    {
        private static List<SQLKeyWord> KeyWordDic = new List<SQLKeyWord>();

        static SQLKeyWordHelper()
        {
            #region 类型
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "null",
                Desc = "空"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "char(n)",
                Desc="字符"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "varchar(n)",
                Desc="可变字符"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "int",
                Desc = "整数类型"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "smallint",
                Desc = "小整数类型"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "smallint",
                Desc = "小整数类型"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "numeric(p,d)",
                Desc = "定点数，精度由用户指定"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "real",
                Desc = "浮点数和双精度浮点数"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "float(n)",
                Desc = "精度至少位n位的浮点数"
            });
            #endregion

            #region 主键、外键
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "primary key(KEY)",
                Desc = "主键,后面括号中是作为主键的属性"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "foreign key(C) references T",
                Desc = "外键，括号中为外键，references后为外键的表"
            });
            #endregion

            #region CURD
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "create",
                Desc = "创建"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "table",
                Desc = "表"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "insert",
                Desc = "插入"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "into",
                Desc = ""
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "values",
                Desc = ""
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "delete",
                Desc = "删除"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "from",
                Desc = "从"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "update",
                Desc = "更新"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "set",
                Desc = "设置"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "where",
                Desc = "条件"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "drop",
                Desc = "移除"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "alter",
                Desc="更改"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "add",
                Desc = "添加"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "select",
                Desc="查询"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "distinct",
                Desc = "去重"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "all",
                Desc = "所有,不去重,全"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "and",
                Desc = "与,并且"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "or",
                Desc = "或者"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "not",
                Desc = "非,不"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "as",
                Desc = "别名"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "between",
                Desc = "范围"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "union",
                Desc="合并"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "intersect",
                Desc="相交"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "except",
                Desc = "差集"
            });


            #endregion

            #region 关联
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
                KeyWord = "right",
                Desc = "右联"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "inner",
                Desc = "内联"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "using",
                Desc="使用"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "on",
                Desc = ""
            });
            #endregion

            #region 排序
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "order",
                Desc = "排序"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "by",
                Desc = "根据"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "desc",
                Desc = "倒序"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "asc",
                Desc = "正序"
            });


            #endregion

            #region 统计
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "avg",
                Desc = "平均"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "min",
                Desc = "最小"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "max",
                Desc = "最大"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "sum",
                Desc = "求和"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "count",
                Desc = "总计"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "group",
                Desc = "分组"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "having",
                Desc = "对group by产生的分组进行筛选，可以使用聚集函数"
            });
            #endregion
        }

        public static List<SQLKeyWord> GetKeyWordList()
        {
            return KeyWordDic;
        }

    }
}
