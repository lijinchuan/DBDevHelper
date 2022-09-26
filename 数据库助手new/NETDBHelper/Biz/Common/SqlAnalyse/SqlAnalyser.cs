using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public abstract class SqlAnalyser : ISqlAnalyser
    {

        public int Deep
        {
            get;
            set;
        }

        public List<ISqlAnalyser> NestAnalyser 
        { 
            get;
            set; 
        }

        public abstract bool Accept(ISqlExpress sqlExpress, bool isKey);
        public abstract void Print(string sql);
    }
}
