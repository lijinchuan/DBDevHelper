namespace NETDBHelper.SubForm
{
    partial class AddTaskInfoDlg
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBValid = new System.Windows.Forms.CheckBox();
            this.TBErrmsg = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CBDB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnSelectServer = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnCanel = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.CBTimeDw = new System.Windows.Forms.ComboBox();
            this.TBRate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PanelEqualValue = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.TBConditionValue = new System.Windows.Forms.TextBox();
            this.CLB_Condition = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBSql = new System.Windows.Forms.TextBox();
            this.TBName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.PanelEqualValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBValid);
            this.groupBox1.Controls.Add(this.TBErrmsg);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.CBDB);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.BtnSelectServer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.BtnCanel);
            this.groupBox1.Controls.Add(this.BtnAdd);
            this.groupBox1.Controls.Add(this.CBTimeDw);
            this.groupBox1.Controls.Add(this.TBRate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.PanelEqualValue);
            this.groupBox1.Controls.Add(this.CLB_Condition);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TBSql);
            this.groupBox1.Controls.Add(this.TBName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 366);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // CBValid
            // 
            this.CBValid.AutoSize = true;
            this.CBValid.Checked = true;
            this.CBValid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBValid.Location = new System.Drawing.Point(496, 16);
            this.CBValid.Name = "CBValid";
            this.CBValid.Size = new System.Drawing.Size(48, 16);
            this.CBValid.TabIndex = 18;
            this.CBValid.Text = "生效";
            this.CBValid.UseVisualStyleBackColor = true;
            // 
            // TBErrmsg
            // 
            this.TBErrmsg.Location = new System.Drawing.Point(83, 293);
            this.TBErrmsg.Name = "TBErrmsg";
            this.TBErrmsg.Size = new System.Drawing.Size(491, 21);
            this.TBErrmsg.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 298);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "信息提示：";
            // 
            // CBDB
            // 
            this.CBDB.FormattingEnabled = true;
            this.CBDB.Location = new System.Drawing.Point(271, 14);
            this.CBDB.Name = "CBDB";
            this.CBDB.Size = new System.Drawing.Size(200, 20);
            this.CBDB.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "连接数据库：";
            // 
            // BtnSelectServer
            // 
            this.BtnSelectServer.Location = new System.Drawing.Point(86, 12);
            this.BtnSelectServer.Name = "BtnSelectServer";
            this.BtnSelectServer.Size = new System.Drawing.Size(75, 23);
            this.BtnSelectServer.TabIndex = 13;
            this.BtnSelectServer.Text = "服务器";
            this.BtnSelectServer.UseVisualStyleBackColor = true;
            this.BtnSelectServer.Click += new System.EventHandler(this.BtnSelectServer_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "服务器：";
            // 
            // BtnCanel
            // 
            this.BtnCanel.Location = new System.Drawing.Point(473, 331);
            this.BtnCanel.Name = "BtnCanel";
            this.BtnCanel.Size = new System.Drawing.Size(75, 23);
            this.BtnCanel.TabIndex = 11;
            this.BtnCanel.Text = "放弃";
            this.BtnCanel.UseVisualStyleBackColor = true;
            this.BtnCanel.Click += new System.EventHandler(this.BtnCanel_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(377, 332);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 10;
            this.BtnAdd.Text = "添加";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // CBTimeDw
            // 
            this.CBTimeDw.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBTimeDw.FormattingEnabled = true;
            this.CBTimeDw.Items.AddRange(new object[] {
            "秒"});
            this.CBTimeDw.Location = new System.Drawing.Point(184, 261);
            this.CBTimeDw.Name = "CBTimeDw";
            this.CBTimeDw.Size = new System.Drawing.Size(40, 20);
            this.CBTimeDw.TabIndex = 9;
            // 
            // TBRate
            // 
            this.TBRate.Location = new System.Drawing.Point(83, 260);
            this.TBRate.Name = "TBRate";
            this.TBRate.Size = new System.Drawing.Size(100, 21);
            this.TBRate.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "监控频率：";
            // 
            // PanelEqualValue
            // 
            this.PanelEqualValue.Controls.Add(this.label4);
            this.PanelEqualValue.Controls.Add(this.TBConditionValue);
            this.PanelEqualValue.Location = new System.Drawing.Point(179, 198);
            this.PanelEqualValue.Name = "PanelEqualValue";
            this.PanelEqualValue.Size = new System.Drawing.Size(200, 45);
            this.PanelEqualValue.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "=";
            // 
            // TBConditionValue
            // 
            this.TBConditionValue.Location = new System.Drawing.Point(18, 18);
            this.TBConditionValue.Name = "TBConditionValue";
            this.TBConditionValue.Size = new System.Drawing.Size(169, 21);
            this.TBConditionValue.TabIndex = 2;
            // 
            // CLB_Condition
            // 
            this.CLB_Condition.FormattingEnabled = true;
            this.CLB_Condition.Items.AddRange(new object[] {
            "空值触发",
            "非空值触发",
            "特定值触发"});
            this.CLB_Condition.Location = new System.Drawing.Point(84, 189);
            this.CLB_Condition.Name = "CLB_Condition";
            this.CLB_Condition.Size = new System.Drawing.Size(95, 52);
            this.CLB_Condition.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "条件：";
            // 
            // TBSql
            // 
            this.TBSql.Location = new System.Drawing.Point(84, 81);
            this.TBSql.Multiline = true;
            this.TBSql.Name = "TBSql";
            this.TBSql.Size = new System.Drawing.Size(501, 92);
            this.TBSql.TabIndex = 3;
            // 
            // TBName
            // 
            this.TBName.Location = new System.Drawing.Point(84, 45);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(297, 21);
            this.TBName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "SQL语句：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称：";
            // 
            // AddTaskInfoDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 366);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddTaskInfoDlg";
            this.ShowIcon = false;
            this.Text = "添加/编辑监控任务";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.PanelEqualValue.ResumeLayout(false);
            this.PanelEqualValue.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBName;
        private System.Windows.Forms.TextBox TBSql;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox CLB_Condition;
        private System.Windows.Forms.Panel PanelEqualValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBConditionValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBRate;
        private System.Windows.Forms.ComboBox CBTimeDw;
        private System.Windows.Forms.Button BtnCanel;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnSelectServer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CBDB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TBErrmsg;
        private System.Windows.Forms.CheckBox CBValid;
    }
}