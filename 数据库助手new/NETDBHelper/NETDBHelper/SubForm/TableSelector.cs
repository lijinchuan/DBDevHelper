﻿using Biz.Common.Data;
using Entity;
using LJC.FrameWorkV3.Comm;
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
    public partial class TableSelector : SubBaseDlg
    {
        private volatile Task<int> saveTask = null;
        private bool cancel = false;
        private bool stop = false;
        private CheckedListBox mainCLB = null;

        public Action<DBSource, TableInfo> Work = null;
        public Action OnFinished = null;

        public TableSelector()
        {
            InitializeComponent();
            this.CLBDBs.Items.Clear();

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
                BeginInvoke(new Action(() => ProcessBar.Value = rate)) ;
            }
        }

        private async Task<int> OK()
        {
            var dbdic = (Dictionary<string, List<StringAndBool>>)this.CLBDBs.Tag;

            try
            {
                SendMsg(string.Empty);
                ProcessTraceUtil.StartTrace();
                var total = CLBDBs.CheckedItems.Count;
                var finished = 0;
                
                for(var i=0;i<CLBDBs.CheckedItems.Count;i++)
                {
                    var item = CLBDBs.CheckedItems[i];
                     var db = item.ToString();
                    try
                    {
                        if (cancel || stop)
                        {
                            break;
                        }
                        //创建库
                        ProcessTraceUtil.Trace(db);
                        var tbs = SQLHelper.GetTBs(DBSource, db);
                        var tbrows = tbs.AsEnumerable().ToList();
                        if (dbdic.ContainsKey(db))
                        {
                            tbrows = tbrows.AsEnumerable().Where(p => dbdic[db].Any(q => q.Boo && q.Str.Equals(p.Field<string>("name"), StringComparison.OrdinalIgnoreCase))).ToList();
                        }

                        total = tbrows.Count* CLBDBs.CheckedItems.Count;
                        finished = i * tbrows.Count;

                        //创建表
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

                            //做事情
                            Work?.Invoke(DBSource, tbinfo);

                            finished++;

                            PublishFinished(total, finished);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("db", $"{db}");
                        LogHelper.Instance.Error(ex);
                        throw ex;
                    }
                }

                if (finished >= total)
                {
                    PublishFinished(100, 100);
                    OnFinished?.Invoke();
                }
                return await Task.FromResult(1);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex);
                SendMsg(ex.Message);
                return await Task.FromResult(0);
            }
            finally
            {
                LogHelper.Instance.Debug(ProcessTraceUtil.PrintTrace());
                if (cancel)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.saveTask = null;
                        this.BtnOk.Enabled = true;
                        BtnStop.Enabled = false;
                        BtnStop.Text = "暂停";
                        this.GBSelDBTB.Enabled = true;
                        this.groupBox2.Enabled = true;
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
            saveTask = Task.Run(() => OK());

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
                    saveTask = Task.Run(() => OK());
                    BtnStop.Text = "暂停";
                }
            }
        }
    }
}
