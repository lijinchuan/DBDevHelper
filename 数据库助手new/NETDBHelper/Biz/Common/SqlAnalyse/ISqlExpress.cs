using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface ISqlExpress
    {
        /// <summary>
        /// 解析深度
        /// </summary>
        int Deep
        {
            get;
            set;
        }

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

        SqlExpressType ExpressType
        {
            get;
            set;
        }

        AnalyseType AnalyseType
        {
            get;
            set;
        }

        string Val
        {
            get;
            set;
        }
    }
}
