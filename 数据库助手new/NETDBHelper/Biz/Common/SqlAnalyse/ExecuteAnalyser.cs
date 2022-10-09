using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class ExecuteAnalyser:ExecAnalyser
    {
        public override string GetPrimaryKey()
        {
            return keyExecute;
        }
    }
}
