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
                KeyWord = "$",
                Desc = "代指符"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$all",
                Desc = "满足所有元素"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "$gt",
                Desc="大于"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$gte",
                Desc = "大于等于"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$in",
                Desc = "满足其中一个元素"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$inc",
                Desc = "修改操作，增加"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$lt",
                Desc = "小于"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$lt",
                Desc = "小于等于"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$match",
                Desc = "",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$or",
                Desc = "满足其中一个字段"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$pop",
                Desc = "指定删除Array中的第一个（-1） 或 最后一个 元素（1）"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$project",
                Desc = "投影"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$pull",
                Desc = "删除Array中的某一个元素"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$push",
                Desc = "对Array数据类型进行增加新元素"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$set",
                Desc = "修改操作，赋值"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "$unset",
                Desc = "删除字段"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "aggregate",
                Desc = "聚合"
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "db",
                Desc = "当前库",
                HighColor=Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "find",
                Desc = "查询",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "findOne",
                Desc = "查询第一条",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "getCollection",
                Desc = "取集合",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "limit",
                Desc = "前x条数据",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "update",
                Desc = "修改数据，默认只更新一条",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "updateMany",
                Desc = "修改数据",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "updateOne",
                Desc = "修改第一条数据",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord= "ObjectId",
                Desc="主键ID",
                HighColor=Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "skip",
                Desc = "跳过前x条数据",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "sort",
                Desc = "根据id字段进行倒序排列：-1  正序排列：1",
                HighColor = Color.Red
            });

            //stats
            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "stats",
                Desc = "服务器状态",
                HighColor = Color.Red
            });

            KeyWordDic.Add(new SQLKeyWord
            {
                KeyWord = "toArray",
                Desc = "转成数组函数",
                HighColor=Color.Red
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
