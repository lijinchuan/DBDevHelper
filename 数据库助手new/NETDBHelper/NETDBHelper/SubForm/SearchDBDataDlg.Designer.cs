
namespace NETDBHelper.SubForm
{
    partial class SearchDBDataDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.PanelSearchOptions = new System.Windows.Forms.Panel();
            this.CBReg = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBKeyword = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TBS = new System.Windows.Forms.CheckedListBox();
            this.DBS = new System.Windows.Forms.CheckedListBox();
            this.TBPath = new System.Windows.Forms.TextBox();
            this.BtnChooseDir = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.GVResult = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProcessBar = new System.Windows.Forms.ToolStripProgressBar();
            this.LBMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.CBEquals = new System.Windows.Forms.CheckBox();
            this.CBIgnoreError = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.PanelSearchOptions.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GVResult)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PanelSearchOptions);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.TBPath);
            this.panel1.Controls.Add(this.BtnChooseDir);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(558, 520);
            this.panel1.TabIndex = 0;
            // 
            // PanelSearchOptions
            // 
            this.PanelSearchOptions.Controls.Add(this.CBIgnoreError);
            this.PanelSearchOptions.Controls.Add(this.CBEquals);
            this.PanelSearchOptions.Controls.Add(this.CBReg);
            this.PanelSearchOptions.Controls.Add(this.label2);
            this.PanelSearchOptions.Controls.Add(this.TBKeyword);
            this.PanelSearchOptions.Location = new System.Drawing.Point(3, 49);
            this.PanelSearchOptions.Name = "PanelSearchOptions";
            this.PanelSearchOptions.Size = new System.Drawing.Size(537, 80);
            this.PanelSearchOptions.TabIndex = 16;
            // 
            // CBReg
            // 
            this.CBReg.AutoSize = true;
            this.CBReg.Location = new System.Drawing.Point(55, 43);
            this.CBReg.Name = "CBReg";
            this.CBReg.Size = new System.Drawing.Size(84, 16);
            this.CBReg.TabIndex = 2;
            this.CBReg.Text = "正则表达式";
            this.CBReg.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "关键字";
            // 
            // TBKeyword
            // 
            this.TBKeyword.Location = new System.Drawing.Point(55, 3);
            this.TBKeyword.Name = "TBKeyword";
            this.TBKeyword.Size = new System.Drawing.Size(218, 21);
            this.TBKeyword.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TBS);
            this.groupBox2.Controls.Add(this.DBS);
            this.groupBox2.Location = new System.Drawing.Point(3, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(543, 382);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择库表";
            // 
            // TBS
            // 
            this.TBS.FormattingEnabled = true;
            this.TBS.Location = new System.Drawing.Point(267, 20);
            this.TBS.Name = "TBS";
            this.TBS.Size = new System.Drawing.Size(270, 356);
            this.TBS.TabIndex = 1;
            // 
            // DBS
            // 
            this.DBS.FormattingEnabled = true;
            this.DBS.Location = new System.Drawing.Point(11, 20);
            this.DBS.Name = "DBS";
            this.DBS.Size = new System.Drawing.Size(247, 356);
            this.DBS.TabIndex = 0;
            // 
            // TBPath
            // 
            this.TBPath.Location = new System.Drawing.Point(3, 18);
            this.TBPath.Name = "TBPath";
            this.TBPath.ReadOnly = true;
            this.TBPath.Size = new System.Drawing.Size(499, 21);
            this.TBPath.TabIndex = 11;
            // 
            // BtnChooseDir
            // 
            this.BtnChooseDir.Location = new System.Drawing.Point(508, 17);
            this.BtnChooseDir.Name = "BtnChooseDir";
            this.BtnChooseDir.Size = new System.Drawing.Size(38, 23);
            this.BtnChooseDir.TabIndex = 10;
            this.BtnChooseDir.Text = "...";
            this.BtnChooseDir.UseVisualStyleBackColor = true;
            this.BtnChooseDir.Click += new System.EventHandler(this.BtnChooseDir_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnStop);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Controls.Add(this.GVResult);
            this.panel2.Location = new System.Drawing.Point(576, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(425, 520);
            this.panel2.TabIndex = 1;
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(347, 488);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 6;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(266, 488);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 5;
            this.BtnSearch.Text = "搜索";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // GVResult
            // 
            this.GVResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GVResult.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.GVResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GVResult.Location = new System.Drawing.Point(3, 3);
            this.GVResult.Name = "GVResult";
            this.GVResult.RowTemplate.Height = 23;
            this.GVResult.Size = new System.Drawing.Size(419, 473);
            this.GVResult.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProcessBar,
            this.LBMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 522);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1013, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProcessBar
            // 
            this.ProcessBar.Name = "ProcessBar";
            this.ProcessBar.Size = new System.Drawing.Size(100, 16);
            // 
            // LBMsg
            // 
            this.LBMsg.Name = "LBMsg";
            this.LBMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // CBEquals
            // 
            this.CBEquals.AutoSize = true;
            this.CBEquals.Location = new System.Drawing.Point(156, 43);
            this.CBEquals.Name = "CBEquals";
            this.CBEquals.Size = new System.Drawing.Size(72, 16);
            this.CBEquals.TabIndex = 3;
            this.CBEquals.Text = "精确查找";
            this.CBEquals.UseVisualStyleBackColor = true;
            // 
            // CBIgnoreError
            // 
            this.CBIgnoreError.AutoSize = true;
            this.CBIgnoreError.Location = new System.Drawing.Point(249, 43);
            this.CBIgnoreError.Name = "CBIgnoreError";
            this.CBIgnoreError.Size = new System.Drawing.Size(72, 16);
            this.CBIgnoreError.TabIndex = 4;
            this.CBIgnoreError.Text = "忽略错误";
            this.CBIgnoreError.UseVisualStyleBackColor = true;
            // 
            // SearchDBDataDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 544);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SearchDBDataDlg";
            this.Text = "SearchDBDataDlg";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanelSearchOptions.ResumeLayout(false);
            this.PanelSearchOptions.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GVResult)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox TBS;
        private System.Windows.Forms.CheckedListBox DBS;
        private System.Windows.Forms.TextBox TBPath;
        private System.Windows.Forms.Button BtnChooseDir;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView GVResult;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Panel PanelSearchOptions;
        private System.Windows.Forms.TextBox TBKeyword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CBReg;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProcessBar;
        private System.Windows.Forms.ToolStripStatusLabel LBMsg;
        private System.Windows.Forms.CheckBox CBEquals;
        private System.Windows.Forms.CheckBox CBIgnoreError;
    }
}