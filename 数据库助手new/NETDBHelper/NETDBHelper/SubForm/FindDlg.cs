using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class FindDlg : Form
    {
        public FindDlg()
        {
            InitializeComponent();
        }

        private int findPos = 0;

        public Func<string,int, int> FindNext;
        public Func<string,int, int> FindLast;

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var word = TBWord.Text;
            if (string.IsNullOrEmpty(word))
            {
                LBMsg.Text = "请输入搜索词";
                return;
            }

            if (FindNext != null)
            {
                var pos = FindNext(word, findPos);
                findPos = pos;
            }
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            var word = TBWord.Text;
            if (string.IsNullOrEmpty(word))
            {
                LBMsg.Text = "请输入搜索词";
                return;
            }

            if (FindLast != null)
            {
                var pos = FindLast(word, findPos);
                findPos = pos;
            }
        }

        private Control OwnerCtl = null;
        public void ShowMe(Control owner)
        {
            this.OwnerCtl = owner;
            this.Show();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.BtnPrev.Enabled = FindLast != null;
            this.BtnNext.Enabled = FindNext != null;

            this.TBWord.ImeMode = ImeMode.On;

            if (this.OwnerCtl != null)
            {
                var pt = this.OwnerCtl.PointToScreen(this.OwnerCtl.Location);
                pt.Offset(this.OwnerCtl.Width / 2 - this.Width, this.OwnerCtl.Height / 2 - this.Height);
                this.Location = pt;
                this.OwnerCtl.VisibleChanged += OwnerCtl_VisibleChanged;
                this.OwnerCtl.ParentChanged += OwnerCtl_ParentChanged;
            }
        }

        private void OwnerCtl_ParentChanged(object sender, EventArgs e)
        {
            if (this.OwnerCtl.Parent == null)
            {
                this.Close();
            }
        }

        private void OwnerCtl_VisibleChanged(object sender, EventArgs e)
        {
            this.Visible = this.OwnerCtl.Visible;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (OwnerCtl != null && !OwnerCtl.IsDisposed)
            {
                this.OwnerCtl.VisibleChanged -= OwnerCtl_VisibleChanged;
                this.OwnerCtl.ParentChanged -= OwnerCtl_ParentChanged;
            }
        }
    }
}
