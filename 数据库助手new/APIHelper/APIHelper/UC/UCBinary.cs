using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace APIHelper.UC
{
    public partial class UCBinary : UserControl
    {
        public UCBinary()
        {
            InitializeComponent();
        }

        public Dictionary<string,Stream> Files
        {
            get
            {
                Dictionary<string, Stream> dic = new Dictionary<string, Stream>();

                foreach (var item in this.ListFiles.Items)
                {
                    dic.Add(Path.GetFileName(item.ToString()), new FileStream(item.ToString(), FileMode.Open));
                }

                return dic;
            }
        }

        private void BtnSelFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!ListFiles.Items.Contains(dlg.FileName))
                {
                    ListFiles.Items.Add(dlg.FileName);
                }
            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (ListFiles.SelectedIndex != -1)
            {
                ListFiles.Items.RemoveAt(ListFiles.SelectedIndex);
            }
        }
    }
}
