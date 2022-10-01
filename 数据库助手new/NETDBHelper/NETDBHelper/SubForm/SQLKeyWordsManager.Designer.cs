
namespace NETDBHelper.SubForm
{
    partial class SQLKeyWordsManager
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CBColor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.TbKeyWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CBKeyType = new System.Windows.Forms.ComboBox();
            this.GVKeyWordList = new System.Windows.Forms.DataGridView();
            this.LBNavList = new System.Windows.Forms.ListBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.LBHidId = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GVKeyWordList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.LBNavList);
            this.panel1.Controls.Add(this.GVKeyWordList);
            this.panel1.Location = new System.Drawing.Point(2, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 310);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.LBHidId);
            this.panel2.Controls.Add(this.BtnReset);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Controls.Add(this.CBKeyType);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.TBDesc);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.CBColor);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.BtnAdd);
            this.panel2.Controls.Add(this.TbKeyWord);
            this.panel2.Location = new System.Drawing.Point(2, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(786, 115);
            this.panel2.TabIndex = 1;
            // 
            // CBColor
            // 
            this.CBColor.FormattingEnabled = true;
            this.CBColor.Location = new System.Drawing.Point(420, 8);
            this.CBColor.Name = "CBColor";
            this.CBColor.Size = new System.Drawing.Size(167, 20);
            this.CBColor.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "关键字：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(373, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "颜色：";
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(706, 7);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(62, 23);
            this.BtnAdd.TabIndex = 1;
            this.BtnAdd.Text = "添加";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // TbKeyWord
            // 
            this.TbKeyWord.Location = new System.Drawing.Point(69, 9);
            this.TbKeyWord.Name = "TbKeyWord";
            this.TbKeyWord.Size = new System.Drawing.Size(147, 21);
            this.TbKeyWord.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "说明：";
            // 
            // TBDesc
            // 
            this.TBDesc.Location = new System.Drawing.Point(69, 36);
            this.TBDesc.Multiline = true;
            this.TBDesc.Name = "TBDesc";
            this.TBDesc.Size = new System.Drawing.Size(699, 76);
            this.TBDesc.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "类型：";
            // 
            // CBKeyType
            // 
            this.CBKeyType.FormattingEnabled = true;
            this.CBKeyType.Location = new System.Drawing.Point(266, 8);
            this.CBKeyType.Name = "CBKeyType";
            this.CBKeyType.Size = new System.Drawing.Size(100, 20);
            this.CBKeyType.TabIndex = 8;
            // 
            // GVKeyWordList
            // 
            this.GVKeyWordList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.GVKeyWordList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GVKeyWordList.Dock = System.Windows.Forms.DockStyle.Right;
            this.GVKeyWordList.Location = new System.Drawing.Point(81, 0);
            this.GVKeyWordList.Name = "GVKeyWordList";
            this.GVKeyWordList.RowTemplate.Height = 23;
            this.GVKeyWordList.Size = new System.Drawing.Size(705, 310);
            this.GVKeyWordList.TabIndex = 0;
            // 
            // LBNavList
            // 
            this.LBNavList.Dock = System.Windows.Forms.DockStyle.Left;
            this.LBNavList.FormattingEnabled = true;
            this.LBNavList.ItemHeight = 12;
            this.LBNavList.Location = new System.Drawing.Point(0, 0);
            this.LBNavList.Name = "LBNavList";
            this.LBNavList.Size = new System.Drawing.Size(72, 310);
            this.LBNavList.TabIndex = 1;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(593, 6);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(53, 23);
            this.BtnSearch.TabIndex = 9;
            this.BtnSearch.Text = "查找";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // BtnReset
            // 
            this.BtnReset.Location = new System.Drawing.Point(652, 7);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(48, 23);
            this.BtnReset.TabIndex = 10;
            this.BtnReset.Text = "重置";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // LBHidId
            // 
            this.LBHidId.AutoSize = true;
            this.LBHidId.Location = new System.Drawing.Point(10, 87);
            this.LBHidId.Name = "LBHidId";
            this.LBHidId.Size = new System.Drawing.Size(17, 12);
            this.LBHidId.TabIndex = 11;
            this.LBHidId.Text = "id";
            this.LBHidId.Visible = false;
            // 
            // SQLKeyWordsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SQLKeyWordsManager";
            this.Text = "关键字管理";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GVKeyWordList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox TbKeyWord;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CBColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBDesc;
        private System.Windows.Forms.ComboBox CBKeyType;
        private System.Windows.Forms.DataGridView GVKeyWordList;
        private System.Windows.Forms.ListBox LBNavList;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Label LBHidId;
    }
}