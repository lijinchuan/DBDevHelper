using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class CreateEntityNavDlg : Form
    {
        public string InputString;
        private string InputTest
        {
            get;
            set;
        }

        public bool SupportMvcDisplay
        {
            get
            {
                return this.CBMVCDisplay.Checked;
            }
        }

        public bool SupportProtobuf
        {
            get
            {
                return this.CbProtobuf.Checked;
            }
        }

        public bool SupportJsonproterty
        {
            get
            {
                return this.CBJsonProperty.Checked;
            }
        }

        public bool SupportDBMapperAttr
        {
            get
            {
                return this.CBDatabaseMapperAttr.Checked;
            }
        }

        public CreateEntityNavDlg(string caption, string oldText = "", string inputTest = "") :
            base()
        {
            InitializeComponent();
            this.Text = caption;
            this.tbInput.Text = oldText;
            if (!string.IsNullOrWhiteSpace(inputTest))
            {
                InputTest = inputTest;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.tbInput.Text))
            {
                this.InputString = this.tbInput.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }
    }
}
