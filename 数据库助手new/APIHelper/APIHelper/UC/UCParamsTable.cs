using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using System.Text.RegularExpressions;

namespace APIHelper.UC
{
    public partial class UCParamsTable : UserControl
    {
        //private TextBox TBBulkEdit = new TextBox();
        private RichTextBox TBBulkEdit = new RichTextBox();
        public UCParamsTable()
        {
            InitializeComponent();
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGV.DataBindingComplete += GridView_DataBindingComplete;

            DGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DGV.KeyDown += GridView_KeyDown;
            DGV.BackgroundColor = Color.White;
            DGV.GridColor = Color.LightBlue;

            DGV.BorderStyle = BorderStyle.None;
            DGV.RowHeadersVisible = false;


            CBEditType.SelectedIndex = 0;
            CBEditType.SelectedIndexChanged += CBEditType_SelectedIndexChanged;

            TBBulkEdit.Visible = false;
            TBBulkEdit.Multiline = true;
            TBBulkEdit.Location = DGV.Location;
            TBBulkEdit.Width = DGV.Width;
            TBBulkEdit.Height = DGV.Height;
            TBBulkEdit.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Controls.Add(TBBulkEdit);
        }

        public object DataSource
        {
            get
            {
                return DGV.DataSource;
            }
            set
            {
                DGV.DataSource = null;
                DGV.DataSource = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void CBEditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBEditType.SelectedIndex == 1)
            {
                DGV.Visible = false;
                TBBulkEdit.Visible = true;
                var ds = DGV.DataSource as List<ParamInfo>;
                if (ds == null || ds.Count == 0)
                {
                    TBBulkEdit.Text = "";
                }
                else
                {
                    TBBulkEdit.Text = string.Join("\n", ds.Select(p => $"{(p.Checked ? "" : "--")}{p.Name}:{p.Value}//{p.Desc}"));
                }
            }
            else
            {
                DGV.Visible = true;
                TBBulkEdit.Visible = false;

                var regex = new Regex("(--)?([^:]+):([^/]*)((?://)[^\r\n]*)?");
                List<ParamInfo> paramInfos = new List<ParamInfo>();
                foreach (var line in TBBulkEdit.Text.Split(new[] { "\n" }, StringSplitOptions.None))
                {
                    var m = regex.Match(line);
                    if (!m.Success)
                    {
                        MessageBox.Show("格式错误");
                        return;
                    }

                    paramInfos.Add(new ParamInfo
                    {
                        Checked = string.IsNullOrWhiteSpace(m.Groups[1].Value),
                        Name = m.Groups[2].Value,
                        Value = m.Groups[3].Value,
                        Desc = m.Groups[4].Value.Split(new[] { "//" }, StringSplitOptions.None).Last()
                    });
                }

                var oldds=DGV.DataSource as List<ParamInfo>;
                oldds.Clear();
                oldds.AddRange(paramInfos);

                DGV.DataSource = null;
                DGV.DataSource = oldds;
                
            }
        }

        private void GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dgv = (sender as DataGridView);
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.HeaderText == "Checked")
                {
                    col.HeaderText = "";
                    col.Width = 30;
                }
                else if (col.HeaderText == "Name")
                {
                    col.HeaderText = "参数名称";
                }
                else if (col.HeaderText == "Value")
                {
                    col.HeaderText = "参数值";
                }
                else if (col.HeaderText == "Desc")
                {
                    col.HeaderText = "参数描述";
                }
            }
        }

        private void GridView_KeyDown(object sender, KeyEventArgs e)
        {
            var gv = (DataGridView)sender;
            if (e.KeyCode == Keys.Enter)
            {
                var cell = gv.CurrentCell;

                if (gv.Rows.Count == 0 || (cell != null && cell == gv.Rows[gv.Rows.Count - 1].Cells[gv.Rows[gv.Rows.Count - 1].Cells.Count - 1]))
                {
                    var ds = gv.DataSource as List<ParamInfo>;
                    ds.Add(new ParamInfo());
                    gv.DataSource = null;
                    gv.DataSource = ds;
                }

            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (gv.CurrentCell != null)
                {
                    var ds = gv.DataSource as List<ParamInfo>;
                    ds.RemoveAt(gv.CurrentCell.RowIndex);
                    gv.DataSource = null;
                    gv.DataSource = ds;
                }
            }
        }

    }
}
