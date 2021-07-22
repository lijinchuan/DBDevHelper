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

namespace NETDBHelper.SubForm
{
    public partial class CopyDB : Form
    {
        private volatile Task<int> saveTask = null;
        private bool cancel = false;

        public CopyDB()
        {
            InitializeComponent();
            this.CLBDBs.Items.Clear();

            this.PannelCopNumber.Enabled = CBData.Checked;
            CBData.CheckedChanged += CBData_CheckedChanged;

            
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

        private async Task<int> Save(bool isTest,bool needIndex,bool needView,bool needProc,bool needFunc,bool needTrigger,int maxSize)
        {
            bool haserror = true;
            var dir = Application.StartupPath + "\\temp\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = Path.Combine(dir, "createdb_" + DateTime.Now.ToString("yyyyyMMddHHmmss") + ".sql");
            var datafilename= Path.Combine(dir, "createdb" + DateTime.Now.ToString("yyyyyMMddHHmmss") + "_data_###.sql");
            int datafilecount = 0;
            try
            {
                var total = CLBDBs.CheckedItems.Count;
                var finished = 0;
                StringBuilder sball = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                StringBuilder sbdata = new StringBuilder();
                
                foreach (var item in CLBDBs.CheckedItems)
                {
                    try
                    {
                        if (cancel)
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
                        var views = SQLHelper.GetViews(DBSource, db);
                        var proclist = SQLHelper.GetProcedures(DBSource, db).ToList();
                        var functionlist = SQLHelper.GetFunctions(DBSource, db);

                        //total += tbs.Rows.Count * (needdata ? 10 : 1) + views.Count + proclist.Count + functionlist.Rows.Count;

                        //创建新的架构
                        var schema=tbs.AsEnumerable().Select(p => p.Field<string>("schema")).Distinct().ToList();
                        foreach(var s in schema.Where(p => !p.Equals("dbo", StringComparison.OrdinalIgnoreCase)))
                        {
                            sb.AppendLine($@"IF SCHEMA_ID('{s}') IS NULL 
    BEGIN
        EXEC('CREATE SCHEMA [{s}] AUTHORIZATION [dbo]')
    END");

                            sb.AppendLine("GO");
                        }

                        //创建表
                        foreach (var tb in tbs.AsEnumerable())
                        {
                            if (cancel)
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

                            var cols = SQLHelper.GetColumns(this.DBSource, tbinfo.DBName, tbinfo.TBId, tbinfo.TBName).ToList();

                            var indexDDL = SQLHelper.GetIndexDDL(this.DBSource, tbinfo.DBName, tbinfo.TBName);
                            sb.AppendLine();
                            sb.AppendLine("--" + tbinfo.TBName);
                            sb.AppendLine(DataHelper.GetCreateTableSQL(tbinfo, cols, indexDDL));
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

                            if (CBData.Checked && NUDMaxNumber.Value > 0)
                            {
                                //导出前100条语句
                                try
                                {
                                    foreach(var s in SQLHelper.ExportData(cols, true, DBSource, tbinfo, (int)NUDMaxNumber.Value))
                                    {
                                        sbdata.AppendLine(s);
                                        sbdata.AppendLine("GO");

                                        if (maxSize > 0 && sbdata.Length > maxSize * 1024 * 1024)
                                        {
                                            if (isTest)
                                            {
                                                sbdata = new StringBuilder(Regex.Replace(sbdata.ToString(), $"use[\\s]+\\[?{db}\\]?", $"use [{destdb}]", RegexOptions.IgnoreCase | RegexOptions.Multiline));
                                            }
                                            var currDataFileName = datafilename.Replace("###", (datafilecount++).ToString());
                                            File.WriteAllText(currDataFileName, sbdata.ToString(), Encoding.UTF8);
                                            sbdata.Clear();
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
                                        sbdata.AppendLine("-----------导出数据出错-----------");
                                    }
                                }
                                //finished += 9;
                                SendMsg("导出库" + db + ",表前" + NUDMaxNumber.Value + "条语句:" + tbinfo.TBName);
                            }
                        }

                        foreach (var tb in tbs.AsEnumerable())
                        {
                            if (cancel)
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

                            

                            if (needTrigger)
                            {
                                //触发器
                                foreach (var tg in SQLHelper.GetTriggers(this.DBSource, tbinfo.DBName, tbinfo.TBName))
                                {
                                    sb.AppendLine(SQLHelper.GetTriggerBody(this.DBSource, tbinfo.DBName, tg.TriggerName));
                                    sb.AppendLine("Go");
                                }
                            }

                            //finished++;
                            SendMsg("导出库" + db + ",表触发器:" + tbinfo.TBName);
                        }

                        //视图
                        if (needView)
                        {
                            foreach (var v in views)
                            {
                                if (cancel)
                                {
                                    break;
                                }
                                var vsql = SQLHelper.GetViewCreateSql(DBSource, db, v.Key);

                                sb.AppendLine(vsql);
                                sb.AppendLine("GO");
                                SendMsg("导出库" + db + ",视图:" + v.Key);
                                //finished++;
                            }
                        }

                        //存储过程
                        if (needProc)
                        {
                            foreach (var proc in proclist)
                            {
                                if (cancel)
                                {
                                    break;
                                }
                                var body = SQLHelper.GetProcedureBody(DBSource, db, proc);
                                sb.AppendLine(body);
                                sb.AppendLine("GO");
                                SendMsg("导出库" + db + ",存储过程:" + proc);
                                //finished++;
                            }
                        }

                        //函数
                        if (needFunc)
                        {
                            foreach (var r in functionlist.AsEnumerable())
                            {
                                if (cancel)
                                {
                                    break;
                                }
                                var body = SQLHelper.GetFunctionBody(DBSource, db, r["name"].ToString());
                                sb.AppendLine(body);
                                sb.AppendLine("GO");
                                SendMsg("导出库" + db + ",函数:" + r["name"].ToString());
                                //finished++;
                            }
                        }

                        finished++;

                        PublishFinished(total, finished);

                        if (isTest)
                        {
                            sb =new StringBuilder(Regex.Replace(sb.ToString(), $"use[\\s]+\\[?{db}\\]?", $"use [{destdb}]",RegexOptions.IgnoreCase|RegexOptions.Multiline));
                        }

                        sball.Append(sb);
                        sball.AppendLine("/*===库分隔线===*/");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (!cancel && sball.Length > 0)
                {
                    File.WriteAllText(filename, sball.ToString(), Encoding.UTF8);
                }

                if (!cancel && sbdata.Length > 0)
                {
                    var currDataFileName = datafilename.Replace("###", (datafilecount++).ToString());
                    File.WriteAllText(currDataFileName, sbdata.ToString(), Encoding.UTF8);
                }

                SendMsg("保存成功:" + filename);

                System.Diagnostics.Process.Start(dir);

                haserror = false;
                return await Task.FromResult(1);
            }
            catch (Exception ex)
            {
                SendMsg(ex.Message);

                return await Task.FromResult(0);
            }
            finally
            {
                PublishFinished(100, 100);
                this.BeginInvoke(new Action(()=>{
                    this.saveTask = null;
                    this.BtnOk.Enabled = true;
                    this.groupBox1.Enabled = true;
                    this.groupBox2.Enabled = true;
                    if (!haserror)
                    {
                        if (this.Modal)
                        {
                            //Biz.Common.Data.SQLHelper.

                            this.DialogResult = DialogResult.OK;

                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }));

                
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
            this.groupBox1.Enabled = false;
            this.groupBox2.Enabled = false;
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
                    var dbs = Biz.Common.Data.SQLHelper.GetDBs(DBSource);
                    this.CLBDBs.Items.AddRange(dbs.AsEnumerable().Select(p => (object)p.Field<string>("name")).OrderBy(p => p.ToString()).ToArray());
                }
                catch(Exception ex)
                {
                    Util.SendMsg(this.OwnerCtl ?? this, ex.Message);
                }
            }
        }

        private void BtnSelAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < this.CLBDBs.Items.Count; i++)
            {
                this.CLBDBs.SetItemChecked(i, true);
            }
        }
    }
}
