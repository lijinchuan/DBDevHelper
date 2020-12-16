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

namespace APIHelper
{
    public partial class MainFrm : Form
    {
        private static MainFrm Instance = null;
        private System.Timers.Timer tasktimer = null;

        private void InitFrm()
        {
            this.tsb_Excute.Enabled = false;
            this.dbServerView1.OnDeleteLogicMap += this.DeleteLogicMap;

            this.TabControl.Selected += new TabControlEventHandler(TabControl_Selected);

            this.TSCBServer.ForeColor = Color.HotPink;
            this.TSCBServer.Visible = false;
            this.TSCBServer.Image = Resources.Resource1.connect;
            this.TSCBServer.Alignment = ToolStripItemAlignment.Right;

            this.MspPanel.TextAlign = ContentAlignment.TopLeft;
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
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("要退出吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
                return;
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
            this.TSCBServer.Visible = false;
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

        public void AddTab(string title, TabPage addpage)
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
                TabControl.SelectedTab = addpage;
            }
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
    }
}
