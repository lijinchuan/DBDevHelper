namespace RedisHelperUI
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.TSM_File = new System.Windows.Forms.ToolStripMenuItem();
            this.TSM_NewRedis = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMDel = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMSearchKey = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelLeft = new System.Windows.Forms.Panel();
            this.TVServerList = new System.Windows.Forms.TreeView();
            this.PanelRight = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.PanelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSM_File,
            this.TSMEdit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(899, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // TSM_File
            // 
            this.TSM_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSM_NewRedis,
            this.TSMDel});
            this.TSM_File.Name = "TSM_File";
            this.TSM_File.Size = new System.Drawing.Size(44, 21);
            this.TSM_File.Text = "文件";
            // 
            // TSM_NewRedis
            // 
            this.TSM_NewRedis.Name = "TSM_NewRedis";
            this.TSM_NewRedis.Size = new System.Drawing.Size(165, 22);
            this.TSM_NewRedis.Text = "添加redis服务器";
            this.TSM_NewRedis.Click += new System.EventHandler(this.TSM_NewRedis_Click);
            // 
            // TSMDel
            // 
            this.TSMDel.Name = "TSMDel";
            this.TSMDel.Size = new System.Drawing.Size(165, 22);
            this.TSMDel.Text = "删除";
            this.TSMDel.Click += new System.EventHandler(this.TSMDel_Click);
            // 
            // TSMEdit
            // 
            this.TSMEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMSearch,
            this.TSMSearchKey});
            this.TSMEdit.Name = "TSMEdit";
            this.TSMEdit.Size = new System.Drawing.Size(44, 21);
            this.TSMEdit.Text = "编辑";
            // 
            // TSMSearch
            // 
            this.TSMSearch.Name = "TSMSearch";
            this.TSMSearch.Size = new System.Drawing.Size(120, 22);
            this.TSMSearch.Text = "查询";
            this.TSMSearch.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // TSMSearchKey
            // 
            this.TSMSearchKey.Name = "TSMSearchKey";
            this.TSMSearchKey.Size = new System.Drawing.Size(120, 22);
            this.TSMSearchKey.Text = "搜索key";
            this.TSMSearchKey.Click += new System.EventHandler(this.TSMSearchKey_Click);
            // 
            // PanelLeft
            // 
            this.PanelLeft.Controls.Add(this.TVServerList);
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelLeft.Location = new System.Drawing.Point(0, 25);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(200, 508);
            this.PanelLeft.TabIndex = 1;
            // 
            // TVServerList
            // 
            this.TVServerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TVServerList.Location = new System.Drawing.Point(0, 0);
            this.TVServerList.Name = "TVServerList";
            this.TVServerList.Size = new System.Drawing.Size(200, 508);
            this.TVServerList.TabIndex = 0;
            // 
            // PanelRight
            // 
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelRight.Location = new System.Drawing.Point(200, 25);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(699, 508);
            this.PanelRight.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 533);
            this.Controls.Add(this.PanelRight);
            this.Controls.Add(this.PanelLeft);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Redis管理";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.PanelLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TSM_File;
        private System.Windows.Forms.ToolStripMenuItem TSM_NewRedis;
        private System.Windows.Forms.Panel PanelLeft;
        private System.Windows.Forms.TreeView TVServerList;
        private System.Windows.Forms.ToolStripMenuItem TSMDel;
        private System.Windows.Forms.Panel PanelRight;
        private System.Windows.Forms.ToolStripMenuItem TSMEdit;
        private System.Windows.Forms.ToolStripMenuItem TSMSearch;
        private System.Windows.Forms.ToolStripMenuItem TSMSearchKey;
    }
}

