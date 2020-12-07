namespace APIHelper.UC
{
    partial class LoadingBox
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
            this.lb1 = new System.Windows.Forms.Label();
            this.LlbStop = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(107, 35);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(101, 12);
            this.lb1.TabIndex = 0;
            this.lb1.Text = "正在执行中......";
            // 
            // LlbStop
            // 
            this.LlbStop.AutoSize = true;
            this.LlbStop.Location = new System.Drawing.Point(107, 59);
            this.LlbStop.Name = "LlbStop";
            this.LlbStop.Size = new System.Drawing.Size(29, 12);
            this.LlbStop.TabIndex = 1;
            this.LlbStop.TabStop = true;
            this.LlbStop.Text = "停止";
            this.LlbStop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LlbStop_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(29, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // LoadingBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LlbStop);
            this.Controls.Add(this.lb1);
            this.Name = "LoadingBox";
            this.Size = new System.Drawing.Size(244, 96);
            this.Load += new System.EventHandler(this.LoadingBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.LinkLabel LlbStop;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
