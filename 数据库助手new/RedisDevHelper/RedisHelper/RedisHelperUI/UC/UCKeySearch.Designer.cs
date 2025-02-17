﻿namespace RedisHelperUI.UC
{
    partial class UCKeySearch
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCKeySearch));
            this.BtnSearchPatten = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageData = new System.Windows.Forms.TabPage();
            this.DGVData = new System.Windows.Forms.DataGridView();
            this.TabPageInfo = new System.Windows.Forms.TabPage();
            this.TBMsg = new System.Windows.Forms.TextBox();
            this.CMSOP = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.增加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增KeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CBServers = new System.Windows.Forms.ComboBox();
            this.CBDefaultDB = new System.Windows.Forms.ComboBox();
            this.TCBSearchKey = new RedisHelperUI.UC.TableCombox();
            this.tabControl1.SuspendLayout();
            this.TabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVData)).BeginInit();
            this.TabPageInfo.SuspendLayout();
            this.CMSOP.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSearchPatten
            // 
            this.BtnSearchPatten.Location = new System.Drawing.Point(605, 6);
            this.BtnSearchPatten.Name = "BtnSearchPatten";
            this.BtnSearchPatten.Size = new System.Drawing.Size(75, 23);
            this.BtnSearchPatten.TabIndex = 1;
            this.BtnSearchPatten.Text = "搜索键";
            this.BtnSearchPatten.UseVisualStyleBackColor = true;
            this.BtnSearchPatten.Click += new System.EventHandler(this.BtnSearchPatten_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.TabPageData);
            this.tabControl1.Controls.Add(this.TabPageInfo);
            this.tabControl1.Location = new System.Drawing.Point(13, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(805, 390);
            this.tabControl1.TabIndex = 4;
            // 
            // TabPageData
            // 
            this.TabPageData.Controls.Add(this.DGVData);
            this.TabPageData.Location = new System.Drawing.Point(4, 22);
            this.TabPageData.Name = "TabPageData";
            this.TabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageData.Size = new System.Drawing.Size(797, 364);
            this.TabPageData.TabIndex = 0;
            this.TabPageData.Text = "数据";
            this.TabPageData.UseVisualStyleBackColor = true;
            // 
            // DGVData
            // 
            this.DGVData.AllowUserToAddRows = false;
            this.DGVData.AllowUserToDeleteRows = false;
            this.DGVData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DGVData.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVData.Location = new System.Drawing.Point(3, 3);
            this.DGVData.Name = "DGVData";
            this.DGVData.RowTemplate.Height = 23;
            this.DGVData.Size = new System.Drawing.Size(791, 358);
            this.DGVData.TabIndex = 2;
            // 
            // TabPageInfo
            // 
            this.TabPageInfo.Controls.Add(this.TBMsg);
            this.TabPageInfo.Location = new System.Drawing.Point(4, 22);
            this.TabPageInfo.Name = "TabPageInfo";
            this.TabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageInfo.Size = new System.Drawing.Size(797, 364);
            this.TabPageInfo.TabIndex = 1;
            this.TabPageInfo.Text = "信息";
            this.TabPageInfo.UseVisualStyleBackColor = true;
            // 
            // TBMsg
            // 
            this.TBMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBMsg.ForeColor = System.Drawing.Color.Red;
            this.TBMsg.Location = new System.Drawing.Point(3, 3);
            this.TBMsg.Multiline = true;
            this.TBMsg.Name = "TBMsg";
            this.TBMsg.ReadOnly = true;
            this.TBMsg.Size = new System.Drawing.Size(791, 358);
            this.TBMsg.TabIndex = 0;
            // 
            // CMSOP
            // 
            this.CMSOP.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.增加ToolStripMenuItem,
            this.复制ToolStripMenuItem,
            this.新增KeyToolStripMenuItem});
            this.CMSOP.Name = "CMSOP";
            this.CMSOP.Size = new System.Drawing.Size(122, 114);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            // 
            // 增加ToolStripMenuItem
            // 
            this.增加ToolStripMenuItem.Name = "增加ToolStripMenuItem";
            this.增加ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.增加ToolStripMenuItem.Text = "新增项";
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            // 
            // 新增KeyToolStripMenuItem
            // 
            this.新增KeyToolStripMenuItem.Name = "新增KeyToolStripMenuItem";
            this.新增KeyToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.新增KeyToolStripMenuItem.Text = "新增Key";
            this.新增KeyToolStripMenuItem.Click += new System.EventHandler(this.新增KeyToolStripMenuItem_Click);
            // 
            // CBServers
            // 
            this.CBServers.FormattingEnabled = true;
            this.CBServers.Location = new System.Drawing.Point(446, 7);
            this.CBServers.Name = "CBServers";
            this.CBServers.Size = new System.Drawing.Size(153, 20);
            this.CBServers.TabIndex = 5;
            // 
            // CBDefaultDB
            // 
            this.CBDefaultDB.FormattingEnabled = true;
            this.CBDefaultDB.Location = new System.Drawing.Point(13, 6);
            this.CBDefaultDB.Name = "CBDefaultDB";
            this.CBDefaultDB.Size = new System.Drawing.Size(66, 20);
            this.CBDefaultDB.TabIndex = 6;
            // 
            // TCBSearchKey
            // 
            this.TCBSearchKey.AutoSize = true;
            this.TCBSearchKey.DataSource = null;
            this.TCBSearchKey.Location = new System.Drawing.Point(85, 4);
            this.TCBSearchKey.Name = "TCBSearchKey";
            this.TCBSearchKey.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("TCBSearchKey.SelectedValues")));
            this.TCBSearchKey.Size = new System.Drawing.Size(355, 26);
            this.TCBSearchKey.TabIndex = 2;
            // 
            // UCKeySearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CBDefaultDB);
            this.Controls.Add(this.CBServers);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.TCBSearchKey);
            this.Controls.Add(this.BtnSearchPatten);
            this.Name = "UCKeySearch";
            this.Size = new System.Drawing.Size(824, 425);
            this.tabControl1.ResumeLayout(false);
            this.TabPageData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVData)).EndInit();
            this.TabPageInfo.ResumeLayout(false);
            this.TabPageInfo.PerformLayout();
            this.CMSOP.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSearchPatten;
        private TableCombox TCBSearchKey;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPageData;
        private System.Windows.Forms.DataGridView DGVData;
        private System.Windows.Forms.TabPage TabPageInfo;
        private System.Windows.Forms.TextBox TBMsg;
        private System.Windows.Forms.ContextMenuStrip CMSOP;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 增加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ComboBox CBServers;
        private System.Windows.Forms.ToolStripMenuItem 新增KeyToolStripMenuItem;
        private System.Windows.Forms.ComboBox CBDefaultDB;
    }
}
