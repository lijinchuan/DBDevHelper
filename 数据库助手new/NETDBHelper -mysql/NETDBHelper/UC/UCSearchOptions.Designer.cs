namespace NETDBHelper.UC
{
    partial class UCSearchOptions
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
            this.CBMatchAll = new System.Windows.Forms.CheckBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.CBDB = new System.Windows.Forms.CheckBox();
            this.CBTB = new System.Windows.Forms.CheckBox();
            this.CBField = new System.Windows.Forms.CheckBox();
            this.CBProc = new System.Windows.Forms.CheckBox();
            this.CBFun = new System.Windows.Forms.CheckBox();
            this.CBOther = new System.Windows.Forms.CheckBox();
            this.CBView = new System.Windows.Forms.CheckBox();
            this.CBSearchAll = new System.Windows.Forms.CheckBox();
            this.CBHardSearch = new System.Windows.Forms.CheckBox();
            this.GB1 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBGlobal = new System.Windows.Forms.CheckBox();
            this.GB1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBMatchAll
            // 
            this.CBMatchAll.AutoSize = true;
            this.CBMatchAll.Location = new System.Drawing.Point(12, 20);
            this.CBMatchAll.Name = "CBMatchAll";
            this.CBMatchAll.Size = new System.Drawing.Size(72, 16);
            this.CBMatchAll.TabIndex = 0;
            this.CBMatchAll.Text = "完全匹配";
            this.CBMatchAll.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(172, 204);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(38, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // CBDB
            // 
            this.CBDB.AutoSize = true;
            this.CBDB.Checked = true;
            this.CBDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBDB.Location = new System.Drawing.Point(12, 21);
            this.CBDB.Name = "CBDB";
            this.CBDB.Size = new System.Drawing.Size(36, 16);
            this.CBDB.TabIndex = 2;
            this.CBDB.Text = "库";
            this.CBDB.UseVisualStyleBackColor = true;
            // 
            // CBTB
            // 
            this.CBTB.AutoSize = true;
            this.CBTB.Checked = true;
            this.CBTB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBTB.Location = new System.Drawing.Point(54, 20);
            this.CBTB.Name = "CBTB";
            this.CBTB.Size = new System.Drawing.Size(36, 16);
            this.CBTB.TabIndex = 3;
            this.CBTB.Text = "表";
            this.CBTB.UseVisualStyleBackColor = true;
            // 
            // CBField
            // 
            this.CBField.AutoSize = true;
            this.CBField.Checked = true;
            this.CBField.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBField.Location = new System.Drawing.Point(98, 20);
            this.CBField.Name = "CBField";
            this.CBField.Size = new System.Drawing.Size(48, 16);
            this.CBField.TabIndex = 4;
            this.CBField.Text = "字段";
            this.CBField.UseVisualStyleBackColor = true;
            // 
            // CBProc
            // 
            this.CBProc.AutoSize = true;
            this.CBProc.Checked = true;
            this.CBProc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBProc.Location = new System.Drawing.Point(12, 44);
            this.CBProc.Name = "CBProc";
            this.CBProc.Size = new System.Drawing.Size(72, 16);
            this.CBProc.TabIndex = 5;
            this.CBProc.Text = "存储过程";
            this.CBProc.UseVisualStyleBackColor = true;
            // 
            // CBFun
            // 
            this.CBFun.AutoSize = true;
            this.CBFun.Checked = true;
            this.CBFun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBFun.Location = new System.Drawing.Point(96, 44);
            this.CBFun.Name = "CBFun";
            this.CBFun.Size = new System.Drawing.Size(48, 16);
            this.CBFun.TabIndex = 6;
            this.CBFun.Text = "函数";
            this.CBFun.UseVisualStyleBackColor = true;
            // 
            // CBOther
            // 
            this.CBOther.AutoSize = true;
            this.CBOther.Checked = true;
            this.CBOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBOther.Location = new System.Drawing.Point(150, 44);
            this.CBOther.Name = "CBOther";
            this.CBOther.Size = new System.Drawing.Size(48, 16);
            this.CBOther.TabIndex = 7;
            this.CBOther.Text = "其它";
            this.CBOther.UseVisualStyleBackColor = true;
            // 
            // CBView
            // 
            this.CBView.AutoSize = true;
            this.CBView.Checked = true;
            this.CBView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBView.Location = new System.Drawing.Point(152, 21);
            this.CBView.Name = "CBView";
            this.CBView.Size = new System.Drawing.Size(48, 16);
            this.CBView.TabIndex = 8;
            this.CBView.Text = "视图";
            this.CBView.UseVisualStyleBackColor = true;
            // 
            // CBSearchAll
            // 
            this.CBSearchAll.AutoSize = true;
            this.CBSearchAll.Location = new System.Drawing.Point(12, 42);
            this.CBSearchAll.Name = "CBSearchAll";
            this.CBSearchAll.Size = new System.Drawing.Size(96, 16);
            this.CBSearchAll.TabIndex = 9;
            this.CBSearchAll.Text = "搜索所有结果";
            this.CBSearchAll.UseVisualStyleBackColor = true;
            // 
            // CBHardSearch
            // 
            this.CBHardSearch.AutoSize = true;
            this.CBHardSearch.Location = new System.Drawing.Point(12, 66);
            this.CBHardSearch.Name = "CBHardSearch";
            this.CBHardSearch.Size = new System.Drawing.Size(132, 16);
            this.CBHardSearch.TabIndex = 10;
            this.CBHardSearch.Text = "硬搜索(很费时费力)";
            this.CBHardSearch.UseVisualStyleBackColor = true;
            // 
            // GB1
            // 
            this.GB1.Controls.Add(this.CBFun);
            this.GB1.Controls.Add(this.CBDB);
            this.GB1.Controls.Add(this.CBTB);
            this.GB1.Controls.Add(this.CBView);
            this.GB1.Controls.Add(this.CBField);
            this.GB1.Controls.Add(this.CBOther);
            this.GB1.Controls.Add(this.CBProc);
            this.GB1.Location = new System.Drawing.Point(3, 3);
            this.GB1.Name = "GB1";
            this.GB1.Size = new System.Drawing.Size(213, 71);
            this.GB1.TabIndex = 11;
            this.GB1.TabStop = false;
            this.GB1.Text = "对象";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBGlobal);
            this.groupBox1.Controls.Add(this.CBMatchAll);
            this.groupBox1.Controls.Add(this.CBSearchAll);
            this.groupBox1.Controls.Add(this.CBHardSearch);
            this.groupBox1.Location = new System.Drawing.Point(3, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 120);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // CBGlobal
            // 
            this.CBGlobal.AutoSize = true;
            this.CBGlobal.Location = new System.Drawing.Point(12, 89);
            this.CBGlobal.Name = "CBGlobal";
            this.CBGlobal.Size = new System.Drawing.Size(72, 16);
            this.CBGlobal.TabIndex = 11;
            this.CBGlobal.Text = "全局查找";
            this.CBGlobal.UseVisualStyleBackColor = true;
            // 
            // UCSearchOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GB1);
            this.Controls.Add(this.BtnOk);
            this.Name = "UCSearchOptions";
            this.Size = new System.Drawing.Size(219, 230);
            this.GB1.ResumeLayout(false);
            this.GB1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox CBMatchAll;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.CheckBox CBDB;
        private System.Windows.Forms.CheckBox CBTB;
        private System.Windows.Forms.CheckBox CBField;
        private System.Windows.Forms.CheckBox CBProc;
        private System.Windows.Forms.CheckBox CBFun;
        private System.Windows.Forms.CheckBox CBOther;
        private System.Windows.Forms.CheckBox CBView;
        private System.Windows.Forms.CheckBox CBSearchAll;
        private System.Windows.Forms.CheckBox CBHardSearch;
        private System.Windows.Forms.GroupBox GB1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CBGlobal;
    }
}
