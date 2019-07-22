namespace NETDBHelper.UC
{
    partial class SqlExcuter
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sqlEditBox1 = new NETDBHelper.UC.SQLEditBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TPInfo = new System.Windows.Forms.TabPage();
            this.TBInfo = new System.Windows.Forms.TextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.TPInfo.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sqlEditBox1
            // 
            this.sqlEditBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sqlEditBox1.DBName = null;
            this.sqlEditBox1.Location = new System.Drawing.Point(8, 3);
            this.sqlEditBox1.Name = "sqlEditBox1";
            this.sqlEditBox1.Size = new System.Drawing.Size(581, 176);
            this.sqlEditBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TPInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 185);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(596, 245);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.DoubleClick += new System.EventHandler(this.TabControl1_DoubleClick);
            // 
            // TPInfo
            // 
            this.TPInfo.Controls.Add(this.TBInfo);
            this.TPInfo.Location = new System.Drawing.Point(4, 23);
            this.TPInfo.Name = "TPInfo";
            this.TPInfo.Padding = new System.Windows.Forms.Padding(3);
            this.TPInfo.Size = new System.Drawing.Size(588, 218);
            this.TPInfo.TabIndex = 1;
            this.TPInfo.Text = "信息";
            this.TPInfo.UseVisualStyleBackColor = true;
            // 
            // TBInfo
            // 
            this.TBInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBInfo.Location = new System.Drawing.Point(3, 3);
            this.TBInfo.Multiline = true;
            this.TBInfo.Name = "TBInfo";
            this.TBInfo.ReadOnly = true;
            this.TBInfo.Size = new System.Drawing.Size(582, 212);
            this.TBInfo.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);
            // 
            // SqlExcuter
            // 
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.sqlEditBox1);
            this.Size = new System.Drawing.Size(596, 430);
            this.tabControl1.ResumeLayout(false);
            this.TPInfo.ResumeLayout(false);
            this.TPInfo.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SQLEditBox sqlEditBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TPInfo;
        private System.Windows.Forms.TextBox TBInfo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}
