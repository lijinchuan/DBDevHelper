using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface ISqlAnalyser
    {
        /// <summary>
        /// 解析深度
        /// </summary>
        int Deep { get; set; }

        bool Accept(ISqlExpress sqlExpress,bool isKey);

        List<ISqlAnalyser> NestAnalyser { get; set; }
    }
}
