using LJC.FrameWork.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouchBaseDevHelper.UI
{
    public partial class FormAdd : Form
    {
        public CouchBaseServerEntity _server;

        public FormAdd()
        {
            InitializeComponent();
        }

        private void FormAdd_Load(object sender, EventArgs e)
        {
            if (_server == null)
            {
                this.Close();
                return;
            }

            this.CBBucket.DataSource = _server.Buckets;
        }

        private void Btnok_Click(object sender, EventArgs e)
        {
            string key = this.TBKey.Text.Trim();
            string bucket = this.CBBucket.Text;
            string val = this.TBVal.Text;
            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("key不能为空");
                return;
            }

            if (string.IsNullOrEmpty(bucket))
            {
                MessageBox.Show("bucket不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(val))
            {
                MessageBox.Show("值不能为空");
                return;
            }

            try
            {
                var server = _server.ConnStr.Split(',')[0];
                var hostandport = server.Split(':');
                var host = hostandport[0];
                var point = hostandport.Length == 2 ? int.Parse(hostandport[1]) : 8091;
                var client = LJC.FrameWork.Couchbase.CouchbaseHelper.GetClient(host,
                            point, bucket);

                if (client.Store(Enyim.Caching.Memcached.StoreMode.Add, key, val))
                {
                    EntityTableEngine.LocalEngine.Insert<SearchLog>(Global.TBName_SearchLog, new SearchLog
                    {
                        Key = key,
                        Mark = "添加",
                        ServerName = _server.ServerName,
                        Connstr = _server.ConnStr
                    });
                    MessageBox.Show("成功");
                }
                else
                {
                    MessageBox.Show("失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加失败" + ex.ToString());
            }
        }
    }
}
