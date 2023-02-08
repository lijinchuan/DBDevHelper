namespace APIHelper.SubForm
{
    partial class SimulateServerDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.TBLocalPort = new System.Windows.Forms.TextBox();
            this.CBOpen = new System.Windows.Forms.CheckBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.LBErrorMsg1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "本地端口:";
            // 
            // TBLocalPort
            // 
            this.TBLocalPort.Location = new System.Drawing.Point(96, 30);
            this.TBLocalPort.Name = "TBLocalPort";
            this.TBLocalPort.Size = new System.Drawing.Size(100, 21);
            this.TBLocalPort.TabIndex = 1;
            this.TBLocalPort.TextChanged += new System.EventHandler(this.TBLocalPort_TextChanged);
            // 
            // CBOpen
            // 
            this.CBOpen.AutoSize = true;
            this.CBOpen.Location = new System.Drawing.Point(96, 77);
            this.CBOpen.Name = "CBOpen";
            this.CBOpen.Size = new System.Drawing.Size(48, 16);
            this.CBOpen.TabIndex = 2;
            this.CBOpen.Text = "开启";
            this.CBOpen.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(128, 137);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(58, 23);
            this.BtnSave.TabIndex = 3;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(203, 137);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // LBErrorMsg1
            // 
            this.LBErrorMsg1.AutoSize = true;
            this.LBErrorMsg1.ForeColor = System.Drawing.Color.Red;
            this.LBErrorMsg1.Location = new System.Drawing.Point(201, 35);
            this.LBErrorMsg1.Name = "LBErrorMsg1";
            this.LBErrorMsg1.Size = new System.Drawing.Size(0, 12);
            this.LBErrorMsg1.TabIndex = 5;
            // 
            // SimulateServerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 168);
            this.Controls.Add(this.LBErrorMsg1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.CBOpen);
            this.Controls.Add(this.TBLocalPort);
            this.Controls.Add(this.label1);
            this.Name = "SimulateServerDlg";
            this.Text = "模拟服务器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBLocalPort;
        private System.Windows.Forms.CheckBox CBOpen;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label LBErrorMsg1;
    }
}