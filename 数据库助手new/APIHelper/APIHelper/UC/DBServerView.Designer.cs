namespace APIHelper
{
    partial class DBServerView
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
            this.tv_DBServers = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.DBServerviewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制表名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加API资源ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加环境ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加环境变量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加APIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数定义ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加WCF接口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增逻辑关系图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除逻辑关系图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批量复制引用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.如何使用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ts_serchKey = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
            this.DBServerviewContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tv_DBServers
            // 
            this.tv_DBServers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tv_DBServers.Location = new System.Drawing.Point(3, 45);
            this.tv_DBServers.Name = "tv_DBServers";
            this.tv_DBServers.ShowNodeToolTips = true;
            this.tv_DBServers.Size = new System.Drawing.Size(248, 442);
            this.tv_DBServers.TabIndex = 0;
            this.tv_DBServers.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tv_DBServers_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "数据库资源视图";
            // 
            // DBServerviewContextMenuStrip
            // 
            this.DBServerviewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制表名ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.添加API资源ToolStripMenuItem,
            this.添加环境ToolStripMenuItem,
            this.添加环境变量ToolStripMenuItem,
            this.添加APIToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.参数定义ToolStripMenuItem,
            this.添加WCF接口ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.新增逻辑关系图ToolStripMenuItem,
            this.删除逻辑关系图ToolStripMenuItem,
            this.批量复制引用ToolStripMenuItem,
            this.如何使用ToolStripMenuItem});
            this.DBServerviewContextMenuStrip.Name = "DBServerviewContextMenuStrip";
            this.DBServerviewContextMenuStrip.Size = new System.Drawing.Size(161, 312);
            // 
            // 复制表名ToolStripMenuItem
            // 
            this.复制表名ToolStripMenuItem.Name = "复制表名ToolStripMenuItem";
            this.复制表名ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.复制表名ToolStripMenuItem.Text = "复制对象名";
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            // 
            // 添加API资源ToolStripMenuItem
            // 
            this.添加API资源ToolStripMenuItem.Name = "添加API资源ToolStripMenuItem";
            this.添加API资源ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.添加API资源ToolStripMenuItem.Text = "添加API资源";
            // 
            // 添加环境ToolStripMenuItem
            // 
            this.添加环境ToolStripMenuItem.Name = "添加环境ToolStripMenuItem";
            this.添加环境ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.添加环境ToolStripMenuItem.Text = "添加环境";
            // 
            // 添加环境变量ToolStripMenuItem
            // 
            this.添加环境变量ToolStripMenuItem.Name = "添加环境变量ToolStripMenuItem";
            this.添加环境变量ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.添加环境变量ToolStripMenuItem.Text = "添加环境变量";
            // 
            // 添加APIToolStripMenuItem
            // 
            this.添加APIToolStripMenuItem.Name = "添加APIToolStripMenuItem";
            this.添加APIToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.添加APIToolStripMenuItem.Text = "添加API";
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.修改ToolStripMenuItem.Text = "编辑";
            // 
            // 参数定义ToolStripMenuItem
            // 
            this.参数定义ToolStripMenuItem.Name = "参数定义ToolStripMenuItem";
            this.参数定义ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.参数定义ToolStripMenuItem.Text = "参数定义";
            // 
            // 添加WCF接口ToolStripMenuItem
            // 
            this.添加WCF接口ToolStripMenuItem.Name = "添加WCF接口ToolStripMenuItem";
            this.添加WCF接口ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.添加WCF接口ToolStripMenuItem.Text = "添加WCF接口";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // 新增逻辑关系图ToolStripMenuItem
            // 
            this.新增逻辑关系图ToolStripMenuItem.Name = "新增逻辑关系图ToolStripMenuItem";
            this.新增逻辑关系图ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.新增逻辑关系图ToolStripMenuItem.Text = "新增逻辑关系图";
            // 
            // 删除逻辑关系图ToolStripMenuItem
            // 
            this.删除逻辑关系图ToolStripMenuItem.Name = "删除逻辑关系图ToolStripMenuItem";
            this.删除逻辑关系图ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.删除逻辑关系图ToolStripMenuItem.Text = "删除逻辑关系图";
            // 
            // 批量复制引用ToolStripMenuItem
            // 
            this.批量复制引用ToolStripMenuItem.Name = "批量复制引用ToolStripMenuItem";
            this.批量复制引用ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.批量复制引用ToolStripMenuItem.Text = "批量复制引用";
            // 
            // 如何使用ToolStripMenuItem
            // 
            this.如何使用ToolStripMenuItem.Name = "如何使用ToolStripMenuItem";
            this.如何使用ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.如何使用ToolStripMenuItem.Text = "如何使用";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_serchKey,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(5, 22);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(247, 22);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ts_serchKey
            // 
            this.ts_serchKey.Name = "ts_serchKey";
            this.ts_serchKey.Size = new System.Drawing.Size(100, 22);
            this.ts_serchKey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ts_serchKey_KeyPress);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Image = global::APIHelper.Properties.Resources.新建位图图像;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(23, 19);
            this.toolStripDropDownButton1.Text = "搜索";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // DBServerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tv_DBServers);
            this.Name = "DBServerView";
            this.Size = new System.Drawing.Size(251, 488);
            this.Load += new System.EventHandler(this.DBServerView_Load);
            this.DBServerviewContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tv_DBServers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip DBServerviewContextMenuStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripComboBox ts_serchKey;
        private System.Windows.Forms.ToolStripMenuItem 复制表名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增逻辑关系图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除逻辑关系图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加API资源ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加APIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加环境ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加环境变量ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数定义ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加WCF接口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 如何使用ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 批量复制引用ToolStripMenuItem;
    }
}
