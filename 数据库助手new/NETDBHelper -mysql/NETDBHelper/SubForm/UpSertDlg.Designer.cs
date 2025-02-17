﻿
namespace NETDBHelper.SubForm
{
    partial class UpSertDlg
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
            this.GBValues = new System.Windows.Forms.GroupBox();
            this.ItemsPannel = new System.Windows.Forms.Panel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.LBNotice = new System.Windows.Forms.Label();
            this.TBNotice = new System.Windows.Forms.TextBox();
            this.GBValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // GBValues
            // 
            this.GBValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBValues.Controls.Add(this.ItemsPannel);
            this.GBValues.Location = new System.Drawing.Point(12, 12);
            this.GBValues.Name = "GBValues";
            this.GBValues.Size = new System.Drawing.Size(776, 397);
            this.GBValues.TabIndex = 0;
            this.GBValues.TabStop = false;
            this.GBValues.Text = "编辑值";
            // 
            // ItemsPannel
            // 
            this.ItemsPannel.AutoScroll = true;
            this.ItemsPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemsPannel.Location = new System.Drawing.Point(3, 17);
            this.ItemsPannel.Name = "ItemsPannel";
            this.ItemsPannel.Size = new System.Drawing.Size(770, 377);
            this.ItemsPannel.TabIndex = 0;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.Location = new System.Drawing.Point(659, 420);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(58, 23);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "放弃";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(729, 419);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(59, 23);
            this.BtnOk.TabIndex = 2;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnReset
            // 
            this.BtnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnReset.Location = new System.Drawing.Point(569, 420);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(75, 23);
            this.BtnReset.TabIndex = 3;
            this.BtnReset.Text = "重置";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // LBNotice
            // 
            this.LBNotice.AutoSize = true;
            this.LBNotice.ForeColor = System.Drawing.Color.Blue;
            this.LBNotice.Location = new System.Drawing.Point(13, 424);
            this.LBNotice.Name = "LBNotice";
            this.LBNotice.Size = new System.Drawing.Size(0, 12);
            this.LBNotice.TabIndex = 4;
            // 
            // TBNotice
            // 
            this.TBNotice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBNotice.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TBNotice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBNotice.ForeColor = System.Drawing.Color.Blue;
            this.TBNotice.Location = new System.Drawing.Point(15, 424);
            this.TBNotice.Name = "TBNotice";
            this.TBNotice.ReadOnly = true;
            this.TBNotice.Size = new System.Drawing.Size(548, 14);
            this.TBNotice.TabIndex = 5;
            // 
            // UpSertDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TBNotice);
            this.Controls.Add(this.LBNotice);
            this.Controls.Add(this.BtnReset);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.GBValues);
            this.MaximizeBox = true;
            this.Name = "UpSertDlg";
            this.Text = "UpSertDlg";
            this.GBValues.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GBValues;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Panel ItemsPannel;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Label LBNotice;
        private System.Windows.Forms.TextBox TBNotice;
    }
}