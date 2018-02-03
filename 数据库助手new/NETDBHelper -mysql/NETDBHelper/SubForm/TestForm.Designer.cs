namespace NETDBHelper.SubForm
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableCombox1 = new NETDBHelper.UC.TableCombox();
            this.multSelectCombox1 = new NETDBHelper.UC.MultSelectCombox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.GrayText;
            this.dataGridView1.Location = new System.Drawing.Point(46, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(185, 73);
            this.dataGridView1.TabIndex = 2;
            // 
            // tableCombox1
            // 
            this.tableCombox1.AutoSize = true;
            this.tableCombox1.DataSource = null;
            this.tableCombox1.Location = new System.Drawing.Point(146, 154);
            this.tableCombox1.Name = "tableCombox1";
            this.tableCombox1.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("tableCombox1.SelectedValues")));
            this.tableCombox1.Size = new System.Drawing.Size(195, 26);
            this.tableCombox1.TabIndex = 1;
            // 
            // multSelectCombox1
            // 
            this.multSelectCombox1.AutoSize = true;
            this.multSelectCombox1.BackColor = System.Drawing.SystemColors.Control;
            this.multSelectCombox1.DataSource = null;
            this.multSelectCombox1.Location = new System.Drawing.Point(155, 105);
            this.multSelectCombox1.Name = "multSelectCombox1";
            this.multSelectCombox1.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("multSelectCombox1.SelectedValues")));
            this.multSelectCombox1.Size = new System.Drawing.Size(186, 25);
            this.multSelectCombox1.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 395);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tableCombox1);
            this.Controls.Add(this.multSelectCombox1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UC.MultSelectCombox multSelectCombox1;
        private UC.TableCombox tableCombox1;
        private System.Windows.Forms.DataGridView dataGridView1;



    }
}