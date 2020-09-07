namespace NETDBHelper.UC
{
    partial class UCTableRelMap
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
            this.PanelMap = new System.Windows.Forms.Panel();
            this.CMSOpMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMDelRelColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.CMSOpMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelMap
            // 
            this.PanelMap.ContextMenuStrip = this.CMSOpMenu;
            this.PanelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMap.Location = new System.Drawing.Point(0, 0);
            this.PanelMap.Name = "PanelMap";
            this.PanelMap.Size = new System.Drawing.Size(565, 412);
            this.PanelMap.TabIndex = 0;
            // 
            // CMSOpMenu
            // 
            this.CMSOpMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加表ToolStripMenuItem,
            this.delStripMenuItem,
            this.TSMDelRelColumn,
            this.TSMI_Export});
            this.CMSOpMenu.Name = "CMSOpMenu";
            this.CMSOpMenu.Size = new System.Drawing.Size(181, 114);
            // 
            // 添加表ToolStripMenuItem
            // 
            this.添加表ToolStripMenuItem.Name = "添加表ToolStripMenuItem";
            this.添加表ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.添加表ToolStripMenuItem.Text = "添加表";
            this.添加表ToolStripMenuItem.Click += new System.EventHandler(this.添加表ToolStripMenuItem_Click);
            // 
            // delStripMenuItem
            // 
            this.delStripMenuItem.Name = "delStripMenuItem";
            this.delStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.delStripMenuItem.Text = "删除表";
            this.delStripMenuItem.Click += new System.EventHandler(this.delStripMenuItem_Click);
            // 
            // TSMDelRelColumn
            // 
            this.TSMDelRelColumn.Name = "TSMDelRelColumn";
            this.TSMDelRelColumn.Size = new System.Drawing.Size(180, 22);
            this.TSMDelRelColumn.Text = "删除字段关联";
            // 
            // TSMI_Export
            // 
            this.TSMI_Export.Name = "TSMI_Export";
            this.TSMI_Export.Size = new System.Drawing.Size(180, 22);
            this.TSMI_Export.Text = "导出为逻辑图片";
            this.TSMI_Export.Click += new System.EventHandler(this.TSMI_Export_Click);
            // 
            // UCTableRelMap
            // 
            this.Controls.Add(this.PanelMap);
            this.Name = "UCTableRelMap";
            this.Size = new System.Drawing.Size(565, 412);
            this.CMSOpMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelMap;
        private System.Windows.Forms.ContextMenuStrip CMSOpMenu;
        private System.Windows.Forms.ToolStripMenuItem 添加表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMDelRelColumn;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Export;
    }
}
