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

        public void DoMessage(Message m)
        {
            base.WndProc(ref m);
        }
    }
}
