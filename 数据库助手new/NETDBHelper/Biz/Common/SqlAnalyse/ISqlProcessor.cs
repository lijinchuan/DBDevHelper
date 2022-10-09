using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface ISqlProcessor
    {
        List<ISqlAnalyser> Handle();

        ISqlAnalyser GetSqlAnalyser(string token);

        List<string> FindTables(List<ISqlAnalyser> sqlAnalysers, int pos);

    }
}
