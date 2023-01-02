
namespace NETDBHelper.SubForm
{
    partial class UpSertDlg
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
            this.GBValues = new System.Windows.Forms.GroupBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GBValues
            // 
            this.GBValues.Location = new System.Drawing.Point(12, 12);
            this.GBValues.Name = "GBValues";
            this.GBValues.Size = new System.Drawing.Size(776, 397);
            this.GBValues.TabIndex = 0;
            this.GBValues.TabStop = false;
            this.GBValues.Text = "编辑值";
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(659, 420);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(58, 23);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(729, 419);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(59, 23);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            // 
            // UpSertDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.GBValues);
            this.Name = "UpSertDlg";
            this.Text = "UpSertDlg";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GBValues;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
    }
}