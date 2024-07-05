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
            this.GPResponseContent = new System.Windows.Forms.GroupBox();
            this.TBCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CBDef = new System.Windows.Forms.CheckBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.CBResponseContentType = new System.Windows.Forms.ComboBox();
            this.CBCharset = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBContentType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LBHost = new System.Windows.Forms.Label();
            this.TBSimulateUrl = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.CBTag = new System.Windows.Forms.ComboBox();
            this.TBContent = new APIHelper.UC.TextBoxEx();
            this.UCParams = new APIHelper.UC.UCParamsTable();
            this.GBResponseHeader.SuspendLayout();
            this.GPResponseContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBResponseHeader
            // 
            this.GBResponseHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBResponseHeader.Controls.Add(this.UCParams);
            this.GBResponseHeader.Location = new System.Drawing.Point(4, 49);
            this.GBResponseHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GBResponseHeader.Name = "GBResponseHeader";
            this.GBResponseHeader.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GBResponseHeader.Size = new System.Drawing.Size(832, 188);
            this.GBResponseHeader.TabIndex = 2;
            this.GBResponseHeader.TabStop = false;
            this.GBResponseHeader.Text = "响应头";
            // 
            // GPResponseContent
            // 
            this.GPResponseContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GPResponseContent.Controls.Add(this.TBCode);
            this.GPResponseContent.Controls.Add(this.label5);
            this.GPResponseContent.Controls.Add(this.CBDef);
            this.GPResponseContent.Controls.Add(this.BtnSave);
            this.GPResponseContent.Controls.Add(this.CBResponseContentType);
            this.GPResponseContent.Controls.Add(this.CBCharset);
            this.GPResponseContent.Controls.Add(this.label2);
            this.GPResponseContent.Controls.Add(this.CBContentType);
            this.GPResponseContent.Controls.Add(this.label1);
            this.GPResponseContent.Controls.Add(this.TBContent);
            this.GPResponseContent.Location = new System.Drawing.Point(4, 244);
            this.GPResponseContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GPResponseContent.Name = "GPResponseContent";
            this.GPResponseContent.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GPResponseContent.Size = new System.Drawing.Size(828, 235);
            this.GPResponseContent.TabIndex = 3;
            this.GPResponseContent.TabStop = false;
            this.GPResponseContent.Text = "响应内容";
            // 
            // TBCode
            // 
            this.TBCode.Location = new System.Drawing.Point(731, 21);
            this.TBCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TBCode.Name = "TBCode";
            this.TBCode.Size = new System.Drawing.Size(85, 25);
            this.TBCode.TabIndex = 10;
            this.TBCode.Text = "200";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(644, 28);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "响应代码:";
            // 
            // CBDef
            // 
            this.CBDef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CBDef.AutoSize = true;
            this.CBDef.Location = new System.Drawing.Point(132, 207);
            this.CBDef.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CBDef.Name = "CBDef";
            this.CBDef.Size = new System.Drawing.Size(59, 19);
            this.CBDef.TabIndex = 8;
            this.CBDef.Text = "默认";
            this.CBDef.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(717, 200);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(100, 29);
            this.BtnSave.TabIndex = 7;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // CBResponseContentType
            // 
            this.CBResponseContentType.FormattingEnabled = true;
            this.CBResponseContentType.Location = new System.Drawing.Point(15, 58);
            this.CBResponseContentType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CBResponseContentType.Name = "CBResponseContentType";
            this.CBResponseContentType.Size = new System.Drawing.Size(108, 23);
            this.CBResponseContentType.TabIndex = 6;
            // 
            // CBCharset
            // 
            this.CBCharset.FormattingEnabled = true;
            this.CBCharset.Location = new System.Drawing.Point(468, 22);
            this.CBCharset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CBCharset.Name = "CBCharset";
            this.CBCharset.Size = new System.Drawing.Size(160, 23);
            this.CBCharset.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(389, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "charset:";
            // 
            // CBContentType
            // 
            this.CBContentType.FormattingEnabled = true;
            this.CBContentType.Location = new System.Drawing.Point(132, 22);
            this.CBContentType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CBContentType.Name = "CBContentType";
            this.CBContentType.Size = new System.Drawing.Size(248, 23);
            this.CBContentType.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Content-Type：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "模拟地址：";
            // 
            // LBHost
            // 
            this.LBHost.AutoSize = true;
            this.LBHost.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LBHost.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LBHost.ForeColor = System.Drawing.Color.Blue;
            this.LBHost.Location = new System.Drawing.Point(323, 18);
            this.LBHost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LBHost.Name = "LBHost";
            this.LBHost.Size = new System.Drawing.Size(143, 15);
            this.LBHost.TabIndex = 5;
            this.LBHost.Text = "http://localhost:";
            this.LBHost.Click += new System.EventHandler(this.LBHost_Click);
            // 
            // TBSimulateUrl
            // 
            this.TBSimulateUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBSimulateUrl.Location = new System.Drawing.Point(472, 11);
            this.TBSimulateUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TBSimulateUrl.Name = "TBSimulateUrl";
            this.TBSimulateUrl.Size = new System.Drawing.Size(252, 25);
            this.TBSimulateUrl.TabIndex = 6;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(732, 16);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(37, 15);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "复制";
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "标签：";
            // 
            // CBTag
            // 
            this.CBTag.FormattingEnabled = true;
            this.CBTag.Location = new System.Drawing.Point(67, 12);
            this.CBTag.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CBTag.Name = "CBTag";
            this.CBTag.Size = new System.Drawing.Size(160, 23);
            this.CBTag.TabIndex = 9;
            // 
            // TBContent
            // 
            this.TBContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBContent.Location = new System.Drawing.Point(132, 55);
            this.TBContent.Margin = new System.Windows.Forms.Padding(4);
            this.TBContent.Multiline = true;
            this.TBContent.Name = "TBContent";
            this.TBContent.Size = new System.Drawing.Size(687, 139);
            this.TBContent.TabIndex = 0;
            // 
            // UCParams
            // 
            this.UCParams.CanUpload = false;
            this.UCParams.DataSource = null;
            this.UCParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UCParams.Location = new System.Drawing.Point(4, 22);
            this.UCParams.Margin = new System.Windows.Forms.Padding(5);
            this.UCParams.Name = "UCParams";
            this.UCParams.Size = new System.Drawing.Size(824, 162);
            this.UCParams.TabIndex = 1;
            // 
            // UCSimulateResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CBTag);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.TBSimulateUrl);
            this.Controls.Add(this.LBHost);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.GPResponseContent);
            this.Controls.Add(this.GBResponseHeader);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UCSimulateResponse";
            this.Size = new System.Drawing.Size(840, 482);
            this.GBResponseHeader.ResumeLayout(false);
            this.GPResponseContent.ResumeLayout(false);
            this.GPResponseContent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCParamsTable UCParams;
        private System.Windows.Forms.GroupBox GBResponseHeader;
        private System.Windows.Forms.GroupBox GPResponseContent;
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CBDef;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBCode;
        private System.Windows.Forms.ComboBox CBTag;
    }
}
