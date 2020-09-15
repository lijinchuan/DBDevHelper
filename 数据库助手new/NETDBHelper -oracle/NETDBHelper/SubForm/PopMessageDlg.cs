using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class PopMessageDlg : Form
    {
        public PopMessageDlg()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private static int AW_HIDE = 0x00010000;//该变量表示动画隐藏窗体
        private static int AW_SLIDE = 0x00040000;//该变量表示出现滑行效果的窗体
        private static int AW_VER_NEGATIVE = 0x00000008;//该变量表示从下向上开屏
        private static int AW_VER_POSITIVE = 0x00000004;//该变量表示从上向下开屏
        private const int AW_ACTIVE = 0x20000;//激活窗口
        private const int AW_BLEND = 0x80000;//应用淡入淡出结果

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);
            AnimateWindow(this.Handle, 1000, AW_ACTIVE);
            this.FormClosing += (a, b) => { AnimateWindow(this.Handle, 1000, AW_BLEND | AW_HIDE); };

        }

        public void PopShow(int cnt)
        {
            var maxcnt = Math.Max(1, Screen.PrimaryScreen.WorkingArea.Height / this.Height);
            if (cnt > maxcnt)
            {
                cnt = Math.Max(1, (cnt % maxcnt));
            }

            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.PointToClient(p);
            this.Location = p;
            this.Show();
            
            for (int i = 0; i < this.Height * cnt; i++)
            {
                this.Location = new Point(p.X, p.Y - i);
                System.Threading.Thread.Sleep(1);//消息框弹出速度，数值越大越慢
            }
        }

        public string GetMsg()
        {
            return this.TBMsg.Text;
        }

        public void SetMsg(string title, string content)
        {
            this.Text = title;
            this.TBMsg.Text = content;
        }

        //弹窗关闭
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 1000, AW_BLEND | AW_HIDE);
        }
    }
}
