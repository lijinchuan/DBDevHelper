namespace NETDBHelper.SubForm
{
    partial class CopyDB
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CLBDBs = new System.Windows.Forms.CheckedListBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSelAll = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NUDMaxSize = new System.Windows.Forms.NumericUpDown();
            this.CBTrigger = new System.Windows.Forms.CheckBox();
            this.CBIndex = new System.Windows.Forms.CheckBox();
            this.CBView = new System.Windows.Forms.CheckBox();
            this.CBFunc = new System.Windows.Forms.CheckBox();
            this.CBProc = new System.Windows.Forms.CheckBox();
            this.CBTest = new System.Windows.Forms.CheckBox();
            this.PannelCopNumber = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.CBIgnoreError = new System.Windows.Forms.CheckBox();
            this.NUDMaxNumber = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.CBData = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProcessBar = new System.Windows.Forms.ToolStripProgressBar();
            this.MsgText = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDMaxSize)).BeginInit();
            this.PannelCopNumber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDMaxNumber)).BeginInit();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CLBDBs);
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 337);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择库";
            // 
            // CLBDBs
            // 
            this.CLBDBs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CLBDBs.FormattingEnabled = true;
            this.CLBDBs.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.CLBDBs.Location = new System.Drawing.Point(3, 17);
            this.CLBDBs.Name = "CLBDBs";
            this.CLBDBs.Size = new System.Drawing.Size(512, 317);
            this.CLBDBs.TabIndex = 0;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(613, 404);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 17;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(710, 404);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 18;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSelAll
            // 
            this.BtnSelAll.Location = new System.Drawing.Point(15, 404);
            this.BtnSelAll.Name = "BtnSelAll";
            this.BtnSelAll.Size = new System.Drawing.Size(78, 23);
            this.BtnSelAll.TabIndex = 19;
            this.BtnSelAll.Text = "全选";
            this.BtnSelAll.UseVisualStyleBackColor = true;
            this.BtnSelAll.Click += new System.EventHandler(this.BtnSelAll_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.NUDMaxSize);
            this.groupBox2.Controls.Add(this.CBTrigger);
            this.groupBox2.Controls.Add(this.CBIndex);
            this.groupBox2.Controls.Add(this.CBView);
            this.groupBox2.Controls.Add(this.CBFunc);
            this.groupBox2.Controls.Add(this.CBProc);
            this.groupBox2.Controls.Add(this.CBTest);
            this.groupBox2.Controls.Add(this.PannelCopNumber);
            this.groupBox2.Controls.Add(this.CBData);
            this.groupBox2.Location = new System.Drawing.Point(536, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 337);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "单个文件大小(M)：";
            // 
            // NUDMaxSize
            // 
            this.NUDMaxSize.Location = new System.Drawing.Point(142, 210);
            this.NUDMaxSize.Name = "NUDMaxSize";
            this.NUDMaxSize.Size = new System.Drawing.Size(79, 21);
            this.NUDMaxSize.TabIndex = 8;
            this.NUDMaxSize.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // CBTrigger
            // 
            this.CBTrigger.AutoSize = true;
            this.CBTrigger.Checked = true;
            this.CBTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBTrigger.Location = new System.Drawing.Point(142, 143);
            this.CBTrigger.Name = "CBTrigger";
            this.CBTrigger.Size = new System.Drawing.Size(60, 16);
            this.CBTrigger.TabIndex = 7;
            this.CBTrigger.Text = "触发器";
            this.CBTrigger.UseVisualStyleBackColor = true;
            // 
            // CBIndex
            // 
            this.CBIndex.AutoSize = true;
            this.CBIndex.Checked = true;
            this.CBIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBIndex.Location = new System.Drawing.Point(142, 118);
            this.CBIndex.Name = "CBIndex";
            this.CBIndex.Size = new System.Drawing.Size(48, 16);
            this.CBIndex.TabIndex = 6;
            this.CBIndex.Text = "索引";
            this.CBIndex.UseVisualStyleBackColor = true;
            // 
            // CBView
            // 
            this.CBView.AutoSize = true;
            this.CBView.Checked = true;
            this.CBView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBView.Location = new System.Drawing.Point(24, 118);
            this.CBView.Name = "CBView";
            this.CBView.Size = new System.Drawing.Size(48, 16);
            this.CBView.TabIndex = 5;
            this.CBView.Text = "视图";
            this.CBView.UseVisualStyleBackColor = true;
            // 
            // CBFunc
            // 
            this.CBFunc.AutoSize = true;
            this.CBFunc.Checked = true;
            this.CBFunc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBFunc.Location = new System.Drawing.Point(25, 166);
            this.CBFunc.Name = "CBFunc";
            this.CBFunc.Size = new System.Drawing.Size(48, 16);
            this.CBFunc.TabIndex = 4;
            this.CBFunc.Text = "函数";
            this.CBFunc.UseVisualStyleBackColor = true;
            // 
            // CBProc
            // 
            this.CBProc.AutoSize = true;
            this.CBProc.Checked = true;
            this.CBProc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBProc.Location = new System.Drawing.Point(24, 143);
            this.CBProc.Name = "CBProc";
            this.CBProc.Size = new System.Drawing.Size(72, 16);
            this.CBProc.TabIndex = 3;
            this.CBProc.Text = "存储过程";
            this.CBProc.UseVisualStyleBackColor = true;
            // 
            // CBTest
            // 
            this.CBTest.AutoSize = true;
            this.CBTest.Location = new System.Drawing.Point(25, 315);
            this.CBTest.Name = "CBTest";
            this.CBTest.Size = new System.Drawing.Size(156, 16);
            this.CBTest.TabIndex = 2;
            this.CBTest.Text = "测试(库会加时间重命名)";
            this.CBTest.UseVisualStyleBackColor = true;
            // 
            // PannelCopNumber
            // 
            this.PannelCopNumber.Controls.Add(this.label2);
            this.PannelCopNumber.Controls.Add(this.CBIgnoreError);
            this.PannelCopNumber.Controls.Add(this.NUDMaxNumber);
            this.PannelCopNumber.Controls.Add(this.label1);
            this.PannelCopNumber.Enabled = false;
            this.PannelCopNumber.Location = new System.Drawing.Point(24, 51);
            this.PannelCopNumber.Name = "PannelCopNumber";
            this.PannelCopNumber.Size = new System.Drawing.Size(222, 58);
            this.PannelCopNumber.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "忽略错误：";
            // 
            // CBIgnoreError
            // 
            this.CBIgnoreError.AutoSize = true;
            this.CBIgnoreError.Location = new System.Drawing.Point(80, 34);
            this.CBIgnoreError.Name = "CBIgnoreError";
            this.CBIgnoreError.Size = new System.Drawing.Size(15, 14);
            this.CBIgnoreError.TabIndex = 3;
            this.CBIgnoreError.UseVisualStyleBackColor = true;
            // 
            // NUDMaxNumber
            // 
            this.NUDMaxNumber.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUDMaxNumber.Location = new System.Drawing.Point(76, 5);
            this.NUDMaxNumber.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NUDMaxNumber.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUDMaxNumber.Name = "NUDMaxNumber";
            this.NUDMaxNumber.Size = new System.Drawing.Size(143, 21);
            this.NUDMaxNumber.TabIndex = 2;
            this.NUDMaxNumber.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "最多数据：";
            // 
            // CBData
            // 
            this.CBData.AutoSize = true;
            this.CBData.Location = new System.Drawing.Point(24, 29);
            this.CBData.Name = "CBData";
            this.CBData.Size = new System.Drawing.Size(72, 16);
            this.CBData.TabIndex = 0;
            this.CBData.Text = "复制数据";
            this.CBData.UseVisualStyleBackColor = true;
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
            // CopyDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BtnSelAll);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnSelectServer);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyDB";
            this.ShowInTaskbar = false;
            this.Text = "备份数据库";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDMaxSize)).EndInit();
            this.PannelCopNumber.ResumeLayout(false);
            this.PannelCopNumber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDMaxNumber)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSelectServer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox CLBDBs;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox CBData;
        private System.Windows.Forms.Panel PannelCopNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NUDMaxNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CBIgnoreError;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProcessBar;
        private System.Windows.Forms.ToolStripStatusLabel MsgText;
        private System.Windows.Forms.CheckBox CBTest;
        private System.Windows.Forms.CheckBox CBProc;
        private System.Windows.Forms.CheckBox CBFunc;
        private System.Windows.Forms.CheckBox CBView;
        private System.Windows.Forms.CheckBox CBIndex;
        private System.Windows.Forms.CheckBox CBTrigger;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NUDMaxSize;
    }
}