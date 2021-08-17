namespace NETDBHelper.SubForm
{
    partial class DBFilter
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GBSelDBTB = new System.Windows.Forms.GroupBox();
            this.CLBTBS = new System.Windows.Forms.CheckedListBox();
            this.CLBDBs = new System.Windows.Forms.CheckedListBox();
            this.BtnSelectServer = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSelAll = new System.Windows.Forms.Button();
            this.CBIsTemp = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.GBSelDBTB.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CBIsTemp);
            this.groupBox2.Location = new System.Drawing.Point(552, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 337);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // GBSelDBTB
            // 
            this.GBSelDBTB.Controls.Add(this.CLBTBS);
            this.GBSelDBTB.Controls.Add(this.CLBDBs);
            this.GBSelDBTB.Location = new System.Drawing.Point(28, 54);
            this.GBSelDBTB.Name = "GBSelDBTB";
            this.GBSelDBTB.Size = new System.Drawing.Size(518, 337);
            this.GBSelDBTB.TabIndex = 23;
            this.GBSelDBTB.TabStop = false;
            this.GBSelDBTB.Text = "选择库表";
            // 
            // CLBTBS
            // 
            this.CLBTBS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CLBTBS.FormattingEnabled = true;
            this.CLBTBS.Location = new System.Drawing.Point(228, 17);
            this.CLBTBS.Name = "CLBTBS";
            this.CLBTBS.Size = new System.Drawing.Size(284, 308);
            this.CLBTBS.TabIndex = 1;
            this.CLBTBS.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CLBTBS_ItemCheck);
            this.CLBTBS.Click += new System.EventHandler(this.CLBTBS_Click);
            // 
            // CLBDBs
            // 
            this.CLBDBs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CLBDBs.FormattingEnabled = true;
            this.CLBDBs.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.CLBDBs.Location = new System.Drawing.Point(3, 17);
            this.CLBDBs.Name = "CLBDBs";
            this.CLBDBs.Size = new System.Drawing.Size(219, 308);
            this.CLBDBs.TabIndex = 0;
            this.CLBDBs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CLBDBs_ItemCheck);
            this.CLBDBs.Click += new System.EventHandler(this.CLBDBs_Click);
            this.CLBDBs.SelectedIndexChanged += new System.EventHandler(this.CLBDBs_SelectedIndexChanged);
            // 
            // BtnSelectServer
            // 
            this.BtnSelectServer.Location = new System.Drawing.Point(117, 18);
            this.BtnSelectServer.Name = "BtnSelectServer";
            this.BtnSelectServer.Size = new System.Drawing.Size(75, 23);
            this.BtnSelectServer.TabIndex = 22;
            this.BtnSelectServer.Text = "服务器";
            this.BtnSelectServer.UseVisualStyleBackColor = true;
            this.BtnSelectServer.Click += new System.EventHandler(this.BtnSelectServer_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "服务器：";
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(622, 408);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 25;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(713, 408);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 26;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSelAll
            // 
            this.BtnSelAll.Location = new System.Drawing.Point(47, 408);
            this.BtnSelAll.Name = "BtnSelAll";
            this.BtnSelAll.Size = new System.Drawing.Size(78, 23);
            this.BtnSelAll.TabIndex = 27;
            this.BtnSelAll.Text = "全选";
            this.BtnSelAll.UseVisualStyleBackColor = true;
            this.BtnSelAll.Click += new System.EventHandler(this.BtnSelAll_Click);
            // 
            // CBIsTemp
            // 
            this.CBIsTemp.AutoSize = true;
            this.CBIsTemp.Checked = true;
            this.CBIsTemp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBIsTemp.Location = new System.Drawing.Point(16, 20);
            this.CBIsTemp.Name = "CBIsTemp";
            this.CBIsTemp.Size = new System.Drawing.Size(48, 16);
            this.CBIsTemp.TabIndex = 0;
            this.CBIsTemp.Text = "临时";
            this.CBIsTemp.UseVisualStyleBackColor = true;
            // 
            // DBFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 443);
            this.Controls.Add(this.BtnSelAll);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.GBSelDBTB);
            this.Controls.Add(this.BtnSelectServer);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DBFilter";
            this.ShowIcon = false;
            this.Text = "库过滤";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.GBSelDBTB.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox GBSelDBTB;
        private System.Windows.Forms.CheckedListBox CLBTBS;
        private System.Windows.Forms.CheckedListBox CLBDBs;
        private System.Windows.Forms.Button BtnSelectServer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelAll;
        private System.Windows.Forms.CheckBox CBIsTemp;
    }
}