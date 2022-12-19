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

        public bool SearchDB
        {
            get
            {
                return CBDB.Checked;
            }
        }

        public bool SearchTB
        {
            get
            {
                return CBTB.Checked;
            }
        }

        public bool SearchField
        {
            get
            {
                return CBField.Checked;
            }
        }

        public bool SearchProc
        {
            get
            {
                return CBProc.Checked;
            }
        }

        public bool SearchFunc
        {
            get
            {
                return CBFun.Checked;
            }
        }

        public bool SearchOther
        {
            get
            {
                return CBOther.Checked;
            }
        }

        public bool SearchView
        {
            get
            {
                return CBView.Checked;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
