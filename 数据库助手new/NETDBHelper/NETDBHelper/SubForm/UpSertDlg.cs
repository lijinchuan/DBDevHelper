using Entity;
using LJC.FrameWork.LogManager;
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
        private DataGridViewRow _oldRow;
        public UpSertDlg()
        {
            InitializeComponent();
        }

        public UpSertDlg(DBSource source, TableInfo tableInfo, DataGridViewRow updateRow = null)
        {
            InitializeComponent();

            _source = source;
            _table = tableInfo;
            _oldRow = updateRow;
        }

        private object getUpdateValue(string field)
        {
            if (_oldRow == null)
            {
                return null;
            }
            if (_oldRow.DataGridView.Columns.Contains(field))
            {
                return _oldRow.Cells[field].Value;
            }
            return null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var cols = Biz.Common.Data.SQLHelper.GetColumns(_source, _table.DBName, _table.TBName, _table.Schema).ToList();

            int preoffsetx = 20, offsetx = 20, preoffsety = 10, offsety = 10, maxHigh = 0;
            foreach (var column in cols)
            {
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Location = new Point(preoffsetx, preoffsety);
                lb.Text = column.Name + $"({column.TypeName}): ";
                ItemsPannel.Controls.Add(lb);

                Control valControl = null;

                preoffsetx += lb.Width;

                var fileVal = getUpdateValue(column.Name);

                if (column.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("float", StringComparison.OrdinalIgnoreCase) > -1
                            //|| column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                        )
                {
                    var tb = new TextBox();
                    if (column.IsID)
                    {
                        tb.Text = fileVal == null? "0": fileVal.ToString();
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
                    valControl = new TextBox();
                    if (fileVal != null)
                    {
                        valControl.Text = fileVal.ToString();
                    }
                }
                else if (column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase))
                {
                    var cb = new ComboBox();
                    cb.Items.AddRange(new object[] { true, false });
                    valControl = cb;
                    if (fileVal != null)
                    {
                        cb.SelectedItem = fileVal;
                    }
                }
                else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("date", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("smalldatetime", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
                {
                    DateTimePicker picker = new DateTimePicker();
                    valControl = picker;
                    if (fileVal != null)
                    {
                        picker.Value = (DateTime)fileVal;
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
                    var tb = new TextBox();
                    if (column.Length == -1 || column.Length > 100)
                    {
                        tb.Multiline = true;
                        tb.Width = GBValues.Width - 20 - lb.Width;
                        tb.Height = 50;
                    }
                    if (fileVal != null)
                    {
                        tb.Text = fileVal.ToString();
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
                if(!(valControl is Label))
                {
                    valControl.Tag = column;
                }
            }

            
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

                    if (column.IsKey)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(ctl.Text) && !column.IsNullAble)
                    {
                        ctl.BackColor = Color.Yellow;
                        hasError = true;
                    }
                    else if (column.TypeName.IndexOf("nvarchar", StringComparison.OrdinalIgnoreCase) > -1
                   || column.TypeName.IndexOf("varchar", StringComparison.OrdinalIgnoreCase) > -1
                   || column.TypeName.IndexOf("char", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (column.Length != -1 && ctl.Text.Length > column.Length)
                        {
                            ctl.BackColor = Color.Pink;
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

                var tb = Biz.Common.Data.SQLHelper.ExecuteDBTable(_source, _table.DBName, sql);

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
                    var val = Biz.Common.Data.DataHelper.ConvertDBType(ctl.Text, valtype);

                    if (val != getUpdateValue(column.Name))
                    {
                        updateCols.Add(column.Name);
                        @params.Add(new SqlParameter
                        {
                            ParameterName = $"@{column.Name}",
                            Value = val
                        });
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

                if (conditionCols.Count() == 0)
                {
                    throw new Exception("无唯一ID或者KEY可以用来更新数据");
                }

                sb.AppendFormat("{0}", string.Join(",", updateCols.Select(p => $"[{p}]=@{p}")));

                sb.AppendFormat(" where {0}", string.Join(" and ", conditionCols.Select(p => $"[{p}]=@{p}")));

                Biz.Common.Data.SQLHelper.ExecuteNoQuery(_source, _table.DBName, sb.ToString(), @params.ToArray());

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

                var tb = Biz.Common.Data.SQLHelper.ExecuteDBTable(_source, _table.DBName, sql);

                StringBuilder sb = new StringBuilder($"insert into [{_table.DBName}].[{_table.Schema}].[{_table.TBName}](");

                List<SqlParameter> @params = new List<SqlParameter>();
                foreach (Control ctl in ItemsPannel.Controls)
                {
                    if (!(ctl.Tag is TBColumn))
                    {
                        continue;
                    }
                    var column = (TBColumn)ctl.Tag;
                    if (column.IsKey)
                    {
                        continue;
                    }

                    var valtype = tb.Columns[column.Name].DataType;
                    var val = Biz.Common.Data.DataHelper.ConvertDBType(ctl.Text, valtype);
                    @params.Add(new SqlParameter
                    {
                        ParameterName = $"@{column.Name}",
                        Value = val
                    });
                }
                sb.AppendFormat("{0}", string.Join(",", cols.Select(p => $"[{p}]")));
                sb.Append(")");
                sb.AppendFormat(" values({0})", string.Join(",", cols.Select(p => $"@{p}")));

                Biz.Common.Data.SQLHelper.ExecuteNoQuery(_source, _table.DBName, sb.ToString(), @params.ToArray());

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
            if (_oldRow == null)
            {
                NewAdd();
            }
            else
            {
                UpdateItem();
            }
        }
    }
}
