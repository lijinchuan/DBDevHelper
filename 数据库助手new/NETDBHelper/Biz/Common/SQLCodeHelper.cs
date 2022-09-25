using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common
{
    public static class SQLCodeHelper
    {
        static HashSet<string> SQLKeys = new HashSet<string>();
        static SQLCodeHelper()
        {
            SQLKeys.Add("select");
            SQLKeys.Add("distinct");
            SQLKeys.Add("top");
            SQLKeys.Add("as");
            SQLKeys.Add("join");
            SQLKeys.Add("on");
            SQLKeys.Add("from");
            SQLKeys.Add("where");
            SQLKeys.Add("with");
            SQLKeys.Add("nolock");
            SQLKeys.Add("group");
            SQLKeys.Add("by");
            SQLKeys.Add("having");
            SQLKeys.Add("order");

        }

        
    }
}
