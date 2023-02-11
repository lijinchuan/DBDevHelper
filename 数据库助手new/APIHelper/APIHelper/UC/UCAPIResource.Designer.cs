
namespace APIHelper.UC
{
    partial class UCAPIResource
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
            this.TBFilePath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.LBMsg = new System.Windows.Forms.Label();
            this.PBButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBButton)).BeginInit();
            this.SuspendLayout();
            // 
            // TBFilePath
            // 
            this.TBFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBFilePath.Location = new System.Drawing.Point(17, 18);
            this.TBFilePath.Name = "TBFilePath";
            this.TBFilePath.Size = new System.Drawing.Size(278, 21);
            this.TBFilePath.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(297, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LBMsg
            // 
            this.LBMsg.AutoSize = true;
            this.LBMsg.ForeColor = System.Drawing.Color.Red;
            this.LBMsg.Location = new System.Drawing.Point(99, 54);
            this.LBMsg.Name = "LBMsg";
            this.LBMsg.Size = new System.Drawing.Size(0, 12);
            this.LBMsg.TabIndex = 3;
            // 
            // PBButton
            // 
            this.PBButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PBButton.Location = new System.Drawing.Point(119, 68);
            this.PBButton.Name = "PBButton";
            this.PBButton.Size = new System.Drawing.Size(100, 92);
            this.PBButton.TabIndex = 4;
            this.PBButton.TabStop = false;
            this.PBButton.Click += new System.EventHandler(this.PBButton_Click);
            // 
            // UCAPIResource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PBButton);
            this.Controls.Add(this.LBMsg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TBFilePath);
            this.Name = "UCAPIResource";
            this.Size = new System.Drawing.Size(378, 202);
            ((System.ComponentModel.ISupportInitialize)(this.PBButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBFilePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LBMsg;
        private System.Windows.Forms.PictureBox PBButton;
    }
}
