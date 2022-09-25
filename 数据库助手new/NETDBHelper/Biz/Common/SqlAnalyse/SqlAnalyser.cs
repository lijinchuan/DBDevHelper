using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SqlAnalyser : ISqlAnalyser
    {
        public SqlAnalyser()
        {
        }

        public int Deep
        {
            get;
            set;
        }

        public bool Accept(ISqlExpress sqlExpress)
        {
            throw new NotImplementedException();
        }
    }
}
