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

namespace NETDBHelper.UC
{
    public partial class LogViewTab : TabPage
    {
        int pageSize = 20;
        UC.LoadingBox loadbox = new LoadingBox();
        public LogViewTab()
        {
            InitializeComponent();
            this.GVLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            PageIndex = 1;
            this.bindingNavigatorMoveLastItem.Click += BindingNavigatorMoveLastItem_Click;
            this.bindingNavigatorMoveNextItem.Click += BindingNavigatorMoveNextItem_Click;
            this.bindingNavigatorMoveFirstItem.Click += BindingNavigatorMoveFirstItem_Click;
            this.bindingNavigatorMovePreviousItem.Click += BindingNavigatorMovePreviousItem_Click;

            this.bindingNavigatorDeleteItem.Click += BindingNavigatorDeleteItem_Click;

            this.GVLog.ContextMenuStrip = new ContextMenuStrip();
            this.GVLog.ContextMenuStrip.Items.Add("复制");
            this.GVLog.ContextMenuStrip.Items.Add("备注");
            this.GVLog.ContextMenuStrip.Items.Add("查看文本");
            this.GVLog.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
            this.GVLog.CellDoubleClick += GVLog_CellDoubleClick;
            this.GVLog.BorderStyle = BorderStyle.None;
            this.GVLog.GridColor = Color.LightBlue;

            this.GVLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.GVLog.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.GVLog.AllowUserToResizeRows = true;

            BeginDate.Value = DateTime.Now.AddMonths(-1);
            EndDate.Value = DateTime.Now.AddDays(1);
            loadbox.OnStop += Loadbox_OnStop;
            TBSearchKey.GotFocus += TBSearchKey_GotFocus;
            TBSearchKey.LostFocus += TBSearchKey_LostFocus;
            TBSearchKey.TextChanged += TBSearchKey_TextChanged;
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

        private void Loadbox_OnStop(object obj)
        {
            try
            {
                if (obj != null)
                {
                    ((Thread)obj).Abort();
                }
            }
            catch
            {

            }
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
                    BigEntityTableRemotingEngine.Delete<Entity.HLogEntity>("HLog", (int)log.Cells["编号"].Value);
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

                    dlg.DlgResult += () =>
                      {
                          var logid = (int)rows[0].Cells["编号"].Value;
                          var log = BigEntityTableRemotingEngine.Find<Entity.HLogEntity>("HLog", logid);
                          if (log != null)
                          {
                              log.TypeName = dlg.InputString;
                              BigEntityTableRemotingEngine.Update("HLog", log);
                              BindData();
                          }
                      };
                    dlg.ShowMe(this);
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
                    SubForm.TextBoxWin win = new SubForm.TextBoxWin("", string.Join("\t", list));
                    win.ShowDialog();
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
            Thread thd=new Thread(new ThreadStart(() =>
            {
                try
                {
                    long total = 0;
                    object logs = null;
                    if (string.IsNullOrWhiteSpace(TBSearchKey.Text) || TBSearchKey.Text.Equals(TBSearchKey.Tag))
                    {
                        logs = BigEntityTableRemotingEngine.ScanDesc<HLogEntity>("HLog", "LogTime",
                            new object[] { BeginDate.Value }, new object[] { EndDate.Value }, PageIndex == 0 ? 1 : PageIndex, pageSize, ref total).Select(p => new
                            {
                                编号 = p.ID,
                                日志时间 = p.LogTime,
                                服务器 = p.Sever,
                                库 = p.DB,
                                对象类型 = p.LogType,
                                对象名称 = p.TypeName,
                                信息 = p.Info
                            }).ToList();
                    }
                    else
                    {
                        var list = BigEntityTableRemotingEngine.Scan<HLogEntity>("HLog", "LogTime",
                            new object[] { BeginDate.Value }, new object[] { EndDate.Value }, 1, int.MaxValue, ref total);
                        var key = TBSearchKey.Text;
                        list = list.Where(p => (p.Info ?? "").Contains(key) || (p.DB ?? "").Contains(key) || (p.TypeName ?? "").Contains(key)).ToList();
                        total = list.Count();
                        list.Reverse();
                        list = list.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
                        logs = list.Select(p => new
                        {
                            编号 = p.ID,
                            日志时间 = p.LogTime,
                            服务器 = p.Sever,
                            库 = p.DB,
                            对象类型 = p.LogType,
                            对象名称 = p.TypeName,
                            信息 = p.Info
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
                catch
                {

                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Controls.Remove(loadbox);
                    }));
                }
            }));

            loadbox.tag = thd;
            loadbox.Location = new Point((this.Width - loadbox.Width) / 2, (this.Height - loadbox.Height) / 2);
            this.Controls.Add(loadbox);
            loadbox.BringToFront();
            
            thd.Start();
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
