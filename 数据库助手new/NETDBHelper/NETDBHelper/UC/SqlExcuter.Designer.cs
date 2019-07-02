namespace NETDBHelper.UC
{
    partial class SqlExcuter
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
            this.sqlEditBox1 = new NETDBHelper.UC.SQLEditBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TPInfo = new System.Windows.Forms.TabPage();
            this.TBInfo = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.TPInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // sqlEditBox1
            // 
            this.sqlEditBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sqlEditBox1.DBName = null;
            this.sqlEditBox1.Location = new System.Drawing.Point(8, 3);
            this.sqlEditBox1.Name = "sqlEditBox1";
            this.sqlEditBox1.Size = new System.Drawing.Size(581, 176);
            this.sqlEditBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TPInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 185);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(596, 245);
            this.tabControl1.TabIndex = 2;
            // 
            // TPInfo
            // 
            this.TPInfo.Controls.Add(this.TBInfo);
            this.TPInfo.Location = new System.Drawing.Point(4, 22);
            this.TPInfo.Name = "TPInfo";
            this.TPInfo.Padding = new System.Windows.Forms.Padding(3);
            this.TPInfo.Size = new System.Drawing.Size(588, 219);
            this.TPInfo.TabIndex = 1;
            this.TPInfo.Text = "信息";
            this.TPInfo.UseVisualStyleBackColor = true;
            // 
            // TBInfo
            // 
            this.TBInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBInfo.Location = new System.Drawing.Point(3, 3);
            this.TBInfo.Multiline = true;
            this.TBInfo.Name = "TBInfo";
            this.TBInfo.ReadOnly = true;
            this.TBInfo.Size = new System.Drawing.Size(582, 213);
            this.TBInfo.TabIndex = 0;
            // 
            // SqlExcuter
            // 
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.sqlEditBox1);
            this.Name = "SqlExcuter";
            this.Size = new System.Drawing.Size(596, 430);
            this.tabControl1.ResumeLayout(false);
            this.TPInfo.ResumeLayout(false);
            this.TPInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SQLEditBox sqlEditBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TPInfo;
        private System.Windows.Forms.TextBox TBInfo;
    }
}
