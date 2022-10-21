using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface ISqlProcessor
    {
        List<ISqlAnalyser> Analyse();

        void SetSql(string sql);

        ISqlAnalyser GetSqlAnalyser(string token);

        List<string> FindTables(int pos);

        ISqlExpress GetNext(int offset = 0);

        ISqlExpress FindExpress(int pos);

    }
}
