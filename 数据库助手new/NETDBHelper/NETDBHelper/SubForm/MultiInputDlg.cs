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
        }

        public string InputString
        {
            get
            {
                return textBox1.Text;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = string.IsNullOrEmpty(InputString) ? DialogResult.Abort : DialogResult.OK;
        }
    }
}
