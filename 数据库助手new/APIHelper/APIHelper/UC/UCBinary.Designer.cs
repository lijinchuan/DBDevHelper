namespace APIHelper.UC
{
    partial class UCBinary
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
            this.BtnSelFile = new System.Windows.Forms.Button();
            this.ListFiles = new System.Windows.Forms.ListBox();
            this.BtnDel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnSelFile
            // 
            this.BtnSelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSelFile.Location = new System.Drawing.Point(321, 302);
            this.BtnSelFile.Name = "BtnSelFile";
            this.BtnSelFile.Size = new System.Drawing.Size(75, 23);
            this.BtnSelFile.TabIndex = 0;
            this.BtnSelFile.Text = "选择文件";
            this.BtnSelFile.UseVisualStyleBackColor = true;
            this.BtnSelFile.Click += new System.EventHandler(this.BtnSelFile_Click);
            // 
            // ListFiles
            // 
            this.ListFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListFiles.FormattingEnabled = true;
            this.ListFiles.ItemHeight = 12;
            this.ListFiles.Location = new System.Drawing.Point(30, 13);
            this.ListFiles.Name = "ListFiles";
            this.ListFiles.Size = new System.Drawing.Size(470, 268);
            this.ListFiles.TabIndex = 1;
            // 
            // BtnDel
            // 
            this.BtnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDel.Location = new System.Drawing.Point(414, 302);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(75, 23);
            this.BtnDel.TabIndex = 2;
            this.BtnDel.Text = "删除文件";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // UCBinary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnDel);
            this.Controls.Add(this.ListFiles);
            this.Controls.Add(this.BtnSelFile);
            this.Name = "UCBinary";
            this.Size = new System.Drawing.Size(522, 339);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnSelFile;
        private System.Windows.Forms.ListBox ListFiles;
        private System.Windows.Forms.Button BtnDel;
    }
}
