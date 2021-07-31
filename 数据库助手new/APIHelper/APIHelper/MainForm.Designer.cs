namespace APIHelper
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
            this.最近ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.代理服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMSetIeVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.标准ie7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标准ie8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.强制ie8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标准ie9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.强制ie9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标准ie10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.强制ie10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标准ie11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.强制ie11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemTool = new System.Windows.Forms.ToolStripMenuItem();
            this.监控任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mD5签名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gUIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.时间戳ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bASE64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.正则表达式测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xML工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jSON工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hTTP状态码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swaggerMarkUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.版本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMReportError = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TabControl = new APIHelper.UC.MyTabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TSCBServer = new System.Windows.Forms.ToolStripLabel();
            this.TSBSave = new System.Windows.Forms.ToolStripButton();
            this.tsb_Excute = new System.Windows.Forms.ToolStripButton();
            this.dbServerView1 = new APIHelper.DBServerView();
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
            this.最近ToolStripMenuItem});
            this.MenItem_File.Name = "MenItem_File";
            this.MenItem_File.Size = new System.Drawing.Size(58, 21);
            this.MenItem_File.Text = "文件(F)";
            // 
            // 最近ToolStripMenuItem
            // 
            this.最近ToolStripMenuItem.Name = "最近ToolStripMenuItem";
            this.最近ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.最近ToolStripMenuItem.Text = "最近访问";
            this.最近ToolStripMenuItem.Click += new System.EventHandler(this.最近ToolStripMenuItem_Click);
            // 
            // SubItemEdit
            // 
            this.SubItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.代理服务器ToolStripMenuItem,
            this.TSMSetIeVersion});
            this.SubItemEdit.Name = "SubItemEdit";
            this.SubItemEdit.Size = new System.Drawing.Size(59, 21);
            this.SubItemEdit.Text = "编辑(E)";
            // 
            // 代理服务器ToolStripMenuItem
            // 
            this.代理服务器ToolStripMenuItem.Name = "代理服务器ToolStripMenuItem";
            this.代理服务器ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.代理服务器ToolStripMenuItem.Text = "代理服务器";
            this.代理服务器ToolStripMenuItem.Click += new System.EventHandler(this.代理服务器ToolStripMenuItem_Click);
            // 
            // TSMSetIeVersion
            // 
            this.TSMSetIeVersion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.标准ie7ToolStripMenuItem,
            this.标准ie8ToolStripMenuItem,
            this.强制ie8ToolStripMenuItem,
            this.标准ie9ToolStripMenuItem,
            this.强制ie9ToolStripMenuItem,
            this.标准ie10ToolStripMenuItem,
            this.强制ie10ToolStripMenuItem,
            this.标准ie11ToolStripMenuItem,
            this.强制ie11ToolStripMenuItem});
            this.TSMSetIeVersion.Name = "TSMSetIeVersion";
            this.TSMSetIeVersion.Size = new System.Drawing.Size(195, 22);
            this.TSMSetIeVersion.Text = "指定内嵌IE浏览器版本";
            // 
            // 标准ie7ToolStripMenuItem
            // 
            this.标准ie7ToolStripMenuItem.Name = "标准ie7ToolStripMenuItem";
            this.标准ie7ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.标准ie7ToolStripMenuItem.Text = "标准ie7";
            // 
            // 标准ie8ToolStripMenuItem
            // 
            this.标准ie8ToolStripMenuItem.Name = "标准ie8ToolStripMenuItem";
            this.标准ie8ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.标准ie8ToolStripMenuItem.Text = "标准ie8";
            // 
            // 强制ie8ToolStripMenuItem
            // 
            this.强制ie8ToolStripMenuItem.Name = "强制ie8ToolStripMenuItem";
            this.强制ie8ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.强制ie8ToolStripMenuItem.Text = "强制ie8";
            // 
            // 标准ie9ToolStripMenuItem
            // 
            this.标准ie9ToolStripMenuItem.Name = "标准ie9ToolStripMenuItem";
            this.标准ie9ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.标准ie9ToolStripMenuItem.Text = "标准ie9";
            // 
            // 强制ie9ToolStripMenuItem
            // 
            this.强制ie9ToolStripMenuItem.Name = "强制ie9ToolStripMenuItem";
            this.强制ie9ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.强制ie9ToolStripMenuItem.Text = "强制ie9";
            // 
            // 标准ie10ToolStripMenuItem
            // 
            this.标准ie10ToolStripMenuItem.Name = "标准ie10ToolStripMenuItem";
            this.标准ie10ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.标准ie10ToolStripMenuItem.Text = "标准ie10";
            // 
            // 强制ie10ToolStripMenuItem
            // 
            this.强制ie10ToolStripMenuItem.Name = "强制ie10ToolStripMenuItem";
            this.强制ie10ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.强制ie10ToolStripMenuItem.Text = "强制ie10";
            // 
            // 标准ie11ToolStripMenuItem
            // 
            this.标准ie11ToolStripMenuItem.Name = "标准ie11ToolStripMenuItem";
            this.标准ie11ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.标准ie11ToolStripMenuItem.Text = "标准ie11";
            // 
            // 强制ie11ToolStripMenuItem
            // 
            this.强制ie11ToolStripMenuItem.Name = "强制ie11ToolStripMenuItem";
            this.强制ie11ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.强制ie11ToolStripMenuItem.Text = "强制ie11";
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
            this.监控任务ToolStripMenuItem,
            this.mD5签名ToolStripMenuItem,
            this.gUIDToolStripMenuItem,
            this.时间戳ToolStripMenuItem,
            this.bASE64ToolStripMenuItem,
            this.正则表达式测试ToolStripMenuItem,
            this.xML工具ToolStripMenuItem,
            this.jSON工具ToolStripMenuItem,
            this.hTTP状态码ToolStripMenuItem,
            this.swaggerMarkUpToolStripMenuItem});
            this.SubItemTool.Name = "SubItemTool";
            this.SubItemTool.Size = new System.Drawing.Size(44, 21);
            this.SubItemTool.Text = "工具";
            // 
            // 监控任务ToolStripMenuItem
            // 
            this.监控任务ToolStripMenuItem.Name = "监控任务ToolStripMenuItem";
            this.监控任务ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.监控任务ToolStripMenuItem.Text = "监控任务";
            this.监控任务ToolStripMenuItem.Visible = false;
            this.监控任务ToolStripMenuItem.Click += new System.EventHandler(this.监控任务ToolStripMenuItem_Click);
            // 
            // mD5签名ToolStripMenuItem
            // 
            this.mD5签名ToolStripMenuItem.Name = "mD5签名ToolStripMenuItem";
            this.mD5签名ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.mD5签名ToolStripMenuItem.Text = "MD5签名";
            this.mD5签名ToolStripMenuItem.Click += new System.EventHandler(this.mD5签名ToolStripMenuItem_Click);
            // 
            // gUIDToolStripMenuItem
            // 
            this.gUIDToolStripMenuItem.Name = "gUIDToolStripMenuItem";
            this.gUIDToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.gUIDToolStripMenuItem.Text = "GUID";
            this.gUIDToolStripMenuItem.Click += new System.EventHandler(this.gUIDToolStripMenuItem_Click);
            // 
            // 时间戳ToolStripMenuItem
            // 
            this.时间戳ToolStripMenuItem.Name = "时间戳ToolStripMenuItem";
            this.时间戳ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.时间戳ToolStripMenuItem.Text = "时间戳";
            this.时间戳ToolStripMenuItem.Click += new System.EventHandler(this.时间戳ToolStripMenuItem_Click);
            // 
            // bASE64ToolStripMenuItem
            // 
            this.bASE64ToolStripMenuItem.Name = "bASE64ToolStripMenuItem";
            this.bASE64ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.bASE64ToolStripMenuItem.Text = "BASE64";
            this.bASE64ToolStripMenuItem.Click += new System.EventHandler(this.bASE64ToolStripMenuItem_Click);
            // 
            // 正则表达式测试ToolStripMenuItem
            // 
            this.正则表达式测试ToolStripMenuItem.Name = "正则表达式测试ToolStripMenuItem";
            this.正则表达式测试ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.正则表达式测试ToolStripMenuItem.Text = "正则表达式测试";
            this.正则表达式测试ToolStripMenuItem.Click += new System.EventHandler(this.正则表达式测试ToolStripMenuItem_Click);
            // 
            // xML工具ToolStripMenuItem
            // 
            this.xML工具ToolStripMenuItem.Name = "xML工具ToolStripMenuItem";
            this.xML工具ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.xML工具ToolStripMenuItem.Text = "XML工具";
            this.xML工具ToolStripMenuItem.Click += new System.EventHandler(this.xML工具ToolStripMenuItem_Click);
            // 
            // jSON工具ToolStripMenuItem
            // 
            this.jSON工具ToolStripMenuItem.Name = "jSON工具ToolStripMenuItem";
            this.jSON工具ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.jSON工具ToolStripMenuItem.Text = "JSON工具";
            this.jSON工具ToolStripMenuItem.Click += new System.EventHandler(this.jSON工具ToolStripMenuItem_Click);
            // 
            // hTTP状态码ToolStripMenuItem
            // 
            this.hTTP状态码ToolStripMenuItem.Name = "hTTP状态码ToolStripMenuItem";
            this.hTTP状态码ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.hTTP状态码ToolStripMenuItem.Text = "HTTP状态码";
            this.hTTP状态码ToolStripMenuItem.Click += new System.EventHandler(this.hTTP状态码ToolStripMenuItem_Click);
            // 
            // swaggerMarkUpToolStripMenuItem
            // 
            this.swaggerMarkUpToolStripMenuItem.Name = "swaggerMarkUpToolStripMenuItem";
            this.swaggerMarkUpToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.swaggerMarkUpToolStripMenuItem.Text = "Swagger->MarkUp";
            this.swaggerMarkUpToolStripMenuItem.Click += new System.EventHandler(this.swaggerMarkUpToolStripMenuItem_Click);
            // 
            // SubItemHelp
            // 
            this.SubItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.版本ToolStripMenuItem,
            this.TSMReportError});
            this.SubItemHelp.Name = "SubItemHelp";
            this.SubItemHelp.Size = new System.Drawing.Size(44, 21);
            this.SubItemHelp.Text = "帮助";
            // 
            // 版本ToolStripMenuItem
            // 
            this.版本ToolStripMenuItem.Name = "版本ToolStripMenuItem";
            this.版本ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.版本ToolStripMenuItem.Text = "当前版本V1.1";
            // 
            // TSMReportError
            // 
            this.TSMReportError.Name = "TSMReportError";
            this.TSMReportError.Size = new System.Drawing.Size(149, 22);
            this.TSMReportError.Text = "故障报告";
            this.TSMReportError.Click += new System.EventHandler(this.TSMReportError_Click);
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
            this.TabControl.ItemSize = new System.Drawing.Size(0, 18);
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(569, 476);
            this.TabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabControl.TabIndex = 0;
            this.TabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TabControl_MouseDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSCBServer,
            this.TSBSave,
            this.tsb_Excute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(830, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // TSCBServer
            // 
            this.TSCBServer.Name = "TSCBServer";
            this.TSCBServer.Size = new System.Drawing.Size(0, 22);
            // 
            // TSBSave
            // 
            this.TSBSave.Image = global::APIHelper.Properties.Resources.disk_black;
            this.TSBSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBSave.Name = "TSBSave";
            this.TSBSave.Size = new System.Drawing.Size(52, 22);
            this.TSBSave.Text = "保存";
            // 
            // tsb_Excute
            // 
            this.tsb_Excute.Image = global::APIHelper.Properties.Resources.新建位图图像__2_;
            this.tsb_Excute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Excute.Name = "tsb_Excute";
            this.tsb_Excute.Size = new System.Drawing.Size(52, 22);
            this.tsb_Excute.Text = "执行";
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
            this.MspPanel.ForeColor = System.Drawing.Color.Red;
            this.MspPanel.Name = "MspPanel";
            this.MspPanel.Size = new System.Drawing.Size(20, 17);
            this.MspPanel.Text = "   ";
            // 
            // TSL_ClearMsg
            // 
            this.TSL_ClearMsg.Image = global::APIHelper.Properties.Resources.cross;
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
            this.Text = "流星蝴蝶剑";
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
        private System.Windows.Forms.ToolStripLabel TSCBServer;
        private System.Windows.Forms.ToolStripStatusLabel TSL_ClearMsg;
        private System.Windows.Forms.ToolStripMenuItem 监控任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swaggerMarkUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton TSBSave;
        private System.Windows.Forms.ToolStripMenuItem mD5签名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 时间戳ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bASE64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 正则表达式测试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xML工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jSON工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hTTP状态码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 版本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gUIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最近ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 代理服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMReportError;
        private System.Windows.Forms.ToolStripMenuItem TSMSetIeVersion;
        private System.Windows.Forms.ToolStripMenuItem 标准ie7ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标准ie8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 强制ie8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标准ie9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 强制ie9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标准ie10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 强制ie10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标准ie11ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 强制ie11ToolStripMenuItem;
    }
}

