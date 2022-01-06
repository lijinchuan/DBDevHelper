
namespace NETDBHelper.SubForm
{
    partial class ServerSettingDlg
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
            this.label9 = new System.Windows.Forms.Label();
            this.TBRemotingPassword = new System.Windows.Forms.TextBox();
            this.TBRemotingUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TBRemotingPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBRemotingIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TBLocalPassword = new System.Windows.Forms.TextBox();
            this.TBLocalUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TBLocalPort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.TBRemotingPassword);
            this.groupBox1.Controls.Add(this.TBRemotingUserName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TBRemotingPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TBRemotingIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 109);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "远程服务器";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Info;
            this.label9.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label9.Location = new System.Drawing.Point(221, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(152, 37);
            this.label9.TabIndex = 17;
            this.label9.Text = "使用远程的测试用例服务，需要对方提供IP端口和账号";
            // 
            // TBRemotingPassword
            // 
            this.TBRemotingPassword.Location = new System.Drawing.Point(71, 73);
            this.TBRemotingPassword.Name = "TBRemotingPassword";
            this.TBRemotingPassword.Size = new System.Drawing.Size(132, 21);
            this.TBRemotingPassword.TabIndex = 7;
            // 
            // TBRemotingUserName
            // 
            this.TBRemotingUserName.Location = new System.Drawing.Point(71, 46);
            this.TBRemotingUserName.Name = "TBRemotingUserName";
            this.TBRemotingUserName.Size = new System.Drawing.Size(132, 21);
            this.TBRemotingUserName.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "用户名：";
            // 
            // TBRemotingPort
            // 
            this.TBRemotingPort.Location = new System.Drawing.Point(266, 20);
            this.TBRemotingPort.Name = "TBRemotingPort";
            this.TBRemotingPort.Size = new System.Drawing.Size(61, 21);
            this.TBRemotingPort.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // TBRemotingIP
            // 
            this.TBRemotingIP.Location = new System.Drawing.Point(71, 20);
            this.TBRemotingIP.Name = "TBRemotingIP";
            this.TBRemotingIP.Size = new System.Drawing.Size(132, 21);
            this.TBRemotingIP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.TBLocalPassword);
            this.groupBox2.Controls.Add(this.TBLocalUserName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.TBLocalPort);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(1, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 115);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "本地服务器";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Info;
            this.label8.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label8.Location = new System.Drawing.Point(213, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 43);
            this.label8.TabIndex = 16;
            this.label8.Text = "打开本地服务，共享测试用例，端口1025-65535，请确保添加防火墙例外";
            // 
            // TBLocalPassword
            // 
            this.TBLocalPassword.Location = new System.Drawing.Point(71, 81);
            this.TBLocalPassword.Name = "TBLocalPassword";
            this.TBLocalPassword.Size = new System.Drawing.Size(132, 21);
            this.TBLocalPassword.TabIndex = 15;
            // 
            // TBLocalUserName
            // 
            this.TBLocalUserName.Location = new System.Drawing.Point(71, 54);
            this.TBLocalUserName.Name = "TBLocalUserName";
            this.TBLocalUserName.Size = new System.Drawing.Size(132, 21);
            this.TBLocalUserName.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "密码：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "用户名：";
            // 
            // TBLocalPort
            // 
            this.TBLocalPort.Location = new System.Drawing.Point(71, 26);
            this.TBLocalPort.Name = "TBLocalPort";
            this.TBLocalPort.Size = new System.Drawing.Size(61, 21);
            this.TBLocalPort.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "端口：";
            // 
            // BtnOk
            // 
            this.BtnOk.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnOk.Location = new System.Drawing.Point(233, 239);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(69, 30);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.Text = "保存";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnCancel.Location = new System.Drawing.Point(308, 239);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(69, 30);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "关闭";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ServerSettingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 272);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ServerSettingDlg";
            this.ShowIcon = false;
            this.Text = "服务器配置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TBRemotingIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBRemotingPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBRemotingPassword;
        private System.Windows.Forms.TextBox TBRemotingUserName;
        private System.Windows.Forms.TextBox TBLocalPassword;
        private System.Windows.Forms.TextBox TBLocalUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TBLocalPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}