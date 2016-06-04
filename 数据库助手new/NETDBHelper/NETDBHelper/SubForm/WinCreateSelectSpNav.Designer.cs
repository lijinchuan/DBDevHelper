namespace NETDBHelper.SubForm
{
    partial class WinCreateSelectSpNav
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
            this.GpOutPut = new System.Windows.Forms.GroupBox();
            this.GpCondition = new System.Windows.Forms.GroupBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBEditer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBAbout = new System.Windows.Forms.TextBox();
            this.panCondition = new System.Windows.Forms.Panel();
            this.panOutput = new System.Windows.Forms.Panel();
            this.GpOutPut.SuspendLayout();
            this.GpCondition.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpOutPut
            // 
            this.GpOutPut.Controls.Add(this.panOutput);
            this.GpOutPut.Location = new System.Drawing.Point(30, 275);
            this.GpOutPut.Name = "GpOutPut";
            this.GpOutPut.Size = new System.Drawing.Size(686, 131);
            this.GpOutPut.TabIndex = 0;
            this.GpOutPut.TabStop = false;
            this.GpOutPut.Text = "输出字段";
            // 
            // GpCondition
            // 
            this.GpCondition.Controls.Add(this.panCondition);
            this.GpCondition.Location = new System.Drawing.Point(30, 122);
            this.GpCondition.Name = "GpCondition";
            this.GpCondition.Size = new System.Drawing.Size(686, 135);
            this.GpCondition.TabIndex = 1;
            this.GpCondition.TabStop = false;
            this.GpCondition.Text = "条件字段";
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(510, 424);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 28);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(636, 424);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 28);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "作者";
            // 
            // TBEditer
            // 
            this.TBEditer.Location = new System.Drawing.Point(105, 26);
            this.TBEditer.Name = "TBEditer";
            this.TBEditer.Size = new System.Drawing.Size(157, 21);
            this.TBEditer.TabIndex = 5;
            this.TBEditer.Text = "lijinchuan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "存储过程说明";
            // 
            // TBAbout
            // 
            this.TBAbout.Location = new System.Drawing.Point(106, 65);
            this.TBAbout.Multiline = true;
            this.TBAbout.Name = "TBAbout";
            this.TBAbout.Size = new System.Drawing.Size(595, 51);
            this.TBAbout.TabIndex = 7;
            // 
            // panCondition
            // 
            this.panCondition.AutoScroll = true;
            this.panCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panCondition.Location = new System.Drawing.Point(3, 17);
            this.panCondition.Name = "panCondition";
            this.panCondition.Size = new System.Drawing.Size(680, 115);
            this.panCondition.TabIndex = 0;
            // 
            // panOutput
            // 
            this.panOutput.AutoScroll = true;
            this.panOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panOutput.Location = new System.Drawing.Point(3, 17);
            this.panOutput.Name = "panOutput";
            this.panOutput.Size = new System.Drawing.Size(680, 111);
            this.panOutput.TabIndex = 0;
            // 
            // WinCreateSelectSpNav
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 464);
            this.Controls.Add(this.TBAbout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBEditer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.GpCondition);
            this.Controls.Add(this.GpOutPut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinCreateSelectSpNav";
            this.Text = "导航";
            this.GpOutPut.ResumeLayout(false);
            this.GpCondition.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GpOutPut;
        private System.Windows.Forms.GroupBox GpCondition;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBEditer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBAbout;
        private System.Windows.Forms.Panel panCondition;
        private System.Windows.Forms.Panel panOutput;
    }
}