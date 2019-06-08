namespace NETDBHelper.SubForm
{
    partial class SyncDataWin
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
            this.BtnSave = new System.Windows.Forms.Button();
            this.TBAutoSyncMS = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.BtnCheckDest = new System.Windows.Forms.Button();
            this.CBKey = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.BtnLoadFields = new System.Windows.Forms.Button();
            this.BtnSync = new System.Windows.Forms.Button();
            this.CBFields = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TBDest = new System.Windows.Forms.TextBox();
            this.TBSource = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TBDestConnStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBSourceConnStr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(696, 402);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 47;
            this.BtnSave.Text = "保存退出";
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // TBAutoSyncMS
            // 
            this.TBAutoSyncMS.Location = new System.Drawing.Point(145, 272);
            this.TBAutoSyncMS.Name = "TBAutoSyncMS";
            this.TBAutoSyncMS.Size = new System.Drawing.Size(100, 21);
            this.TBAutoSyncMS.TabIndex = 46;
            this.TBAutoSyncMS.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 278);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 12);
            this.label10.TabIndex = 45;
            this.label10.Text = "自动同步间隔（s）:";
            // 
            // BtnCheckDest
            // 
            this.BtnCheckDest.Location = new System.Drawing.Point(330, 216);
            this.BtnCheckDest.Name = "BtnCheckDest";
            this.BtnCheckDest.Size = new System.Drawing.Size(75, 23);
            this.BtnCheckDest.TabIndex = 44;
            this.BtnCheckDest.Text = "检查";
            this.BtnCheckDest.UseVisualStyleBackColor = true;
            this.BtnCheckDest.Click += new System.EventHandler(this.BtnCheckDest_Click);
            // 
            // CBKey
            // 
            this.CBKey.FormattingEnabled = true;
            this.CBKey.Location = new System.Drawing.Point(480, 59);
            this.CBKey.Name = "CBKey";
            this.CBKey.Size = new System.Drawing.Size(121, 20);
            this.CBKey.TabIndex = 43;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(433, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 42;
            this.label9.Text = "主键：";
            // 
            // BtnLoadFields
            // 
            this.BtnLoadFields.Location = new System.Drawing.Point(330, 57);
            this.BtnLoadFields.Name = "BtnLoadFields";
            this.BtnLoadFields.Size = new System.Drawing.Size(42, 23);
            this.BtnLoadFields.TabIndex = 41;
            this.BtnLoadFields.Text = "加载";
            this.BtnLoadFields.UseVisualStyleBackColor = true;
            this.BtnLoadFields.Click += new System.EventHandler(this.BtnLoadFields_Click);
            // 
            // BtnSync
            // 
            this.BtnSync.Location = new System.Drawing.Point(596, 402);
            this.BtnSync.Name = "BtnSync";
            this.BtnSync.Size = new System.Drawing.Size(75, 23);
            this.BtnSync.TabIndex = 40;
            this.BtnSync.Text = "同步";
            this.BtnSync.UseVisualStyleBackColor = true;
            this.BtnSync.Click += new System.EventHandler(this.BtnSync_Click);
            // 
            // CBFields
            // 
            this.CBFields.FormattingEnabled = true;
            this.CBFields.Location = new System.Drawing.Point(145, 90);
            this.CBFields.Name = "CBFields";
            this.CBFields.Size = new System.Drawing.Size(120, 84);
            this.CBFields.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 32;
            this.label5.Text = "同步目标库字段：";
            // 
            // TBDest
            // 
            this.TBDest.Location = new System.Drawing.Point(145, 217);
            this.TBDest.Name = "TBDest";
            this.TBDest.Size = new System.Drawing.Size(179, 21);
            this.TBDest.TabIndex = 31;
            // 
            // TBSource
            // 
            this.TBSource.Location = new System.Drawing.Point(145, 58);
            this.TBSource.Name = "TBSource";
            this.TBSource.Size = new System.Drawing.Size(179, 21);
            this.TBSource.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "目标表：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "源表：";
            // 
            // TBDestConnStr
            // 
            this.TBDestConnStr.Location = new System.Drawing.Point(145, 184);
            this.TBDestConnStr.Name = "TBDestConnStr";
            this.TBDestConnStr.Size = new System.Drawing.Size(626, 21);
            this.TBDestConnStr.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "目标数据库连接串：";
            // 
            // TBSourceConnStr
            // 
            this.TBSourceConnStr.Location = new System.Drawing.Point(145, 25);
            this.TBSourceConnStr.Name = "TBSourceConnStr";
            this.TBSourceConnStr.Size = new System.Drawing.Size(626, 21);
            this.TBSourceConnStr.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "源数据库连接串：";
            // 
            // SyncDataWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.TBAutoSyncMS);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.BtnCheckDest);
            this.Controls.Add(this.CBKey);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.BtnLoadFields);
            this.Controls.Add(this.BtnSync);
            this.Controls.Add(this.CBFields);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TBDest);
            this.Controls.Add(this.TBSource);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBDestConnStr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBSourceConnStr);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SyncDataWin";
            this.ShowIcon = false;
            this.Text = "syncdata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.TextBox TBAutoSyncMS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BtnCheckDest;
        private System.Windows.Forms.ComboBox CBKey;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BtnLoadFields;
        private System.Windows.Forms.Button BtnSync;
        private System.Windows.Forms.CheckedListBox CBFields;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBDest;
        private System.Windows.Forms.TextBox TBSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBDestConnStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBSourceConnStr;
        private System.Windows.Forms.Label label1;
    }
}