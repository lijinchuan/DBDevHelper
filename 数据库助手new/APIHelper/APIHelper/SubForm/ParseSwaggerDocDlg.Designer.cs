namespace APIHelper.SubForm
{
    partial class ParseSwaggerDocDlg
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
            this.TBInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBOutPut = new System.Windows.Forms.TextBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.CBIsResp = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(37, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入：";
            // 
            // TBInput
            // 
            this.TBInput.Location = new System.Drawing.Point(84, 27);
            this.TBInput.Multiline = true;
            this.TBInput.Name = "TBInput";
            this.TBInput.Size = new System.Drawing.Size(442, 173);
            this.TBInput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(37, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "结果：";
            // 
            // TBOutPut
            // 
            this.TBOutPut.Location = new System.Drawing.Point(84, 239);
            this.TBOutPut.Multiline = true;
            this.TBOutPut.Name = "TBOutPut";
            this.TBOutPut.Size = new System.Drawing.Size(442, 188);
            this.TBOutPut.TabIndex = 3;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(185, 206);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 4;
            this.BtnOK.Text = "确定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // CBIsResp
            // 
            this.CBIsResp.AutoSize = true;
            this.CBIsResp.Location = new System.Drawing.Point(84, 210);
            this.CBIsResp.Name = "CBIsResp";
            this.CBIsResp.Size = new System.Drawing.Size(84, 16);
            this.CBIsResp.TabIndex = 5;
            this.CBIsResp.Text = "是否是响应";
            this.CBIsResp.UseVisualStyleBackColor = true;
            // 
            // ParseSwaggerDocDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::APIHelper.Properties.Resources.SwaggerApi;
            this.ClientSize = new System.Drawing.Size(554, 450);
            this.Controls.Add(this.CBIsResp);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.TBOutPut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBInput);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParseSwaggerDocDlg";
            this.ShowIcon = false;
            this.Text = "Swagger文档转换工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBOutPut;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.CheckBox CBIsResp;
    }
}