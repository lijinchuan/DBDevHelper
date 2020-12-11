namespace APIHelper.SubForm
{
    partial class AddAPIStep1Dlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.TBAPIName = new System.Windows.Forms.TextBox();
            this.LBReqType = new System.Windows.Forms.Label();
            this.CBWebMethod = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TBUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBDesc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "API名称";
            // 
            // TBAPIName
            // 
            this.TBAPIName.Location = new System.Drawing.Point(112, 12);
            this.TBAPIName.Name = "TBAPIName";
            this.TBAPIName.Size = new System.Drawing.Size(324, 21);
            this.TBAPIName.TabIndex = 1;
            // 
            // LBReqType
            // 
            this.LBReqType.AutoSize = true;
            this.LBReqType.Location = new System.Drawing.Point(60, 54);
            this.LBReqType.Name = "LBReqType";
            this.LBReqType.Size = new System.Drawing.Size(29, 12);
            this.LBReqType.TabIndex = 2;
            this.LBReqType.Text = "类型";
            // 
            // CBWebMethod
            // 
            this.CBWebMethod.FormattingEnabled = true;
            this.CBWebMethod.Location = new System.Drawing.Point(112, 51);
            this.CBWebMethod.Name = "CBWebMethod";
            this.CBWebMethod.Size = new System.Drawing.Size(95, 20);
            this.CBWebMethod.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-3, 246);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(479, 10);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(284, 270);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 5;
            this.BtnOK.Text = "确定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(380, 269);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "地址";
            // 
            // TBUrl
            // 
            this.TBUrl.Location = new System.Drawing.Point(112, 91);
            this.TBUrl.Multiline = true;
            this.TBUrl.Name = "TBUrl";
            this.TBUrl.Size = new System.Drawing.Size(324, 63);
            this.TBUrl.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "说明";
            // 
            // TBDesc
            // 
            this.TBDesc.Location = new System.Drawing.Point(112, 177);
            this.TBDesc.Multiline = true;
            this.TBDesc.Name = "TBDesc";
            this.TBDesc.Size = new System.Drawing.Size(324, 63);
            this.TBDesc.TabIndex = 10;
            // 
            // AddAPIStep1Dlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 304);
            this.Controls.Add(this.TBDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CBWebMethod);
            this.Controls.Add(this.LBReqType);
            this.Controls.Add(this.TBAPIName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAPIStep1Dlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加API资源";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBAPIName;
        private System.Windows.Forms.Label LBReqType;
        private System.Windows.Forms.ComboBox CBWebMethod;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBDesc;
    }
}