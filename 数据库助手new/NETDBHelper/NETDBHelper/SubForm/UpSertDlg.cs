using Biz.Common.Data;
using Entity;
using LJC.FrameWork.LogManager;
using LJC.FrameWorkV3.Data.EntityDataBase;
using NETDBHelper.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class UpSertDlg : SubBaseDlg
    {
        private DBSource _source;
        private TableInfo _table;
        private DataGridViewRow _editRow;
        private DataGridViewRow _copyRow;
        
        public UpSertDlg()
        {
            InitializeComponent();
        }

        public UpSertDlg(DBSource source, TableInfo tableInfo, DataGridViewRow updateRow = null, DataGridViewRow copyRow = null)
        {
            InitializeComponent();

            _source = source;
            _table = tableInfo;
            _editRow = updateRow;
            if (updateRow == null)
            {
                _copyRow = copyRow;
            }

            if (_editRow != null)
            {
                Text = "编辑数据";
            }
            else if (_copyRow != null)
            {
                Text = "复制新增数据";
            }
            else
            {
                Text = "新增数据";
            }
        }

        private void SetControls(bool reset=false)
        {
            if (reset)
            {
                ItemsPannel.Controls.Clear();
            }

            var cols = SQLHelper.GetColumns(_source, _table.DBName, _table.TBName, _table.Schema).ToList();

            int preoffsetx = 20, offsetx = 20, preoffsety = 10, offsety = 10, maxHigh = 0;
            foreach (var column in cols)
            {
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Location = new Point(preoffsetx, preoffsety);
                lb.Text = column.Name+":";
                ItemsPannel.Controls.Add(lb);

                Control valControl = null;

                preoffsetx += lb.Width;

                var copyVal = (column.IsKey || column.IsID) ? null : getCopyValue(column.Name);
                var editVal = getUpdateValue(column.Name) ?? copyVal;


                if (column.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("float", StringComparison.OrdinalIgnoreCase) > -1
                            //|| column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                        )
                {
                    var tb = new UCTextBox();
                    tb.ShowCheckBox = column.IsNullAble;
                    tb.Text = editVal == null && column.IsNullAble ? "" : (editVal ?? 0).ToString();
                    if (column.IsID)
                    {
                        tb.ReadOnly = true;
                    }
                    valControl = tb;
                }
                else if (column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("binary", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("varbinary", StringComparison.OrdinalIgnoreCase))
                {
                    //
                    valControl = new Label();
                    valControl.Text = "不支持";
                }
                else if (column.TypeName.Equals("uniqueidentifier", StringComparison.OrdinalIgnoreCase))
                {
                    var tbb = new UCTextBox();
                    tbb.ShowCheckBox = column.IsNullAble;
                    valControl = tbb;
                    
                    if (editVal != null)
                    {
                        valControl.Text = editVal.ToString();
                    }
                }
                else if (column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase))
                {
                    var cb = new ComboBox();
                    cb.Items.AddRange(new object[] { true, false });
                    valControl = cb;
                    if (editVal != null)
                    {
                        cb.SelectedItem = editVal;
                    }
                }
                else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("date", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("smalldatetime", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
                {
                    UCDateTime picker = new UCDateTime();

                    if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                        || column.TypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
                    {
                        picker.Format = DateTimePickerFormat.Custom;
                        picker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    }
                    else if (column.TypeName.Equals("smalldatetime", StringComparison.OrdinalIgnoreCase))
                    {
                        picker.Format = DateTimePickerFormat.Short;
                    }

                    valControl = picker;
                    if (editVal != null)
                    {
                        picker.Value = (DateTime)editVal;
                    }
                }
                else if (column.TypeName.Equals("sql_variant", StringComparison.OrdinalIgnoreCase))
                {
                    valControl = new Label();
                    valControl.Text = "不支持";
                }
                else if (
                    column.TypeName.IndexOf("nvarchar", StringComparison.OrdinalIgnoreCase) > -1
                    || column.TypeName.IndexOf("varchar", StringComparison.OrdinalIgnoreCase) > -1
                    || column.TypeName.IndexOf("char", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var tb = new UCTextBox();
                    tb.ShowCheckBox = column.IsNullAble;
                    if (column.Length == -1 || column.Length > 100)
                    {
                        tb.Multiline = true;
                        tb.Width = ItemsPannel.Width - lb.Width - 40;
                        tb.Height = 100;
                    }
                    else
                    {
                        tb.Width = 250;
                    }
                    if (editVal != null)
                    {
                        tb.Text = editVal.ToString();
                    }
                    valControl = tb;
                }
                valControl.Location = new Point(preoffsetx, preoffsety);

                preoffsetx += valControl.Width;
                if (preoffsetx > ItemsPannel.Width)
                {
                    offsetx = 20;
                    offsety += maxHigh + 10;
                    lb.Location = new Point(offsetx, offsety);
                    offsetx += lb.Width;
                    valControl.Location = new Point(offsetx, offsety);
                    offsetx += valControl.Width;
                    preoffsetx = offsetx;
                    preoffsety = offsety;

                    maxHigh = 0;
                }
                else
                {
                    offsetx = preoffsetx;
                }

                ItemsPannel.Controls.Add(valControl);

                if (valControl.Height > maxHigh)
                {
                    maxHigh = valControl.Height;
                }
                if (!(valControl is Label))
                {
                    valControl.Tag = column;
                }

                var typestr = (string.IsNullOrEmpty(column.Description) ? "" : $"{column.Description}:") + column.TypeName;
                if (column.Length>0)
                {
                    typestr += "(" + column.Length + ")";
                }
                
                Win32Utility.SetCueText(valControl, typestr);
            }
        }

        private object getUpdateValue(string field)
        {
            if (_editRow == null)
            {
                return null;
            }
            if (_editRow.DataGridView.Columns.Contains(field))
            {
                var obj = _editRow.Cells[field].Value;
                if (Equals(obj, DBNull.Value))
                {
                    return null;
                }
                return obj;
            }
            return null;
        }

        private object getCopyValue(string field)
        {
            if (_copyRow == null)
            {
                return null;
            }
            if (_copyRow.DataGridView.Columns.Contains(field))
            {
                var obj = _copyRow.Cells[field].Value;
                if (Equals(obj, DBNull.Value))
                {
                    return null;
                }
                return obj;
            }
            return null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            SetControls();
            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool CheckHasError(ref List<string> cols)
        {
            var hasError = false;
            foreach (Control ctl in ItemsPannel.Controls)
            {
                if (ctl.Tag is TBColumn)
                {
                    var column = (TBColumn)ctl.Tag;

                    if (column.IsID)
                    {
                        continue;
                    }

                    if (ctl.Text==null && !column.IsNullAble)
                    {
                        ctl.BackColor = Color.Yellow;
                        hasError = true;
                    }
                    else if (column.TypeName.IndexOf("nvarchar", StringComparison.OrdinalIgnoreCase) > -1
                   || column.TypeName.IndexOf("varchar", StringComparison.OrdinalIgnoreCase) > -1
                   || column.TypeName.IndexOf("char", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (column.Length != -1 && ctl.Text?.Length > column.Length)
                        {
                            ctl.BackColor = Color.Red;
                            hasError = true;
                        }
                    }
                    else
                    {
                        ctl.BackColor = Color.White;
                    }

                    cols.Add(column.Name);
                }
            }
            return hasError;
        }

        private void UpdateItem()
        {
            List<string> cols = new List<string>();
            if (CheckHasError(ref cols))
            {
                return;
            }

            try
            {
                var sql = $"select top 0 {string.Join(",", cols.Select(p => $"[{p}]"))} from [{_table.DBName}].[{_table.Schema}].[{_table.TBName}] with(nolock)";

                var tb = SQLHelper.ExecuteDBTable(_source, _table.DBName, sql);

                StringBuilder sb = new StringBuilder($"update [{_table.DBName}].[{_table.Schema}].[{_table.TBName}] set ");
                List<string> updateCols = new List<string>();
                List<string> conditionCols = new List<string>();
                List<SqlParameter> @params = new List<SqlParameter>();
                foreach (Control ctl in ItemsPannel.Controls)
                {
                    if (!(ctl.Tag is TBColumn))
                    {
                        continue;
                    }
                    var column = (TBColumn)ctl.Tag;
                    if (column.IsID)
                    {
                        conditionCols.Add(column.Name);
                        @params.Add(new SqlParameter
                        {
                            ParameterName = $"@{column.Name}",
                            Value = getUpdateValue(column.Name)
                        });
                        continue;
                    }


                    var valtype = tb.Columns[column.Name].DataType;
                    object val = null;
                    if (ctl is UCDateTime)
                    {
                        var picker = ctl as UCDateTime;
                        val = picker.Value;
                        
                    }
                    else
                    {
                        val = (ctl.Text == null && column.IsNullAble) ? null : DataHelper.ConvertDBType(ctl.Text, valtype);
                    }

                    if (!Equals(val, getUpdateValue(column.Name)))
                    {
                        updateCols.Add(column.Name);
                        @params.Add(new SqlParameter
                        {
                            ParameterName = $"@{column.Name}",
                            Value = val??DBNull.Value
                        });

                        ctl.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        ctl.BackColor = Color.White;
                    }

                    if (column.IsKey)
                    {
                        conditionCols.Add(column.Name);
                        @params.Add(new SqlParameter
                        {
                            ParameterName = $"@{column.Name}",
                            Value = getUpdateValue(column.Name)
                        });
                    }
                }

                if (!conditionCols.Any())
                {
                    throw new Exception("无唯一ID或者KEY可以用来更新数据");
                }

                if (!updateCols.Any())
                {
                    throw new Exception("数据没有修改");
                }

                if (MessageBox.Show("确认修改吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                sb.AppendFormat("{0}", string.Join(",", updateCols.Select(p => $"[{p}]=@{p}")));

                sb.AppendFormat(" where {0}", string.Join(" and ", conditionCols.Select(p => $"[{p}]=@{p}")));

                //记录日志
                //备份
                var sqlquery = $"select * from [{_table.DBName}].[{_table.Schema}].[{_table.TBName}] where {string.Join(" and ", conditionCols.Select(p => "[" + p + "]=@" + p))}";

                var datatable = SQLHelper.ExecuteDBTable(_source, _table.DBName, sqlquery, @params.Where(p => conditionCols.Contains(p.ParameterName.TrimStart('@'))).Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());
                if (datatable.Rows.Count != 1)
                {
                    MessageBox.Show("修改失败，不能备份数据");
                    return;
                }

                Dictionary<string, object> oldValue = new Dictionary<string, object>();
                foreach (DataColumn col in datatable.Columns)
                {
                    oldValue.Add(col.ColumnName, datatable.Rows[0][col]);
                }

                Dictionary<string, object> upValue = new Dictionary<string, object>();
                foreach(var pa in @params)
                {
                    upValue.Add(pa.ParameterName.TrimStart('@'), pa.Value);
                }

                LogHelper.Instance.Info($"预修改，前值为:" + Newtonsoft.Json.JsonConvert.SerializeObject(oldValue) + "，修改值为:" + Newtonsoft.Json.JsonConvert.SerializeObject(upValue));

                SQLHelper.ExecuteNoQuery(_source, _table.DBName, sb.ToString(), @params.ToArray());
                BigEntityTableRemotingEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = "table",
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.db,
                    DB = _table.DBName,
                    Sever = _source.ServerName,
                    Info = $"修改，前值为:" + Newtonsoft.Json.JsonConvert.SerializeObject(oldValue) + "，修改值为:" + Newtonsoft.Json.JsonConvert.SerializeObject(upValue),
                    Valid = true
                });
                MessageBox.Show("更新成功");
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex.Message, ex);
                MessageBox.Show("失败:" + ex.Message);
            }
        }

        private void NewAdd()
        {
            List<string> cols = new List<string>();
            if (CheckHasError(ref cols))
            {
                return;
            }

            try
            {
                var sql = $"select top 0 {string.Join(",", cols.Select(p => $"[{p}]"))} from [{_table.DBName}].[{_table.Schema}].[{_table.TBName}] with(nolock)";

                var tb = SQLHelper.ExecuteDBTable(_source, _table.DBName, sql);

                StringBuilder sb = new StringBuilder($"insert into [{_table.DBName}].[{_table.Schema}].[{_table.TBName}](");

                List<SqlParameter> @params = new List<SqlParameter>();
                foreach (Control ctl in ItemsPannel.Controls)
                {
                    if (!(ctl.Tag is TBColumn))
                    {
                        continue;
                    }
                    var column = (TBColumn)ctl.Tag;
                    if (column.IsID)
                    {
                        continue;
                    }

                    var valtype = tb.Columns[column.Name].DataType;
                    if (ctl is UCDateTime)
                    {
                        var picker = ctl as UCDateTime;

                        var val = picker.Value;
                        @params.Add(new SqlParameter
                        {
                            ParameterName = $"@{column.Name}",
                            Value = val
                        });
                    }
                    else
                    {
                        var val = (ctl.Text == null && column.IsNullAble) ? null : DataHelper.ConvertDBType(ctl.Text, valtype);
                        @params.Add(new SqlParameter
                        {
                            ParameterName = $"@{column.Name}",
                            Value = val ?? DBNull.Value
                        });
                    }
                }
                sb.AppendFormat("{0}", string.Join(",", cols.Select(p => $"[{p}]")));
                sb.Append(")");
                sb.AppendFormat(" values({0})", string.Join(",", cols.Select(p => $"@{p}")));

                SQLHelper.ExecuteNoQuery(_source, _table.DBName, sb.ToString(), @params.ToArray());

                MessageBox.Show("新增成功");
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex.Message, ex);
                MessageBox.Show("失败:" + ex.Message);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (_editRow == null)
            {
                NewAdd();
            }
            else
            {
                UpdateItem();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("重置吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            SetControls(true);
        }
    }
}
