namespace CouchBaseDevHelper.UI.UC
{
    partial class DataViewUC
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.TBKey = new System.Windows.Forms.TextBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TPData = new System.Windows.Forms.TabPage();
            this.TBData = new System.Windows.Forms.TextBox();
            this.TPMsg = new System.Windows.Forms.TabPage();
            this.TBMSG = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBBucket = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.TPData.SuspendLayout();
            this.TPMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TBKey
            // 
            this.TBKey.Location = new System.Drawing.Point(46, 12);
            this.TBKey.Name = "TBKey";
            this.TBKey.Size = new System.Drawing.Size(250, 21);
            this.TBKey.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(576, 12);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 3;
            this.BtnOK.Text = "查询";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.TPData);
            this.tabControl1.Controls.Add(this.TPMsg);
            this.tabControl1.Location = new System.Drawing.Point(0, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(789, 222);
            this.tabControl1.TabIndex = 4;
            // 
            // TPData
            // 
            this.TPData.Controls.Add(this.TBData);
            this.TPData.Location = new System.Drawing.Point(4, 22);
            this.TPData.Name = "TPData";
            this.TPData.Padding = new System.Windows.Forms.Padding(3);
            this.TPData.Size = new System.Drawing.Size(781, 196);
            this.TPData.TabIndex = 0;
            this.TPData.Text = "数据";
            this.TPData.UseVisualStyleBackColor = true;
            // 
            // TBData
            // 
            this.TBData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBData.Location = new System.Drawing.Point(3, 3);
            this.TBData.Multiline = true;
            this.TBData.Name = "TBData";
            this.TBData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBData.Size = new System.Drawing.Size(775, 190);
            this.TBData.TabIndex = 0;
            // 
            // TPMsg
            // 
            this.TPMsg.Controls.Add(this.TBMSG);
            this.TPMsg.Location = new System.Drawing.Point(4, 22);
            this.TPMsg.Name = "TPMsg";
            this.TPMsg.Padding = new System.Windows.Forms.Padding(3);
            this.TPMsg.Size = new System.Drawing.Size(781, 196);
            this.TPMsg.TabIndex = 1;
            this.TPMsg.Text = "消息";
            this.TPMsg.UseVisualStyleBackColor = true;
            // 
            // TBMSG
            // 
            this.TBMSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBMSG.Location = new System.Drawing.Point(3, 3);
            this.TBMSG.Multiline = true;
            this.TBMSG.Name = "TBMSG";
            this.TBMSG.Size = new System.Drawing.Size(775, 190);
            this.TBMSG.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Bucket";
            // 
            // CBBucket
            // 
            this.CBBucket.FormattingEnabled = true;
            this.CBBucket.Location = new System.Drawing.Point(353, 14);
            this.CBBucket.Name = "CBBucket";
            this.CBBucket.Size = new System.Drawing.Size(206, 20);
            this.CBBucket.TabIndex = 6;
            // 
            // DataViewUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CBBucket);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.TBKey);
            this.Controls.Add(this.label1);
            this.Name = "DataViewUC";
            this.Size = new System.Drawing.Size(789, 275);
            this.tabControl1.ResumeLayout(false);
            this.TPData.ResumeLayout(false);
            this.TPData.PerformLayout();
            this.TPMsg.ResumeLayout(false);
            this.TPMsg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBKey;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TPData;
        private System.Windows.Forms.TabPage TPMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CBBucket;
        private System.Windows.Forms.TextBox TBMSG;
        private System.Windows.Forms.TextBox TBData;
    }
}
