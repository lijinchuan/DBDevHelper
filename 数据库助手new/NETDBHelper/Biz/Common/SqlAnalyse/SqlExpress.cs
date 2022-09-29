using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SqlExpress:ISqlExpress
    {
        public int Deep
        {
            get;
            set;
        }

        public int StartLine
        {
            get;
            set;
        }

        public int StartIndex
        {
            get;
            set;
        }

        public int StartInineIndex
        {
            get;
            set;
        }

        public int EndLine
        {
            get;
            set;
        }

        public int EndIndex
        {
            get;
            set;
        }

        public int EndInineIndex
        {
            get;
            set;
        }

        public string Val
        {
            get;
            set;
        } = string.Empty;

        public SqlExpressType ExpressType
        {
            get;
            set;
        }

        public AnalyseType AnalyseType
        {
            get;
            set;
        }

    }
}
