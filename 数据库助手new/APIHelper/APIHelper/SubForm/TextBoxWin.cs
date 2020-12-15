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
    public partial class TextBoxWin : Form
    {
        private string text;
        private string caption;
        public TextBoxWin(string caption, string text) :
            base()
        {
            this.text = text;
            this.caption = caption;
            InitializeComponent();

            this.ShowInTaskbar = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.TBContent.Text = text;
            this.Text = caption;
        }

        private void BtnCpy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TBContent.Text))
                return;
            Clipboard.SetText(this.TBContent.Text);
        }
    }
}
