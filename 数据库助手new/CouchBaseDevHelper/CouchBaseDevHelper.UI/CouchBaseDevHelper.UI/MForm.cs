using LJC.FrameWork.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CouchBaseDevHelper.UI
{
    public partial class MForm : Form
    {
        public MForm()
        {
            InitializeComponent();

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
                    Text = "couchase服务器"
                });
            }

            var parent = TVServerList.Nodes[0];
            parent.Nodes.Clear();
            foreach (var item in EntityTableEngine.LocalEngine.ListAll<CouchBaseServerEntity>(Global.TBName_RedisServer))
            {
                var newnode = new TreeNode
                {
                    Text = item.ServerName,
                    Tag = item
                };

                foreach (var hp in Utils.GetHostAndPoint(item.ConnStr))
                {
                    var hostnode = new TreeNode
                    {
                        Text = hp
                    };
                    hostnode.Tag = item;
                    newnode.Nodes.Add(hostnode);
                }

                parent.Nodes.Add(newnode);
            }
            parent.Expand();
        }

        private void TSM_NewRedis_Click(object sender, EventArgs e)
        {
            AddServerForm addform = new AddServerForm();
            if (addform.ShowDialog() == DialogResult.OK)
            {
                var parent = TVServerList.Nodes[0];
                var item = addform.NewServer;
                var newnode = new TreeNode
                {
                    Text = item.ServerName,
                    Tag = item
                };
                foreach (var hp in Utils.GetHostAndPoint(item.ConnStr))
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

        private void GetKeyType(string key, string connstr)
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
                if (MessageBox.Show("要删除所有的服务器吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                var subnodes = node.Nodes;
                foreach (TreeNode subnode in subnodes)
                {
                    if (EntityTableEngine.LocalEngine.Delete(Global.TBName_RedisServer, subnode.Text))
                    {
                        //subnode.Remove();
                    }
                }

                LoadRedisServers();
            }
            else if (node.Level == 1)
            {
                if (MessageBox.Show("要删除服务器" + node.Text + "?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
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
            
        }

        private void TSMSearchKey_Click(object sender, EventArgs e)
        {
            
        }

        private void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.PanelRight.Controls.Clear();
            var node = TVServerList.SelectedNode;
            if (node != null && node.Level == 2)
            {
                this.Text = ((CouchBaseServerEntity)node.Tag).ServerName;
                UC.DataViewUC dv = new UC.DataViewUC(new CouchBaseServerEntity
                {
                    ConnStr=node.Text,
                    ServerName = ((CouchBaseServerEntity)node.Tag).ServerName,
                    Buckets=((CouchBaseServerEntity)node.Tag).Buckets
                });
                dv.Dock=DockStyle.Fill;
                this.PanelRight.Controls.Add(dv);
            }
            else
            {

            }
        }

        private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UC.UCLog logview = new UC.UCLog();
            logview.Dock = DockStyle.Fill;
            this.PanelRight.Controls.Clear();
            this.PanelRight.Controls.Add(logview);

            logview.OnLogSelected += (log) =>
                {
                    this.PanelRight.Controls.Clear();
                    var server = EntityTableEngine.LocalEngine.Find<CouchBaseServerEntity>(Global.TBName_RedisServer, log.ServerName).FirstOrDefault();
                    if (server == null)
                    {
                        MessageBox.Show("服务已经删除:"+log.ServerName);
                        return;
                    }
                    this.Text = log.ServerName;
                    UC.DataViewUC dv = new UC.DataViewUC(new CouchBaseServerEntity
                    {
                        ConnStr = log.Connstr,
                        ServerName = log.ServerName,
                        Buckets = server.Buckets
                    });
                    dv.Key = log.Key;
                    dv.Dock = DockStyle.Fill;
                    this.PanelRight.Controls.Add(dv);
                };
        }
    }
}
