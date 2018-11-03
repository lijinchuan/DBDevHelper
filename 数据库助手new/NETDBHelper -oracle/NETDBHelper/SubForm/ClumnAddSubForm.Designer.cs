namespace NETDBHelper.SubForm
{
    partial class ClumnAddSubForm
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
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBColumnName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBType = new System.Windows.Forms.ComboBox();
            this.LBLen = new System.Windows.Forms.Label();
            this.TBLen = new System.Windows.Forms.TextBox();
            this.LBPrecison = new System.Windows.Forms.Label();
            this.TBPrecison = new System.Windows.Forms.TextBox();
            this.LBScale = new System.Windows.Forms.Label();
            this.TBScale = new System.Windows.Forms.TextBox();
            this.TBAbout = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBAboutDetail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TBDefault = new System.Windows.Forms.TextBox();
            this.CBNullAble = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(423, 312);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 0;
            this.BtnOK.Text = "添加";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(506, 312);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "列名:";
            // 
            // TBColumnName
            // 
            this.TBColumnName.Location = new System.Drawing.Point(90, 28);
            this.TBColumnName.Name = "TBColumnName";
            this.TBColumnName.Size = new System.Drawing.Size(221, 21);
            this.TBColumnName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "类型:";
            // 
            // CBType
            // 
            this.CBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBType.FormattingEnabled = true;
            this.CBType.Location = new System.Drawing.Point(90, 72);
            this.CBType.Name = "CBType";
            this.CBType.Size = new System.Drawing.Size(161, 20);
            this.CBType.TabIndex = 5;
            // 
            // LBLen
            // 
            this.LBLen.AutoSize = true;
            this.LBLen.Location = new System.Drawing.Point(267, 75);
            this.LBLen.Name = "LBLen";
            this.LBLen.Size = new System.Drawing.Size(29, 12);
            this.LBLen.TabIndex = 6;
            this.LBLen.Text = "长度";
            // 
            // TBLen
            // 
            this.TBLen.Location = new System.Drawing.Point(302, 71);
            this.TBLen.Name = "TBLen";
            this.TBLen.Size = new System.Drawing.Size(51, 21);
            this.TBLen.TabIndex = 7;
            // 
            // LBPrecison
            // 
            this.LBPrecison.AutoSize = true;
            this.LBPrecison.Location = new System.Drawing.Point(362, 76);
            this.LBPrecison.Name = "LBPrecison";
            this.LBPrecison.Size = new System.Drawing.Size(29, 12);
            this.LBPrecison.TabIndex = 8;
            this.LBPrecison.Text = "精度";
            // 
            // TBPrecison
            // 
            this.TBPrecison.Location = new System.Drawing.Point(399, 72);
            this.TBPrecison.Name = "TBPrecison";
            this.TBPrecison.Size = new System.Drawing.Size(46, 21);
            this.TBPrecison.TabIndex = 9;
            // 
            // LBScale
            // 
            this.LBScale.AutoSize = true;
            this.LBScale.Location = new System.Drawing.Point(463, 75);
            this.LBScale.Name = "LBScale";
            this.LBScale.Size = new System.Drawing.Size(41, 12);
            this.LBScale.TabIndex = 10;
            this.LBScale.Text = "小数位";
            // 
            // TBScale
            // 
            this.TBScale.Location = new System.Drawing.Point(507, 72);
            this.TBScale.Name = "TBScale";
            this.TBScale.Size = new System.Drawing.Size(59, 21);
            this.TBScale.TabIndex = 11;
            // 
            // TBAbout
            // 
            this.TBAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBAbout.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TBAbout.Location = new System.Drawing.Point(45, 190);
            this.TBAbout.Multiline = true;
            this.TBAbout.Name = "TBAbout";
            this.TBAbout.ReadOnly = true;
            this.TBAbout.Size = new System.Drawing.Size(536, 116);
            this.TBAbout.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "说明:";
            // 
            // TBAboutDetail
            // 
            this.TBAboutDetail.Location = new System.Drawing.Point(90, 147);
            this.TBAboutDetail.Name = "TBAboutDetail";
            this.TBAboutDetail.Size = new System.Drawing.Size(476, 21);
            this.TBAboutDetail.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "默认值:";
            // 
            // TBDefault
            // 
            this.TBDefault.Location = new System.Drawing.Point(90, 110);
            this.TBDefault.Name = "TBDefault";
            this.TBDefault.Size = new System.Drawing.Size(325, 21);
            this.TBDefault.TabIndex = 16;
            // 
            // CBNullAble
            // 
            this.CBNullAble.AutoSize = true;
            this.CBNullAble.Checked = true;
            this.CBNullAble.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBNullAble.Location = new System.Drawing.Point(435, 113);
            this.CBNullAble.Name = "CBNullAble";
            this.CBNullAble.Size = new System.Drawing.Size(48, 16);
            this.CBNullAble.TabIndex = 17;
            this.CBNullAble.Text = "可空";
            this.CBNullAble.UseVisualStyleBackColor = true;
            // 
            // ClumnAddSubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 347);
            this.Controls.Add(this.CBNullAble);
            this.Controls.Add(this.TBDefault);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBAboutDetail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBAbout);
            this.Controls.Add(this.TBScale);
            this.Controls.Add(this.LBScale);
            this.Controls.Add(this.TBPrecison);
            this.Controls.Add(this.LBPrecison);
            this.Controls.Add(this.TBLen);
            this.Controls.Add(this.LBLen);
            this.Controls.Add(this.CBType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBColumnName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClumnAddSubForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加列";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBColumnName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CBType;
        private System.Windows.Forms.Label LBLen;
        private System.Windows.Forms.TextBox TBLen;
        private System.Windows.Forms.Label LBPrecison;
        private System.Windows.Forms.TextBox TBPrecison;
        private System.Windows.Forms.Label LBScale;
        private System.Windows.Forms.TextBox TBScale;
        private System.Windows.Forms.TextBox TBAbout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBAboutDetail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBDefault;
        private System.Windows.Forms.CheckBox CBNullAble;
    }
}