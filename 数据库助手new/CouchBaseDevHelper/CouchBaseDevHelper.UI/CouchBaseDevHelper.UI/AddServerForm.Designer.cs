namespace CouchBaseDevHelper.UI
{
    partial class AddServerForm
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
            this.CBIsprd = new System.Windows.Forms.CheckBox();
            this.CBTest = new System.Windows.Forms.CheckBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.TBConnstr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CBServerType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TBDLLFile = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnLoadFile = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBIsprd
            // 
            this.CBIsprd.AutoSize = true;
            this.CBIsprd.Checked = true;
            this.CBIsprd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBIsprd.Location = new System.Drawing.Point(96, 312);
            this.CBIsprd.Name = "CBIsprd";
            this.CBIsprd.Size = new System.Drawing.Size(72, 16);
            this.CBIsprd.TabIndex = 13;
            this.CBIsprd.Text = "生产环境";
            this.CBIsprd.UseVisualStyleBackColor = true;
            // 
            // CBTest
            // 
            this.CBTest.AutoSize = true;
            this.CBTest.Checked = true;
            this.CBTest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBTest.Location = new System.Drawing.Point(207, 312);
            this.CBTest.Name = "CBTest";
            this.CBTest.Size = new System.Drawing.Size(96, 16);
            this.CBTest.TabIndex = 12;
            this.CBTest.Text = "验证连接配置";
            this.CBTest.UseVisualStyleBackColor = true;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(420, 340);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 11;
            this.BtnAdd.Text = "确定";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // TBConnstr
            // 
            this.TBConnstr.Location = new System.Drawing.Point(96, 103);
            this.TBConnstr.Multiline = true;
            this.TBConnstr.Name = "TBConnstr";
            this.TBConnstr.Size = new System.Drawing.Size(443, 117);
            this.TBConnstr.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "连接配置";
            // 
            // TBName
            // 
            this.TBName.Location = new System.Drawing.Point(96, 53);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(382, 21);
            this.TBName.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "类型";
            // 
            // CBServerType
            // 
            this.CBServerType.FormattingEnabled = true;
            this.CBServerType.Location = new System.Drawing.Point(96, 246);
            this.CBServerType.Name = "CBServerType";
            this.CBServerType.Size = new System.Drawing.Size(121, 20);
            this.CBServerType.TabIndex = 15;
            this.CBServerType.SelectedIndexChanged += new System.EventHandler(this.CBServerType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "客户端插件：";
            // 
            // TBDLLFile
            // 
            this.TBDLLFile.Location = new System.Drawing.Point(84, 10);
            this.TBDLLFile.Name = "TBDLLFile";
            this.TBDLLFile.ReadOnly = true;
            this.TBDLLFile.Size = new System.Drawing.Size(183, 21);
            this.TBDLLFile.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.BtnLoadFile);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.TBDLLFile);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(235, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 72);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(7, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 30);
            this.label5.TabIndex = 19;
            this.label5.Text = "默认客户端框架是Enyim.Caching，不需要填写。自定义插件需实现\r\nLJC.FrameWork.MemCached.ICachClient接口，并实现构造" +
    "函数\r\n(string serverip, int port, string bucket)";
            // 
            // BtnLoadFile
            // 
            this.BtnLoadFile.Location = new System.Drawing.Point(268, 9);
            this.BtnLoadFile.Name = "BtnLoadFile";
            this.BtnLoadFile.Size = new System.Drawing.Size(34, 23);
            this.BtnLoadFile.TabIndex = 18;
            this.BtnLoadFile.Text = "...";
            this.BtnLoadFile.UseVisualStyleBackColor = true;
            this.BtnLoadFile.Click += new System.EventHandler(this.BtnLoadFile_Click);
            // 
            // AddServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 379);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CBServerType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CBIsprd);
            this.Controls.Add(this.CBTest);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.TBConnstr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddServerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加服务器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CBIsprd;
        private System.Windows.Forms.CheckBox CBTest;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TextBox TBConnstr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBServerType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBDLLFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnLoadFile;
        private System.Windows.Forms.Label label5;
    }
}