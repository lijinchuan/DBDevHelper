namespace NETDBHelper.UC
{
    partial class UCCreateCodeCallSP
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_spName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Params = new System.Windows.Forms.TextBox();
            this.tb_code = new System.Windows.Forms.TextBox();
            this.tb_Entity = new System.Windows.Forms.TextBox();
            this.lab = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.CBType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "存储过程名称";
            // 
            // tb_spName
            // 
            this.tb_spName.Location = new System.Drawing.Point(166, 31);
            this.tb_spName.Name = "tb_spName";
            this.tb_spName.Size = new System.Drawing.Size(142, 21);
            this.tb_spName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "存储过程参数";
            // 
            // tb_Params
            // 
            this.tb_Params.AcceptsReturn = true;
            this.tb_Params.AcceptsTab = true;
            this.tb_Params.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Params.Location = new System.Drawing.Point(167, 78);
            this.tb_Params.MaxLength = 32767000;
            this.tb_Params.Multiline = true;
            this.tb_Params.Name = "tb_Params";
            this.tb_Params.Size = new System.Drawing.Size(624, 226);
            this.tb_Params.TabIndex = 3;
            // 
            // tb_code
            // 
            this.tb_code.AcceptsReturn = true;
            this.tb_code.AcceptsTab = true;
            this.tb_code.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_code.Location = new System.Drawing.Point(169, 327);
            this.tb_code.MaxLength = 32767000;
            this.tb_code.Multiline = true;
            this.tb_code.Name = "tb_code";
            this.tb_code.Size = new System.Drawing.Size(622, 280);
            this.tb_code.TabIndex = 4;
            // 
            // tb_Entity
            // 
            this.tb_Entity.Location = new System.Drawing.Point(398, 34);
            this.tb_Entity.Name = "tb_Entity";
            this.tb_Entity.Size = new System.Drawing.Size(156, 21);
            this.tb_Entity.TabIndex = 5;
            // 
            // lab
            // 
            this.lab.AutoSize = true;
            this.lab.Location = new System.Drawing.Point(327, 37);
            this.lab.Name = "lab";
            this.lab.Size = new System.Drawing.Size(65, 12);
            this.lab.TabIndex = 6;
            this.lab.Text = "实体类实例";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(68, 336);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(572, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "调用方式 ";
            // 
            // CBType
            // 
            this.CBType.FormattingEnabled = true;
            this.CBType.Location = new System.Drawing.Point(628, 34);
            this.CBType.Name = "CBType";
            this.CBType.Size = new System.Drawing.Size(121, 20);
            this.CBType.TabIndex = 9;
            // 
            // UCCreateCodeCallSP
            // 
            this.Controls.Add(this.CBType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lab);
            this.Controls.Add(this.tb_Entity);
            this.Controls.Add(this.tb_code);
            this.Controls.Add(this.tb_Params);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_spName);
            this.Controls.Add(this.label1);
            this.Name = "UCCreateCodeCallSP";
            this.Size = new System.Drawing.Size(843, 639);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_spName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Params;
        private System.Windows.Forms.TextBox tb_code;
        private System.Windows.Forms.TextBox tb_Entity;
        private System.Windows.Forms.Label lab;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBType;
    }
}
