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
        public bool CanUpload
        {
            get;
            set;
        }
        public UCParamsTable()
        {

            InitializeComponent();
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGV.DataBindingComplete += GridView_DataBindingComplete;
            DGV.AllowUserToAddRows = false;

            DGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DGV.KeyDown += GridView_KeyDown;
            DGV.BackgroundColor = Color.White;
            DGV.GridColor = Color.LightBlue;

            DGV.BorderStyle = BorderStyle.None;
            DGV.RowHeadersVisible = false;

            DGV.DataError += DGV_DataError;


            CBEditType.SelectedIndex = 0;
            CBEditType.SelectedIndexChanged += CBEditType_SelectedIndexChanged;

            TBBulkEdit.Visible = false;
            TBBulkEdit.WordWrap = false;
            TBBulkEdit.Multiline = true;
            TBBulkEdit.Location = DGV.Location;
            TBBulkEdit.Width = DGV.Width;
            TBBulkEdit.Height = DGV.Height;
            TBBulkEdit.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Controls.Add(TBBulkEdit);

            this.DGV.ContextMenuStrip = this.contextMenuStrip1;

            this.DGV.CellContentClick += DGV_CellContentClick;
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击button按钮事件
            if (DGV.Columns[e.ColumnIndex].Name == "file" && e.RowIndex >= 0)
            {
                //说明点击的列是DataGridViewButtonColumn列
                DataGridViewColumn column = DGV.Columns[e.ColumnIndex];

                SubForm.FileDlg dlg = new SubForm.FileDlg();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var list = this.DataSource as List<ParamInfo>;
                    list[e.RowIndex].Value = dlg.FilePath;
                    DataSource = list;
                }
            }
        }

        private void DGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }

        public object DataSource
        {
            get
            {
                return (DGV.DataSource as BindingSource)?.DataSource;
            }
            set
            {
                if (DGV.DataSource == null)
                {
                    BindingSource bs = new BindingSource();
                    bs.DataSource = value;
                    DGV.DataSource = bs;
                }
                else
                {
                    BindingSource bs = DGV.DataSource as BindingSource;
                    bs.DataSource = null;
                    bs.DataSource = value;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PbAddParams.Image = Resources.SQLTypeRs.control_add;
            PbAddParams.Height = Resources.SQLTypeRs.control_add.Height;
            PbAddParams.Width = Resources.SQLTypeRs.control_add.Width;
            PbAddParams.Click += PbAddParams_Click;
            PbAddParams.Cursor = Cursors.Hand;

            PbRemParams.Image = Resources.SQLTypeRs.control_remove;
            PbRemParams.Height = Resources.SQLTypeRs.control_remove.Height;
            PbRemParams.Width = Resources.SQLTypeRs.control_remove.Width;
            PbRemParams.Click += PbRemParams_Click;
            PBHelp.Cursor = Cursors.Hand;

            PBHelp.Image = Resources.Resource1.help;
            PBHelp.Height = Resources.Resource1.help.Height;
            PBHelp.Width = Resources.Resource1.help.Width;
            PBHelp.Click += PBHelp_Click;
            PBHelp.Cursor = Cursors.Help;
        }

        private void PBHelp_Click(object sender, EventArgs e)
        {
            var page = new UC.DocPage();
            Util.AddToMainTab(this, $"帮助文档-请求参数", page);
            page.InitDoc(Application.StartupPath + "\\help.html#inputparams", null);
        }

        private void PbRemParams_Click(object sender, EventArgs e)
        {
            RemParamsRow();
        }

        private void PbAddParams_Click(object sender, EventArgs e)
        {
            AddNewParamsRow();
        }

        private void CBEditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBEditType.SelectedIndex == 1)
            {
                DGV.Visible = false;
                TBBulkEdit.Visible = true;
                var ds = DataSource as List<ParamInfo>;
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
                var regex = new Regex("(--)?([^:]+):((.*)(?://))([^\r\n/]*$)");
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
                        Value = m.Groups[4].Value,
                        Desc = m.Groups[5].Value
                    });
                }
                
                var oldds=(DataSource as List<ParamInfo>);
                oldds.Clear();
                oldds.AddRange(paramInfos);

                DataSource = oldds;

                DGV.Visible = true;
                TBBulkEdit.Visible = false;
            }
        }

        private void GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dgv = (sender as DataGridView);
            var filecolumn = "file";
            if (CanUpload)
            {
                if (dgv.Columns.Contains(filecolumn))
                {
                    dgv.Columns.Remove(filecolumn);
                }
            }
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Name == "Checked")
                {
                    col.HeaderText = "";
                    col.Width = 30;
                }
                else if (col.Name == "Name")
                {
                    col.HeaderText = "参数名称";
                }
                else if (col.Name == "Value")
                {
                    ((DataGridViewTextBoxColumn)dgv.Columns["Value"]).MaxInputLength = 1024 * 1024 * 10;
                    col.HeaderText = "参数值";
                }
                else if (col.Name == "Desc")
                {
                    col.HeaderText = "参数描述";
                }
            }

            if (CanUpload)
            {
                if (dgv.Columns.Count > 0 && !dgv.Columns.Contains(filecolumn))
                {
                    DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                    btn.Name = filecolumn;
                    btn.HeaderText = "上传";
                    btn.Width = 40;
                    btn.DefaultCellStyle.NullValue = "上传";
                    dgv.Columns.Insert(dgv.Columns.Count, btn);
                }
            }
        }

        private void AddNewParamsRow()
        {
            var ds = DataSource as List<ParamInfo>;
            if (ds == null)
            {
                ds = new List<ParamInfo>();
            }
            ds.Add(new ParamInfo());
            DataSource = ds;
        }

        private void RemParamsRow()
        {
            if (DGV.CurrentCell != null)
            {
                var ds = DataSource as List<ParamInfo>;
                ds.RemoveAt(DGV.CurrentCell.RowIndex);
                DataSource = ds;
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
                    AddNewParamsRow();
                }
                e.Handled = true;

            }
            else if (e.KeyCode == Keys.Delete)
            {
                RemParamsRow();
                e.Handled = true;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.DGV.Rows.Count > 0)
            {
                this.DGV.SelectAll();
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cells = new List<DataGridViewCell>();
            foreach (DataGridViewCell cell in DGV.SelectedCells)
            {
                cells.Add(cell);
            }
            foreach (var row in cells.GroupBy(p => p.RowIndex))
            {
                sb.AppendLine(string.Join(":", row.OrderBy(p => p.ColumnIndex).Select(p => p.Value)));
            }
            if (sb.Length > 0)
            {
                Clipboard.SetText(sb.ToString());
            }
        }
    }
}
