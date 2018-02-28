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
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (RedisServer == null)
            {
                return;
            }
            DateTime time = DateTime.Now;
            RedisUtil.Execute(RedisServer.ConnStr, (client) =>
                {
                    tabControl1.SelectedTab = TabPageData;
                    TBMsg.Text = "";
                    var keytype = client.KeyType(this.TBSearchKey.Text);
                    switch (keytype)
                    {
                        case RedisType.Unknown:
                        case RedisType.None:
                            {
                                tabControl1.SelectedTab = TabPageInfo;
                                TBMsg.Text = string.Format("键 {0} 不存在", TBSearchKey.Text);
                                break;
                            }
                        case RedisType.String:
                            {
                                var str = (string)client.StringGet(this.TBSearchKey.Text);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("key");
                                dt.Columns.Add("value");
                                dt.Rows.Add(TBSearchKey.Text, str);
                                this.DGVData.DataSource = dt;

                                break;
                            }
                        case RedisType.Hash:
                            {
                                var hashs = client.HashGetAll(TBSearchKey.Text);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("name");
                                dt.Columns.Add("value");
                                foreach (var hash in hashs)
                                {
                                    dt.Rows.Add(hash.Name, hash.Value);
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                        case RedisType.Set:
                            {
                                var sets = client.SetMembers(TBSearchKey.Text);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("members");
                                foreach (var set in sets)
                                {
                                    dt.Rows.Add(set);
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                        case RedisType.List:
                            {
                                var list = client.ListRange(TBSearchKey.Text, 0, 100);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("item");
                                foreach (var item in list)
                                {
                                    dt.Rows.Add(item);
                                }
                                this.DGVData.DataSource = dt;
                                break;
                            }
                        case RedisType.SortedSet:
                            {
                                var ssets=client.SortedSetRangeByRankWithScores(TBSearchKey.Text, 0, 100);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Element");
                                dt.Columns.Add("Score");
                                foreach (var set in ssets)
                                {
                                    dt.Rows.Add(set.Element,set.Score);
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
