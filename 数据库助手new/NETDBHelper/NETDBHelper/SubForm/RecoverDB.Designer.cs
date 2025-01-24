
namespace NETDBHelper.SubForm
{
    partial class RecoverDBDlg
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
            this.BtnChooseDir = new System.Windows.Forms.Button();
            this.TBPath = new System.Windows.Forms.TextBox();
            this.BtnRecover = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProcessBar = new System.Windows.Forms.ToolStripProgressBar();
            this.LBMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBClear = new System.Windows.Forms.CheckBox();
            this.CBIgnoreError = new System.Windows.Forms.CheckBox();
            this.TimeOutMins = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TBS = new System.Windows.Forms.CheckedListBox();
            this.DBS = new System.Windows.Forms.CheckedListBox();
            this.BtnSelAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TBDBReName = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeOutMins)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnChooseDir
            // 
            this.BtnChooseDir.Location = new System.Drawing.Point(817, 11);
            this.BtnChooseDir.Name = "BtnChooseDir";
            this.BtnChooseDir.Size = new System.Drawing.Size(38, 23);
            this.BtnChooseDir.TabIndex = 0;
            this.BtnChooseDir.Text = "...";
            this.BtnChooseDir.UseVisualStyleBackColor = true;
            this.BtnChooseDir.Click += new System.EventHandler(this.BtnChooseDir_Click);
            // 
            // TBPath
            // 
            this.TBPath.Location = new System.Drawing.Point(12, 12);
            this.TBPath.Name = "TBPath";
            this.TBPath.ReadOnly = true;
            this.TBPath.Size = new System.Drawing.Size(799, 21);
            this.TBPath.TabIndex = 1;
            // 
            // BtnRecover
            // 
            this.BtnRecover.ForeColor = System.Drawing.Color.Sienna;
            this.BtnRecover.Location = new System.Drawing.Point(689, 343);
            this.BtnRecover.Name = "BtnRecover";
            this.BtnRecover.Size = new System.Drawing.Size(75, 23);
            this.BtnRecover.TabIndex = 2;
            this.BtnRecover.Text = "还原数据";
            this.BtnRecover.UseVisualStyleBackColor = true;
            this.BtnRecover.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(780, 343);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProcessBar,
            this.LBMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 369);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(870, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProcessBar
            // 
            this.ProcessBar.Name = "ProcessBar";
            this.ProcessBar.Size = new System.Drawing.Size(100, 16);
            // 
            // LBMsg
            // 
            this.LBMsg.ForeColor = System.Drawing.Color.Blue;
            this.LBMsg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LBMsg.Name = "LBMsg";
            this.LBMsg.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.LBMsg.Size = new System.Drawing.Size(753, 17);
            this.LBMsg.Spring = true;
            this.LBMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TBDBReName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CBClear);
            this.groupBox1.Controls.Add(this.CBIgnoreError);
            this.groupBox1.Controls.Add(this.TimeOutMins);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(448, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 85);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // CBClear
            // 
            this.CBClear.AutoSize = true;
            this.CBClear.Location = new System.Drawing.Point(110, 62);
            this.CBClear.Name = "CBClear";
            this.CBClear.Size = new System.Drawing.Size(84, 16);
            this.CBClear.TabIndex = 3;
            this.CBClear.Text = "清空目标表";
            this.CBClear.UseVisualStyleBackColor = true;
            // 
            // CBIgnoreError
            // 
            this.CBIgnoreError.AutoSize = true;
            this.CBIgnoreError.Location = new System.Drawing.Point(20, 62);
            this.CBIgnoreError.Name = "CBIgnoreError";
            this.CBIgnoreError.Size = new System.Drawing.Size(72, 16);
            this.CBIgnoreError.TabIndex = 2;
            this.CBIgnoreError.Text = "忽略错误";
            this.CBIgnoreError.UseVisualStyleBackColor = true;
            // 
            // TimeOutMins
            // 
            this.TimeOutMins.Location = new System.Drawing.Point(124, 25);
            this.TimeOutMins.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.TimeOutMins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TimeOutMins.Name = "TimeOutMins";
            this.TimeOutMins.Size = new System.Drawing.Size(76, 21);
            this.TimeOutMins.TabIndex = 1;
            this.TimeOutMins.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "执行超时时间(M)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Sienna;
            this.label3.Location = new System.Drawing.Point(21, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "第二步：点击还原数据，导入数据。";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(21, 45);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(209, 12);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "第一步：手动执行数据库表创建脚本。";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TBS);
            this.groupBox2.Controls.Add(this.DBS);
            this.groupBox2.Location = new System.Drawing.Point(12, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(823, 208);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择库表";
            // 
            // TBS
            // 
            this.TBS.FormattingEnabled = true;
            this.TBS.Location = new System.Drawing.Point(386, 20);
            this.TBS.Name = "TBS";
            this.TBS.Size = new System.Drawing.Size(431, 180);
            this.TBS.TabIndex = 1;
            // 
            // DBS
            // 
            this.DBS.FormattingEnabled = true;
            this.DBS.Location = new System.Drawing.Point(11, 20);
            this.DBS.Name = "DBS";
            this.DBS.Size = new System.Drawing.Size(369, 180);
            this.DBS.TabIndex = 0;
            this.DBS.SelectedIndexChanged += new System.EventHandler(this.DBS_SelectedIndexChanged);
            // 
            // BtnSelAll
            // 
            this.BtnSelAll.Location = new System.Drawing.Point(12, 343);
            this.BtnSelAll.Name = "BtnSelAll";
            this.BtnSelAll.Size = new System.Drawing.Size(75, 23);
            this.BtnSelAll.TabIndex = 10;
            this.BtnSelAll.Text = "全消";
            this.BtnSelAll.UseVisualStyleBackColor = true;
            this.BtnSelAll.Click += new System.EventHandler(this.BtnSelAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "库重命名";
            // 
            // TBDBReName
            // 
            this.TBDBReName.Location = new System.Drawing.Point(269, 60);
            this.TBDBReName.Name = "TBDBReName";
            this.TBDBReName.Size = new System.Drawing.Size(112, 21);
            this.TBDBReName.TabIndex = 5;
            // 
            // RecoverDBDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 391);
            this.Controls.Add(this.BtnSelAll);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnRecover);
            this.Controls.Add(this.TBPath);
            this.Controls.Add(this.BtnChooseDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecoverDBDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "还原数据库";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeOutMins)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnChooseDir;
        private System.Windows.Forms.TextBox TBPath;
        private System.Windows.Forms.Button BtnRecover;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProcessBar;
        private System.Windows.Forms.ToolStripStatusLabel LBMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown TimeOutMins;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox CBIgnoreError;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox DBS;
        private System.Windows.Forms.CheckedListBox TBS;
        private System.Windows.Forms.Button BtnSelAll;
        private System.Windows.Forms.CheckBox CBClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBDBReName;
    }
}