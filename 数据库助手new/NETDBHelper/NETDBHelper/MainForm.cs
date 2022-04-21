using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Biz.Common.Data;
using Entity;
using Entity.WatchTask;
using ICSharpCode.SharpZipLib.Zip;
using LJC.FrameWorkV3.Data.EntityDataBase;
using NETDBHelper.SubForm;
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
            this.dbServerView1.OnCreateSelectSql += this.CreateSelectSql;
            this.dbServerView1.OnShowTableData += this.ShowTableData;
            this.dbServerView1.OnAddEntityTB += this.AddEntityDB;
            this.dbServerView1.OnCreatePorcSQL += this.CreateProcSql;
            this.dbServerView1.OnShowProc += this.ShowProc;
            this.dbServerView1.OnShowDataDic += this.ShowDataDic;
            this.dbServerView1.OnViewTable += this.ShowTables;
            this.dbServerView1.OnViewCloumns += this.ShowColumns;
            this.dbServerView1.OnFilterProc += this.FilterProc;
            this.dbServerView1.OnFilterFunction += this.FilterFunction;
            this.dbServerView1.OnExecutSql += this.ExecutSql;
            this.dbServerView1.OnShowViewSql += this.ShowViewSql;
            this.dbServerView1.OnShowRelMap += this.ShowRelMap;
            this.dbServerView1.OnAddNewLogicMap += this.AddNewLogicMap;
            this.dbServerView1.OnDeleteLogicMap += this.DeleteLogicMap;

            this.TabControl.Selected += new TabControlEventHandler(TabControl_Selected);

            this.TSCBServer.ForeColor = Color.HotPink;
            this.TSCBServer.Visible = false;
            this.TSCBServer.Image = Resources.Resource1.connect;
            this.TSCBServer.Alignment = ToolStripItemAlignment.Right;

            this.MspPanel.TextAlign = ContentAlignment.TopLeft;

            复制数据库ToolStripMenuItem.Visible = 搜索数据库ToolStripMenuItem.Visible = 还原数据库ToolStripMenuItem.Visible = Util.LoginUserLevel() >= 5;
            登录ToolStripMenuItem.Visible = Util.LoginUserLevel() == 0;
            登出ToolStripMenuItem.Visible = Util.LoginUserLevel() > 0;


            Util.OnUserLogin += Util_OnUserLogin;
            Util.OnUserLoginOut += Util_OnUserLoginOut;

            TSBar.Image = Resources.Resource1.side_contract;
            TSBar.Click += TSBar_Click;

            显示左侧窗口ToolStripMenuItem.Enabled = false;
        }

        private void TSBar_Click(object sender, EventArgs e)
        {
            ChangeLeftWindow();
        }

        private void Util_OnUserLoginOut(LoginUser user)
        {
            复制数据库ToolStripMenuItem.Visible = 搜索数据库ToolStripMenuItem.Visible = 还原数据库ToolStripMenuItem.Visible = false;
            登录ToolStripMenuItem.Visible = true;
            登出ToolStripMenuItem.Visible = false;

            Text = Text.Split('-')[0];
        }

        private void Util_OnUserLogin(LoginUser loginUser)
        {
            复制数据库ToolStripMenuItem.Visible = 搜索数据库ToolStripMenuItem.Visible = 还原数据库ToolStripMenuItem.Visible = loginUser.UserLevel >= 5;
            登录ToolStripMenuItem.Visible = false;
            登出ToolStripMenuItem.Visible = true;

            Text += "-" + loginUser.UserName;
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

            try
            {
                var dbs = Biz.RecoverManager.GetDBSources().ToList();

                if (dbs.Count > 0)
                {
                    var allDBs = Biz.Common.XMLHelper.DeSerializeFromFile<DBSourceCollection>(Resources.Resource1.DbServersFile) ?? new DBSourceCollection();
                    foreach (var ds in dbs)
                    {
                        if (!this.dbServerView1.DBServers.Contains(ds))
                        {
                            var server = allDBs.FirstOrDefault(p => p.ServerName == ds.ServerName && p.LoginName == ds.LoginName);
                            if (server == null)
                            {
                                server = allDBs.FirstOrDefault(p => p.ServerName == ds.ServerName);
                            }
                            if (server != null)
                            {
                                dbServerView1.DBServers.Add(server);
                            }
                            else
                            {
                                dbServerView1.DBServers.Add(ds);
                            }
                        }
                    }
                    this.dbServerView1.Bind();
                }
                TabPage selecedpage = null;
                foreach (var tp in Biz.RecoverManager.Recove())
                {
                    this.TabControl.TabPages.Add(tp.Item1);
                    if (tp.Item2 == true)
                    {
                        selecedpage = tp.Item1;
                    }
                }
                this.TabControl.SelectedIndex = -1;
                if (selecedpage != null)
                {
                    this.TabControl.SelectedTab = selecedpage;
                }
                else
                {
                    if (this.TabControl.TabPages.Count > 0)
                    {
                        this.TabControl.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            if (this.TabControl.TabPages.Count == 0)
            {
                连接对象资源管理器ToolStripMenuItem_Click(null, null);
            }
            Biz.WatchTask.WatchTaskInfoManage.OnTiggerError += (s, o) =>
            {
                this.BeginInvoke(new Action(() =>
                {
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
            if (MessageBox.Show("要退出吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            if (e.Cancel)
            {
                e.Cancel = false;
            }
            base.OnClosing(e);
            if (tasktimer != null)
            {
                tasktimer.Stop();
                tasktimer.Close();
            }

            try
            {
                foreach (TabPage tab in this.TabControl.TabPages)
                {
                    bool isSelected = this.TabControl.SelectedTab == tab;
                    if (tab is IRecoverAble)
                    {
                        Biz.RecoverManager.AddRecoverInstance(tab, isSelected);
                    }
                }
                Biz.RecoverManager.SaveRecoverInstance();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            tsb_Excute.Enabled = e.TabPage is UC.ViewTBData
                ||e.TabPage is UC.SqlExcuter;

            if (e.TabPage is UC.SqlExcuter)
            {
                this.TSCBServer.Text = (e.TabPage as UC.SqlExcuter).Server.ServerName+"::"+ (e.TabPage as UC.SqlExcuter).GetDB();
                this.TSCBServer.Visible = true;
            }
            else
            {
                this.TSCBServer.Visible = false;
            }
        }

        private void CreateProcSql(DBSource dbSource, string dbName, string tableID, string table,string tbOwner,CreateProceEnum createProcType)
        {
            UC.CreateProc cp = new CreateProc(dbSource, dbName, table, tableID,tbOwner, createProcType);
            cp.Create();
            this.TabControl.TabPages.Add(cp);
            
            TabControl.SelectedTab = cp;
        }

        private void ShowProc(DBSource dBSource,string dbname, string procname, string procbody)
        {
            UC.SQLCodePanel panel = new SQLCodePanel();
            panel.SetCode(dBSource,dbname,procbody);
            panel.Text = $"存储过程-{procname}";
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void ShowFunction(DBSource dBSource, string dbname, string procname, string procbody)
        {
            UC.SQLCodePanel panel = new SQLCodePanel();
            panel.SetCode(dBSource,dbname, procbody);
            panel.Text = $"函数-{procname}";
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void ShowDataDic(DBSource dBSource,string dbname,string tbname,string html)
        {
            var tit = $"数据字典-[{dbname}].[{tbname}]";
            foreach(TabPage tab in this.TabControl.TabPages)
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

        private void ShowTables(DBSource dBSource,string dbname, string html)
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
            UC.WebTab panel = new WebTab(dBSource,dbname);
            panel.SetHtml(html);
            panel.Text = tit;
            panel.OnSearch += (d, n, w) =>
            {
                throw new NotImplementedException();
            };
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void ShowColumns(DBSource dbsource,string dbname, string html)
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
                var tbs = SQLHelper.GetTBs(d, n);

                if (tbs.Rows.Count == 0)
                {
                    return lst;
                }
                int finishcount = 0;
                foreach (DataRow row in tbs.Rows)
                {
                    var tbname = row["name"].ToString();
                    var schema = row["schema"].ToString();
                    var searchColumns = BigEntityTableRemotingEngine.Find<TBSearchColumn>("TBSearchColumn", "DBName_TBName", new[] { n.ToLower(), tbname.ToLower() }).ToArray();
                    if (searchColumns.Length == 0)
                    {
                        try
                        {
                            var cols = Biz.Common.Data.SQLHelper.GetColumns(d, n, tbname, schema).ToArray();

                            if (cols.Length > 0)
                            {
                                searchColumns = cols.Select(p => new TBSearchColumn
                                {
                                    DBName = n.ToLower(),
                                    TBName = tbname.ToLower(),
                                    Name = p.Name,
                                    Description = p.Description
                                }).ToArray();
                                BigEntityTableRemotingEngine.InsertBatch<TBSearchColumn>("TBSearchColumn", searchColumns);
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

        private void FilterProc(DBSource dbsource,string dbname, string html)
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
            UC.WebTab panel = new WebTab(dbsource,dbname);
            panel.SetHtml(html);
            panel.Text = tit;
            panel.OnShowProc += (s, d, p, b) => this.ShowProc(s, d, p, b);
            panel.OnShowFunction += (s, d, p, b) => this.ShowFunction(s, d, p, b);
            panel.OnSearch += (s, n, w) =>
            {
                List<object> lst = new List<object>();
                var proclist = Biz.Common.Data.SQLHelper.GetProcedures(dbsource, dbname).ToList();
                if (proclist.Count == 0)
                {
                    return lst;
                }
                int finishcount = 0;
                foreach(var proc in proclist)
                {
                    var spcontent = BigEntityTableRemotingEngine.Find<SPContent>("SPContent", "SPName", new[] { proc }).FirstOrDefault();
                    if (spcontent == null)
                    {
                        try
                        {
                            var body = SQLHelper.GetProcedureBody(dbsource, dbname, proc);
                        
                            if (!string.IsNullOrEmpty(body))
                            {
                                spcontent = new SPContent { SPName = proc, Content = body };
                                BigEntityTableRemotingEngine.Insert<SPContent>("SPContent", new SPContent
                                {
                                    Content=body,
                                    SPName=proc
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

        private void FilterFunction(DBSource dbsource, string dbname, string html)
        {
            var tit = $"查看{dbname}的函数";
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
            panel.OnShowFunction += (s, d, p, b) => this.ShowFunction(s, d, p, b);
            panel.OnSearch += (s, n, w) =>
            {
                List<object> lst = new List<object>();
                var proclist = Biz.Common.Data.SQLHelper.GetFunctions(dbsource, dbname).AsEnumerable().Select(p=>p.Field<string>("name")).ToList();
                if (proclist.Count == 0)
                {
                    return lst;
                }
                int finishcount = 0;
                foreach (var proc in proclist)
                {
                    var spcontent = BigEntityTableRemotingEngine.Find<FunContent>("FunContent", "FunName", new[] { proc }).FirstOrDefault();
                    if (spcontent == null)
                    {
                        try
                        {
                            var body = Biz.Common.Data.SQLHelper.GetFunctionBody(dbsource, dbname, proc);

                            if (!string.IsNullOrEmpty(body))
                            {
                                spcontent = new FunContent { FunName = proc, Content = body };
                                BigEntityTableRemotingEngine.Insert("FunContent", new FunContent
                                {
                                    Content = body,
                                    FunName = proc
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
                    Util.SendMsg(this, $"正在搜索函数>{proc}，完成{rat.ToString("f2")}%......");
                }
                Util.SendMsg(this, $"正在搜索函数过程，完成100%，共{lst.Count}条数据......");
                return lst;
            };
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void ExecutSql(DBSource source,string db,string sql)
        {
            var tit = $"执行语句";
            //foreach (TabPage tab in this.TabControl.TabPages)
            //{
            //    if (tab.Text.Equals(tit))
            //    {
            //        (tab as UC.WebTab).SetHtml(html);
            //        TabControl.SelectedTab = tab;
            //        return;
            //    }
            //}
            
            SqlExcuter se = new SqlExcuter(source, db, sql);
            se.Text = tit;
            this.TabControl.TabPages.Add(se);
            this.TabControl.SelectedTab = se;
            tsb_Excute.Enabled = true;
            this.TSCBServer.Text = source.ServerName + "::" + db;
            this.TSCBServer.Visible = true;
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

        public void AddEntityDB(DBSource db,string dbName)
        {
            var page = new UC.UCAddTableByEntity(db,dbName);
            page.Text = "添加实体映射表";
            TabControl.TabPages.Add(page);
            TabControl.SelectedTab = page;
            page.OnNewTableAdd += this.OnNewTableAdd;
        }

        private void OnNewTableAdd(DBSource db, string dbName)
        {
            var dbnode = dbServerView1.FindNode(db.ServerName, dbName);
            if (dbnode != null)
            {
                Biz.UILoadHelper.LoadTBsAnsy(this, dbServerView1.FindNode(dbnode, NodeContentType.TBParent), db, dbName, null,null,null);
            }
        }

        public void ShowTableData(DBSource db,string dbName,string tablename, string sql)
        {
            var title = $"[查询数据 {tablename} -{db.ServerName}]";
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
            viewTb.Text =title;
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
            var server = this.dbServerView1.DisConnectSelectDBServer();
            //if (server != null && this.TSCBServer.Items.Contains(server))
            //{
            //    this.TSCBServer.Items.Remove(server);
            //}
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

        /// <summary>
        /// 根据模型建表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubItemModelCreateTableTool_Click(object sender, EventArgs e)
        {
            
        }

        public static void SendMsg(string msg)
        {
            if (Instance == null)
            {
                return;
            }
            Instance.SetMsg(msg);
        }

        private void TabControl_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(TabPage page in this.TabControl.TabPages)
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

        private void ShowViewSql(DBSource source,string dbname,string viewname,string sql)
        {
            var title = $"视图{dbname}.{viewname}";
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == title)
                {
                    TabControl.SelectedTab = page;
                    return;
                }
            }
            var tip= $"视图{dbname}.{viewname}-{source.ServerName}";
            UC.SQLCodePanel panel = new SQLCodePanel();
            panel.ToolTipText = tip;
            panel.SetCode(source,dbname, sql);
            panel.Text = title;
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
        }

        private void ShowRelMap(DBSource dbSource,string dbname,string tbname)
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

            UC.UCTableRelMap panel = new UCTableRelMap(dbSource, dbname,tbname);
            panel.Text = title;
            this.TabControl.TabPages.Add(panel);
            this.TabControl.SelectedTab = panel;
            panel.Load();
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


        private void TSL_ClearMsg_Click(object sender, EventArgs e)
        {
            this.MspPanel.Text = "";
            TSL_ClearMsg.Visible = false;
            this.MspPanel.Spring = false;
        }

        private void 监控任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SubForm.WatchTaskList().Show();

            //Util.PopMsg(1, "test", "test");
        }

        private void 复制数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SubForm.CopyDB();
            dlg.ShowMe(this);
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.LoginDlg dlg = new LoginDlg();
            dlg.ShowDialog();
        }

        private void 登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.LoginOut();
        }

        private void 还原数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.RecoverDBDlg dlg = new RecoverDBDlg();
            dlg.Show();
        }

        private void ChangeLeftWindow()
        {
            if (TSBar.Tag == null)
            {
                隐藏左侧窗口ToolStripMenuItem.Enabled = false;
                显示左侧窗口ToolStripMenuItem.Enabled = true;
                TSBar.Image = Resources.Resource1.side_expand;
                TSBar.Tag = panel1.Location;
                var location = dbServerView1.Location;
                location.Offset(2, 0);
                panel1.Location = location;
                panel1.Width += ((Point)TSBar.Tag).X - location.X;
                dbServerView1.Hide();
            }
            else
            {
                TSBar.Image = Resources.Resource1.side_contract;
                panel1.Width -= ((Point)TSBar.Tag).X - panel1.Location.X;
                panel1.Location = (Point)TSBar.Tag;
                隐藏左侧窗口ToolStripMenuItem.Enabled = true;
                显示左侧窗口ToolStripMenuItem.Enabled = false;
                TSBar.Tag = null;
                dbServerView1.Show();
            }
        }

        private void 隐藏左侧窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLeftWindow();
        }

        private void 显示左侧窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLeftWindow();
        }

        private void 服务器设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new ServerSettingDlg();

            dlg.ShowDialog();
        }

        private void 搜索数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SearchDBDataDlg();

            dlg.Show();
        }

        private void 符号大全ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.SymbolsDlg dlg = new SymbolsDlg();
            dlg.Show();
        }
    }
}
