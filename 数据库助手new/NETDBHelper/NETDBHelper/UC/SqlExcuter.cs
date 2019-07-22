using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;

namespace NETDBHelper.UC
{
    public partial class SqlExcuter : TabPage
    {
        public SqlExcuter()
        {
            InitializeComponent();

            
        }

        public DBSource Server
        {
            get;
            set;
        }

        private string DB
        {
            get;
            set;
        }

        public SqlExcuter(DBSource server,string db,string sql)
        {
            InitializeComponent();

            this.Server = server;
            this.DB = db;
            this.sqlEditBox1.DBName = db;
            this.sqlEditBox1.Text = sql;

            this.TBInfo.ScrollBars = ScrollBars.Both;
            this.TBInfo.ContextMenuStrip = contextMenuStrip1;

            this.imageList1.Images.Add(Resources.Resource1.tbview);
            this.imageList1.Images.Add(Resources.Resource1.msg);
            this.tabControl1.TabPages[0].ImageIndex = 1;
        }

        public void Execute()
        {
            var seltext = this.sqlEditBox1.SelectedText;
            if (string.IsNullOrWhiteSpace(seltext))
            {
                seltext = this.sqlEditBox1.Text;
            }

            try
            {
                List<TabPage> listrm = new List<TabPage>();
                foreach(TabPage tp in tabControl1.TabPages)
                {
                    if (tp == TPInfo)
                    {
                        continue;
                    }
                    listrm.Add(tp);
                }
                foreach (var tp in listrm)
                {
                    tabControl1.TabPages.Remove(tp);
                }
                DateTime now = DateTime.Now;
                var ts = Biz.Common.Data.SQLHelper.ExecuteDataSet(Server, DB, seltext, (s, e) =>
                 {
                     TBInfo.Text += $"{e.Message}\r\n\r\n";
                     if (e.Errors != null && e.Errors.Count > 0)
                     {
                         for (int i = 0; i < e.Errors.Count; i++)
                         {
                             TBInfo.Text += $"{e.Errors[i].Message}\r\n\r\n";
                         }
                     }
                 });
                TBInfo.Text += $"用时:{DateTime.Now.Subtract(now).TotalMilliseconds}ms\r\n";
                if (ts != null && ts.Tables.Count > 0)
                {
                    for(int i = 0; i < ts.Tables.Count; i++)
                    {
                        var tb = ts.Tables[i];
                        TabPage page = new TabPage(tb.TableName ?? "未命名表");
                        page.ImageIndex = 0;
                        var dgv = new DataGridView();
                        page.Controls.Add(dgv);
                        dgv.CellDoubleClick += (s,e)=>
                        {
                            if (dgv.CurrentCell.Value != null)
                            {
                                Clipboard.SetText(dgv.CurrentCell.Value.ToString());
                                MessageBox.Show("已复制到剪贴板");
                            }
                        };
                        dgv.BorderStyle = BorderStyle.None;
                        dgv.GridColor = Color.LightBlue;
                        dgv.Dock = DockStyle.Fill;
                        dgv.BackgroundColor = Color.White;
                        dgv.AllowUserToAddRows = false;
                        dgv.ReadOnly = true;
                        dgv.DataSource = tb;
                        tabControl1.TabPages.Insert(i, page);
                        if (i == 0)
                        {
                            tabControl1.SelectedTab = page;
                        }
                    }
                }
                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = "sql",
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.sql,
                    DB = this.DB,
                    Sever = Server.ServerName,
                    Info = seltext,
                    Valid = true
                });
            }
            catch (Exception ex)
            {
                TBInfo.Text += ex.Message+"\r\n";
                tabControl1.SelectedTab = TPInfo;
            }

        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TBInfo.Text = string.Empty;
        }

        private void TabControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.tabControl1.Dock == DockStyle.Fill)
            {
                this.tabControl1.Dock = DockStyle.Bottom;
            }
            else
            {
                this.tabControl1.Dock = DockStyle.Fill;
            }
        }
    }
}
