using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public class BaseUserControl:UserControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, int nBar, int wParam, int lParam);

        private const int WM_PAINT = 0x000F;
        private const int WM_ERASEBKGND = 0X0014;
        private const int WM_VSCROLL = 0x115;
        private const int SB_THUMBPOSITION = 4;
        public Action<PaintEventArgs> ManDraw;
        public bool LockPaint = false;

        public event Action<Message> OnWndProc;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT && LockPaint)
            {
                return;
            }

            base.WndProc(ref m);
            if (OnWndProc != null)
            {
                OnWndProc(m);
            }
        }
    }
}