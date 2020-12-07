using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.BtnPrev.Enabled = FindLast != null;
            this.BtnNext.Enabled = FindNext != null;
        }
    }
}
