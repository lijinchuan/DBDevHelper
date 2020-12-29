namespace APIHelper.UC
{
    partial class UCApiResult
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
            this.TBResult = new System.Windows.Forms.RichTextBox();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TPBody = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RBFormat = new System.Windows.Forms.RadioButton();
            this.RBRow = new System.Windows.Forms.RadioButton();
            this.CBEncode = new System.Windows.Forms.ComboBox();
            this.TPHeader = new System.Windows.Forms.TabPage();
            this.DGVHeader = new System.Windows.Forms.DataGridView();
            this.TPCookie = new System.Windows.Forms.TabPage();
            this.DGVCookie = new System.Windows.Forms.DataGridView();
            this.LBStatuCode = new System.Windows.Forms.Label();
            this.LBMs = new System.Windows.Forms.Label();
            this.LBSize = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CMSTool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TPErrors = new System.Windows.Forms.TabPage();
            this.TBErrors = new System.Windows.Forms.TextBox();
            this.Tabs.SuspendLayout();
            this.TPBody.SuspendLayout();
            this.panel1.SuspendLayout();
            this.TPHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVHeader)).BeginInit();
            this.TPCookie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCookie)).BeginInit();
            this.panel2.SuspendLayout();
            this.CMSTool.SuspendLayout();
            this.TPErrors.SuspendLayout();
            this.SuspendLayout();
            // 
            // TBResult
            // 
            this.TBResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBResult.Location = new System.Drawing.Point(6, 41);
            this.TBResult.Name = "TBResult";
            this.TBResult.Size = new System.Drawing.Size(455, 279);
            this.TBResult.TabIndex = 0;
            this.TBResult.Text = "";
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.TPBody);
            this.Tabs.Controls.Add(this.TPHeader);
            this.Tabs.Controls.Add(this.TPCookie);
            this.Tabs.Controls.Add(this.TPErrors);
            this.Tabs.Location = new System.Drawing.Point(3, 22);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(475, 357);
            this.Tabs.TabIndex = 1;
            // 
            // TPBody
            // 
            this.TPBody.Controls.Add(this.panel1);
            this.TPBody.Controls.Add(this.CBEncode);
            this.TPBody.Controls.Add(this.TBResult);
            this.TPBody.Location = new System.Drawing.Point(4, 22);
            this.TPBody.Name = "TPBody";
            this.TPBody.Padding = new System.Windows.Forms.Padding(3);
            this.TPBody.Size = new System.Drawing.Size(467, 331);
            this.TPBody.TabIndex = 0;
            this.TPBody.Text = "body";
            this.TPBody.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RBFormat);
            this.panel1.Controls.Add(this.RBRow);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 29);
            this.panel1.TabIndex = 4;
            // 
            // RBFormat
            // 
            this.RBFormat.AutoSize = true;
            this.RBFormat.Checked = true;
            this.RBFormat.Location = new System.Drawing.Point(5, 7);
            this.RBFormat.Name = "RBFormat";
            this.RBFormat.Size = new System.Drawing.Size(59, 16);
            this.RBFormat.TabIndex = 1;
            this.RBFormat.TabStop = true;
            this.RBFormat.Text = "格式化";
            this.RBFormat.UseVisualStyleBackColor = true;
            // 
            // RBRow
            // 
            this.RBRow.AutoSize = true;
            this.RBRow.Location = new System.Drawing.Point(75, 7);
            this.RBRow.Name = "RBRow";
            this.RBRow.Size = new System.Drawing.Size(71, 16);
            this.RBRow.TabIndex = 2;
            this.RBRow.Text = "原始数据";
            this.RBRow.UseVisualStyleBackColor = true;
            // 
            // CBEncode
            // 
            this.CBEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBEncode.FormattingEnabled = true;
            this.CBEncode.Location = new System.Drawing.Point(336, 11);
            this.CBEncode.Name = "CBEncode";
            this.CBEncode.Size = new System.Drawing.Size(121, 20);
            this.CBEncode.TabIndex = 3;
            // 
            // TPHeader
            // 
            this.TPHeader.Controls.Add(this.DGVHeader);
            this.TPHeader.Location = new System.Drawing.Point(4, 22);
            this.TPHeader.Name = "TPHeader";
            this.TPHeader.Padding = new System.Windows.Forms.Padding(3);
            this.TPHeader.Size = new System.Drawing.Size(467, 331);
            this.TPHeader.TabIndex = 1;
            this.TPHeader.Text = "header";
            this.TPHeader.UseVisualStyleBackColor = true;
            // 
            // DGVHeader
            // 
            this.DGVHeader.BackgroundColor = System.Drawing.Color.White;
            this.DGVHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVHeader.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.DGVHeader.Location = new System.Drawing.Point(3, 3);
            this.DGVHeader.Name = "DGVHeader";
            this.DGVHeader.RowTemplate.Height = 23;
            this.DGVHeader.Size = new System.Drawing.Size(461, 325);
            this.DGVHeader.TabIndex = 0;
            // 
            // TPCookie
            // 
            this.TPCookie.Controls.Add(this.DGVCookie);
            this.TPCookie.Location = new System.Drawing.Point(4, 22);
            this.TPCookie.Name = "TPCookie";
            this.TPCookie.Size = new System.Drawing.Size(467, 331);
            this.TPCookie.TabIndex = 2;
            this.TPCookie.Text = "Cookie";
            this.TPCookie.UseVisualStyleBackColor = true;
            // 
            // DGVCookie
            // 
            this.DGVCookie.BackgroundColor = System.Drawing.Color.White;
            this.DGVCookie.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVCookie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVCookie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVCookie.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.DGVCookie.Location = new System.Drawing.Point(0, 0);
            this.DGVCookie.Name = "DGVCookie";
            this.DGVCookie.RowTemplate.Height = 23;
            this.DGVCookie.Size = new System.Drawing.Size(467, 331);
            this.DGVCookie.TabIndex = 1;
            // 
            // LBStatuCode
            // 
            this.LBStatuCode.AutoSize = true;
            this.LBStatuCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LBStatuCode.Location = new System.Drawing.Point(3, 4);
            this.LBStatuCode.Name = "LBStatuCode";
            this.LBStatuCode.Size = new System.Drawing.Size(23, 12);
            this.LBStatuCode.TabIndex = 2;
            this.LBStatuCode.Text = "200";
            // 
            // LBMs
            // 
            this.LBMs.AutoSize = true;
            this.LBMs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.LBMs.Location = new System.Drawing.Point(65, 4);
            this.LBMs.Name = "LBMs";
            this.LBMs.Size = new System.Drawing.Size(23, 12);
            this.LBMs.TabIndex = 3;
            this.LBMs.Text = "0ms";
            // 
            // LBSize
            // 
            this.LBSize.AutoSize = true;
            this.LBSize.ForeColor = System.Drawing.Color.Blue;
            this.LBSize.Location = new System.Drawing.Point(128, 3);
            this.LBSize.Name = "LBSize";
            this.LBSize.Size = new System.Drawing.Size(17, 12);
            this.LBSize.TabIndex = 4;
            this.LBSize.Text = "0B";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.LBStatuCode);
            this.panel2.Controls.Add(this.LBSize);
            this.panel2.Controls.Add(this.LBMs);
            this.panel2.Location = new System.Drawing.Point(274, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 19);
            this.panel2.TabIndex = 5;
            // 
            // CMSTool
            // 
            this.CMSTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.查找ToolStripMenuItem});
            this.CMSTool.Name = "CMSTool";
            this.CMSTool.Size = new System.Drawing.Size(101, 48);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.查找ToolStripMenuItem.Text = "查找";
            // 
            // TPErrors
            // 
            this.TPErrors.Controls.Add(this.TBErrors);
            this.TPErrors.Location = new System.Drawing.Point(4, 22);
            this.TPErrors.Name = "TPErrors";
            this.TPErrors.Padding = new System.Windows.Forms.Padding(3);
            this.TPErrors.Size = new System.Drawing.Size(467, 331);
            this.TPErrors.TabIndex = 3;
            this.TPErrors.Text = "错误";
            this.TPErrors.UseVisualStyleBackColor = true;
            // 
            // TBErrors
            // 
            this.TBErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBErrors.Location = new System.Drawing.Point(3, 3);
            this.TBErrors.Multiline = true;
            this.TBErrors.Name = "TBErrors";
            this.TBErrors.ReadOnly = true;
            this.TBErrors.Size = new System.Drawing.Size(461, 325);
            this.TBErrors.TabIndex = 0;
            // 
            // UCApiResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Tabs);
            this.Name = "UCApiResult";
            this.Size = new System.Drawing.Size(481, 382);
            this.Tabs.ResumeLayout(false);
            this.TPBody.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.TPHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVHeader)).EndInit();
            this.TPCookie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVCookie)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.CMSTool.ResumeLayout(false);
            this.TPErrors.ResumeLayout(false);
            this.TPErrors.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox TBResult;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TPBody;
        private System.Windows.Forms.TabPage TPHeader;
        private System.Windows.Forms.TabPage TPCookie;
        private System.Windows.Forms.RadioButton RBRow;
        private System.Windows.Forms.RadioButton RBFormat;
        private System.Windows.Forms.ComboBox CBEncode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView DGVHeader;
        private System.Windows.Forms.DataGridView DGVCookie;
        private System.Windows.Forms.Label LBStatuCode;
        private System.Windows.Forms.Label LBMs;
        private System.Windows.Forms.Label LBSize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip CMSTool;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查找ToolStripMenuItem;
        private System.Windows.Forms.TabPage TPErrors;
        private System.Windows.Forms.TextBox TBErrors;
    }
}
