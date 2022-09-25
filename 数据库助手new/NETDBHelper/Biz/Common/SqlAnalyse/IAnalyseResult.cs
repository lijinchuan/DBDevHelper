using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface IAnalyseResult
    {
        int StartLine
        {
            get;
            set;
        }

        int StartIndex
        {
            get;
            set;
        }

        int StartInineIndex
        {
            get;
            set;
        }

        int EndLine
        {
            get;
            set;
        }

        int EndIndex
        {
            get;
            set;
        }

        int EndInineIndex
        {
            get;
            set;
        }

        List<IAnalyseResult> NestResults
        {
            get;
            set;
        }
    }
}
