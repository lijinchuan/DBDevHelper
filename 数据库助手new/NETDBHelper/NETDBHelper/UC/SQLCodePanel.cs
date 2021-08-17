using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;

namespace NETDBHelper.UC
{
    public partial class SQLCodePanel : TabPage
    {
        public SQLCodePanel()
        {
            InitializeComponent();
        }

        public void SetCode(DBSource dbSource,string dbname,string code)
        {
            this.sqlEditBox1.Text = code;
            this.sqlEditBox1.DBName = dbname;
            this.sqlEditBox1.DBServer = dbSource;
        }
    }
}
