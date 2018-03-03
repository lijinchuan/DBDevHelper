namespace RedisHelperUI
{
    partial class AddRedisServerForm
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
            this.TBName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBConnstr = new System.Windows.Forms.TextBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.CBTest = new System.Windows.Forms.CheckBox();
            this.CBIsprd = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称";
            // 
            // TBName
            // 
            this.TBName.Location = new System.Drawing.Point(99, 31);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(382, 21);
            this.TBName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "连接配置";
            // 
            // TBConnstr
            // 
            this.TBConnstr.Location = new System.Drawing.Point(99, 81);
            this.TBConnstr.Multiline = true;
            this.TBConnstr.Name = "TBConnstr";
            this.TBConnstr.Size = new System.Drawing.Size(443, 117);
            this.TBConnstr.TabIndex = 3;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(406, 281);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 4;
            this.BtnAdd.Text = "确定";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // CBTest
            // 
            this.CBTest.AutoSize = true;
            this.CBTest.Checked = true;
            this.CBTest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBTest.Location = new System.Drawing.Point(210, 229);
            this.CBTest.Name = "CBTest";
            this.CBTest.Size = new System.Drawing.Size(96, 16);
            this.CBTest.TabIndex = 5;
            this.CBTest.Text = "验证连接配置";
            this.CBTest.UseVisualStyleBackColor = true;
            // 
            // CBIsprd
            // 
            this.CBIsprd.AutoSize = true;
            this.CBIsprd.Checked = true;
            this.CBIsprd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBIsprd.Location = new System.Drawing.Point(99, 229);
            this.CBIsprd.Name = "CBIsprd";
            this.CBIsprd.Size = new System.Drawing.Size(72, 16);
            this.CBIsprd.TabIndex = 6;
            this.CBIsprd.Text = "生产环境";
            this.CBIsprd.UseVisualStyleBackColor = true;
            // 
            // AddRedisServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 339);
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
            this.Name = "AddRedisServerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加服务器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBConnstr;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.CheckBox CBTest;
        private System.Windows.Forms.CheckBox CBIsprd;
    }
}