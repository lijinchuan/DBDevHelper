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
    public partial class AuthDlg : Form
    {
        public AuthDlg()
        {
            InitializeComponent();
        }


        public IEnumerable<string> Users
        {
            get;
            set;
        }

        public string TBName
        {
            get;
            set;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public bool AuthSelect
        {
            get
            {
                return CBSELECT.Checked;
            }
        }

        public bool AuthInsert
        {
            get
            {
                return CBINSERT.Checked;
            }
        }

        public bool AuthUpdate
        {
            get
            {
                return CBUPDATE.Checked;
            }
        }

        public bool AuthDelete
        {
            get
            {
                return CBDELETE.Checked;
            }
        }

        public bool AuthAll
        {
            get
            {
                return CBALL.Checked;
            }
        }

        public string User
        {
            get
            {
                return (string)CBUsers.SelectedItem;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CBUsers.SelectedIndex == -1)
            {
                MessageBox.Show("未选择用户");
                return;
            }

            if (CBUsers.SelectedText.Equals("public", StringComparison.OrdinalIgnoreCase))
            {
                if (!AuthSelect && !AuthUpdate && !AuthInsert && !AuthDelete)
                {
                    MessageBox.Show("未选择权限");
                }
                return;
            }

            if (MessageBox.Show("授权吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }

            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Text = string.Format(this.Text, TBName);
            var users = this.Users.ToList();
            users.Insert(0, "public");

            CBUsers.DataSource = users;
            CBUsers.SelectedIndex = 0;
            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CBALL_CheckedChanged(object sender, EventArgs e)
        {
            if (CBALL.Checked)
            {
                CBSELECT.Checked = CBUPDATE.Checked = CBDELETE.Checked = CBUPDATE.Checked
                    = CBVIEW.Checked = CBCHANGE.Checked = CBINDEX.Checked = true;
            }
        }
    }
}
