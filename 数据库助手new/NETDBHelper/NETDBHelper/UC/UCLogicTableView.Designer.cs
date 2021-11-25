namespace NETDBHelper.UC
{
    partial class UCLogicTableView
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
            this.CBCoumns = new System.Windows.Forms.ComboBox();
            this.ColumnsPanel = new System.Windows.Forms.Panel();
            this.LBTabname = new System.Windows.Forms.Label();
            this.CBTables = new System.Windows.Forms.ComboBox();
            this.ColumnsPanelOutPut = new System.Windows.Forms.Panel();
            this.CBCoumnsOutput = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ColumnsPanel.SuspendLayout();
            this.ColumnsPanelOutPut.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBCoumns
            // 
            this.CBCoumns.FormattingEnabled = true;
            this.CBCoumns.Location = new System.Drawing.Point(1, 1);
            this.CBCoumns.Name = "CBCoumns";
            this.CBCoumns.Size = new System.Drawing.Size(136, 20);
            this.CBCoumns.TabIndex = 0;
            // 
            // ColumnsPanel
            // 
            this.ColumnsPanel.Controls.Add(this.CBCoumns);
            this.ColumnsPanel.Location = new System.Drawing.Point(2, 43);
            this.ColumnsPanel.Name = "ColumnsPanel";
            this.ColumnsPanel.Size = new System.Drawing.Size(142, 78);
            this.ColumnsPanel.TabIndex = 5;
            // 
            // LBTabname
            // 
            this.LBTabname.ForeColor = System.Drawing.Color.Blue;
            this.LBTabname.Location = new System.Drawing.Point(0, 23);
            this.LBTabname.Name = "LBTabname";
            this.LBTabname.Size = new System.Drawing.Size(147, 17);
            this.LBTabname.TabIndex = 4;
            this.LBTabname.Text = "label1";
            // 
            // CBTables
            // 
            this.CBTables.FormattingEnabled = true;
            this.CBTables.Location = new System.Drawing.Point(0, 0);
            this.CBTables.Name = "CBTables";
            this.CBTables.Size = new System.Drawing.Size(147, 20);
            this.CBTables.TabIndex = 3;
            // 
            // ColumnsPanelOutPut
            // 
            this.ColumnsPanelOutPut.Controls.Add(this.CBCoumnsOutput);
            this.ColumnsPanelOutPut.Location = new System.Drawing.Point(1, 133);
            this.ColumnsPanelOutPut.Name = "ColumnsPanelOutPut";
            this.ColumnsPanelOutPut.Size = new System.Drawing.Size(142, 81);
            this.ColumnsPanelOutPut.TabIndex = 6;
            // 
            // CBCoumnsOutput
            // 
            this.CBCoumnsOutput.FormattingEnabled = true;
            this.CBCoumnsOutput.Location = new System.Drawing.Point(1, 1);
            this.CBCoumnsOutput.Name = "CBCoumnsOutput";
            this.CBCoumnsOutput.Size = new System.Drawing.Size(136, 20);
            this.CBCoumnsOutput.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SaddleBrown;
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(2, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 1);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // UCLogicTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ColumnsPanelOutPut);
            this.Controls.Add(this.ColumnsPanel);
            this.Controls.Add(this.LBTabname);
            this.Controls.Add(this.CBTables);
            this.Name = "UCLogicTableView";
            this.Size = new System.Drawing.Size(147, 216);
            this.ColumnsPanel.ResumeLayout(false);
            this.ColumnsPanelOutPut.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CBCoumns;
        private System.Windows.Forms.Panel ColumnsPanel;
        private System.Windows.Forms.Label LBTabname;
        private System.Windows.Forms.ComboBox CBTables;
        private System.Windows.Forms.Panel ColumnsPanelOutPut;
        private System.Windows.Forms.ComboBox CBCoumnsOutput;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
