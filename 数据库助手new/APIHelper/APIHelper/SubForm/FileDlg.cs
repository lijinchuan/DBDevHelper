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
    public partial class FileDlg : SubBaseDlg
    {
        private OpenFileDialog fileDialog = new OpenFileDialog();
        public FileDlg()
        {
            InitializeComponent();
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
    }
}
