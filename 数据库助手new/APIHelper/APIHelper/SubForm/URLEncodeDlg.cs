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
    public partial class URLEncodeDlg : SubBaseDlg
    {
        public URLEncodeDlg()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            TBEncode.DoubleClick += TBEncode_DoubleClick;
            TBDecode.DoubleClick += TBDecode_DoubleClick;
        }

        private void TBDecode_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(TBDecode.Text);
            Util.SendMsg(this, "已复制到剪贴板");
        }

        private void TBEncode_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(TBEncode.Text);
            Util.SendMsg(this, "已复制到剪贴板");
        }

        private void TBDecode_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnEncode_Click(object sender, EventArgs e)
        {
            TBEncode.Text = LJC.FrameWorkV3.Comm.WebUtility.UrlEncode(TBDecode.Text);
        }

        private void BtnDecode_Click(object sender, EventArgs e)
        {
            TBDecode.Text = LJC.FrameWorkV3.Comm.WebUtility.UrlDecode(TBEncode.Text);
        }
    }
}
