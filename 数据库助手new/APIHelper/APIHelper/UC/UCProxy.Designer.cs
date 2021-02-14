namespace APIHelper.UC
{
    partial class UCProxy
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TBProxyPwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TBProxyName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TBProxyIp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TBProxyPwd
            // 
            this.TBProxyPwd.Location = new System.Drawing.Point(92, 71);
            this.TBProxyPwd.Name = "TBProxyPwd";
            this.TBProxyPwd.Size = new System.Drawing.Size(100, 21);
            this.TBProxyPwd.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "密码：";
            // 
            // TBProxyName
            // 
            this.TBProxyName.Location = new System.Drawing.Point(91, 41);
            this.TBProxyName.Name = "TBProxyName";
            this.TBProxyName.Size = new System.Drawing.Size(100, 21);
            this.TBProxyName.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "用户名：";
            // 
            // TBProxyIp
            // 
            this.TBProxyIp.Location = new System.Drawing.Point(91, 9);
            this.TBProxyIp.Name = "TBProxyIp";
            this.TBProxyIp.Size = new System.Drawing.Size(159, 21);
            this.TBProxyIp.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "服务器地址：";
            // 
            // UCProxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TBProxyPwd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TBProxyName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TBProxyIp);
            this.Controls.Add(this.label4);
            this.Name = "UCProxy";
            this.Size = new System.Drawing.Size(345, 102);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBProxyPwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TBProxyName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBProxyIp;
        private System.Windows.Forms.Label label4;
    }
}
