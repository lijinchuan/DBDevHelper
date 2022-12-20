using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class SearchResultsDlg : SubBaseDlg
    {
        public Func<TreeNode, bool> Choose;

        public SearchResultsDlg()
        {
            InitializeComponent();

            DGVResult.DataBindingComplete += DGVResult_DataBindingComplete;
            DGVResult.DoubleClick += DGVResult_DoubleClick;

            Text = "搜索结果";
        }

        private void DGVResult_DoubleClick(object sender, EventArgs e)
        {
            if (DGVResult.CurrentRow != null)
            {
                var node = (TreeNode)DGVResult.CurrentRow.Cells["obj"].Value;
                Choose?.Invoke(node);
            }
        }

        private void DGVResult_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DGVResult.Columns["obj"].Visible = false;
        }

        public object Ds
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            DGVResult.BorderStyle = System.Windows.Forms.BorderStyle.None;

            DGVResult.GridColor = Color.LightBlue;
            DGVResult.Dock = DockStyle.Fill;
            DGVResult.BackgroundColor = Color.White;
            DGVResult.AllowUserToAddRows = false;
            DGVResult.ReadOnly = true;
            DGVResult.RowHeadersDefaultCellStyle.ForeColor = Color.Red;

            DGVResult.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            DGVResult.DefaultCellStyle.SelectionForeColor = Color.Black;

            this.DGVResult.BorderStyle = BorderStyle.None;
            this.DGVResult.GridColor = Color.LightBlue;

            this.DGVResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            DGVResult.RowStateChanged += (s, ee) =>
            {
                ee.Row.HeaderCell.Value = string.Format("{0}", ee.Row.Index + 1);

            };

            DGVResult.DataSource = Ds;
        }
    }
}
