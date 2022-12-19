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
        public Action<TreeNode> Choose;

        public SearchResultsDlg()
        {
            InitializeComponent();

            DGVResult.DataBindingComplete += DGVResult_DataBindingComplete;
            DGVResult.DoubleClick += DGVResult_DoubleClick;
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

            this.DGVResult.BorderStyle = BorderStyle.None;
            this.DGVResult.GridColor = Color.LightBlue;

            this.DGVResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DGVResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVResult.AllowUserToResizeRows = true;

            this.DGVResult.DataSource = Ds;
        }
    }
}
