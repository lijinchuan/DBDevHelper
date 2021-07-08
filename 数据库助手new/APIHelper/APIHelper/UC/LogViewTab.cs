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
using LJC.FrameWorkV3.Data.EntityDataBase;
using System.Threading;

namespace APIHelper.UC
{
    public partial class LogViewTab : UserControl
    {
        int pageSize = 20;
        UC.LoadingBox loadbox = new LoadingBox();

        public event Action<APIInvokeLog,bool> ReInvoke;

        int _apiid = 0,_envid=0;

        public void Init(int apiid, int envid)
        {
            if (apiid != _apiid || envid != _envid)
            {
                _apiid = apiid;
                _envid = envid;

            }

            this.TBSearchKey.Visible = apiid > 0;

            this.PageIndex = 1;
            BindData();
        }

        public LogViewTab()
        {
            InitializeComponent();
            PageIndex = 1;
            this.bindingNavigatorMoveLastItem.Click += BindingNavigatorMoveLastItem_Click;
            this.bindingNavigatorMoveNextItem.Click += BindingNavigatorMoveNextItem_Click;
            this.bindingNavigatorMoveFirstItem.Click += BindingNavigatorMoveFirstItem_Click;
            this.bindingNavigatorMovePreviousItem.Click += BindingNavigatorMovePreviousItem_Click;

            this.bindingNavigatorDeleteItem.Click += BindingNavigatorDeleteItem_Click;

            this.GVLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.GVLog.ContextMenuStrip = new ContextMenuStrip();
            this.GVLog.ContextMenuStrip.Items.Add("复制");
            this.GVLog.ContextMenuStrip.Items.Add("备注");
            this.GVLog.ContextMenuStrip.Items.Add("查看文本");
            this.GVLog.ContextMenuStrip.Items.Add("查看请求");
            this.GVLog.ContextMenuStrip.Items.Add("查看结果");
            this.GVLog.ContextMenuStrip.Items.Add("再次执行");
            this.GVLog.ContextMenuStrip.Items.Add("带参数再次执行");
            this.GVLog.ContextMenuStrip.Items.Add("添加到示例");
            this.GVLog.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
            this.GVLog.CellDoubleClick += GVLog_CellDoubleClick;
            this.GVLog.BorderStyle = BorderStyle.None;
            this.GVLog.GridColor = Color.LightBlue;

            this.GVLog.AllowUserToResizeRows = true;

            BeginDate.Value = DateTime.Now.AddMonths(-1);
            EndDate.Value = DateTime.Now.AddDays(1);
            TBSearchKey.GotFocus += TBSearchKey_GotFocus;
            TBSearchKey.LostFocus += TBSearchKey_LostFocus;
            TBSearchKey.TextChanged += TBSearchKey_TextChanged;

            GVLog.RowHeadersVisible = false;
            GVLog.DataBindingComplete += GVLog_DataBindingComplete;

            this.AutoScroll = true;
        }

        public void SetPageSize(int size)
        {
            if (size > 0)
            {
                this.pageSize = size;
            }
        }

        private void GVLog_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (GVLog.Columns.Contains("编号"))
            {
                GVLog.Columns["编号"].Visible = false;
            }

            if (GVLog.Columns.Contains("时间"))
            {
                GVLog.Columns["时间"].Width = 120;
            }

            if (GVLog.Columns.Contains("地址"))
            {
                GVLog.Columns["地址"].Width = 200;
            }

            if (GVLog.Columns.Contains("请求大小"))
            {
                GVLog.Columns["请求大小"].Width = 100;
            }

            if (GVLog.Columns.Contains("响应大小"))
            {
                GVLog.Columns["响应大小"].Width = 100;
            }

            if (GVLog.Columns.Contains("状态码"))
            {
                GVLog.Columns["状态码"].Width = 80;
            }

            if (GVLog.Columns.Contains("用时"))
            {
                GVLog.Columns["用时"].Width = 100;
            }

            if (GVLog.Rows.Count > 0)
            {
                GVLog.Height = GVLog.Rows[0].Height * GVLog.RowCount + GVLog.ColumnHeadersHeight;
            }
        }

        private void TBSearchKey_GotFocus(object sender, EventArgs e)
        {
            if (TBSearchKey.Text.Equals(TBSearchKey.Tag))
            {
                TBSearchKey.Text = string.Empty;
                TBSearchKey.ForeColor = Color.Black;
            }
        }

        private void TBSearchKey_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBSearchKey.Text))
            {
                TBSearchKey.Text = TBSearchKey.Tag.ToString();
                TBSearchKey.ForeColor = Color.Gray;
            }
        }

        private void TBSearchKey_TextChanged(object sender, EventArgs e)
        {
            this.PageIndex = 1;
        }

        private void GVLog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = GVLog.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Style.WrapMode == DataGridViewTriState.True)
            {
                cell.Style.WrapMode = DataGridViewTriState.False;
                cell.ReadOnly = true;
                GVLog.EndEdit();
            }
            else
            {
                cell.Style.WrapMode = DataGridViewTriState.True;
                GVLog.ReadOnly = false;
                cell.ReadOnly = false;
                GVLog.BeginEdit(true);
            }
        }

        private void BindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            var logs = GVLog.SelectedRows;
            if(logs.Count>0&&MessageBox.Show("要删除"+logs.Count+"条数据吗？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                foreach(DataGridViewRow log in logs)
                {
                    BigEntityTableEngine.LocalEngine.Delete<APIInvokeLog>(nameof(APIInvokeLog), (int)log.Cells["编号"].Value);
                }
                BindData();
            }
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "复制")
            {
                var cells = GVLog.SelectedCells;
                if (cells.Count > 0)
                {
                    List<string> list = new List<string>();
                    foreach (DataGridViewCell cell in cells)
                    {
                        list.Add(cell.Value?.ToString());
                    }
                    Clipboard.SetText(string.Join("\t", list));
                }
            }
            else if (e.ClickedItem.Text == "备注")
            {
                var rows = GVLog.SelectedRows;
                if (rows.Count > 0)
                {
                    SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("备注");

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        var logid = (int)rows[0].Cells["编号"].Value;
                        var log = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<APIInvokeLog>(nameof(APIInvokeLog), logid);
                        if (log != null)
                        {
                            //log. = dlg.InputString;
                            BigEntityTableEngine.LocalEngine.Update<APIInvokeLog>(nameof(APIInvokeLog), log);
                            BindData();
                        }
                    }
                }
            }
            else if (e.ClickedItem.Text == "查看文本")
            {
                var cells = GVLog.SelectedCells;
                if (cells.Count > 0)
                {
                    List<string> list = new List<string>();
                    foreach (DataGridViewCell cell in cells)
                    {
                        list.Add(cell.Value?.ToString());
                    }
                    SubForm.TextBoxWin win = new SubForm.TextBoxWin($"查看文本", string.Join("\t", list));
                    win.ShowDialog();
                }
            }
            else if (e.ClickedItem.Text == "查看请求")
            {
                var row = GVLog.CurrentRow;
                if (row != null)
                {
                    var id = (int)row.Cells["编号"].Value;
                    var log = BigEntityTableEngine.LocalEngine.Find<APIInvokeLog>(nameof(APIInvokeLog), id);
                    SubForm.TextBoxWin win = new SubForm.TextBoxWin($"查看文本", log?.GetRequestDetail().ToString());
                    win.ShowDialog();
                }
            }
            else if (e.ClickedItem.Text == "查看结果")
            {
                var row = GVLog.CurrentRow;
                if (row != null)
                {
                    var id = (int)row.Cells["编号"].Value;
                    var log = BigEntityTableEngine.LocalEngine.Find<APIInvokeLog>(nameof(APIInvokeLog), id);
                    SubForm.TextBoxWin win = new SubForm.TextBoxWin($"查看文本", log?.GetRespDetail().ToString());
                    win.ShowDialog();
                }
            }
            else if (e.ClickedItem.Text == "再次执行")
            {
                var row = GVLog.CurrentRow;
                if (row != null)
                {
                    var id = (int)row.Cells["编号"].Value;
                    var log = BigEntityTableEngine.LocalEngine.Find<APIInvokeLog>(nameof(APIInvokeLog), id);
                    if (log != null && ReInvoke != null)
                    {
                        ReInvoke(log,false);
                    }
                }
            }
            else if(e.ClickedItem.Text== "带参数再次执行")
            {
                var row = GVLog.CurrentRow;
                if (row != null)
                {
                    var id = (int)row.Cells["编号"].Value;
                    var log = BigEntityTableEngine.LocalEngine.Find<APIInvokeLog>(nameof(APIInvokeLog), id);
                    if (log != null && ReInvoke != null)
                    {
                        ReInvoke(log,true);
                    }
                }
            }
            else if (e.ClickedItem.Text == "添加到示例")
            {
                var row = GVLog.CurrentRow;
                if (row != null)
                {
                    var id = (int)row.Cells["编号"].Value;
                    var log = BigEntityTableEngine.LocalEngine.Find<APIInvokeLog>(nameof(APIInvokeLog), id);
                    if (log != null)
                    {
                        var api = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), log.APIId);
                        if (api != null)
                        {
                            var source = BigEntityTableEngine.LocalEngine.Find<APISource>(nameof(APISource), api.SourceId);
                            Util.AddToMainTab(this, $"{source.SourceName}.{api.APIName}示例管理", new UC.UCAPIExampleTabPage(log.APIId,log));
                        }
                    }
                }
            }
        }


        private void BindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (Total == 0)
            {
                PageIndex = 1;
            }
            else
            {
                if (PageIndex > 1)
                {
                    PageIndex -= 1;
                }
            }

            BindData();
        }

        private void BindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            PageIndex = 1;

            BindData();
        }

        private void BindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (Total == 0)
            {
                PageIndex = 1;
            }
            else
            {
                var lastpage = (int)Math.Ceiling(Total * 1.0 / pageSize);
                if (PageIndex < lastpage)
                {
                    PageIndex += 1;
                }
            }

            BindData();
        }

        private int PageIndex
        {
            get;
            set;
        }

        private int Total
        {
            get;
            set;
        }

        private void BindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            if (Total == 0)
            {
                PageIndex = 1;
            }
            else
            {
                var lastpage = (int)Math.Ceiling(Total * 1.0 / pageSize);
                PageIndex = lastpage;
            }

            BindData();

        }

        public void BindData()
        {
            LogViewTab.CheckForIllegalCrossThreadCalls = false;
            loadbox.Waiting(this,() =>
            {
                try
                {
                    long total = 0;
                    object logs = null;
                    if (string.IsNullOrWhiteSpace(TBSearchKey.Text) || TBSearchKey.Text.Equals(TBSearchKey.Tag))
                    {
                        if (_apiid > 0)
                        {
                            logs = BigEntityTableEngine.LocalEngine.Scan<APIInvokeLog>(nameof(APIInvokeLog), "APIId_ApiEnvId_CDate",
                                new object[] { _apiid, _envid, EndDate.Value.Date.AddDays(1) }, new object[] { _apiid, _envid, BeginDate.Value.Date }, PageIndex == 0 ? 1 : PageIndex, pageSize, ref total).Select(p => new
                                {
                                    编号 = p.Id,
                                    时间 = p.CDate,
                                    地址 = p.Path,
                                    请求大小 = p.GetRequestDetail().Length,
                                    响应大小 = p.GetRespDetail().Length,
                                    状态码 = p.StatusCode,
                                    用时 = p.Ms
                                }).ToList();
                        }
                        else
                        {
                            logs = BigEntityTableEngine.LocalEngine.Scan<APIInvokeLog>(nameof(APIInvokeLog), "CDate",
                                new object[] { EndDate.Value.Date.AddDays(1) }, new object[] { BeginDate.Value.Date }, PageIndex == 0 ? 1 : PageIndex, pageSize, ref total).Select(p => new
                                {
                                    编号 = p.Id,
                                    时间 = p.CDate,
                                    地址 = p.Path,
                                    请求大小 = p.GetRequestDetail().Length,
                                    响应大小 = p.GetRespDetail().Length,
                                    状态码 = p.StatusCode,
                                    用时 = p.Ms
                                }).ToList();
                        }
                    }
                    else
                    {
                        var list = BigEntityTableEngine.LocalEngine.Scan<APIInvokeLog>(nameof(APIInvokeLog), "APIId_ApiEnvId_CDate",
                            new object[] { _apiid, _envid, EndDate.Value.Date.AddDays(1) }, new object[] { _apiid, _envid, BeginDate.Value.Date }, 1, int.MaxValue, ref total);
                        var key = TBSearchKey.Text;
                        list = list.Where(p => p.GetRequestBody().ToString().Contains(key) || (p.ResponseText ?? "").Contains(key)).ToList();
                        total = list.Count();
                        list = list.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
                        logs = list.Select(p => new
                        {
                            编号 = p.Id,
                            时间 = p.CDate,
                            地址 = p.Path,
                            请求大小 = p.GetRequestDetail().Length,
                            响应大小 = p.GetRespDetail().Length,
                            状态码 = p.StatusCode,
                            用时 = p.Ms
                        }).ToList();
                    }

                    this.Total = (int)total;

                    this.Invoke(new Action(() =>
                    {
                        this.GVLog.DataSource = logs;
                        var totalpage = (int)Math.Ceiling(total * 1.0 / pageSize);
                        this.bindingNavigatorCountItem.Text = totalpage.ToString();
                        this.bindingNavigatorPositionItem.Text = PageIndex.ToString();
                        this.bindingNavigatorDeleteItem.Enabled = total > 0;

                        this.bindingNavigatorMoveNextItem.Enabled = PageIndex < totalpage;
                        this.bindingNavigatorMoveFirstItem.Enabled = PageIndex > 1;
                        this.bindingNavigatorMoveLastItem.Enabled = PageIndex < totalpage;
                        this.bindingNavigatorMovePreviousItem.Enabled = PageIndex > 1;
                    }));
                }
                catch(Exception ex)
                {
                    Util.SendMsg(this, ex.Message);
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Controls.Remove(loadbox);
                    }));
                }
            });
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void TBSearchKey_MouseEnter(object sender, EventArgs e)
        {
            //if (TBSearchKey.Text.Equals(TBSearchKey.Tag))
            //{
            //    TBSearchKey.Text = string.Empty;
            //    TBSearchKey.ForeColor = Color.Black;
            //}
        }

        private void TBSearchKey_MouseLeave(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(TBSearchKey.Text))
            //{
            //    TBSearchKey.Text = TBSearchKey.Tag.ToString();
            //    TBSearchKey.ForeColor = Color.Gray;
            //}
        }
    }
}
