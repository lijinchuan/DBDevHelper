namespace APIHelper.UC
{
    partial class UCJsonViewer
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
            this.DTV = new System.Windows.Forms.TreeView();
            this.CMSOP = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSMCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMView = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMExpend = new System.Windows.Forms.ToolStripMenuItem();
            this.CMSOP.SuspendLayout();
            this.SuspendLayout();
            // 
            // DTV
            // 
            this.DTV.ContextMenuStrip = this.CMSOP;
            this.DTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DTV.Location = new System.Drawing.Point(0, 0);
            this.DTV.Name = "DTV";
            this.DTV.Size = new System.Drawing.Size(432, 352);
            this.DTV.TabIndex = 0;
            // 
            // CMSOP
            // 
            this.CMSOP.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMCopy,
            this.TSMView,
            this.TSMExpend});
            this.CMSOP.Name = "CMSOP";
            this.CMSOP.Size = new System.Drawing.Size(181, 92);
            // 
            // TSMCopy
            // 
            this.TSMCopy.Name = "TSMCopy";
            this.TSMCopy.Size = new System.Drawing.Size(180, 22);
            this.TSMCopy.Text = "复制";
            this.TSMCopy.Click += new System.EventHandler(this.TSMCopy_Click);
            // 
            // TSMView
            // 
            this.TSMView.Name = "TSMView";
            this.TSMView.Size = new System.Drawing.Size(180, 22);
            this.TSMView.Text = "查看";
            this.TSMView.Click += new System.EventHandler(this.TSMView_Click);
            // 
            // TSMExpend
            // 
            this.TSMExpend.Name = "TSMExpend";
            this.TSMExpend.Size = new System.Drawing.Size(180, 22);
            this.TSMExpend.Text = "展开";
            this.TSMExpend.Click += new System.EventHandler(this.TSMExpend_Click);
            // 
            // UCJsonViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DTV);
            this.Name = "UCJsonViewer";
            this.Size = new System.Drawing.Size(432, 352);
            this.CMSOP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView DTV;
        private System.Windows.Forms.ContextMenuStrip CMSOP;
        private System.Windows.Forms.ToolStripMenuItem TSMCopy;
        private System.Windows.Forms.ToolStripMenuItem TSMView;
        private System.Windows.Forms.ToolStripMenuItem TSMExpend;
    }
}
