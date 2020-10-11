using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common
{
    public static  class NumberHelper
    {
        public static int CovertToInt(string s)
        {
            int result=0;
            int.TryParse(s, out result);
            return result;
        }
        public static int CovertToInt(object o)
        {
            if (o == null || o == DBNull.Value)
                return 0;
            int result = 0;
            int.TryParse(o.ToString(), out result);
            return result;
        }
    }
}
