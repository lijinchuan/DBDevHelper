using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using NETDBHelper.UC;

namespace NETDBHelper
{
    public partial class MainFrm : Form
    {
        private static MainFrm Instance = null;
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
            this.TabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.dbServerView1.OnViewTable += this.ShowTables;
            this.TabControl.Selected += new TabControlEventHandler(TabControl_Selected);

            this.TSCBServer.ForeColor = Color.HotPink;
            this.TSCBServer.Visible = false;
            this.TSCBServer.Image = Resources.Resource1.connect;
            this.TSCBServer.Alignment = ToolStripItemAlignment.Right;
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

        private void ShowTables(string dbname, string html)
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
            UC.WebTab panel = new WebTab(null,null);
            panel.SetHtml(html);
            panel.Text = tit;
            //panel.OnSearch += (w) =>
            //{

            //};
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
            Biz.UILoadHelper.LoadTBsAnsy(this, dbServerView1.FindNode(db.ServerName, dbName), db, null);
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
                    var allDBs = Biz.Common.XMLHelper.DeSerializeFromFile<DBSourceCollection>(Application.StartupPath + Resources.Resource1.DbServersFile) ?? new DBSourceCollection();
                    allDBs.Add(obj.DBSource);
                    Biz.Common.XMLHelper.Serialize(allDBs, Application.StartupPath + Resources.Resource1.DbServersFile);
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
            UC.WebTab panel = new WebTab(null, null);
            panel.SetHtml(html);
            panel.Text = tit;
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
    }
}
