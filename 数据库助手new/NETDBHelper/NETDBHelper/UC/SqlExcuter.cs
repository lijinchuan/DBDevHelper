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
using Biz.Common;
using System.IO;
using System.Threading;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Biz.Common.Data;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using LJC.FrameWork.LogManager;

namespace NETDBHelper.UC
{
    public partial class SqlExcuter : TabPage, IDataExport,IRecoverAble, IMessageFilter
    {
        UC.LoadingBox loadbox = new LoadingBox();
        bool isexecuting = false;
        private bool preFilterMessageFlag = false;
        private bool runtimeInitialized = false;
        private volatile bool stopRequested = false;

        public SqlExcuter()
        {
            InitializeComponent();
            InitializeRuntime();
        }

        public DBSource Server
        {
            get;
            set;
        }

        private string DB
        {
            get;
            set;
        }

        public string GetDB()
        {
            return DB;
        }

        ContextMenuStrip datastrip = null;

        private ContextMenuStrip CreateDataStrip()
        {
            var strip = new ContextMenuStrip();
            strip.Items.Add("复制内容");
            strip.Items.Add("查看文本");
            strip.Items.Add("复制标题");
            strip.Items.Add("复制标题+内容");
            strip.Items.Add("转换为JSON格式数据");
            strip.Items.Add("统计条数");
            strip.Items.Add("选择这一列");
            strip.Items.Add("锁定这一列");
            strip.Items.Add(new ToolStripSeparator());
            strip.Items.Add("表重命名");
            strip.Items.Add("导出表格数据");
            strip.Items.Add("导出全部表格数据");
            strip.Items.Add(new ToolStripSeparator());
            strip.Items.Add("新增数据");
            strip.Items.Add("复制数据");
            strip.Items.Add("修改数据");
            strip.Items.Add("删除数据");
            strip.ItemClicked += Datastrip_ItemClicked;
            strip.VisibleChanged += Datastrip_VisibleChanged;
            return strip;
        }

        private void InitializeRuntime()
        {
            if (runtimeInitialized)
            {
                return;
            }

            Application.AddMessageFilter(this);
            CanAdjust();
            runtimeInitialized = true;
        }

        private void CanAdjust()
        {
            var mousePos = Point.Empty;
            this.MouseMove += mouseMouse;

            void mouseUp(object s, MouseEventArgs e)
            {
                mousePos = Point.Empty;
                this.MouseUp -= mouseUp;
                preFilterMessageFlag = false;
                this.Cursor = Cursors.Default;
            }

            void mouseMouse(object s, MouseEventArgs e)
            {
                if (e.Y - this.tabControl1.Location.Y < 5 && e.Y - this.tabControl1.Location.Y > 0)
                {
                    this.Cursor = Cursors.SizeNS;
                    preFilterMessageFlag = true;
                    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        if (mousePos == Point.Empty)
                        {
                            mousePos = new Point(e.X, e.Y);
                            this.MouseUp += mouseUp;
                        }
                    }
                }

                if (mousePos != Point.Empty && (e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    if (Math.Abs(e.Y - mousePos.Y) > 10 && Math.Abs(e.Y - mousePos.Y) < 100)
                    {
                        if (sqlEditBox1.Height + e.Y - mousePos.Y >= 50 && sqlEditBox1.Height + e.Y - mousePos.Y <= this.Height - 50
                                   && this.tabControl1.Height + mousePos.Y - e.Y >= 50 && this.tabControl1.Height + mousePos.Y - e.Y <= this.Height - 50)
                        {
                            sqlEditBox1.Height += e.Y - mousePos.Y;
                            this.tabControl1.Height += mousePos.Y - e.Y;
                            var newLoaction = this.tabControl1.Location;
                            newLoaction.Offset(0, mousePos.Y - e.Y);
                            this.tabControl1.Location = newLoaction;
                            mousePos = new Point(e.X, e.Y);
                        }
                    }
                }
                else if (Math.Abs(e.Y - this.tabControl1.Location.Y) > 10 && (e.Button & MouseButtons.Left) == MouseButtons.None)
                {
                    this.Cursor = Cursors.Default;
                    preFilterMessageFlag = false;
                }
            }
        }

        public SqlExcuter(DBSource server, string db, string sql)
        {
            InitializeComponent();

            this.Server = server;
            this.DB = db;
            this.sqlEditBox1.DBName = db;
            this.sqlEditBox1.DBServer = server;
            this.sqlEditBox1.Text = sql;

            this.TBInfo.ScrollBars = ScrollBars.Both;
            this.TBInfo.ContextMenuStrip = contextMenuStrip1;

            this.imageList1.Images.Add(Resources.Resource1.tbview);
            this.imageList1.Images.Add(Resources.Resource1.msg);
            this.tabControl1.TabPages[0].ImageIndex = 1;

            datastrip = new ContextMenuStrip();
            datastrip.Items.Add("复制内容");
            datastrip.Items.Add("查看文本");
            datastrip.Items.Add("复制标题");
            datastrip.Items.Add("复制标题+内容");
            datastrip.Items.Add("转换为JSON格式数据");
            datastrip.Items.Add("统计条数");
            datastrip.Items.Add("选择这一列");
            datastrip.Items.Add("锁定这一列");
            datastrip.Items.Add(new ToolStripSeparator());
            datastrip.Items.Add("表重命名");
            datastrip.Items.Add("导出表格数据");
            datastrip.Items.Add("导出全部表格数据");
            datastrip.Items.Add(new ToolStripSeparator());
            datastrip.Items.Add("新增数据");
            datastrip.Items.Add("复制数据");
            datastrip.Items.Add("修改数据");
            datastrip.Items.Add("删除数据");
            datastrip.ItemClicked += Datastrip_ItemClicked;
            datastrip.VisibleChanged += Datastrip_VisibleChanged;
            InitializeRuntime();
        }

        private void Datastrip_VisibleChanged(object sender, EventArgs e)
        {
            if (datastrip.Visible)
            {
                var items = FindItems(new[] { "新增数据", "复制数据", "修改数据", "删除数据" });
                var visible = false;
                var editble = false;
                var gv = FindDgv();
                if (gv != null)
                {
                    editble = gv.CurrentRow != null;
                    visible = gv.Tag is TableInfo;
                }

                foreach (var item in items)
                {
                    item.Visible = visible;
                    item.Enabled = visible;
                }
                foreach(var item in items.Skip(1))
                {
                    item.Visible = editble;
                }
            }

            IEnumerable<ToolStripItem> FindItems(string[] texts)
            {
                foreach(ToolStripItem item in datastrip.Items)
                {
                    if (texts.Contains(item.Text))
                    {
                        yield return item;
                    }
                }
            }
        }

        private void Datastrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            (sender as ContextMenuStrip).Visible = false;

            if (e.ClickedItem.Text == "导出表格数据")
            {
                Export();
            }
            else if (e.ClickedItem.Text == "导出全部表格数据")
            {
                MExport();
            }
            else if (e.ClickedItem.Text == "复制内容")
            {
                Copy();
            }
            else if (e.ClickedItem.Text == "查看文本")
            {
                ViewText();
            }
            else if (e.ClickedItem.Text == "复制标题")
            {
                CopyTitle();
            }
            else if (e.ClickedItem.Text == "复制标题+内容")
            {
                CopyDataWithTitle();
            }
            else if (e.ClickedItem.Text == "选择这一列")
            {
                SelectCurrColumn();
            }
            else if (e.ClickedItem.Text == "表重命名")
            {
                RenameViewName();
            }
            else if (e.ClickedItem.Text == "转换为JSON格式数据")
            {
                ViewJSONData();
            }
            else if (e.ClickedItem.Text == "统计条数")
            {
                var view = FindDgv();
                if (view != null)
                {
                    this.datastrip.Visible = false;
                    Util.SendMsg(this, "记录数:" + view.RowCount + "条");
                }
            }
            else if (e.ClickedItem.Text == "锁定这一列")
            {
                LockColumn();
            }
            else if (e.ClickedItem.Text == "新增数据")
            {
                var view = FindDgv();
                if (view != null && view.Tag is TableInfo)
                {
                    var tbinfo = view.Tag as TableInfo;

                    var dlg = new SubForm.UpSertDlg(Server, tbinfo);
                    dlg.Show();
                }
            }
            else if (e.ClickedItem.Text == "复制数据")
            {
                var view = FindDgv();
                if (view != null && view.Tag is TableInfo&& view.CurrentRow != null)
                {
                    var tbinfo = view.Tag as TableInfo;

                    var dlg = new SubForm.UpSertDlg(Server, tbinfo, copyRow: view.CurrentRow);
                    dlg.Show();
                }
            }
            else if (e.ClickedItem.Text == "修改数据")
            {
                var view = FindDgv();
                if (view != null && view.Tag is TableInfo && view.CurrentRow != null)
                {
                    var tbinfo = view.Tag as TableInfo;

                    var dlg = new SubForm.UpSertDlg(Server, tbinfo, view.CurrentRow);
                    dlg.Show();
                }
            }
            else if (e.ClickedItem.Text == "删除数据")
            {
                var view = FindDgv();
                if (view != null && view.Tag is TableInfo && view.CurrentRow != null)
                {
                    var currentRow = view.CurrentRow;
                    var tbinfo = view.Tag as TableInfo;

                    var cols = SQLHelper.GetColumns(Server, tbinfo.DBName, tbinfo.TBName, tbinfo.Schema);
                    List<SqlParameter> @params = new List<SqlParameter>();
                    var sb = new StringBuilder($"delete [{tbinfo.DBName}].[{tbinfo.Schema}].[{tbinfo.TBName}] where ");
                    foreach(var col in cols)
                    {
                        if (col.IsKey || col.IsID)
                        {
                            if (!currentRow.DataGridView.Columns.Contains(col.Name))
                            {
                                Util.SendMsg(this, $"删除失败,不包含条件字段:{col.Name}");
                                return;
                            }
                            @params.Add(new SqlParameter
                            {
                                ParameterName = $"@{col.Name}",
                                Value = currentRow.Cells[col.Name].Value
                            });       
                        }
                    }
                    if (!@params.Any())
                    {
                        Util.SendMsg(this, "删除失败,需要主键字段");
                    }
                    else
                    {
                        if (MessageBox.Show("是否要删除?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            Util.SendMsg(this, "取消删除");
                            return;
                        }

                        //删除前备份
                        var sqlquery = $"select * from [{tbinfo.DBName}].[{tbinfo.Schema}].[{tbinfo.TBName}] where {string.Join(" and ", @params.Select(p => p.ParameterName.TrimStart('@') + "=" + p.ParameterName))}";

                        var datatable = SQLHelper.ExecuteDBTable(Server, tbinfo.DBName, sqlquery, @params.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());
                        if (datatable.Rows.Count != 1)
                        {
                            Util.SendMsg(this, "删除失败，不能备份数据");
                            return;
                        }

                        sb.Append($"{string.Join(" and ", @params.Select(p => p.ParameterName.TrimStart('@') + "=" + p.ParameterName))}");
                        Dictionary<string, object> delValue = new Dictionary<string, object>();
                        foreach (DataColumn col in datatable.Columns)
                        {
                            delValue.Add(col.ColumnName, datatable.Rows[0][col]);
                        }
                        LogHelper.Instance.Info("准备删除数据:" + Newtonsoft.Json.JsonConvert.SerializeObject(delValue));

                        if (SQLHelper.ExecuteNoQuery(Server, tbinfo.DBName, sb.ToString(), @params.ToArray()) > 0)
                        {
                            BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = "table",
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.db,
                                DB = tbinfo.DBName,
                                Sever = Server.ServerName,
                                Info = $"删除:" + Newtonsoft.Json.JsonConvert.SerializeObject(delValue),
                                Valid = true
                            });

                            Util.SendMsg(this, "删除成功");
                        }
                    }
                }
            }
        }

        DataGridView FindDgv()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    return view;
                }
            }
            return null;
        }

        private static List<DataGridViewCell> GetSelectedCells(DataGridView view)
        {
            return view.SelectedCells.Cast<DataGridViewCell>().ToList();
        }

        private static IEnumerable<IGrouping<int, DataGridViewCell>> GetSelectedCellGroups(DataGridView view)
        {
            return GetSelectedCells(view).GroupBy(p => p.RowIndex).OrderBy(p => p.Key);
        }

        private static string GetCellText(DataGridViewCell cell)
        {
            if (cell.Value == null)
            {
                return string.Empty;
            }

            if (cell.ValueType == typeof(byte[]))
            {
                return "0x" + BitConverter.ToString((byte[])cell.Value).Replace("-", string.Empty);
            }

            return cell.Value.ToString();
        }

        private static string BuildSelectedCellText(DataGridView view, bool includeHeader, Func<DataGridViewCell, string> formatter = null)
        {
            var rows = GetSelectedCellGroups(view).ToList();
            if (rows.Count == 0)
            {
                return string.Empty;
            }

            formatter = formatter ?? GetCellText;
            StringBuilder sb = new StringBuilder();

            if (includeHeader)
            {
                foreach (DataGridViewCell cell in rows[0].OrderBy(p => p.ColumnIndex))
                {
                    sb.Append(view.Columns[cell.ColumnIndex].Name);
                    sb.Append("\t");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }

            foreach (var row in rows)
            {
                foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                {
                    sb.Append(formatter(cell));
                    sb.Append("\t");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static List<Dictionary<string, object>> BuildSelectedCellJson(DataGridView view)
        {
            var result = new List<Dictionary<string, object>>();

            foreach (var row in GetSelectedCellGroups(view))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                {
                    dic.Add(view.Columns[cell.ColumnIndex].Name, cell.Value);
                }
                result.Add(dic);
            }

            return result;
        }

        private static bool TryBuildKeyParameters(DataGridViewRow row, IEnumerable<TBColumn> cols, Func<TBColumn, bool> predicate, string excludedColumnName, out List<SqlParameter> parameters, out string errorMsg)
        {
            parameters = new List<SqlParameter>();
            errorMsg = string.Empty;
            var matched = false;

            foreach (var col in cols.Where(predicate))
            {
                matched = true;

                if (!row.DataGridView.Columns.Contains(col.Name))
                {
                    errorMsg = $"不包含条件字段:{col.Name}";
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(excludedColumnName) && col.Name.Equals(excludedColumnName, StringComparison.OrdinalIgnoreCase))
                {
                    errorMsg = col.IsID ? "不能更新自增长键" : "不能更新主键";
                    return false;
                }

                parameters.Add(new SqlParameter
                {
                    ParameterName = $"@{col.Name}",
                    Value = row.Cells[col.Name].Value
                });
            }

            return !matched || parameters.Count > 0;
        }

        private static string BuildWhereClause(IEnumerable<SqlParameter> parameters)
        {
            return string.Join(" and ", parameters.Select(p => p.ParameterName.TrimStart('@') + "=" + p.ParameterName));
        }

        private static string BuildSelectedHeaderText(DataGridView view)
        {
            var rows = GetSelectedCellGroups(view).ToList();
            if (rows.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataGridViewCell cell in rows[0].OrderBy(p => p.ColumnIndex))
            {
                sb.Append(view.Columns[cell.ColumnIndex].Name);
                sb.Append("\t");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static string GetTempExportDir()
        {
            var dir = Path.Combine(Application.StartupPath, "temp");
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static string PromptExportFileName(string dir, string extension)
        {
            SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("导出文件名");
            if (dlg.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dlg.InputString))
            {
                return null;
            }

            return Path.Combine(dir, dlg.InputString + extension);
        }

        private static void WriteExcelCell(ICell cell, DataGridViewCell gridCell)
        {
            if (gridCell.Value == null || gridCell.Value == DBNull.Value)
            {
                cell.SetCellValue(string.Empty);
                return;
            }

            if (gridCell.ValueType == typeof(string))
            {
                cell.SetCellValue((string)gridCell.Value);
            }
            else if (gridCell.ValueType == typeof(bool))
            {
                cell.SetCellValue((bool)gridCell.Value);
            }
            else if (gridCell.ValueType == typeof(DateTime))
            {
                cell.SetCellValue(((DateTime)gridCell.Value).ToString("yyyy/MM/dd HH:mm:ss"));
            }
            else if (gridCell.ValueType == typeof(double) || gridCell.ValueType == typeof(Single))
            {
                cell.SetCellValue(Convert.ToDouble(gridCell.Value));
            }
            else
            {
                cell.SetCellValue(gridCell.Value.ToString());
            }
        }

        private static void WriteGridToSheet(IWorkbook book, TabPage page, DataGridView gv)
        {
            var sheet = book.CreateSheet(page.Text);
            var rowIndex = 0;
            var headerRow = sheet.CreateRow(rowIndex++);
            var colcount = 0;

            foreach (DataGridViewColumn col in gv.Columns)
            {
                var cell = headerRow.CreateCell(colcount, CellType.String);
                cell.SetCellValue(col.Name);
                cell.CellStyle = CreateHeaderStyle(book);
                AjustColWidth(sheet, 0, colcount);
                colcount++;
            }

            foreach (DataGridViewRow gvrow in gv.Rows)
            {
                var row = sheet.CreateRow(rowIndex++);
                for (int i = 0; i < colcount; i++)
                {
                    var cell = row.CreateCell(i);
                    WriteExcelCell(cell, gvrow.Cells[i]);
                }
            }
        }

        private void Stop(object o)
        {
            try
            {
                var th = (Thread)o;
                if (th.ThreadState != ThreadState.Stopped)
                {
                    th.Abort();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Execute()
        {
            if (isexecuting)
            {
                return;
            }
            isexecuting = true;
            stopRequested = false;
            var seltext = this.sqlEditBox1.SelectedText;
            if (string.IsNullOrWhiteSpace(seltext))
            {
                seltext = this.sqlEditBox1.Text;
            }
            Thread exthread = new Thread(new ThreadStart(() =>
            {

                try
                {
                    List<TabPage> listrm = new List<TabPage>();
                    foreach (TabPage tp in tabControl1.TabPages)
                    {
                        if (tp == TPInfo
                            || tp.Controls.Contains(sqlEditBox1))
                        {
                            continue;
                        }
                        listrm.Add(tp);
                    }
                    foreach (var tp in listrm)
                    {
                        this.Invoke(new Action(() => tabControl1.TabPages.Remove(tp)));
                    }

                    this.Invoke(new Action(() =>
                    {
                        loadbox.Location = new Point((this.Width - loadbox.Width) / 2, (this.Height - loadbox.Height) / 2);
                        this.Controls.Add(loadbox);
                        loadbox.BringToFront();
                    }));

                    DateTime now = DateTime.Now;
                    var ts = SQLHelper.ExecuteDataSet(Server, DB, seltext, (s, e) =>
                     {
                         if (stopRequested)
                         {
                             return;
                         }
                         this.Invoke(new Action(() =>
                         {
                             TBInfo.Text += $"{e.Message}\r\n\r\n";
                             if (e.Errors != null && e.Errors.Count > 0)
                             {
                                 for (int i = 0; i < e.Errors.Count; i++)
                                 {
                                     TBInfo.Text += $"{e.Errors[i].Message}\r\n\r\n";
                                 }
                             }
                         }));
                     });

                    this.Invoke(new Action(() =>
                    {
                        if (stopRequested)
                        {
                            return;
                        }
                        TBInfo.Text += $"用时:{DateTime.Now.Subtract(now).TotalMilliseconds}ms\r\n";
                    }));
                    if (stopRequested)
                    {
                        return;
                    }
                    if (ts != null && ts.Tables.Count > 0)
                    {
                        int pos = 0;
                        if (this.tabControl1.TabPages.Count > 1)
                        {
                            pos = 1;
                        }
                        for (int i = 0; i < ts.Tables.Count; i++)
                        {
                            var tb = ts.Tables[i];
                            TabPage page = new TabPage(tb.TableName ?? "未命名表");
                            page.ImageIndex = 0;

                            var dgv = new DataGridView();
                            Util.DoubleBuffered(dgv, true);
                            dgv.Cursor = Cursors.Default;
                            dgv.CellFormatting += (s, e) =>
                            {
                                var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                                if (cell != dgv.CurrentCell || !cell.IsInEditMode)
                                {
                                    if (e.Value == DBNull.Value)
                                    {
                                        //var font = e.CellStyle.Font;
                                        //e.CellStyle.Font = new Font(font.FontFamily, font.Size, FontStyle.Italic);

                                        e.Value = Common.NullDisplayValue;
                                        e.FormattingApplied = true;
                                    }
                                }
                            };
                            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                            dgv.AllowUserToResizeRows = true;
                            page.Controls.Add(dgv);

                            bool canEdit = false;
                            string targetTable = null;
                            if (ts.Tables.Count == 1)
                            {
                                var regex = new Regex(@"^[\r\n\s\t]*select[\r\n\s\t]+(?:top[\r\n\s\t]+\d{1,}[\r\n\s\t]+)?(?:\*[\r\n\s\t]*|(?:\[?\w+\]?\.){0,}\[\w+\][\,\r\n\s\t]+)+from[\r\n\s\t]+(?:\[?\w+\]?\.){0,}\[?(\w+)\]?", RegexOptions.IgnoreCase);
                                var m = regex.Match(seltext);
                                if (m.Success)
                                {
                                    targetTable = m.Groups[1].Value;
                                    tb.TableName = targetTable;

                                    var tbinfo = SQLHelper.GetTB(Server, DB, tb.TableName);
                                    if (tbinfo != null)
                                    {
                                        dgv.Tag = tbinfo;
                                        canEdit = true;
                                    }
                                }
                            }

                            if (canEdit) {
                                dgv.CellDoubleClick += (s, e) =>
                                {
                                    if (dgv.CurrentCell.Value != null && (dgv.CurrentCell.ValueType == typeof(string)
                                    || dgv.CurrentCell.ValueType == typeof(bool)
                                    || dgv.CurrentCell.ValueType == typeof(int)
                                    || dgv.CurrentCell.ValueType == typeof(DateTime)
                                    || dgv.CurrentCell.ValueType == typeof(Int64)))
                                    {
                                        var cell = dgv.CurrentCell;

                                        if (cell.Style.WrapMode == DataGridViewTriState.True)
                                        {
                                            cell.Style.WrapMode = DataGridViewTriState.False;
                                            dgv.EndEdit();
                                        }
                                        else
                                        {
                                            cell.Style.WrapMode = DataGridViewTriState.True;
                                            cell.ReadOnly = false;
                                            dgv.ReadOnly = false;
                                            dgv.BeginEdit(true);

                                            if (cell.Value.Equals(Common.NullDisplayValue))
                                            {
                                                cell.Value = DBNull.Value;
                                            }
                                        }
                                    }
                                };
                                dgv.CellBeginEdit += Dgv_CellBeginEdit;
                                dgv.CellEndEdit += Dgv_CellEndEdit;
                                dgv.CellValueChanged += Dgv_CellValueChanged;
                            }
                            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                            dgv.DataError += (s, e) =>
                            {
                                e.Cancel = true;
                            };

                            dgv.GridColor = Color.LightBlue;
                            dgv.Dock = DockStyle.Fill;
                            dgv.BackgroundColor = Color.White;
                            dgv.AllowUserToAddRows = false;
                            dgv.ReadOnly = true;
                            dgv.DataSource = tb;
                            dgv.ContextMenuStrip = this.datastrip;
                            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Red;
                            dgv.RowHeadersWidth = 30 + (tb.Rows.Count.ToString().Length + 1) * 8;
                            dgv.RowStateChanged += (s, e) =>
                            {
                                e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);

                            };
                            dgv.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

                            this.Invoke(new Action(() =>
                            {
                                tabControl1.TabPages.Insert(i + pos, page);
                                if (i == 0)
                                {
                                    tabControl1.SelectedTab = page;
                                }
                            }));
                        }
                    }
                    BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
                    {
                        TypeName = "sql",
                        LogTime = DateTime.Now,
                        LogType = LogTypeEnum.sql,
                        DB = this.DB,
                        Sever = Server.ServerName,
                        Info = seltext,
                        Valid = true
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        TBInfo.Text += ex.Message + "\r\n";
                        tabControl1.SelectedTab = TPInfo;
                    }));
                }
                finally
                {
                    isexecuting = false;
                    this.loadbox.OnStop -= Stop;
                    if (this.Controls.Contains(loadbox))
                    {
                        this.Invoke(new Action(() => this.Controls.Remove(loadbox)));
                    }
                }

            }));
            this.loadbox.tag = exthread;
            this.loadbox.OnStop += Stop;
            exthread.Start();
        }

        private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var dgv = sender as DataGridView;
            dgv.Tag = new Tuple<DataGridViewCellCancelEventArgs, object, object>(e, dgv[e.ColumnIndex, e.RowIndex].Value, dgv.Tag is TableInfo ? dgv.Tag : null);
        }

        private void Dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (sender is DataGridView)
            {
                var gv = sender as DataGridView;
                var tb = gv.DataSource as DataTable;

                string errorMsg = string.Empty;

                var updateColName = gv.Columns[e.ColumnIndex].Name;
                var newVal = gv[e.ColumnIndex, e.RowIndex].Value;
                object oldVal = null;
                object oldTag = null;
                if (gv.Tag == null)
                {
                    errorMsg = "没有记录到旧值";
                }
                else
                {
                    var tagVal = (Tuple<DataGridViewCellCancelEventArgs, object,object>)gv.Tag;
                    oldTag = tagVal.Item3;
                    if (tagVal.Item1.RowIndex != e.RowIndex || tagVal.Item1.ColumnIndex != e.ColumnIndex)
                    {
                        errorMsg = "没有记录到旧的值";
                    }
                    else
                    {
                        oldVal = tagVal.Item2;
                        if (object.Equals(oldVal, newVal))
                        {
                            errorMsg = "值没有修改";
                        }
                    }
                }

                gv.Tag = oldTag;

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Util.SendMsg(this, errorMsg);
                    return;
                }

                if (MessageBox.Show("要修改值吗?", "修改确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    //gv.CancelEdit();
                    gv[e.ColumnIndex, e.RowIndex].Value = oldVal;
                    return;
                }

                var tbinfo = SQLHelper.GetTB(Server, DB, tb.TableName);
                if (tbinfo != null)
                {
                    var cols = SQLHelper.GetColumns(Server, DB, tb.TableName, tbinfo.Schema);

                    var orgtablecolumns = SQLHelper.GetDateTableColumns(Server, cols, tbinfo);

                    var checkPass = true;

                    foreach (DataColumn col in tb.Columns)
                    {
                        var findCol = false;
                        foreach (DataColumn orgcol in orgtablecolumns)
                        {
                            if (orgcol.ColumnName.ToLower() == col.ColumnName.ToLower())
                            {
                                if (orgcol.DataType != col.DataType)
                                {
                                    checkPass = false;
                                    errorMsg = $"字段{col.ColumnName}与目标表{tb.TableName}类型不同";
                                }
                                else
                                {
                                    findCol = true;
                                }
                                break;
                            }
                        }
                        if (!checkPass)
                        {
                            break;
                        }
                        if (!findCol)
                        {
                            errorMsg = $"字段{col.ColumnName}在目标表{tb.TableName}不存在";
                            checkPass = false;
                            break;
                        }
                    }
                    if (checkPass)
                    {
                        List<SqlParameter> parameList;
                        if (!TryBuildKeyParameters(gv.Rows[e.RowIndex], cols, p => p.IsID, updateColName, out parameList, out errorMsg))
                        {
                            if (string.IsNullOrEmpty(errorMsg) && !TryBuildKeyParameters(gv.Rows[e.RowIndex], cols, p => p.IsKey || p.IsID, updateColName, out parameList, out errorMsg))
                            {
                                if (string.IsNullOrEmpty(errorMsg))
                                {
                                    errorMsg = $"没有主键值，无法更新到表{tb.TableName}";
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(errorMsg) && parameList.Count > 0)
                        {
                            parameList.Add(new SqlParameter
                            {
                                ParameterName = $"@{updateColName}",
                                Value = newVal
                            });

                            var whereClause = BuildWhereClause(parameList.Take(parameList.Count - 1));
                            var sql = $"update [{tb.TableName}] set [{updateColName}]=@{updateColName} where {whereClause}";
                            var ret = SQLHelper.ExecuteNoQuery(Server, DB, sql, parameList.ToArray());

                            Util.SendMsg(this, $"更新{(ret > 0 ? "成功" : "失败")}");
                            BigEntityTableRemotingEngine.Insert("HLog", new HLogEntity
                            {
                                TypeName = tb.TableName,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.sql,
                                DB = DB,
                                Sever = Server.ServerName,
                                Info = $"更新字段:{updateColName},主键:{(string.Join(";", parameList.Take(parameList.Count - 1).Select(p => p.ParameterName.TrimStart('@') + ":" + p.Value)))},更新为:{newVal}，前值为:{oldVal}，结果:{(ret > 0 ? "成功" : "失败")}",
                                Valid = true
                            });
                        }
                    }

                }
                else
                {
                    errorMsg = $"{tb.TableName}表不存在";
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    gv[e.ColumnIndex, e.RowIndex].Value = oldVal;
                    Util.SendMsg(this, errorMsg);
                }
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TBInfo.Text = string.Empty;
        }

        private void TabControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.tabControl1.Dock == DockStyle.Fill)
            {
                this.tabControl1.Dock = DockStyle.Bottom;
                if (this.tabControl1.TabPages[0].Controls.Contains(sqlEditBox1))
                {
                    this.tabControl1.TabPages[0].Controls.Remove(sqlEditBox1);
                    this.tabControl1.TabPages.RemoveAt(0);
                    this.Controls.Add(sqlEditBox1);
                    sqlEditBox1.Dock = DockStyle.None;
                    sqlEditBox1.Width = this.tabControl1.Width;
                    sqlEditBox1.Height = tabControl1.Location.Y - sqlEditBox1.Location.Y - 5;
                    sqlEditBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                }
            }
            else
            {
                this.tabControl1.Dock = DockStyle.Fill;
                var tabpage = new TabPage();
                tabpage.Text = "sql";
                this.Controls.Remove(this.sqlEditBox1);
                tabpage.Controls.Add(sqlEditBox1);
                this.sqlEditBox1.Dock = DockStyle.Fill;
                this.tabControl1.TabPages.Insert(0, tabpage);
            }
        }

        public void Export()
        {
            var gv = FindDgv();
            if (gv == null)
            {
                return;
            }

            var dir = GetTempExportDir();
            if (gv.DataSource != null && gv.DataSource is DataTable)
            {
                var filename = PromptExportFileName(dir, ".csv");
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    using (StreamWriter fs = new StreamWriter(filename, false, Encoding.UTF8))
                    {
                        var table = (DataTable)gv.DataSource;
                        if (table != null)
                        {
                            var str = table.ToCsv();
                            if (!string.IsNullOrWhiteSpace(str))
                            {
                                fs.Write(str);
                            }
                        }
                    }
                    Util.SendMsg(this, $"文件已保存:{filename}");
                    System.Diagnostics.Process.Start("explorer.exe", dir);
                }
            }
        }

        public void RenameViewName()
        {
            var gv = FindDgv();
            if (gv == null)
            {
                return;
            }

            if (gv.DataSource != null && gv.DataSource is DataTable)
            {
                SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("重命名");
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    tabControl1.SelectedTab.Text = dlg.InputString;
                }
            }
        }

        public static ICellStyle CreateHeaderStyle(IWorkbook book)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont fontLeft = book.CreateFont();
            fontLeft.FontHeightInPoints = 15;
            fontLeft.Boldweight = (short)FontBoldWeight.Bold;
            fontLeft.FontName = "宋体";
            cellStyle.ShrinkToFit = false;
            cellStyle.SetFont(fontLeft);
            return cellStyle;
        }

        public static ICellStyle CreateCellStyle(IWorkbook book)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont fontLeft = book.CreateFont();
            fontLeft.FontHeightInPoints = 15;
            fontLeft.FontName = "宋体";
            cellStyle.ShrinkToFit = false;
            cellStyle.SetFont(fontLeft);
            return cellStyle;
        }

        public static void AjustColWidth(ISheet ffSheet, int row, int col)
        {

            int columnWidth = ffSheet.GetColumnWidth(col) / 256;//获取当前列宽度

            IRow currentRow = ffSheet.GetRow(row);
            ICell currentCell = currentRow.GetCell(col);
            int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度
            if (columnWidth < length + 1)
            {
                columnWidth = length + 1;
            }//若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符

            ffSheet.SetColumnWidth(col, columnWidth * 400);

        }

        public void MExport()
        {
            var dir = GetTempExportDir();
            var filename = PromptExportFileName(dir, ".xls");
            if (!string.IsNullOrWhiteSpace(filename))
            {
                HSSFWorkbook book = new HSSFWorkbook();

                foreach (TabPage page in tabControl1.TabPages)
                {
                    foreach (var ctl in page.Controls)
                    {
                        if (ctl is DataGridView)
                        {
                            var gv = (DataGridView)ctl;
                            if (gv.DataSource != null && gv.DataSource is DataTable)
                            {
                                WriteGridToSheet(book, page, gv);
                                break;
                            }
                        }
                    }
                }

                using (var fs = new System.IO.FileStream(filename, FileMode.Create))
                {
                    book.Write(fs);

                    Util.SendMsg(this, $"文件已保存:{filename}");
                    System.Diagnostics.Process.Start("explorer.exe", dir);
                }
            }
        }

        public void Copy()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    if (view.SelectedCells.Count == 0)
                    {
                        return;
                    }
                    StringBuilder sb = new StringBuilder();

                    foreach (var row in GetSelectedCellGroups(view))
                    {
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            sb.Append(GetCellText(cell));
                            sb.Append("\t");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();
                    }
                    Clipboard.SetText(sb.ToString().Trim('\t'));
                    Util.SendMsg(this, $"内容已复制到剪贴板");
                    break;
                }
            }
        }

        public void ViewText()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    if (view.SelectedCells.Count == 0)
                    {
                        return;
                    }
                    StringBuilder sb = new StringBuilder();

                    foreach (var row in GetSelectedCellGroups(view))
                    {
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            sb.Append(GetCellText(cell));
                            sb.Append("\t");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();
                    }
                    new SubForm.TextBoxWin("查看文本", sb.ToString().Trim('\t').Replace("\n", Environment.NewLine)).ShowDialog();
                    break;
                }
            }
        }

        public void SelectCurrColumn()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    var currentcell = view.CurrentCell;
                    if (currentcell == null)
                    {
                        return;
                    }
                    foreach (DataGridViewRow row in view.Rows)
                    {
                        row.Cells[currentcell.ColumnIndex].Selected = true;
                    }
                    break;
                }
            }
        }

        public void CopyTitle()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    if (view.SelectedCells.Count == 0)
                    {
                        return;
                    }
                    StringBuilder sb = new StringBuilder();

                    foreach (DataGridViewCell cell in GetSelectedCellGroups(view).First().OrderBy(p => p.ColumnIndex))
                    {
                        sb.Append(view.Columns[cell.ColumnIndex].Name.ToString());
                        sb.Append("\t");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    Clipboard.SetText(sb.ToString());
                    Util.SendMsg(this, $"标题已复制到剪贴板");
                    break;
                }
            }
        }

        public void ViewJSONData()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    if (view.SelectedCells.Count == 0)
                    {
                        return;
                    }

                    List<Dictionary<string, object>> datalist = new List<Dictionary<string, object>>();
                    List<DataGridViewCell> list = new List<DataGridViewCell>();
                    foreach (DataGridViewCell cell in view.SelectedCells)
                    {
                        list.Add(cell);
                    }

                    foreach (var row in list.GroupBy(p => p.RowIndex).OrderBy(p => p.Key))
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            dic.Add(view.Columns[cell.ColumnIndex].Name, cell.Value);
                        }
                        datalist.Add(dic);
                    }

                    //Clipboard.SetText(sb.ToString());
                    new SubForm.TextBoxWin("查看JSON文本", Newtonsoft.Json.JsonConvert.SerializeObject(datalist, Newtonsoft.Json.Formatting.Indented)).ShowDialog();
                    //Util.SendMsg(this, $"内容和标题已复制到剪贴板");
                    break;
                }
            }
        }

        public void CopyDataWithTitle()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    if (view.SelectedCells.Count == 0)
                    {
                        return;
                    }
                    StringBuilder sb = new StringBuilder();

                    foreach (DataGridViewCell cell in GetSelectedCellGroups(view).First().OrderBy(p => p.ColumnIndex))
                    {
                        sb.Append(view.Columns[cell.ColumnIndex].Name.ToString());
                        sb.Append("\t");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                    foreach (var row in GetSelectedCellGroups(view))
                    {
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            sb.Append(GetCellText(cell));
                            sb.Append("\t");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();
                    }

                    Clipboard.SetText(sb.ToString());
                    Util.SendMsg(this, $"内容和标题已复制到剪贴板");
                    break;
                }
            }
        }

        public void LockColumn()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var view = (DataGridView)ctl;
                    foreach (DataGridViewColumn col in view.Columns)
                    {
                        if (col.Frozen)
                        {
                            col.Frozen = false;
                        }
                    }
                    if (view.CurrentCell != null)
                    {
                        view.Columns[view.CurrentCell.ColumnIndex].Frozen = true;
                    }
                    break;
                }
            }
        }

        public object[] GetRecoverData()
        {
            return new object[] { this.Text, this.Server, this.DB, this.sqlEditBox1.Text };
        }

        public IRecoverAble Recover(object[] recoverData)
        {
            this.Text = (string)recoverData[0];
            this.Server =(DBSource)recoverData[1] ;
            this.DB = (string)recoverData[2];
            this.sqlEditBox1.DBName = (string)recoverData[2];
            this.sqlEditBox1.DBServer = this.Server;
            this.sqlEditBox1.Text = (string)recoverData[3];

            this.TBInfo.ScrollBars = ScrollBars.Both;
            this.TBInfo.ContextMenuStrip = contextMenuStrip1;

            this.imageList1.Images.Add(Resources.Resource1.tbview);
            this.imageList1.Images.Add(Resources.Resource1.msg);
            this.tabControl1.TabPages[0].ImageIndex = 1;

            datastrip = new ContextMenuStrip();
            datastrip.Items.Add("复制内容");
            datastrip.Items.Add("查看文本");
            datastrip.Items.Add("复制标题");
            datastrip.Items.Add("复制标题+内容");
            datastrip.Items.Add("转换为JSON格式数据");
            datastrip.Items.Add("统计条数");
            datastrip.Items.Add("选择这一列");
            datastrip.Items.Add("锁定这一列");
            datastrip.Items.Add(new ToolStripSeparator());
            datastrip.Items.Add("表重命名");
            datastrip.Items.Add("导出表格数据");
            datastrip.Items.Add("导出全部表格数据");
            datastrip.Items.Add(new ToolStripSeparator());
            datastrip.Items.Add("新增数据");
            datastrip.Items.Add("复制数据");
            datastrip.Items.Add("修改数据");
            datastrip.Items.Add("删除数据");
            datastrip.VisibleChanged += Datastrip_VisibleChanged;
            datastrip.ItemClicked += Datastrip_ItemClicked;

            Application.AddMessageFilter(this);
            CanAdjust();

            return this;
        }


        static int MakeLParam(int x, int y)
        {
            return (y << 16) | (x & 0xFFFF);
        }
        /// <summary>
        /// Y
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        static int GetHiWord(int lp)
        {
            return lp >> 16;
        }
        /// <summary>
        /// X
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        static int GetLoWord(int lp)
        {
            return lp & 0xFFFF;
        }


        public bool PreFilterMessage(ref Message m)
        {
            if (preFilterMessageFlag && m.Msg == 0x200)
            {
                //mousemove
                var x = GetLoWord(m.LParam.ToInt32());
                var y = GetHiWord(m.LParam.ToInt32());

                this.OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
            }
            return false;
        }
    }
}
