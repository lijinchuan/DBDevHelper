namespace NETDBHelper.UC
{
    partial class UCTextBox
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
            this.components = new System.ComponentModel.Container();
            this.CBNull = new System.Windows.Forms.CheckBox();
            this.TBValue = new NETDBHelper.UC.AdjustTextBox(this.components);
            this.SuspendLayout();
            // 
            // CBNull
            // 
            this.CBNull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CBNull.AutoSize = true;
            this.CBNull.Location = new System.Drawing.Point(123, 5);
            this.CBNull.Name = "CBNull";
            this.CBNull.Size = new System.Drawing.Size(15, 14);
            this.CBNull.TabIndex = 1;
            this.CBNull.UseVisualStyleBackColor = true;
            // 
            // TBValue
            // 
            this.TBValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBValue.DrawPrompt = null;
            this.TBValue.Location = new System.Drawing.Point(0, 1);
            this.TBValue.Name = "TBValue";
            this.TBValue.Size = new System.Drawing.Size(120, 21);
            this.TBValue.TabIndex = 2;
            // 
            // UCTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TBValue);
            this.Controls.Add(this.CBNull);
            this.Name = "UCTextBox";
            this.Size = new System.Drawing.Size(141, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox CBNull;
        private AdjustTextBox TBValue;
    }
}
