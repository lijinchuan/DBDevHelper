using Biz.WCF;
using Entity;
using Entity.WCF;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParamInfo = Entity.ParamInfo;

namespace APIHelper.SubForm
{
    public partial class AddWCFApiDlg : Form
    {
        private int _apiresourceid = 0;

        public APIUrl APIUrl
        {
            get;
            private set;
        }

        public AddWCFApiDlg()
        {
            InitializeComponent();
        }

        public AddWCFApiDlg(int apiresourceid)
        {
            InitializeComponent();
            _apiresourceid = apiresourceid;
        }

        public AddWCFApiDlg(int apiresourceid, APIUrl apiUrl)
        {
            InitializeComponent();
            _apiresourceid = apiresourceid;
            APIUrl = apiUrl;

            this.TBAPIName.Text = apiUrl.APIName;
            this.TBAPIName.ReadOnly = true;

            this.TBUrl.Text = APIUrl.Path;
            this.TBDesc.Text = APIUrl.Desc;

            this.Text = "编辑API接口";

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.LBInterfaceMethod.DoubleClick += LBInterfaceMethod_DoubleClick;

            if (this.APIUrl?.Id > 0)
            {
                this.LBInterfaceMethod.Enabled = false;
                BtnFindService_Click(null, null);
            }
        }

        private void LBInterfaceMethod_DoubleClick(object sender, EventArgs e)
        {
            InterfaceInfo wcfinterface = this.LBInterfaceMethod.SelectedItem as InterfaceInfo;
            if (wcfinterface != null)
            {
                this.TBAPIName.Text = wcfinterface.OperationName;
            }
        }

        private void BtnFindService_Click(object sender, EventArgs e)
        {
            var url = TBUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            try
            {
                new UC.LoadingBox().Waiting(this, new Action(() =>
                 {

                     List<InterfaceInfo> interfacelist = new List<InterfaceInfo>();

                     if (url.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase))
                     {
                         var client = Biz.WCF.WebSrvClient.CreateClient(url);
                         interfacelist = client.GetInterfaceInfos().SelectMany(p => p.Value).OrderBy(p => p.OperationName).ToList();
                     }
                     else
                     {
                         var client = Biz.WCF.WCFClient.CreateClient(url);
                         interfacelist = client.GetInterfaceInfos().SelectMany(p => p.Value).OrderBy(p => p.OperationName).ToList();
                     }

                     this.BeginInvoke(new Action(() =>
                     {
                         this.LBInterfaceMethod.DataSource = interfacelist;
                         this.LBInterfaceMethod.DisplayMember = "OperationName";

                         if (APIUrl?.Id > 0)
                         {
                             var apidata = BigEntityTableEngine.LocalEngine.Find<APIData>(nameof(APIData), "ApiId", new object[] { APIUrl.Id }).FirstOrDefault();
                             if (apidata != null)
                             {
                                 this.LBInterfaceMethod.SelectedItem = interfacelist.Find(p => p.SoapAction == apidata.Headers?.Find(q => q.Name == "SOAPAction")?.Value.Trim('"'));
                             }

                         }
                     }));
                 }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错", ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var apiname = TBAPIName.Text.Trim();
            var apiurl = TBUrl.Text.Trim();
            if (_apiresourceid == 0)
            {
                MessageBox.Show("不能添加");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiname))
            {
                MessageBox.Show("api名称不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiurl))
            {
                MessageBox.Show("api地址不能为空");
                return;
            }

            InterfaceInfo wcfinterface= this.LBInterfaceMethod.SelectedItem as InterfaceInfo;
            if (wcfinterface == null)
            {
                MessageBox.Show("没选择接口");
                return;
            }

            if (APIUrl == null)
            {
                var isexists = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), "SourceId_APIName", new object[] { _apiresourceid, apiname }).ToList().Any();
                if (isexists)
                {
                    MessageBox.Show("API名称不能重复");
                    return;
                }
                this.APIUrl = new APIUrl
                {
                    APIMethod = APIMethod.POST,
                    APIName = apiname,
                    BodyDataType=BodyDataType.wcf,
                    ApplicationType=ApplicationType.XML,
                    Path = apiurl,
                    SourceId = _apiresourceid,
                    Desc = TBDesc.Text.Trim()
                };
            }
            else
            {
                this.APIUrl.APIMethod = APIMethod.POST;
                this.APIUrl.APIName = apiname;
                this.APIUrl.BodyDataType = BodyDataType.wcf;
                this.APIUrl.ApplicationType = ApplicationType.XML;
                this.APIUrl.Path = apiurl;
                this.APIUrl.SourceId = _apiresourceid;
                this.APIUrl.Desc = TBDesc.Text.Trim();
            }

            if (this.APIUrl.Id == 0)
            {
                BigEntityTableEngine.LocalEngine.Insert<APIUrl>(nameof(APIUrl), this.APIUrl);

                Biz.WCF.Envelope envelope = new Biz.WCF.Envelope(wcfinterface.OperationName);
                foreach(var item in wcfinterface.InputParams)
                {
                    if (item.ChildParamInfos.Count == 0)
                    {
                        envelope.Body.Add(item.Name, new EnvelopeValue());
                    }
                    else
                    {
                        EnvelopeBody body = new Biz.WCF.EnvelopeBody();
                        foreach(var c in item.ChildParamInfos)
                        {
                            body.Add(c.Name, new EnvelopeValue());
                        }
                        envelope.Body.Add(item.Name, new EnvelopeValue(body));
                    }
                }

                APIData apidata = new APIData
                {
                    ApiId=APIUrl.Id,
                    Headers=new List<ParamInfo>
                    {
                        new ParamInfo
                        {
                            Checked=true,
                            Name="SOAPAction",
                            Value=$"\"{wcfinterface.SoapAction}\"",
                            Desc=""
                        },
                        new ParamInfo
                        {
                            Checked=true,
                            Name="Expect",
                            Value="100",
                            Desc=""
                        }
                    },
                    RawText=Biz.WCF.WCFClient.Serializer(envelope) 
                };

                BigEntityTableEngine.LocalEngine.Insert<APIData>(nameof(APIData), apidata);

            }
            else
            {
                BigEntityTableEngine.LocalEngine.Update<APIUrl>(nameof(APIUrl), this.APIUrl);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
