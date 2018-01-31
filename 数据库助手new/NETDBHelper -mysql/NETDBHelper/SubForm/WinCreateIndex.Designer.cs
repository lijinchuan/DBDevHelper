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
            this.GpCondition = new System.Windows.Forms.GroupBox();
            this.panItmes = new System.Windows.Forms.Panel();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBIndexName = new System.Windows.Forms.TextBox();
            this.CBUnique = new System.Windows.Forms.CheckBox();
            this.CBKey = new System.Windows.Forms.CheckBox();
            this.GpCondition.SuspendLayout();
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
            this.GpCondition.Size = new System.Drawing.Size(725, 224);
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
            this.panItmes.Size = new System.Drawing.Size(719, 204);
            this.panItmes.TabIndex = 0;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(630, 325);
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
            this.label1.Location = new System.Drawing.Point(18, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "索引名称";
            // 
            // TBIndexName
            // 
            this.TBIndexName.Location = new System.Drawing.Point(88, 281);
            this.TBIndexName.Name = "TBIndexName";
            this.TBIndexName.Size = new System.Drawing.Size(649, 21);
            this.TBIndexName.TabIndex = 5;
            // 
            // CBUnique
            // 
            this.CBUnique.AutoSize = true;
            this.CBUnique.Location = new System.Drawing.Point(88, 325);
            this.CBUnique.Name = "CBUnique";
            this.CBUnique.Size = new System.Drawing.Size(48, 16);
            this.CBUnique.TabIndex = 6;
            this.CBUnique.Text = "唯一";
            this.CBUnique.UseVisualStyleBackColor = true;
            // 
            // CBKey
            // 
            this.CBKey.AutoSize = true;
            this.CBKey.Location = new System.Drawing.Point(152, 325);
            this.CBKey.Name = "CBKey";
            this.CBKey.Size = new System.Drawing.Size(48, 16);
            this.CBKey.TabIndex = 7;
            this.CBKey.Text = "主键";
            this.CBKey.UseVisualStyleBackColor = true;
            // 
            // WinCreateIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 399);
            this.Controls.Add(this.CBKey);
            this.Controls.Add(this.CBUnique);
            this.Controls.Add(this.TBIndexName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.GpCondition);
            this.Name = "WinCreateIndex";
            this.Text = "创建索引";
            this.GpCondition.ResumeLayout(false);
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
    }
}