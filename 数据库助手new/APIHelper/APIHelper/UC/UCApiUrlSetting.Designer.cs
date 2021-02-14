namespace APIHelper.UC
{
    partial class UCApiUrlSetting
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
            this.TBTimeOut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBSaveResp = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CBNoproxy = new System.Windows.Forms.CheckBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "超时时间（秒）：";
            // 
            // TBTimeOut
            // 
            this.TBTimeOut.Location = new System.Drawing.Point(154, 23);
            this.TBTimeOut.Name = "TBTimeOut";
            this.TBTimeOut.Size = new System.Drawing.Size(100, 21);
            this.TBTimeOut.TabIndex = 1;
            this.TBTimeOut.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "保存结果：";
            // 
            // CBSaveResp
            // 
            this.CBSaveResp.AutoSize = true;
            this.CBSaveResp.Checked = true;
            this.CBSaveResp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBSaveResp.Location = new System.Drawing.Point(155, 62);
            this.CBSaveResp.Name = "CBSaveResp";
            this.CBSaveResp.Size = new System.Drawing.Size(15, 14);
            this.CBSaveResp.TabIndex = 3;
            this.CBSaveResp.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "不使用代理服务器：";
            // 
            // CBNoproxy
            // 
            this.CBNoproxy.AutoSize = true;
            this.CBNoproxy.Checked = true;
            this.CBNoproxy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBNoproxy.Location = new System.Drawing.Point(155, 96);
            this.CBNoproxy.Name = "CBNoproxy";
            this.CBNoproxy.Size = new System.Drawing.Size(15, 14);
            this.CBNoproxy.TabIndex = 5;
            this.CBNoproxy.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(422, 270);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 6;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // UCApiUrlSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.CBNoproxy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CBSaveResp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBTimeOut);
            this.Controls.Add(this.label1);
            this.Name = "UCApiUrlSetting";
            this.Size = new System.Drawing.Size(539, 324);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBTimeOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CBSaveResp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CBNoproxy;
        private System.Windows.Forms.Button BtnSave;
    }
}
