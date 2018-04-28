using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouchBaseDevHelper.UI
{
    public partial class FormInput : Form
    {
        public FormInput()
        {
            InitializeComponent();
        }

        public string Val
        {
            get
            {
                return this.TBValue.Text;
            }
            set
            {
                this.TBValue.Text = value;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
