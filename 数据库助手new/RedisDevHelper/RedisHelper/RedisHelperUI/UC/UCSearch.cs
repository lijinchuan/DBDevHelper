﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StackExchange.Redis;
using LJC.FrameWork.Data.EntityDataBase;

namespace RedisHelperUI.UC
{
    public partial class UCSearch : UserControl
    {
        private RedisType _redistype;
        private RedisType RedisType
        {
            get
            {
                return _redistype;
            }
            set
            {
                _redistype = value;
                switch (value)
                {
                    case StackExchange.Redis.RedisType.Hash:
                    case StackExchange.Redis.RedisType.List:
                    case StackExchange.Redis.RedisType.Set:
                    case StackExchange.Redis.RedisType.SortedSet:
                        {
                            this.统计条数ToolStripMenuItem.Enabled = true;
                            break;
                        }
                    default:
                        {
                            this.统计条数ToolStripMenuItem.Enabled = false;
                            break;
                        }
                }
                
            }
        }

        private string RedisKey
        {
            get;
            set;
        }

        public RedisHelper.Model.RedisServerEntity RedisServer
        {
            get;
            set;
        }

        public UCSearch()
        {
            InitializeComponent();

            List<KeyValuePair<int?, string>> list = new List<KeyValuePair<int?, string>>();
            list.Add(new KeyValuePair<int?, string>((int?)null,string.Empty));
            for (int i = 0; i < 100; i++)
            {
                list.Add(new KeyValuePair<int?, string>(i, $"db({i})"));
            }
            this.CBDefaultDB.DataSource = list;
            this.CBDefaultDB.ValueMember = "Key";
            this.CBDefaultDB.DisplayMember = "Value";
            
        }

        public void SetKey(string key)
        {
            this.TBSearchKey.Text = key;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Dock = DockStyle.Fill;

            this.DGVData.ContextMenuStrip = CMSOP;
            this.DGVData.DoubleClick += DGVData_DoubleClick;
            foreach (ToolStripMenuItem item in CMSOP.Items)
            {
                item.Click += item_Click;
            }

            this.CBDefaultDB.SelectedIndex = 0;
        }

        void DGVData_DoubleClick(object sender, EventArgs e)
        {
            var cell = DGVData.CurrentCell;
            if (cell == null)
            {
                return;
            }

            var content = cell.Value.ToString();
            TextView tv = new TextView();
            tv.Content = content;
            tv.ShowDialog();
        }

        private void Del()
        {
            switch (this.RedisType)
            {
                case RedisType.String:
                    {
                        if (MessageBox.Show("要删除 " + this.RedisKey + " 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr,(int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.KeyDelete(this.RedisKey))
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.Hash:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }
                        var field = (string)this.DGVData.CurrentRow.Cells["name"].Value;
                        if (MessageBox.Show("要删除 " + this.RedisKey + ":" + field + " 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.HashDelete(this.RedisKey, field))
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.List:
                    {

                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }
                        var field = (string)this.DGVData.CurrentRow.Cells["item"].Value;
                        if (MessageBox.Show("要删除 " + this.RedisKey + ":" + field + " 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.ListRemove(this.RedisKey, field) >= 0)
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.Set:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }
                        var field = (string)this.DGVData.CurrentRow.Cells["members"].Value;
                        if (MessageBox.Show("要删除 " + this.RedisKey + ":" + field + " 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.SetRemove(this.RedisKey, field))
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.SortedSet:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }
                        var field = (string)this.DGVData.CurrentRow.Cells["Element"].Value;
                        if (MessageBox.Show("要删除 " + this.RedisKey + ":" + field + " 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.SortedSetRemove(this.RedisKey, field))
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
            }
        }

        private void DelMul()
        {
            switch (this.RedisType)
            {
                case RedisType.String:
                    {
                        if (MessageBox.Show("要删除 " + this.RedisKey + " 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.KeyDelete(this.RedisKey))
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.Hash:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }

                        if (MessageBox.Show("要删除 " + this.RedisKey + ":选择的项吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            int delcount = 0;
                            int failcount = 0;
                            foreach (DataGridViewRow item in DGVData.SelectedRows)
                            {
                                var field = (string)item.Cells["name"].Value;
                                RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                                {
                                    if (db.HashDelete(this.RedisKey, field))
                                    {
                                        //MessageBox.Show("success");
                                        delcount++;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("fail");
                                        failcount++;
                                    }


                                }, (ex) =>
                                {
                                    MessageBox.Show(ex.Message);
                                });
                            }
                            MessageBox.Show("success:" + delcount + ",fail:" + failcount);
                        }
                        break;
                    }
                case RedisType.List:
                    {

                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }

                        if (MessageBox.Show("要删除 " + this.RedisKey + ":选定的项吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            int delcount = 0, failcount = 0;
                            foreach (DataGridViewRow row in DGVData.SelectedRows)
                            {
                                var field = (string)row.Cells["item"].Value;
                                RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                                {
                                    if (db.ListRemove(this.RedisKey, field) >= 0)
                                    {
                                        //MessageBox.Show("success");
                                        delcount++;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("fail");
                                        failcount++;
                                    }
                                }, (ex) =>
                                {
                                    MessageBox.Show(ex.Message);
                                });
                            }

                            MessageBox.Show("success:" + delcount + ",fail:" + failcount);
                        }
                        break;
                    }
                case RedisType.Set:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }

                        if (MessageBox.Show("要删除 " + this.RedisKey + ":选定的项吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            int delcount = 0, failcount = 0;
                            foreach (DataGridViewRow row in DGVData.SelectedRows)
                            {
                                var field = (string)row.Cells["members"].Value;
                                RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                                {
                                    if (db.SetRemove(this.RedisKey, field))
                                    {
                                        //MessageBox.Show("success");
                                        delcount++;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("fail");
                                        failcount++;
                                    }
                                }, (ex) =>
                                {
                                    MessageBox.Show(ex.Message);
                                });

                            }
                            MessageBox.Show("success:" + delcount + ",fail:" + failcount);
                        }
                        break;
                    }
                case RedisType.SortedSet:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }

                        if (MessageBox.Show("要删除 " + this.RedisKey + ":选定的项 吗？", "ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            int delcount = 0, failcount = 0;
                            foreach (DataGridViewRow row in DGVData.SelectedRows)
                            {
                                var field = (string)row.Cells["Element"].Value;
                                RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                                {
                                    if (db.SortedSetRemove(this.RedisKey, field))
                                    {
                                        //MessageBox.Show("success");
                                        delcount++;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("fail");
                                        failcount++;
                                    }
                                }, (ex) =>
                                {
                                    MessageBox.Show(ex.Message);
                                });
                            }
                            MessageBox.Show("success:" + delcount + ",fail:" + failcount);
                        }
                        break;
                    }
            }
        }

        private void RedisUpdate()
        {
            SubUpdateForm subform = null;
            switch (this.RedisType)
            {
                case RedisType.String:
                    {
                        subform = new SubUpdateForm();
                        subform.Key = this.RedisKey;
                        subform.OldValue = (string)DGVData.Rows[0].Cells[1].Value;
                        subform.IsNumber = false;
                        if (subform.ShowDialog()==DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                if (db.StringSet(this.RedisKey,subform.NewValue))
                                {
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.Hash:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }
                        subform = new SubUpdateForm();
                        
                        var field = (string)this.DGVData.CurrentRow.Cells["name"].Value;
                        subform.Key = this.RedisKey+":"+field;
                        subform.OldValue = this.DGVData.CurrentRow.Cells["value"].Value.ToString();
                        if (subform.ShowDialog() == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                db.HashSet(this.RedisKey, field, subform.NewValue);
                                MessageBox.Show("success");
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
                case RedisType.SortedSet:
                    {
                        if (this.DGVData.CurrentRow == null)
                        {
                            return;
                        }
                        subform = new SubUpdateForm();
                        var field = (string)this.DGVData.CurrentRow.Cells["Element"].Value;
                        subform.Key = this.RedisKey + ":" + field;
                        subform.OldValue = this.DGVData.CurrentRow.Cells["Score"].Value.ToString();
                        subform.IsNumber = true;
                        if (subform.ShowDialog() == DialogResult.Yes)
                        {
                            RedisUtil.Execute(this.RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (db) =>
                            {
                                db.SortedSetAdd(RedisKey, field, (double)subform.NewValue);

                                MessageBox.Show("success");
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                        }
                        break;
                    }
            }
        }

        private void Add()
        {
            SubInsertForm form = new SubInsertForm();
            form.DefaultDB = (int?)CBDefaultDB.SelectedValue;
            form.Key = this.RedisKey;
            form.RedisType = this.RedisType;
            form.RedisServer = this.RedisServer;

            if (form.ShowDialog() == DialogResult.Yes)
            {

            }

            
        }

        void item_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var item = (ToolStripMenuItem)sender;
                switch (item.Text)
                {
                    case "删除":
                        {
                            DelMul();
                            break;
                        }
                    case "修改":
                        {
                            RedisUpdate();
                            break;
                        }
                    case "增加项":
                        {
                            Add();
                            break;
                        }
                    case "复制":
                        {
                            if (DGVData.CurrentCell != null)
                            {
                               Clipboard.SetText(DGVData.CurrentCell.Value.ToString());
                               MessageBox.Show("已复制到粘贴板");
                            }
                            break;
                        }
                }
            }
        }

        private string GetSubKey
        {
            get
            {
                return TBSubKey.Text.Trim();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (RedisServer == null)
            {
                return;
            }
            var key=this.TBSearchKey.Text;
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            if (!EntityTableEngine.LocalEngine.Exists(Global.TBName_SearchLog, key))
            {
                EntityTableEngine.LocalEngine.Insert<SearchLog>(Global.TBName_SearchLog, new SearchLog
                {
                    Key = key,
                    Mark = string.Empty,
                    ServerName = this.RedisServer.ServerName,
                });
            }

            DateTime time = DateTime.Now;
            RedisUtil.Execute(RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (client) =>
                {
                    
                    tabControl1.SelectedTab = TabPageData;
                    TBMsg.Text = "";
                    var keytype = client.KeyType(key);
                    this.RedisType = keytype;
                    this.RedisKey = key;
                    switch (keytype)
                    {
                        case RedisType.Unknown:
                        case RedisType.None:
                            {
                                tabControl1.SelectedTab = TabPageInfo;
                                TBMsg.Text = string.Format("键 {0} 不存在", key);
                                break;
                            }
                        case RedisType.String:
                            {
                                var str = (string)client.StringGet(key);
                                DataTable dt = new DataTable();
                                
                                dt.Columns.Add("key");
                                dt.Columns.Add("value");
                                dt.Columns.Add("valuetype");
                                dt.Rows.Add(TBSearchKey.Text, str,"str");
                                this.DGVData.DataSource = dt;

                                break;
                            }
                        case RedisType.Hash:
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("name");
                                dt.Columns.Add("nametype");
                                dt.Columns.Add("value");
                                dt.Columns.Add("valuetype");
                                if (string.IsNullOrWhiteSpace(GetSubKey))
                                {
                                    var hashs = client.HashGetAll(key);
                                    
                                    foreach (var hash in hashs)
                                    {
                                        dt.Rows.Add(hash.Name,hash.Name.IsInteger?"int":"str", hash.Value,hash.Value.IsInteger?"int":"str");
                                    }
                                    
                                }
                                else
                                {
                                    var value = client.HashGet(key, GetSubKey);
                                    if (value.HasValue)
                                    {
                                        dt.Rows.Add(GetSubKey,"str", value,value.IsInteger?"int":"str");
                                    }
                                    else
                                    {
                                          RedisValue rv=RedisValue.Null;
                                          if (RedisUtil.TryParseNumber(GetSubKey, out rv))
                                          {
                                              
                                              dt.Rows.Add(GetSubKey,"int",value,value.IsInteger?"int":"str");
                                          }
                                    }
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                        case RedisType.Set:
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("members");
                                dt.Columns.Add("valuetype");

                                if (string.IsNullOrWhiteSpace(GetSubKey))
                                {
                                    var sets = client.SetMembers(key);

                                    foreach (var set in sets)
                                    {
                                        dt.Rows.Add(set,set.IsInteger?"int":"str");
                                    }
                                }
                                else
                                {
                                    var value = client.SetContains(key, GetSubKey);
                                    if (value)
                                    {
                                        dt.Rows.Add(GetSubKey,"str");
                                    }
                                    else
                                    {
                                        RedisValue rv=RedisValue.Null;
                                        if(RedisUtil.TryParseNumber(GetSubKey,out rv))
                                        {
                                            value = client.SetContains(key, GetSubKey);
                                            if (value)
                                            {
                                                dt.Rows.Add(GetSubKey,"int");
                                            }
                                        }
                                    }
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                        case RedisType.List:
                            {
                                var list = client.ListRange(key, 0, 2000);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("item");
                                dt.Columns.Add("valuetype");
                                foreach (var item in list)
                                {
                                    dt.Rows.Add(item,item.IsInteger?"int":"str");
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                        case RedisType.SortedSet:
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Element");
                                dt.Columns.Add("ElementType");
                                dt.Columns.Add("Score");

                                if (string.IsNullOrWhiteSpace(GetSubKey))
                                {
                                    //var ssets = client.SortedSetRangeByRankWithScores(key, 0, 100);
                                    var ssets = client.SortedSetRangeByRankWithScores(key, 0, 2000);

                                    foreach (var set in ssets)
                                    {
                                        dt.Rows.Add(set.Element, set.Element.IsInteger ? "int" : "str", set.Score);
                                    }
                                }
                                else
                                {
                                    var sset = client.SortedSetScore(key, GetSubKey);
                                    if (sset.HasValue)
                                    {
                                        dt.Rows.Add(GetSubKey, "str", sset.Value);
                                    }
                                    else
                                    {
                                         RedisValue rv=RedisValue.Null;
                                         if (RedisUtil.TryParseNumber(GetSubKey, out rv))
                                         {
                                             sset = client.SortedSetScore(key, rv);
                                             if (sset.HasValue)
                                             {
                                                 dt.Rows.Add(GetSubKey, "int", sset.Value);
                                             }
                                         }
                                    }
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                            
                    }

                    TBMsg.Text += string.Format("\r\n查询用时{0}ms",DateTime.Now.Subtract(time).TotalMilliseconds);
                }, (ex) =>
                {
                    tabControl1.SelectedTab = TabPageInfo;
                    TBMsg.Text = ex.ToString();
                    //TBMsg.Text = string.Format("查询用时{0}ms", DateTime.Now.Subtract(time).TotalMilliseconds);
                });
        }

        private void 统计条数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RedisServer == null)
            {
                return;
            }
            var key = this.TBSearchKey.Text;
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            if (!EntityTableEngine.LocalEngine.Exists(Global.TBName_SearchLog, key))
            {
                EntityTableEngine.LocalEngine.Insert<SearchLog>(Global.TBName_SearchLog, new SearchLog
                {
                    Key = key,
                    Mark = string.Empty,
                    ServerName = this.RedisServer.ServerName,
                });
            }

            DateTime time = DateTime.Now;
            RedisUtil.Execute(RedisServer.ConnStr, (int?)CBDefaultDB.SelectedValue, (client) =>
            {

                tabControl1.SelectedTab = TabPageData;
                TBMsg.Text = "";
                var keytype = client.KeyType(key);
                this.RedisType = keytype;
                this.RedisKey = key;
                switch (keytype)
                {
                    case RedisType.Unknown:
                    case RedisType.None:
                        {
                            break;
                        }
                    case RedisType.Hash:
                        {
                            MessageBox.Show("总条数" + client.HashLength(key));
                            break;
                        }
                    case RedisType.Set:
                        {
                            MessageBox.Show("总条数" + client.SetLength(key));
                            break;
                        }
                    case RedisType.List:
                        {
                            MessageBox.Show("总条数" + client.ListLength(key));
                            break;
                        }
                    case RedisType.SortedSet:
                        {
                            MessageBox.Show("总条数" + client.SortedSetLength(key));
                            break;
                        }

                }

                TBMsg.Text += string.Format("\r\n查询用时{0}ms", DateTime.Now.Subtract(time).TotalMilliseconds);
            }, (ex) =>
            {
                tabControl1.SelectedTab = TabPageInfo;
                TBMsg.Text = ex.ToString();
                //TBMsg.Text = string.Format("查询用时{0}ms", DateTime.Now.Subtract(time).TotalMilliseconds);
            });
        }

        private void 新增keyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewForm addform = new AddNewForm();
            addform.Defaultdb = (int?)CBDefaultDB.SelectedValue;
            addform.RediServer = this.RedisServer;
            if (addform.ShowDialog() == DialogResult.OK)
            {
                this.TBSearchKey.Text = addform.NewKey;
            }
        }
    }
}
