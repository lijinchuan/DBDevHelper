namespace CouchBaseDevHelper.UI
{
    partial class SumFormFindAssembly
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
            this.tbpath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnok = new System.Windows.Forms.Button();
            this.butcanel = new System.Windows.Forms.Button();
            this.lbtext = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbpath
            // 
            this.tbpath.Location = new System.Drawing.Point(27, 49);
            this.tbpath.Name = "tbpath";
            this.tbpath.ReadOnly = true;
            this.tbpath.Size = new System.Drawing.Size(338, 21);
            this.tbpath.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnok
            // 
            this.btnok.Location = new System.Drawing.Point(275, 90);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(75, 22);
            this.btnok.TabIndex = 2;
            this.btnok.Text = "确定";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // butcanel
            // 
            this.butcanel.Location = new System.Drawing.Point(356, 89);
            this.butcanel.Name = "butcanel";
            this.butcanel.Size = new System.Drawing.Size(75, 23);
            this.butcanel.TabIndex = 3;
            this.butcanel.Text = "放弃";
            this.butcanel.UseVisualStyleBackColor = true;
            this.butcanel.Click += new System.EventHandler(this.butcanel_Click);
            // 
            // lbtext
            // 
            this.lbtext.AutoSize = true;
            this.lbtext.Location = new System.Drawing.Point(25, 20);
            this.lbtext.Name = "lbtext";
            this.lbtext.Size = new System.Drawing.Size(41, 12);
            this.lbtext.TabIndex = 4;
            this.lbtext.Text = "label1";
            // 
            // SumFormFindAssembly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 133);
            this.Controls.Add(this.lbtext);
            this.Controls.Add(this.butcanel);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbpath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SumFormFindAssembly";
            this.ShowIcon = false;
            this.Text = "查找程序集";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbpath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnok;
        private System.Windows.Forms.Button butcanel;
        private System.Windows.Forms.Label lbtext;
    }
}