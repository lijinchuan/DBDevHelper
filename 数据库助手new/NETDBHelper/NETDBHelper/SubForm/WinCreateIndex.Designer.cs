namespace NETDBHelper.SubForm
{
    partial class WinCreateIndex
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinCreateIndex));
            this.GpCondition = new System.Windows.Forms.GroupBox();
            this.panItmes = new System.Windows.Forms.Panel();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBIndexName = new System.Windows.Forms.TextBox();
            this.CBUnique = new System.Windows.Forms.CheckBox();
            this.CBKey = new System.Windows.Forms.CheckBox();
            this.CB_AutoIncr = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnUp = new System.Windows.Forms.Button();
            this.BtnDown = new System.Windows.Forms.Button();
            this.LbxColumn = new System.Windows.Forms.ListBox();
            this.CBClustered = new System.Windows.Forms.CheckBox();
            this.GpCondition.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpCondition
            // 
            this.GpCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GpCondition.Controls.Add(this.panItmes);
            this.GpCondition.Location = new System.Drawing.Point(12, 27);
            this.GpCondition.Name = "GpCondition";
            this.GpCondition.Size = new System.Drawing.Size(725, 177);
            this.GpCondition.TabIndex = 2;
            this.GpCondition.TabStop = false;
            this.GpCondition.Text = "选择字段";
            // 
            // panItmes
            // 
            this.panItmes.AutoScroll = true;
            this.panItmes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panItmes.Location = new System.Drawing.Point(3, 17);
            this.panItmes.Name = "panItmes";
            this.panItmes.Size = new System.Drawing.Size(719, 157);
            this.panItmes.TabIndex = 0;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(617, 450);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(104, 28);
            this.BtnAdd.TabIndex = 3;
            this.BtnAdd.Text = "生成索引";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 404);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "索引名称";
            // 
            // TBIndexName
            // 
            this.TBIndexName.Location = new System.Drawing.Point(72, 401);
            this.TBIndexName.Name = "TBIndexName";
            this.TBIndexName.Size = new System.Drawing.Size(649, 21);
            this.TBIndexName.TabIndex = 5;
            // 
            // CBUnique
            // 
            this.CBUnique.AutoSize = true;
            this.CBUnique.Location = new System.Drawing.Point(503, 268);
            this.CBUnique.Name = "CBUnique";
            this.CBUnique.Size = new System.Drawing.Size(48, 16);
            this.CBUnique.TabIndex = 6;
            this.CBUnique.Text = "唯一";
            this.CBUnique.UseVisualStyleBackColor = true;
            // 
            // CBKey
            // 
            this.CBKey.AutoSize = true;
            this.CBKey.Location = new System.Drawing.Point(503, 289);
            this.CBKey.Name = "CBKey";
            this.CBKey.Size = new System.Drawing.Size(48, 16);
            this.CBKey.TabIndex = 7;
            this.CBKey.Text = "主键";
            this.CBKey.UseVisualStyleBackColor = true;
            // 
            // CB_AutoIncr
            // 
            this.CB_AutoIncr.AutoSize = true;
            this.CB_AutoIncr.Location = new System.Drawing.Point(503, 245);
            this.CB_AutoIncr.Name = "CB_AutoIncr";
            this.CB_AutoIncr.Size = new System.Drawing.Size(48, 16);
            this.CB_AutoIncr.TabIndex = 8;
            this.CB_AutoIncr.Text = "自增";
            this.CB_AutoIncr.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnUp);
            this.groupBox1.Controls.Add(this.BtnDown);
            this.groupBox1.Controls.Add(this.LbxColumn);
            this.groupBox1.Location = new System.Drawing.Point(15, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 172);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "调整顺序";
            // 
            // BtnUp
            // 
            this.BtnUp.Image = ((System.Drawing.Image)(resources.GetObject("BtnUp.Image")));
            this.BtnUp.Location = new System.Drawing.Point(383, 88);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(25, 33);
            this.BtnUp.TabIndex = 2;
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.Image = ((System.Drawing.Image)(resources.GetObject("BtnDown.Image")));
            this.BtnDown.Location = new System.Drawing.Point(383, 35);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(25, 33);
            this.BtnDown.TabIndex = 1;
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // LbxColumn
            // 
            this.LbxColumn.FormattingEnabled = true;
            this.LbxColumn.ItemHeight = 12;
            this.LbxColumn.Location = new System.Drawing.Point(6, 20);
            this.LbxColumn.Name = "LbxColumn";
            this.LbxColumn.Size = new System.Drawing.Size(370, 148);
            this.LbxColumn.TabIndex = 0;
            // 
            // CBClustered
            // 
            this.CBClustered.AutoSize = true;
            this.CBClustered.Location = new System.Drawing.Point(503, 311);
            this.CBClustered.Name = "CBClustered";
            this.CBClustered.Size = new System.Drawing.Size(48, 16);
            this.CBClustered.TabIndex = 10;
            this.CBClustered.Text = "聚集";
            this.CBClustered.UseVisualStyleBackColor = true;
            // 
            // WinCreateIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 490);
            this.Controls.Add(this.CBClustered);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CB_AutoIncr);
            this.Controls.Add(this.CBKey);
            this.Controls.Add(this.CBUnique);
            this.Controls.Add(this.TBIndexName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.GpCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinCreateIndex";
            this.Text = "创建索引";
            this.GpCondition.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GpCondition;
        private System.Windows.Forms.Panel panItmes;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBIndexName;
        private System.Windows.Forms.CheckBox CBUnique;
        private System.Windows.Forms.CheckBox CBKey;
        private System.Windows.Forms.CheckBox CB_AutoIncr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox LbxColumn;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.CheckBox CBClustered;
    }
}