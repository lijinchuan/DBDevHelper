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
    public partial class RecoverDBDlg : Form
    {
        public RecoverDBDlg()
        {
            InitializeComponent();
        }

        private void BtnChooseDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TBPath.Text = dlg.SelectedPath;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dir = TBPath.Text;
            if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
            {
                foreach(var file in System.IO.Directory.GetFiles(dir))
                {
                    if (file.EndsWith(".data"))
                    {
                        var datatableobject = (Entity.DataTableObject)LJC.FrameWorkV3.EntityBuf.EntityBufCore.DeSerialize(typeof(Entity.DataTableObject), file);

                        if (datatableobject != null)
                        {
                            DataTable table = new DataTable();
                            foreach(var col in datatableobject.Columns)
                            {
                                table.Columns.Add(new DataColumn
                                {
                                    ColumnName=col.ColumnName,
                                    DataType=Type.GetType(col.ColumnType)
                                });
                            }

                            foreach(var r in datatableobject.Rows)
                            {
                                var row = table.NewRow();
                                int i = 0;
                                foreach(var c in r.Cells)
                                {
                                    if (c.IsDBNull)
                                    {
                                        row[i] = DBNull.Value;
                                    }
                                    else if (c.ByteValue != null)
                                    {
                                        row[i] = c.ByteValue;
                                    }
                                    else
                                    {
                                        row[i] = Convert.ChangeType(c.StringValue, table.Columns[i].DataType);
                                    }
                                    i++;
                                }

                                table.Rows.Add(row);
                            }
                        }
                    }
                }
            }
        }
    }
}
