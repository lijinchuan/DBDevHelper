namespace APIHelper.UC
{
    partial class UPAPIParamTable
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
            this.DGVRequest = new System.Windows.Forms.DataGridView();
            this.BtnMultiAddReqParams = new System.Windows.Forms.Button();
            this.BtnReqParamUp = new System.Windows.Forms.Button();
            this.BtnReqParamDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVRequest)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVRequest
            // 
            this.DGVRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVRequest.BackgroundColor = System.Drawing.Color.White;
            this.DGVRequest.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVRequest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVRequest.Location = new System.Drawing.Point(3, 3);
            this.DGVRequest.Name = "DGVRequest";
            this.DGVRequest.RowTemplate.Height = 23;
            this.DGVRequest.Size = new System.Drawing.Size(520, 162);
            this.DGVRequest.TabIndex = 1;
            // 
            // BtnMultiAddReqParams
            // 
            this.BtnMultiAddReqParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMultiAddReqParams.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnMultiAddReqParams.Image = global::APIHelper.Properties.Resources.text_columns;
            this.BtnMultiAddReqParams.Location = new System.Drawing.Point(534, 26);
            this.BtnMultiAddReqParams.Name = "BtnMultiAddReqParams";
            this.BtnMultiAddReqParams.Size = new System.Drawing.Size(30, 23);
            this.BtnMultiAddReqParams.TabIndex = 6;
            this.BtnMultiAddReqParams.UseVisualStyleBackColor = true;
            this.BtnMultiAddReqParams.Click += new System.EventHandler(this.BtnMultiAddReqParams_Click);
            // 
            // BtnReqParamUp
            // 
            this.BtnReqParamUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnReqParamUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnReqParamUp.Image = global::APIHelper.Properties.Resources.arrow_up;
            this.BtnReqParamUp.Location = new System.Drawing.Point(534, 63);
            this.BtnReqParamUp.Name = "BtnReqParamUp";
            this.BtnReqParamUp.Size = new System.Drawing.Size(30, 23);
            this.BtnReqParamUp.TabIndex = 5;
            this.BtnReqParamUp.UseVisualStyleBackColor = true;
            this.BtnReqParamUp.Click += new System.EventHandler(this.BtnReqParamUp_Click);
            // 
            // BtnReqParamDown
            // 
            this.BtnReqParamDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnReqParamDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnReqParamDown.Image = global::APIHelper.Properties.Resources.arrow_down;
            this.BtnReqParamDown.Location = new System.Drawing.Point(534, 100);
            this.BtnReqParamDown.Name = "BtnReqParamDown";
            this.BtnReqParamDown.Size = new System.Drawing.Size(30, 23);
            this.BtnReqParamDown.TabIndex = 4;
            this.BtnReqParamDown.UseVisualStyleBackColor = true;
            this.BtnReqParamDown.Click += new System.EventHandler(this.BtnReqParamDown_Click);
            // 
            // UPAPIParamTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnMultiAddReqParams);
            this.Controls.Add(this.BtnReqParamUp);
            this.Controls.Add(this.BtnReqParamDown);
            this.Controls.Add(this.DGVRequest);
            this.Name = "UPAPIParamTable";
            this.Size = new System.Drawing.Size(578, 168);
            ((System.ComponentModel.ISupportInitialize)(this.DGVRequest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVRequest;
        private System.Windows.Forms.Button BtnMultiAddReqParams;
        private System.Windows.Forms.Button BtnReqParamUp;
        private System.Windows.Forms.Button BtnReqParamDown;
    }
}
