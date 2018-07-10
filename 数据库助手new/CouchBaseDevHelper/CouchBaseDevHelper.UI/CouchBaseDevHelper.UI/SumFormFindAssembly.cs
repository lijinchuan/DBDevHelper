using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouchBaseDevHelper.UI
{
    public partial class SumFormFindAssembly : Form
    {
        private string _assemblyname = string.Empty;

        public SumFormFindAssembly()
        {
            InitializeComponent();
        }

        public SumFormFindAssembly(string assemblyname)
        {
            InitializeComponent();

            this._assemblyname = assemblyname;
            lbtext.Text = string.Format("寻找程序集体“{0}”",assemblyname);
            
        }

        public string FilePath
        {
            get
            {
                return tbpath.Text;
            }
        }

        private void butcanel_Click(object sender, EventArgs e)
        {      
            this.DialogResult = DialogResult.Cancel;           
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            var path = tbpath.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("请选择文件路径");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            if(fd.ShowDialog()==DialogResult.OK)
            {
                tbpath.Text = fd.FileName;
            }
        }
    }
}
