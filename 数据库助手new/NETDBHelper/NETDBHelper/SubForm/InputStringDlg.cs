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

        public event Action DlgResult;

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

        private Control OwnerCtl = null;
        public void ShowMe(Control owner)
        {
            this.OwnerCtl = owner;
            this.Show();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tbInput.ImeMode = ImeMode.On;

            if (this.OwnerCtl != null)
            {
                var pt = this.OwnerCtl.PointToScreen(this.OwnerCtl.Location);
                pt.Offset(this.OwnerCtl.Width / 2 - this.Width, this.OwnerCtl.Height / 2 - this.Height);
                this.Location = pt;
                this.OwnerCtl.VisibleChanged += OwnerCtl_VisibleChanged;
                
            }
        }

        private void OwnerCtl_VisibleChanged(object sender, EventArgs e)
        {
            this.Visible = this.OwnerCtl.Visible;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (OwnerCtl != null)
            {
                this.OwnerCtl.VisibleChanged -= OwnerCtl_VisibleChanged;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.tbInput.Text))
            {
                this.InputString = this.tbInput.Text;

                if (this.DlgResult != null)
                {
                    this.DlgResult();
                }

                if (this.Modal)
                    this.DialogResult = DialogResult.OK;
                else
                    this.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (this.Modal)
                this.DialogResult = DialogResult.Abort;
            else
                this.Close();
        }
    }
}
