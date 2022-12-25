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
using LJC.FrameWorkV3.LogManager;
using System.Threading.Tasks;
using System.Threading;
using Biz;

namespace NETDBHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string, string> OnCreateEntity;
        public Action<DBSource, string, string, string> OnShowTableData;
        public Action<DBSource, string> OnAddEntityTB;
        public Action<string, string> OnCreateSelectSql;
        public Action<DBSource, string, string, string,string, CreateProceEnum> OnCreatePorcSQL;
        public Action<DBSource, string, string, string> OnShowProc;
        public Action<DBSource, string, string, string> OnShowDataDic;
        public Action<DBSource, string, string> OnViewTable;
        public Action<DBSource, string, string> OnViewCloumns;
        public Action<DBSource, string, string> OnFilterProc;
        public Action<DBSource, string, string> OnFilterFunction;
        public Action<DBSource, string, string> OnExecutSql;
        public Action<DBSource, string, string, string> OnShowViewSql;
        public Action<DBSource, string, string> OnShowRelMap;
        public Action<DBSource, string, LogicMap> OnAddNewLogicMap;
        public Action<string, LogicMap> OnDeleteLogicMap;
        private DBSourceCollection _dbServers;
        private UC.LoadingBox searchingWaitingBox = new UC.LoadingBox(); 
        /// <summary>
        /// 实体命名空间
        /// </summary>
        private static string DefaultEntityNamespace = "Nonamespace";
        private UC.UCSearchOptions UCSearchOptions = new UC.UCSearchOptions();

        private AutoResetEvent searchWaitEvent = new AutoResetEvent(true);

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
            tv_DBServers.ImageList.Images.Add("ASC", Resources.Resource1.ASC);
            tv_DBServers.ImageList.Images.Add("DESC", Resources.Resource1.DESC);
            tv_DBServers.ImageList.Images.Add("plugin", Resources.Resource1.plugin);
            tv_DBServers.ImageList.Images.Add("lightning", Resources.Resource1.lightning);//25
            tv_DBServers.ImageList.Images.Add("lightning_delete", Resources.Resource1.lightning_delete);
            tv_DBServers.ImageList.Images.Add("table", Resources.Resource1.table);
            tv_DBServers.ImageList.Images.Add("table_link", Resources.Resource1.table_link);
            tv_DBServers.ImageList.Images.Add("key_go", Resources.Resource1.key_go);
            tv_DBServers.Nodes.Add("0", "资源管理器", 0);
            tv_DBServers.ImageList.Images.Add("database_stop", Resources.Resource1.database_stop);
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);
            tv_DBServers.NodeMouseDoubleClick += Tv_DBServers_NodeMouseDoubleClick;
            tv_DBServers.HideSelection = false;
            this.DBServerviewContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuStrip_ItemClicked);
            this.CommMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(CommMenuStrip_ItemClicked);
            this.CommSubMenuitem_ReorderColumn.DropDownItemClicked += CommSubMenuitem_ReorderColumn_DropDownItemClicked;

            toolStripDropDownButton1.ToolTipText = "搜索表、字段、备注";

            this.Controls.Add(UCSearchOptions);
            TSPShowOptions.Image = Resources.Resource1.cog;
            TSPShowOptions.AutoToolTip = false;
            TSPShowOptions.DropDownOpening += TSPShowOptions_DropDownOpening;
            TSPShowOptions.DropDownClosed += TSPShowOptions_DropDownClosed;
        }

        private void TSPShowOptions_DropDownClosed(object sender, EventArgs e)
        {
            UCSearchOptions.Visible = false;
        }

        private void TSPShowOptions_DropDownOpening(object sender, EventArgs e)
        {
            if (UCSearchOptions.Visible)
            {
                UCSearchOptions.Visible = false;
            }
            else
            {
                UCSearchOptions.Show();
                var loc = toolStrip1.Location;
                loc.Offset(10, TSPShowOptions.Height + 2);
                UCSearchOptions.Location = loc;
                UCSearchOptions.BringToFront();
            }
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
                CommMenuStrip.Hide();
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
                            OnAddEntityTB(GetDBSource(node), GetDBName(node));
                        }
                        break;
                    case "删除对象":
                        if (MessageBox.Show("确认要删除数据库" + tv_DBServers.SelectedNode.Text + "吗？", "询问",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            var node = tv_DBServers.SelectedNode;
                            Biz.Common.Data.SQLHelper.DeleteDataBase(GetDBSource(node), node.Text);
                            ReLoadDBObj(node.Parent);

                            BigEntityTableRemotingEngine.Insert("HLog", new HLogEntity
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
                        {
                            var selnode = tv_DBServers.SelectedNode;
                            var dlg = new SubForm.InputStringDlg("请输入库名：", isMult: false);
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                Biz.Common.Data.SQLHelper.CreateDataBase(GetDBSource(selnode), selnode.FirstNode?.Text, dlg.InputString);
                                ReLoadDBObj(selnode);

                                BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
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
                        }
                    case "从...迁移":
                        {
                            var selNode = tv_DBServers.SelectedNode;
                            if (selNode == null)
                            {
                                return;
                            }
                            var dbsource = GetDBSource(selNode);
                            var dlg = new SubForm.InputStringDlg("旧服务器", isMult: false);
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                var oldserver = dlg.InputString.Trim();
                                if (dbsource.ServerName.Equals(oldserver, StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show("服务器没改");
                                    return;
                                }
                                if (MessageBox.Show($"将旧服务器上的本地数据转到新服务器({oldserver}->{dbsource.ServerName})？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        if (!BigEntityTableRemotingEngine.BackUp(null))
                                        {
                                            MessageBox.Show("备份失败");
                                            return;
                                        }
                                        int count = 0;
                                        int failcount = 0;
                                        var markColumnInfoList = BigEntityTableRemotingEngine.Find<MarkObjectInfo>(nameof(MarkObjectInfo), p => p.Servername.Equals(dlg.InputString.Trim(), StringComparison.OrdinalIgnoreCase), limit: int.MaxValue).ToList();
                                        foreach (var item in markColumnInfoList)
                                        {
                                            item.Servername = dbsource.ServerName;
                                            try
                                            {
                                                count++;
                                                BigEntityTableRemotingEngine.Update<MarkObjectInfo>(nameof(MarkObjectInfo), item);
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHelper.Instance.Error("更换Servername失败：" + Newtonsoft.Json.JsonConvert.SerializeObject(item), ex);
                                                failcount++;
                                            }
                                        }

                                        var spInfoList = BigEntityTableRemotingEngine.Find<SPInfo>(nameof(SPInfo), p => p.Servername.Equals(dlg.InputString.Trim(), StringComparison.OrdinalIgnoreCase), limit: int.MaxValue).ToList();
                                        foreach (var item in spInfoList)
                                        {
                                            item.Servername = dbsource.ServerName;
                                            try
                                            {
                                                count++;
                                                BigEntityTableRemotingEngine.Update(nameof(SPInfo), item);
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHelper.Instance.Error("更换Servername失败：" + Newtonsoft.Json.JsonConvert.SerializeObject(item), ex);
                                                failcount++;
                                            }
                                        }
                                        MessageBox.Show($"迁移成功{count}条记录");
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("操作失败:" + ex.Message);
                                    }
                                }
                            }
                            break;
                        }
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
                    case "筛选函数":
                        {
                            FilterFunction();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private NodeContentType GetNodeContentType(TreeNode node)
        {
            var nc = node.Tag as INodeContents;
            NodeContentType nctype = NodeContentType.UNKNOWN;
            if (nc != null)
            {
                nctype = nc.GetNodeContentType();
            }

            return nctype;
        }

        async Task<bool> ReLoadDBObj(TreeNode selNode, bool loadall = false,bool ansy=true)
        {
            //TreeNode selNode = tv_DBServers.SelectedNode;
            if (selNode == null || selNode.Tag == null)
                return false;

            List<IAsyncResult> asyncResults = new List<IAsyncResult>();
            AsyncCallback callback = null;
            if (loadall)
            {
                callback = new AsyncCallback((o) =>
                {
                    var node = selNode;
                    foreach (TreeNode c in node.Nodes)
                    {
                        var ar = this.BeginInvoke(new Action(() =>
                          {
                              ReLoadDBObj(c, loadall, ansy);
                          }));
                        if (!ansy)
                        {
                            ar.AsyncWaitHandle.WaitOne();
                        }
                    }
                });
            }
            if (selNode.Tag is ServerInfo)
            {
                var ar = Biz.UILoadHelper.LoadDBsAnsy(this.ParentForm, selNode, GetDBSource(selNode), callback, selNode);
                asyncResults.Add(ar);
                selNode.Parent.Expand();
            }
            else if (selNode.Tag is DBInfo)
            {
                if (loadall)
                {
                    var node = selNode;
                    foreach (TreeNode c in node.Nodes)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            ReLoadDBObj(c, loadall, ansy);
                        }));
                    }
                }
            }
            else if (GetNodeContentType(selNode) == NodeContentType.TBParent)
            {
                var dbname = GetDBName(selNode).ToUpper();
                var ar = Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, selNode, GetDBSource(selNode), dbname, name =>
                   {
                       var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                    [] { dbname, name.ToUpper(), string.Empty }).FirstOrDefault();
                       return mark == null ? string.Empty : mark.MarkInfo;
                   }, callback, selNode);
                asyncResults.Add(ar);
            }
            else if (GetNodeContentType(selNode) == NodeContentType.PROCParent)
            {
                var dbname = GetDBName(selNode).ToUpper();
                var ar = Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, selNode, GetDBSource(selNode), p =>
                   {
                       var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, p.ToUpper() }).FirstOrDefault();

                       if (item != null)
                       {
                           return item.Mark;
                       }
                       return string.Empty;
                   });
                asyncResults.Add(ar);
            }
            else if (GetNodeContentType(selNode) == NodeContentType.FUNPARENT)
            {
                var dbname = GetDBName(selNode).ToUpper();
                var ar = Biz.UILoadHelper.LoadFunctionsAnsy(this.ParentForm, selNode, GetDBSource(selNode), p =>
                  {
                      var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, p.ToUpper() }).FirstOrDefault();

                      if (item != null)
                      {
                          return item.Mark;
                      }
                      return string.Empty;
                  });
                asyncResults.Add(ar);
            }
            else if (GetNodeContentType(selNode) == NodeContentType.VIEWParent)
            {
                var ar = Biz.UILoadHelper.LoadViewsAnsy(this.ParentForm, selNode, GetDBSource(selNode), (view) =>
                  {
                      var dbname = GetDBName(selNode).ToUpper();
                      var dbsource = GetDBSource(selNode);
                      var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                  [] { dbname, view.Name.ToUpper(), string.Empty }).FirstOrDefault();

                      return mark == null ? string.Empty : mark.MarkInfo;
                  }, (col) =>
                  {
                      var dbname = GetDBName(selNode).ToUpper();
                      var dbsource = GetDBSource(selNode);
                      var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                  [] { dbname, col.TBName.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();

                      return mark == null ? string.Empty : mark.MarkInfo;
                  });
                asyncResults.Add(ar);
            }
            else if (selNode.Tag is TableInfo)
            {
                var dbname = GetDBName(selNode).ToUpper();
                var dbsource = GetDBSource(selNode);
                var synccolumnmark = BigEntityTableRemotingEngine.
                    Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { dbname, selNode.Text.ToUpper() }).FirstOrDefault() != null;
                var ar = Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, selNode, dbsource, (col) =>
                   {
                       var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                   [] { dbname, selNode.Text.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();

                       if (mark == null && !synccolumnmark)
                       {
                           BigEntityTableRemotingEngine.Insert("MarkObjectInfo", new MarkObjectInfo
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
                asyncResults.Add(ar);

                if (!synccolumnmark)
                {
                    BigEntityTableRemotingEngine.Insert<ColumnMarkSyncRecord>("ColumnMarkSyncRecord",
                        new ColumnMarkSyncRecord
                        {
                            DBName = dbname.ToUpper(),
                            SyncDate = DateTime.Now,
                            TBName = selNode.Text.ToUpper()
                        });
                }

                BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = selNode.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.table,
                    DB = dbname,
                    Sever = GetDBSource(selNode).ServerName,
                    Valid = true
                });
            }
            else if (GetNodeContentType(selNode) == NodeContentType.INDEXParent)
            {
                var ar = Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, selNode, GetDBSource(selNode), GetDBName(selNode));
                asyncResults.Add(ar);
            }
            else if (GetNodeContentType(selNode) == NodeContentType.TRIGGERPARENT)
            {
                var ar = Biz.UILoadHelper.LoadTriggersAnsy(this.ParentForm, selNode, GetDBSource(selNode), GetDBName(selNode));
                asyncResults.Add(ar);
            }
            else if (GetNodeContentType(selNode) == NodeContentType.LOGICMAPParent)
            {
                var ar = Biz.UILoadHelper.LoadLogicMapsAnsy(this.ParentForm, selNode, GetDBName(selNode));
                asyncResults.Add(ar);
            }

            if (asyncResults.Any() && !ansy)
            {
                var t = Task.Factory.StartNew(new Func<bool>(()=> asyncResults.Select(p => p.AsyncWaitHandle).All(p => p.WaitOne())));
                await t;
            }

            return true;
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
                            BigEntityTableRemotingEngine.Insert("HLog", new HLogEntity
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

                            BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
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
                                this.OnCreatePorcSQL(GetDBSource(_node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName,tableinfo.Schema, CreateProceEnum.Insert);
                            }
                            break;
                        }
                    case "BatchInsert":
                        {
                            _node = tv_DBServers.SelectedNode;
                            if (_node.Tag is TableInfo && this.OnCreatePorcSQL != null)
                            {
                                var tableinfo = _node.Tag as TableInfo;
                                this.OnCreatePorcSQL(GetDBSource(_node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName,tableinfo.Schema, CreateProceEnum.BatchInsert);
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
                                if (selnode.Tag is ProcInfo)
                                {
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
                                else if (selnode.Tag is FunInfo)
                                {
                                    var funInfo = ((FunInfo)selnode.Tag);
                                    StringBuilder sb = new StringBuilder();
                                    if (funInfo.IsTableValue)
                                    {
                                        sb.Append($"select * from {funInfo.Name}(");
                                    }
                                    else
                                    {
                                        sb.Append($"exec ");
                                    }
                                    StringBuilder sb1 = new StringBuilder($"use {GetDBName(selnode)}");
                                    sb1.AppendLine();
                                    sb1.AppendLine("GO");
                                    StringBuilder sb2 = new StringBuilder();
                                    StringBuilder sb3 = new StringBuilder();
                                    for(var i=0;i<funInfo.FuncParamInfos.Count;i++)
                                    {
                                        if (funInfo.IsTableValue)
                                        {
                                            if (i == 0 && funInfo.FuncParamInfos.Any(p => !p.IsOutparam))
                                            {
                                                sb2.Append("declare ");
                                            }

                                            var pa = funInfo.FuncParamInfos[i];
                                            if (pa.IsOutparam)
                                            {

                                            }
                                            else
                                            {
                                                sb.Append($"{pa.Name},");
                                                sb2.AppendLine($"{pa.Name} {(Biz.Common.Data.Common.GetDBType(new TBColumn { Name = pa.Name, TypeName = pa.TypeName, Length = pa.Len }))}=<{(Biz.Common.Data.Common.GetDBType(new TBColumn { Name = pa.Name, TypeName = pa.TypeName, Length = pa.Len }))}>,");
                                            }
                                        }
                                        else
                                        {
                                            if (i == 0 && funInfo.FuncParamInfos.Any(p => p.IsOutparam))
                                            {
                                                sb2.Append("declare ");
                                            }

                                            var pa = funInfo.FuncParamInfos[i];
                                            if (pa.IsOutparam)
                                            {
                                                sb.AppendLine($"{funInfo.Name}");
                                                var pname = pa.Name;
                                                if (string.IsNullOrEmpty(pname))
                                                {
                                                    pname = $"@ret{i}";
                                                }
                                                sb2.AppendLine($"{pname} {(Biz.Common.Data.Common.GetDBType(new TBColumn { Name = pname, TypeName = pa.TypeName, Length = pa.Len }))},");

                                                sb3.AppendLine($"SELECT {pname} AS 'Return Result'");
                                            }
                                            else
                                            {
                                                sb.AppendLine($"{pa.Name}=<{(Biz.Common.Data.Common.GetDBType(new TBColumn { Name = pa.Name, TypeName = pa.TypeName, Length = pa.Len }))}>,");
                                            }
                                        }
                                    }

                                    if (sb.Length > 2 && sb[sb.Length - 1] == ',')
                                    {
                                        sb = sb.Remove(sb.Length - 1, 1);
                                    }

                                    if (sb.Length > 2 && sb[sb.Length - 3] == ',')
                                    {
                                        sb = sb.Remove(sb.Length - 3, 1);

                                    }

                                    if (funInfo.IsTableValue)
                                    {
                                        sb.Append(")");
                                    }

                                    if (sb2.Length > 2 && sb2[sb2.Length - 3] == ',')
                                    {
                                        sb2 = sb2.Remove(sb2.Length - 3, 1);
                                    }

                                    var sql = sb1.ToString() + sb2.ToString() + sb.ToString() + sb3.ToString();

                                    OnExecutSql(GetDBSource(selnode), GetDBName(selnode), sql);
                                }
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

                var schema = "";
                if (!"dbo".Equals(tb.Schema, StringComparison.OrdinalIgnoreCase))
                {
                    schema = "[" + tb.Schema + "].";
                }
                sb.Append(string.Join(",\r\n", cols.Select(p => "[" + p.Key + "]")));
                sb.AppendLine("");
                sb.Append(" from ");
                sb.Append(schema + "[" + tv_DBServers.SelectedNode.Text + "]");
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
                var tbOwner = (tv_DBServers.SelectedNode.Tag as TableInfo)?.Schema;

                var tp = DataHelper.CreateTableEntity(dbsource, dbname, tbname, tbOwner, tid, DefaultEntityNamespace, tv_DBServers.SelectedNode.Tag is ViewInfo,
                    isSupportProtobuf, isSupportDBMapperAttr, isSupportJsonproterty, isSupportMvcDisplay, dlg.ReaderEntityCode, name =>
                      {
                          var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                   [] { dbname.ToUpper(), tbname.ToUpper(), name.ToUpper() }).FirstOrDefault();

                          return mark == null ? name : mark.MarkInfo;

                      }, out hasKey);
                var classcode = tp.Item1;
                if (OnCreateEntity != null)
                {
                    OnCreateEntity("实体类" + tbname, classcode);
                }
                Clipboard.SetText(classcode);
                MainFrm.SendMsg(string.Format("实体代码已经复制到剪贴板,{0}", hasKey ? "" : "警告：表没有自增主键。"));
                if (dlg.ReaderEntityCode)
                {
                    new SubForm.TextBoxWin("reader " + dbname + "." + tbname, tp.Item2).Show();
                }
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
            {
                return (node.Tag as TableInfo).TBName;
            }
            else if (node.Tag is ViewInfo)
            {
                return (node.Tag as ViewInfo).Name;
            }
            return GetTBName(node.Parent);
        }

        private string GetDBName(TreeNode node)
        {
            return GetDB(node)?.Name;
        }

        private DBInfo GetDB(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Tag is DBInfo)
            {
                return node.Tag as DBInfo;
            }
            return GetDB(node.Parent);
        }

        void tv_DBServers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //throw new NotImplementedException();
            if (GetNodeContentType(e.Node) == NodeContentType.TBParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;

                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), GetDBName(e.Node), name =>
                  {
                      var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                   [] { GetDBName(e.Node).ToUpper(), name.ToUpper(), string.Empty }).FirstOrDefault();
                      return mark == null ? string.Empty : mark.MarkInfo;
                  }, null, null);

                BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = e.Node.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.db,
                    DB = e.Node.Text,
                    Sever = GetDBSource(e.Node).ServerName,
                    Valid = true
                });
            }
            else if (GetNodeContentType(e.Node) == NodeContentType.FUNPARENT)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                var dbname = GetDBName(e.Node).ToUpper();
                Biz.UILoadHelper.LoadFunctionsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), p =>
                {
                    var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, p.ToUpper() }).FirstOrDefault();

                    if (item != null)
                    {
                        return item.Mark;
                    }
                    return string.Empty;
                });
            }
            else if (GetNodeContentType(e.Node) == NodeContentType.PROCParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                var dbname = GetDBName(e.Node).ToUpper();
                Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), p =>
                {
                    var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, p.ToUpper() }).FirstOrDefault();

                    if (item != null)
                    {
                        return item.Mark;
                    }
                    return string.Empty;
                });
            }
            else if (GetNodeContentType(e.Node) == NodeContentType.VIEWParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadViewsAnsy(ParentForm, e.Node, GetDBSource(e.Node), (view) =>
                {
                    var dbname = GetDBName(e.Node).ToUpper();
                    var dbsource = GetDBSource(e.Node);
                    var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { dbname, view.Name.ToUpper(), string.Empty }).FirstOrDefault();

                    return mark == null ? string.Empty : mark.MarkInfo;
                }, (col) =>
                 {
                     var dbname = GetDBName(e.Node).ToUpper();
                     var dbsource = GetDBSource(e.Node);
                     var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                 [] { dbname, col.TBName.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();

                     return mark == null ? string.Empty : mark.MarkInfo;
                 });
            }
            else if (e.Node.Tag is TableInfo)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                var tb = e.Node.Tag as TableInfo;
                var dbname = GetDBName(e.Node).ToUpper();
                var dbsource = GetDBSource(e.Node);
                var synccolumnmark = BigEntityTableRemotingEngine.
                   Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { dbname, tb.TBName.ToUpper() }).FirstOrDefault() != null;
                Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), (col) =>
                {
                    var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { dbname, tb.TBName.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();
                    if (mark == null && !synccolumnmark)
                    {
                        BigEntityTableRemotingEngine.Insert<MarkObjectInfo>("MarkObjectInfo", new MarkObjectInfo
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
                    BigEntityTableRemotingEngine.Insert<ColumnMarkSyncRecord>("ColumnMarkSyncRecord",
                        new ColumnMarkSyncRecord
                        {
                            DBName = dbname.ToUpper(),
                            SyncDate = DateTime.Now,
                            TBName = tb.TBName.ToUpper()
                        });
                }

                BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = e.Node.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.table,
                    DB = dbname,
                    Sever = GetDBSource(e.Node).ServerName,
                    Valid = true
                });
            }
            else if (GetNodeContentType(e.Node) == NodeContentType.INDEXParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), GetDBName(e.Node));
            }
            else if (GetNodeContentType(e.Node) == NodeContentType.TRIGGERPARENT)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadTriggersAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), GetDBName(e.Node));
            }
            else if (GetNodeContentType(e.Node) == NodeContentType.LOGICMAPParent)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                Biz.UILoadHelper.LoadLogicMapsAnsy(this.ParentForm, e.Node, GetDBName(e.Node));
            }
        }


        private void Tv_DBServers_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (GetNodeContentType(e.Node) == NodeContentType.LOGICMAP)
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
            ReLoadDBObj(tv_DBServers.Nodes[0]);
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
                    NodeContentType nctype = GetNodeContentType(node);

                    if (nctype == NodeContentType.TB
                        || nctype == NodeContentType.PROC
                        || nctype == NodeContentType.FUN
                        || nctype == NodeContentType.VIEW
                        || nctype == NodeContentType.INDEX
                        || nctype == NodeContentType.INDEXParent
                        || nctype == NodeContentType.TRIGGERPARENT
                        || nctype == NodeContentType.TRIGGER
                        || nctype == NodeContentType.LOGICMAPParent
                        || nctype == NodeContentType.LOGICMAP)
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
                            || nctype == NodeContentType.TRIGGERPARENT
                            || nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.VIEWParent
                            || nctype == NodeContentType.PROCParent
                            || nctype == NodeContentType.PROC
                            || nctype == NodeContentType.FUNPARENT
                            || nctype == NodeContentType.FUN
                            || nctype == NodeContentType.LOGICMAPParent;

                        导出ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.PROC
                            || nctype == NodeContentType.FUN
                            || nctype == NodeContentType.TRIGGER
                            || nctype == NodeContentType.TB;
                        CreateMSSQLToolStripMenuItem.Visible = nctype == NodeContentType.PROC
                            || nctype == NodeContentType.FUN
                            || nctype == NodeContentType.TRIGGER
                            || nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB;
                        创建语句ToolStripMenuItem.Visible = 数据MSSQLToolStripMenuItem.Visible =
                            ExpdataToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        生成实体类ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB;
                        复制表名ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB
                            || nctype == NodeContentType.INDEX
                            || nctype == NodeContentType.TRIGGER
                            || nctype == NodeContentType.LOGICMAP;
                        显示前100条数据ToolStripMenuItem.Visible = nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.TB;
                        清理备注ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        备注ToolStripMenuItem.Visible = nctype == NodeContentType.TB
                            || nctype == NodeContentType.VIEW
                            || nctype == NodeContentType.PROC
                            || nctype == NodeContentType.FUN;
                        表关系图ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        TSMI_ExeProc.Visible = nctype == NodeContentType.PROC || nctype == NodeContentType.FUN;
                        生成数据字典ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        SyncDataToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        修改表名ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        删除表ToolStripMenuItem.Visible = nctype == NodeContentType.TB;
                        SubMenuItem_Proc.Visible = nctype == NodeContentType.TB;
                        新增逻辑关系图ToolStripMenuItem.Visible = nctype == NodeContentType.LOGICMAPParent;
                        删除逻辑关系图ToolStripMenuItem.Visible = nctype == NodeContentType.LOGICMAP;
                        复制逻辑关系图ToolStripMenuItem.Visible= nctype == NodeContentType.LOGICMAP;

                        TSM_ManIndex.Visible = nctype == NodeContentType.INDEX
                        || nctype == NodeContentType.INDEXParent;
                        TTSM_CreateIndex.Visible = nctype == NodeContentType.INDEXParent;
                        TTSM_DelIndex.Visible = nctype == NodeContentType.INDEX;
                    }
                    else
                    {
                        this.tv_DBServers.ContextMenuStrip = this.CommMenuStrip;
                        subMenuItemAddEntityTB.Visible = nctype == NodeContentType.TBParent;
                        TSMI_ViewTableList.Visible = nctype == NodeContentType.DB;
                        TSMI_ViewColumnList.Visible = nctype == NodeContentType.DB;
                        CommSubMenuItem_Delete.Visible = nctype == NodeContentType.DB;
                        CommSubMenuitem_add.Visible = nctype == NodeContentType.SEVER;
                        ServerMoveToolStripMenuItem.Visible = nctype == NodeContentType.SEVER;
                        CommSubMenuitem_ViewConnsql.Visible = nctype == NodeContentType.DB;
                        CommSubMenuitem_ReorderColumn.Visible = nctype == NodeContentType.COLUMN;
                        备注本地ToolStripMenuItem.Visible = nctype == NodeContentType.COLUMN
                            || nctype == NodeContentType.VIEWCOLUMN
                            || nctype == NodeContentType.DB;
                        TSMI_MulMarkLocal.Visible = nctype == NodeContentType.COLUMN;
                        TSMI_FilterProc.Visible = nctype == NodeContentType.PROCParent;
                        TSMI_FilterFunction.Visible = nctype == NodeContentType.FUNPARENT;
                        过滤显示ToolStripMenuItem.Visible = nctype == NodeContentType.SEVER;
                    }
                }
            }
        }

        private T OpCtrol<T>(Func<T> func)
        {
            if (InvokeRequired)
            {
                T t = default;
                var ar = this.BeginInvoke(new Action(()=>t=func()));
                ar.AsyncWaitHandle.WaitOne();
                return t;
            }
            return func();
        }

        private List<TreeNode> SearchAllResults = new List<TreeNode>();
        private TreeNode SearchStartTreeNode = null;
        private TreeNode SearchLastNode = null;

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            string serchkey = ts_serchKey.Text;

            bool matchall = UCSearchOptions.IsMatchAll;
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

            if (!UCSearchOptions.GlobSearch && SearchStartTreeNode == null)
            {
                SearchStartTreeNode = tv_DBServers.SelectedNode;
            }

            searchingWaitingBox.Msg = "搜索中...";
            var oldLoadedExpand = UILoadHelper.LoadedExpand;
            UILoadHelper.LoadedExpand = false;
            searchingWaitingBox.Waiting(this, () =>
            {
                int boo = 0;
                while (true)
                {
                    var nextNode = getNextNode();
                    if (nextNode != null)
                    {
                        boo = SearchNode(nextNode, serchkey, matchall, true).Result;
                        if (boo == 1)
                        {
                            UILoadHelper.LoadedExpand = oldLoadedExpand;
                            break;
                        }
                        else if (boo == 2)
                        {
                            searchWaitEvent.WaitOne();
                        }
                        else if (boo == 3)
                        {
                            //继续
                        }
                        else
                        {
                            finishSearch();
                            break;
                        }
                    }
                    else
                    {
                        finishSearch();
                        break;
                    }
                }
                showResult();
            },()=> { UILoadHelper.LoadedExpand = oldLoadedExpand; });

            TreeNode getNextNode()
            {
                return OpCtrol(() =>
                {
                    TreeNode node = null;
                    var currNode = SearchLastNode;
                    if (currNode == null)
                    {
                        currNode = tv_DBServers.SelectedNode;
                    }
                    if (currNode != null)
                    {
                        if (currNode.Nodes.Count > 0)
                        {
                            node = currNode.Nodes[0];
                        }
                        else if (currNode.NextNode != null)
                        {
                            node = currNode.NextNode;
                        }

                        if (node == null)
                        {
                            var parentNode = currNode.Parent;
                            while (parentNode != null && parentNode.Level > SearchStartTreeNode.Level)
                            {
                                if (parentNode.NextNode != null)
                                {
                                    node = parentNode.NextNode;
                                    break;
                                }
                                else
                                {
                                    parentNode = parentNode.Parent;
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                    return node;
                });
            }

            void showResult()
            {
                if (SearchAllResults.Any())
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        SearchResultsDlg dlg = new SearchResultsDlg();
                        dlg.Ds = SearchAllResults.Select(p =>
                        {
                            var dbSource = GetDBSource(p);
                            var dbName = GetDBName(p);
                            var tbName = GetTBName(p);
                            return new
                            {
                                server = dbSource.ServerName,
                                db = dbName,
                                tb = tbName,
                                type = GetNodeContentType(p),
                                text = p.Text,
                                obj = p
                            };
                        }).ToList();
                        dlg.Choose += node => OpCtrol(() =>
                        {
                            tv_DBServers.SelectedNode = node;
                            return tv_DBServers.SelectedNode == node;
                        });

                        SearchAllResults.Clear();
                        searchedNodes.Clear();
                        dlg.Show();
                    }));
                };
            }

            void finishSearch()
            {
                UILoadHelper.LoadedExpand = oldLoadedExpand;
                this.BeginInvoke(new Action(() =>
                {
                    if (SearchStartTreeNode != null)
                    {
                        tv_DBServers.SelectedNode = SearchStartTreeNode;
                        SearchStartTreeNode = null;
                    }
                    else
                    {
                        tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
                    }
                }));

                SearchLastNode = null;
            }
        }

        private HashSet<TreeNode> searchedNodes = new HashSet<TreeNode>();


        private async Task<int> SearchNode(TreeNode nodeStart, string txt, bool matchall, bool maxsearch)
        {
            if (nodeStart == null)
            {
                return await Task.FromResult(0);
            }

            //if (searchedNodes.Contains(nodeStart))
            //{

            //}
            //else
            //{
            //    searchedNodes.Add(nodeStart);
            //}

            if (!UCSearchOptions.GlobSearch && nodeStart.Level <= SearchStartTreeNode.Level)
            {
                return 0;
            }

            SearchLastNode = nodeStart;

            var nodeType = GetNodeContentType(nodeStart);

            var find = matchall ? (nodeStart.Text.Equals(txt, StringComparison.OrdinalIgnoreCase)
                || nodeStart.ToolTipText?.Equals(txt, StringComparison.OrdinalIgnoreCase) == true) :
                (nodeStart.Text.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1
                || nodeStart.ToolTipText?.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1);
            if (find)
            {
                if (Filter(nodeType))
                {
                    if (UCSearchOptions.SearchComplete)
                    {
                        SearchAllResults.Add(nodeStart);
                        SearchLastNode = nodeStart;
                        return 3;
                    }
                    else
                    {
                        this.BeginInvoke(new Action(() => tv_DBServers.SelectedNode = nodeStart));
                        return 1;
                    }

                }
            }

            if (nodeStart.Nodes.Count > 0)
            {
                foreach (TreeNode node in nodeStart.Nodes)
                {
                    var ret = SearchNode(node, txt, matchall, true).Result;
                    if (ret != 0)
                    {
                        return ret;
                    }
                }
            }
            else if (GetDB(nodeStart) != null && GetDB(nodeStart).State == DBState.ONLINE)
            {
                var dbSource = GetDBSource(nodeStart);
                var exists = false;
                var loadAll = false;

                if (nodeType == NodeContentType.DB)
                {

                    if (UCSearchOptions.HardSearch)
                    {
                        exists = SQLHelper.GetDBs(dbSource).Rows.Count > 0;
                    }
                    else
                    {
                        exists = (Filter(NodeContentType.DB) || Filter(NodeContentType.TB) || Filter(NodeContentType.COLUMN)) && LocalDBHelper.GetAllMarkObjectInfoFromCach().Exists(p => p.Servername.Equals(dbSource.ServerName, StringComparison.OrdinalIgnoreCase) &&
                              (matchall ? ((p.DBName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.TBName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.ColumnName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.MarkInfo ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase))
                              : ((p.DBName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.TBName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.ColumnName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.MarkInfo ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1)));
                    }
                }
                else if (nodeType == NodeContentType.TBParent && (Filter(NodeContentType.TB) || Filter(NodeContentType.COLUMN) || Filter(NodeContentType.INDEX)))
                {
                    if (UCSearchOptions.HardSearch)
                    {
                        exists = SQLHelper.GetTBs(dbSource, GetDBName(nodeStart)).Rows.Count > 0;
                    }
                    else
                    {
                        exists = LocalDBHelper.GetAllMarkObjectInfoFromCach().Exists(p => p.Servername.Equals(dbSource.ServerName, StringComparison.OrdinalIgnoreCase) && p.DBName.Equals(GetDBName(nodeStart), StringComparison.OrdinalIgnoreCase) &&
                            (matchall ? ((p.TBName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.ColumnName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.MarkInfo ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase))
                            : ((p.TBName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.ColumnName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.MarkInfo ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1)));
                    }

                }
                else if (nodeType == NodeContentType.VIEWParent)
                {
                    if (UCSearchOptions.HardSearch)
                    {
                        exists = SQLHelper.GetViews(dbSource, GetDBName(nodeStart)).Any();
                    }
                    else
                    {
                        exists = Filter(NodeContentType.VIEW) && SQLHelper.GetViews(dbSource, GetDBName(nodeStart)).Any();
                    }
                }
                else if (nodeType == NodeContentType.TB && (Filter(NodeContentType.COLUMN) || Filter(NodeContentType.INDEX)))
                {
                    if (UCSearchOptions.HardSearch)
                    {
                        exists = SQLHelper.GetColumns(dbSource, GetDBName(nodeStart), GetTBName(nodeStart), (nodeStart.Tag as TableInfo).Schema).Any();
                    }
                    else
                    {
                        exists = (Filter(NodeContentType.TB) || Filter(NodeContentType.COLUMN)) && LocalDBHelper.GetMarkObjectInfoFromCach(dbSource.ServerName, GetDBName(nodeStart), GetTBName(nodeStart)).Exists(p =>
                               matchall ? ((p.ColumnName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.MarkInfo ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase))
                               : ((p.ColumnName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.MarkInfo ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1));
                    }
                    loadAll = true;
                }
                else if (nodeType == NodeContentType.PROCParent)
                {
                    exists = Filter(NodeContentType.PROC) && SQLHelper.GetProcedures(dbSource, GetDBName(nodeStart)).Any();
                    //exists = LocalDBHelper.GetAllSPInfoFromCach().Exists(p => p.Servername.Equals(dbSource.ServerName, StringComparison.OrdinalIgnoreCase) && p.DBName.Equals(GetDBName(nodeStart), StringComparison.OrdinalIgnoreCase) &&
                    // (matchall ? ((p.SPName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.Mark ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase))
                    // : ((p.SPName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.Mark ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1)));
                }
                else if (nodeType == NodeContentType.FUNPARENT)
                {
                    exists = Filter(NodeContentType.FUN) && SQLHelper.GetFunctions(dbSource, GetDBName(nodeStart)).Rows.Count > 0;
                    //exists = LocalDBHelper.GetAllSPInfoFromCach().Exists(p => p.Servername.Equals(dbSource.ServerName, StringComparison.OrdinalIgnoreCase) && p.DBName.Equals(GetDBName(nodeStart), StringComparison.OrdinalIgnoreCase) &&
                    // (matchall ? ((p.SPName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase) || (p.Mark ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase))
                    // : ((p.SPName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1 || (p.Mark ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1)));
                }
                else if (nodeType == NodeContentType.LOGICMAPParent)
                {
                    exists = Filter(NodeContentType.LOGICMAP) && LocalDBHelper.GetAllLogicMapFromCach().Exists(p => p.DBName.Equals(GetDBName(nodeStart), StringComparison.OrdinalIgnoreCase) &&
                    (matchall ? ((p.LogicName ?? string.Empty).Equals(txt, StringComparison.OrdinalIgnoreCase))
                     : ((p.LogicName ?? string.Empty).IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1)));
                }
                else if (nodeType == NodeContentType.TRIGGERPARENT)
                {
                    //
                    exists = Filter(NodeContentType.TRIGGER) && SQLHelper.GetTriggers(dbSource, GetDBName(nodeStart), GetTBName(nodeStart)).Any();

                }

                if (exists)
                {
                    searchWaitEvent.Reset();
                    var t = ReLoadDBObj(nodeStart, loadAll, false);
                    _ = new Action(async () =>
                      {
                          await t;
                          //if (nodeStart.Nodes.Count > 0)
                          //{
                          //    this.BeginInvoke(new Action(() =>
                          //    {
                          //        SearchLastNode = nodeStart.Nodes[0];
                          //        //tv_DBServers.SelectedNode = nodeStart.Nodes[0];
                          //        //toolStripDropDownButton1_Click(null, null);
                          //    }));
                          //}
                          searchWaitEvent.Set();
                      }).BeginInvoke(null, null);
                    return 2;
                }
                else
                {
                    return 3;
                }
            }

            if (maxsearch)
            {
                if (nodeStart.NextNode != null)
                {
                    return SearchNode(nodeStart.NextNode, txt, matchall, true).Result;
                }
                else
                {
                    var parent = nodeStart.Parent;
                    while (parent != null && parent.NextNode == null)
                    {
                        parent = parent.Parent;
                    }
                    if (parent != null)
                    {
                        return SearchNode(parent.NextNode, txt, matchall, true).Result;
                    }
                }
            }

            return 0;

            bool Filter(NodeContentType nodeContentType)
            {
                if (UCSearchOptions.SearchDB && UCSearchOptions.SearchTB && UCSearchOptions.SearchField && UCSearchOptions.SearchProc && UCSearchOptions.SearchFunc &&
                    UCSearchOptions.SearchOther)
                {
                    return true;
                }
                switch (nodeContentType)
                {
                    case NodeContentType.DB:
                        {
                            return UCSearchOptions.SearchDB;
                        }
                    case NodeContentType.TB:
                        {
                            return UCSearchOptions.SearchTB;
                        }
                    case NodeContentType.COLUMN:
                        {
                            return UCSearchOptions.SearchField;
                        }
                    case NodeContentType.PROC:
                        {
                            return UCSearchOptions.SearchProc;
                        }
                    case NodeContentType.FUN:
                        {
                            return UCSearchOptions.SearchFunc;
                        }
                    case NodeContentType.VIEW:
                        {
                            return UCSearchOptions.SearchView;
                        }
                    default:
                        {
                            return UCSearchOptions.SearchOther;
                        }
                }
            }
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

        public TreeNode FindNode(TreeNode node, NodeContentType nodeContentType)
        {
            if (GetNodeContentType(node) == nodeContentType)
            {
                return node;
            }

            foreach(TreeNode n in node.Nodes)
            {
                var fn = FindNode(n, nodeContentType);
                if (fn != null)
                {
                    return fn;
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
                this.OnCreatePorcSQL(GetDBSource(_node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName,tableinfo.Schema, CreateProceEnum.Insert);
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
            foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName,tableinfo.Schema))
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
            var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, tableinfo.Schema);
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

            var cols = Biz.Common.Data.SQLHelper.GetColumns(dbsource, dbname, tid, tbname,tableinfo.Schema).ToList();

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

                var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, tableinfo.Schema).ToList();
                var tableDesc = SQLHelper.GetTableDescription(GetDBSource(node), tableinfo.DBName, tableinfo.TBName);

                var colDesc = SQLHelper.GetTableColsDescription(GetDBSource(node), tableinfo.DBName, tableinfo.TBName);

                TextBoxWin win = new TextBoxWin("创建表" + node.Text, DataHelper.GetCreateTableSQL(tableinfo, cols, SQLHelper.GetIndexDDL(GetDBSource(node), tableinfo.DBName, tableinfo.TBName), tableDesc, colDesc));
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

                    BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
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
            else if (node != null && node.Tag is FunInfo)
            {
                if (OnShowProc != null)
                {
                    var procinfo = node.Tag as FunInfo;
                    var dbname = GetDBName(node);
                    var body = Biz.Common.Data.SQLHelper.GetFunctionBody(GetDBSource(node), dbname, procinfo.Name);
                    //TextBoxWin win = new TextBoxWin("存储过程[" + node.Text + "]", body);
                    //win.Show();
                    OnShowProc(GetDBSource(node), dbname, procinfo.Name, body);

                    BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
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
            else if (node != null && node.Tag is TriggerEntity)
            {
                if (OnShowProc != null)
                {
                    var procinfo = node.Tag as TriggerEntity;
                    var dbname = GetDBName(node);
                    var body = Biz.Common.Data.SQLHelper.GetTriggerBody(GetDBSource(node), dbname, procinfo.TriggerName);
                    //TextBoxWin win = new TextBoxWin("存储过程[" + node.Text + "]", body);
                    //win.Show();
                    OnShowProc(GetDBSource(node), dbname, procinfo.TriggerName, body);

                    BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
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

                BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
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

            var cols = Biz.Common.Data.SQLHelper.GetColumns(GetDBSource(node), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, tableinfo.Schema).ToList();
            
            if (cols.ToList().Exists(p => p.IsID))
            {
                if (MessageBox.Show("是否要导出自增列数据，如果导出会删除原来的数据。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    notExportId = false;
                }
            }

            StringBuilder sql = new StringBuilder();
            foreach(var item in SQLHelper.ExportData(cols, notExportId, GetDBSource(node), tableinfo, topNum))
            {
                sql.AppendLine(item);
            }

            TextBoxWin win = new TextBoxWin("导出数据", sql.ToString());
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

                var tbclumns = Biz.Common.Data.SQLHelper.GetColumns(this.GetDBSource(selnode), tableinfo.DBName, tableinfo.TBId, tableinfo.TBName, tableinfo.Schema).ToList();

                var tbmark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                 [] { tableinfo.DBName.ToUpper(), tableinfo.TBName.ToUpper(), string.Empty }).FirstOrDefault();
                var tbdesc = tbmark == null ? (tableinfo.Desc ?? tableinfo.TBName) : tbmark.MarkInfo;

                DataTable resulttb = new DataTable();
                resulttb.Columns.AddRange(new string[][] {
                    new []{"line","行号"},
                    new []{"name","列名"},
                    new []{"iskey","是否主键"},
                    new []{"null","可空"},
                    new []{"type","类型"},
                    new []{"defaultvalue","默认值"},
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
                        object y = null;
                        var mark = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { GetDBName(selnode).ToUpper(), GetTBName(selnode).ToUpper(), m.Groups[1].Value.ToUpper() }).FirstOrDefault();
                        if (mark != null && !string.IsNullOrWhiteSpace(mark.MarkInfo))
                        {
                            y = mark.MarkInfo;
                        }
                        else
                        {
                            y = (from x in tbDesc.AsEnumerable()
                                 where string.Equals((string)x["ColumnName"], m.Groups[1].Value, StringComparison.OrdinalIgnoreCase)
                                 select x["Description"]).FirstOrDefault();
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
                            newrow["defaultvalue"] = col.DefaultValue == null ? "" : DataHelper.TrimDefaultValue(col.DefaultValue.ToString());
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
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName,tabeinfo.Schema, CreateProceEnum.BatchInsert);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName,tabeinfo.Schema, CreateProceEnum.Update);
            }
        }

        private void SubMenuItem_Delete_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName,tabeinfo.Schema, CreateProceEnum.Delete);
            }
        }

        private void upsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                var tabeinfo = _node.Tag as TableInfo;
                this.OnCreatePorcSQL(GetDBSource(_node), tabeinfo.DBName, tabeinfo.TBId, tabeinfo.TBName,tabeinfo.Schema, CreateProceEnum.Upsert);
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
                var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), tbname.ToUpper(), tbcol.Name.ToUpper() }).FirstOrDefault();

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
                    BigEntityTableRemotingEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                    selnode.ToolTipText = item.MarkInfo;
                    MessageBox.Show("备注成功");
                }
            }
            else if (selnode != null && selnode.Tag is ViewColumn)
            {
                var tbcol = ((ViewColumn)selnode.Tag);
                var tbname = GetTBName(selnode);
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), tbname.ToUpper(), tbcol.Name.ToUpper() }).FirstOrDefault();

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
                    BigEntityTableRemotingEngine.Upsert("MarkObjectInfo", item);
                    selnode.ToolTipText = item.MarkInfo;
                    MessageBox.Show("备注成功");
                }
            }
            else if (selnode != null && selnode.Tag is ProcInfo)
            {
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var spname = (selnode.Tag as ProcInfo).Name;
                var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                if (item == null)
                {
                    item = new SPInfo { Servername = servername, DBName = dbname, SPName = spname, Mark = "" };
                }
                InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    item.Mark = dlg.InputString;
                    BigEntityTableRemotingEngine.Upsert<SPInfo>("SPInfo", item);
                    selnode.ToolTipText = item.Mark;
                    selnode.ImageIndex = 13;
                    selnode.SelectedImageIndex = 14;
                    MessageBox.Show("备注成功");
                }
            }
            else if (selnode != null && selnode.Tag is FunInfo)
            {
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var spname = (selnode.Tag as FunInfo).Name;
                var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                if (item == null)
                {
                    item = new SPInfo { Servername = servername, DBName = dbname, SPName = spname, Mark = "" };
                }
                InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    item.Mark = dlg.InputString;
                    BigEntityTableRemotingEngine.Upsert<SPInfo>("SPInfo", item);
                    selnode.ToolTipText = item.Mark;
                    selnode.ImageIndex = 13;
                    selnode.SelectedImageIndex = 14;
                    MessageBox.Show("备注成功");
                }
            }
            else if (selnode != null && selnode.Tag is DBInfo)
            {
                var tbname = string.Empty;
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), tbname.ToUpper(), string.Empty }).FirstOrDefault();

                if (item == null)
                {
                    item = new MarkObjectInfo { ColumnName = string.Empty, DBName = dbname.ToUpper(), TBName = tbname.ToUpper(), Servername = servername };
                }
                InputStringDlg dlg = new InputStringDlg($"备注[{dbname}]", item.MarkInfo);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (selnode.ImageIndex == 18)
                    {
                        selnode.ImageIndex = selnode.SelectedImageIndex = 5;
                    }
                    item.MarkInfo = dlg.InputString;
                    BigEntityTableRemotingEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                    selnode.ToolTipText = item.MarkInfo;
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
                    var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), string.Empty }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new MarkObjectInfo { ColumnName = string.Empty, DBName = tb.DBName.ToUpper(), TBName = tb.TBName.ToUpper(), Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注:{tb.TBName}", item.MarkInfo);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.MarkInfo = dlg.InputString;
                        BigEntityTableRemotingEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                        currnode.ToolTipText = item.MarkInfo;
                        MessageBox.Show("备注成功");
                    }
                }
                else if (currnode.Tag != null && currnode.Tag is ViewInfo)
                {
                    var tb = (ViewInfo)currnode.Tag;
                    var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.Name.ToUpper(), string.Empty }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new MarkObjectInfo { ColumnName = string.Empty, DBName = tb.DBName.ToUpper(), TBName = tb.Name.ToUpper(), Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注:{tb.Name}", item.MarkInfo);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.MarkInfo = dlg.InputString;
                        BigEntityTableRemotingEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                        currnode.ToolTipText = item.MarkInfo;
                        MessageBox.Show("备注成功");
                    }
                }
                else if (currnode.Tag is ProcInfo)
                {
                    var servername = GetDBSource(currnode).ServerName;
                    var dbname = GetDBName(currnode);
                    var spname = currnode.Text;
                    var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new SPInfo { Servername = servername, DBName = dbname.ToUpper(), SPName = spname.ToUpper(), Mark = "" };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.Mark = dlg.InputString;
                        BigEntityTableRemotingEngine.Upsert<SPInfo>("SPInfo", item);
                        currnode.ToolTipText = item.Mark;
                        currnode.ImageIndex = 13;
                        currnode.SelectedImageIndex = 14;
                        MessageBox.Show("备注成功");
                    }
                }
                else if (currnode.Tag is FunInfo)
                {
                    var servername = GetDBSource(currnode).ServerName;
                    var dbname = GetDBName(currnode);
                    var spname = currnode.Text;
                    var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new SPInfo { Servername = servername, DBName = dbname.ToUpper(), SPName = spname.ToUpper(), Mark = "" };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.Mark = dlg.InputString;
                        BigEntityTableRemotingEngine.Upsert<SPInfo>("SPInfo", item);
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
                    var cols = SQLHelper.GetColumns(GetDBSource(currnode), tb.DBName, tb.TBId, tb.TBName,tb.Schema).ToList();
                    
                    var markedcolumns = BigEntityTableRemotingEngine.Scan<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), LJC.FrameWorkV3.Data.EntityDataBase.Consts.STRINGCOMPAIRMIN },
                        new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), LJC.FrameWorkV3.Data.EntityDataBase.Consts.STRINGCOMPAIRMAX }, 1, int.MaxValue);

                    foreach (var col in markedcolumns)
                    {
                        if (string.IsNullOrWhiteSpace(col.ColumnName))
                        {
                            continue;
                        }
                        if (!cols.Any(p => p.Name.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase)) || string.IsNullOrWhiteSpace(col.MarkInfo))
                        {
                            BigEntityTableRemotingEngine.Delete<MarkObjectInfo>("MarkObjectInfo", col.ID);
                        }
                    }
                    var columnMarkSyncRecorditem = BigEntityTableRemotingEngine.Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper() }).ToList();
                    foreach(var rec in columnMarkSyncRecorditem)
                    {
                        BigEntityTableRemotingEngine.Delete<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", rec.ID);
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
                        var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname, tbname, column }).FirstOrDefault();

                        if (item == null)
                        {
                            item = new MarkObjectInfo { ColumnName = column, DBName = dbname, TBName = tbname, Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                        }

                        item.MarkInfo = mark;
                        BigEntityTableRemotingEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
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
                       var regx=new RegExp(w,'i')
                       for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText.toUpperCase().indexOf(w)>-1||tds[i+2].innerText.toUpperCase().indexOf(w)>-1||regx.test(tds[i+1].innerText)
                           tds[i].parentNode.style.display=boo?'':'none'
                           if(boo) tds[i].innerText=idx++
                       }
                   }
                   
                  </script>");
                sb.Append("<input id='w' type='text' style='height:23px; line-height:23px;' onkeypress='k()' value=''/><input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/><span style='color:red;'>* 支持正则</span>");
                sb.Append("<p/>");
                sb.Append("<table>");
                sb.Append("<tr><th>序号</th><th>表名</th><th>描述</th></tr>");
                int i = 1;
                foreach (DataRow row in tb.Rows)
                {
                    var name = (string)row["name"];
                    var item = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), name.ToUpper(), string.Empty }).FirstOrDefault();

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

                var allcolumns = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", p => !string.IsNullOrEmpty(p.ColumnName) && p.DBName.Equals(dbname, StringComparison.OrdinalIgnoreCase), true, int.MaxValue);

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
                       var regx=new RegExp(w,'i')
                       for(var i=0;i<tds.length;i+=4){
                           var boo=tds[i+2].innerText.toUpperCase().indexOf(w)>-1||tds[i+3].innerText.toUpperCase().indexOf(w)>-1||regx.test(tds[i+2].innerText)
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
                            <input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/><span style='color:red;'>* 支持正则</span>");
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
                       var regx=new RegExp(w,'i')
                       for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText.toUpperCase().indexOf(w)>-1||tds[i+2].innerText.toUpperCase().indexOf(w)>-1||regx.test(tds[i+1].innerText)
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
                            <input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/><span style='color:red;'>* 支持正则</span>");
                sb.Append("<p/>");
                sb.Append($"<div id='clearcachtip' style='margin-top:5px;display:none;width:98%;font-size:9pt;height:18px; line-height:18px;background-color:lightyellow;border:solid 1px lightblue'>如果没有找到，可以选择<a href='javascript:tryclearcach()'>清空缓存</a>试试</div>");
                sb.Append("<p/>");
                sb.Append("<table>");
                sb.Append("<tr><th>序号</th><th>存储过程</th><th>描述</th></tr>");
                int i = 1;
                dbname = dbname.ToUpper();
                foreach (string name in proclist)
                {
                    var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, name.ToUpper() }).FirstOrDefault();
                    sb.Append($"<tr><td>{i++}</td><td><a href='javascript:window.external.ShowProc(\"{name}\")'>{name}</a></td><td>{item?.Mark}</td></tr>");
                }
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                this.OnFilterProc(dbsource, dbname, sb.ToString());
            }
        }

        private void FilterFunction()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (this.OnFilterFunction != null && selnode != null)
            {
                var dbsource = GetDBSource(selnode);
                var dbname = GetDBName(selnode);
                var proclist = Biz.Common.Data.SQLHelper.GetFunctions(dbsource, dbname).AsEnumerable().Select(p => p.Field<string>("name")).ToList();

                StringBuilder sb = new StringBuilder("<html>");
                sb.Append("<head>");
                sb.Append($"<title>查看{dbname}的函数</title>");
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
                       var regx=new RegExp(w,'i')
                       for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText.toUpperCase().indexOf(w)>-1||tds[i+2].innerText.toUpperCase().indexOf(w)>-1||regx.test(tds[i+1].innerText)
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
                sb.AppendFormat("<script>{0}</script>", System.IO.File.ReadAllText("json2.js"));
                sb.Append(@"<input id='w' type='text' style='height:23px; line-height:23px;width:30%;' onkeypress='k()' value=''/>
                            <input type='checkbox' id='scontent' value='1'>搜索内容</input>
                            <input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/><span style='color:red;'>* 支持正则</span>");
                sb.Append("<p/>");
                sb.Append($"<div id='clearcachtip' style='margin-top:5px;display:none;width:98%;font-size:9pt;height:18px; line-height:18px;background-color:lightyellow;border:solid 1px lightblue'>如果没有找到，可以选择<a href='javascript:tryclearcach()'>清空缓存</a>试试</div>");
                sb.Append("<p/>");
                sb.Append("<table>");
                sb.Append("<tr><th>序号</th><th>存储过程</th><th>描述</th></tr>");
                int i = 1;
                dbname = dbname.ToUpper();
                foreach (string name in proclist)
                {
                    var item = BigEntityTableRemotingEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname, name.ToUpper() }).FirstOrDefault();
                    sb.Append($"<tr><td>{i++}</td><td><a href='javascript:window.external.ShowFunction(\"{name}\")'>{name}</a></td><td>{item?.Mark}</td></tr>");
                }
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                this.OnFilterFunction(dbsource, dbname, sb.ToString());
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
                    if (BigEntityTableRemotingEngine.Scan<LogicMap>(nameof(LogicMap), "DB_LogicName",
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
                        BigEntityTableRemotingEngine.Insert(nameof(LogicMap), logicmap);
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

        private void 复制逻辑关系图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currnode = this.tv_DBServers.SelectedNode;
            if (currnode != null && currnode.Tag is LogicMap)
            {
                var logicmap = currnode.Tag as LogicMap;
                var logicmapCopy = BigEntityTableEngine.LocalEngine.Find<LogicMap>(nameof(LogicMap), logicmap.ID);
                var inputNameDlg = new SubForm.InputStringDlg($"复制逻辑关系图{logicmap.LogicName}", "输入名称", "输入名称");
                if (inputNameDlg.ShowDialog() == DialogResult.OK)
                {
                    var logicTables = BigEntityTableEngine.LocalEngine.Find<LogicMapTable>(nameof(LogicMapTable), "LogicID", new object[] { logicmap.ID }).ToList();
                    var logicMapRelColumns = BigEntityTableEngine.LocalEngine.Find<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID", new object[] { logicmap.ID }).ToList();

                    logicmapCopy.ID = 0;
                    logicmapCopy.LogicName = inputNameDlg.InputString;
                    Dictionary<string, string> tableMapping = new Dictionary<string, string>();
                    if(BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMap), logicmapCopy))
                    {
                        if (logicTables.Any())
                        {
                            logicTables.ForEach(p =>
                            {
                                p.ID = 0;
                                p.LogicID = logicmapCopy.ID;

                                if (Util.IsNoteTable(p.TBName))
                                {
                                    var noteTable = BigEntityTableEngine.LocalEngine.Find<TempNotesTable>(nameof(TempNotesTable), nameof(TempNotesTable.TBName), new object[] { p.TBName.ToUpper() }).FirstOrDefault();
                                    var newName = Util.NameNoteTalbe();
                                    tableMapping.Add(p.TBName, newName);
                                    noteTable.TBName = newName;
                                    noteTable.Id = 0;
                                    p.TBName = newName;
                                    BigEntityTableEngine.LocalEngine.Insert(nameof(TempNotesTable), noteTable);
                                    BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapTable), p);
                                }
                                else if (Util.IsTempTable(p.TBName))
                                {
                                    var temptb = BigEntityTableRemotingEngine.Find<TempTB>(nameof(TempTB), TempTB.INDEX_DB_TB, new object[] { p.TBName }).FirstOrDefault();
                                    var temptbCol= BigEntityTableRemotingEngine.Find<TempTBColumn>(nameof(TempTBColumn), nameof(TempTBColumn.TBId), new object[] { temptb.Id }).ToList();
                                    var newName = Util.NameTempTable();
                                    tableMapping.Add(p.TBName, newName);
                                    p.TBName = newName;
                                    temptb.TBName = newName;
                                    temptb.Id = 0;
                                    if (BigEntityTableEngine.LocalEngine.Insert(nameof(TempTB), temptb))
                                    {
                                        temptbCol.ForEach(q =>
                                        {
                                            q.Id = 0;
                                            q.TBId = temptb.Id;
                                        });
                                        BigEntityTableEngine.LocalEngine.InsertBatch(nameof(TempTBColumn), temptbCol);
                                        BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapTable), p);
                                    }
                                    
                                }
                                else
                                {
                                    BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapTable), p);
                                }
                            });
                        }

                        if (logicMapRelColumns.Any())
                        {
                            logicMapRelColumns.ForEach(p =>
                            {
                                p.ID = 0;
                                p.LogicID = logicmapCopy.ID;
                                if (tableMapping.ContainsKey(p.TBName))
                                {
                                    p.TBName = tableMapping[p.TBName];
                                }
                                if (tableMapping.ContainsKey(p.RelTBName))
                                {
                                    p.RelTBName = tableMapping[p.RelTBName];
                                }
                            });
                            BigEntityTableEngine.LocalEngine.InsertBatch(nameof(LogicMapRelColumn), logicMapRelColumns);
                        }
                    }

                    ReLoadDBObj(currnode.Parent);
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
                    BigEntityTableRemotingEngine.Delete<LogicMap>(nameof(LogicMap), logicmap.ID);
                    if (this.OnDeleteLogicMap != null)
                    {
                        OnDeleteLogicMap(GetDBName(currnode), logicmap);
                    }
                    ReLoadDBObj(currnode.Parent);
                }
            }
        }

        private void TTSM_CreateIndex_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            var ds = GetDBSource(_node);
            var tb = _node.Parent.Tag as TableInfo;
            var cols = SQLHelper.GetColumns(ds, tb.DBName, tb.TBName,tb.Schema).ToList();
            WinCreateIndex win = new WinCreateIndex(cols);
            if (win.ShowDialog() == DialogResult.OK && MessageBox.Show("要创建索引吗？") == DialogResult.OK)
            {
                try
                {
                    SQLHelper.CreateIndex(ds, tb.DBName, tb.TBName, win.IndexName, win.IsUnique(), win.IsPrimaryKey(), win.IsAutoIncr(), win.IsClustered(), win.IndexColumns);
                    MessageBox.Show("创建索引成功");
                    ReLoadDBObj(_node);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "创建索引出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TTSM_DelIndex_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (_node.Tag is IndexEntry)
            {
                var idx = _node.Tag as IndexEntry;
                if (MessageBox.Show("要删除索引\"" + idx.IndexName + "\"吗?", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var ds = GetDBSource(_node);
                    try
                    {
                        SQLHelper.DropIndex(ds, GetDBName(_node), GetTBName(_node), idx.IndexName.Equals("primary", StringComparison.OrdinalIgnoreCase), idx.IndexName);
                        MessageBox.Show("删除成功");
                        ReLoadDBObj(_node.Parent);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "删除索引失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void 完全加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReLoadDBObj(tv_DBServers.SelectedNode, true);
        }

        private void 过滤显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.DBFilter dlg = new DBFilter();
            dlg.ShowMe(this.Parent);
            dlg.SetDBSource(GetDBSource(this.tv_DBServers.SelectedNode)).Init(0, false);
        }
    }
}
