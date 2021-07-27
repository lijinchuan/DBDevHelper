using LJC.FrameWorkV3.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Environment;

namespace NETDBHelper.SubForm
{
    public partial class RecoverDBDlg : Form
    {
        private volatile bool isCancel = false;
        private volatile bool isRunning = false;
        public RecoverDBDlg()
        {
            InitializeComponent();
            linkLabel1.Enabled = false;
            BtnRecover.Enabled = false;
        }

        private void BtnChooseDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var sqlfile = System.IO.Path.Combine(dlg.SelectedPath, "createdb.sql");
                if (!System.IO.File.Exists(sqlfile))
                {
                    MessageBox.Show("失败，找不到创建文件。");
                    return;
                }
                TBPath.Text = dlg.SelectedPath;
                linkLabel1.Enabled = true;
                BtnRecover.Enabled = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                if (MessageBox.Show("正在导入数据，确认取消吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    isCancel = true;
                }
            }
            if (!isRunning)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            groupBox1.Enabled = false;
            linkLabel1.Enabled = false;
            var dir = TBPath.Text;
            if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
            {
                var connsqlserver = new ConnSQLServer();
                if (connsqlserver.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("已取消。");
                    return;
                }

                new Action(() =>
                {
                    try
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            ProcessBar.Value = 0;
                            LBMsg.Text = "开始导入数据库和表";
                        }), null);

                        //var sql = File.ReadAllText(sqlfile, Encoding.UTF8);
                        //sql = Regex.Replace(sql, "^[\\s]*GO[\\s]*$", ";", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        //Biz.Common.Data.SQLHelper.ExecuteNoQuery(connsqlserver.DBSource, "master", sql, 1000 * 3600);

                        this.BeginInvoke(new Action(() =>
                        {
                            ProcessBar.Value = 0;
                            LBMsg.Text = "导入数据库和表完成，开始导入数据";
                        }), null);

                        var files = new DirectoryInfo(dir).GetFiles("*.data").OrderBy(p => p.CreationTime).Select(p => p.FullName).ToArray();
                        var finished = 0;
                        var batchSize = 2000;
                        foreach (var file in files)
                        {
                            if (isCancel)
                            {
                                break;
                            }

                            this.BeginInvoke(new Action(() =>
                            {
                                ProcessBar.Value = finished * 100 / files.Length;
                                LBMsg.Text = "导入数据：" + file;
                            }), null);
                            if (file.EndsWith(".data"))
                            {
                                var datatableobject = (Entity.DataTableObject)LJC.FrameWorkV3.EntityBuf.EntityBufCore.DeSerialize(typeof(Entity.DataTableObject), file);

                                if (datatableobject != null)
                                {
                                    DataTable table = new DataTable();
                                    foreach (var col in datatableobject.Columns)
                                    {
                                        table.Columns.Add(new DataColumn
                                        {
                                            ColumnName = col.ColumnName,
                                            DataType = Type.GetType(col.ColumnType)
                                        });
                                    }

                                    foreach (var r in datatableobject.Rows)
                                    {
                                        var row = table.NewRow();
                                        int i = 0;
                                        foreach (var c in r.Cells)
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
                                                if (table.Columns[i].DataType == typeof(Guid))
                                                {
                                                    row[i] = Guid.Parse(c.StringValue);
                                                }
                                                else
                                                {
                                                    row[i] = Convert.ChangeType(c.StringValue, table.Columns[i].DataType);
                                                }
                                            }
                                            i++;
                                        }

                                        table.Rows.Add(row);

                                        if (isCancel)
                                        {
                                            break;
                                        }

                                        if (table.Rows.Count >= batchSize)
                                        {
                                            //批量
                                            try
                                            {
                                                Biz.Common.Data.SQLHelper.SqlBulkCopy(connsqlserver.DBSource, datatableobject.DBName, (int)TimeOutMins.Value * 60 * 1000, "[" + datatableobject.Schema + "].[" + datatableobject.TableName + "]", table);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (!CBIgnoreError.Checked)
                                                {
                                                    throw;
                                                }
                                                else
                                                {
                                                    ex.Data.Add("file", file);
                                                    LogHelper.Instance.Error("还原数据失败", ex);
                                                }
                                            }
                                            table.Rows.Clear();
                                        }
                                    }

                                    if (isCancel)
                                    {
                                        break;
                                    }

                                    //批量
                                    try
                                    {
                                        Biz.Common.Data.SQLHelper.SqlBulkCopy(connsqlserver.DBSource, datatableobject.DBName, (int)TimeOutMins.Value * 60 * 1000, "[" + datatableobject.Schema + "].[" + datatableobject.TableName + "]", table);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!CBIgnoreError.Checked)
                                        {
                                            throw;
                                        }
                                        else
                                        {
                                            ex.Data.Add("file", file);
                                            LogHelper.Instance.Error("还原数据失败", ex);
                                        }
                                    }
                                }
                            }
                            finished++;
                        }

                        this.BeginInvoke(new Action(() =>
                        {
                            ProcessBar.Value = 100;
                            LBMsg.Text = "完成";
                        }), null);
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            ProcessBar.Value = 100;
                            LBMsg.Text = "失败：" + ex.Message;
                        }), null);

                        LogHelper.Instance.Error("还原数据失败", ex);
                    }
                    finally
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            isRunning = false;
                            groupBox1.Enabled = true;
                            linkLabel1.Enabled = true;

                            if (isCancel)
                            {
                                this.Close();
                            }
                        }));
                    }
                }).BeginInvoke(null, null);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var file = Path.Combine(TBPath.Text, "createdb.sql");
            System.Diagnostics.Process.Start(file);
        }
    }
}
