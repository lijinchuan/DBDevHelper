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
using Entity;
using Entity.WatchTask;
using ICSharpCode.SharpZipLib.Zip;
using APIHelper.SubForm;
using APIHelper.UC;
using System.Threading;
using LJC.FrameWorkV3.Data.EntityDataBase;
using Biz;
using LJC.FrameWorkV3.Net.HTTP.Server;
using LJC.FrameWorkV3.LogManager;

namespace APIHelper
{
    public partial class MainFrm : Form
    {
        private static MainFrm Instance = null;
        private System.Timers.Timer tasktimer = null;
        private WatingDlg wdlg = new WatingDlg();

        private void InitFrm()
        {
            this.tsb_Excute.Enabled = false;
            this.TSBSave.Enabled = false;
            this.dbServerView1.OnDeleteLogicMap += this.DeleteLogicMap;

            this.TabControl.Selected += new TabControlEventHandler(TabControl_Selected);

            this.TSCBServer.ForeColor = Color.HotPink;
            this.TSCBServer.Visible = false;
            this.TSCBServer.Image = Resources.Resource1.connect;
            this.TSCBServer.Alignment = ToolStripItemAlignment.Right;

            this.MspPanel.TextAlign = ContentAlignment.TopLeft;

            TSBSave.Click += TSBSave_Click;
            tsb_Excute.Click += Tsb_Excute_Click;

            
            TSMSetIeVersion.DropDownOpened += TSMSetIeVersion_DropDownOpened;
            TSMSetIeVersion.DropDownItemClicked += TSMSetIeVersion_DropDownItemClicked;

            TSBar.Image = Resources.Resource1.side_contract;
            TSBar.Click += TSBar_Click;
        }

        private void TSBar_Click(object sender, EventArgs e)
        {
            if (TSBar.Tag == null)
            {
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

                TSBar.Tag = null;
                dbServerView1.Show();
            }
        }

        private void TSMSetIeVersion_DropDownOpened(object sender, EventArgs e)
        {
            var ieversion = IEUtil.GetIEVersion();
            foreach (ToolStripItem item in TSMSetIeVersion.DropDownItems)
            {
                if (item.Text == ieversion.ToString())
                {
                    item.Enabled = false;
                }
                else
                {
                    item.Enabled = true;
                }
            }
        }

        private void TSMSetIeVersion_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var version = (IEUtil.IeVersion)Enum.Parse(typeof(IEUtil.IeVersion), e.ClickedItem.Text);
            if (IEUtil.SetIE(version))
            {
                Util.SendMsg(this, $"设置IE版本为{e.ClickedItem.Text}成功");
            }
            else
            {
                Util.SendMsg(this, $"设置IE版本为{e.ClickedItem.Text}失败");
            }
        }

        private void Tsb_Excute_Click(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab != null && TabControl.SelectedTab is IExcuteAble)
            {
                (TabControl.SelectedTab as IExcuteAble).Execute();
            }
        }

        private void TSBSave_Click(object sender, EventArgs e)
        {
            if(TabControl.SelectedTab!=null&&TabControl.SelectedTab is ISaveAble)
            {
                (TabControl.SelectedTab as ISaveAble).Save();
            }
        }

        public MainFrm()
        {
            InitializeComponent();
            InitFrm();
            Instance = this;

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("进入主窗口程序");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = false;
            wdlg.Show("初始化数据，请稍候...");
            
            try
            {
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
                Util.SendMsg(this, $"恢复关闭前选项卡失败:{ex.Message}", 180);
            }

            wdlg.Hide();
            this.Visible = true;

            try
            {
                var server = BigEntityTableEngine.LocalEngine.Find<SimulateServerConfig>(nameof(SimulateServerConfig), 1);
                if (server != null && server.Open)
                {
                    Biz.SimulateServer.SimulateServerManager.StartServer(server.Port);

                    LogHelper.Instance.Info("模拟服务器启动成功:" + server.Port);
                    Util.SendMsg(this, "模拟服务器启动成功,端口:" + server.Port);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Instance.Error("模拟服务器启动失败", ex);
                Util.SendMsg(this, "模拟服务器启动失败");
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("要退出吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            this.Visible = false;
            wdlg.Show("程序退出...");

            base.OnClosing(e);

            if (tasktimer != null)
            {
                tasktimer.Stop();
                tasktimer.Close();
            }

            try
            {
                wdlg.Msg = "保存工作数据...";
                Thread.Sleep(1000);
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

            try
            {
                wdlg.Msg = "其它工作...";
                BigEntityTableEngine.LocalEngine.ShutDown();
            }
            catch
            {

            }

            wdlg.Close();
        }

        void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            this.TSCBServer.Visible = false;

            TSBSave.Enabled = e.TabPage is ISaveAble;
            tsb_Excute.Enabled = e.TabPage is IExcuteAble;
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

        internal void ClearMsg(string oldmsg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (this.MspPanel.Text == oldmsg)
                    {
                        this.MspPanel.Text = string.Empty;
                    }
                }));
            }
            else
            {
                if (this.MspPanel.Text == oldmsg)
                {
                    this.MspPanel.Text = string.Empty;
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

        private void TabControl_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        public bool AddTab(string title, TabPage addpage)
        {
            bool isExists = false;
            foreach (TabPage page in this.TabControl.TabPages)
            {
                if (page.Text == title || page == addpage)
                {
                    isExists = true;
                    TabControl.SelectedTab = page;
                    break;
                }
            }

            if (!isExists)
            {
                this.TabControl.TabPages.Add(addpage);
                TabControl.SelectedTab = null;
                TabControl.SelectedTab = addpage;
                return true;
            }

            return false;
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

        private void swaggerMarkUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.ParseSwaggerDocDlg dlg = new ParseSwaggerDocDlg();
            dlg.Show();
        }

        private void mD5签名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.MD5Dlg dlg = new MD5Dlg();
            dlg.Owner = this;
            dlg.Show();
        }

        private void 时间戳ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tool.lu/timestamp/");
        }

        private void bASE64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tool.lu/encdec/");
        }

        private void 正则表达式测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tool.lu/regex/");
        }

        private void xML工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tool.lu/xml/");
        }

        private void jSON工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tool.lu/json/");
        }

        private void hTTP状态码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://tool.lu/httpcode/");
        }

        private void gUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.iamwawa.cn/guid.html");
        }

        private void 最近ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tp = new TabPage();
            
            if (Util.AddToMainTab(this, "最近访问", tp))
            {
                var logview = new LogViewTab();
                logview.Dock = DockStyle.Fill;
                tp.Controls.Add(logview);

                logview.Init(0, 0);

                logview.ReInvoke += (log,b) =>
                {
                    var apiurl = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), log.APIId);
                    if (apiurl != null)
                    {
                        var apisource = BigEntityTableEngine.LocalEngine.Find<APISource>(nameof(APISource), apiurl.SourceId);
                        if (apisource != null)
                        {
                            Util.AddToMainTab(this, $"[{apisource.SourceName}]{apiurl.APIName}", new UCAddAPI(apiurl));
                        }
                    }
                };
            }
        }

        private void 代理服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SubForm.SubBaseDlg();
            dlg.Text = "全局代理服务器";
            var globProxyServer = BigEntityTableEngine.LocalEngine.Find<ProxyServer>(nameof(ProxyServer), p => p.Name.Equals(ProxyServer.GlobName)).FirstOrDefault();
            
            var ucproxy =new UC.UCProxy(globProxyServer);
            ucproxy.Dock = DockStyle.Fill;
            dlg.Controls.Add(ucproxy);
            dlg.FormClosing += (s, ee) =>
            {
                var proxyserver = ucproxy.GetProxyServer();

                if (ucproxy.HasChanged && MessageBox.Show("要保存吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (proxyserver.Id == 0)
                    {
                        proxyserver.Name = ProxyServer.GlobName;
                        BigEntityTableEngine.LocalEngine.Insert<ProxyServer>(nameof(ProxyServer), proxyserver);
                        Util.SendMsg(this, "新增成功");
                    }
                    else
                    {
                        BigEntityTableEngine.LocalEngine.Update<ProxyServer>(nameof(ProxyServer), proxyserver);
                        Util.SendMsg(this, "修改成功");
                    }
                    
                }
            };
            dlg.ShowDialog();
        }

        private void TSMReportError_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:403479851@qq.com?subject=api管理工具v1.0使用问题反馈");
            }
            catch
            {
                MessageBox.Show("启动发送邮件应用失败，请手动发送邮件到：403479851@qq.com");
            }
        }

        private void 重置IE浏览器COOKIEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var boo = Biz.IEUtil.SuppressWininetBehavior();
                if (boo)
                {
                    Util.SendMsg(this, "重置COOKIE成功");
                }
                else
                {
                    Util.SendMsg(this, "重置COOKIE失败");
                }
            }
            catch (Exception ex)
            {
                Util.SendMsg(this, "重置COOKIE出错:" + ex.Message);
            }
        }

        private void uRLEncodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.URLEncodeDlg dlg = new URLEncodeDlg();

            dlg.Show();
        }

        private void 模拟服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.SimulateServerDlg dlg = new SimulateServerDlg();
            dlg.ShowDialog();
        }
    }
}
