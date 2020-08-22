namespace NETDBHelper.UC
{
    partial class UCTableView
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
            this.DGVColumns = new System.Windows.Forms.DataGridView();
            this.LBTabname = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGVColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // CBTables
            // 
            this.CBTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.CBTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CBTables.FormattingEnabled = true;
            this.CBTables.Location = new System.Drawing.Point(0, 0);
            this.CBTables.Name = "CBTables";
            this.CBTables.Size = new System.Drawing.Size(150, 20);
            this.CBTables.TabIndex = 0;
            // 
            // DGVColumns
            // 
            this.DGVColumns.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVColumns.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVColumns.GridColor = System.Drawing.SystemColors.Control;
            this.DGVColumns.Location = new System.Drawing.Point(0, 38);
            this.DGVColumns.MultiSelect = false;
            this.DGVColumns.Name = "DGVColumns";
            this.DGVColumns.RowHeadersVisible = false;
            this.DGVColumns.RowTemplate.Height = 23;
            this.DGVColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVColumns.ShowEditingIcon = false;
            this.DGVColumns.ShowRowErrors = false;
            this.DGVColumns.Size = new System.Drawing.Size(150, 107);
            this.DGVColumns.TabIndex = 1;
            // 
            // LBTabname
            // 
            this.LBTabname.AutoSize = true;
            this.LBTabname.Location = new System.Drawing.Point(3, 23);
            this.LBTabname.Name = "LBTabname";
            this.LBTabname.Size = new System.Drawing.Size(41, 12);
            this.LBTabname.TabIndex = 2;
            this.LBTabname.Text = "label1";
            // 
            // UCTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LBTabname);
            this.Controls.Add(this.DGVColumns);
            this.Controls.Add(this.CBTables);
            this.Name = "UCTableView";
            ((System.ComponentModel.ISupportInitialize)(this.DGVColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CBTables;
        private System.Windows.Forms.DataGridView DGVColumns;
        private System.Windows.Forms.Label LBTabname;
    }
}
