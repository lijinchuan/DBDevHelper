namespace APIHelper.UC
{
    partial class UCParamsTable
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
            this.DGV = new System.Windows.Forms.DataGridView();
            this.CBEditType = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PbAddParams = new System.Windows.Forms.PictureBox();
            this.PbRemParams = new System.Windows.Forms.PictureBox();
            this.PBHelp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbAddParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbRemParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV.BackgroundColor = System.Drawing.Color.White;
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.DGV.Location = new System.Drawing.Point(3, 31);
            this.DGV.Name = "DGV";
            this.DGV.RowTemplate.Height = 23;
            this.DGV.Size = new System.Drawing.Size(317, 157);
            this.DGV.TabIndex = 0;
            // 
            // CBEditType
            // 
            this.CBEditType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBEditType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBEditType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CBEditType.FormattingEnabled = true;
            this.CBEditType.Items.AddRange(new object[] {
            "表格输入",
            "批量输入"});
            this.CBEditType.Location = new System.Drawing.Point(184, 5);
            this.CBEditType.Name = "CBEditType";
            this.CBEditType.Size = new System.Drawing.Size(121, 20);
            this.CBEditType.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全选ToolStripMenuItem,
            this.复制ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.全选ToolStripMenuItem.Text = "全选";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // PbAddParams
            // 
            this.PbAddParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PbAddParams.Location = new System.Drawing.Point(96, 3);
            this.PbAddParams.Name = "PbAddParams";
            this.PbAddParams.Size = new System.Drawing.Size(28, 25);
            this.PbAddParams.TabIndex = 2;
            this.PbAddParams.TabStop = false;
            // 
            // PbRemParams
            // 
            this.PbRemParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PbRemParams.Location = new System.Drawing.Point(124, 3);
            this.PbRemParams.Name = "PbRemParams";
            this.PbRemParams.Size = new System.Drawing.Size(28, 25);
            this.PbRemParams.TabIndex = 3;
            this.PbRemParams.TabStop = false;
            // 
            // PBHelp
            // 
            this.PBHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PBHelp.Location = new System.Drawing.Point(152, 3);
            this.PBHelp.Name = "PBHelp";
            this.PBHelp.Size = new System.Drawing.Size(28, 25);
            this.PBHelp.TabIndex = 4;
            this.PBHelp.TabStop = false;
            // 
            // UCParamsTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PBHelp);
            this.Controls.Add(this.PbRemParams);
            this.Controls.Add(this.PbAddParams);
            this.Controls.Add(this.CBEditType);
            this.Controls.Add(this.DGV);
            this.Name = "UCParamsTable";
            this.Size = new System.Drawing.Size(323, 191);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbAddParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbRemParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBHelp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ComboBox CBEditType;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.PictureBox PbAddParams;
        private System.Windows.Forms.PictureBox PbRemParams;
        private System.Windows.Forms.PictureBox PBHelp;
    }
}
