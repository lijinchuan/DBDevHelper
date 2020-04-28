using LJC.FrameWork.Data.EntityDataBase;
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
    public partial class AddServerForm : Form
    {
        public AddServerForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CBServerType.DataSource = new[]
            {
                new
                {
                    val=0,
                    text="CouchBase"
                },
                new
                {
                    val=1,
                    text="Memcached"
                }
            };

            this.CBServerType.ValueMember = "val";
            this.CBServerType.DisplayMember = "text";

            this.CBServerType.SelectedIndex = 0;
        }

        public CouchBaseServerEntity NewServer
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

            if (EntityTableEngine.LocalEngine.Exists(Global.TBName_RedisServer, TBName.Text))
            {
                MessageBox.Show(this, "连接名称已存在:" + TBName.Text);
                TBName.Focus();
                return;
            }

            if (CBTest.Checked)
            {
                //bool testsuccess = true;
                //RedisUtil.Execute(TBConnstr.Text, (client) => { }, (ex) =>
                //{
                //    testsuccess = false;
                //    MessageBox.Show("验证失败:" + ex.Message);
                //});

                //if (!testsuccess)
                //{
                //    return;
                //}
            }

            if (!string.IsNullOrWhiteSpace(TBDLLFile.Text))
            {
                try
                {
                    var serverandport = TBConnstr.Text.Split(',').First().Split(':');
                    var checkclient = new LJC.FrameWork.MemCached.ExportMemcachClient(TBDLLFile.Text, serverandport[0], int.Parse(serverandport[1]), string.Empty);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("插件检查错误：" + ex.Message);
                    return;
                }
            }

            this.NewServer = new CouchBaseServerEntity
            {
                ConnStr = TBConnstr.Text.Trim(),
                ServerName = TBName.Text.Trim(),
                CachServerType = (int)CBServerType.SelectedValue,
                ClientFile=TBDLLFile.Text,
                IsPrd = CBIsprd.Checked,
            };
            EntityTableEngine.LocalEngine.Upsert(Global.TBName_RedisServer, NewServer);

            this.DialogResult = DialogResult.OK;
        }

        private void CBTest_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TBConnstr_TextChanged(object sender, EventArgs e)
        {

        }

        private void CBIsprd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TBName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var basedir = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.InitialDirectory = basedir;
            openFileDialog.Filter = "dll|*.dll";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName.StartsWith(basedir))
                {
                    TBDLLFile.Text = openFileDialog.FileName.Substring(basedir.Length);
                }
                else
                {
                    TBDLLFile.Text = "";
                    MessageBox.Show("客户端插件只能放到当前运行目录下");
                }
                
            }
        }

        private void CBServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = CBServerType.SelectedValue.Equals(1);
            TBDLLFile.Text = "";
        }
    }
}
