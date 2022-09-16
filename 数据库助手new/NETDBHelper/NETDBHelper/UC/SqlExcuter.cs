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

namespace NETDBHelper.UC
{
    public partial class SqlExcuter : TabPage, IDataExport,IRecoverAble
    {
        UC.LoadingBox loadbox = new LoadingBox();
        bool isexecuting = false;

        public SqlExcuter()
        {
            InitializeComponent();
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

        private void CanAdjust()
        {
            var mousePos = Point.Empty;
            this.MouseMove += mouseMouse;

            void mouseUp(object s, MouseEventArgs e)
            {
                mousePos = Point.Empty;
                this.MouseUp -= mouseUp;
                this.Cursor = Cursors.Default;
            }

            void mouseMouse(object s, MouseEventArgs e)
            {
                if (e.Y - this.tabControl1.Location.Y < 5 && e.Y - this.tabControl1.Location.Y > 0)
                {
                    this.Cursor = Cursors.SizeNS;
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
            datastrip.ItemClicked += Datastrip_ItemClicked;

            CanAdjust();
        }

        private void Datastrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
                foreach (var ctl in tabControl1.SelectedTab.Controls)
                {
                    if (ctl is DataGridView)
                    {
                        var view = (DataGridView)ctl;
                        this.datastrip.Visible = false;
                        Util.SendMsg(this, "记录数:" + view.RowCount + "条");
                        //MessageBox.Show("记录数:" + view.RowCount + "条");
                    }
                }
            }
            else if (e.ClickedItem.Text == "锁定这一列")
            {
                LockColumn();
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
                        TBInfo.Text += $"用时:{DateTime.Now.Subtract(now).TotalMilliseconds}ms\r\n";
                    }));
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
                                    canEdit = true;
                                    targetTable = m.Groups[1].Value;
                                    tb.TableName = targetTable;
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
            dgv.Tag = new KeyValuePair<DataGridViewCellCancelEventArgs, object>(e, dgv[e.ColumnIndex, e.RowIndex].Value);
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
                if (gv.Tag == null)
                {
                    errorMsg = "没有记录到旧值";
                }
                else
                {
                    var tagVal = (KeyValuePair<DataGridViewCellCancelEventArgs, object>)gv.Tag;
                    if (tagVal.Key.RowIndex != e.RowIndex || tagVal.Key.ColumnIndex != e.ColumnIndex)
                    {
                        errorMsg = "没有记录到旧的值";
                    }
                    else
                    {
                        oldVal = tagVal.Value;
                        if (object.Equals(oldVal, newVal))
                        {
                            errorMsg = "值没有修改";
                        }
                    }
                }

                gv.Tag = null;

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
                        var parameList = new List<SqlParameter>();
                        StringBuilder sbsql = new StringBuilder();

                        sbsql.Append($"update [{tb.TableName}] set [{updateColName}]=@{updateColName} where ");

                        foreach (var keycol in cols.Where(p => p.IsID))
                        {
                            if (!gv.Columns.Contains(keycol.Name))
                            {
                                break;
                            }

                            if (keycol.Name.Equals(updateColName, StringComparison.OrdinalIgnoreCase))
                            {
                                errorMsg = "不能更新自增长键";
                                checkPass = false;
                                break;
                            }

                            parameList.Add(new SqlParameter
                            {
                                ParameterName = $"@{keycol.Name}",
                                Value = gv[keycol.Name, e.RowIndex].Value
                            });

                            sbsql.Append($"[{keycol.Name}]=@{keycol.Name}");
                        }

                        if (parameList.Count == 0)
                        {
                            foreach (var keycol in cols.Where(p => p.IsKey || p.IsID))
                            {
                                if (!gv.Columns.Contains(keycol.Name))
                                {
                                    checkPass = false;
                                    break;
                                }

                                if (keycol.Name.Equals(updateColName, StringComparison.OrdinalIgnoreCase))
                                {
                                    errorMsg = "不能更新主键";
                                    checkPass = false;
                                    break;
                                }

                                parameList.Add(new SqlParameter
                                {
                                    ParameterName = $"@{keycol.Name}",
                                    Value = gv[keycol.Name, e.RowIndex].Value
                                });

                                sbsql.Append($"[{keycol.Name}]=@{keycol.Name}");
                            }
                        }

                        if (checkPass)
                        {
                            if (parameList.Count == 0)
                            {
                                errorMsg = $"没有主键值，无法更新到表{tb.TableName}";
                                checkPass = false;
                            }
                            else
                            {
                                parameList.Add(new SqlParameter
                                {
                                    ParameterName = $"@{updateColName}",
                                    Value = newVal
                                });

                                var ret = SQLHelper.ExecuteNoQuery(Server, DB, sbsql.ToString(), parameList.ToArray());

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
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var gv = (DataGridView)ctl;
                    var dir = Application.StartupPath + "\\temp\\";
                    if (gv.DataSource != null && gv.DataSource is DataTable)
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("导出文件名");
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            var filename = $"{dir}\\{dlg.InputString}.csv";
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
                        break;

                    }
                }
            }
        }

        public void RenameViewName()
        {
            foreach (var ctl in tabControl1.SelectedTab.Controls)
            {
                if (ctl is DataGridView)
                {
                    var gv = (DataGridView)ctl;
                    if (gv.DataSource != null && gv.DataSource is DataTable)
                    {
                        SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("重命名");
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            tabControl1.SelectedTab.Text = dlg.InputString;
                        }
                        break;

                    }
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
            var dir = Application.StartupPath + "\\temp\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("导出文件名");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var filename = $"{dir}\\{dlg.InputString}.xls";
                HSSFWorkbook book = new HSSFWorkbook();

                foreach (TabPage page in tabControl1.TabPages)
                {
                    foreach (var ctl in page.Controls)
                    {
                        if (ctl is DataGridView)
                        {
                            var gv = (DataGridView)ctl;
                            var rowidx = 0;
                            if (gv.DataSource != null && gv.DataSource is DataTable)
                            {
                                var sheet = book.CreateSheet(page.Text);
                                //book.CreateSheet()
                                var row = sheet.CreateRow(rowidx++);
                                var colcount = 0;
                                foreach (DataGridViewColumn col in gv.Columns)
                                {
                                    var cell = row.CreateCell(colcount, CellType.String);
                                    cell.SetCellValue(col.Name);
                                    cell.CellStyle = CreateHeaderStyle(book);
                                    AjustColWidth(sheet, 0, colcount);
                                    colcount++;
                                }


                                foreach (DataGridViewRow gvrow in gv.Rows)
                                {
                                    row = sheet.CreateRow(rowidx++);
                                    for (int i = 0; i < colcount; i++)
                                    {
                                        var cell = row.CreateCell(i);
                                        //cell.CellStyle = CreateCellStyle(book);
                                        if (gvrow.Cells[i].Value == DBNull.Value)
                                        {
                                            cell.SetCellValue(string.Empty);
                                        }
                                        else if (gvrow.Cells[i].ValueType == typeof(string))
                                        {
                                            cell.SetCellValue((string)gvrow.Cells[i].Value);
                                        }
                                        else if (gvrow.Cells[i].ValueType == typeof(bool))
                                        {
                                            cell.SetCellValue((bool)gvrow.Cells[i].Value);
                                        }
                                        else if (gvrow.Cells[i].ValueType == typeof(DateTime))
                                        {
                                            cell.SetCellValue(((DateTime)gvrow.Cells[i].Value).ToString("yyyy/MM/dd HH:mm:ss"));
                                        }
                                        else if (gvrow.Cells[i].ValueType == typeof(double)
                                            || gvrow.Cells[i].ValueType == typeof(Single))
                                        {
                                            cell.SetCellValue((double)gvrow.Cells[i].Value);
                                        }
                                        else
                                        {
                                            cell.SetCellValue(gvrow.Cells[i].Value?.ToString() ?? string.Empty);
                                        }
                                    }
                                }

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

                    List<DataGridViewCell> list = new List<DataGridViewCell>();
                    foreach (DataGridViewCell cell in view.SelectedCells)
                    {
                        list.Add(cell);
                    }
                    foreach (var row in list.GroupBy(p => p.RowIndex).OrderBy(p => p.Key))
                    {
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            if (cell.Value != null)
                            {
                                sb.Append(cell.Value.ToString());
                            }
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

                    List<DataGridViewCell> list = new List<DataGridViewCell>();
                    foreach (DataGridViewCell cell in view.SelectedCells)
                    {
                        list.Add(cell);
                    }
                    foreach (var row in list.GroupBy(p => p.RowIndex).OrderBy(p => p.Key))
                    {
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            if (cell.Value != null)
                            {
                                if (cell.ValueType == typeof(byte[]))
                                {
                                    sb.Append("0x" + BitConverter.ToString((byte[])cell.Value).Replace("-", ""));
                                }
                                else
                                {
                                    sb.Append(cell.Value.ToString());
                                }
                            }
                            sb.Append("\t");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();
                    }
                    new SubForm.TextBoxWin("查看文本", sb.ToString().Trim('\t')).ShowDialog();
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
                    List<DataGridViewCell> list = new List<DataGridViewCell>();
                    foreach (DataGridViewCell cell in view.SelectedCells)
                    {
                        list.Add(cell);
                    }

                    foreach (DataGridViewCell cell in list.GroupBy(p => p.RowIndex).First().OrderBy(p => p.ColumnIndex))
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

                    List<DataGridViewCell> list = new List<DataGridViewCell>();
                    foreach (DataGridViewCell cell in view.SelectedCells)
                    {
                        list.Add(cell);
                    }

                    foreach (DataGridViewCell cell in list.GroupBy(p => p.RowIndex).First().OrderBy(p => p.ColumnIndex))
                    {
                        sb.Append(view.Columns[cell.ColumnIndex].Name.ToString());
                        sb.Append("\t");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                    foreach (var row in list.GroupBy(p => p.RowIndex).OrderBy(p => p.Key))
                    {
                        foreach (var cell in row.OrderBy(p => p.ColumnIndex))
                        {
                            if (cell.Value != null)
                            {
                                sb.Append(cell.Value.ToString());
                            }
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
            datastrip.ItemClicked += Datastrip_ItemClicked;

            CanAdjust();

            return this;
        }
    }
}
