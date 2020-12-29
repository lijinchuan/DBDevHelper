namespace APIHelper.SubForm
{
    partial class WatingDlg
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
            this.loadingBox1 = new APIHelper.UC.LoadingBox();
            this.SuspendLayout();
            // 
            // loadingBox1
            // 
            this.loadingBox1.BackColor = System.Drawing.Color.Transparent;
            this.loadingBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loadingBox1.Location = new System.Drawing.Point(10, 9);
            this.loadingBox1.Name = "loadingBox1";
            this.loadingBox1.Size = new System.Drawing.Size(244, 96);
            this.loadingBox1.TabIndex = 0;
            // 
            // WatingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 117);
            this.ControlBox = false;
            this.Controls.Add(this.loadingBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WatingDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WatingDlg";
            this.ResumeLayout(false);

        }

        #endregion

        private UC.LoadingBox loadingBox1;
    }
}