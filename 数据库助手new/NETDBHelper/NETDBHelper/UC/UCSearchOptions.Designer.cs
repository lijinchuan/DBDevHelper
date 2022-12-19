namespace NETDBHelper.UC
{
    partial class UCSearchOptions
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
            this.CBMatchAll = new System.Windows.Forms.CheckBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CBMatchAll
            // 
            this.CBMatchAll.AutoSize = true;
            this.CBMatchAll.Location = new System.Drawing.Point(14, 15);
            this.CBMatchAll.Name = "CBMatchAll";
            this.CBMatchAll.Size = new System.Drawing.Size(72, 16);
            this.CBMatchAll.TabIndex = 0;
            this.CBMatchAll.Text = "完全匹配";
            this.CBMatchAll.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(98, 58);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(38, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // UCSearchOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.CBMatchAll);
            this.Name = "UCSearchOptions";
            this.Size = new System.Drawing.Size(150, 89);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CBMatchAll;
        private System.Windows.Forms.Button BtnOk;
    }
}
