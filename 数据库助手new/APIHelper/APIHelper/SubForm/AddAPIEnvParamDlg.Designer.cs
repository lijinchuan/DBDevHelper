namespace APIHelper.SubForm
{
    partial class AddAPIEnvParamDlg
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
            this.DGV = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "变量名称 ";
            // 
            // TBName
            // 
            this.TBName.Location = new System.Drawing.Point(102, 23);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(168, 21);
            this.TBName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "变量值";
            // 
            // DGV
            // 
            this.DGV.BackgroundColor = System.Drawing.Color.White;
            this.DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.DGV.Location = new System.Drawing.Point(104, 65);
            this.DGV.Name = "DGV";
            this.DGV.RowTemplate.Height = 23;
            this.DGV.Size = new System.Drawing.Size(325, 33);
            this.DGV.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-7, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 10);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(261, 269);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 7;
            this.BtnOK.Text = "确定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(368, 269);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 8;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // AddAPIEnvParamDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 297);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAPIEnvParamDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑环境变量";
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
    }
}