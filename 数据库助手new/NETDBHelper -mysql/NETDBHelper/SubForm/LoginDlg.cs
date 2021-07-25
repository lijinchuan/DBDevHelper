using Ljc.Com.Blog.Model.Contract;
using LJC.FrameWork.SOA;
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
    public partial class LoginDlg : Form
    {
        public LoginDlg()
        {
            InitializeComponent();
        }

        private void BTNLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBUser.Text) || string.IsNullOrWhiteSpace(TBPwd.Text))
            {
                MessageBox.Show("用户名密码不能为空");
                return;
            }

            var resp = ESBClient.DoSOARequest<UserLoginV1Response>(Ljc.Com.Blog.Model.Consts.SNo,
                Ljc.Com.Blog.Model.Consts.Func_UserLoginV1, new UserLoginV1Request
                {
                    UserName = TBUser.Text,
                    UserPassword = TBPwd.Text
                });

            if (resp.Code == 1)
            {
                Util.UserLogin(TBUser.Text, resp.UserLevel);
                LJC.FrameWork.SOA.ESBClient.Close();
                this.Hide();
                MessageBox.Show("登录成功");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(resp.Msg);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            try
            {
                LJC.FrameWork.SOA.ESBClient.Close();
            }
            catch
            {
                
            }
        }

        private void BTNCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
