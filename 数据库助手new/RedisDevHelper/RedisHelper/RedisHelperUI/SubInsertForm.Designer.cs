namespace RedisHelperUI
{
    partial class SubInsertForm
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
            this.LabKey = new System.Windows.Forms.Label();
            this.TBKey = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabKey
            // 
            this.LabKey.AutoSize = true;
            this.LabKey.Location = new System.Drawing.Point(38, 21);
            this.LabKey.Name = "LabKey";
            this.LabKey.Size = new System.Drawing.Size(23, 12);
            this.LabKey.TabIndex = 0;
            this.LabKey.Text = "key";
            this.LabKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TBKey
            // 
            this.TBKey.Location = new System.Drawing.Point(67, 18);
            this.TBKey.Name = "TBKey";
            this.TBKey.Size = new System.Drawing.Size(292, 21);
            this.TBKey.TabIndex = 1;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(215, 182);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(84, 28);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(305, 182);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 26);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SubInsertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 254);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.TBKey);
            this.Controls.Add(this.LabKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubInsertForm";
            this.ShowIcon = false;
            this.Text = "添加";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabKey;
        private System.Windows.Forms.TextBox TBKey;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
    }
}