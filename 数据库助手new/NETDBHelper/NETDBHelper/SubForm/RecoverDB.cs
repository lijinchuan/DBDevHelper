using Biz.Common.Data;
using Entity;
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
        private CheckedListBox mainCLB = null;
        public RecoverDBDlg()
        {
            InitializeComponent();
            linkLabel1.Enabled = false;
            BtnRecover.Enabled = false;

            DBS.Click += DBS_Click;
            TBS.Click += TBS_Click;
        }

        private void TBS_Click(object sender, EventArgs e)
        {
            mainCLB = TBS;
        }

        private void DBS_Click(object sender, EventArgs e)
        {
            mainCLB = DBS;
            if (DBS.SelectedItem == null)
            {
                return;
            }
            TBS.ItemCheck -= TBS_ItemCheck;

            var seldb = DBS.SelectedItem.ToString();
            var souces = (Dictionary<string, List<StringAndBool>>)DBS.Tag;
            TBS.Items.Clear();

            for (var i = 0; i < souces[seldb].Count; i++)
            {
                var item = souces[seldb][i];
                TBS.Items.Add(item.Str);
                if (item.Boo)
                {
                    TBS.SetItemChecked(i, item.Boo);
                }
            }
            TBS.ItemCheck += TBS_ItemCheck;
            TBS.Tag = souces[seldb];
        }

        private void TBS_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (TBS.Tag == null)
            {
                return;
            }

            var list = (List<StringAndBool>)TBS.Tag;
            var sb = list.Find(p => p.Str == TBS.Items[e.Index].ToString());
            sb.Boo = e.NewValue == CheckState.Checked;
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

                Dictionary<string, List<StringAndBool>> dbtbs = new Dictionary<string, List<StringAndBool>>();
                var files = new DirectoryInfo(dlg.SelectedPath).GetFiles("*.data").OrderBy(p => p.CreationTime).ToArray();
                foreach (var file in files)
                {
                    var m = Regex.Match(file.Name, @"\[([^\]]+)\]\.\[([^\]]+)\]");
                    if (m.Success)
                    {
                        var db = m.Groups[1].Value;
                        var tb = m.Groups[2].Value;
                        if (!dbtbs.ContainsKey(db))
                        {
                            dbtbs.Add(db, new List<StringAndBool>());
                        }

                        dbtbs[db].Add(new StringAndBool(tb, true));
                    }
                }

                DBS.Items.Clear();
                TBS.Items.Clear();

                foreach(var item in dbtbs)
                {
                    DBS.Items.Add(item.Key);
                }
                for(var i = 0; i < DBS.Items.Count; i++)
                {
                    DBS.SetItemChecked(i, true);
                }
                DBS.Tag = dbtbs;

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
            var successDir = Path.Combine(dir, "success");
            if (!Directory.Exists(successDir))
            {
                Directory.CreateDirectory(successDir);
            }
            if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
            {
                var connsqlserver = new ConnSQLServer();
                if (connsqlserver.ShowDialog() != DialogResult.OK)
                {
                    isRunning = false;
                    MessageBox.Show("已取消。");
                    return;
                }

                new Action(() =>
                {
                    string file = string.Empty;
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

                        var source = (Dictionary<string, List<StringAndBool>>)DBS.Tag;

                        this.BeginInvoke(new Action(() =>
                        {
                            ProcessBar.Value = 0;
                            LBMsg.Text = "导入数据库和表完成，开始导入数据";
                        }), null);

                        var seldbs = new List<string>();
                        foreach(var item in DBS.CheckedItems)
                        {
                            seldbs.Add(item.ToString());
                        }
                        var files = new DirectoryInfo(dir).GetFiles("*.data").OrderBy(p => p.CreationTime).ToArray();
                        var finished = 0;
                        var batchSize = 2000;
                        foreach (var fileinfo in files)
                        {
                            if (isCancel)
                            {
                                break;
                            }

                            var m = Regex.Match(fileinfo.Name, @"\[([^\]]+)\]\.\[([^\]]+)\]");
                            if (m.Success)
                            {
                                var db = m.Groups[1].Value;
                                var tb = m.Groups[2].Value;
                                if (!seldbs.Contains(db))
                                {
                                    continue;
                                }
                                if (source != null && source.ContainsKey(db) && !source[db].Any(p => p.Str == tb && p.Boo))
                                {
                                    continue;
                                }
                            }


                            file = fileinfo.FullName;

                            this.BeginInvoke(new Action(() =>
                            {
                                ProcessBar.Value = finished * 100 / files.Length;
                                LBMsg.Text = "导入数据：" + fileinfo.Name;
                            }), null);
                            if (file.EndsWith(".data"))
                            {
                                var datatableobject = (Entity.DataTableObject)LJC.FrameWorkV3.EntityBuf.EntityBufCore.DeSerialize(typeof(Entity.DataTableObject), file);

                                if (datatableobject != null)
                                {
                                    bool hasError = false;
                                    if (CBClear.Checked)
                                    {
                                        try
                                        {
                                            SQLHelper.ExecuteNoQuery(connsqlserver.DBSource, datatableobject.DBName, $"truncate table [{datatableobject.DBName}].[{datatableobject.Schema}].[{datatableobject.TableName}]");
                                        }
                                        catch
                                        {
                                            SQLHelper.ExecuteNoQuery(connsqlserver.DBSource, datatableobject.DBName, $"delete [{datatableobject.DBName}].[{datatableobject.Schema}].[{datatableobject.TableName}]");
                                        }
                                    }

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
                                            try
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
                                                    row[i] = DataHelper.ConvertDBType(c.StringValue, table.Columns[i].DataType);
                                                }
                                                i++;
                                            }
                                            catch (Exception ex)
                                            {
                                                ex.Data.Add("DataType", table.Columns[i].DataType);
                                                ex.Data.Add("StringValue", c.StringValue);
                                                throw;
                                            }
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
                                                SQLHelper.SqlBulkCopy(connsqlserver.DBSource, datatableobject.DBName, (int)TimeOutMins.Value * 60 * 1000, "[" + datatableobject.Schema + "].[" + datatableobject.TableName + "]", table);
                                            }
                                            catch (Exception ex)
                                            {
                                                hasError = true;
                                                if (!CBIgnoreError.Checked)
                                                {
                                                    throw;
                                                }
                                                else
                                                {
                                                    LogHelper.Instance.Error($"{datatableobject.DBName}.{datatableobject.Schema}.{datatableobject.TableName}还原数据失败", ex);
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
                                        SQLHelper.SqlBulkCopy(connsqlserver.DBSource, datatableobject.DBName, (int)TimeOutMins.Value * 60 * 1000, "[" + datatableobject.Schema + "].[" + datatableobject.TableName + "]", table);

                                        if (!hasError)
                                        {
                                            LJC.FrameWorkV3.Comm.IOUtil.MoveFileToDir(file, successDir);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!CBIgnoreError.Checked)
                                        {
                                            throw;
                                        }
                                        else
                                        {
                                            LogHelper.Instance.Error($"{datatableobject.DBName}.{datatableobject.Schema}.{datatableobject.TableName}还原数据失败", ex);
                                        }
                                    }
                                }
                            }
                            finished++;
                        }

                        BeginInvoke(new Action(() =>
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
                            LBMsg.Text = new FileInfo(file).Name + "出错:" + ex.Message;
                        }), null);

                        ex.Data.Add("file", file);
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

        private void BtnSelAll_Click(object sender, EventArgs e)
        {
            if(mainCLB==null)
            {
                return;
            }
            if (BtnSelAll.Text == "全选")
            {
                for (int i = 0; i < this.mainCLB.Items.Count; i++)
                {
                    this.mainCLB.SetItemChecked(i, true);
                }
                BtnSelAll.Text = "全消";
            }
            else
            {
                for (int i = 0; i < this.mainCLB.Items.Count; i++)
                {
                    this.mainCLB.SetItemChecked(i, false);
                }
                BtnSelAll.Text = "全选";
            }
        }
    }
}
