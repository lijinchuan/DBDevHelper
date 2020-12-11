namespace APIHelper.UC.Auth
{
    partial class UCApiKey
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TBKey = new System.Windows.Forms.TextBox();
            this.TBValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CBAddTo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "键:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "值:";
            // 
            // TBKey
            // 
            this.TBKey.Location = new System.Drawing.Point(84, 28);
            this.TBKey.Name = "TBKey";
            this.TBKey.Size = new System.Drawing.Size(214, 21);
            this.TBKey.TabIndex = 2;
            // 
            // TBValue
            // 
            this.TBValue.Location = new System.Drawing.Point(84, 69);
            this.TBValue.Name = "TBValue";
            this.TBValue.Size = new System.Drawing.Size(214, 21);
            this.TBValue.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "添加到:";
            // 
            // CBAddTo
            // 
            this.CBAddTo.FormattingEnabled = true;
            this.CBAddTo.Location = new System.Drawing.Point(84, 114);
            this.CBAddTo.Name = "CBAddTo";
            this.CBAddTo.Size = new System.Drawing.Size(121, 20);
            this.CBAddTo.TabIndex = 5;
            // 
            // UCApiKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CBAddTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBValue);
            this.Controls.Add(this.TBKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UCApiKey";
            this.Size = new System.Drawing.Size(367, 206);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBKey;
        private System.Windows.Forms.TextBox TBValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBAddTo;
    }
}
