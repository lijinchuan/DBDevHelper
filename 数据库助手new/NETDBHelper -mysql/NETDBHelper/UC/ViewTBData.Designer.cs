namespace NETDBHelper.UC
{
    partial class ViewTBData
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dv_Data = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tb_Msg = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_DelItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CopyValue = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CopyColumnName = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_sql = new NETDBHelper.UC.EditTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dv_Data)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dv_Data
            // 
            this.dv_Data.AllowUserToAddRows = false;
            this.dv_Data.AllowUserToDeleteRows = false;
            this.dv_Data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dv_Data.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dv_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dv_Data.Location = new System.Drawing.Point(3, 3);
            this.dv_Data.Name = "dv_Data";
            this.dv_Data.ReadOnly = true;
            this.dv_Data.RowTemplate.Height = 23;
            this.dv_Data.Size = new System.Drawing.Size(803, 212);
            this.dv_Data.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 325);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 244);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dv_Data);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(809, 218);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tb_Msg);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(809, 218);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "消息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tb_Msg
            // 
            this.tb_Msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Msg.Location = new System.Drawing.Point(3, 3);
            this.tb_Msg.Multiline = true;
            this.tb_Msg.Name = "tb_Msg";
            this.tb_Msg.ReadOnly = true;
            this.tb_Msg.Size = new System.Drawing.Size(803, 212);
            this.tb_Msg.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_CopyValue,
            this.MenuItem_CopyColumnName,
            this.toolStripSeparator1,
            this.MenuItem_DelItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 70);
            // 
            // MenuItem_DelItem
            // 
            this.MenuItem_DelItem.Name = "MenuItem_DelItem";
            this.MenuItem_DelItem.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_DelItem.Text = "删除记录";
            this.MenuItem_DelItem.Click += new System.EventHandler(this.MenuItem_DelItem_Click);
            // 
            // MenuItem_CopyValue
            // 
            this.MenuItem_CopyValue.Name = "MenuItem_CopyValue";
            this.MenuItem_CopyValue.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_CopyValue.Text = "复制值";
            // 
            // MenuItem_CopyColumnName
            // 
            this.MenuItem_CopyColumnName.Name = "MenuItem_CopyColumnName";
            this.MenuItem_CopyColumnName.Size = new System.Drawing.Size(136, 22);
            this.MenuItem_CopyColumnName.Text = "复制此列名";
            // 
            // tb_sql
            // 
            this.tb_sql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_sql.Location = new System.Drawing.Point(3, 0);
            this.tb_sql.Name = "tb_sql";
            this.tb_sql.Size = new System.Drawing.Size(842, 319);
            this.tb_sql.TabIndex = 3;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // ViewTBData
            // 
            this.Controls.Add(this.tb_sql);
            this.Controls.Add(this.tabControl1);
            this.Size = new System.Drawing.Size(845, 572);
            ((System.ComponentModel.ISupportInitialize)(this.dv_Data)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dv_Data;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tb_Msg;
        private EditTextBox tb_sql;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_DelItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyValue;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyColumnName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
