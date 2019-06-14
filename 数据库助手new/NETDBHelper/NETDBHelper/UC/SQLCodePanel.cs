using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class SQLCodePanel : TabPage
    {
        public SQLCodePanel()
        {
            InitializeComponent();
        }

        public void SetCode(string code)
        {
            this.sqlEditBox1.Text = code;
        }
    }
}
