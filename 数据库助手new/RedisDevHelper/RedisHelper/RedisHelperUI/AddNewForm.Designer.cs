namespace RedisHelperUI
{
    partial class AddNewForm
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
            this.BtnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBThridKey = new System.Windows.Forms.CheckBox();
            this.CBSecKey = new System.Windows.Forms.CheckBox();
            this.TBThridKey = new System.Windows.Forms.TextBox();
            this.LBThridKey = new System.Windows.Forms.Label();
            this.TBSecKey = new System.Windows.Forms.TextBox();
            this.LBSecKey = new System.Windows.Forms.Label();
            this.CBType = new System.Windows.Forms.ComboBox();
            this.TB_FirstKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(293, 206);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(98, 25);
            this.BtnOk.TabIndex = 3;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CBThridKey);
            this.groupBox1.Controls.Add(this.CBSecKey);
            this.groupBox1.Controls.Add(this.TBThridKey);
            this.groupBox1.Controls.Add(this.LBThridKey);
            this.groupBox1.Controls.Add(this.TBSecKey);
            this.groupBox1.Controls.Add(this.LBSecKey);
            this.groupBox1.Controls.Add(this.CBType);
            this.groupBox1.Controls.Add(this.TB_FirstKey);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-2, -7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 203);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // CBThridKey
            // 
            this.CBThridKey.AutoSize = true;
            this.CBThridKey.Location = new System.Drawing.Point(333, 156);
            this.CBThridKey.Name = "CBThridKey";
            this.CBThridKey.Size = new System.Drawing.Size(48, 16);
            this.CBThridKey.TabIndex = 18;
            this.CBThridKey.Text = "数字";
            this.CBThridKey.UseVisualStyleBackColor = true;
            // 
            // CBSecKey
            // 
            this.CBSecKey.AutoSize = true;
            this.CBSecKey.Location = new System.Drawing.Point(333, 112);
            this.CBSecKey.Name = "CBSecKey";
            this.CBSecKey.Size = new System.Drawing.Size(48, 16);
            this.CBSecKey.TabIndex = 17;
            this.CBSecKey.Text = "数字";
            this.CBSecKey.UseVisualStyleBackColor = true;
            // 
            // TBThridKey
            // 
            this.TBThridKey.Location = new System.Drawing.Point(97, 153);
            this.TBThridKey.Name = "TBThridKey";
            this.TBThridKey.Size = new System.Drawing.Size(230, 21);
            this.TBThridKey.TabIndex = 16;
            // 
            // LBThridKey
            // 
            this.LBThridKey.AutoSize = true;
            this.LBThridKey.Location = new System.Drawing.Point(32, 156);
            this.LBThridKey.Name = "LBThridKey";
            this.LBThridKey.Size = new System.Drawing.Size(41, 12);
            this.LBThridKey.TabIndex = 15;
            this.LBThridKey.Text = "label3";
            // 
            // TBSecKey
            // 
            this.TBSecKey.Location = new System.Drawing.Point(97, 107);
            this.TBSecKey.Name = "TBSecKey";
            this.TBSecKey.Size = new System.Drawing.Size(230, 21);
            this.TBSecKey.TabIndex = 14;
            // 
            // LBSecKey
            // 
            this.LBSecKey.AutoSize = true;
            this.LBSecKey.Location = new System.Drawing.Point(32, 110);
            this.LBSecKey.Name = "LBSecKey";
            this.LBSecKey.Size = new System.Drawing.Size(41, 12);
            this.LBSecKey.TabIndex = 13;
            this.LBSecKey.Text = "label2";
            // 
            // CBType
            // 
            this.CBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBType.FormattingEnabled = true;
            this.CBType.Items.AddRange(new object[] {
            "String",
            "Set",
            "Hash",
            "List",
            "SortedSet"});
            this.CBType.Location = new System.Drawing.Point(97, 62);
            this.CBType.Name = "CBType";
            this.CBType.Size = new System.Drawing.Size(121, 20);
            this.CBType.TabIndex = 12;
            // 
            // TB_FirstKey
            // 
            this.TB_FirstKey.Location = new System.Drawing.Point(97, 22);
            this.TB_FirstKey.Name = "TB_FirstKey";
            this.TB_FirstKey.Size = new System.Drawing.Size(230, 21);
            this.TB_FirstKey.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "主键";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "类型";
            // 
            // AddNewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 240);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增数据";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CBThridKey;
        private System.Windows.Forms.CheckBox CBSecKey;
        private System.Windows.Forms.TextBox TBThridKey;
        private System.Windows.Forms.Label LBThridKey;
        private System.Windows.Forms.TextBox TBSecKey;
        private System.Windows.Forms.Label LBSecKey;
        private System.Windows.Forms.ComboBox CBType;
        private System.Windows.Forms.TextBox TB_FirstKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}