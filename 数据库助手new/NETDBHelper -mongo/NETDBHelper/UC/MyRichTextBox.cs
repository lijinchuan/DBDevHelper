using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class MyRichTextBox : RichTextBox
    {
        private const int WM_PAINT = 0x000F;
        private const int WM_ERASEBKGND = 0X0014;
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

        public void DoMessage(Message m)
        {
            base.WndProc(ref m);
        }
    }
}
