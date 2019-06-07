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
using NETDBHelper.SubForm;
using Biz.Common;
using Biz.Common.Data;

namespace NETDBHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string,string> OnCreateEntity;
        public Action<DBSource,string, string> OnShowTableData;
        public Action<DBSource,string> OnAddEntityTB;
        public Action<string, string> OnCreateSelectSql;
        public Action<DBSource, string, string, string,CreateProceEnum> OnCreatePorcSQL;
        private DBSourceCollection _dbServers;
        /// <summary>
        /// 实体命名空间
        /// </summary>
        private static string DefaultEntityNamespace = "Nonamespace";

        public DBSourceCollection DBServers
        {
            get
            {
                return _dbServers;
            }
        }
        public DBServerView()
        {
            InitializeComponent();
            ts_serchKey.Height = 20;
            _dbServers = new DBSourceCollection();
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
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB16);
            tv_DBServers.Nodes.Add("0", "资源管理器", 0);
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);

            this.DBServerviewContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuStrip_ItemClicked);
            this.CommMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(CommMenuStrip_ItemClicked);
            this.CommSubMenuitem_ReorderColumn.DropDownItemClicked += CommSubMenuitem_ReorderColumn_DropDownItemClicked;
        }

        void CommSubMenuitem_ReorderColumn_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var selnode = tv_DBServers.SelectedNode;
            if (selnode == null)
                return;
            var newnode = (TreeNode)selnode.Clone();
            switch (e.ClickedItem.Text)
            {
                case "上移":
                    if (selnode.Index == 0)
                        return;
                    selnode.Parent.Nodes.Insert(selnode.Index - 1, newnode);
                    selnode.Remove();
                    break;
                case "下移":
                    if (selnode.Index == selnode.Parent.Nodes.Count-1)
                        return;
                    selnode.Parent.Nodes.Insert(selnode.Index + 2, newnode);
                    selnode.Remove();
                    break;

            }
            tv_DBServers.SelectedNode = newnode;
        }

        void CommMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "刷新":
                        ReLoadDBObj(tv_DBServers.SelectedNode);
                        break;
                    case "复制对象名":
                        if (this.tv_DBServers.SelectedNode != null)
                        {
                            string s = tv_DBServers.SelectedNode.Text;
                            if (s.IndexOf('(') > -1)
                                Clipboard.SetText(s.Substring(0, s.IndexOf('(')));
                            else
                                Clipboard.SetText(s);
                        }
                        break;
                    case "添加实体映射表":
                        if (OnAddEntityTB != null)
                        {
                            var node = tv_DBServers.SelectedNode;
                            if (node == null)
                                return;
                            OnAddEntityTB(GetDBSource(node), node.Text);
                        }
                        break;
                    case "删除对象":
                        if (MessageBox.Show("确认要删除数据库" + tv_DBServers.SelectedNode.Text + "吗？", "询问",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            var node = tv_DBServers.SelectedNode;
                            Biz.Common.Data.SQLHelper.DeleteDataBase(GetDBSource(node), node.Text);
                            ReLoadDBObj(node.Parent);
                        }
                        break;
                    case "新增对象":
                        var selnode = tv_DBServers.SelectedNode;
                        var dlg = new SubForm.InputStringDlg("请输入库名：");
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            Biz.Common.Data.SQLHelper.CreateDataBase(GetDBSource(selnode),selnode.FirstNode.Text, dlg.InputString);
                            ReLoadDBObj(selnode);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        void ReLoadDBObj(TreeNode selNode)
        {
            //TreeNode selNode = tv_DBServers.SelectedNode;
            if (selNode == null)
                return;
            if (selNode.Level == 1)
            {
                Biz.UILoadHelper.LoadDBsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 2)
            {
                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 3 && selNode.Text.Equals("存储过程"))
            {
                Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 3 && selNode.Text.Equals("视图"))
            {
                Biz.UILoadHelper.LoadViewsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 3)
            {
                Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 4 && selNode.Text.Equals("索引"))
            {
                Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
        }

        void OnMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "生成实体类":
                        CreateEntityClass();
                        break;
                    case "显示前100条数据":
                        ShowTop100Data();
                        break;
                    case "复制对象名":
                        if (this.tv_DBServers.SelectedNode != null)
                        {
                            Clipboard.SetText(tv_DBServers.SelectedNode.Text);
                        }
                        break;
                    case "删除表":
                        if (MessageBox.Show("确认要删除表" + tv_DBServers.SelectedNode.Text + "吗？", "询问",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            var node = tv_DBServers.SelectedNode;
                            Biz.Common.Data.SQLHelper.DeleteTable(GetDBSource(node), node.Parent.Text, node.Text);
                            ReLoadDBObj(node.Parent);
                        }
                        break;
                    case "刷新":
                        ReLoadDBObj(tv_DBServers.SelectedNode);
                        break;
                    case "修改表名":
                        var _node=tv_DBServers.SelectedNode;
                        var oldname = _node.Text;
                        var dlg=new SubForm.InputStringDlg("修改表名:", _node.Text);
                        if ( dlg.ShowDialog()== DialogResult.OK)
                        {
                            if (string.Equals(dlg.InputString, oldname, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                            Biz.Common.Data.SQLHelper.ReNameTableName(GetDBSource(_node), _node.Parent.Text,
                                oldname, dlg.InputString);
                            ReLoadDBObj(_node.Parent);
                        }
                        break;
                    case "Insert":
                        _node=tv_DBServers.SelectedNode;
                        if (this.OnCreatePorcSQL != null)
                        {
                            this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text,CreateProceEnum.Insert);
                        }
                        break;
                    case "BatchInsert":
                        _node=tv_DBServers.SelectedNode;
                        if (this.OnCreatePorcSQL != null)
                        {
                            this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text,CreateProceEnum.BatchInsert);
                        }
                        break;
                    case "Delete":

                        break;
                    case "Select":

                        break;
                    case "创建语句":
                        MessageBox.Show("Create");
                        break;
                    default:
                        _node = tv_DBServers.SelectedNode;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        void ShowTop100Data()
        {
            if (tv_DBServers.SelectedNode != null && tv_DBServers.SelectedNode.Level == 3)
            {
                List<KeyValuePair<string, bool>> cols = new List<KeyValuePair<string, bool>>();
                foreach (TreeNode node in tv_DBServers.SelectedNode.Nodes)
                {
                    if (node.Text == "索引" && node == tv_DBServers.SelectedNode.LastNode)
                    {
                        continue;
                    }
                    cols.Add(new KeyValuePair<string, bool>(node.Text.Substring(0,node.Text.IndexOf('(')), (node.Tag as TBColumn).IsKey));
                }
                StringBuilder sb = new StringBuilder("select top 100 ");
                sb.AppendLine();
                sb.Append(string.Join(",\r\n", cols.Select(p =>"["+p.Key+"]")));
                sb.AppendLine("");
                sb.Append(" from ");
                sb.Append(tv_DBServers.SelectedNode.Text);
                sb.Append("(nolock)");
                if (this.OnShowTableData != null)
                {
                    OnShowTableData(this.tv_DBServers.SelectedNode.Parent.Parent.Tag as DBSource,this.tv_DBServers.SelectedNode.Parent.Text, sb.ToString());
                }
            }
        }

        void CreateEntityClass()
        {
            if (tv_DBServers.SelectedNode != null && tv_DBServers.SelectedNode.Level == 3)
            {
                //命名空间
                SubForm.CreateEntityNavDlg dlg = new CreateEntityNavDlg("请输入实体命名空间", DefaultEntityNamespace);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DefaultEntityNamespace = dlg.InputString;
                }

                bool isSupportProtobuf = dlg.SupportProtobuf;
                bool isSupportDBMapperAttr = dlg.SupportDBMapperAttr;
                bool isSupportJsonproterty = dlg.SupportJsonproterty;
                bool isSupportMvcDisplay = dlg.SupportMvcDisplay;
                bool hasKey = false;

                var dbsource = GetDBSource(tv_DBServers.SelectedNode);
                var tbname = tv_DBServers.SelectedNode.Text;
                string dbname = tv_DBServers.SelectedNode.Parent.Text;
                var tid = tv_DBServers.SelectedNode.Name;

                var classcode = DataHelper.CreateTableEntity(dbsource, dbname, tbname, tid, DefaultEntityNamespace,
                    isSupportProtobuf, isSupportDBMapperAttr, isSupportJsonproterty, isSupportMvcDisplay, out hasKey);

                if (OnCreateEntity != null)
                {
                    OnCreateEntity("实体类" + tbname, classcode);
                }
                Clipboard.SetText(classcode);
                MainFrm.SendMsg(string.Format("实体代码已经复制到剪贴板,{0}", hasKey ? "" : "警告：表没有自增主键。"));

            }
        }

        private DBSource GetDBSource(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Level < 1)
                return null;
            if (node.Level == 1)
                return DBServers.FirstOrDefault(p => p.ServerName.Equals(node.Text));
            return GetDBSource(node.Parent);
        }

        void tv_DBServers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Node.Level == 2)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                //var server=DBServers.FirstOrDefault(p=>p.ServerName.Equals(e.Node.Parent.Text));
                //if (server == null)
                //    return;
                //DataTable tb= Biz.Common.Data.SQLHelper.GetTBs(server, e.Node.Text);
                //for (int i = 0; i < tb.Rows.Count; i++)
                //{
                //    TreeNode newNode = new TreeNode(tb.Rows[i]["name"].ToString(),3, 3);
                //    newNode.Name=tb.Rows[i]["id"].ToString();
                //    e.Node.Nodes.Add(newNode);
                //}
                //e.Node.Expand();
                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));
            }
            if (e.Node.Level == 3)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Parent.Text));
                //if (server == null)
                //    return;
                //foreach(TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(server,e.Node.Parent.Text,e.Node.Name))
                //{
                //    int imgIdx = col.IsKey ? 4 : 5;
                //    TreeNode newNode = new TreeNode(string.Concat(col.Name,"(",col.TypeName,")"),imgIdx, imgIdx);
                //    newNode.Tag = col;
                //    e.Node.Nodes.Add(newNode);
                //}
                //e.Node.Expand();
                if (e.Node.Text.Equals("存储过程"))
                {
                    Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));

                }
                if (e.Node.Text.Equals("视图"))
                {
                    Biz.UILoadHelper.LoadViewsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));

                }
                else
                {
                    Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));
                }
            }
            else if (e.Node.Level == 4)
            {
                if (e.Node.Nodes.Count > 0)
                    return;

                if (e.Node.Text.Equals("索引"))
                {
                    Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));
                }
            }
        }
        public void Bind()
        {
            if (DBServers == null)
                return;
            foreach (DBSource s in DBServers)
            {
                bool isAdd = false;
                foreach (TreeNode n in tv_DBServers.Nodes[0].Nodes)
                {
                    if (n.Text.Equals(s.ServerName))
                    {
                        isAdd = true;
                        break;
                    }
                }
                if (isAdd)
                    continue;
                if (tv_DBServers.Nodes[0].Nodes.ContainsKey(s.ServerName))
                    continue;
               TreeNode node = new TreeNode(s.ServerName,1,1);
               node.Tag = s;
               tv_DBServers.Nodes[0].Nodes.Add(node);
               DataTable table= Biz.Common.Data.SQLHelper.GetDBs(s);
               for (int i = 0; i < table.Rows.Count; i++)
               {
                   TreeNode tbNode = new TreeNode(table.Rows[i]["Name"].ToString(), 2, 2);
                   node.Nodes.Add(tbNode);
               }
            }
        }

        private void DBServerView_Load(object sender, EventArgs e)
        {
            Bind();
        }

        public void DisConnectSelectDBServer()
        {
            if (this.tv_DBServers.SelectedNode==null||this.tv_DBServers.SelectedNode.Level != 1)
                return;
            DisConnectServer(this.tv_DBServers.SelectedNode.Text);
        }

        private void DisConnectServer(string serverName)
        {
            this.DBServers.Remove(this.DBServers.FirstOrDefault(p=>p.ServerName.Equals(serverName)));
            foreach (TreeNode node in tv_DBServers.Nodes[0].Nodes)
            {
                if (node.Text.Equals(serverName))
                {
                    tv_DBServers.Nodes[0].Nodes.Remove(node);
                    break;
                }
            }
        }

        private void tv_DBServers_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node=tv_DBServers.SelectedNode;
                if (node != null)
                {
                    if ((tv_DBServers.SelectedNode.Level == 3 && !tv_DBServers.SelectedNode.Text.Equals("存储过程"))
                        || (tv_DBServers.SelectedNode.Level == 4 && tv_DBServers.SelectedNode.Parent.Text.Equals("存储过程"))
                        || (tv_DBServers.SelectedNode.Level == 4 && tv_DBServers.SelectedNode.Parent.Text.Equals("视图"))
                        || (tv_DBServers.SelectedNode.Level == 5 && tv_DBServers.SelectedNode.Parent.Text.Equals("索引")))
                    {
                        this.tv_DBServers.ContextMenuStrip = this.DBServerviewContextMenuStrip;
                        if (tv_DBServers.SelectedNode.Parent.Text.Equals("存储过程")
                            || tv_DBServers.SelectedNode.Parent.Text.Equals("视图"))
                        {
                            foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = false;
                            }

                            导出ToolStripMenuItem.Visible = true;
                            foreach(ToolStripItem ts in 导出ToolStripMenuItem.DropDownItems)
                            {
                                if (ts != CreateMSSQLToolStripMenuItem)
                                {
                                    ts.Visible = false;
                                }
                            }
                            复制表名ToolStripMenuItem.Visible = true;
                        }
                        else if (tv_DBServers.SelectedNode.Parent.Text.Equals("索引"))
                        {
                            foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = false;
                            }
                            //TSM_ManIndex.Visible = true;
                        }
                        else if (tv_DBServers.SelectedNode.Text.Equals("视图"))
                        {
                            foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = false;
                            }
                            //TSM_ManIndex.Visible = true;
                            刷新ToolStripMenuItem.Visible = true;
                        }
                        else
                        {
                            foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = true;
                                ExpdataToolStripMenuItem.Visible = false;
                            }
                        }
                        //TTSM_CreateIndex.Visible = node.Level == 3;
                        //TTSM_DelIndex.Visible = node.Level == 5 && node.Parent.Text.Equals("索引");

                        ExpdataToolStripMenuItem.Visible = node.Level == 3;
                    }
                    else if (tv_DBServers.SelectedNode.Text.Equals("索引"))
                    {
                        foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                        {
                            item.Visible = false;
                        }
                        刷新ToolStripMenuItem.Visible = true;
                        //TSM_ManIndex.Visible = true;
                    }
                    else
                    {
                        this.tv_DBServers.ContextMenuStrip = this.CommMenuStrip;
                        subMenuItemAddEntityTB.Visible=node.Level==2;
                        CommSubMenuItem_Delete.Visible = node.Level ==2;
                        CommSubMenuitem_add.Visible = node.Level == 1;
                        CommSubMenuitem_ViewConnsql.Visible = node.Level == 2;
                        CommSubMenuitem_ReorderColumn.Visible = node.Level == 4;
                    }
                }
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            string serchkey = ts_serchKey.Text;
            if (!ts_serchKey.Items.Contains(serchkey))
            {
                ts_serchKey.Items.Add(serchkey);
            }
            if (tv_DBServers.SelectedNode == null)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }
            bool boo = false;
            if (tv_DBServers.SelectedNode.Nodes.Count > 0)
                boo=SearchNode(tv_DBServers.SelectedNode.Nodes[0], serchkey);
            else if (tv_DBServers.SelectedNode.NextNode != null)
                boo=SearchNode(tv_DBServers.SelectedNode.NextNode, serchkey);
            if (!boo)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }
        }

        private bool SearchNode(TreeNode nodeStart, string txt)
        {
            if (nodeStart == null)
            {
                return false;
            }
            if (nodeStart.Text.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1)
            {
                tv_DBServers.SelectedNode = nodeStart;
                return true;
            }
            if (nodeStart.Nodes.Count > 0)
            {
                foreach (TreeNode node in nodeStart.Nodes)
                {
                    if (SearchNode(node, txt))
                        return true;
                }
            }
            if (nodeStart.NextNode != null)
            {
                return SearchNode(nodeStart.NextNode,txt);
            }
            if (nodeStart.Parent != null)
            {
                if (nodeStart.Parent.NextNode != null)
                {
                    return SearchNode(nodeStart.Parent.NextNode, txt);
                }
            }
            if (tv_DBServers.Nodes.Count > 0)
            {
                tv_DBServers.SelectedNode=tv_DBServers.Nodes[0];
            }
            return true;
        }

        private void ts_serchKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                tv_DBServers.Focus();
                toolStripDropDownButton1_Click(null, null);
            }
        }

        public TreeNode FindNode(string serverName, string dbName=null, string tbName=null)
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
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.Insert);
            }
        }

        private void 创建语句ToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || node.Level != 3)
            {
                return;
            }
            StringBuilder sb = new StringBuilder(string.Format("CREATE TABLE `{0}`(",node.Text));
            sb.AppendLine();
            foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), node.Parent.Text, node.Name, node.Text))
            {
                sb.AppendFormat("`{0}` {1} {2} {3},", col.Name, Biz.Common.Data.Common.GetDBType(col), (col.IsID||col.IsKey) ? "NOT NULL" : (col.IsNullAble ? "NULL" : "NOT NULL"),col.IsID?"AUTO_INCREMENT":"");
                if (col.IsID)
                {
                    sb.AppendLine();
                    sb.AppendFormat("PRIMARY KEY (`{0}`),",col.Name);
                }
                sb.AppendLine();
            }
            sb.AppendLine("`last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            sb.AppendLine(")ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=utf8;");
            sb.AppendLine("//注意：bit类型要手工改成TINYINT(1)。");
            TextBoxWin win = new TextBoxWin("创建表"+node.Text, sb.ToString());
            win.Show();
        }

        private void ExpdataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isNotExportKey = false;
            //SET IDENTITY_INSERT
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || node.Level != 3)
            {
                return;
            }
            var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), node.Parent.Text, node.Name, node.Text);
            if(isNotExportKey)
            {
                cols=cols.Where(p => !p.IsID);
            }
            cols = cols.OrderBy(p => p.IsID ? 0 : 1);

            string sqltext = string.Format("select {0} from {1}(nolock)", string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))), string.Concat("[", node.Text, "]"));
            var datas = Biz.Common.Data.SQLHelper.ExecuteDBTable(GetDBSource(node), node.Parent.Text, sqltext, null);
            StringBuilder sb = new StringBuilder();
            if(!isNotExportKey)
            {
                sb.AppendLine("SET IDENTITY_INSERT");
            }
            sb.AppendFormat("Insert into {0} ({1}) values", string.Concat("`", node.Text, "`"), string.Join(",", cols.Select(p => string.Concat("`", p.Name, "`"))));
            foreach (DataRow row in datas.Rows)
            {
                StringBuilder sb1 = new StringBuilder();
                foreach (var column in cols)
                {
                    object data = row[column.Name];
                    if (data == DBNull.Value)
                    {
                        sb1.Append("NULL,");
                    }
                    else
                    {
                        if (column.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("float", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                        )
                        {
                            sb1.AppendFormat("{0},", data);
                        }
                        else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("'{0}',",((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            sb1.Append(string.Concat("'", data, "',"));
                        }
                    }
                }
                if (sb1.Length > 0)
                    sb1.Remove(sb1.Length - 1, 1);
                sb.AppendFormat("({0}),", sb1.ToString());
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            TextBoxWin win = new TextBoxWin("导出数据", sb.ToString());
            win.Show();
        }

        private void SubMenuItem_Select_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || node.Level != 3)
            {
                return;
            }

            DBSource dbsource = GetDBSource(node);
            string dbname = node.Parent.Text,
                tbname = node.Text, tid = node.Name;

            var cols = Biz.Common.Data.SQLHelper.GetColumns(dbsource, dbname, tid, tbname).ToList();

            SubForm.WinCreateSelectSpNav nav = new WinCreateSelectSpNav(cols);
            
            if(nav.ShowDialog()==DialogResult.Cancel)
            {
                return;
            }

            var conditioncols = nav.ConditionColumns;
            var outputcols = nav.OutPutColumns;

            string codes = DataHelper.CreateSelectSql(dbname, tbname, nav.Editer, nav.SPAbout, cols, conditioncols, outputcols);

            if(OnCreateSelectSql!=null)
            {
                OnCreateSelectSql(string.Format("查询[{0}.{1}]",dbname,tbname), codes);
            }

            //SubForm.WinWebBroswer webdlg = new WinWebBroswer();
            //webdlg.SetTitle("生成存储过程").SetBody(codes);
            //webdlg.ShowDialog();
        }

        private void CreateMSSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node != null && node.Level == 3)
            {
                StringBuilder sb = new StringBuilder(string.Format("Use [{0}]", node.Parent.Text));
                sb.AppendLine();
                sb.AppendLine("Go");
                sb.Append(@"SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO");
                sb.AppendLine();
                sb.AppendLine(string.Format("CREATE TABLE dbo.[{0}](", node.Text));
                //sb.AppendLine();
                List<string> keys = new List<string>();
                foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), node.Parent.Text, node.Name, node.Text))
                {
                    sb.AppendFormat("[{0}] {1} {2} {3},", col.Name, Biz.Common.Data.Common.GetDBType(col), (col.IsID || col.IsKey) ? "NOT NULL" : (col.IsNullAble ? "NULL" : "NOT NULL"), col.IsID ? "IDENTITY(1,1)" : "");
                    sb.AppendLine();
                    if (col.IsKey)
                    {
                        keys.Add(col.Name);
                    }
                }
                sb.AppendLine(")");

                if (keys.Count > 0)
                {
                    sb.AppendLine("alter table " + node.Text + " add constraint pk_" + string.Join("_", keys) + "_1 primary key(" + string.Join(",", keys) + ")");

                }
                sb.AppendLine("Go");
                TextBoxWin win = new TextBoxWin("创建表" + node.Text, sb.ToString());
                win.Show();
            }
            else if (node != null && node.Level == 4 && node.Parent.Text.Equals("存储过程"))
            {
                var body = Biz.Common.Data.SQLHelper.GetProcedureBody(GetDBSource(node), node.Parent.Parent.Text, node.Text);
                TextBoxWin win = new TextBoxWin("存储过程[" + node.Text + "]", body);
                win.Show();
            }
            else if (node != null && node.Level == 4 && node.Parent.Text.Equals("视图"))
            {
                var body = Biz.Common.Data.SQLHelper.GetViewCreateSql(GetDBSource(node), node.Parent.Parent.Text, node.Text);
                TextBoxWin win = new TextBoxWin("视图[" + node.Text + "]", body);
                win.Show();
            }
        }

        //导出数据
        private void 数据MSSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || node.Level != 3)
            {
                return;
            }

            InputStringDlg dlg = new InputStringDlg("要导出的数据量","1000");
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            bool notExportId = true;
            
            

            int topNum = 1000;
            int.TryParse(dlg.InputString,out topNum);

            var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), node.Parent.Text, node.Name, node.Text);
            if(cols.ToList().Exists(p=>p.IsID))
            {
                if (MessageBox.Show("是否要导出自增列数据，如果导出会删除原来的数据。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    notExportId = false;
                }
            }
            if(notExportId)
            {
                cols = cols.Where(p => !p.IsID);
            }
            cols = cols.OrderBy(p => p.IsID ? 0 : 1);

            string sqltext = string.Format("select top {2} {0} from {1}(nolock)", string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))), string.Concat("[", node.Text, "]"), topNum);
            var datas = Biz.Common.Data.SQLHelper.ExecuteDBTable(GetDBSource(node), node.Parent.Text, sqltext, null);
            StringBuilder sb = new StringBuilder();
            if(!notExportId)
            {
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} ON", string.Concat("[", node.Text, "]")));
                sb.AppendLine("GO");
                sb.AppendLine(string.Format("delete from {0}", string.Concat("[", node.Text, "]")));
                sb.AppendLine(string.Format("DBCC CHECKIDENT({0},RESEED,0)", string.Concat("[", node.Text, "]")));
                sb.AppendLine("GO");
            }
            sb.AppendFormat("Insert into {0} ({1})  ", string.Concat("[", node.Text, "]"), string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))));
            int idx = 0;
            foreach (DataRow row in datas.Rows)
            {
                idx++;
                StringBuilder sb1 = new StringBuilder(idx>1?" union select ":"select ");
                foreach (var column in cols)
                {
                    object data = row[column.Name];
                    if (data == DBNull.Value)
                    {
                        sb1.Append("NULL,");
                    }
                    else
                    {
                        if (column.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("float", StringComparison.OrdinalIgnoreCase) > -1
                            //|| column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                        )
                        {
                            sb1.AppendFormat("{0},", data);
                        }
                        else if (column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("{0},", (bool)data?1:0);
                        }
                        else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                            ||column.TypeName.Equals("datetime2",StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("'{0}',", ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            sb1.Append(string.Concat("'",string.IsNullOrEmpty((string)data)?string.Empty: data.ToString().Replace("'","''"), "',"));
                        }
                    }
                }
                if (sb1.Length > 0)
                    sb1.Remove(sb1.Length - 1, 1);
                sb.AppendLine();
                sb.AppendFormat("{0}", sb1.ToString());
            }
           
            if(!notExportId)
            {
                sb.AppendLine();
                sb.AppendLine(string.Format("SET IDENTITY_INSERT {0} OFF", string.Concat("[", node.Text, "]")));
                sb.AppendLine("GO");
            }
            TextBoxWin win = new TextBoxWin("导出数据", sb.ToString());
            win.Show();
        }

        private void 生成数据字典ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selnode = tv_DBServers.SelectedNode;
            if (selnode != null && selnode.Level == 3)
            {
                //库名
                string tbname = string.Format("[{0}].[{1}]", selnode.Parent.Text, selnode.Text);

                var tbclumns = Biz.Common.Data.SQLHelper.GetColumns(this.GetDBSource(selnode), selnode.Parent.Text, selnode.Name, selnode.Text).ToList();

                DataTable resulttb = new DataTable();
                resulttb.Columns.AddRange(new string[][] { 
                    new []{"line","行号"},
                    new []{"name","列名"},
                    new []{"iskey","是否主键"},
                    new []{"null","可空"},
                    new []{"type","类型"},
                    new []{"len","长度"},
                    new []{"desc","说明"} }.Select(s => new DataColumn
                {
                    ColumnName=s[0],
                    Caption=s[1],
                }).ToArray());

                var tbDesc = Biz.Common.Data.SQLHelper.GetTableColsDescription(GetDBSource(tv_DBServers.SelectedNode), tv_DBServers.SelectedNode.Parent.Text,
                    tv_DBServers.SelectedNode.Text);

                Regex rg = new Regex(@"(\w+)\s*\((\w+)\)");

                TreeNode selNode = tv_DBServers.SelectedNode;
                int idx = 1;
                foreach (TreeNode node in selNode.Nodes)
                {
                    if (node.Text == "索引")
                    {
                        continue;
                    }
                    var newrow = resulttb.NewRow();
                    newrow["line"] = idx++;
                    Match m = rg.Match(node.Text);
                    if (m.Success)
                    {
                        var y = (from x in tbDesc.AsEnumerable()
                                 where string.Equals((string)x["ColumnName"], m.Groups[1].Value, StringComparison.OrdinalIgnoreCase)
                                 select x["Description"]).FirstOrDefault();

                        //字段描述
                        string desc = y == DBNull.Value ? "&nbsp;" : (string)y;
                        newrow["desc"] = desc;
                        newrow["name"] = m.Groups[1].Value;
                        newrow["type"] = m.Groups[2].Value;

                        bool iskey = false;
                        if (node.Tag != null && node.Tag is TBColumn)
                        {
                            iskey = (node.Tag as TBColumn).IsKey;
                        }
                        newrow["iskey"] = iskey?"是":"否";
                        
                        var col=tbclumns.Find(p => p.Name.Equals(m.Groups[1].Value, StringComparison.OrdinalIgnoreCase));
                        if (col != null)
                        {
                            newrow["len"] = col.Length == -1 ? "max" : col.Length.ToString();
                            newrow["null"] = col.IsNullAble ? "是" : "否";
                        }

                        resulttb.Rows.Add(newrow);
                    }
                    else
                    {
                        MessageBox.Show("生成数据字典错误：" + node.Text);
                        break;
                    }

                }

                //生成HTML
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<html><head><title>数据字典-{0}</title></head><body><table cellpadding='1' cellspacing='0' border='1'>", tbname);
                sb.Append("<tr>");
                foreach (DataColumn col in resulttb.Columns)
                {
                    sb.AppendFormat("<th>{0}</th>", col.Caption);
                }
                sb.Append("</tr>");

                foreach (DataRow row in resulttb.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn col in resulttb.Columns)
                    {
                        sb.AppendFormat("<td>{0}</td>", row[col.ColumnName]);
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</table></body></html>");

                SubForm.WinWebBroswer web = new WinWebBroswer();
                web.SetHtml(sb.ToString());
                web.ShowDialog();

            }
        }

        private void 查看连接串ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = tv_DBServers.SelectedNode;
            if (node == null)
                return;

            string conndb = string.Empty;
            if (node.Level < 2)
                return;

            if(node.Level==2)
            {
                conndb = node.Text;
            }
            else
            {
                var pnode = node.Parent;
                while(pnode.Level!=2)
                {
                    pnode = pnode.Parent;
                }
                conndb = pnode.Text;
            }

            var connsql = SQLHelper.GetConnstringFromDBSource(GetDBSource(node), conndb);
            SubForm.WinWebBroswer web = new WinWebBroswer();
            web.SetHtml(string.Format("<html><head><title>连接串_{1}</title></head><body><br/>&lt;add name=\"ConndbDB${1}\" connectionString=\"{0}\" providerName=\"System.Data.SqlClient\"/&gt;</body></html>", connsql, conndb));
            web.Show();
        }

        private void 批量插入更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.BatchInsert);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.Update);
            }
        }

        private void SubMenuItem_Delete_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.Delete);
            }
        }

        private void upsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.Upsert);
            }
        }
    }
}
