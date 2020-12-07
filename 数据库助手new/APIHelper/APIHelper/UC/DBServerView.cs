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

namespace APIHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string, string> OnCreateSelectSql;
        public Action<string, LogicMap> OnDeleteLogicMap;
        /// <summary>
        /// 实体命名空间
        /// </summary>
        private static string DefaultEntityNamespace = "Nonamespace";

        public DBServerView()
        {
            InitializeComponent();
            ts_serchKey.Height = 20;
            tv_DBServers.ImageList = new ImageList();
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB1);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB2);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB3);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB4);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB5);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB6);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB7);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB8);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB9);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB10);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB11);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.param);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.paramout);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code); //13
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB16);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB161); //16
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.loading);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ColQ); //18
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_no);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red_no);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.logic);
            tv_DBServers.ImageList.Images.Add("ASC", Resources.Resource1.ASC);
            tv_DBServers.ImageList.Images.Add("DESC", Resources.Resource1.DESC);
            tv_DBServers.ImageList.Images.Add("plugin", Resources.Resource1.plugin);
            tv_DBServers.Nodes.Add("0", "资源管理器", 0);
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);
            tv_DBServers.NodeMouseDoubleClick += Tv_DBServers_NodeMouseDoubleClick;
            tv_DBServers.HideSelection = false;
            this.DBServerviewContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuStrip_ItemClicked);
        }

       

        void ReLoadDBObj(TreeNode selNode)
        {
            //TreeNode selNode = tv_DBServers.SelectedNode;
            if (selNode == null)
                return;
            if ((selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.TBParent)
            {

            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.PROCParent)
            {

            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.VIEWParent)
            {
               
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.INDEXParent)
            {
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
                switch (e.ClickedItem.Text)
                {
                    case "复制对象名":
                        if (this.tv_DBServers.SelectedNode != null)
                        {
                            Clipboard.SetText(tv_DBServers.SelectedNode.Text);
                        }
                        break;
                    case "删除表":
                        break;
                    case "刷新":
                        ReLoadDBObj(tv_DBServers.SelectedNode);
                        break;
                    case "修改表名":
                        break;
                    case "Delete":

                        break;
                    case "Select":

                        break;
                    case "创建语句":
                        MessageBox.Show("Create");
                        break;
                    case "同步数据":
                        {
                            break;
                        }
                    case "备注":
                        {
                            MarkResource();
                            break;
                        }
                    case "清理本地缓存":
                        {
                            ClearMarkResource();
                            break;
                        }
                    case "生成调用代码":
                        {

                            break;
                        }
                    default:
                        break;
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
            
        }


        private void Tv_DBServers_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.LOGICMAP)
            {
            }
        }

        public void Bind()
        {
            
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
                if (node.Level == 0)
                {
                    this.tv_DBServers.ContextMenuStrip = null;
                    return;
                }
                if (node != null)
                {
                    var nc = node.Tag as INodeContents;
                    NodeContentType nctype = NodeContentType.UNKNOWN;
                    if (nc != null)
                    {
                        nctype = nc.GetNodeContentType();
                    }

                    if (nctype == NodeContentType.TB
                        || nctype == NodeContentType.PROC
                        || nctype == NodeContentType.VIEW
                        || nctype == NodeContentType.INDEX
                        || nctype == NodeContentType.INDEXParent
                        || nctype == NodeContentType.LOGICMAPParent
                        ||nctype==NodeContentType.LOGICMAP)
                    {
                        this.tv_DBServers.ContextMenuStrip = this.DBServerviewContextMenuStrip;
                        foreach (ToolStripItem item in DBServerviewContextMenuStrip.Items)
                        {
                            if (item is ToolStripItem)
                            {
                                continue;
                            }
                            item.Visible = false;
                        }


                        刷新ToolStripMenuItem.Visible = nctype == NodeContentType.DBParent
                            || nctype == NodeContentType.DB
                            || nctype == NodeContentType.TBParent
                            || nctype == NodeContentType.TB
                            || nctype == NodeContentType.INDEXParent
                            || nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.VIEWParent
                            || nctype == NodeContentType.PROCParent
                            || nctype == NodeContentType.PROC
                            || nctype == NodeContentType.LOGICMAPParent;

                        复制表名ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB
                            || nctype == NodeContentType.INDEX
                            || nctype == NodeContentType.LOGICMAP;
                        清理备注ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        备注ToolStripMenuItem.Visible = nctype == NodeContentType.TB
                            || nctype == NodeContentType.PROC;
                        新增逻辑关系图ToolStripMenuItem.Visible = nctype == NodeContentType.LOGICMAPParent;
                        删除逻辑关系图ToolStripMenuItem.Visible = nctype == NodeContentType.LOGICMAP;
                    }
                }
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
