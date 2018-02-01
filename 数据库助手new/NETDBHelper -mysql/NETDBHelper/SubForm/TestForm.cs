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
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            this.multSelectCombox1.DataSource = new List<string> { "男", "女", "1000", "2000", "3000", "4000" };
            this.multSelectCombox1.Text = "sfasdasfasdfasdf";
        }

        
    }
}
