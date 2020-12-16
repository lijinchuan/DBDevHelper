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
        private byte[] buffer = null;
        public TextBoxWin(string caption, string text) :
            base()
        {
            this.text = text;
            this.caption = caption;
            InitializeComponent();

            this.ShowInTaskbar = true;
        }

        public TextBoxWin(string caption, byte[] textbuffer) :
            base()
        {
            this.buffer = textbuffer;
            this.caption = caption;
            InitializeComponent();

            this.ShowInTaskbar = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (buffer == null)
            {
                this.TBContent.Text = text;
                this.Text = caption;
            }
            else
            {

            }
        }

        private void BtnCpy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TBContent.Text))
                return;
            Clipboard.SetText(this.TBContent.Text);
        }
    }
}
