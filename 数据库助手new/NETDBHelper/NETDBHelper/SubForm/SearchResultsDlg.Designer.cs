
namespace NETDBHelper.SubForm
{
    partial class SearchResultsDlg
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
            this.DGVResult = new System.Windows.Forms.DataGridView();
            this.CBDB = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGVResult)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVResult
            // 
            this.DGVResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVResult.Location = new System.Drawing.Point(0, 30);
            this.DGVResult.Name = "DGVResult";
            this.DGVResult.Size = new System.Drawing.Size(800, 420);
            this.DGVResult.TabIndex = 0;
            // 
            // CBDB
            // 
            this.CBDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBDB.FormattingEnabled = true;
            this.CBDB.Location = new System.Drawing.Point(667, 4);
            this.CBDB.Name = "CBDB";
            this.CBDB.Size = new System.Drawing.Size(121, 20);
            this.CBDB.TabIndex = 1;
            // 
            // SearchResultsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CBDB);
            this.Controls.Add(this.DGVResult);
            this.Name = "SearchResultsDlg";
            this.Text = "SearchResultsDlg";
            ((System.ComponentModel.ISupportInitialize)(this.DGVResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVResult;
        private System.Windows.Forms.ComboBox CBDB;
    }
}