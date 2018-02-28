using LJC.FrameWork.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RedisHelperUI
{
    public partial class AddRedisServerForm : Form
    {
        public AddRedisServerForm()
        {
            InitializeComponent();
        }

        public RedisHelper.Model.RedisServerEntity NewServer
        {
            get;
            set;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBName.Text))
            {
                MessageBox.Show("连接名称不能为空");
                TBName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TBConnstr.Text))
            {
                MessageBox.Show("连接串不能为空");
                TBConnstr.Focus();
                return;
            }

            if(EntityTableEngine.LocalEngine.Exists(Global.TBName_RedisServer,TBName.Text))
            {
                MessageBox.Show(this,"连接名称已存在:"+TBName.Text);
                TBName.Focus();
                return;
            }

            if (CBTest.Checked)
            {
                bool testsuccess = true;
                RedisUtil.Execute(TBConnstr.Text, (client) => { }, (ex) =>
                    {
                        testsuccess = false;
                        MessageBox.Show("验证失败:" + ex.Message);
                    });

                if (!testsuccess)
                {
                    return;
                }
            }

            this.NewServer=new RedisHelper.Model.RedisServerEntity
            {
                ConnStr=TBConnstr.Text.Trim(),
                ServerName=TBName.Text.Trim(),
                IsPrd=CBIsprd.Checked,
            };
            EntityTableEngine.LocalEngine.Upsert(Global.TBName_RedisServer, NewServer);

            this.DialogResult = DialogResult.OK;
        }
    }
}
