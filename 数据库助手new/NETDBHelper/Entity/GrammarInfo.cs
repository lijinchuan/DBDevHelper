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
