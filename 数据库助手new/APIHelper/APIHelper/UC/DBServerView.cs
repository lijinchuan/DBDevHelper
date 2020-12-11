using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Text.RegularExpressions;
using APIHelper.SubForm;
using Biz.Common;
using LJC.FrameWorkV3.Data.EntityDataBase;
using Biz;

namespace APIHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string, string> OnCreateSelectSql;
        public Action<string, LogicMap> OnDeleteLogicMap;

        public DBServerView()
        {
            InitializeComponent();
            ts_serchKey.Height = 20;
            tv_DBServers.ImageList = new ImageList();
            
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ForderClose);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ForderDB);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ForderOpen);
            tv_DBServers.Nodes.Add(new TreeNodeEx("资源管理器", 0, 1));
            tv_DBServers.BeforeExpand += Tv_DBServers_BeforeExpand;
            tv_DBServers.BeforeCollapse += Tv_DBServers_BeforeCollapse;
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);
            tv_DBServers.NodeMouseDoubleClick += Tv_DBServers_NodeMouseDoubleClick;
            
            tv_DBServers.HideSelection = false;
            this.DBServerviewContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuStrip_ItemClicked);
        }

        private void Tv_DBServers_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Node is Biz.TreeNodeEx)
            {
                e.Node.ImageIndex= e.Node.SelectedImageIndex = (e.Node as Biz.TreeNodeEx).CollapseImgIndex;
            }
        }

        private void Tv_DBServers_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is Biz.TreeNodeEx)
            {
                e.Node.ImageIndex=e.Node.SelectedImageIndex = (e.Node as Biz.TreeNodeEx).ExpandImgIndex;
            }
        }

        void ReLoadDBObj(TreeNode selNode)
        {
            //TreeNode selNode = tv_DBServers.SelectedNode;
            if (selNode == null)
                return;
            if (selNode.Level == 0)
            {
                Biz.UILoadHelper.LoadApiResurceAsync(this.ParentForm, selNode);
            }
            else if (selNode.Tag is APISource)
            {

            }
            else if (selNode.Tag is NodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.APIPARENT)
            {
                var sid = (selNode.Parent.Tag as APISource).Id;
                Biz.UILoadHelper.LoadApiAsync(this.ParentForm, selNode, sid);
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.LOGICMAPParent)
            {
                Biz.UILoadHelper.LoadLogicMapsAnsy(this.ParentForm, selNode, GetDBName(selNode));
            }
        }

        void OnMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                DBServerviewContextMenuStrip.Visible = false;
                var selnode = tv_DBServers.SelectedNode;
                if (selnode == null)
                {
                    return;
                }
                switch (e.ClickedItem.Text)
                {
                    case "添加API资源":
                        {
                            if(new SubForm.AddAPISource().ShowDialog() == DialogResult.OK)
                            {
                                Bind();
                            }
                            break;
                        }
                    case "编辑":
                        {
                            if(selnode.Tag is APISource)
                            {
                                if (new SubForm.AddAPISource((selnode.Tag as APISource).Id).ShowDialog() == DialogResult.OK)
                                {
                                    Bind();
                                }
                            }
                            break;
                        }
                    case "删除":
                        {
                            if (selnode.Tag is APISource && MessageBox.Show("要删除吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                if (BigEntityTableEngine.LocalEngine.Delete<APISource>(nameof(APISource), (selnode.Tag as APISource).Id))
                                {
                                    Bind();
                                }
                            }
                            break;
                        }
                    case "添加API":
                        {
                            var apisource = selnode.Parent.Tag as APISource;
                            var sourceid= apisource.Id;
                            var step1dlg = new SubForm.AddAPIStep1Dlg(sourceid);
                            if (step1dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.ReLoadDBObj(selnode);
                                Util.AddToMainTab(this,$"[{apisource.SourceName}]{step1dlg.APIUrl.APIName}", new UC.UCAddAPI(step1dlg.APIUrl));
                            }
                            break;
                        }
                    default:
                        {
                            MessageBox.Show(e.ClickedItem.Text);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private string GetDBName(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Level < 1)
                return null;
            if (node.Level == 2)
                return node.Text;
            return GetDBName(node.Parent);
        }

        void tv_DBServers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                if (e.Node.Tag is APIUrl)
                {
                    var apiurl = e.Node.Tag as APIUrl;
                    var source = e.Node.Parent.Parent.Tag as APISource;
                    Util.AddToMainTab(this, $"[{source.SourceName}]{apiurl.APIName}", new UC.UCAddAPI(apiurl));
                }
                else
                {
                    ReLoadDBObj(e.Node);
                }
            }
        }


        private void Tv_DBServers_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.UNKNOWN)
            {
            }
        }

        public void Bind()
        {
            ReLoadDBObj(tv_DBServers.Nodes[0]);
        }

        private void DBServerView_Load(object sender, EventArgs e)
        {
            Bind();
        }

        public string DisConnectSelectDBServer()
        {
            if (this.tv_DBServers.SelectedNode == null || this.tv_DBServers.SelectedNode.Level != 1)
                return null;
            var server = this.tv_DBServers.SelectedNode.Text;
            DisConnectServer(server);
            return server;
        }

        private void DisConnectServer(string serverName)
        {

        }

        private void tv_DBServers_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node = tv_DBServers.SelectedNode;
                this.tv_DBServers.ContextMenuStrip = this.DBServerviewContextMenuStrip;

                添加API资源ToolStripMenuItem.Visible = node.Level == 0;

                添加APIToolStripMenuItem.Visible = (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.APIPARENT;

                修改ToolStripMenuItem.Visible = node.Tag is APISource;
                删除ToolStripMenuItem.Visible = node.Tag is APISource;

                
            }

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            string serchkey = ts_serchKey.Text;

            bool matchall = serchkey.StartsWith("'");
            if (matchall)
            {
                serchkey = serchkey.Trim('\'');
            }
            if (!ts_serchKey.Items.Contains(serchkey))
            {
                ts_serchKey.Items.Add(serchkey);
            }
            if (tv_DBServers.SelectedNode == null)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }

            if (!tv_DBServers.Focused)
            {
                this.tv_DBServers.Focus();
            }

            bool boo = false;
            if (tv_DBServers.SelectedNode.Nodes.Count > 0)
                boo = SearchNode(tv_DBServers.SelectedNode.Nodes[0], serchkey, matchall, true);
            else if (tv_DBServers.SelectedNode.NextNode != null)
                boo = SearchNode(tv_DBServers.SelectedNode.NextNode, serchkey, matchall, true);
            else
            {
                var parent = tv_DBServers.SelectedNode.Parent;
                while (parent != null && parent.NextNode == null)
                {
                    parent = parent.Parent;
                }
                if (parent != null)
                {
                    if (parent.NextNode != null)
                    {
                        boo = SearchNode(parent.NextNode, serchkey, matchall, true);
                    }
                }
            }

            if (!boo)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }

        }

        private bool SearchNode(TreeNode nodeStart, string txt, bool matchall, bool maxsearch)
        {
            if (nodeStart == null)
            {
                return false;
            }
            var find = matchall ? nodeStart.Text.Equals(txt, StringComparison.OrdinalIgnoreCase) : nodeStart.Text.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1;
            if (find)
            {
                tv_DBServers.SelectedNode = nodeStart;
                return true;
            }
            if (nodeStart.Nodes.Count > 0)
            {
                foreach (TreeNode node in nodeStart.Nodes)
                {
                    if (SearchNode(node, txt, matchall, false))
                        return true;
                }
            }

            if (maxsearch)
            {
                if (nodeStart.NextNode != null)
                {
                    return SearchNode(nodeStart.NextNode, txt, matchall, true);
                }
                else
                {
                    if (maxsearch)
                    {
                        var parent = nodeStart.Parent;
                        while (parent != null && parent.NextNode == null)
                        {
                            parent = parent.Parent;
                        }
                        if (parent != null)
                        {
                            return SearchNode(parent.NextNode, txt, matchall, true);
                        }
                    }
                }
            }

            return false;
        }

        private void ts_serchKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                tv_DBServers.Focus();
                toolStripDropDownButton1_Click(null, null);
            }
        }

        public TreeNode FindNode(string serverName, string dbName = null, string tbName = null)
        {
            foreach (TreeNode node in tv_DBServers.Nodes[0].Nodes)
            {
                if (node.Text.Equals(serverName, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(dbName))
                        return node;
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        if (subNode.Text.Equals(dbName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (string.IsNullOrWhiteSpace(tbName))
                                return subNode;
                            foreach (TreeNode subSubNode in subNode.Nodes)
                            {
                                if (subSubNode.Text.Equals(tbName, StringComparison.OrdinalIgnoreCase))
                                    return subSubNode;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void SubMenuItem_Insert_Click(object sender, EventArgs e)
        {

        }

        private void Mark_Local()
        {
            

        }

        private void 备注本地ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mark_Local();
        }

        private void MarkResource()
        {
           
        }

        private void ClearMarkResource()
        {
           
        }

        private void 新增逻辑关系图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currnode = this.tv_DBServers.SelectedNode;
            if (currnode == null)
            {
                return;
            }

            while (true)
            {
                SubForm.InputStringDlg dlg = new InputStringDlg("输入逻辑关系图名称");
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var db = GetDBName(currnode).ToUpper();
                    var name = dlg.InputString.ToUpper();
                    long total = 0;
                    if (BigEntityTableEngine.LocalEngine.Scan<LogicMap>(nameof(LogicMap), "DB_LogicName",
                        new[] { db, name }, new[] { db, name }, 1, 1, ref total).Count > 0)
                    {
                        MessageBox.Show("名称不能重复");
                    }
                    else
                    {
                        var logicmap = new LogicMap
                        {
                            DBName=db,
                            LogicName=dlg.InputString
                        };
                        BigEntityTableEngine.LocalEngine.Insert<LogicMap>(nameof(LogicMap), logicmap);
                        ReLoadDBObj(currnode);
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
        }

        private void 删除逻辑关系图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currnode = this.tv_DBServers.SelectedNode;
            if(currnode!=null&&currnode.Tag is LogicMap)
            {
                var logicmap = currnode.Tag as LogicMap;
                if (MessageBox.Show($"要删除逻辑关系图【{logicmap.LogicName}】吗？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BigEntityTableEngine.LocalEngine.Delete<LogicMap>(nameof(LogicMap), logicmap.ID);
                    if (this.OnDeleteLogicMap != null)
                    {
                        this.OnDeleteLogicMap(GetDBName(currnode), logicmap);
                    }
                    ReLoadDBObj(currnode.Parent);
                }
            }
        }

        private void TTSM_CreateIndex_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
        }

        private void TTSM_DelIndex_Click(object sender, EventArgs e)
        {
        }
    }
}
