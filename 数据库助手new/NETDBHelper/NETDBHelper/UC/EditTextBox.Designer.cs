namespace NETDBHelper.UC
{
    partial class EditTextBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTextBox));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.剪切ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.撤消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重做ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.搜索ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SaveAsFile = new System.Windows.Forms.ToolStripMenuItem();
            this.RichText = new NETDBHelper.UC.MyRichTextBox();
            this.ScaleNos = new NETDBHelper.UC.Scale();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全选ToolStripMenuItem,
            this.复制ToolStripMenuItem,
            this.剪切ToolStripMenuItem,
            this.粘贴ToolStripMenuItem,
            this.撤消ToolStripMenuItem,
            this.重做ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.搜索ToolStripMenuItem,
            this.TSMI_Save,
            this.TSMI_SaveAsFile});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 208);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.全选ToolStripMenuItem.Text = "全选";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 剪切ToolStripMenuItem
            // 
            this.剪切ToolStripMenuItem.Name = "剪切ToolStripMenuItem";
            this.剪切ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.剪切ToolStripMenuItem.Text = "剪切";
            this.剪切ToolStripMenuItem.Click += new System.EventHandler(this.剪切ToolStripMenuItem_Click);
            // 
            // 粘贴ToolStripMenuItem
            // 
            this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
            this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.粘贴ToolStripMenuItem.Text = "粘贴";
            this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItem_Click);
            // 
            // 撤消ToolStripMenuItem
            // 
            this.撤消ToolStripMenuItem.Name = "撤消ToolStripMenuItem";
            this.撤消ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.撤消ToolStripMenuItem.Text = "撤消";
            this.撤消ToolStripMenuItem.Click += new System.EventHandler(this.撤消ToolStripMenuItem_Click);
            // 
            // 重做ToolStripMenuItem
            // 
            this.重做ToolStripMenuItem.Name = "重做ToolStripMenuItem";
            this.重做ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.重做ToolStripMenuItem.Text = "重做";
            this.重做ToolStripMenuItem.Click += new System.EventHandler(this.重做ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(133, 6);
            // 
            // 搜索ToolStripMenuItem
            // 
            this.搜索ToolStripMenuItem.Name = "搜索ToolStripMenuItem";
            this.搜索ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.搜索ToolStripMenuItem.Text = "搜索";
            this.搜索ToolStripMenuItem.Click += new System.EventHandler(this.搜索ToolStripMenuItem_Click);
            // 
            // TSMI_Save
            // 
            this.TSMI_Save.Name = "TSMI_Save";
            this.TSMI_Save.Size = new System.Drawing.Size(136, 22);
            this.TSMI_Save.Text = "保存";
            this.TSMI_Save.Click += new System.EventHandler(this.TSMI_Save_Click);
            // 
            // TSMI_SaveAsFile
            // 
            this.TSMI_SaveAsFile.Name = "TSMI_SaveAsFile";
            this.TSMI_SaveAsFile.Size = new System.Drawing.Size(136, 22);
            this.TSMI_SaveAsFile.Text = "保存为文件";
            this.TSMI_SaveAsFile.Click += new System.EventHandler(this.TSMI_SaveAsFile_Click);
            // 
            // RichText
            // 
            this.RichText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichText.HorizontalPosition = 0;
            this.RichText.Location = new System.Drawing.Point(46, 0);
            this.RichText.Name = "RichText";
            this.RichText.Size = new System.Drawing.Size(625, 448);
            this.RichText.TabIndex = 1;
            this.RichText.Text = "";
            this.RichText.VerticalPosition = 0;
            this.RichText.SelectionChanged += new System.EventHandler(this.RichText_SelectionChanged);
            this.RichText.MouseHover += new System.EventHandler(this.RichText_MouseHover);
            // 
            // ScaleNos
            // 
            this.ScaleNos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ScaleNos.LineNos = ((System.Collections.Generic.Dictionary<int, System.Drawing.PointF>)(resources.GetObject("ScaleNos.LineNos")));
            this.ScaleNos.Location = new System.Drawing.Point(2, 2);
            this.ScaleNos.Name = "ScaleNos";
            this.ScaleNos.Size = new System.Drawing.Size(44, 446);
            this.ScaleNos.TabIndex = 0;
            // 
            // EditTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RichText);
            this.Controls.Add(this.ScaleNos);
            this.Name = "EditTextBox";
            this.Size = new System.Drawing.Size(674, 448);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Scale ScaleNos;
        private MyRichTextBox RichText;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 粘贴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 搜索ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Save;
        private System.Windows.Forms.ToolStripMenuItem 剪切ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 撤消ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重做ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMI_SaveAsFile;
    }
}
