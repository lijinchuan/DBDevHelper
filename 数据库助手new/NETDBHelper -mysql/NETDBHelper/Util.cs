using NETDBHelper.SubForm;
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
        private static Dictionary<int, PopMessageDlg> PopDlgDic = new Dictionary<int, PopMessageDlg>();

        public static void SendMsg(Control ctl, string msg)
        {
            var parent = ctl;
            while (parent != null)
            {
                if (parent is MainFrm)
                {
                    ((MainFrm)parent).SetMsg(msg);
                    break;
                }

                parent = parent.Parent;
            }
        }

        public static void PopMsg(int msgid, string title, string content)
        {
            PopMessageDlg dlg = null;
            var cnt = 0;
            lock (PopDlgDic)
            {
                if (!PopDlgDic.TryGetValue(msgid, out dlg))
                {
                    dlg = new PopMessageDlg();
                    dlg.Text = title;
                    PopDlgDic.Add(msgid, dlg);
                    dlg.FormClosed += (s, e) =>
                    {
                        lock (PopDlgDic)
                        {
                            PopDlgDic.Remove(msgid);
                        }
                    };
                }

                foreach (var item in PopDlgDic)
                {
                    cnt++;
                    if (item.Value.Equals(dlg))
                    {
                        break;
                    }
                }
            }

            if (dlg.GetMsg() != content || dlg.Text != title)
            {
                dlg.SetMsg(title, content);
                dlg.PopShow(cnt);
            }
        }
    }
}
