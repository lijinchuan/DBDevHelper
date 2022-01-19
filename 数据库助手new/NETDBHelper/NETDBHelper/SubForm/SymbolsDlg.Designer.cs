namespace NETDBHelper.SubForm
{
    partial class SymbolsDlg
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
            this.WBSymbols = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // WBSymbols
            // 
            this.WBSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WBSymbols.Location = new System.Drawing.Point(0, 0);
            this.WBSymbols.MinimumSize = new System.Drawing.Size(20, 20);
            this.WBSymbols.Name = "WBSymbols";
            this.WBSymbols.Size = new System.Drawing.Size(800, 450);
            this.WBSymbols.TabIndex = 0;
            // 
            // SymbolsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WBSymbols);
            this.Name = "SymbolsDlg";
            this.Text = "常用符号大全";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WBSymbols;
    }
}