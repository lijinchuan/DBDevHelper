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
    public partial class ModifyUserDlg : Form
    {
        public string User
        {
            get;
            private set;
        }

        public ModifyUserDlg(string user=null)
        {
            InitializeComponent();
            this.User = user;
        }

        public string NewPassword
        {
            get
            {
                return this.TBNewPassword.Text;
            }
        }

        private void BtnUpdatePassword_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(this.User))
            {
                MessageBox.Show("用户不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                MessageBox.Show("密码不能为空");
                return;
            }

            if (MessageBox.Show("要修改用户" + User + "密码吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
