using Biz.Common.Data;
using Entity;
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
using System.Linq;

namespace NETDBHelper.SubForm
{
    public partial class CopyDB : Form
    {
        private volatile Task<int> saveTask = null;
        private bool cancel = false;
        private bool stop = false;
        private CheckedListBox mainCLB = null;
        private CopyDBTask currentTask = null;
        public CopyDB()
        {
            InitializeComponent();
            this.CLBDBs.Items.Clear();

            this.PannelCopNumber.Enabled = CBData.Checked;
            CBData.CheckedChanged += CBData_CheckedChanged;

            mainCLB = CLBDBs;
            CLBDBs.Click += CLBDBs_Click;
            CLBTBS.Click += CLBTBS_Click;
            
        }

        private void CLBTBS_Click(object sender, EventArgs e)
        {
            mainCLB = CLBTBS;
            if (CLBTBS.CheckedItems.Count < CLBTBS.Items.Count)
            {
                BtnSelAll.Text = "全选";
            }
            else
            {
                BtnSelAll.Text = "全消";
            }
        }

        private void CLBTBS_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<StringAndBool> tuples = (List<StringAndBool>)CLBTBS.Tag;

            var tp = tuples.Find(p => p.Str == CLBTBS.Items[e.Index].ToString());
            tp.Boo = e.NewValue == CheckState.Checked;
        }

        private void CLBDBs_Click(object sender, EventArgs e)
        {
            mainCLB = CLBDBs;
            if (CLBDBs.CheckedItems.Count < CLBDBs.Items.Count)
            {
                BtnSelAll.Text = "全选";
            }
            else
            {
                BtnSelAll.Text = "全消";
            }

            CLBTBS.ItemCheck -= CLBTBS_ItemCheck;
            this.CLBTBS.Items.Clear();

            var db = (string)CLBDBs.SelectedItem;

            if (CLBDBs.Tag != null)
            {
                var dic = (Dictionary<string, List<StringAndBool>>)CLBDBs.Tag;

                if (!dic.ContainsKey(db))
                {
                    var tbs = SQLHelper.GetTBs(DBSource, db);

                    var list = new List<StringAndBool>();
                    foreach (var row in tbs.AsEnumerable())
                    {
                        list.Add(new StringAndBool(row.Field<string>("name"), true));
                    }
                    dic.Add(db, list);
                }

                var i = 0;
                foreach (var item in dic[db])
                {
                    this.CLBTBS.Items.Add(item.Str);
                    this.CLBTBS.SetItemChecked(i, item.Boo);
                    i++;
                }
                this.CLBTBS.Tag = dic[db];
                CLBTBS.ItemCheck += CLBTBS_ItemCheck;
            }

        }

        private void CBData_CheckedChanged(object sender, EventArgs e)
        {
            this.PannelCopNumber.Enabled = CBData.Checked;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.OwnerCtl != null)
            {
                var pt = this.OwnerCtl.PointToScreen(this.OwnerCtl.Location);
                pt.Offset(this.OwnerCtl.Width / 2 - this.Width / 2, this.OwnerCtl.Height / 2 - this.Height / 2);
                this.Location = pt;
            }

            BtnStop.Enabled = false;
        }

        private Control OwnerCtl = null;
        public void ShowMe(Control owner)
        {
            this.OwnerCtl = owner;
            this.Show();
        }

        public DBSource DBSource { get; private set; }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (saveTask != null)
            {
                if (MessageBox.Show("取消任务吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cancel = true;
                }
                return;
            }

            if (this.Modal)
            {
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                this.Close();
            }
        }

        private void SendMsg(string msg)
        {
            msg = msg.Replace("\r", "").Replace("\n", "");
            this.BeginInvoke(new Action(() => this.MsgText.Text = msg.Length > 100 ? msg.Substring(0, 100) : msg));
        }

        private void PublishFinished(int total,int finished)
        {
            if (total > 0)
            {
                var rate = finished * 100 / total;
                BeginInvoke(new Action(() => ProcessBar.Value = rate));
            }
        }

        private async Task<int> Save(bool isTest, bool needIndex, bool needView, bool needProc, bool needFunc, bool needTrigger, int maxSize)
        {
            bool hasNotSupport = false;
            var dir = string.Empty;
            var filename = string.Empty;
            var dbdic = (Dictionary<string, List<StringAndBool>>)this.CLBDBs.Tag;
            var needCreateSql = true;
            if (currentTask == null)
            {
                dir = Application.StartupPath + "\\temp\\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                dir += $"exportdb{DateTime.Now.ToString("yyyyyMMddHHmmss")}";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                currentTask = new CopyDBTask
                {
                    Dir = dir
                };
                filename = Path.Combine(dir, "createdb.sql");
                foreach (string item in CLBDBs.CheckedItems)
                {
                    currentTask.TargetDBList.Add(item);
                }

                currentTask.TargetTBList.AddRange(dbdic.SelectMany(p => p.Value.Select(q => new CopyDBTask.CopyTB
                {
                    TB = q.Str,
                    DB = p.Key
                })));
            }
            else
            {
                dir = currentTask.Dir;
                filename = Path.Combine(dir, "createdb.sql");

                if (currentTask.CopyTBDataTasks.Count > 0 && dbdic.Any(p => !currentTask.TargetDBList.Contains(p.Key)||
                p.Value.Where(p2 => p2.Boo).Any(p3 => currentTask.CopyTBDataTasks.Any(q=>q.DB==p.Key)
                &&!currentTask.CopyTBDataTasks.Any(q => q.DB == p.Key && p3.Str == q.TB))))
                {
                    currentTask.TargetDBList = dbdic.Select(p => p.Key).ToList();
                    currentTask.TargetTBList = dbdic.SelectMany(p => p.Value.Select(q => new CopyDBTask.CopyTB
                    {
                        TB = q.Str,
                        DB = p.Key
                    })).ToList();

                    File.Delete(filename);
                }
                else
                {
                    if (File.Exists(filename))
                    {
                        needCreateSql = false;
                    }
                }
            }
            var datafilename= Path.Combine(dir, "createdb_###.data");

            try
            {
                var total = CLBDBs.CheckedItems.Count;
                var finished = 0;
                StringBuilder sball = new StringBuilder();
                StringBuilder sb = new StringBuilder();

                foreach (var item in CLBDBs.CheckedItems)
                {
                    try
                    {
                        if (cancel || stop)
                        {
                            break;
                        }

                        //创建库
                        var db = item.ToString();
                        var destdb = db;

                        if (isTest)
                        {
                            destdb += "-" + DateTime.Now.ToString("yyyyMMddHHmm");
                        }

                        sb.Clear();
                        sb.AppendLine("use [master]");
                        sb.AppendLine("GO");
                        sb.AppendLine("create database [" + destdb + "]");
                        sb.AppendLine("GO");
                        sb.AppendLine();
                        sb.AppendLine("use [" + db + "]");
                        sb.AppendLine("GO");

                        var needdata = CBData.Checked && NUDMaxNumber.Value > 0;
                        var tbs = SQLHelper.GetTBs(this.DBSource, db);
                        var foreignKeys = SQLHelper.GetForeignKeys(this.DBSource, db);
                        List<string> foreignTables = DataHelper.SortForeignKeys(foreignKeys);

                        var tbrows = tbs.AsEnumerable().ToList();
                        if (dbdic.ContainsKey(db))
                        {
                            tbrows = tbrows.AsEnumerable().Where(p => dbdic[db].Any(q => q.Boo && q.Str.Equals(p.Field<string>("name"), StringComparison.OrdinalIgnoreCase)))
                                .OrderBy(p =>
                                {
                                    for (var m = 0; m < foreignTables.Count; m++)
                                    {
                                        if (foreignTables[m].Equals(p.Field<string>("name"), StringComparison.OrdinalIgnoreCase))
                                        {
                                            return m;
                                        }
                                    }
                                    return int.MaxValue;
                                }).ToList();
                        }

                        var views = SQLHelper.GetViews(DBSource, db);
                        var proclist = SQLHelper.GetProcedures(DBSource, db).ToList();
                        var functionlist = SQLHelper.GetFunctions(DBSource, db);

                        //total += tbs.Rows.Count * (needdata ? 10 : 1) + views.Count + proclist.Count + functionlist.Rows.Count;

                        //创建新的架构
                        var schema = tbrows.Select(p => p.Field<string>("schema")).Distinct().ToList();
                        foreach (var s in schema.Where(p => !p.Equals("dbo", StringComparison.OrdinalIgnoreCase)))
                        {
                            sb.AppendLine($@"IF SCHEMA_ID('{s}') IS NULL 
    BEGIN
        EXEC('CREATE SCHEMA [{s}] AUTHORIZATION [dbo]')
    END");

                            sb.AppendLine("GO");
                        }

                        DataTable tableDesc = null;
                        //创建表
                        foreach (var tb in tbrows)
                        {
                            if (cancel || stop)
                            {
                                break;
                            }
                            var owner = tb["schema"].ToString();
                            var tbinfo = new TableInfo
                            {
                                DBName = db,
                                Schema = owner,
                                TBId = tb["id"].ToString(),
                                TBName = tb["name"].ToString()
                            };

                            var cols = SQLHelper.GetColumns(this.DBSource, tbinfo.DBName, tbinfo.TBId, tbinfo.TBName,owner).ToList();

                            if (needCreateSql)
                            {
                                var indexDDL = SQLHelper.GetIndexDDL(this.DBSource, tbinfo.DBName, tbinfo.TBName);
                                sb.AppendLine();
                                sb.AppendLine("--" + tbinfo.TBName);
                                if (tableDesc == null)
                                {
                                    tableDesc = SQLHelper.GetTableDescription(DBSource, db, string.Empty);
                                }
                                var colDesc = SQLHelper.GetTableColsDescription(DBSource, db, tbinfo.TBName);
                                sb.AppendLine(DataHelper.GetCreateTableSQL(tbinfo, cols, indexDDL, tableDesc, colDesc));
                                sb.AppendLine();
                                sb.AppendLine("GO");

                                //索引
                                if (needIndex)
                                {
                                    foreach (var idx in indexDDL.AsEnumerable().Where(p => !p.Field<bool>("is_primary_key")))
                                    {
                                        sb.AppendLine(idx.Field<string>("INDEX_DDL"));
                                        sb.AppendLine("GO");
                                    }
                                }

                                //finished++;
                                SendMsg("导出库" + db + ",表:" + tbinfo.TBName);
                            }

                            if (needdata)
                            {
                                //导出前100条语句
                                try
                                {
                                    int no = 0;
                                    foreach (var data in SQLHelper.ExportData2(cols, true, DBSource, tbinfo, (int)NUDMaxNumber.Value, () => cancel || stop, currentTask))
                                    {
                                        if (cancel)
                                        {
                                            break;
                                        }
                                        if (data != null && data.Rows.Count > 0)
                                        {
                                            if (isTest)
                                            {
                                                data.DBName = destdb;
                                            }
                                            var currDataFileName = datafilename.Replace("###", "[" + tbinfo.DBName + "].[" + tbinfo.TBName + "]_" + no);
                                            while (File.Exists(currDataFileName))
                                            {
                                                no++;
                                                currDataFileName = datafilename.Replace("###", "[" + tbinfo.DBName + "].[" + tbinfo.TBName + "]_" + no);
                                            }
                                            LJC.FrameWorkV3.EntityBuf.EntityBufCore.Serialize(data, currDataFileName);
                                            currentTask.CopyTBDataTasks.Add(new CopyDBTask.CopyTBDataTask
                                            {
                                                DB = tbinfo.DBName,
                                                TB = tbinfo.TBName,
                                                Key = data.Key,
                                                Size = data.Size,
                                                TotalCount = data.TotalCount
                                            });
                                            no++;
                                        }
                                        if (stop)
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    if (!CBIgnoreError.Checked)
                                    {
                                        throw;
                                    }
                                    else
                                    {
                                        SendMsg("导出库" + db + ",表前" + NUDMaxNumber.Value + "条语句:" + tbinfo.TBName + "导出数据出错");
                                    }
                                }
                                //finished += 9;
                                SendMsg("导出库" + db + ",表前" + NUDMaxNumber.Value + "条语句:" + tbinfo.TBName);
                            }
                        }

                        foreach (var tb in tbrows)
                        {
                            if (cancel || stop)
                            {
                                break;
                            }
                            var tbinfo = new TableInfo
                            {
                                DBName = db,
                                Schema = tb["schema"].ToString(),
                                TBId = tb["id"].ToString(),
                                TBName = tb["name"].ToString()
                            };


                            if (needCreateSql)
                            {
                                if (needTrigger)
                                {
                                    //触发器
                                    foreach (var tg in SQLHelper.GetTriggers(this.DBSource, tbinfo.DBName, tbinfo.TBName))
                                    {
                                        sb.AppendLine($"----------触发器:{tg.TriggerName}------------");
                                        sb.AppendLine(SQLHelper.GetTriggerBody(this.DBSource, tbinfo.DBName, tg.TriggerName));
                                        sb.AppendLine("Go");
                                    }
                                }

                                //finished++;
                                SendMsg("导出库" + db + ",表触发器:" + tbinfo.TBName);
                            }
                        }

                        if (needCreateSql)
                        {
                            var templist = new List<Tuple<string, string,int>>();

                            //视图
                            if (needView)
                            {
                                foreach (var v in views)
                                {
                                    if (cancel || stop)
                                    {
                                        break;
                                    }
                                    var vsql = SQLHelper.GetViewCreateSql(DBSource, db, v.Key);

                                    templist.Add(new Tuple<string, string, int>(v.Key, vsql, 1));
                                }
                            }

                            //存储过程
                            if (needProc)
                            {                       
                                foreach (var proc in proclist)
                                {
                                    if (cancel || stop)
                                    {
                                        break;
                                    }
                                    var body = SQLHelper.GetProcedureBody(DBSource, db, proc);
                                    templist.Add(new Tuple<string, string, int>(proc, body, 2));
                                }
                            }

                            //函数
                            if (needFunc)
                            {
                                foreach (var r in functionlist.AsEnumerable())
                                {
                                    if (cancel || stop)
                                    {
                                        break;
                                    }

                                    var name= r["name"].ToString();
                                    var body = SQLHelper.GetFunctionBody(DBSource, db, name);
                                    templist.Add(new Tuple<string, string,int>(name, body, 3));
                                }
                            }

                            var sortProceList = DataHelper.SortProcList(templist);
                            var sortPorceList2 = sortProceList.Where(p => p.Equals("V_TB_HxrOffer_Ext")).ToList();
                            foreach (var temp in templist.OrderBy(p =>
                            {
                                var m = 0;
                                for (m = 0; m < sortProceList.Count; m++)
                                {
                                    if (sortProceList[m].Equals(p.Item1, StringComparison.OrdinalIgnoreCase))
                                    {
                                        return m;
                                    }
                                }

                                return m;
                            }))
                            {
                                if(temp.Item3 == 1)
                                {
                                    sb.AppendLine($"----------view:{temp.Item1}------------");
                                    sb.AppendLine(temp.Item2);
                                    sb.AppendLine("GO");
                                    SendMsg("导出库" + db + ",视图:" + temp.Item1);
                                }
                                else if (temp.Item3==2)
                                {
                                    sb.AppendLine($"----------存储过程:{temp.Item1}------------");
                                    sb.AppendLine(temp.Item2);
                                    sb.AppendLine("GO");
                                    SendMsg("导出库" + db + ",存储过程:" + temp.Item1);
                                }
                                else if (temp.Item3 == 3)
                                {
                                    sb.AppendLine($"----------函数:{temp.Item1}------------");
                                    sb.AppendLine(temp.Item2);
                                    sb.AppendLine("GO");
                                    SendMsg("导出库" + db + ",函数:" + temp.Item1);
                                }
                                //finished++;
                            }



                            //用户自定义类型
                            var userTypeTb = SQLHelper.GetUserTypes(this.DBSource, db);
                            if (userTypeTb.Rows.Count > 0)
                            {
                                hasNotSupport = true;
                                foreach(DataRow r in userTypeTb.Rows)
                                {
                                    sb.AppendLine($"----------不支持的类型，用户自定义类型:{r.Field<string>("name")}------------");

                                    sb.AppendLine("GO");
                                }
                            }


                            if (isTest)
                            {
                                sb = new StringBuilder(Regex.Replace(sb.ToString(), $"use[\\s]+\\[?{db}\\]?", $"use [{destdb}]", RegexOptions.IgnoreCase | RegexOptions.Multiline));
                            }

                            sball.Append(sb);
                            sball.AppendLine("/*===库分隔线===*/");
                        }

                        finished++;

                        PublishFinished(total, finished);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (needCreateSql && sball.Length > 0)
                {
                    File.WriteAllText(filename, sball.ToString(), Encoding.UTF8);
                }

                if (finished == total)
                {
                    PublishFinished(100, 100);
                    SendMsg("保存成功:" + filename);
                    if (hasNotSupport)
                    {
                        MessageBox.Show("有用户自定义类型数据，请手动导出。");
                    }
                    System.Diagnostics.Process.Start(dir);

                    currentTask = null;
                }
                return await Task.FromResult(1);
            }
            catch (Exception ex)
            {
                SendMsg(ex.Message);
                return await Task.FromResult(0);
            }
            finally
            {
                if (cancel)
                {
                    if (MessageBox.Show("要删除备份目录吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                        }
                        catch
                        {
                            MessageBox.Show($"删除失败:{dir}，需要手工删除。");
                        }
                    }

                    this.BeginInvoke(new Action(() =>
                    {
                        this.saveTask = null;
                        this.BtnOk.Enabled = true;
                        BtnStop.Enabled = false;
                        BtnStop.Text = "暂停";
                        this.GBSelDBTB.Enabled = true;
                        this.groupBox2.Enabled = true;
                        this.currentTask = null;
                        cancel = false;
                    }));
                }
                else if (stop)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        BtnStop.Enabled = true;
                        this.GBSelDBTB.Enabled = true;
                        stop = false;
                    }));
                }
                else
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.saveTask = null;
                        this.BtnOk.Enabled = true;
                        BtnStop.Enabled = false;
                        BtnStop.Text = "暂停";
                        this.GBSelDBTB.Enabled = true;
                        this.groupBox2.Enabled = true;
                        this.currentTask = null;
                    }));
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (saveTask != null)
            {
                e.Cancel = true;
                Util.SendMsg(this.OwnerCtl ?? this, "正在处理中，不能关闭");
                return;
            }
            base.OnClosing(e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.BtnOk.Enabled = false;
            this.GBSelDBTB.Enabled = false;
            this.groupBox2.Enabled = false;
            this.BtnStop.Enabled = true;
            //this.BtnCancel.Enabled = false;
            PublishFinished(100, 0);
            saveTask = Task.Run(() => Save(CBTest.Checked, CBIndex.Checked, CBView.Checked, CBProc.Checked, CBFunc.Checked, CBTrigger.Checked, (int)NUDMaxSize.Value));

        }

        private void BtnSelectServer_Click(object sender, EventArgs e)
        {
            var connsqlserver = new ConnSQLServer();
            if (connsqlserver.ShowDialog() == DialogResult.OK)
            {
                this.DBSource = connsqlserver.DBSource;
                try
                {
                    this.CLBDBs.Items.Clear();
                    var dbs = SQLHelper.GetDBs(DBSource);
                    this.CLBDBs.Items.AddRange(dbs.AsEnumerable().Select(p => (object)p.Field<string>("name")).OrderBy(p => p.ToString()).ToArray());
                    for(var i = 0; i < this.CLBDBs.Items.Count; i++)
                    {
                        CLBDBs.SetItemChecked(i, true);
                    }
                    BtnSelAll.Text = "全消";
                    this.CLBDBs.Tag = new Dictionary<string, List<StringAndBool>>();
                }
                catch(Exception ex)
                {
                    Util.SendMsg(this.OwnerCtl ?? this, ex.Message);
                }
            }
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

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (BtnStop.Text == "暂停")
            {
                if (MessageBox.Show("要暂停吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BtnStop.Enabled = false;
                    stop = true;
                    BtnStop.Text = "继续";
                }
            }
            else if (BtnStop.Text == "继续")
            {
                if (MessageBox.Show("要继续吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BtnStop.Enabled = true;
                    stop = false;
                    saveTask = Task.Run(() => Save(CBTest.Checked, CBIndex.Checked, CBView.Checked, CBProc.Checked, CBFunc.Checked, CBTrigger.Checked, (int)NUDMaxSize.Value));
                    BtnStop.Text = "暂停";
                }
            }
        }
    }
}
