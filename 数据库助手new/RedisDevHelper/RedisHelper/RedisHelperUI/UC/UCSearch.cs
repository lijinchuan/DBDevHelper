using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StackExchange.Redis;

namespace RedisHelperUI.UC
{
    public partial class UCSearch : UserControl
    {
        private RedisType RedisType
        {
            get;
            set;
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            RedisUtil.Execute(this.RedisServer.ConnStr, (db) =>
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
                            Del();
                            break;
                        }
                    case "修改":
                        {
                            RedisUpdate();
                            break;
                        }
                    case "增加":
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
            DateTime time = DateTime.Now;
            RedisUtil.Execute(RedisServer.ConnStr, (client) =>
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
                                var list = client.ListRange(key, 0, 100);
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
                                    var ssets = client.SortedSetRangeByRankWithScores(key, 0, 100);

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
    }
}
