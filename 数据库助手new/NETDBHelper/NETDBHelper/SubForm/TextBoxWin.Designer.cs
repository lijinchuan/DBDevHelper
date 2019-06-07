namespace NETDBHelper.SubForm
{
    partial class TextBoxWin
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
            this.components = new System.ComponentModel.Container();
            this.BtnCpy = new System.Windows.Forms.Button();
            this.TBContent = new NETDBHelper.UC.SQLEditBox();
            this.SuspendLayout();
            // 
            // BtnCpy
            // 
            this.BtnCpy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCpy.Location = new System.Drawing.Point(530, 278);
            this.BtnCpy.Name = "BtnCpy";
            this.BtnCpy.Size = new System.Drawing.Size(106, 26);
            this.BtnCpy.TabIndex = 1;
            this.BtnCpy.Text = "复制";
            this.BtnCpy.UseVisualStyleBackColor = true;
            this.BtnCpy.Click += new System.EventHandler(this.BtnCpy_Click);
            // 
            // TBContent
            // 
            this.TBContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBContent.Location = new System.Drawing.Point(12, 12);
            this.TBContent.Name = "TBContent";
            this.TBContent.Size = new System.Drawing.Size(624, 260);
            this.TBContent.TabIndex = 2;
            // 
            // TextBoxWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 316);
            this.Controls.Add(this.TBContent);
            this.Controls.Add(this.BtnCpy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "TextBoxWin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TextBoxWin";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnCpy;
        private UC.SQLEditBox TBContent;
    }
}