namespace APIHelper.SubForm
{
    partial class WatchTaskLogDlg
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
            this.DGVLog = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.DTStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.DTEnd = new System.Windows.Forms.DateTimePicker();
            this.BtnSearchLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVLog)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVLog
            // 
            this.DGVLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVLog.BackgroundColor = System.Drawing.Color.White;
            this.DGVLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DGVLog.Location = new System.Drawing.Point(0, 29);
            this.DGVLog.Name = "DGVLog";
            this.DGVLog.RowTemplate.Height = 23;
            this.DGVLog.Size = new System.Drawing.Size(800, 421);
            this.DGVLog.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "查询时间";
            // 
            // DTStart
            // 
            this.DTStart.Location = new System.Drawing.Point(73, 5);
            this.DTStart.Name = "DTStart";
            this.DTStart.Size = new System.Drawing.Size(113, 21);
            this.DTStart.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "-";
            // 
            // DTEnd
            // 
            this.DTEnd.Location = new System.Drawing.Point(211, 5);
            this.DTEnd.Name = "DTEnd";
            this.DTEnd.Size = new System.Drawing.Size(111, 21);
            this.DTEnd.TabIndex = 4;
            // 
            // BtnSearchLog
            // 
            this.BtnSearchLog.Location = new System.Drawing.Point(329, 4);
            this.BtnSearchLog.Name = "BtnSearchLog";
            this.BtnSearchLog.Size = new System.Drawing.Size(75, 23);
            this.BtnSearchLog.TabIndex = 5;
            this.BtnSearchLog.Text = "搜索";
            this.BtnSearchLog.UseVisualStyleBackColor = true;
            this.BtnSearchLog.Click += new System.EventHandler(this.BtnSearchLog_Click);
            // 
            // WatchTaskLogDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnSearchLog);
            this.Controls.Add(this.DTEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DTStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DGVLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WatchTaskLogDlg";
            this.ShowIcon = false;
            this.Text = "监控日志";
            ((System.ComponentModel.ISupportInitialize)(this.DGVLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DTStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DTEnd;
        private System.Windows.Forms.Button BtnSearchLog;
    }
}