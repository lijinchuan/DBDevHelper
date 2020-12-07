namespace APIHelper.SubForm
{
    partial class PopMessageDlg
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
            this.TBMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TBMsg
            // 
            this.TBMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBMsg.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TBMsg.Location = new System.Drawing.Point(0, 0);
            this.TBMsg.Multiline = true;
            this.TBMsg.Name = "TBMsg";
            this.TBMsg.ReadOnly = true;
            this.TBMsg.Size = new System.Drawing.Size(331, 174);
            this.TBMsg.TabIndex = 0;
            this.TBMsg.Text = "消息";
            // 
            // PopMessageDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 174);
            this.Controls.Add(this.TBMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopMessageDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "PopMessageDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBMsg;
    }
}