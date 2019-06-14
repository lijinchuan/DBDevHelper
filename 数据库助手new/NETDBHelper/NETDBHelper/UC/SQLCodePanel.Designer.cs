namespace NETDBHelper.UC
{
    partial class SQLCodePanel
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
            this.SuspendLayout();
            // 
            // sqlEditBox1
            // 
            this.sqlEditBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlEditBox1.Location = new System.Drawing.Point(0, 0);
            this.sqlEditBox1.Name = "sqlEditBox1";
            this.sqlEditBox1.Size = new System.Drawing.Size(467, 414);
            this.sqlEditBox1.TabIndex = 0;
            // 
            // SQLCodePanel
            // 
            this.Controls.Add(this.sqlEditBox1);
            this.Name = "SQLCodePanel";
            this.Size = new System.Drawing.Size(467, 414);
            this.ResumeLayout(false);

        }

        #endregion

        private SQLEditBox sqlEditBox1;
    }
}
