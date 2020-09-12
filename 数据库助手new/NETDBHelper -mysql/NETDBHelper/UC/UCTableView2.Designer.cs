namespace NETDBHelper.UC
{
    partial class UCTableView2
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
            this.CBTables = new System.Windows.Forms.ComboBox();
            this.LBTabname = new System.Windows.Forms.Label();
            this.ColumnsPanel = new System.Windows.Forms.Panel();
            this.CBCoumns = new System.Windows.Forms.ComboBox();
            this.ColumnsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBTables
            // 
            this.CBTables.FormattingEnabled = true;
            this.CBTables.Location = new System.Drawing.Point(3, 3);
            this.CBTables.Name = "CBTables";
            this.CBTables.Size = new System.Drawing.Size(147, 20);
            this.CBTables.TabIndex = 0;
            // 
            // LBTabname
            // 
            this.LBTabname.ForeColor = System.Drawing.Color.Blue;
            this.LBTabname.Location = new System.Drawing.Point(3, 26);
            this.LBTabname.Name = "LBTabname";
            this.LBTabname.Size = new System.Drawing.Size(147, 17);
            this.LBTabname.TabIndex = 1;
            this.LBTabname.Text = "label1";
            // 
            // ColumnsPanel
            // 
            this.ColumnsPanel.Controls.Add(this.CBCoumns);
            this.ColumnsPanel.Location = new System.Drawing.Point(5, 46);
            this.ColumnsPanel.Name = "ColumnsPanel";
            this.ColumnsPanel.Size = new System.Drawing.Size(142, 148);
            this.ColumnsPanel.TabIndex = 2;
            // 
            // CBCoumns
            // 
            this.CBCoumns.FormattingEnabled = true;
            this.CBCoumns.Location = new System.Drawing.Point(3, 3);
            this.CBCoumns.Name = "CBCoumns";
            this.CBCoumns.Size = new System.Drawing.Size(136, 20);
            this.CBCoumns.TabIndex = 0;
            // 
            // UCTableView2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ColumnsPanel);
            this.Controls.Add(this.LBTabname);
            this.Controls.Add(this.CBTables);
            this.Name = "UCTableView2";
            this.Size = new System.Drawing.Size(150, 194);
            this.ColumnsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CBTables;
        private System.Windows.Forms.Label LBTabname;
        private System.Windows.Forms.Panel ColumnsPanel;
        private System.Windows.Forms.ComboBox CBCoumns;
    }
}
