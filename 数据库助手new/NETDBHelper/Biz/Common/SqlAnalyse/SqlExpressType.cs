using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public enum SqlExpressType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        Bracket,
        /// <summary>
        /// 注释
        /// </summary>
        Annotation,
        Token
    }
}
