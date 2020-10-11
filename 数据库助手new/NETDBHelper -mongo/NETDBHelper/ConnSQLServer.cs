using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;

namespace NETDBHelper
{
    public partial class ConnSQLServer : Form
    {
        protected Timer timer;
        public DBSource DBSource
        {
            get;
            set;
        }

        private static int _lastServerSelectedIndex = 0;

        private static DBSourceCollection dbs;
        
        public ConnSQLServer()
        {
            InitializeComponent();
            this.pictureBox1.BackgroundImage = Resources.Resource1.mongo;

            this.cb_ServerType.DataSource = Biz.Common.Data.Common.GetSQLServerType().ToList();
            this.cb_ServerType.DisplayMember = "value";
            this.cb_ServerType.ValueMember = "key";


            this.cb_yz.DataSource = Biz.Common.Data.Common.GetEnumIDTypes().ToList();
            this.cb_yz.DisplayMember = "value";
            this.cb_yz.ValueMember = "key";
            this.cb_yz.SelectedIndex = 0;

            panel_yz.Enabled = false;
            dbs = dbs ?? (Biz.Common.XMLHelper.DeSerializeFromFile<DBSourceCollection>(Resources.Resource1.DbServersFile) ?? new DBSourceCollection());
            this.cb_servers.SelectedIndexChanged += new EventHandler(cb_Servers_SelectedIndexChanged);

            this.cb_username.SelectedIndexChanged += cb_username_SelectedIndexChanged;
        }

        void cb_username_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cb_yz.SelectedIndex == 0)
                return;

            var list = this.cb_username.DataSource as List<DBSource>;
            var loginname = list.FirstOrDefault(p => p.LoginName.Equals(cb_username.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase));
            if (loginname != null)
            {
                tb_password.Text = loginname.LoginPassword;
            }
        }

        void cb_Servers_SelectedIndexChanged(object sender, EventArgs e)
        {
            object val = Biz.Common.ReflectionHelper.Eval(cb_servers.SelectedValue, "Server") ?? cb_servers.SelectedValue;
            this.cb_yz.SelectedIndex = 0;
            this.cb_username.Text = string.Empty;
            this.tb_password.Text = string.Empty;

            var servers = dbs.Where(p => p.ServerName.Equals(val)).ToList();

            this.cb_username.DataSource = servers;
            this.cb_username.ValueMember = "LoginName";
            this.cb_username.DisplayMember = "LoginName";
            this.tb_password.Text = servers.Count == 0 ? string.Empty : servers[0].LoginPassword;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (this.panel_main.Enabled)
            {
                labmsg.Text = "";
            }
            else
            {
                string txt = "请稍等";
                if (labmsg.Text.Length == 3)
                {
                    txt += ".";
                }
                else if (labmsg.Text.Length == 4)
                {
                    txt += "..";
                }
                else if (labmsg.Text.Length == 5)
                {
                    txt += "...";
                }
                labmsg.Text = txt;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void cb_yz_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel_yz.Enabled = this.cb_yz.SelectedValue.ToString()=="2";
        }

        private void FillSqlServer(DataTable tb)
        {
            if (tb == null)
            {
                tb = new DataTable();
                tb.Columns.Add("Name");
                tb.Columns.Add("Server");
            }

            for (int i = 0; i < dbs.Count(); i++)
            {
                var row = tb.NewRow();
                if (dbs[i].Port == 0 || dbs[i].Port == 27017)
                {
                    row["Name"] = dbs[i].ServerName;
                }
                else
                {
                    row["Name"] = dbs[i].ServerName + ":" + dbs[i].Port;
                }
                row["Server"] = dbs[i].ServerName;
                tb.Rows.InsertAt(row, i);
            }
            this.cb_servers.DataSource = tb.AsEnumerable().Select(p => new { Name = p["Name"], Server = p["Server"] }).Distinct().ToList();
            this.cb_servers.DisplayMember = "Name";
            this.cb_servers.ValueMember = "Server";
            if (tb.Rows.Count > 0)
            {
                this.cb_servers.SelectedIndex = _lastServerSelectedIndex;
            }

            this.panel_main.Enabled = true;
        }

        private void ConnSQLServer_Load(object sender, EventArgs e)
        {
            //this.panel_main.Enabled = false;
            //Biz.UILoadHelper.LoadSqlServer(this, FillSqlServer);
            if (dbs != null)
            {
                FillSqlServer(null);
            }
        }

        private void Btn_Conn_Click(object sender, EventArgs e)
        {
            DBSource = new DBSource();
            try
            {
                var server = cb_servers.Text.Split(':');
                DBSource.ServerName = server.First();
                if (server.Length > 1)
                {
                    DBSource.Port = int.Parse(server[1]);
                }
            }
            catch
            {
                MessageBox.Show("服务器名填写错误");
                return;
            }

            DBSource.IDType = (IDType)(int)cb_yz.SelectedValue;
            DBSource.LoginName = cb_username.Text;
            DBSource.LoginPassword = tb_password.Text;
            if (DBSource.IDType == IDType.uidpwd)
            {
                if (string.IsNullOrWhiteSpace(DBSource.LoginName))
                {
                    MessageBox.Show("请填写登陆名");
                    return;
                }
            }
            this.panel_main.Enabled = false;
            if (!Biz.Common.Data.MongoDBHelper.CheckSQLConn(DBSource))
            {
                MessageBox.Show(Biz.Common.Data.MongoDBHelper.LastException.Message);
                this.panel_main.Enabled = true;
                return;
            }
            //保存一下

            this.DialogResult = DialogResult.OK;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnRefrash_Click(object sender, EventArgs e)
        {
            this.panel_main.Enabled = false;
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            Biz.UILoadHelper.LoadSqlServer(this, FillSqlServer);
        }
    }
}
