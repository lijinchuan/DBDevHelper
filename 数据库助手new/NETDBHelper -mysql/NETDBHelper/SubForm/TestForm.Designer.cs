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
            this.multSelectCombox1 = new NETDBHelper.UC.MultSelectCombox();
            this.SuspendLayout();
            // 
            // multSelectCombox1
            // 
            this.multSelectCombox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.multSelectCombox1.DataSource = null;
            this.multSelectCombox1.Location = new System.Drawing.Point(150, 100);
            this.multSelectCombox1.Name = "multSelectCombox1";
            this.multSelectCombox1.SelectedValues = null;
            this.multSelectCombox1.Size = new System.Drawing.Size(194, 35);
            this.multSelectCombox1.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 395);
            this.Controls.Add(this.multSelectCombox1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private UC.MultSelectCombox multSelectCombox1;

    }
}