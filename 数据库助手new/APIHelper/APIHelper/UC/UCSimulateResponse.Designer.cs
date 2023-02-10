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
            this.GBResponseHeader = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.CBResponseContentType = new System.Windows.Forms.ComboBox();
            this.CBCharset = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBContentType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LBHost = new System.Windows.Forms.Label();
            this.TBSimulateUrl = new System.Windows.Forms.TextBox();
            this.TBContent = new APIHelper.UC.TextBoxEx();
            this.UCParams = new APIHelper.UC.UCParamsTable();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.GBResponseHeader.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBResponseHeader
            // 
            this.GBResponseHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBResponseHeader.Controls.Add(this.UCParams);
            this.GBResponseHeader.Location = new System.Drawing.Point(3, 39);
            this.GBResponseHeader.Name = "GBResponseHeader";
            this.GBResponseHeader.Size = new System.Drawing.Size(485, 150);
            this.GBResponseHeader.TabIndex = 2;
            this.GBResponseHeader.TabStop = false;
            this.GBResponseHeader.Text = "响应头";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BtnSave);
            this.groupBox1.Controls.Add(this.CBResponseContentType);
            this.groupBox1.Controls.Add(this.CBCharset);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CBContentType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TBContent);
            this.groupBox1.Location = new System.Drawing.Point(3, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 188);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "响应内容";
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(399, 160);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 7;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // CBResponseContentType
            // 
            this.CBResponseContentType.FormattingEnabled = true;
            this.CBResponseContentType.Location = new System.Drawing.Point(11, 46);
            this.CBResponseContentType.Name = "CBResponseContentType";
            this.CBResponseContentType.Size = new System.Drawing.Size(82, 20);
            this.CBResponseContentType.TabIndex = 6;
            // 
            // CBCharset
            // 
            this.CBCharset.FormattingEnabled = true;
            this.CBCharset.Location = new System.Drawing.Point(351, 18);
            this.CBCharset.Name = "CBCharset";
            this.CBCharset.Size = new System.Drawing.Size(121, 20);
            this.CBCharset.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "charset:";
            // 
            // CBContentType
            // 
            this.CBContentType.FormattingEnabled = true;
            this.CBContentType.Location = new System.Drawing.Point(99, 18);
            this.CBContentType.Name = "CBContentType";
            this.CBContentType.Size = new System.Drawing.Size(187, 20);
            this.CBContentType.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Content-Type：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "模拟地址：";
            // 
            // LBHost
            // 
            this.LBHost.AutoSize = true;
            this.LBHost.Location = new System.Drawing.Point(65, 13);
            this.LBHost.Name = "LBHost";
            this.LBHost.Size = new System.Drawing.Size(107, 12);
            this.LBHost.TabIndex = 5;
            this.LBHost.Text = "http://localhost:";
            // 
            // TBSimulateUrl
            // 
            this.TBSimulateUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBSimulateUrl.Location = new System.Drawing.Point(175, 9);
            this.TBSimulateUrl.Name = "TBSimulateUrl";
            this.TBSimulateUrl.Size = new System.Drawing.Size(230, 21);
            this.TBSimulateUrl.TabIndex = 6;
            // 
            // TBContent
            // 
            this.TBContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBContent.Location = new System.Drawing.Point(99, 44);
            this.TBContent.Multiline = true;
            this.TBContent.Name = "TBContent";
            this.TBContent.Size = new System.Drawing.Size(377, 112);
            this.TBContent.TabIndex = 0;
            // 
            // UCParams
            // 
            this.UCParams.CanUpload = false;
            this.UCParams.DataSource = null;
            this.UCParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UCParams.Location = new System.Drawing.Point(3, 17);
            this.UCParams.Name = "UCParams";
            this.UCParams.Size = new System.Drawing.Size(479, 130);
            this.UCParams.TabIndex = 1;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(410, 13);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(29, 12);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "复制";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // UCSimulateResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.TBSimulateUrl);
            this.Controls.Add(this.LBHost);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GBResponseHeader);
            this.Name = "UCSimulateResponse";
            this.Size = new System.Drawing.Size(491, 386);
            this.GBResponseHeader.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCParamsTable UCParams;
        private System.Windows.Forms.GroupBox GBResponseHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private TextBoxEx TBContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBContentType;
        private System.Windows.Forms.ComboBox CBCharset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CBResponseContentType;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LBHost;
        private System.Windows.Forms.TextBox TBSimulateUrl;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
