namespace RedisHelperUI
{
    partial class SubUpdateForm
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
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBNew = new System.Windows.Forms.TextBox();
            this.TBOld = new System.Windows.Forms.TextBox();
            this.CBNum = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(331, 138);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 0;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(423, 138);
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
            this.label1.Location = new System.Drawing.Point(236, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "===========>";
            // 
            // TBNew
            // 
            this.TBNew.Location = new System.Drawing.Point(313, 33);
            this.TBNew.Multiline = true;
            this.TBNew.Name = "TBNew";
            this.TBNew.Size = new System.Drawing.Size(204, 74);
            this.TBNew.TabIndex = 3;
            // 
            // TBOld
            // 
            this.TBOld.Location = new System.Drawing.Point(22, 33);
            this.TBOld.Multiline = true;
            this.TBOld.Name = "TBOld";
            this.TBOld.Size = new System.Drawing.Size(204, 74);
            this.TBOld.TabIndex = 4;
            // 
            // CBNum
            // 
            this.CBNum.AutoSize = true;
            this.CBNum.Location = new System.Drawing.Point(313, 113);
            this.CBNum.Name = "CBNum";
            this.CBNum.Size = new System.Drawing.Size(48, 16);
            this.CBNum.TabIndex = 5;
            this.CBNum.Text = "数字";
            this.CBNum.UseVisualStyleBackColor = true;
            // 
            // SubUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 189);
            this.Controls.Add(this.CBNum);
            this.Controls.Add(this.TBOld);
            this.Controls.Add(this.TBNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubUpdateForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SubUpdateForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBNew;
        private System.Windows.Forms.TextBox TBOld;
        private System.Windows.Forms.CheckBox CBNum;
    }
}