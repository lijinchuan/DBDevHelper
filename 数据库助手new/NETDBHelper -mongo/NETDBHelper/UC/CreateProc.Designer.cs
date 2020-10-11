namespace NETDBHelper.UC
{
    partial class CreateProc
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
            this.editTextBox1 = new NETDBHelper.UC.SQLEditBox();
            this.SuspendLayout();
            // 
            // editTextBox1
            // 
            this.editTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTextBox1.Location = new System.Drawing.Point(0, 0);
            this.editTextBox1.Name = "editTextBox1";
            this.editTextBox1.Size = new System.Drawing.Size(743, 505);
            this.editTextBox1.TabIndex = 0;
            // 
            // CreateProc
            // 
            this.Controls.Add(this.editTextBox1);
            this.Name = "CreateProc";
            this.Size = new System.Drawing.Size(743, 505);
            this.ResumeLayout(false);

        }

        #endregion

        private SQLEditBox editTextBox1;

    }
}
