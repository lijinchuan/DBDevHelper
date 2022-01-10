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

namespace NETDBHelper.SubForm
{
    public partial class SearchDBDataDlg : SubBaseDlg
    {
        private volatile bool isCancel = false;
        private volatile bool isRunning = false;
        private CheckedListBox mainCLB = null;

        private static string dir = Application.StartupPath + "\\temp\\cachdb";

        public SearchDBDataDlg()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.GVResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            this.GVResult.BorderStyle = BorderStyle.None;
            this.GVResult.GridColor = Color.LightBlue;

            this.GVResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.GVResult.AllowUserToResizeRows = true;

            DBS.Click += DBS_Click;
            TBS.Click += TBS_Click;

            PBCachData.Image = Resources.Resource1.drive_disk;

            ChooseDir(dir);
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

        private bool FilterFun(object val, DataTableColumn col)
        {
            if (val == null)
            {
                return false;
            }
            var key = TBKeyword.Text.Trim();

            if (CBEquals.Checked)
            {
                if (key.ToString() == val.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (CBReg.Checked)
            {
                return Regex.IsMatch(val.ToString(), key);
            }

            return val.ToString().IndexOf(key, StringComparison.OrdinalIgnoreCase) > 0;
            
        }

        private bool Filter(object val, DataTableObject dataTableObject, DataTableColumn col)
        {
            var boo = FilterFun(val, col);
            if (boo)
            {
                BeginInvoke(new Action(() =>
                {
                    if (GVResult.Columns.Count == 0)
                    {
                        GVResult.Columns.Add("库名", "库名");
                        GVResult.Columns.Add("表名", "表名");
                        GVResult.Columns.Add("字段名", "字段名");
                        GVResult.EndEdit();
                    }
                    GVResult.Rows.Add(dataTableObject.DBName, dataTableObject.TableName, col.ColumnName);
                    GVResult.EndEdit();
                }));
            }

            return boo;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBKeyword.Text))
            {
                return;
            }

            if (isRunning)
            {
                return;
            }
            
            isRunning = true;
            GVResult.Rows.Clear();
            FileInfo[] files = null;
            if (Directory.Exists(dir))
            {
                files = new DirectoryInfo(dir).GetFiles("*.data").OrderBy(p => p.CreationTime).ToArray();
            }

            if (files == null || files.Length == 0)
            {
                isRunning = false;
                MessageBox.Show("没有数据，请先缓存数据");
                return;
            }

            if (!string.IsNullOrEmpty(dir))
            {

                new Action(() =>
                {
                    string file = string.Empty;
                    try
                    {

                        var source = (Dictionary<string, List<StringAndBool>>)DBS.Tag;

                        this.BeginInvoke(new Action(() =>
                        {
                            PanelSearchOptions.Enabled = false;
                            ProcessBar.Value = 0;
                            LBMsg.Text = "开始搜索数据";
                        }), null);

                        var seldbs = new List<string>();
                        foreach (var item in DBS.CheckedItems)
                        {
                            seldbs.Add(item.ToString());
                        }
                        
                        var finished = 0;
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
                                if (source != null && !source.ContainsKey(db) && !source[db].Any(p => p.Str == tb && p.Boo))
                                {
                                    continue;
                                }
                            }


                            file = fileinfo.FullName;

                            this.BeginInvoke(new Action(() =>
                            {
                                ProcessBar.Value = finished * 100 / files.Length;
                                LBMsg.Text = "搜索数据：" + fileinfo.Name;
                            }), null);
                            if (file.EndsWith(".data"))
                            {
                                var datatableobject = (DataTableObject)LJC.FrameWorkV3.EntityBuf.EntityBufCore.DeSerialize(typeof(Entity.DataTableObject), file);

                                if (datatableobject != null)
                                {
                                    var hash = new HashSet<string>();
                                    foreach (var r in datatableobject.Rows)
                                    {
                                        int i = 0;
                                        foreach (var c in r.Cells)
                                        {
                                            if (!hash.Contains(datatableobject.Columns[i].ColumnName))
                                            {
                                                try
                                                {
                                                    if (c.IsDBNull)
                                                    {
                                                        //row[i] = DBNull.Value;
                                                    }
                                                    else if (c.ByteValue != null)
                                                    {
                                                        //row[i] = c.ByteValue;
                                                    }
                                                    else
                                                    {
                                                        var val = DataHelper.ConvertDBType(c.StringValue, Type.GetType(datatableobject.Columns[i].ColumnType));

                                                        if (Filter(val, datatableobject, datatableobject.Columns[i]))
                                                        {
                                                            hash.Add(datatableobject.Columns[i].ColumnName);
                                                        }
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    ex.Data.Add("DataType", datatableobject.Columns[i].ColumnType);
                                                    ex.Data.Add("StringValue", c.StringValue);
                                                    throw;
                                                }
                                            }

                                            i++;
                                        }

                                        if (isCancel)
                                        {
                                            break;
                                        }
                                    }

                                    if (isCancel)
                                    {
                                        break;
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

                        ex.Data.Add("file", file);
                        LogHelper.Instance.Error("还原数据失败", ex);
                    }
                    finally
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            isRunning = false;
                            PanelSearchOptions.Enabled = true;

                            if (isCancel)
                            {
                                this.Close();
                            }
                        }));
                    }
                }).BeginInvoke(null, null);
            }
        }

        private void ChooseDir(string path)
        {
            Dictionary<string, List<StringAndBool>> dbtbs = new Dictionary<string, List<StringAndBool>>();
            var files = new DirectoryInfo(path).GetFiles("*.data").OrderBy(p => p.CreationTime).ToArray();
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

            foreach (var item in dbtbs)
            {
                DBS.Items.Add(item.Key);
            }
            for (var i = 0; i < DBS.Items.Count; i++)
            {
                DBS.SetItemChecked(i, true);
            }
            DBS.Tag = dbtbs;

            BtnSearch.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                if (MessageBox.Show("正在搜索数据，确认取消吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    isCancel = true;
                }
            }
            if (!isRunning)
            {
                this.Close();
            }
        }

        private void PBCachData_Click(object sender, EventArgs e)
        {
            new SubForm.CachDB().ShowDialog();
        }

        private void BtnSelAll_Click(object sender, EventArgs e)
        {
            var reg = TBReg.Text;
            if (BtnSelAll.Text == "全选")
            {
                for (int i = 0; i < mainCLB.Items.Count; i++)
                {
                    mainCLB.SetItemChecked(i, IsMatch(mainCLB.Items[i].ToString()));
                }
                BtnSelAll.Text = "全消";
            }
            else
            {
                for (int i = 0; i < mainCLB.Items.Count; i++)
                {
                    mainCLB.SetItemChecked(i, !IsMatch(mainCLB.Items[i].ToString()));
                }
                BtnSelAll.Text = "全选";
            }

            bool IsMatch(string item)
            {
                if (string.IsNullOrEmpty(reg))
                {
                    return true;
                }

                return Regex.IsMatch(item, reg, RegexOptions.IgnoreCase);
            }
        }
    }
}
