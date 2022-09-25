using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface ISqlProcessor
    {
        void Handle();

        ISqlAnalyser GetSqlAnalyser(string token);

    }
}
