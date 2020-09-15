using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class InputStringDlg : Form
    {
        public string InputString;
        private string InputTest
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tbInput.ImeMode = ImeMode.On;
        }

        public InputStringDlg(string caption,string oldText="",string inputTest=""):
            base()
        {
            InitializeComponent();
            this.TopMost = true;
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
