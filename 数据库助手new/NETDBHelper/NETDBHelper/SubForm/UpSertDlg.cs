using Entity;
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
    public partial class UpSertDlg : SubBaseDlg
    {
        private DBSource _source;
        private TableInfo _table;
        public UpSertDlg()
        {
            InitializeComponent();
        }

        public UpSertDlg(DBSource source,TableInfo tableInfo)
        {
            InitializeComponent();

            _source = source;
            _table = tableInfo;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var cols = Biz.Common.Data.SQLHelper.GetColumns(_source, _table.DBName, _table.TBName, _table.Schema).ToList();

            int preoffsetx = 20, offsetx = 20, preoffsety = 10, offsety = 10;
            foreach (var column in cols)
            {
                Label lb = new Label();
                lb.Location = new Point(preoffsetx, preoffsety);
                lb.Text = column.Name + ": ";
                GBValues.Controls.Add(lb);

                Control valControl = null;

                preoffsetx += lb.Width;

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
                        tb.Text = "0";
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
                }
                else if (column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase))
                {
                    var cb = new ComboBox();
                    cb.Items.AddRange(new object[] { true, false });
                    valControl = cb;
                }
                else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("date", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("smalldatetime", StringComparison.OrdinalIgnoreCase)
                    || column.TypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
                {
                    DateTimePicker picker = new DateTimePicker();
                    valControl = picker;
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
                        tb.Height = 30;
                    }
                    valControl = tb;
                }

                preoffsetx += valControl.Width;
                if (preoffsetx > GBValues.Width)
                {
                    offsetx = 20;
                    offsety += lb.Height;
                    lb.Location = new Point(offsetx, offsety);
                    offsetx += lb.Width;
                    valControl.Location = new Point(offsetx, offsety);

                    preoffsetx = offsetx;
                    preoffsety = offsety;
                }
                else
                {
                    offsetx = preoffsetx;
                }
            }

        }
    }
}
