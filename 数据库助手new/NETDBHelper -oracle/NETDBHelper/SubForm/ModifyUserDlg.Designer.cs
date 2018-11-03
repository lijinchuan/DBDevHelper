namespace NETDBHelper.SubForm
{
    partial class ModifyUserDlg
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
            this.TBNewPassword = new System.Windows.Forms.TextBox();
            this.BtnUpdatePassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "修改密码：";
            // 
            // TBNewPassword
            // 
            this.TBNewPassword.Location = new System.Drawing.Point(94, 29);
            this.TBNewPassword.Name = "TBNewPassword";
            this.TBNewPassword.Size = new System.Drawing.Size(156, 21);
            this.TBNewPassword.TabIndex = 1;
            // 
            // BtnUpdatePassword
            // 
            this.BtnUpdatePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnUpdatePassword.Location = new System.Drawing.Point(265, 29);
            this.BtnUpdatePassword.Name = "BtnUpdatePassword";
            this.BtnUpdatePassword.Size = new System.Drawing.Size(75, 23);
            this.BtnUpdatePassword.TabIndex = 2;
            this.BtnUpdatePassword.Text = "修改";
            this.BtnUpdatePassword.UseVisualStyleBackColor = true;
            this.BtnUpdatePassword.Click += new System.EventHandler(this.BtnUpdatePassword_Click);
            // 
            // ModifyUserDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 77);
            this.Controls.Add(this.BtnUpdatePassword);
            this.Controls.Add(this.TBNewPassword);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyUserDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改用户密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBNewPassword;
        private System.Windows.Forms.Button BtnUpdatePassword;
    }
}