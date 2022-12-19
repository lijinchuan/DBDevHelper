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
    public partial class UCSearchOptions : UserControl
    {
        public UCSearchOptions()
        {
            InitializeComponent();
            Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public bool IsMatchAll
        {
            get
            {
                return CBMatchAll.Checked;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
