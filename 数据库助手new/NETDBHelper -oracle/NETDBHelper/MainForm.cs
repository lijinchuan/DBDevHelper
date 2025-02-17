﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Biz.Common.Data;
using Entity;
using NETDBHelper.UC;

namespace NETDBHelper
{
    public partial class MainFrm : Form
    {
        private static MainFrm Instance = null;
        private System.Timers.Timer tasktimer = null;
        private void InitFrm()
        {
            this.tsb_Excute.Enabled = false;
            this.dbServerView1.OnCreateEntity += this.CreateEntity;
            this.dbServerView1.OnShowTableData += this.ShowTableData;
            this.dbServerView1.OnAddEntityTB += this.AddEntityDB;
            this.dbServerView1.OnCreateSelectSql += this.CreateSelectSql;
            this.dbServerView1.OnCreatePorcSQL += this.CreateProcSql;
            this.dbServerView1.OnAddSqlExecuter += this.AddSqlExecute;
            this.dbServerView1.OnShowProc += this.ShowProc;
            this.dbServerView1.OnShowDataDic += this.ShowDataDic;
            this.dbServerView1.OnViewCloumns += this.ShowColumns;
            this.dbServerView1.OnFilterProc += this.FilterProc;
            this.TabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.dbServerView1.OnViewTable += this.ShowTables;
            this.TabControl.Selected += new TabControlEventHandler(TabControl_Selected);

            this.TSCBServer.ForeColor = Color.HotPink;
            this.TSCBServer.Visible = false;
            this.TSCBServer.Image = Resources.Resource1.connect;
            this.TSCBServer.Alignment = ToolStripItemAlignment.Right;

            this.MspPanel.TextAlign = ContentAlignment.TopLeft;

            this.dbServerView1.OnShowRelMap += this.ShowRelMap;
            this.dbServerView1.OnAddNewLogicMap += this.AddNewLogicMap;
            this.dbServerView1.OnDeleteLogicMap += this.DeleteLogicMap;
        }

        protected void CreateSelectSql(string sqlname, string s)
        {
            foreach (TabPage page in TabControl.TabPages)
            {
                if (page.Text.Equals(sqlname) && page is UC.SqlProceCodePanel)
                {
                    (page as UC.SqlProceCodePanel).Text = s;
                    TabControl.SelectedTab = page;
                    return;
                }
            }
            UC.SqlProceCodePanel panel = new UC.SqlProceCodePanel();
            panel.Text = sqlname;
            panel.Code = s;
            TabControl.TabPages.Add(panel);
            TabControl.SelectedTab = panel;
            Clipboard.SetText(s);
            
            this.SetMsg("代码已经复制到剪粘板。");
        }

        public MainFrm()
        {
            InitializeComponent();
            InitFrm();
            Instance = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            连接对象资源管理器ToolStripMenuItem_Click(null, null);
            Biz.WatchTask.WatchTaskInfoManage.OnTiggerError += (s, o) =>
            {
                this.BeginInvoke(new Action(() => {
                    Util.PopMsg(s.ID, s.Name, s.ErrorMsg);
                }));
            };
            Biz.WatchTask.WatchTaskInfoManage.OnErrorDisappear += (s) =>
            {
                this.BeginInvoke(new Action(() => {
                    Util.ClosePopMsg(s.ID);
                }));
            };
            tasktimer = LJC.FrameWorkV3.Comm.TaskHelper.SetInterval(10000, () =>
            {
                Biz.WatchTask.WatchTaskInfoManage.LoopTask();
                return false;
            });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (tasktimer != null)
            {
                tasktimer.Stop();
                tasktimer.Close();
            }
        }

        void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            tsb_Excute.Enabled = e.TabPage is UC.ViewTBData
            || e.TabPage is UC.SqlExcuter;

            if (e.TabPage is UC.SqlExcuter)
            {
                this.TSCBServer.Text = (e.TabPage as UC.SqlExcuter).Server.ServerName + "::" + (e.TabPage as UC.SqlExcuter).GetDB();
                this.TSCBServer.Visible = true;
            }
            else
            {
                this.TSCBServer.Visible = false;
            }
        }

        private void CreateProcSql(DBSource dbSource, string dbName, string tableID, string table,CreateProceEnum createProcType)
        {
            UC.CreateProc cp = new CreateProc(dbSource, dbName, table, tableID, createProcType);
            cp.Create();
            this.TabControl.TabPages.Add(cp);
            TabControl.SelectedTab = cp;
        }

        private void ShowTables(DBSource dbSource, string dbname, string html)
        {
            var tit = $"查看{dbname}的库表";
            foreach (TabPage tab in this.TabControl.TabPages)
            {
                if (tab.Text.Equals(tit))
                {
                    (tab as UC.WebTab).SetHtml(html);
                    TabControl.SelectedTab = tab;
                    return;
                }
            }
            UC.WebTab panel = new WebTab(dbSource, dbname);
            panel.SetHtml(html);
            panel.Text = tit;
            panel.OnSearch += (d, n, w) =>
            {
                return null;
            };
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }


        protected void CreateEntity(string entityName,string s)
        {
            foreach (TabPage page in TabControl.TabPages)
            {
                if (page.Text.Equals(entityName) && page is UC.EntityCodePanel)
                {
                    (page as UC.EntityCodePanel).EntityBody = s;
                    TabControl.SelectedTab = page;
                    return;
                }
            }
            UC.EntityCodePanel panel = new UC.EntityCodePanel();
            panel.EntityBody = s;
            panel.Text = entityName;
            TabControl.TabPages.Add(panel);
            TabControl.SelectedTab = panel;
        }

        public void AddEntityDB(DBSource db,string dbName)
        {
            var page = new UC.UCAddTableByEntity(db,dbName);
            page.Text = "添加实体映射表";
            TabControl.TabPages.Add(page);
            TabControl.SelectedTab = page;
            page.OnNewTableAdd += this.OnNewTableAdd;
        }

        private void OnNewTableAdd(DBSource db,string dbName)
        {
            var node = dbServerView1.FindNode(db.ServerName, dbName);
            TreeNode pnode = null;
            foreach (TreeNode n in node.Nodes)
            {
                var c = n.Tag as Entity.INodeContents;
                if (c.GetNodeContentType() == NodeContentType.TBParent)
                {
                    pnode = n;
                    break;
                }

            }
            if (pnode != null)
                Biz.UILoadHelper.LoadTBsAnsy(this, pnode, db, dbName, null);
        }

        public void ShowTableData(DBSource db, string dbName, string tbName, string sql)
        {
            var title = $"[查询数据 {tbName} -{db.ServerName}]";
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == title)
                {
                    TabControl.SelectedTab = page;
                    return;
                }
            }
            var viewTb = new SqlExcuter(db, dbName, sql);
            //ViewTBData viewTb = new ViewTBData();
            viewTb.Text = title;
            viewTb.BorderStyle = BorderStyle.None;
            this.TabControl.TabPages.Add(viewTb);
            TabControl.SelectedTab = viewTb;
            tsb_Excute.Enabled = true;
            this.TSCBServer.Text = db.ServerName + "::" + dbName;
            this.TSCBServer.Visible = true;
            //viewTb.DBSource = db;
            //viewTb.DBName = dbName;
            //viewTb.SQLString = sql;
        }

        public void AddSqlExecute(DBSource db,string dbName,string tbname)
        {
            ViewTBData viewTb = new ViewTBData();
            viewTb.Text = string.Format("{0}-查询分析器",dbName );
            this.TabControl.TabPages.Add(viewTb);
            TabControl.SelectedTab = viewTb;
            tsb_Excute.Enabled = true;
            viewTb.DBSource = db;
            viewTb.DBName = dbName;
            viewTb.TBName = tbname;
            viewTb.SQLString = string.Empty;
        }

        private void 连接对象资源管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnSQLServer obj = new ConnSQLServer();
            if (obj.ShowDialog() == DialogResult.OK)
            {
                if (!this.dbServerView1.DBServers.Contains(obj.DBSource))
                {
                    this.dbServerView1.DBServers.Add(obj.DBSource);
                    var allDBs = Biz.Common.XMLHelper.DeSerializeFromFile<DBSourceCollection>(Resources.Resource1.DbServersFile) ?? new DBSourceCollection();
                    allDBs.Add(obj.DBSource);
                    Biz.Common.XMLHelper.Serialize(allDBs, Resources.Resource1.DbServersFile);
                    this.dbServerView1.Bind();
                }
            }
        }

        private void ToolStripMenuItem_callsp_Click(object sender, EventArgs e)
        {
            var pg = new UC.UCCreateCodeCallSP();
            pg.Text = "生成代码工具";
            this.TabControl.TabPages.Add(pg);
        }

        private void 断开对象资源管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dbServerView1.DisConnectSelectDBServer();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "执行":
                    if (this.TabControl.SelectedTab != null && this.TabControl.SelectedTab is UC.ViewTBData)
                    {
                        (this.TabControl.SelectedTab as UC.ViewTBData).Execute();
                    }
                    else if (this.TabControl.SelectedTab != null && this.TabControl.SelectedTab is UC.SqlExcuter)
                    {
                        (this.TabControl.SelectedTab as UC.SqlExcuter).Execute();
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据模型建表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubItemModelCreateTableTool_Click(object sender, EventArgs e)
        {

        }

        internal void SetMsg(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    this.MspPanel.Text = msg;
                    if (this.MspPanel.Width >= this.statusStrip1.Width - this.MspPanel.Width - 10)
                    {
                        this.MspPanel.Spring = true;
                        this.TSL_ClearMsg.Visible = true;
                    }
                }));
            }
            else
            {
                this.MspPanel.Text = msg;
                if (this.MspPanel.Width >= this.statusStrip1.Width - this.MspPanel.Width - 10)
                {
                    this.TSL_ClearMsg.Visible = true;
                    this.MspPanel.Spring = true;
                }
            }
        }

        public static void SendMsg(string msg)
        {
            if (Instance == null)
            {
                return;
            }
            Instance.SetMsg(msg);
        }


        private void ShowProc(DBSource dBSource, string dbname, string procname, string procbody)
        {
            UC.SQLCodePanel panel = new SQLCodePanel();
            panel.SetCode(dbname, procbody);
            panel.Text = $"存储过程-{procname}";
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void ShowDataDic(DBSource dBSource, string dbname, string tbname, string html)
        {
            var tit = $"数据字典-[{dbname}].[{tbname}]";
            foreach (TabPage tab in this.TabControl.TabPages)
            {
                if (tab.Text.Equals(tit))
                {
                    (tab as UC.WebTab).SetHtml(html);
                    TabControl.SelectedTab = tab;
                    return;
                }
            }
            UC.WebTab panel = new WebTab(dBSource, dbname);
            panel.SetHtml(html);
            panel.Text = tit;
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }
        private void ShowColumns(DBSource dbsource, string dbname, string html)
        {
            var tit = $"查看{dbname}的字段";
            foreach (TabPage tab in this.TabControl.TabPages)
            {
                if (tab.Text.Equals(tit))
                {
                    (tab as UC.WebTab).SetHtml(html);
                    TabControl.SelectedTab = tab;
                    return;
                }
            }
            UC.WebTab panel = new WebTab(dbsource, dbname);
            panel.SetHtml(html);
            panel.Text = tit;
            panel.OnSearch += (d, n, w) =>
            {
                List<object> lst = new List<object>();
                var tbs = OracleHelper.GetTBs(d, n);

                if (tbs.Rows.Count == 0)
                {
                    return lst;
                }
                int finishcount = 0;
                foreach (DataRow row in tbs.Rows)
                {
                    var tbname = row["name"].ToString();
                    var searchColumns = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<TBSearchColumn>("TBSearchColumn", "DBName_TBName", new[] { n.ToLower(), tbname.ToLower() }).ToArray();
                    if (searchColumns.Length == 0)
                    {
                        try
                        {
                            var cols = OracleHelper.GetColumns(d, n, tbname).ToArray();

                            if (cols.Length > 0)
                            {
                                searchColumns = cols.Select(p => new TBSearchColumn
                                {
                                    DBName = n.ToLower(),
                                    TBName = tbname.ToLower(),
                                    Name = p.Name,
                                    Description = p.Description
                                }).ToArray();
                                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.InsertBatch<TBSearchColumn>("TBSearchColumn", searchColumns);
                            }
                        }
                        catch (Exception ex)
                        {
                            Util.SendMsg(this, ex.Message);
                        }

                    }

                    if (searchColumns.Length > 0)
                    {
                        foreach (var c in searchColumns)
                        {
                            if (c.Name.IndexOf(w, StringComparison.OrdinalIgnoreCase) > -1 || c.Description?.IndexOf(w, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                lst.Add(c);
                            }
                        }
                    }

                    finishcount++;
                    var rat = finishcount * 100 / tbs.Rows.Count;
                    Util.SendMsg(this, $"正在搜索表>{tbname}，完成{rat.ToString("f2")}%......");
                }
                Util.SendMsg(this, $"正在搜索表，完成100%，共{lst.Count}条数据......");
                return lst;
            };
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void FilterProc(DBSource dbsource, string dbname, string html)
        {
            var tit = $"查看{dbname}的存储过程";
            foreach (TabPage tab in this.TabControl.TabPages)
            {
                if (tab.Text.Equals(tit))
                {
                    (tab as UC.WebTab).SetHtml(html);
                    TabControl.SelectedTab = tab;
                    return;
                }
            }
            UC.WebTab panel = new WebTab(dbsource, dbname);
            panel.SetHtml(html);
            panel.Text = tit;
            panel.OnSearch += (s, n, w) =>
            {
                List<object> lst = new List<object>();
                var proclist = Biz.Common.Data.OracleHelper.GetProcList(dbsource).ToList();
                if (proclist.Count == 0)
                {
                    return lst;
                }
                int finishcount = 0;
                foreach (var proc in proclist)
                {
                    var spcontent = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPContent>("SPContent", "SPName", new[] { proc }).FirstOrDefault();
                    if (spcontent == null)
                    {
                        try
                        {
                            var body = OracleHelper.GetProcedureBody(dbsource, proc);

                            if (!string.IsNullOrEmpty(body))
                            {
                                spcontent = new SPContent { SPName = proc, Content = body };
                                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<SPContent>("SPContent", new SPContent
                                {
                                    Content = body,
                                    SPName = proc
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Util.SendMsg(this, ex.Message);
                        }

                    }

                    if (spcontent != null)
                    {
                        if (spcontent.Content.IndexOf(w, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            lst.Add(proc);
                        }
                        else
                        {
                            try
                            {
                                if (Regex.IsMatch(spcontent.Content, w))
                                {
                                    lst.Add(proc);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }

                    finishcount++;
                    var rat = finishcount * 100 / proclist.Count;
                    Util.SendMsg(this, $"正在搜索存储过程>{proc}，完成{rat.ToString("f2")}%......");
                }
                Util.SendMsg(this, $"正在搜索存储过程，完成100%，共{lst.Count}条数据......");
                return lst;
            };
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == "查看日志")
                {
                    TabControl.SelectedTab = page;
                    (page as LogViewTab).BindData();
                    return;
                }
            }
            LogViewTab logview = new LogViewTab();
            logview.Text = "查看日志";

            this.TabControl.TabPages.Add(logview);
            this.TabControl.SelectedTab = logview;
            logview.BindData();
        }

        private void ShowRelMap(DBSource dbSource, string dbname, string tbname)
        {
            var title = $"表关系图{dbname}.{tbname}";
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == title)
                {
                    TabControl.SelectedTab = page;
                    return;
                }
            }

            UC.UCTableRelMap panel = new UCTableRelMap(dbSource, dbname, tbname);
            panel.Text = title;
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
            panel.Load();
        }
        public void DeleteLogicMap(string dbname, LogicMap logicMap)
        {
            var title = $"{dbname}逻辑关系图{logicMap.LogicName}";
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == title)
                {
                    this.TabControl.TabPages.Remove(page);
                    return;
                }
            }
        }

        private void 常用SQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == "常用SQL")
                {
                    TabControl.SelectedTab = page;
                    (page as SqlSaveViewTab).BindData();
                    return;
                }
            }
            SqlSaveViewTab view = new SqlSaveViewTab();
            view.Text = "常用SQL";

            this.TabControl.TabPages.Add(view);
            this.TabControl.SelectedTab = view;
            view.BindData();
        }
        public void AddNewLogicMap(DBSource dbSource, string dbname, LogicMap logicMap)
        {
            var title = $"{dbname}逻辑关系图{logicMap.LogicName}";
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == title)
                {
                    TabControl.SelectedTab = page;
                    return;
                }
            }

            UC.UCLogicMap panel = new UCLogicMap(dbSource, logicMap.ID);
            panel.Text = title;
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
            panel.Load();
        }

        private void TSL_ClearMsg_Click(object sender, EventArgs e)
        {
            this.MspPanel.Text = "";
            TSL_ClearMsg.Visible = false;
            this.MspPanel.Spring = false;
        }

        private void 监控任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SubForm.WatchTaskList().Show();
        }
    }
}
