using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class MD5Dlg : SubBaseDlg
    {
        public MD5Dlg()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DGV.AllowUserToAddRows = false;
            DGV.ReadOnly = true;
            DGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DGV.BackgroundColor = Color.White;
            DGV.GridColor = Color.LightBlue;

            DGV.BorderStyle = BorderStyle.None;
            DGV.RowHeadersVisible = false;
            DGV.ColumnHeadersVisible = false;

            DGV.CellDoubleClick += DGV_CellDoubleClick;
            DGV.DataBindingComplete += DGV_DataBindingComplete;
        }

        private void DGV_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.Height = this.DGV.Location.Y + this.DGV.Height + 2;
        }

        private void DGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex > 0)
            {
                var md5 = DGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Clipboard.SetText(md5);
                Util.SendMsg(this, "已复制到剪贴板");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var md532 = LJC.FrameWorkV3.Comm.HashEncrypt.MD5_JS(TBWord.Text);
            var md516 = md532.Substring(8, 24);

            DGV.DataSource = new[]
            {
                new
                {
                    word="16位大写",
                    js=md516.ToUpper()
                },
                new
                {
                    word="16位小写",
                    js=md516.ToLower()
                },
                new
                {
                    word="32位大写",
                    js=md532.ToUpper()
                },
                new
                {
                    word="32位小写",
                    js=md532.ToLower()
                },
            };
        }
    }
}
