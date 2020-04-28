using LJC.FrameWork.Comm;
using LJC.FrameWork.MemCached;
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
    public partial class FormUpdate : Form
    {
        public CouchBaseServerEntity Server
        {
            get;
            set;
        }

        public string Bucket
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public object Val
        {
            get;
            set;
        }

        public FormUpdate()
        {
            InitializeComponent();
        }

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                this.Close();
                return;
            }

            if (Val == null)
            {
                this.Close();
                return;
            }

            if (string.IsNullOrWhiteSpace(Server.ConnStr))
            {
                this.Close();
                return;
            }

            if (string.IsNullOrWhiteSpace(Bucket))
            {
                this.Close();
                return;
            }

            this.Text = "修改->" + Key;
            this.TBKey.Text = Key;

            if (Val is string)
            {
                var valstr = (string)Val;
                if ((valstr.StartsWith("{") && valstr.EndsWith("}"))
                                     || (valstr.StartsWith("[") && valstr.EndsWith("]")))
                {
                    this.TBValContent.Text = JsonUtil<dynamic>.Serialize(JsonUtil<dynamic>.Deserialize(valstr), true);
                }
                else
                {
                    this.TBValContent.Text = valstr;
                }

            }
            else
            {
                this.TBValContent.Text = LJC.FrameWork.Comm.JsonUtil<object>.Serialize(Val, true);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.TBKey.Text))
                {
                    throw new Exception("键不能为空");
                }

                object obj = null;
                if (Val is string)
                {
                    var str = (string)Val;
                    if (str.StartsWith("{") && str.EndsWith("}"))
                    {
                        obj =JsonUtil<dynamic>.Serialize(JsonUtil<dynamic>.Deserialize(TBValContent.Text));
                    }
                    else
                    {
                        obj = TBValContent.Text;
                    }
                }
                else
                {
                    obj = LJC.FrameWork.Comm.JsonUtil<object>.Deserialize(TBValContent.Text, Val.GetType());
                }
                var connstr = Server.ConnStr.Split(',').First();
                var host = connstr.Split(':')[0];
                var point = int.Parse(connstr.Split(':')[1]);
                var bucket = Bucket;
                LJC.FrameWork.MemCached.ICachClient client = null;
                if (Server.CachServerType == 1)
                {
                    if (!string.IsNullOrWhiteSpace(Server.ClientFile))
                    {
                        client = new LJC.FrameWork.MemCached.ExportMemcachClient(Server.ClientFile, host, point, bucket);
                    }
                    else
                    {
                        client = new LJC.FrameWork.MemCached.MemcachedClient(host, point, bucket);
                    }
                }
                else
                    client = new LJC.FrameWork.MemCached.CouchbaseClient(host, point, bucket);

                if (this.TBKey.Text != Key)
                {
                    if (client.KeyExists(this.TBKey.Text))
                    {
                        throw new Exception("新的键已经存在，不能修改。");
                    }
                }
                else
                {
                    LJC.FrameWork.LogManager.LogHelper.Instance.Info("修改备份,key=" + Key + "类型：" + Val.GetType().FullName + ",原值:" + JsonUtil<object>.Serialize(Val));
                }

                if(client.Store(StoreMode.Set, this.TBKey.Text, obj))
                {
                    MessageBox.Show("存储成功");
                }
                else
                {
                    MessageBox.Show("存储失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "修改出错", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
        }
    }
}
