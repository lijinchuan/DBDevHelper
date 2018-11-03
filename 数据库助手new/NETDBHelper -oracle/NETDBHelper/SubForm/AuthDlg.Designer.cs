namespace NETDBHelper.SubForm
{
    partial class AuthDlg
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBCHANGE = new System.Windows.Forms.CheckBox();
            this.CBVIEW = new System.Windows.Forms.CheckBox();
            this.CBINDEX = new System.Windows.Forms.CheckBox();
            this.CBUPDATE = new System.Windows.Forms.CheckBox();
            this.CBDELETE = new System.Windows.Forms.CheckBox();
            this.CBINSERT = new System.Windows.Forms.CheckBox();
            this.CBSELECT = new System.Windows.Forms.CheckBox();
            this.CBUsers = new System.Windows.Forms.ComboBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.CBALL = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBALL);
            this.groupBox1.Controls.Add(this.CBCHANGE);
            this.groupBox1.Controls.Add(this.CBVIEW);
            this.groupBox1.Controls.Add(this.CBINDEX);
            this.groupBox1.Controls.Add(this.CBUPDATE);
            this.groupBox1.Controls.Add(this.CBDELETE);
            this.groupBox1.Controls.Add(this.CBINSERT);
            this.groupBox1.Controls.Add(this.CBSELECT);
            this.groupBox1.Location = new System.Drawing.Point(35, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "权限";
            // 
            // CBCHANGE
            // 
            this.CBCHANGE.AutoSize = true;
            this.CBCHANGE.Location = new System.Drawing.Point(368, 20);
            this.CBCHANGE.Name = "CBCHANGE";
            this.CBCHANGE.Size = new System.Drawing.Size(48, 16);
            this.CBCHANGE.TabIndex = 6;
            this.CBCHANGE.Text = "改变";
            this.CBCHANGE.UseVisualStyleBackColor = true;
            // 
            // CBVIEW
            // 
            this.CBVIEW.AutoSize = true;
            this.CBVIEW.Location = new System.Drawing.Point(305, 20);
            this.CBVIEW.Name = "CBVIEW";
            this.CBVIEW.Size = new System.Drawing.Size(48, 16);
            this.CBVIEW.TabIndex = 5;
            this.CBVIEW.Text = "视图";
            this.CBVIEW.UseVisualStyleBackColor = true;
            // 
            // CBINDEX
            // 
            this.CBINDEX.AutoSize = true;
            this.CBINDEX.Location = new System.Drawing.Point(251, 20);
            this.CBINDEX.Name = "CBINDEX";
            this.CBINDEX.Size = new System.Drawing.Size(48, 16);
            this.CBINDEX.TabIndex = 4;
            this.CBINDEX.Text = "索引";
            this.CBINDEX.UseVisualStyleBackColor = true;
            // 
            // CBUPDATE
            // 
            this.CBUPDATE.AutoSize = true;
            this.CBUPDATE.Location = new System.Drawing.Point(197, 20);
            this.CBUPDATE.Name = "CBUPDATE";
            this.CBUPDATE.Size = new System.Drawing.Size(48, 16);
            this.CBUPDATE.TabIndex = 3;
            this.CBUPDATE.Text = "更新";
            this.CBUPDATE.UseVisualStyleBackColor = true;
            // 
            // CBDELETE
            // 
            this.CBDELETE.AutoSize = true;
            this.CBDELETE.Location = new System.Drawing.Point(143, 20);
            this.CBDELETE.Name = "CBDELETE";
            this.CBDELETE.Size = new System.Drawing.Size(48, 16);
            this.CBDELETE.TabIndex = 2;
            this.CBDELETE.Text = "删除";
            this.CBDELETE.UseVisualStyleBackColor = true;
            // 
            // CBINSERT
            // 
            this.CBINSERT.AutoSize = true;
            this.CBINSERT.Location = new System.Drawing.Point(89, 20);
            this.CBINSERT.Name = "CBINSERT";
            this.CBINSERT.Size = new System.Drawing.Size(48, 16);
            this.CBINSERT.TabIndex = 1;
            this.CBINSERT.Text = "新增";
            this.CBINSERT.UseVisualStyleBackColor = true;
            // 
            // CBSELECT
            // 
            this.CBSELECT.AutoSize = true;
            this.CBSELECT.Location = new System.Drawing.Point(35, 20);
            this.CBSELECT.Name = "CBSELECT";
            this.CBSELECT.Size = new System.Drawing.Size(48, 16);
            this.CBSELECT.TabIndex = 0;
            this.CBSELECT.Text = "选择";
            this.CBSELECT.UseVisualStyleBackColor = true;
            // 
            // CBUsers
            // 
            this.CBUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBUsers.FormattingEnabled = true;
            this.CBUsers.Location = new System.Drawing.Point(80, 24);
            this.CBUsers.Name = "CBUsers";
            this.CBUsers.Size = new System.Drawing.Size(200, 20);
            this.CBUsers.TabIndex = 2;
            this.CBUsers.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(286, 208);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 3;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(376, 208);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CBALL
            // 
            this.CBALL.AutoSize = true;
            this.CBALL.Location = new System.Drawing.Point(36, 54);
            this.CBALL.Name = "CBALL";
            this.CBALL.Size = new System.Drawing.Size(48, 16);
            this.CBALL.TabIndex = 7;
            this.CBALL.Text = "所有";
            this.CBALL.UseVisualStyleBackColor = true;
            this.CBALL.CheckedChanged += new System.EventHandler(this.CBALL_CheckedChanged);
            // 
            // AuthDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.CBUsers);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "{0}授权";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CBSELECT;
        private System.Windows.Forms.CheckBox CBINSERT;
        private System.Windows.Forms.CheckBox CBDELETE;
        private System.Windows.Forms.CheckBox CBUPDATE;
        private System.Windows.Forms.CheckBox CBINDEX;
        private System.Windows.Forms.CheckBox CBVIEW;
        private System.Windows.Forms.CheckBox CBCHANGE;
        private System.Windows.Forms.ComboBox CBUsers;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.CheckBox CBALL;
    }
}