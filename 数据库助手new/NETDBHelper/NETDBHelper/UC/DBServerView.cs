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
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace NETDBHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string, string> OnCreateEntity;
        public Action<DBSource, string, string, string> OnShowTableData;
        public Action<DBSource, string> OnAddEntityTB;
        public Action<string, string> OnCreateSelectSql;
        public Action<DBSource, string, string, string, CreateProceEnum> OnCreatePorcSQL;
        public Action<DBSource, string, string, string> OnShowProc;
        public Action<DBSource, string, string, string> OnShowDataDic;
        public Action<DBSource,string, string> OnViewTable;
        public Action<DBSource,string, string> OnViewCloumns;
        public Action<DBSource,string, string> OnFilterProc;
        public Action<DBSource, string, string> OnExecutSql;
        public Action<DBSource, string, string, string> OnShowViewSql;
        public Action<DBSource, string,string> OnShowRelMap;
        public Action<DBSource, string, LogicMap> OnAddNewLogicMap;
        public Action<string, LogicMap> OnDeleteLogicMap;
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
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code); //13
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB16);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB161); //16
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.loading);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ColQ); //18
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_no);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red_no);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.logic);
            tv_DBServers.Nodes.Add("0", "资源管理器", 0);
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);
            tv_DBServers.NodeMouseDoubleClick += Tv_DBServers_NodeMouseDoubleClick;
            tv_DBServers.HideSelection = false;
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
                    if (selnode.Index == selnode.Parent.Nodes.Count - 1)
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

                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = node.Text,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.db,
                                DB = node.Text,
                                Sever = GetDBSource(node).ServerName,
                                Info = "删除",
                                Valid = true
                            });
                        }
                        break;
                    case "新增对象":
                        var selnode = tv_DBServers.SelectedNode;
                        var dlg = new SubForm.InputStringDlg("请输入库名：");
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            Biz.Common.Data.SQLHelper.CreateDataBase(GetDBSource(selnode), selnode.FirstNode.Text, dlg.InputString);
                            ReLoadDBObj(selnode);

                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = dlg.InputString,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.db,
                                DB = dlg.InputString,
                                Sever = GetDBSource(selnode).ServerName,
                                Info = "新增",
                                Valid = true
                            });
                        }

                        break;
                    case "查看表":
                        {
                            OnViewTables();
                            break;
                        }
                    case "查看字段":
                        {
                            OnViewColumn();
                            break;
                        }
                    case "筛选存储过程":
                        {
                            FilterProc();
                            break;
                        }
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
            if (selNode.Tag is ServerInfo)
            {
                Biz.UILoadHelper.LoadDBsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if ((selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.TBParent)
            {
                var dbname = GetDBName(selNode).ToUpper();
                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, selNode, GetDBSource(selNode), dbname, name =>
                  {
                      var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                   [] { dbname, name.ToUpper(), string.Empty }).FirstOrDefault();
                      return mark == null ? string.Empty : mark.MarkInfo;
                  });
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.PROCParent)
            {
                var dbname = GetDBName(selNode).ToUpper();
                Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, selNode, GetDBSource(selNode), p =>
                 {
                     var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, p.ToUpper() }).FirstOrDefault();

                     if (item != null)
                     {
                         return item.Mark;
                     }
                     return string.Empty;
                 });
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.VIEWParent)
            {
                Biz.UILoadHelper.LoadViewsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Tag is TableInfo)
            {
                var dbname = GetDBName(selNode).ToUpper();
                var dbsource = GetDBSource(selNode);
                var synccolumnmark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.
                    Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { dbname, selNode.Text.ToUpper() }).FirstOrDefault() != null;
                Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, selNode, dbsource, (col) =>
                 {
                     var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                 [] { dbname, selNode.Text.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();

                     if (mark == null && !synccolumnmark)
                     {
                         LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<MarkObjectInfo>("MarkObjectInfo", new MarkObjectInfo
                         {
                             DBName = dbname.ToUpper(),
                             ColumnName = col.Name.ToUpper(),
                             Servername = dbsource.ServerName,
                             TBName = selNode.Text.ToUpper(),
                             ColumnType = col.ToDBType(),
                             MarkInfo = col.Description
                         });
                     }

                     return mark == null ? string.Empty : mark.MarkInfo;
                 });

                if (!synccolumnmark)
                {
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<ColumnMarkSyncRecord>("ColumnMarkSyncRecord",
                        new ColumnMarkSyncRecord
                        {
                            DBName = dbname.ToUpper(),
                            SyncDate = DateTime.Now,
                            TBName = selNode.Text.ToUpper()
                        });
                }

                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = selNode.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.table,
                    DB = dbname,
                    Sever = GetDBSource(selNode).ServerName,
                    Valid = true
                });
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.INDEXParent)
            {
                Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, selNode, GetDBSource(selNode), GetDBName(selNode));
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
                            var tableinfo = node.Tag as TableInfo;
                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = tableinfo.TBName,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.table,
                                DB = tableinfo.DBName,
                                Sever = GetDBSource(node).ServerName,
                                Info = "删除",
                                Valid = true
                            });

                            Biz.Common.Data.SQLHelper.DeleteTable(GetDBSource(node), tableinfo.DBName, tableinfo.TBName);
                            ReLoadDBObj(node.Parent);


                        }
                        break;
                    case "刷新":
                        ReLoadDBObj(tv_DBServers.SelectedNode);
                        break;
                    case "修改表名":
                        var _node = tv_DBServers.SelectedNode;
                        var tb = _node.Tag as TableInfo;
                        var oldname = tb.TBName;
                        var dlg = new SubForm.InputStringDlg("修改表名:", tb.TBName);
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            if (string.Equals(dlg.InputString, oldname, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                            
                            Biz.Common.Data.SQLHelper.ReNameTableName(GetDBSource(_node), tb.DBName,
                                oldname, dlg.InputString);
                            ReLoadDBObj(_node.Parent);

                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = dlg.InputString,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.table,
                                DB = tb.DBName,
                                Sever = GetDBSource(_node).ServerName,
                                Info = string.Format("重命名：{0}-{1}", oldname, dlg.InputString),
                                Valid = true
                            });
                        }
                        break;
                    case "Insert":
                        {
                            _node = tv_DBServers.SelectedNode;
                            if (_node.Tag is TableInfo && this.OnCreatePorcSQL != null)
                            {
                                var tableinfo = _node.Tag as TableInfo;
                                this.OnCreatePorcSQL(GetDBSource(_node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, CreateProceEnum.Insert);
                            }
                            break;
                        }
                    case "BatchInsert":
                        {
                            _node = tv_DBServers.SelectedNode;
                            if (_node.Tag is TableInfo && this.OnCreatePorcSQL != null)
                            {
                                var tableinfo = _node.Tag as TableInfo;
                                this.OnCreatePorcSQL(GetDBSource(_node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, CreateProceEnum.BatchInsert);
                            }
                            break;
                        }
                    case "Delete":

                        break;
                    case "Select":

                        break;
                    case "创建语句":
                        MessageBox.Show("Create");
                        break;
                    case "同步数据":
                        {
                            SyncTableData();
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
                    case "执行存储过程":
                        {
                            var selnode = tv_DBServers.SelectedNode;
                            if (selnode != null && OnExecutSql != null)
                            {
                                //foreach (var row in kv)
                                //{
                                //    var len = row.Field<Int16>("length");
                                //    var isnullable = row.Field<int>("isnullable") == 1;
                                //    var isoutparam = row.Field<int>("isoutparam") == 1;
                                //    newNode.Nodes.Add(row.Field<string>("pname"), $"{row.Field<string>("pname")}({row.Field<string>("tpname")}{(len == -1 ? string.Empty : "(" + len.ToString() + ")")}{(isnullable ? " null" : "")}{(isoutparam ? " output" : "")})", isoutparam ? 12 : 11, isoutparam ? 12 : 11);
                                //}
                                var procInfo = ((ProcInfo)selnode.Tag);
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine($"exec {procInfo.Name} ");
                                StringBuilder sb1 = new StringBuilder($"use {GetDBName(selnode)}");
                                sb1.AppendLine();
                                sb1.AppendLine("GO");
                                StringBuilder sb2 = new StringBuilder();
                                foreach (var pa in procInfo.ProcParamInfos)
                                {
                                    if (pa.IsOutparam)
                                    {
                                        sb.AppendLine($"{pa.Name}={pa.Name}1 output,");
                                        sb1.AppendLine($"declare {pa.Name}1 {(Biz.Common.Data.Common.GetDBType(new TBColumn { Name = pa.Name, TypeName = pa.TypeName, Length = pa.Len }))}");
                                        sb2.AppendLine($"select {pa.Name}1");
                                    }
                                    else
                                    {
                                        sb.AppendLine($"{pa.Name}=<{(Biz.Common.Data.Common.GetDBType(new TBColumn { Name = pa.Name, TypeName = pa.TypeName, Length = pa.Len }))}>,");
                                    }
                                }

                                if (sb.Length > 2 && sb[sb.Length - 3] == ',')
                                {
                                    sb = sb.Remove(sb.Length - 3, 1);
                                }
                                var sql = sb1.ToString() + sb.ToString() + sb2.ToString();

                                OnExecutSql(GetDBSource(selnode), GetDBName(selnode), sql);
                            }
                            break;
                        }
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
            if (tv_DBServers.SelectedNode != null && tv_DBServers.SelectedNode.Tag is TableInfo)
            {
                var tb = tv_DBServers.SelectedNode.Tag as TableInfo;
                List<KeyValuePair<string, bool>> cols = new List<KeyValuePair<string, bool>>();
                foreach (TreeNode node in tv_DBServers.SelectedNode.Nodes)
                {
                    var col = node.Tag as TBColumn;
                    if (col != null)
                    {
                        cols.Add(new KeyValuePair<string, bool>(col.Name, col.IsKey));
                    }
                }
                StringBuilder sb = new StringBuilder("select top 100 ");
                sb.AppendLine();
                sb.Append(string.Join(",\r\n", cols.Select(p => "[" + p.Key + "]")));
                sb.AppendLine("");
                sb.Append(" from ");
                sb.Append("[");
                sb.Append(tv_DBServers.SelectedNode.Text);
                sb.Append("]");
                sb.Append(" with(nolock)");
                if (this.OnShowTableData != null)
                {
                    OnShowTableData(GetDBSource(this.tv_DBServers.SelectedNode), tb.DBName, tb.TBName, sb.ToString());
                }
            }
            if (tv_DBServers.SelectedNode != null && tv_DBServers.SelectedNode.Tag is ViewInfo)
            {
                var viewinfo = tv_DBServers.SelectedNode.Tag as ViewInfo;
                List<KeyValuePair<string, bool>> cols = new List<KeyValuePair<string, bool>>();
                foreach (TreeNode node in tv_DBServers.SelectedNode.Nodes)
                {
                    var viewcol = node.Tag as ViewColumn;
                    cols.Add(new KeyValuePair<string, bool>(viewcol.Name, viewcol.IsKey));
                }
                StringBuilder sb = new StringBuilder("select top 100 ");
                sb.AppendLine();
                sb.Append(string.Join(",\r\n", cols.Select(p => "[" + p.Key + "]")));
                sb.AppendLine("");
                sb.Append(" from ");
                sb.Append("[");
                sb.Append(tv_DBServers.SelectedNode.Text);
                sb.Append("]");
                sb.Append(" with(nolock)");
                if (this.OnShowTableData != null)
                {
                    OnShowTableData(GetDBSource(this.tv_DBServers.SelectedNode), viewinfo.DBName, viewinfo.Name, sb.ToString());
                }
            }
        }

        void CreateEntityClass()
        {
            if (tv_DBServers.SelectedNode != null)
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
                string dbname = GetDBName(tv_DBServers.SelectedNode);
                var tid = (tv_DBServers.SelectedNode.Tag as TableInfo)?.TBId;

                var classcode = DataHelper.CreateTableEntity(dbsource, dbname, tbname, tid, DefaultEntityNamespace, tv_DBServers.SelectedNode.Tag is ViewInfo,
                    isSupportProtobuf, isSupportDBMapperAttr, isSupportJsonproterty, isSupportMvcDisplay, name =>
                     {
                         var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                  [] { dbname.ToUpper(), tbname.ToUpper(), name.ToUpper() }).FirstOrDefault();

                         return mark == null ? name : mark.MarkInfo;

                     }, out hasKey);

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
            if (node.Tag is ServerInfo)
                return (node.Tag as ServerInfo).DBSource;
            return GetDBSource(node.Parent);
        }

        private string GetTBName(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Tag is TableInfo)
                return (node.Tag as TableInfo).TBName;
            return GetTBName(node.Parent);
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
            //throw new NotImplementedException();
            if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.TBParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;

                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), GetDBName(e.Node), name =>
                  {
                      var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                   [] { GetDBName(e.Node).ToUpper(), name.ToUpper(), string.Empty }).FirstOrDefault();
                      return mark == null ? string.Empty : mark.MarkInfo;
                  });

                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = e.Node.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.db,
                    DB = e.Node.Text,
                    Sever = GetDBSource(e.Node).ServerName,
                    Valid = true
                });
            }
            else if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.PROCParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                var dbname = GetDBName(e.Node).ToUpper();
                Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), p =>
                {
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, p.ToUpper() }).FirstOrDefault();

                    if (item != null)
                    {
                        return item.Mark;
                    }
                    return string.Empty;
                });
            }
            else if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.VIEWParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadViewsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));
            }
            else if (e.Node.Tag is TableInfo)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                var tb = e.Node.Tag as TableInfo;
                var dbname = GetDBName(e.Node).ToUpper();
                var dbsource = GetDBSource(e.Node);
                var synccolumnmark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.
                   Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { dbname, tb.TBName.ToUpper() }).FirstOrDefault() != null;
                Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), (col) =>
                {
                    var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { dbname, tb.TBName.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();
                    if (mark == null && !synccolumnmark)
                    {
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<MarkObjectInfo>("MarkObjectInfo", new MarkObjectInfo
                        {
                            DBName = dbname.ToUpper(),
                            ColumnName = col.Name.ToUpper(),
                            Servername = dbsource.ServerName,
                            TBName = tb.TBName.ToUpper(),
                            ColumnType = col.ToDBType(),
                            MarkInfo = col.Description
                        });
                    }
                    return mark == null ? string.Empty : mark.MarkInfo;
                });

                if (!synccolumnmark)
                {
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<ColumnMarkSyncRecord>("ColumnMarkSyncRecord",
                        new ColumnMarkSyncRecord
                        {
                            DBName = dbname.ToUpper(),
                            SyncDate = DateTime.Now,
                            TBName = tb.TBName.ToUpper()
                        });
                }

                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = e.Node.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.table,
                    DB = dbname,
                    Sever = GetDBSource(e.Node).ServerName,
                    Valid = true
                });
            }
            else if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.INDEXParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), GetDBName(e.Node));
            }
            else if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.LOGICMAPParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadLogicMapsAnsy(this.ParentForm, e.Node, GetDBName(e.Node));
            }
        }


        private void Tv_DBServers_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e.Node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.LOGICMAP)
            {
                OnAddNewLogicMap(GetDBSource(e.Node), GetDBName(e.Node), e.Node.Tag as LogicMap);
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
                TreeNode node = new TreeNode(s.ServerName, 1, 1);
                var serverinfo = new ServerInfo { DBSource = s };
                node.Tag = serverinfo;
                tv_DBServers.Nodes[0].Nodes.Add(node);
                ReLoadDBObj(node);
            }
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
            this.DBServers.Remove(this.DBServers.FirstOrDefault(p => p.ServerName.Equals(serverName)));
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
                            || nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.VIEWParent
                            || nctype == NodeContentType.PROCParent
                            || nctype == NodeContentType.PROC
                            || nctype == NodeContentType.LOGICMAPParent;

                        导出ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.PROC
                            || nctype == NodeContentType.TB;
                        CreateMSSQLToolStripMenuItem.Visible = nctype == NodeContentType.PROC
                            || nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB;
                        创建语句ToolStripMenuItem.Visible = 数据MSSQLToolStripMenuItem.Visible =
                            ExpdataToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        生成实体类ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB;
                        复制表名ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB
                            || nctype == NodeContentType.INDEX
                            || nctype == NodeContentType.LOGICMAP;
                        显示前100条数据ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB;
                        清理备注ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        备注ToolStripMenuItem.Visible = nctype == NodeContentType.TB
                            || nctype == NodeContentType.PROC;
                        表关系图ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        TSMI_ExeProc.Visible = nctype == NodeContentType.PROC;
                        生成数据字典ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        SyncDataToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        修改表名ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        删除表ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        SubMenuItem_Proc.Visible = nctype == NodeContentType.TB;
                        新增逻辑关系图ToolStripMenuItem.Visible = nctype == NodeContentType.LOGICMAPParent;
                        删除逻辑关系图ToolStripMenuItem.Visible = nctype == NodeContentType.LOGICMAP;
                    }
                    else
                    {
                        this.tv_DBServers.ContextMenuStrip = this.CommMenuStrip;
                        subMenuItemAddEntityTB.Visible = nctype == NodeContentType.DB;
                        TSMI_ViewTableList.Visible = nctype == NodeContentType.DB;
                        TSMI_ViewColumnList.Visible = nctype == NodeContentType.DB;
                        CommSubMenuItem_Delete.Visible = nctype == NodeContentType.DB;
                        CommSubMenuitem_add.Visible = nctype == NodeContentType.SEVER;
                        CommSubMenuitem_ViewConnsql.Visible = nctype == NodeContentType.DB;
                        CommSubMenuitem_ReorderColumn.Visible = nctype == NodeContentType.COLUMN;
                        备注本地ToolStripMenuItem.Visible = nctype == NodeContentType.COLUMN;
                        TSMI_MulMarkLocal.Visible = nctype == NodeContentType.COLUMN;
                        TSMI_FilterProc.Visible = nctype == NodeContentType.PROCParent;
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
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tableinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, CreateProceEnum.Insert);
            }
        }

        private void 创建语句ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || !(node.Tag is TableInfo))
            {
                return;
            }
            StringBuilder sb = new StringBuilder(string.Format("CREATE TABLE `{0}`(", node.Text));
            sb.AppendLine();
            var tableinfo = node.Tag as TableInfo;
            foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName))
            {
                sb.AppendFormat("`{0}` {1} {2} {3},", col.Name, Biz.Common.Data.Common.GetDBType(col), (col.IsID || col.IsKey) ? "NOT NULL" : (col.IsNullAble ? "NULL" : "NOT NULL"), col.IsID ? "AUTO_INCREMENT" : "");
                if (col.IsID)
                {
                    sb.AppendLine();
                    sb.AppendFormat("PRIMARY KEY (`{0}`),", col.Name);
                }
                sb.AppendLine();
            }
            sb.AppendLine("`last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            sb.AppendLine(")ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=utf8;");
            sb.AppendLine("//注意：bit类型要手工改成TINYINT(1)。");
            TextBoxWin win = new TextBoxWin("创建表" + node.Text, sb.ToString());
            win.Show();
        }

        private void ExpdataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isNotExportKey = false;
            //SET IDENTITY_INSERT
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || !(node.Tag is TableInfo))
            {
                return;
            }
            var tableinfo = node.Tag as TableInfo;
            var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName);
            if (isNotExportKey)
            {
                cols = cols.Where(p => !p.IsID);
            }
            cols = cols.OrderBy(p => p.IsID ? 0 : 1);

            string sqltext = string.Format("select {0} from {1} with(nolock)", string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))), string.Concat("[", node.Text, "]"));
            var datas = Biz.Common.Data.SQLHelper.ExecuteDBTable(GetDBSource(node), tableinfo.DBName, sqltext, null);
            StringBuilder sb = new StringBuilder();
            if (!isNotExportKey)
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
                            sb1.AppendFormat("'{0}',", ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
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
            if (node == null || !(node.Tag is TableInfo))
            {
                return;
            }
            var tableinfo = node.Tag as TableInfo;
            DBSource dbsource = GetDBSource(node);
            string dbname = tableinfo.DBName,
            tbname = tableinfo.TBName, tid = tableinfo.TBId;

            var cols = Biz.Common.Data.SQLHelper.GetColumns(dbsource, dbname, tid, tbname).ToList();

            SubForm.WinCreateSelectSpNav nav = new WinCreateSelectSpNav(cols);

            if (nav.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            var conditioncols = nav.ConditionColumns;
            var outputcols = nav.OutPutColumns;

            string codes = DataHelper.CreateSelectSql(dbname, tbname, nav.Editer, nav.SPAbout, cols, conditioncols, outputcols);

            if (OnCreateSelectSql != null)
            {
                OnCreateSelectSql(string.Format("查询[{0}.{1}]", dbname, tbname), codes);
            }

            //SubForm.WinWebBroswer webdlg = new WinWebBroswer();
            //webdlg.SetTitle("生成存储过程").SetBody(codes);
            //webdlg.ShowDialog();
        }

        private void CreateMSSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node != null && node.Tag is TableInfo)
            {
                var tableinfo = node.Tag as TableInfo;
                StringBuilder sb = new StringBuilder(string.Format("Use [{0}]", tableinfo.DBName));
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
                foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName))
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
            else if (node != null && node.Tag is ProcInfo)
            {
                if (OnShowProc != null)
                {
                    var procinfo = node.Tag as ProcInfo;
                    var dbname = GetDBName(node);
                    var body = Biz.Common.Data.SQLHelper.GetProcedureBody(GetDBSource(node), dbname, procinfo.Name);
                    //TextBoxWin win = new TextBoxWin("存储过程[" + node.Text + "]", body);
                    //win.Show();
                    OnShowProc(GetDBSource(node), dbname, procinfo.Name, body);

                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                    {
                        TypeName = node.Text,
                        LogTime = DateTime.Now,
                        LogType = LogTypeEnum.proc,
                        DB = dbname,
                        Sever = GetDBSource(node).ServerName,
                        Valid = true
                    });
                }

            }
            else if (node != null && node.Tag is ViewInfo)
            {
                var dbsource = GetDBSource(node);
                var dbname = GetDBName(node);
                var viewinfo = node.Tag as ViewInfo;
                var body = Biz.Common.Data.SQLHelper.GetViewCreateSql(dbsource, dbname, viewinfo.Name);
                //TextBoxWin win = new TextBoxWin("视图[" + node.Text + "]", body);
                //win.Show();

                if (OnShowViewSql != null)
                {
                    OnShowViewSql(dbsource, dbname, viewinfo.Name, body);
                }

                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = viewinfo.Name,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.view,
                    DB = dbname,
                    Sever = GetDBSource(node).ServerName,
                    Valid = true
                });
            }
        }

        //导出数据
        private void 数据MSSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || !(node.Tag is TableInfo))
            {
                return;
            }

            var tableinfo = node.Tag as TableInfo;

            InputStringDlg dlg = new InputStringDlg("要导出的数据量", "1000");
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            bool notExportId = true;



            int topNum = 1000;
            int.TryParse(dlg.InputString, out topNum);

            var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName);
            if (cols.ToList().Exists(p => p.IsID))
            {
                if (MessageBox.Show("是否要导出自增列数据，如果导出会删除原来的数据。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    notExportId = false;
                }
            }
            if (notExportId)
            {
                cols = cols.Where(p => !p.IsID);
            }
            cols = cols.OrderBy(p => p.IsID ? 0 : 1);

            string sqltext = string.Format("select top {2} {0} from {1} with(nolock)", string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))), string.Concat("[", node.Text, "]"), topNum);
            var datas = Biz.Common.Data.SQLHelper.ExecuteDBTable(GetDBSource(node), tableinfo.DBName, sqltext, null);
            StringBuilder sb = new StringBuilder();
            if (!notExportId)
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
                StringBuilder sb1 = new StringBuilder(idx > 1 ? " union select " : "select ");
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
                            sb1.AppendFormat("{0},", (bool)data ? 1 : 0);
                        }
                        else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("smalldatetime", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("'{0}',", ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            sb1.Append(string.Concat("'", string.IsNullOrEmpty((string)data) ? string.Empty : data.ToString().Replace("'", "''"), "',"));
                        }
                    }
                }
                if (sb1.Length > 0)
                    sb1.Remove(sb1.Length - 1, 1);
                sb.AppendLine();
                sb.AppendFormat("{0}", sb1.ToString());
            }

            if (!notExportId)
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
            if (selnode != null && selnode.Tag is TableInfo)
            {
                var tableinfo = selnode.Tag as TableInfo;
                //库名
                string tbname = string.Format("[{0}].[{1}]", tableinfo.DBName, tableinfo.TBName);

                var tbclumns = Biz.Common.Data.SQLHelper.GetColumns(this.GetDBSource(selnode), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName).ToList();

                var tbmark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                 [] { tableinfo.DBName.ToUpper(), tableinfo.TBName.ToUpper(), string.Empty }).FirstOrDefault();
                var tbdesc = tbmark == null ? (tableinfo.Desc ?? tableinfo.TBName) : tbmark.MarkInfo;

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
                        ColumnName = s[0],
                        Caption = s[1],
                    }).ToArray());

                var tbDesc = Biz.Common.Data.SQLHelper.GetTableColsDescription(GetDBSource(tv_DBServers.SelectedNode), tableinfo.DBName,tableinfo.TBName);

                Regex rg = new Regex(@"(\w+)\s*\((\w+)\)");

                TreeNode selNode = tv_DBServers.SelectedNode;
                int idx = 1;
                foreach (TreeNode node in selNode.Nodes)
                {
                    if (!(node.Tag is TBColumn))
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

                        if (y == null || string.IsNullOrEmpty(y.ToString()))
                        {
                            var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { GetDBName(selnode).ToUpper(), GetTBName(selnode).ToUpper(), m.Groups[1].Value.ToUpper() }).FirstOrDefault();
                            if (mark != null)
                            {
                                y = mark.MarkInfo;
                            }
                        }

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
                        newrow["iskey"] = iskey ? "√" : "✕";

                        var col = tbclumns.Find(p => p.Name.Equals(m.Groups[1].Value, StringComparison.OrdinalIgnoreCase));
                        if (col != null)
                        {
                            newrow["len"] = col.Length == -1 ? "max" : col.Length.ToString();
                            newrow["null"] = col.IsNullAble ? "√" : "✕";
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
                sb.AppendFormat(@"<html><head><title>数据字典-{0}</title><style>
p{{font-size:11px;}}
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style></head><body><p>表名：{0}</p><p>表说明：{1}</p><table cellpadding='0' cellspacing='0' border='1'>", tbname, tbdesc);
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

                if (OnShowDataDic != null)
                {
                    OnShowDataDic(GetDBSource(selnode), GetDBName(selnode), selnode.Text, sb.ToString());
                }
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

            if (node.Level == 2)
            {
                conndb = node.Text;
            }
            else
            {
                var pnode = node.Parent;
                while (pnode.Level != 2)
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
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName, CreateProceEnum.BatchInsert);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName, CreateProceEnum.Update);
            }
        }

        private void SubMenuItem_Delete_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName, CreateProceEnum.Delete);
            }
        }

        private void upsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName, CreateProceEnum.Upsert);
            }
        }

        private void Mark_Local()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (selnode != null && selnode.Tag is TBColumn)
            {
                var tbcol = ((TBColumn)selnode.Tag);
                var tbname = GetTBName(selnode);
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), tbname.ToUpper(), tbcol.Name.ToUpper() }).FirstOrDefault();

                if (item == null)
                {
                    item = new MarkObjectInfo { ColumnName = tbcol.Name.ToUpper(), DBName = dbname.ToUpper(), TBName = tbname.ToUpper(), Servername = servername };
                }
                InputStringDlg dlg = new InputStringDlg($"备注字段[{tbname}.{tbcol.Name}]", item.MarkInfo);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (selnode.ImageIndex == 18)
                    {
                        selnode.ImageIndex = selnode.SelectedImageIndex = 5;
                    }
                    item.MarkInfo = dlg.InputString;
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                    selnode.ToolTipText = item.MarkInfo;
                    MessageBox.Show("备注成功");
                }
            }
            else if (selnode.Tag is ProcInfo)
            {
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var spname = (selnode.Tag as ProcInfo).Name;
                var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                if (item == null)
                {
                    item = new SPInfo { Servername = servername, DBName = dbname, SPName = spname, Mark = "" };
                }
                InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    item.Mark = dlg.InputString;
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<SPInfo>("SPInfo", item);
                    selnode.ToolTipText = item.Mark;
                    selnode.ImageIndex = 13;
                    selnode.SelectedImageIndex = 14;
                    MessageBox.Show("备注成功");
                }
            }

        }

        private void SyncTableData()
        {
            var node = tv_DBServers.SelectedNode;
            if (node != null && node.Tag is TableInfo)
            {
                var tableinfo = node.Tag as TableInfo;

                var dlg = new SubForm.SyncDataWin(GetDBSource(node), tableinfo.DBName, tableinfo.TBName);
                dlg.Show();
            }
        }

        private void 备注本地ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mark_Local();
        }

        private void MarkResource()
        {
            var currnode = tv_DBServers.SelectedNode;
            if (currnode != null)
            {
                if (currnode.Tag != null && currnode.Tag is TableInfo)
                {
                    var tb = (TableInfo)currnode.Tag;
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), string.Empty }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new MarkObjectInfo { ColumnName = string.Empty, DBName = tb.DBName.ToUpper(), TBName = tb.TBName.ToUpper(), Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注:{tb.TBName}", item.MarkInfo);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.MarkInfo = dlg.InputString;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                        currnode.ToolTipText = item.MarkInfo;
                        MessageBox.Show("备注成功");
                    }
                }
                else if (currnode.Tag is ProcInfo)
                {
                    var servername = GetDBSource(currnode).ServerName;
                    var dbname = GetDBName(currnode);
                    var spname = currnode.Text;
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new SPInfo { Servername = servername, DBName = dbname.ToUpper(), SPName = spname.ToUpper(), Mark = "" };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.Mark = dlg.InputString;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<SPInfo>("SPInfo", item);
                        currnode.ToolTipText = item.Mark;
                        currnode.ImageIndex = 13;
                        currnode.SelectedImageIndex = 14;
                        MessageBox.Show("备注成功");
                    }
                }
            }
        }

        private void ClearMarkResource()
        {
            var currnode = tv_DBServers.SelectedNode;
            if (currnode != null)
            {
                if (currnode.Tag != null && currnode.Tag is TableInfo)
                {
                    if (MessageBox.Show("要清理本地缓存吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                    var tb = (TableInfo)currnode.Tag;
                    var cols = SQLHelper.GetColumns(GetDBSource(currnode), tb.DBName, tb.TBId, tb.TBName).ToList();
                    
                    var markedcolumns = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), LJC.FrameWorkV3.Data.EntityDataBase.Consts.STRINGCOMPAIRMIN },
                        new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), LJC.FrameWorkV3.Data.EntityDataBase.Consts.STRINGCOMPAIRMAX }, 1, int.MaxValue);

                    foreach (var col in markedcolumns)
                    {
                        if (string.IsNullOrWhiteSpace(col.ColumnName))
                        {
                            continue;
                        }
                        if (!cols.Any(p => p.Name.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase)) || string.IsNullOrWhiteSpace(col.MarkInfo))
                        {
                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Delete<MarkObjectInfo>("MarkObjectInfo", col.ID);
                        }
                    }
                    var columnMarkSyncRecorditem = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper() }).ToList();
                    foreach(var rec in columnMarkSyncRecorditem)
                    {
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Delete<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", rec.ID);
                    }
                    ReLoadDBObj(currnode);
                    MessageBox.Show("清理成功");
                }
            }
        }

        private void TSMI_MulMarkLocal_Click(object sender, EventArgs e)
        {
            var currnode = tv_DBServers.SelectedNode;
            if (currnode == null)
            {
                return;
            }
            MultiInputDlg dlg = new MultiInputDlg();
            dlg.Text = "批量注释";
            dlg.Moke = "每行一个，示例：列名#####说明文字";
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var tbname = GetTBName(currnode)?.ToUpper();
            if (string.IsNullOrWhiteSpace(tbname))
            {
                return;
            }

            var dbname = GetDBName(currnode).ToUpper();

            var allnodes = currnode.Parent.Nodes;
            var dic = new Dictionary<string, TreeNode>();
            foreach (TreeNode node in allnodes)
            {
                if (node.Tag is TBColumn)
                {
                    if (string.IsNullOrWhiteSpace(node.ToolTipText))
                    {
                        var colinfo = (TBColumn)node.Tag;
                        dic.Add(colinfo.Name.ToUpper(), node);
                    }
                }
            }

            int scount = 0;

            foreach (var ln in dlg.InputString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var arr = ln.Split(new[] { "#####" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 1)
                {
                    var mark = arr[1];
                    if (string.IsNullOrWhiteSpace(arr[1]))
                    {
                        continue;
                    }
                    var column = arr[0].Trim().ToUpper();

                    if (dic.ContainsKey(column))
                    {
                        var tb = (TBColumn)dic[column].Tag;
                        var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname, tbname, column }).FirstOrDefault();

                        if (item == null)
                        {
                            item = new MarkObjectInfo { ColumnName = column, DBName = dbname, TBName = tbname, Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                        }

                        item.MarkInfo = mark;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                        dic[column].ToolTipText = mark;

                        if (dic[column].ImageIndex == 18)
                        {
                            dic[column].ImageIndex = dic[column].SelectedImageIndex = 5;
                        }

                        scount++;
                        //MessageBox.Show("备注成功");

                    }
                }
            }


            MessageBox.Show($"备注成功{scount}条");
        }

        private void OnViewTables()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (this.OnViewTable != null && selnode != null)
            {
                var dbname = GetDBName(selnode);
                var dbsource = GetDBSource(selnode);
                DataTable tb = Biz.Common.Data.SQLHelper.GetTBs(dbsource, dbname);

                StringBuilder sb = new StringBuilder("<html>");
                sb.Append("<head>");
                sb.Append($"<title>查看{dbname}的库表</title>");
                sb.Append(@"<style>
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style>");
                sb.Append("</head>");
                sb.Append(@"<body>
                  <script>
                      function k(){
                          if (event.keyCode == 13) s();
                      }
                      function s(){
                       var w=document.getElementById('w').value
                       if(/^\s*$/.test(w)){
                           var idx=1
                           var trs= document.getElementsByTagName('tr');
                           for(var i=0;i<trs.length;i++){
                               trs[i].style.display=''
                               if(trs[i].firstChild.tagName=='TD')
                                   trs[i].firstChild.innerText=idx++
                            }
                           return
                       }
                       var idx=1;
                       var tds= document.getElementsByTagName('td');
                       w=w.toUpperCase();
                       for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText.toUpperCase().indexOf(w)>-1||tds[i+2].innerText.toUpperCase().indexOf(w)>-1
                           tds[i].parentNode.style.display=boo?'':'none'
                           if(boo) tds[i].innerText=idx++
                       }
                   }
                   
                  </script>");
                sb.Append("<input id='w' type='text' style='height:23px; line-height:23px;' onkeypress='k()' value=''/><input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/>");
                sb.Append("<p/>");
                sb.Append("<table>");
                sb.Append("<tr><th>序号</th><th>表名</th><th>描述</th></tr>");
                int i = 1;
                foreach (DataRow row in tb.Rows)
                {
                    var name = (string)row["name"];
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), name.ToUpper(), string.Empty }).FirstOrDefault();

                    sb.Append($"<tr><td>{i++}</td><td>{name}</td><td>{(item == null ? string.Empty : item.MarkInfo)}</td></tr>");
                }
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                this.OnViewTable(dbsource, dbname, sb.ToString());
            }

        }

        private void OnViewColumn()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (this.OnViewCloumns != null && selnode != null)
            {
                var dbname = GetDBName(selnode);

                var allcolumns = BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", p => !string.IsNullOrEmpty(p.ColumnName) && p.DBName.Equals(dbname, StringComparison.OrdinalIgnoreCase));

                StringBuilder sb = new StringBuilder("<html>");
                sb.Append("<head>");
                sb.Append($"<title>查看{dbname}的字段表</title>");
                sb.Append(@"<style>
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style>");
                sb.Append("</head>");
                sb.Append(@"<body>
                  <script>
                      function k(){
                          if (event.keyCode == 13) s();
                      }
                      function s(){
                       var w=document.getElementById('w').value
                       if(/^\s*$/.test(w)){
                           var idx=1
                           var trs= document.getElementsByTagName('tr');
                           for(var i=0;i<trs.length;i++){
                               trs[i].style.display=''
                               if(trs[i].firstChild.tagName=='TD')
                                   trs[i].firstChild.innerText=idx++
                            }
                           return
                       }

                       var ckbody=document.getElementById('scontent').checked;
                       if(!ckbody){
                       var idx=1;
                       var tds= document.getElementsByTagName('td');
                       w=w.toUpperCase();
                       for(var i=0;i<tds.length;i+=4){
                           var boo=tds[i+2].innerText.toUpperCase().indexOf(w)>-1||tds[i+3].innerText.toUpperCase().indexOf(w)>-1
                           tds[i].parentNode.style.display=boo?'':'none'
                           if(boo) tds[i].innerText=idx++
                       }
                     }else{
                         window.external.Search(w);
                     }
                       
                   }

                    function searchcallback(str){
                     var searchresult= JSON.parse(str);                                
                      var tdsDic={};
                      var tds= document.getElementsByTagName('td');
                       for(var i=0;i<tds.length;i+=4){
                           tds[i].parentNode.style.display='none';
                           var key=tds[i+1].innerText+'_'+tds[i+2].innerText;
                           tdsDic[key]=tds[i];
                       }
                      var idx=1;
                      var newcols=[];
                      for(var j=0;j<searchresult.length;j++){
                         var key=(searchresult[j].TBName+'_'+searchresult[j].Name).toLowerCase();
                         var boo=tdsDic[key];
                        if(boo){
                           boo.parentNode.style.display='';
                           boo.innerText=idx++;
                        }else{
                            newcols.push(j);
                         }
                      }
                      if(newcols.length>0){
                         var tb=document.getElementById('colstb').tBodies[0];
                         for(var i=0;i<newcols.length;i++){
                            var col=searchresult[newcols[i]];
                            var tr=document.createElement('tr');
                            var td1=document.createElement('td');
                            td1.innerText=idx++;
                            tr.appendChild(td1);
                            var td2=document.createElement('td');
                            td2.innerText=col.TBName.toLowerCase();
                            tr.appendChild(td2);
                            var td3=document.createElement('td');
                            td3.innerText=col.Name.toLowerCase();
                            tr.appendChild(td3);
                            
                            var td4=document.createElement('td');
                            td4.innerText=col.Description;
                            tr.appendChild(td4);
                            //tr.innerHTML='<td>'+(idx++)+'</td><td>'+col.TBName.toLowerCase()+'</td><td>'+col.Name.toLowerCase()+'</td><td>'+col.Description+'</td>';
                            tb.appendChild(tr);
                            
                            //alert(tr.innerHTML);
                            //
                            
                         }
                      }
                      document.getElementById('clearcachtip').style.display='';
                  }

                   function tryclearcach(){
                       window.external.ClearColSearchCach();
                      document.getElementById('clearcachtip').style.display='none';
                      
                   }
                  </script>");
                sb.AppendFormat("<script>{0}</script>", System.IO.File.ReadAllText("json2.js"));
                sb.Append(@"<input id='w' type='text' style='height:23px; line-height:23px;width:30%;' onkeypress='k()' value=''/>
                            <input type='checkbox' id='scontent' value='1'>全库搜索</input>
                            <input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/>");
                sb.Append("<p/>");
                sb.Append($"<div id='clearcachtip' style='margin-top:5px;display:none;width:98%;font-size:9pt;height:18px; line-height:18px;background-color:lightyellow;border:solid 1px lightblue'>如果没有找到，可以选择<a href='javascript:tryclearcach()'>清空缓存</a>试试</div>");
                sb.Append("<p/>");
                sb.Append("<table id='colstb'>");
                sb.Append("<tr><th>序号</th><th>表名</th><th>字段</th><th>描述</th></tr>");
                int i = 1;
                foreach (MarkObjectInfo c in allcolumns)
                {
                    var name = c.ColumnName;

                    sb.Append($"<tr><td>{i++}</td><td>{c.TBName.ToLower()}</td><td>{name.ToLower()}</td><td>{c.MarkInfo}</td></tr>");
                }
                sb.Append("</table>");
                sb.Append("<table id='colstb2'></table>");
                sb.Append("</body>");
                sb.Append("</html>");

                this.OnViewCloumns(GetDBSource(selnode), dbname, sb.ToString());
            }

        }

        private void FilterProc()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (this.OnFilterProc != null && selnode != null)
            {
                var dbsource = GetDBSource(selnode);
                var dbname = GetDBName(selnode);
                var proclist = Biz.Common.Data.SQLHelper.GetProcedures(dbsource, dbname);
                
                StringBuilder sb = new StringBuilder("<html>");
                sb.Append("<head>");
                sb.Append($"<title>查看{dbname}的存储过程</title>");
                sb.Append(@"<style>
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style>");
                sb.Append("</head>");
                sb.Append(@"<body>
                  
                  <script>
                      function k(){
                          if (event.keyCode == 13) s();
                      }
                      function s(){
                       document.getElementById('clearcachtip').style.display='none';
                       var w=document.getElementById('w').value
                       if(/^\s*$/.test(w)){
                           var idx=1
                           var trs= document.getElementsByTagName('tr');
                           for(var i=0;i<trs.length;i++){
                               trs[i].style.display=''
                               if(trs[i].firstChild.tagName=='TD')
                                   trs[i].firstChild.innerText=idx++
                            }
                           return
                       }
                       var ckbody=document.getElementById('scontent').checked;
                       if(!ckbody){
                       var idx=1;
                       var tds= document.getElementsByTagName('td');
                       w=w.toUpperCase();
                       for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText.toUpperCase().indexOf(w)>-1||tds[i+2].innerText.toUpperCase().indexOf(w)>-1
                           tds[i].parentNode.style.display=boo?'':'none'
                           if(boo) tds[i].innerText=idx++
                       }
                     }else{
                         window.external.Search(w);
                     }
                   }
                    
                   function tryclearcach(){
                       window.external.ClearCach();
                      document.getElementById('clearcachtip').style.display='none';
                      
                   }

                   function searchcallback(str){
                      var searchresult= JSON.parse(str);
                      var tds= document.getElementsByTagName('td');
                       for(var i=0;i<tds.length;i+=3){
                           tds[i].parentNode.style.display='none';
                       }
                      var idx=1;
                      for(var j=0;j<searchresult.length;j++){
                         var spname=searchresult[j];
                         for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText==spname;
                           if(boo){
                               tds[i].parentNode.style.display='';
                               tds[i].innerText=idx++;
                               break;
                            }
                        }
                      }
                      document.getElementById('clearcachtip').style.display='';
                  }
                  </script>");
                sb.AppendFormat("<script>{0}</script>",System.IO.File.ReadAllText("json2.js"));
                sb.Append(@"<input id='w' type='text' style='height:23px; line-height:23px;width:30%;' onkeypress='k()' value=''/>
                            <input type='checkbox' id='scontent' value='1'>搜索内容</input>
                            <input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/>");
                sb.Append("<p/>");
                sb.Append($"<div id='clearcachtip' style='margin-top:5px;display:none;width:98%;font-size:9pt;height:18px; line-height:18px;background-color:lightyellow;border:solid 1px lightblue'>如果没有找到，可以选择<a href='javascript:tryclearcach()'>清空缓存</a>试试</div>");
                sb.Append("<p/>");
                sb.Append("<table>");
                sb.Append("<tr><th>序号</th><th>存储过程</th><th>描述</th></tr>");
                int i = 1;
                dbname = dbname.ToUpper();
                foreach (string name in proclist)
                {
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, name.ToUpper() }).FirstOrDefault();
                    sb.Append($"<tr><td>{i++}</td><td><a href='javascript:window.external.ShowProc(\"{name}\")'>{name}</a></td><td>{item?.Mark}</td></tr>");
                }
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                this.OnFilterProc(dbsource, dbname, sb.ToString());
            }
        }

        private void 表关系图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selnode = tv_DBServers.SelectedNode;
            if (selnode != null && selnode.Tag is TableInfo)
            {
                var dbsource = GetDBSource(selnode);
                var dbname = GetDBName(selnode);
                if (this.OnShowRelMap != null)
                {
                    this.OnShowRelMap(dbsource, dbname, selnode.Text);
                }
            }
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
                        if (OnAddNewLogicMap != null)
                        {
                            OnAddNewLogicMap(GetDBSource(currnode), db, logicmap);
                        }
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
    }
}
