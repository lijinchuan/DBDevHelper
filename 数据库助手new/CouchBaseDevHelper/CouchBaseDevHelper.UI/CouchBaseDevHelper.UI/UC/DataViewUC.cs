using System;
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
                var client = LJC.FrameWork.Couchbase.CouchbaseHelper.GetClient(_server.ConnStr.Split(':')[0],
                    int.Parse(_server.ConnStr.Split(':')[1]), CBBucket.Text);
                
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
                                Connstr=_server.ConnStr
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
                                if (str.StartsWith("{") && str.EndsWith("}"))
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
            fu.Connstr = _server.ConnStr.Split(',').First();
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
    }
}
