
namespace NETDBHelper.SubForm
{
    partial class AddTempTableDlg
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
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBDisplayTableName = new System.Windows.Forms.TextBox();
            this.PannelColumns = new System.Windows.Forms.Panel();
            this.PannelBox = new System.Windows.Forms.Panel();
            this.PBRem = new System.Windows.Forms.PictureBox();
            this.PBAdd = new System.Windows.Forms.PictureBox();
            this.TBColName = new System.Windows.Forms.TextBox();
            this.CBType = new System.Windows.Forms.ComboBox();
            this.PannelColumns.SuspendLayout();
            this.PannelBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBRem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-7, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 10);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(229, 226);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(319, 226);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "表名：";
            // 
            // TBDisplayTableName
            // 
            this.TBDisplayTableName.Location = new System.Drawing.Point(59, 6);
            this.TBDisplayTableName.Name = "TBDisplayTableName";
            this.TBDisplayTableName.Size = new System.Drawing.Size(167, 21);
            this.TBDisplayTableName.TabIndex = 5;
            // 
            // PannelColumns
            // 
            this.PannelColumns.AutoScroll = true;
            this.PannelColumns.Controls.Add(this.PannelBox);
            this.PannelColumns.Location = new System.Drawing.Point(2, 33);
            this.PannelColumns.Name = "PannelColumns";
            this.PannelColumns.Size = new System.Drawing.Size(423, 171);
            this.PannelColumns.TabIndex = 6;
            // 
            // PannelBox
            // 
            this.PannelBox.Controls.Add(this.PBRem);
            this.PannelBox.Controls.Add(this.PBAdd);
            this.PannelBox.Controls.Add(this.TBColName);
            this.PannelBox.Controls.Add(this.CBType);
            this.PannelBox.Location = new System.Drawing.Point(3, 3);
            this.PannelBox.Name = "PannelBox";
            this.PannelBox.Size = new System.Drawing.Size(408, 23);
            this.PannelBox.TabIndex = 4;
            // 
            // PBRem
            // 
            this.PBRem.Location = new System.Drawing.Point(381, 2);
            this.PBRem.Name = "PBRem";
            this.PBRem.Size = new System.Drawing.Size(20, 20);
            this.PBRem.TabIndex = 3;
            this.PBRem.TabStop = false;
            // 
            // PBAdd
            // 
            this.PBAdd.Location = new System.Drawing.Point(353, 2);
            this.PBAdd.Name = "PBAdd";
            this.PBAdd.Size = new System.Drawing.Size(20, 20);
            this.PBAdd.TabIndex = 2;
            this.PBAdd.TabStop = false;
            // 
            // TBColName
            // 
            this.TBColName.Location = new System.Drawing.Point(98, 1);
            this.TBColName.Name = "TBColName";
            this.TBColName.Size = new System.Drawing.Size(249, 21);
            this.TBColName.TabIndex = 1;
            // 
            // CBType
            // 
            this.CBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBType.FormattingEnabled = true;
            this.CBType.Location = new System.Drawing.Point(3, 2);
            this.CBType.Name = "CBType";
            this.CBType.Size = new System.Drawing.Size(89, 20);
            this.CBType.TabIndex = 0;
            // 
            // AddTempTableDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 257);
            this.Controls.Add(this.PannelColumns);
            this.Controls.Add(this.TBDisplayTableName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddTempTableDlg";
            this.Text = "添加临时表";
            this.PannelColumns.ResumeLayout(false);
            this.PannelBox.ResumeLayout(false);
            this.PannelBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBRem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBDisplayTableName;
        private System.Windows.Forms.Panel PannelColumns;
        private System.Windows.Forms.Panel PannelBox;
        private System.Windows.Forms.PictureBox PBRem;
        private System.Windows.Forms.PictureBox PBAdd;
        private System.Windows.Forms.TextBox TBColName;
        private System.Windows.Forms.ComboBox CBType;
    }
}