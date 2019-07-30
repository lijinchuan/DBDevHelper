using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper
{
    public class Util
    {
        public static void SendMsg(Control ctl,string msg)
        {
            var parent = ctl;
            while (parent != null)
            {
                if(parent is MainFrm)
                {
                    ((MainFrm)parent).SetMsg(msg);
                    break;
                }

                parent = parent.Parent;
            }
        }
    }
}
