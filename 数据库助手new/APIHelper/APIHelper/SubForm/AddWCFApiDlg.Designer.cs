namespace APIHelper.SubForm
{
    partial class AddWCFApiDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TBDesc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TBAPIName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LBInterfaceMethod = new System.Windows.Forms.ListBox();
            this.BtnFindService = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TBDesc
            // 
            this.TBDesc.Location = new System.Drawing.Point(79, 266);
            this.TBDesc.Multiline = true;
            this.TBDesc.Name = "TBDesc";
            this.TBDesc.Size = new System.Drawing.Size(588, 63);
            this.TBDesc.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 275);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "说明";
            // 
            // TBUrl
            // 
            this.TBUrl.Location = new System.Drawing.Point(79, 46);
            this.TBUrl.Name = "TBUrl";
            this.TBUrl.Size = new System.Drawing.Size(532, 21);
            this.TBUrl.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "地址";
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(613, 353);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 17;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(517, 354);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 16;
            this.BtnOK.Text = "确定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-36, 335);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 10);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // TBAPIName
            // 
            this.TBAPIName.Location = new System.Drawing.Point(79, 10);
            this.TBAPIName.Name = "TBAPIName";
            this.TBAPIName.Size = new System.Drawing.Size(324, 21);
            this.TBAPIName.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "API名称";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "接口";
            // 
            // LBInterfaceMethod
            // 
            this.LBInterfaceMethod.FormattingEnabled = true;
            this.LBInterfaceMethod.ItemHeight = 12;
            this.LBInterfaceMethod.Location = new System.Drawing.Point(79, 87);
            this.LBInterfaceMethod.Name = "LBInterfaceMethod";
            this.LBInterfaceMethod.Size = new System.Drawing.Size(588, 160);
            this.LBInterfaceMethod.TabIndex = 23;
            // 
            // BtnFindService
            // 
            this.BtnFindService.Location = new System.Drawing.Point(617, 44);
            this.BtnFindService.Name = "BtnFindService";
            this.BtnFindService.Size = new System.Drawing.Size(62, 23);
            this.BtnFindService.TabIndex = 24;
            this.BtnFindService.Text = "发现服务";
            this.BtnFindService.UseVisualStyleBackColor = true;
            this.BtnFindService.Click += new System.EventHandler(this.BtnFindService_Click);
            // 
            // AddWCFApiDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 384);
            this.Controls.Add(this.BtnFindService);
            this.Controls.Add(this.LBInterfaceMethod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TBAPIName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddWCFApiDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加WCF接口";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBDesc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TBAPIName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox LBInterfaceMethod;
        private System.Windows.Forms.Button BtnFindService;
    }
}