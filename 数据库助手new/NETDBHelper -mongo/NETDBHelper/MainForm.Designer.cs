﻿namespace NETDBHelper
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.mainMenuBar = new System.Windows.Forms.MenuStrip();
            this.MenItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.连接对象资源管理器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.断开对象资源管理器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemTool = new System.Windows.Forms.ToolStripMenuItem();
            this.查看日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.常用SQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.监控任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TabControl = new NETDBHelper.UC.MyTabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_Excute = new System.Windows.Forms.ToolStripButton();
            this.TSCBServer = new System.Windows.Forms.ToolStripLabel();
            this.dbServerView1 = new NETDBHelper.DBServerView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.MspPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSL_ClearMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuBar
            // 
            this.mainMenuBar.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.mainMenuBar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.mainMenuBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.mainMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenItem_File,
            this.SubItemEdit,
            this.SubItemView,
            this.SubItemTool,
            this.SubItemHelp});
            this.mainMenuBar.Location = new System.Drawing.Point(0, 0);
            this.mainMenuBar.Name = "mainMenuBar";
            this.mainMenuBar.Size = new System.Drawing.Size(830, 25);
            this.mainMenuBar.TabIndex = 0;
            this.mainMenuBar.Text = "menuStrip1";
            // 
            // MenItem_File
            // 
            this.MenItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接对象资源管理器ToolStripMenuItem,
            this.断开对象资源管理器ToolStripMenuItem});
            this.MenItem_File.Name = "MenItem_File";
            this.MenItem_File.Size = new System.Drawing.Size(58, 21);
            this.MenItem_File.Text = "文件(F)";
            // 
            // 连接对象资源管理器ToolStripMenuItem
            // 
            this.连接对象资源管理器ToolStripMenuItem.Name = "连接对象资源管理器ToolStripMenuItem";
            this.连接对象资源管理器ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.连接对象资源管理器ToolStripMenuItem.Text = "连接对象资源管理器";
            this.连接对象资源管理器ToolStripMenuItem.Click += new System.EventHandler(this.连接对象资源管理器ToolStripMenuItem_Click);
            // 
            // 断开对象资源管理器ToolStripMenuItem
            // 
            this.断开对象资源管理器ToolStripMenuItem.Name = "断开对象资源管理器ToolStripMenuItem";
            this.断开对象资源管理器ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.断开对象资源管理器ToolStripMenuItem.Text = "断开对象资源管理器";
            this.断开对象资源管理器ToolStripMenuItem.Click += new System.EventHandler(this.断开对象资源管理器ToolStripMenuItem_Click);
            // 
            // SubItemEdit
            // 
            this.SubItemEdit.Name = "SubItemEdit";
            this.SubItemEdit.Size = new System.Drawing.Size(59, 21);
            this.SubItemEdit.Text = "编辑(E)";
            // 
            // SubItemView
            // 
            this.SubItemView.Name = "SubItemView";
            this.SubItemView.Size = new System.Drawing.Size(44, 21);
            this.SubItemView.Text = "视图";
            // 
            // SubItemTool
            // 
            this.SubItemTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看日志ToolStripMenuItem,
            this.常用SQLToolStripMenuItem,
            this.监控任务ToolStripMenuItem});
            this.SubItemTool.Name = "SubItemTool";
            this.SubItemTool.Size = new System.Drawing.Size(44, 21);
            this.SubItemTool.Text = "工具";
            // 
            // 查看日志ToolStripMenuItem
            // 
            this.查看日志ToolStripMenuItem.Name = "查看日志ToolStripMenuItem";
            this.查看日志ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.查看日志ToolStripMenuItem.Text = "查看日志";
            this.查看日志ToolStripMenuItem.Click += new System.EventHandler(this.查看日志ToolStripMenuItem_Click);
            // 
            // 常用SQLToolStripMenuItem
            // 
            this.常用SQLToolStripMenuItem.Name = "常用SQLToolStripMenuItem";
            this.常用SQLToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.常用SQLToolStripMenuItem.Text = "常用SQL";
            this.常用SQLToolStripMenuItem.Click += new System.EventHandler(this.常用SQLToolStripMenuItem_Click);
            // 
            // 监控任务ToolStripMenuItem
            // 
            this.监控任务ToolStripMenuItem.Name = "监控任务ToolStripMenuItem";
            this.监控任务ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.监控任务ToolStripMenuItem.Text = "监控任务";
            this.监控任务ToolStripMenuItem.Click += new System.EventHandler(this.监控任务ToolStripMenuItem_Click);
            // 
            // SubItemHelp
            // 
            this.SubItemHelp.Name = "SubItemHelp";
            this.SubItemHelp.Size = new System.Drawing.Size(44, 21);
            this.SubItemHelp.Text = "帮助";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.TabControl);
            this.panel1.Location = new System.Drawing.Point(259, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(571, 478);
            this.panel1.TabIndex = 2;
            // 
            // TabControl
            // 
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TabControl.ItemSize = new System.Drawing.Size(1, 18);
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(569, 476);
            this.TabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabControl.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Excute,
            this.TSCBServer});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(830, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tsb_Excute
            // 
            this.tsb_Excute.Image = global::NETDBHelper.Properties.Resources.新建位图图像__2_;
            this.tsb_Excute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Excute.Name = "tsb_Excute";
            this.tsb_Excute.Size = new System.Drawing.Size(52, 22);
            this.tsb_Excute.Text = "执行";
            // 
            // TSCBServer
            // 
            this.TSCBServer.Name = "TSCBServer";
            this.TSCBServer.Size = new System.Drawing.Size(0, 22);
            // 
            // dbServerView1
            // 
            this.dbServerView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dbServerView1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dbServerView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dbServerView1.Location = new System.Drawing.Point(0, 53);
            this.dbServerView1.Name = "dbServerView1";
            this.dbServerView1.Size = new System.Drawing.Size(253, 478);
            this.dbServerView1.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MspPanel,
            this.TSL_ClearMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 531);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(830, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MspPanel
            // 
            this.MspPanel.Name = "MspPanel";
            this.MspPanel.Size = new System.Drawing.Size(0, 17);
            // 
            // TSL_ClearMsg
            // 
            this.TSL_ClearMsg.Image = global::NETDBHelper.Properties.Resources.cross;
            this.TSL_ClearMsg.Name = "TSL_ClearMsg";
            this.TSL_ClearMsg.Size = new System.Drawing.Size(16, 17);
            this.TSL_ClearMsg.Visible = false;
            this.TSL_ClearMsg.Click += new System.EventHandler(this.TSL_ClearMsg_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 553);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dbServerView1);
            this.Controls.Add(this.mainMenuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuBar;
            this.Name = "MainFrm";
            this.Text = "数据库开发助手（Mongo）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuBar.ResumeLayout(false);
            this.mainMenuBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuBar;
        private System.Windows.Forms.ToolStripMenuItem MenItem_File;
        private System.Windows.Forms.ToolStripMenuItem 连接对象资源管理器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 断开对象资源管理器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SubItemEdit;
        private System.Windows.Forms.ToolStripMenuItem SubItemView;
        private System.Windows.Forms.ToolStripMenuItem SubItemTool;
        private System.Windows.Forms.ToolStripMenuItem SubItemHelp;
        private DBServerView dbServerView1;
        private System.Windows.Forms.Panel panel1;
        private UC.MyTabControl TabControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Excute;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel MspPanel;
        private System.Windows.Forms.ToolStripMenuItem 查看日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 常用SQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel TSCBServer;
        private System.Windows.Forms.ToolStripStatusLabel TSL_ClearMsg;
        private System.Windows.Forms.ToolStripMenuItem 监控任务ToolStripMenuItem;
    }
}

