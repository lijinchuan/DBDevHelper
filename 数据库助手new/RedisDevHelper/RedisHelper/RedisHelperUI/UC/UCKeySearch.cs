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
    public partial class UCKeySearch : UserControl
    {
        public RedisHelper.Model.RedisServerEntity RedisServer
        {
            get;
            set;
        }

        public UCKeySearch()
        {
            InitializeComponent();

            this.TCBSearchKey.TextChanged += TCBSearchKey_TextChanged;
        }

        void TCBSearchKey_TextChanged(object obj)
        {
            var key = (string)LJC.FrameWork.Comm.ReflectionHelper.Eval(obj, "key");

            if (RedisServer == null)
            {
                return;
            }
            DateTime time = DateTime.Now;
            RedisUtil.Execute(RedisServer.ConnStr, (client) =>
            {
                tabControl1.SelectedTab = TabPageData;
                TBMsg.Text = "";
                var keytype = client.KeyType(key);
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
                            dt.Rows.Add(key, str);
                            this.DGVData.DataSource = dt;

                            break;
                        }
                    case RedisType.Hash:
                        {
                            var hashs = client.HashGetAll(key);
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
                            var sets = client.SetMembers(key);
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
                            var list = client.ListRange(key, 0, 100);
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
                            var ssets = client.SortedSetRangeByRankWithScores(key, 0, 100);
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Element");
                            dt.Columns.Add("Score");
                            foreach (var set in ssets)
                            {
                                dt.Rows.Add(set.Element, set.Score);
                            }
                            this.DGVData.DataSource = dt;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
        }

        private void BtnSearchPatten_Click(object sender, EventArgs e)
        {

            var key = TCBSearchKey.Text.Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            if (RedisServer == null)
            {
                return;
            }
            TBMsg.Text = "";

            if (key != "*")
            {
                //key = string.Format("{0}{1}{2}", key.StartsWith("*") ? "" : "*", key, key.EndsWith("*") ? "" : "*");
                key = string.Format("{0}{1}{2}", "", key, key.EndsWith("*") ? "" : "*");
            }
            DateTime time = DateTime.Now;
            RedisUtil.SearchKey(RedisServer.ConnStr, key, (d) =>
                {
                    tabControl1.SelectedTab = TabPageData;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("key");
                    foreach (var s in d)
                    {
                        dt.Rows.Add(s);
                    }
                    TCBSearchKey.DataSource = dt.AsEnumerable().Select(p=>new{
                        key=p["key"]
                    }).ToList();

                    TBMsg.Text += string.Format("\r\n搜索用时{0}ms", DateTime.Now.Subtract(time).TotalMilliseconds);

                }, (ex) =>
                    {
                        this.TBMsg.Text = ex.ToString();
                        tabControl1.SelectedTab = TabPageInfo;
                    });
        }
    }
}
