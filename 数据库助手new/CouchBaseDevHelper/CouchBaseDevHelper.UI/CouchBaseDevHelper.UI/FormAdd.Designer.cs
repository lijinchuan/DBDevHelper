namespace CouchBaseDevHelper.UI
{
    partial class FormAdd
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
            this.TBKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBBucket = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBVal = new System.Windows.Forms.TextBox();
            this.Btnok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "key:";
            // 
            // TBKey
            // 
            this.TBKey.Location = new System.Drawing.Point(62, 6);
            this.TBKey.Name = "TBKey";
            this.TBKey.Size = new System.Drawing.Size(464, 21);
            this.TBKey.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "bucket:";
            // 
            // CBBucket
            // 
            this.CBBucket.FormattingEnabled = true;
            this.CBBucket.Location = new System.Drawing.Point(64, 52);
            this.CBBucket.Name = "CBBucket";
            this.CBBucket.Size = new System.Drawing.Size(186, 20);
            this.CBBucket.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "value:";
            // 
            // TBVal
            // 
            this.TBVal.Location = new System.Drawing.Point(64, 106);
            this.TBVal.Multiline = true;
            this.TBVal.Name = "TBVal";
            this.TBVal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBVal.Size = new System.Drawing.Size(788, 336);
            this.TBVal.TabIndex = 5;
            // 
            // Btnok
            // 
            this.Btnok.Location = new System.Drawing.Point(758, 460);
            this.Btnok.Name = "Btnok";
            this.Btnok.Size = new System.Drawing.Size(75, 23);
            this.Btnok.TabIndex = 6;
            this.Btnok.Text = "保存";
            this.Btnok.UseVisualStyleBackColor = true;
            this.Btnok.Click += new System.EventHandler(this.Btnok_Click);
            // 
            // FormAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 495);
            this.Controls.Add(this.Btnok);
            this.Controls.Add(this.TBVal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CBBucket);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBKey);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAdd";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加键";
            this.Load += new System.EventHandler(this.FormAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CBBucket;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBVal;
        private System.Windows.Forms.Button Btnok;
    }
}