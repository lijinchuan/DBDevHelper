namespace NETDBHelper
{
    partial class ConnSQLServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnSQLServer));
            this.label1 = new System.Windows.Forms.Label();
            this.cb_ServerType = new System.Windows.Forms.ComboBox();
            this.cb_servers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_yz = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_username = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn_Conn = new System.Windows.Forms.Button();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.panel_yz = new System.Windows.Forms.Panel();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.panel_main = new System.Windows.Forms.Panel();
            this.labmsg = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnRefrash = new System.Windows.Forms.Button();
            this.panel_yz.SuspendLayout();
            this.panel_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "服务器类型";
            // 
            // cb_ServerType
            // 
            this.cb_ServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ServerType.FormattingEnabled = true;
            this.cb_ServerType.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_ServerType.Items.AddRange(new object[] {
            "数据库引擎"});
            this.cb_ServerType.Location = new System.Drawing.Point(126, 10);
            this.cb_ServerType.Name = "cb_ServerType";
            this.cb_ServerType.Size = new System.Drawing.Size(328, 20);
            this.cb_ServerType.TabIndex = 2;
            // 
            // cb_servers
            // 
            this.cb_servers.FormattingEnabled = true;
            this.cb_servers.Location = new System.Drawing.Point(126, 42);
            this.cb_servers.Name = "cb_servers";
            this.cb_servers.Size = new System.Drawing.Size(328, 20);
            this.cb_servers.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "服务器名称";
            // 
            // cb_yz
            // 
            this.cb_yz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_yz.FormattingEnabled = true;
            this.cb_yz.Items.AddRange(new object[] {
            "数据库引擎"});
            this.cb_yz.Location = new System.Drawing.Point(126, 75);
            this.cb_yz.Name = "cb_yz";
            this.cb_yz.Size = new System.Drawing.Size(328, 20);
            this.cb_yz.TabIndex = 6;
            this.cb_yz.SelectedIndexChanged += new System.EventHandler(this.cb_yz_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "身份验证";
            // 
            // cb_username
            // 
            this.cb_username.FormattingEnabled = true;
            this.cb_username.Items.AddRange(new object[] {
            "sa"});
            this.cb_username.Location = new System.Drawing.Point(121, 6);
            this.cb_username.Name = "cb_username";
            this.cb_username.Size = new System.Drawing.Size(319, 20);
            this.cb_username.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "用户名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "密码";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-3, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 10);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // Btn_Conn
            // 
            this.Btn_Conn.Location = new System.Drawing.Point(47, 186);
            this.Btn_Conn.Name = "Btn_Conn";
            this.Btn_Conn.Size = new System.Drawing.Size(106, 33);
            this.Btn_Conn.TabIndex = 12;
            this.Btn_Conn.Text = "连接";
            this.Btn_Conn.UseVisualStyleBackColor = true;
            this.Btn_Conn.Click += new System.EventHandler(this.Btn_Conn_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(311, 185);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(113, 33);
            this.Btn_Cancel.TabIndex = 13;
            this.Btn_Cancel.Text = "取消";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // panel_yz
            // 
            this.panel_yz.Controls.Add(this.tb_password);
            this.panel_yz.Controls.Add(this.label4);
            this.panel_yz.Controls.Add(this.cb_username);
            this.panel_yz.Controls.Add(this.label5);
            this.panel_yz.Location = new System.Drawing.Point(14, 104);
            this.panel_yz.Name = "panel_yz";
            this.panel_yz.Size = new System.Drawing.Size(451, 62);
            this.panel_yz.TabIndex = 14;
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(121, 34);
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.Size = new System.Drawing.Size(319, 21);
            this.tb_password.TabIndex = 10;
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.BtnRefrash);
            this.panel_main.Controls.Add(this.label1);
            this.panel_main.Controls.Add(this.panel_yz);
            this.panel_main.Controls.Add(this.cb_ServerType);
            this.panel_main.Controls.Add(this.Btn_Cancel);
            this.panel_main.Controls.Add(this.label2);
            this.panel_main.Controls.Add(this.Btn_Conn);
            this.panel_main.Controls.Add(this.cb_servers);
            this.panel_main.Controls.Add(this.groupBox1);
            this.panel_main.Controls.Add(this.label3);
            this.panel_main.Controls.Add(this.cb_yz);
            this.panel_main.Location = new System.Drawing.Point(3, 86);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(464, 233);
            this.panel_main.TabIndex = 15;
            // 
            // labmsg
            // 
            this.labmsg.AutoSize = true;
            this.labmsg.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labmsg.Location = new System.Drawing.Point(8, 61);
            this.labmsg.Name = "labmsg";
            this.labmsg.Size = new System.Drawing.Size(0, 12);
            this.labmsg.TabIndex = 16;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(468, 86);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // BtnRefrash
            // 
            this.BtnRefrash.Location = new System.Drawing.Point(183, 186);
            this.BtnRefrash.Name = "BtnRefrash";
            this.BtnRefrash.Size = new System.Drawing.Size(106, 33);
            this.BtnRefrash.TabIndex = 15;
            this.BtnRefrash.Text = "刷新";
            this.BtnRefrash.UseVisualStyleBackColor = true;
            this.BtnRefrash.Click += new System.EventHandler(this.BtnRefrash_Click);
            // 
            // ConnSQLServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 317);
            this.Controls.Add(this.labmsg);
            this.Controls.Add(this.panel_main);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnSQLServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "连接数据库服务器";
            this.Load += new System.EventHandler(this.ConnSQLServer_Load);
            this.panel_yz.ResumeLayout(false);
            this.panel_yz.PerformLayout();
            this.panel_main.ResumeLayout(false);
            this.panel_main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_ServerType;
        private System.Windows.Forms.ComboBox cb_servers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_yz;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_username;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_Conn;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Panel panel_yz;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.Label labmsg;
        private System.Windows.Forms.Button BtnRefrash;
    }
}