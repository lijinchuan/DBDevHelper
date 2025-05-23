﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LJC.FrameWork.Data.EntityDataBase;
using LJC.FrameWork.Comm;
using LJC.FrameWork.MemCached;

namespace CouchBaseDevHelper.UI.UC
{
    public partial class DataViewUC : UserControl
    {
        private CouchBaseServerEntity _server = null;
        private object KeyVal = null;

        public DataViewUC()
        {
            InitializeComponent();
        }

        public DataViewUC(CouchBaseServerEntity server)
        {
            this._server = server;
            InitializeComponent();

            this.CBBucket.DataSource = server.Buckets;
            this.CBBucket.SelectedIndex = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        public string Key
        {
            get
            {
                return TBKey.Text;
            }
            set
            {
                TBKey.Text = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.TBMSG.ReadOnly = true;
            //this.TBMSG.Enabled = false;
        }

        private string GetValue(ArraySegment<byte> bytes)
        {
            //byte[] temp = new byte[bytes.Count];
            //Array.Copy(bytes.Array, bytes.Offset, temp, 0, bytes.Count);  
            return Encoding.UTF8.GetString(bytes.Array,bytes.Offset,bytes.Count);
        }

        private ICachClient GetClient()
        {
            var server = _server.ConnStr.Split(',')[0];
            var hostandport = server.Split(':');
            var host = hostandport[0];
            var point = hostandport.Length == 2 ? int.Parse(hostandport[1]) : 8091;
            var bucket = CBBucket.Text;

            LJC.FrameWork.MemCached.ICachClient client = null;
            if (_server.CachServerType == 1)
            {
                if (!string.IsNullOrWhiteSpace(_server.ClientFile))
                {
                    client = new LJC.FrameWork.MemCached.ExportMemcachClient(_server.ClientFile,host, point, bucket);
                }
                else
                {
                    client = new LJC.FrameWork.MemCached.MemcachedClient(host, point, bucket);
                }
            }
            else
                client = new LJC.FrameWork.MemCached.CouchbaseClient(host, point, bucket);

            return client;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var key = TBKey.Text.Trim();
            KeyVal = null;
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            DateTime now = DateTime.Now;
            try
            {
                var bucket = CBBucket.Text;
                var client = GetClient();

                if (!_server.Buckets.Contains(bucket))
                {
                    _server.Buckets.Add(bucket);
                    var item = EntityTableEngine.LocalEngine.Find<CouchBaseServerEntity>(Global.TBName_RedisServer, _server.ServerName).FirstOrDefault();
                    if (!item.Buckets.Contains(bucket))
                    {
                        item.Buckets.Add(bucket);
                        EntityTableEngine.LocalEngine.Update<CouchBaseServerEntity>(Global.TBName_RedisServer, item);
                    }
                    this.CBBucket.DataSource = _server.Buckets;
                    this.CBBucket.SelectedText = bucket;
                }
                
                object val = null;
                while (true)
                {
                    try
                    {
                        if (!EntityTableEngine.LocalEngine.Exists(Global.TBName_SearchLog,key))
                        {
                            EntityTableEngine.LocalEngine.Insert<SearchLog>(Global.TBName_SearchLog, new SearchLog
                            {
                                Key=key,
                                Mark=string.Empty,
                                ServerName=_server.ServerName,
                                Connstr=_server.ConnStr,
                                CachServerType=_server.CachServerType,
                                ClientFile=_server.ClientFile
                            });
                        }

                        if (!client.TryGet(key, out val))
                        {
                            TBMSG.Text = "键不存在";
                            this.tabControl1.SelectedTab = TPMsg;
                            break;
                        }
                        else
                        {
                            KeyVal = val;
                            if (val == null)
                            {
                                TBData.Text = "null";
                            }
                            else if (val is string)
                            {
                                var str = (string)val;
                                if ((str.StartsWith("{") && str.EndsWith("}"))
                                    ||(str.StartsWith("[") && str.EndsWith("]")))
                                {
                                    TBData.Text =JsonUtil<dynamic>.Serialize(JsonUtil<dynamic>.Deserialize(str),true);
                                }
                                else
                                {
                                    TBData.Text = str;
                                }
                            }
                            else
                            {
                                TBData.Text = LJC.FrameWork.Comm.JsonUtil<object>.Serialize(val,true);
                            }

                            this.tabControl1.SelectedTab = TPData;

                            break;
                        }
                    }
                    catch (System.Runtime.Serialization.SerializationException ex)
                    {
                        //无法找到程序集“EM.LVB.SNS.Lib.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null”
                        System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex("无法找到程序集“(.*)”");
                        var m = rg.Match(ex.Message);
                        if (m.Success)
                        {
                            var model = m.Groups[1].Value;

                            SumFormFindAssembly subform = new SumFormFindAssembly(model);
                            if (subform.ShowDialog() == DialogResult.Cancel)
                            {
                                throw ex;
                            }

                            //FileStream fs = new FileStream(subform.FilePath, FileMode.Open, FileAccess.Read);
                            //BinaryReader br = new BinaryReader(fs);
                            //byte[] bFile = br.ReadBytes((int)fs.Length);
                            //br.Close();
                            //fs.Close();

                            var localfile = AppDomain.CurrentDomain.BaseDirectory + new FileInfo(subform.FilePath).Name;
                            File.Copy(subform.FilePath,localfile);
                            System.Reflection.Assembly.LoadFrom(localfile);
                        }
                        else
                        {
                            throw ex;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                TBMSG.Text = "查询出错:" + ex.ToString();
                this.tabControl1.SelectedTab = TPMsg;
            }
            finally
            {
                TBMSG.Text += "\r\n用时:" + DateTime.Now.Subtract(now).TotalMilliseconds+"毫秒";
            }

        }

        private void TBData_TextChanged(object sender, EventArgs e)
        {

        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUpdate fu = new FormUpdate();
            fu.Server = _server;
            fu.Key = TBKey.Text.Trim();
            fu.Bucket = CBBucket.Text;
            fu.Val = KeyVal;
            fu.ShowDialog();
        }

        private void TBKey_TextChanged(object sender, EventArgs e)
        {
            this.KeyVal = null;
        }

        private void CBBucket_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.KeyVal = null;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key=TBKey.Text.Trim();
            string backkey = key + "_$delbak";
            var bucket = CBBucket.Text;
            if (MessageBox.Show("要删除:" + key + "吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            try
            {
                //先备份
                var client = GetClient();
                object oldval = null;
                if (client.TryGet(key, out oldval))
                {
                    if (!client.Store(StoreMode.Set, backkey, oldval))
                    {
                        MessageBox.Show("备份失败");
                        return;
                    }
                    LJC.FrameWork.LogManager.LogHelper.Instance.Info("备份：key=" + key + ",备份key:" + backkey + ",内容：" + JsonUtil<object>.Serialize(oldval));
                    if (client.Remove(key))
                    {
                        MessageBox.Show("删除成功");
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }
                else
                {
                    MessageBox.Show("键不存在");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败:"+ex.ToString());
            }
        }

        private void 删除bucketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bucket = CBBucket.SelectedText;
            if (!string.IsNullOrWhiteSpace(bucket))
            {
                var item = EntityTableEngine.LocalEngine.Find<CouchBaseServerEntity>(Global.TBName_RedisServer, _server.ServerName).FirstOrDefault();
                if (item.Buckets.Contains(bucket))
                {
                    item.Buckets.Remove(bucket);
                    EntityTableEngine.LocalEngine.Update<CouchBaseServerEntity>(Global.TBName_RedisServer, item);
                    this.CBBucket.DataSource = item.Buckets;
                }
            }
        }
    }
}
