using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public enum AnalyseType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,
        Key,
        Table,
        TableAlias,
        Column,
        ColumnAlas
    }
}
