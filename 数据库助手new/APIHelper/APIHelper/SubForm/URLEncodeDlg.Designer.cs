namespace APIHelper.SubForm
{
    partial class URLEncodeDlg
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
            this.TBEncode = new System.Windows.Forms.TextBox();
            this.BtnEncode = new System.Windows.Forms.Button();
            this.BtnDecode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBDecode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TBEncode
            // 
            this.TBEncode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBEncode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TBEncode.Location = new System.Drawing.Point(25, 261);
            this.TBEncode.Multiline = true;
            this.TBEncode.Name = "TBEncode";
            this.TBEncode.Size = new System.Drawing.Size(736, 220);
            this.TBEncode.TabIndex = 0;
            // 
            // BtnEncode
            // 
            this.BtnEncode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BtnEncode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEncode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnEncode.Location = new System.Drawing.Point(588, 230);
            this.BtnEncode.Name = "BtnEncode";
            this.BtnEncode.Size = new System.Drawing.Size(75, 25);
            this.BtnEncode.TabIndex = 1;
            this.BtnEncode.Text = "Encode";
            this.BtnEncode.UseVisualStyleBackColor = false;
            this.BtnEncode.Click += new System.EventHandler(this.BtnEncode_Click);
            // 
            // BtnDecode
            // 
            this.BtnDecode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BtnDecode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDecode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnDecode.Location = new System.Drawing.Point(669, 230);
            this.BtnDecode.Name = "BtnDecode";
            this.BtnDecode.Size = new System.Drawing.Size(75, 25);
            this.BtnDecode.TabIndex = 3;
            this.BtnDecode.Text = "DeCode";
            this.BtnDecode.UseVisualStyleBackColor = false;
            this.BtnDecode.Click += new System.EventHandler(this.BtnDecode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(23, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(329, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Encode上方输入下方输出结果，Decode下方输入上方输出结果";
            // 
            // TBDecode
            // 
            this.TBDecode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBDecode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TBDecode.Location = new System.Drawing.Point(25, 12);
            this.TBDecode.Multiline = true;
            this.TBDecode.Name = "TBDecode";
            this.TBDecode.Size = new System.Drawing.Size(736, 212);
            this.TBDecode.TabIndex = 5;
            this.TBDecode.TextChanged += new System.EventHandler(this.TBDecode_TextChanged);
            // 
            // URLEncodeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 493);
            this.Controls.Add(this.TBDecode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnDecode);
            this.Controls.Add(this.BtnEncode);
            this.Controls.Add(this.TBEncode);
            this.Name = "URLEncodeDlg";
            this.Text = "URLEncode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBEncode;
        private System.Windows.Forms.Button BtnEncode;
        private System.Windows.Forms.Button BtnDecode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBDecode;
    }
}