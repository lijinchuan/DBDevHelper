namespace APIHelper.UC
{
    partial class UCSimulateResponse
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
            this.ucParamsTable1 = new APIHelper.UC.UCParamsTable();
            this.GBResponseHeader = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxEx1 = new APIHelper.UC.TextBoxEx();
            this.BtnSaveFile = new System.Windows.Forms.Button();
            this.GBResponseHeader.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucParamsTable1
            // 
            this.ucParamsTable1.CanUpload = false;
            this.ucParamsTable1.DataSource = null;
            this.ucParamsTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucParamsTable1.Location = new System.Drawing.Point(3, 17);
            this.ucParamsTable1.Name = "ucParamsTable1";
            this.ucParamsTable1.Size = new System.Drawing.Size(479, 125);
            this.ucParamsTable1.TabIndex = 1;
            // 
            // GBResponseHeader
            // 
            this.GBResponseHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBResponseHeader.Controls.Add(this.ucParamsTable1);
            this.GBResponseHeader.Location = new System.Drawing.Point(3, 8);
            this.GBResponseHeader.Name = "GBResponseHeader";
            this.GBResponseHeader.Size = new System.Drawing.Size(485, 145);
            this.GBResponseHeader.TabIndex = 2;
            this.GBResponseHeader.TabStop = false;
            this.GBResponseHeader.Text = "响应头";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BtnSaveFile);
            this.groupBox1.Controls.Add(this.textBoxEx1);
            this.groupBox1.Location = new System.Drawing.Point(3, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 224);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "响应内容";
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEx1.Location = new System.Drawing.Point(50, 20);
            this.textBoxEx1.Multiline = true;
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Size = new System.Drawing.Size(426, 198);
            this.textBoxEx1.TabIndex = 0;
            // 
            // BtnSaveFile
            // 
            this.BtnSaveFile.Location = new System.Drawing.Point(6, 20);
            this.BtnSaveFile.Name = "BtnSaveFile";
            this.BtnSaveFile.Size = new System.Drawing.Size(38, 72);
            this.BtnSaveFile.TabIndex = 1;
            this.BtnSaveFile.Text = "响应文件";
            this.BtnSaveFile.UseVisualStyleBackColor = true;
            // 
            // UCSimulateResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GBResponseHeader);
            this.Name = "UCSimulateResponse";
            this.Size = new System.Drawing.Size(491, 386);
            this.GBResponseHeader.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UCParamsTable ucParamsTable1;
        private System.Windows.Forms.GroupBox GBResponseHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private TextBoxEx textBoxEx1;
        private System.Windows.Forms.Button BtnSaveFile;
    }
}
