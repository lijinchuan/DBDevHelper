namespace RedisHelperUI.UC
{
    partial class UCSearch
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TBSearchKey = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageData = new System.Windows.Forms.TabPage();
            this.DGVData = new System.Windows.Forms.DataGridView();
            this.TabPageInfo = new System.Windows.Forms.TabPage();
            this.TBMsg = new System.Windows.Forms.TextBox();
            this.CMSOP = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.增加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.TabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVData)).BeginInit();
            this.TabPageInfo.SuspendLayout();
            this.CMSOP.SuspendLayout();
            this.SuspendLayout();
            // 
            // TBSearchKey
            // 
            this.TBSearchKey.Location = new System.Drawing.Point(18, 9);
            this.TBSearchKey.Name = "TBSearchKey";
            this.TBSearchKey.Size = new System.Drawing.Size(287, 21);
            this.TBSearchKey.TabIndex = 0;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(311, 9);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(78, 21);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "查询键值";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.TabPageData);
            this.tabControl1.Controls.Add(this.TabPageInfo);
            this.tabControl1.Location = new System.Drawing.Point(18, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(539, 254);
            this.tabControl1.TabIndex = 3;
            // 
            // TabPageData
            // 
            this.TabPageData.Controls.Add(this.DGVData);
            this.TabPageData.Location = new System.Drawing.Point(4, 22);
            this.TabPageData.Name = "TabPageData";
            this.TabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageData.Size = new System.Drawing.Size(531, 228);
            this.TabPageData.TabIndex = 0;
            this.TabPageData.Text = "数据";
            this.TabPageData.UseVisualStyleBackColor = true;
            // 
            // DGVData
            // 
            this.DGVData.AllowUserToAddRows = false;
            this.DGVData.AllowUserToDeleteRows = false;
            this.DGVData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DGVData.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVData.Location = new System.Drawing.Point(3, 3);
            this.DGVData.Name = "DGVData";
            this.DGVData.RowTemplate.Height = 23;
            this.DGVData.Size = new System.Drawing.Size(525, 222);
            this.DGVData.TabIndex = 2;
            // 
            // TabPageInfo
            // 
            this.TabPageInfo.Controls.Add(this.TBMsg);
            this.TabPageInfo.Location = new System.Drawing.Point(4, 22);
            this.TabPageInfo.Name = "TabPageInfo";
            this.TabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageInfo.Size = new System.Drawing.Size(531, 228);
            this.TabPageInfo.TabIndex = 1;
            this.TabPageInfo.Text = "信息";
            this.TabPageInfo.UseVisualStyleBackColor = true;
            // 
            // TBMsg
            // 
            this.TBMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBMsg.ForeColor = System.Drawing.Color.Red;
            this.TBMsg.Location = new System.Drawing.Point(3, 3);
            this.TBMsg.Multiline = true;
            this.TBMsg.Name = "TBMsg";
            this.TBMsg.ReadOnly = true;
            this.TBMsg.Size = new System.Drawing.Size(525, 222);
            this.TBMsg.TabIndex = 0;
            // 
            // CMSOP
            // 
            this.CMSOP.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.增加ToolStripMenuItem,
            this.复制ToolStripMenuItem});
            this.CMSOP.Name = "CMSOP";
            this.CMSOP.Size = new System.Drawing.Size(153, 114);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            // 
            // 增加ToolStripMenuItem
            // 
            this.增加ToolStripMenuItem.Name = "增加ToolStripMenuItem";
            this.增加ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.增加ToolStripMenuItem.Text = "增加";
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            // 
            // UCSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.TBSearchKey);
            this.Name = "UCSearch";
            this.Size = new System.Drawing.Size(560, 290);
            this.tabControl1.ResumeLayout(false);
            this.TabPageData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVData)).EndInit();
            this.TabPageInfo.ResumeLayout(false);
            this.TabPageInfo.PerformLayout();
            this.CMSOP.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBSearchKey;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPageData;
        private System.Windows.Forms.TabPage TabPageInfo;
        private System.Windows.Forms.TextBox TBMsg;
        private System.Windows.Forms.DataGridView DGVData;
        private System.Windows.Forms.ContextMenuStrip CMSOP;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 增加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
    }
}
