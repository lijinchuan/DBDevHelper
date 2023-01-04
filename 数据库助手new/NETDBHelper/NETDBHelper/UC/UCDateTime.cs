using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace NETDBHelper.UC
{
    public class UCDateTime: DateTimePicker
    {

        public UCDateTime() : base()
        {
            base.ValueChanged += UCDateTime_ValueChanged;
        }


        private void UCDateTime_ValueChanged(object sender, EventArgs e)
        {
            this._val = base.Value;
        }

        private Color _backColor = Color.White;
        /// <summary>
        ///     Gets or sets the background color of the control
        /// </summary>
        public override Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; Invalidate(); }
        }

        private DateTime? _val = null;
        public new DateTime? Value
        {
            get
            {
                return _val;
            }
            set
            {
                _val = value;
                if (value != null)
                {
                    base.Value = value.Value;
                }
                else
                {
                    base.Value = DateTime.MinValue;
                }
            }
        }



        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, object lParam);


        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);


        [DllImport("user32")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        const int WM_ERASEBKGND = 0x14;
        const int WM_NC_PAINT = 0x85;
        const int WM_PAINT = 0xF;
        const int WM_PRINTCLIENT = 0x318;


        //边框颜色
        private Pen BorderPen = new Pen(SystemColors.ControlDark, 2);

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            IntPtr hDC = IntPtr.Zero;
            Graphics gdc = null;
            switch (m.Msg)
            {
                //画背景色
                case WM_ERASEBKGND:
                    gdc = Graphics.FromHdc(m.WParam);
                    gdc.FillRectangle(new SolidBrush(_backColor), new Rectangle(0, 0, this.Width, this.Height));
                    gdc.Dispose();
                    break;
                case WM_NC_PAINT:
                    hDC = GetWindowDC(m.HWnd);
                    gdc = Graphics.FromHdc(hDC);
                    SendMessage(this.Handle, WM_ERASEBKGND, hDC, 0);
                    SendPrintClientMsg();
                    SendMessage(this.Handle, WM_PAINT, IntPtr.Zero, 0);
                    m.Result = (IntPtr)1;    // indicate msg has been processed
                    ReleaseDC(m.HWnd, hDC);
                    gdc.Dispose();
                    break;
                //画边框
                case WM_PAINT:
                    base.WndProc(ref m);
                    hDC = GetWindowDC(m.HWnd);
                    gdc = Graphics.FromHdc(hDC);
                    OverrideControlBorder(gdc);
                    ReleaseDC(m.HWnd, hDC);
                    gdc.Dispose();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }


        private void SendPrintClientMsg()
        {
            // We send this message for the control to redraw the client area
            Graphics gClient = this.CreateGraphics();
            IntPtr ptrClientDC = gClient.GetHdc();
            SendMessage(this.Handle, WM_PRINTCLIENT, ptrClientDC, 0);
            gClient.ReleaseHdc(ptrClientDC);
            gClient.Dispose();
        }


        private void OverrideControlBorder(Graphics g)
        {
            g.DrawRectangle(BorderPen, new Rectangle(0, 0, this.Width, this.Height));
        }
    }
}
