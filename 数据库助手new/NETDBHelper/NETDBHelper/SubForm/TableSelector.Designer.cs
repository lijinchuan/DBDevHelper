namespace NETDBHelper.SubForm
{
    partial class TableSelector
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
            this.BtnSelectServer = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.GBSelDBTB = new System.Windows.Forms.GroupBox();
            this.CLBTBS = new System.Windows.Forms.CheckedListBox();
            this.CLBDBs = new System.Windows.Forms.CheckedListBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSelAll = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProcessBar = new System.Windows.Forms.ToolStripProgressBar();
            this.MsgText = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnStop = new System.Windows.Forms.Button();
            this.TBReg = new System.Windows.Forms.TextBox();
            this.GBSelDBTB.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSelectServer
            // 
            this.BtnSelectServer.Location = new System.Drawing.Point(101, 16);
            this.BtnSelectServer.Name = "BtnSelectServer";
            this.BtnSelectServer.Size = new System.Drawing.Size(75, 23);
            this.BtnSelectServer.TabIndex = 15;
            this.BtnSelectServer.Text = "服务器";
            this.BtnSelectServer.UseVisualStyleBackColor = true;
            this.BtnSelectServer.Click += new System.EventHandler(this.BtnSelectServer_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "服务器：";
            // 
            // GBSelDBTB
            // 
            this.GBSelDBTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBSelDBTB.Controls.Add(this.CLBTBS);
            this.GBSelDBTB.Controls.Add(this.CLBDBs);
            this.GBSelDBTB.Location = new System.Drawing.Point(12, 52);
            this.GBSelDBTB.Name = "GBSelDBTB";
            this.GBSelDBTB.Size = new System.Drawing.Size(518, 337);
            this.GBSelDBTB.TabIndex = 16;
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
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(537, 404);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 17;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(710, 403);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 18;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSelAll
            // 
            this.BtnSelAll.Location = new System.Drawing.Point(121, 399);
            this.BtnSelAll.Name = "BtnSelAll";
            this.BtnSelAll.Size = new System.Drawing.Size(78, 23);
            this.BtnSelAll.TabIndex = 19;
            this.BtnSelAll.Text = "全选";
            this.BtnSelAll.UseVisualStyleBackColor = true;
            this.BtnSelAll.Click += new System.EventHandler(this.BtnSelAll_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(536, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 337);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProcessBar,
            this.MsgText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProcessBar
            // 
            this.ProcessBar.Name = "ProcessBar";
            this.ProcessBar.Size = new System.Drawing.Size(100, 16);
            // 
            // MsgText
            // 
            this.MsgText.ForeColor = System.Drawing.Color.Red;
            this.MsgText.Name = "MsgText";
            this.MsgText.Size = new System.Drawing.Size(683, 17);
            this.MsgText.Spring = true;
            this.MsgText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(623, 404);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 22;
            this.BtnStop.Text = "暂停";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // TBReg
            // 
            this.TBReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TBReg.Location = new System.Drawing.Point(15, 401);
            this.TBReg.Name = "TBReg";
            this.TBReg.Size = new System.Drawing.Size(100, 21);
            this.TBReg.TabIndex = 23;
            // 
            // TableSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TBReg);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BtnSelAll);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.GBSelDBTB);
            this.Controls.Add(this.BtnSelectServer);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TableSelector";
            this.Text = "选择库表";
            this.GBSelDBTB.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSelectServer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox GBSelDBTB;
        private System.Windows.Forms.CheckedListBox CLBDBs;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProcessBar;
        private System.Windows.Forms.ToolStripStatusLabel MsgText;
        private System.Windows.Forms.CheckedListBox CLBTBS;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.TextBox TBReg;
    }
}