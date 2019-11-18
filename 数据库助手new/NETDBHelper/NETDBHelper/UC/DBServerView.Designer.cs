namespace NETDBHelper
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
            this.生成实体类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示前100条数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成数据字典ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制表名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.备注ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubMenuItem_Proc = new System.Windows.Forms.ToolStripMenuItem();
            this.SubMenuItem_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubMenuItem_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.SubMenuItem_Select = new System.Windows.Forms.ToolStripMenuItem();
            this.批量插入更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upsertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_ExeProc = new System.Windows.Forms.ToolStripMenuItem();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.创建语句ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExpdataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateMSSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据MSSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SyncDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.修改表名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ts_serchKey = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
            this.CommMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemReload = new System.Windows.Forms.ToolStripMenuItem();
            this.CommSubMenuitem_ViewConnsql = new System.Windows.Forms.ToolStripMenuItem();
            this.复制对象名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_ViewTableList = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItemAddEntityTB = new System.Windows.Forms.ToolStripMenuItem();
            this.备注本地ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_MulMarkLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.CommSubMenuitem_ReorderColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.ColumnMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.ColumnMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.CommSubMenuitem_add = new System.Windows.Forms.ToolStripMenuItem();
            this.CommSubMenuItem_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_ViewColumnList = new System.Windows.Forms.ToolStripMenuItem();
            this.DBServerviewContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.CommMenuStrip.SuspendLayout();
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
            this.生成实体类ToolStripMenuItem,
            this.显示前100条数据ToolStripMenuItem,
            this.生成数据字典ToolStripMenuItem,
            this.复制表名ToolStripMenuItem,
            this.备注ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.SubMenuItem_Proc,
            this.TSMI_ExeProc,
            this.导出ToolStripMenuItem,
            this.SyncDataToolStripMenuItem,
            this.toolStripMenuItem2,
            this.修改表名ToolStripMenuItem,
            this.删除表ToolStripMenuItem});
            this.DBServerviewContextMenuStrip.Name = "DBServerviewContextMenuStrip";
            this.DBServerviewContextMenuStrip.Size = new System.Drawing.Size(170, 274);
            // 
            // 生成实体类ToolStripMenuItem
            // 
            this.生成实体类ToolStripMenuItem.Name = "生成实体类ToolStripMenuItem";
            this.生成实体类ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.生成实体类ToolStripMenuItem.Text = "生成实体类";
            // 
            // 显示前100条数据ToolStripMenuItem
            // 
            this.显示前100条数据ToolStripMenuItem.Name = "显示前100条数据ToolStripMenuItem";
            this.显示前100条数据ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.显示前100条数据ToolStripMenuItem.Text = "显示前100条数据";
            // 
            // 生成数据字典ToolStripMenuItem
            // 
            this.生成数据字典ToolStripMenuItem.Name = "生成数据字典ToolStripMenuItem";
            this.生成数据字典ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.生成数据字典ToolStripMenuItem.Text = "查看数据字典";
            this.生成数据字典ToolStripMenuItem.Click += new System.EventHandler(this.生成数据字典ToolStripMenuItem_Click);
            // 
            // 复制表名ToolStripMenuItem
            // 
            this.复制表名ToolStripMenuItem.Name = "复制表名ToolStripMenuItem";
            this.复制表名ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.复制表名ToolStripMenuItem.Text = "复制对象名";
            // 
            // 备注ToolStripMenuItem
            // 
            this.备注ToolStripMenuItem.Name = "备注ToolStripMenuItem";
            this.备注ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.备注ToolStripMenuItem.Text = "备注";
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            // 
            // SubMenuItem_Proc
            // 
            this.SubMenuItem_Proc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SubMenuItem_Insert,
            this.updateToolStripMenuItem,
            this.SubMenuItem_Delete,
            this.SubMenuItem_Select,
            this.批量插入更新ToolStripMenuItem,
            this.upsertToolStripMenuItem});
            this.SubMenuItem_Proc.Name = "SubMenuItem_Proc";
            this.SubMenuItem_Proc.Size = new System.Drawing.Size(169, 22);
            this.SubMenuItem_Proc.Text = "存储过程";
            // 
            // SubMenuItem_Insert
            // 
            this.SubMenuItem_Insert.Name = "SubMenuItem_Insert";
            this.SubMenuItem_Insert.Size = new System.Drawing.Size(141, 22);
            this.SubMenuItem_Insert.Text = "Insert";
            this.SubMenuItem_Insert.Click += new System.EventHandler(this.SubMenuItem_Insert_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // SubMenuItem_Delete
            // 
            this.SubMenuItem_Delete.Name = "SubMenuItem_Delete";
            this.SubMenuItem_Delete.Size = new System.Drawing.Size(141, 22);
            this.SubMenuItem_Delete.Text = "Delete";
            this.SubMenuItem_Delete.Click += new System.EventHandler(this.SubMenuItem_Delete_Click);
            // 
            // SubMenuItem_Select
            // 
            this.SubMenuItem_Select.Name = "SubMenuItem_Select";
            this.SubMenuItem_Select.Size = new System.Drawing.Size(141, 22);
            this.SubMenuItem_Select.Text = "Select";
            this.SubMenuItem_Select.Click += new System.EventHandler(this.SubMenuItem_Select_Click);
            // 
            // 批量插入更新ToolStripMenuItem
            // 
            this.批量插入更新ToolStripMenuItem.Name = "批量插入更新ToolStripMenuItem";
            this.批量插入更新ToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.批量插入更新ToolStripMenuItem.Text = "BatchInsert";
            this.批量插入更新ToolStripMenuItem.Click += new System.EventHandler(this.批量插入更新ToolStripMenuItem_Click);
            // 
            // upsertToolStripMenuItem
            // 
            this.upsertToolStripMenuItem.Name = "upsertToolStripMenuItem";
            this.upsertToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.upsertToolStripMenuItem.Text = "Upsert";
            this.upsertToolStripMenuItem.Click += new System.EventHandler(this.upsertToolStripMenuItem_Click);
            // 
            // TSMI_ExeProc
            // 
            this.TSMI_ExeProc.Name = "TSMI_ExeProc";
            this.TSMI_ExeProc.Size = new System.Drawing.Size(169, 22);
            this.TSMI_ExeProc.Text = "执行存储过程";
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.创建语句ToolStripMenuItem,
            this.ExpdataToolStripMenuItem,
            this.CreateMSSQLToolStripMenuItem,
            this.数据MSSQLToolStripMenuItem});
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.导出ToolStripMenuItem.Text = "导出";
            // 
            // 创建语句ToolStripMenuItem
            // 
            this.创建语句ToolStripMenuItem.Name = "创建语句ToolStripMenuItem";
            this.创建语句ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.创建语句ToolStripMenuItem.Text = "创建语句(MySQL)";
            this.创建语句ToolStripMenuItem.Click += new System.EventHandler(this.创建语句ToolStripMenuItem_Click);
            // 
            // ExpdataToolStripMenuItem
            // 
            this.ExpdataToolStripMenuItem.Name = "ExpdataToolStripMenuItem";
            this.ExpdataToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.ExpdataToolStripMenuItem.Text = "数据(MySQL)";
            this.ExpdataToolStripMenuItem.Click += new System.EventHandler(this.ExpdataToolStripMenuItem_Click);
            // 
            // CreateMSSQLToolStripMenuItem
            // 
            this.CreateMSSQLToolStripMenuItem.Name = "CreateMSSQLToolStripMenuItem";
            this.CreateMSSQLToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.CreateMSSQLToolStripMenuItem.Text = "创建语句(MSSQL)";
            this.CreateMSSQLToolStripMenuItem.Click += new System.EventHandler(this.CreateMSSQLToolStripMenuItem_Click);
            // 
            // 数据MSSQLToolStripMenuItem
            // 
            this.数据MSSQLToolStripMenuItem.Name = "数据MSSQLToolStripMenuItem";
            this.数据MSSQLToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.数据MSSQLToolStripMenuItem.Text = "数据(MSSQL)";
            this.数据MSSQLToolStripMenuItem.Click += new System.EventHandler(this.数据MSSQLToolStripMenuItem_Click);
            // 
            // SyncDataToolStripMenuItem
            // 
            this.SyncDataToolStripMenuItem.Name = "SyncDataToolStripMenuItem";
            this.SyncDataToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.SyncDataToolStripMenuItem.Text = "同步数据";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(166, 6);
            // 
            // 修改表名ToolStripMenuItem
            // 
            this.修改表名ToolStripMenuItem.Name = "修改表名ToolStripMenuItem";
            this.修改表名ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.修改表名ToolStripMenuItem.Text = "修改表名";
            // 
            // 删除表ToolStripMenuItem
            // 
            this.删除表ToolStripMenuItem.Name = "删除表ToolStripMenuItem";
            this.删除表ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.删除表ToolStripMenuItem.Text = "删除表";
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
            this.toolStripDropDownButton1.Image = global::NETDBHelper.Properties.Resources.新建位图图像;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(23, 19);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // CommMenuStrip
            // 
            this.CommMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemReload,
            this.CommSubMenuitem_ViewConnsql,
            this.复制对象名ToolStripMenuItem,
            this.TSMI_ViewTableList,
            this.TSMI_ViewColumnList,
            this.subMenuItemAddEntityTB,
            this.备注本地ToolStripMenuItem,
            this.TSMI_MulMarkLocal,
            this.toolStripMenuItem3,
            this.CommSubMenuitem_ReorderColumn,
            this.CommSubMenuitem_add,
            this.CommSubMenuItem_Delete});
            this.CommMenuStrip.Name = "CommMenuStrip";
            this.CommMenuStrip.Size = new System.Drawing.Size(181, 274);
            // 
            // ToolStripMenuItemReload
            // 
            this.ToolStripMenuItemReload.Name = "ToolStripMenuItemReload";
            this.ToolStripMenuItemReload.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemReload.Text = "刷新";
            // 
            // CommSubMenuitem_ViewConnsql
            // 
            this.CommSubMenuitem_ViewConnsql.Name = "CommSubMenuitem_ViewConnsql";
            this.CommSubMenuitem_ViewConnsql.Size = new System.Drawing.Size(180, 22);
            this.CommSubMenuitem_ViewConnsql.Text = "查看连接串";
            this.CommSubMenuitem_ViewConnsql.Visible = false;
            this.CommSubMenuitem_ViewConnsql.Click += new System.EventHandler(this.查看连接串ToolStripMenuItem_Click);
            // 
            // 复制对象名ToolStripMenuItem
            // 
            this.复制对象名ToolStripMenuItem.Name = "复制对象名ToolStripMenuItem";
            this.复制对象名ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.复制对象名ToolStripMenuItem.Text = "复制对象名";
            // 
            // TSMI_ViewTableList
            // 
            this.TSMI_ViewTableList.Name = "TSMI_ViewTableList";
            this.TSMI_ViewTableList.Size = new System.Drawing.Size(180, 22);
            this.TSMI_ViewTableList.Text = "查看表";
            // 
            // subMenuItemAddEntityTB
            // 
            this.subMenuItemAddEntityTB.Name = "subMenuItemAddEntityTB";
            this.subMenuItemAddEntityTB.Size = new System.Drawing.Size(180, 22);
            this.subMenuItemAddEntityTB.Text = "添加实体映射表";
            // 
            // 备注本地ToolStripMenuItem
            // 
            this.备注本地ToolStripMenuItem.Name = "备注本地ToolStripMenuItem";
            this.备注本地ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.备注本地ToolStripMenuItem.Text = "备注(本地)";
            this.备注本地ToolStripMenuItem.Click += new System.EventHandler(this.备注本地ToolStripMenuItem_Click);
            // 
            // TSMI_MulMarkLocal
            // 
            this.TSMI_MulMarkLocal.Name = "TSMI_MulMarkLocal";
            this.TSMI_MulMarkLocal.Size = new System.Drawing.Size(180, 22);
            this.TSMI_MulMarkLocal.Text = "批量备注(本地)";
            this.TSMI_MulMarkLocal.Click += new System.EventHandler(this.TSMI_MulMarkLocal_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // CommSubMenuitem_ReorderColumn
            // 
            this.CommSubMenuitem_ReorderColumn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ColumnMoveUp,
            this.ColumnMoveDown});
            this.CommSubMenuitem_ReorderColumn.Name = "CommSubMenuitem_ReorderColumn";
            this.CommSubMenuitem_ReorderColumn.Size = new System.Drawing.Size(180, 22);
            this.CommSubMenuitem_ReorderColumn.Text = "调整字段顺序";
            // 
            // ColumnMoveUp
            // 
            this.ColumnMoveUp.Name = "ColumnMoveUp";
            this.ColumnMoveUp.Size = new System.Drawing.Size(100, 22);
            this.ColumnMoveUp.Text = "上移";
            // 
            // ColumnMoveDown
            // 
            this.ColumnMoveDown.Name = "ColumnMoveDown";
            this.ColumnMoveDown.Size = new System.Drawing.Size(100, 22);
            this.ColumnMoveDown.Text = "下移";
            // 
            // CommSubMenuitem_add
            // 
            this.CommSubMenuitem_add.Name = "CommSubMenuitem_add";
            this.CommSubMenuitem_add.Size = new System.Drawing.Size(180, 22);
            this.CommSubMenuitem_add.Text = "新增对象";
            // 
            // CommSubMenuItem_Delete
            // 
            this.CommSubMenuItem_Delete.Name = "CommSubMenuItem_Delete";
            this.CommSubMenuItem_Delete.Size = new System.Drawing.Size(180, 22);
            this.CommSubMenuItem_Delete.Text = "删除对象";
            // 
            // TSMI_ViewColumnList
            // 
            this.TSMI_ViewColumnList.Name = "TSMI_ViewColumnList";
            this.TSMI_ViewColumnList.Size = new System.Drawing.Size(180, 22);
            this.TSMI_ViewColumnList.Text = "查看字段";
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
            this.CommMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tv_DBServers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip DBServerviewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 生成实体类ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示前100条数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripComboBox ts_serchKey;
        private System.Windows.Forms.ToolStripMenuItem 复制表名ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip CommMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReload;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制对象名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subMenuItemAddEntityTB;
        private System.Windows.Forms.ToolStripMenuItem CommSubMenuItem_Delete;
        private System.Windows.Forms.ToolStripMenuItem 删除表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CommSubMenuitem_add;
        private System.Windows.Forms.ToolStripMenuItem 修改表名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SubMenuItem_Proc;
        private System.Windows.Forms.ToolStripMenuItem SubMenuItem_Insert;
        private System.Windows.Forms.ToolStripMenuItem SubMenuItem_Delete;
        private System.Windows.Forms.ToolStripMenuItem SubMenuItem_Select;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 创建语句ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExpdataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateMSSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据MSSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成数据字典ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CommSubMenuitem_ViewConnsql;
        private System.Windows.Forms.ToolStripMenuItem 批量插入更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upsertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CommSubMenuitem_ReorderColumn;
        private System.Windows.Forms.ToolStripMenuItem ColumnMoveUp;
        private System.Windows.Forms.ToolStripMenuItem ColumnMoveDown;
        private System.Windows.Forms.ToolStripMenuItem SyncDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 备注本地ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 备注ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem TSMI_MulMarkLocal;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ViewTableList;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ExeProc;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ViewColumnList;
    }
}
