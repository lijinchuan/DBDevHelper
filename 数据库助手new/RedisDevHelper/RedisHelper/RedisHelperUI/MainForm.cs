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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadRedisServers();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            LJC.FrameWork.Comm.Coroutine.CoroutineEngine.DefaultCoroutineEngine.Close();
        }

        private void LoadRedisServers()
        {
            if (TVServerList.Nodes.Count == 0)
            {
                TVServerList.Nodes.Add(new TreeNode
                {
                    Text="redis服务器"
                });
            }

            var parent = TVServerList.Nodes[0];
            parent.Nodes.Clear();
            foreach (var item in EntityTableEngine.LocalEngine.ListAll<RedisHelper.Model.RedisServerEntity>(Global.TBName_RedisServer))
            {
                var newnode = new TreeNode
                {
                    Text = item.ServerName,
                    Tag=item
                };

                foreach (var hp in RedisUtil.GetHostAndPoint(item.ConnStr))
                {
                    var hostnode = new TreeNode
                    {
                       Text=hp
                    };

                    newnode.Nodes.Add(hostnode);
                }

                parent.Nodes.Add(newnode);
            }
            parent.Expand();
        }

        private void TSM_NewRedis_Click(object sender, EventArgs e)
        {
            AddRedisServerForm addform = new AddRedisServerForm();
            if (addform.ShowDialog() == DialogResult.OK)
            {
                var parent = TVServerList.Nodes[0];
                var item = addform.NewServer;
                var newnode = new TreeNode
                {
                    Text = item.ServerName,
                    Tag = item
                };
                foreach (var hp in RedisUtil.GetHostAndPoint(item.ConnStr))
                {
                    var hostnode = new TreeNode
                    {
                        Text = hp
                    };

                    newnode.Nodes.Add(hostnode);
                }
                parent.Nodes.Add(newnode);
                parent.Expand();
            }
        }

        private void GetKeyType(string key,string connstr)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {

            }
        }

        private void TSMDel_Click(object sender, EventArgs e)
        {
            var node = TVServerList.SelectedNode;
            if (node == null)
            {
                return;
            }

            if (node.Level == 0)
            {
                if (MessageBox.Show("要删除所有的redis服务器吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)==DialogResult.No)
                {
                    return;
                }

                var subnodes=node.Nodes;
                foreach (TreeNode subnode in subnodes)
                {
                    if(EntityTableEngine.LocalEngine.Delete(Global.TBName_RedisServer, subnode.Text))
                    {
                        //subnode.Remove();
                    }
                }

                LoadRedisServers();
            }
            else if (node.Level == 1)
            {
                if (MessageBox.Show("要删除redis服务器" + node.Text + "?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                if (EntityTableEngine.LocalEngine.Delete(Global.TBName_RedisServer, node.Text))
                {
                    node.Remove();
                }
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = TVServerList.SelectedNode;
            if (node != null && node.Level == 1)
            {
                this.PanelRight.Controls.Clear();
                UC.UCSearch us = new UC.UCSearch();
                us.RedisServer = (RedisHelper.Model.RedisServerEntity)node.Tag;

                this.Text = string.Format("redis管理【查询 {0}】", us.RedisServer.ServerName);

                this.PanelRight.Controls.Add(us);
            }
        }

        private void TSMSearchKey_Click(object sender, EventArgs e)
        {
            var node = TVServerList.SelectedNode;
            if (node != null && node.Level == 1)
            {
                this.PanelRight.Controls.Clear();
                UC.UCKeySearch us = new UC.UCKeySearch();
                us.Dock = DockStyle.Fill;
                us.RedisServer = (RedisHelper.Model.RedisServerEntity)node.Tag;

                this.Text = string.Format("redis管理【key搜索 {0}】", us.RedisServer.ServerName);

                this.PanelRight.Controls.Add(us);
            }
        }
    }
}
