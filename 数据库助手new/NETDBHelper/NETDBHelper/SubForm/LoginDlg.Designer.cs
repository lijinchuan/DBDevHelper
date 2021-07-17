
namespace NETDBHelper.SubForm
{
    partial class LoginDlg
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
            this.label2 = new System.Windows.Forms.Label();
            this.TBUser = new System.Windows.Forms.TextBox();
            this.TBPwd = new System.Windows.Forms.TextBox();
            this.BTNLogin = new System.Windows.Forms.Button();
            this.BTNCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码";
            // 
            // TBUser
            // 
            this.TBUser.Location = new System.Drawing.Point(115, 32);
            this.TBUser.Name = "TBUser";
            this.TBUser.Size = new System.Drawing.Size(154, 21);
            this.TBUser.TabIndex = 2;
            // 
            // TBPwd
            // 
            this.TBPwd.Location = new System.Drawing.Point(115, 70);
            this.TBPwd.Name = "TBPwd";
            this.TBPwd.PasswordChar = '*';
            this.TBPwd.Size = new System.Drawing.Size(154, 21);
            this.TBPwd.TabIndex = 3;
            // 
            // BTNLogin
            // 
            this.BTNLogin.Location = new System.Drawing.Point(127, 128);
            this.BTNLogin.Name = "BTNLogin";
            this.BTNLogin.Size = new System.Drawing.Size(75, 23);
            this.BTNLogin.TabIndex = 4;
            this.BTNLogin.Text = "登录";
            this.BTNLogin.UseVisualStyleBackColor = true;
            this.BTNLogin.Click += new System.EventHandler(this.BTNLogin_Click);
            // 
            // BTNCancel
            // 
            this.BTNCancel.Location = new System.Drawing.Point(219, 128);
            this.BTNCancel.Name = "BTNCancel";
            this.BTNCancel.Size = new System.Drawing.Size(71, 23);
            this.BTNCancel.TabIndex = 5;
            this.BTNCancel.Text = "退出";
            this.BTNCancel.UseVisualStyleBackColor = true;
            this.BTNCancel.Click += new System.EventHandler(this.BTNCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-4, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 10);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // LoginDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 163);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BTNCancel);
            this.Controls.Add(this.BTNLogin);
            this.Controls.Add(this.TBPwd);
            this.Controls.Add(this.TBUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBUser;
        private System.Windows.Forms.TextBox TBPwd;
        private System.Windows.Forms.Button BTNLogin;
        private System.Windows.Forms.Button BTNCancel;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}