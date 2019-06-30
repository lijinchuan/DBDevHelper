using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class MultiInputDlg : Form
    {
        public MultiInputDlg()
        {
            InitializeComponent();
            this.textBox1.Click += TextBox1_Click;
            this.textBox1.MouseLeave += TextBox1_MouseLeave;
        }

        private void TextBox1_MouseLeave(object sender, EventArgs e)
        {
            SetMoke();
        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
            SetMoke();
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            SetMoke();
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            SetMoke();
        }

        private void SetMoke()
        {
            if (string.IsNullOrWhiteSpace(Moke))
            {
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = Moke;
                textBox1.ForeColor = Color.LightGray;
            }
            else
            {
                if (textBox1.Text == Moke)
                {
                    textBox1.Text = string.Empty;
                    textBox1.ForeColor = Color.Black;
                }
            }
        }

        public string Moke
        {
            get;
            set;
        }

        public string InputString
        {
            get
            {
                return textBox1.Text;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetMoke();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = string.IsNullOrEmpty(InputString) ? DialogResult.Abort : DialogResult.OK;
        }
    }
}
