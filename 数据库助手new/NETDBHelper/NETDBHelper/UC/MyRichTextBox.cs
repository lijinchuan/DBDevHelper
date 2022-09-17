using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class MyRichTextBox : RichTextBox
    {
        #region API Stuff
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
        public MyRichTextBox()
        {
            InitializeComponent();
        }

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

        public new void Undo()
        {
            while (this.CanUndo)
            {
                if (this.UndoActionName != "未知" && !"UnKnown".Equals(this.UndoActionName, StringComparison.OrdinalIgnoreCase))
                {
                    base.Undo();
                    break;
                }
                else
                {
                    base.Undo();
                }
            }
        }

        public new void Redo()
        {
            while (this.CanRedo)
            {
                if (this.RedoActionName != "未知" && !"UnKnown".Equals(this.RedoActionName, StringComparison.OrdinalIgnoreCase))
                {
                    base.Redo();
                    break;
                }
                else
                {
                    base.Redo();
                }
            }
        }

        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        #endregion
        public int HorizontalPosition
        {
            get { return GetScrollPos((IntPtr)this.Handle, SB_HORZ); }
            set {
                SetScrollPos((IntPtr)this.Handle, SB_HORZ, value, true);
            }
        }

        public int VerticalPosition
        {
            get { return GetScrollPos((IntPtr)this.Handle, SB_VERT); }
            set {
                SetScrollPos(Handle, SB_VERT, value, true);

                PostMessage(Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * value, 0);
            }
        }

        public void DoMessage(Message m)
        {
            base.WndProc(ref m);
        }
    }
}
