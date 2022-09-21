using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 注释
    /// </summary>
    public class GrammarInfo
    {
        public int Start
        {
            get;
            set;
        }

        public int StartLine
        {
            get;
            set;
        }

        public int End
        {
            get;
            set;
        }

        public int EndLine
        {
            get;
            set;
        }

        public int Len
        {
            get;
            set;
        }

        /// <summary>
        /// 是否在范围内
        /// </summary>
        /// <param name="line"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool Contains(int line, int pos)
        {
            return (StartLine < line && EndLine > line)
                            || (StartLine == line && EndLine == line && Start <= pos && End >= pos)
                            || (StartLine == line && EndLine != line && Start <= pos)
                            || (StartLine != line && EndLine == line && End >= pos);
        }
    }

    public class GrammarInfoComparer : IEqualityComparer<GrammarInfo>
    {
        public bool Equals(GrammarInfo x, GrammarInfo y)
        {
            return x.Start == y.Start && x.End == y.End && x.StartLine == y.StartLine && x.EndLine == y.EndLine && x.Len == y.Len;
        }

        public int GetHashCode(GrammarInfo obj)
        {
            return obj.Start + obj.End + obj.StartLine + obj.EndLine + obj.Len;
        }
    }
}
