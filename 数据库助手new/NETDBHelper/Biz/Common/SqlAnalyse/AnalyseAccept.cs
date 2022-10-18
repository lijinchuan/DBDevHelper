using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public enum AnalyseAccept
    {
        None,
        /// <summary>
        /// 接受
        /// </summary>
        Accept,
        /// <summary>
        /// 接受作为子串
        /// </summary>
        AcceptDeeper,
        /// <summary>
        /// 拒绝
        /// </summary>
        Reject
    }
}
