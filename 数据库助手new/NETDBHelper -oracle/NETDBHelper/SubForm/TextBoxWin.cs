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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.textBox1.Text = text;
            this.Text = caption;
        }

        private void BtnCpy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            Clipboard.SetText(this.textBox1.Text);
        }
    }
}
