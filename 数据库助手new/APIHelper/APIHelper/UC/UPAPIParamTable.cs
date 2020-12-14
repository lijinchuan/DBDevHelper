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

namespace APIHelper.UC
{
    public partial class UPAPIParamTable : UserControl
    {
        public List<APIParam> RequstParams
        {
            get;
            set;
        }

        private int Type
        {
            get;
            set;
        }

        private int SourceId
        {
            get;
            set;
        }

        private int APIUrlId
        {
            get;
            set;
        }

        public void Init(int type, List<APIParam> data,int sourceid,int apiurlid)
        {
            this.Type = type;
            this.RequstParams = data;
            this.SourceId = sourceid;
            this.APIUrlId = apiurlid;

            DGVRequest.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGVRequest.RowHeadersVisible = false;

            DGVRequest.SelectionMode = DataGridViewSelectionMode.CellSelect;

            DGVRequest.DataBindingComplete += GridView_DataBindingComplete;

            DGVRequest.KeyDown += GridView_KeyDown;
        }

        public UPAPIParamTable()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DGVRequest.DataSource = RequstParams;
        }

        private void BtnReqParamUp_Click(object sender, EventArgs e)
        {
            var currentrow = DGVRequest.CurrentRow;
            if (currentrow == null || currentrow.Index == 0)
            {
                return;
            }

            var currentparam = currentrow.DataBoundItem as APIParam;
            var destSort = currentparam.Sort - 1;
            var destparam = RequstParams.Find(p => p.Sort == destSort);
            if (destparam != null)
            {
                destparam.Sort++;
            }
            currentparam.Sort = destSort;

            RequstParams = RequstParams.OrderBy(p => p.Sort).ToList();
            DGVRequest.DataSource = null;
            DGVRequest.DataSource = RequstParams;
        }

        private void BtnMultiAddReqParams_Click(object sender, EventArgs e)
        {
            if (DGVRequest.Visible)
            {
                TextBox tb = new TextBox();
                tb.Name = "_tbrequestm";
                tb.Multiline = true;
                tb.Location = DGVRequest.Location;
                tb.Anchor = DGVRequest.Anchor;
                tb.Width = DGVRequest.Width;
                tb.Height = DGVRequest.Height;
                DGVRequest.Visible = false;
                DGVRequest.Parent.Controls.Add(tb);
                StringBuilder sb = new StringBuilder();
                foreach (var item in RequstParams)
                {
                    if (Type == 0)
                    {
                        sb.AppendLine($"{item.Name}|{item.TypeName}|{item.IsRequried}|{item.Desc}");
                    }
                    else
                    {
                        sb.AppendLine($"{item.Name}|{item.TypeName}|{item.Desc}");
                    }
                }

                tb.Text = sb.ToString();
            }
            else
            {
                var tb = (TextBox)DGVRequest.Parent.Controls.Find("_tbrequestm", false).FirstOrDefault();
                List<APIParam> tempParams = new List<APIParam>();
                int sort = 0;
                foreach (var line in tb.Lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    sort++;
                    var texts = line.Split('|');
                    if (Type == 0)
                    {
                        if (texts.Length != 4)
                        {
                            Util.SendMsg(this, "格式错误");
                            return;
                        }
                    }
                    else
                    {
                        if (texts.Length != 3)
                        {
                            Util.SendMsg(this, "格式错误");
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(texts[0]))
                    {
                        Util.SendMsg(this, "字段名称不能为空");
                        return;
                    }
                    if (string.IsNullOrEmpty(texts[1]))
                    {
                        Util.SendMsg(this, "类型名称不能为空");
                        return;
                    }

                    var boo = false;
                    if (Type == 0)
                    {
                        if (!string.IsNullOrEmpty(texts[2]) && !bool.TryParse(texts[2], out boo))
                        {
                            Util.SendMsg(this, "是否必填格式错误为空");
                            return;
                        }
                    }

                    var oldparam = RequstParams.Find(p => p.Name == texts[0]);
                    if (oldparam == null)
                    {
                        tempParams.Add(new APIParam
                        {
                            APIId = APIUrlId,
                            APISourceId = SourceId,
                            Name = texts[0],
                            Sort = sort,
                            Type = Type,
                            TypeName = texts[1],
                            IsRequried = boo,
                            Desc = texts.Last()
                        });
                    }
                    else
                    {
                        tempParams.Add(new APIParam
                        {
                            APIId = oldparam.APIId,
                            APISourceId = oldparam.APISourceId,
                            Id = oldparam.Id,
                            Name = oldparam.Name,
                            IsRequried = boo,
                            Sort = sort,
                            Type = oldparam.Type,
                            TypeName = texts[1],
                            Desc = texts.Last()
                        });
                    }
                }
                RequstParams = tempParams.OrderBy(p => p.Sort).ToList();
                tb.Parent.Controls.Remove(tb);
                DGVRequest.Visible = true;
                DGVRequest.DataSource = null;
                DGVRequest.DataSource = RequstParams;
            }
        }

        private void BtnReqParamDown_Click(object sender, EventArgs e)
        {
            var currentrow = DGVRequest.CurrentRow;
            if (currentrow == null || currentrow.Index == DGVRequest.Rows.Count - 1)
            {
                return;
            }

            var currentparam = currentrow.DataBoundItem as APIParam;
            var destSort = currentparam.Sort + 1;
            var destparam = RequstParams.Find(p => p.Sort == destSort);
            if (destparam != null)
            {
                destparam.Sort--;
            }
            currentparam.Sort = destSort;

            RequstParams = RequstParams.OrderBy(p => p.Sort).ToList();
            DGVRequest.DataSource = null;
            DGVRequest.DataSource = RequstParams;
        }

        private void GridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dgv = (sender as DataGridView);
            string[] displayColumns = new[] { "Name", "TypeName", "IsRequried", "Desc" };
            List<DataGridViewColumn> hidcols = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!displayColumns.Contains(col.Name))
                {
                    hidcols.Add(col);
                    continue;
                }
                if (col.Name == "Name")
                {
                    col.HeaderText = "名称";
                    col.Width = 150;
                }
                else if (col.Name == "IsRequried")
                {
                    if (Type == 0)
                    {
                        col.HeaderText = "必填";
                        col.Width = 50;
                    }
                    else
                    {
                        col.Visible = false;
                    }
                }
                else if (col.Name == "TypeName")
                {
                    col.HeaderText = "类型";
                    col.Width = 150;
                }
                else if (col.Name == "Desc")
                {
                    col.HeaderText = "参数描述";
                }
            }

            if (hidcols.Count > 0)
            {
                foreach(var col in hidcols)
                {
                    col.Visible = false;
                }
            }

            if (dgv.Rows.Count > 0)
            {
                if (dgv.Height < dgv.Parent.Height)
                {
                    //dgv.Height = dgv.ColumnHeadersHeight + (dgv.Rows.Count * (dgv.Rows[0].Height + 1));
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
                    var ds = gv.DataSource as List<APIParam>;
                    var sort = 1;
                    if (ds.Count > 0)
                    {
                        sort = ds.Max(p => p.Sort) + 1;
                    }
                    ds.Add(new APIParam()
                    {
                        APIId = APIUrlId,
                        APISourceId = SourceId,
                        Sort = sort,
                        Type=Type
                    });
                    gv.DataSource = null;
                    gv.DataSource = ds;
                }

            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (gv.CurrentCell != null)
                {
                    var ds = gv.DataSource as List<APIParam>;
                    ds.RemoveAt(gv.CurrentCell.RowIndex);
                    var sort = 0;
                    ds.ForEach(p => p.Sort = ++sort);
                    gv.DataSource = null;
                    gv.DataSource = ds;
                }
            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (DGVRequest.CurrentRow == null)
            {
                return;
            }

            RequstParams.RemoveAt(DGVRequest.CurrentRow.Index);

            var sort = 0;
            foreach(var item in RequstParams)
            {
                sort++;
                item.Sort = sort;
            }

            DGVRequest.DataSource = null;
            DGVRequest.DataSource = RequstParams;
        }
    }
}
