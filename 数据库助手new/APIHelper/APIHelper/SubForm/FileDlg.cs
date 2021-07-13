using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class FileDlg : SubBaseDlg
    {
        private OpenFileDialog fileDialog = new OpenFileDialog();
        public FileDlg()
        {
            InitializeComponent();

            BtnCopyBase64.Visible = RBBase64.Checked;
            RBBase64.CheckedChanged += RBBase64_VisibleChanged;
        }

        private void RBBase64_VisibleChanged(object sender, EventArgs e)
        {
            BtnCopyBase64.Visible = RBBase64.Checked;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string FileType
        {
            get
            {
                if (RBFile.Checked)
                {
                    return "file";
                }
                else if (RBBase64.Checked)
                {
                    return "base64";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string FilePath
        {
            get
            {
                return $"[{FileType}]{TBFileName.Text}";
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TBFileName.Text))
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.TBFileName.Text = fileDialog.FileName;
            }

        }

        private void BtnCopyBase64_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TBFileName.Text))
            {
                var base64 = Convert.ToBase64String(File.ReadAllBytes(TBFileName.Text));
                Clipboard.SetText(base64);

                MessageBox.Show("复制成功");
            }
        }
    }
}
