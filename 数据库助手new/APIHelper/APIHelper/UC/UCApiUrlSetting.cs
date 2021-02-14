using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace APIHelper.UC
{
    public partial class UCApiUrlSetting : UserControl
    {
        private int _apiid = 0;
        private ApiUrlSetting _apiUrlSetting = null;
        private ApiUrlSettingObj _apiUrlSettingObj = null;
        public UCApiUrlSetting()
        {
            InitializeComponent();
            this.AutoScroll = true;
            BtnSave.Visible = false;
            Init();
        }

        public UCApiUrlSetting(int apiid)
        {
            InitializeComponent();
            this.AutoScroll = true;
            _apiid = apiid;
            Init();
        }

        public bool NoPrxoy()
        {
            return _apiUrlSettingObj?.NoPrxoy == true;
        }

        public bool SaveResp()
        {
            return _apiUrlSettingObj?.SaveResp==true;
        }

        public int TimeOut()
        {
            return _apiUrlSettingObj?.TimeOut ?? 0;
        }

        private void Init()
        {
            if (_apiid > 0)
            {
                var setting = BigEntityTableEngine.LocalEngine.Find<ApiUrlSetting>(nameof(ApiUrlSetting), "ApiId", new object[] { _apiid }).FirstOrDefault();
                _apiUrlSetting = setting ?? new ApiUrlSetting() { ApiId = this._apiid };

                if (!string.IsNullOrWhiteSpace(_apiUrlSetting.SettingJson))
                {
                    _apiUrlSettingObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiUrlSettingObj>(_apiUrlSetting.SettingJson);
                }
                else
                {
                    _apiUrlSettingObj = new ApiUrlSettingObj();
                }

                this.TBTimeOut.Text = _apiUrlSettingObj.TimeOut.ToString();
                this.CBNoproxy.Checked = _apiUrlSettingObj.NoPrxoy;
                this.CBSaveResp.Checked = _apiUrlSettingObj.SaveResp;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newapiUrlSettingObj = new ApiUrlSettingObj
                {
                    NoPrxoy = CBNoproxy.Checked,
                    SaveResp = CBSaveResp.Checked,
                    TimeOut = int.Parse(TBTimeOut.Text)
                };
                _apiUrlSetting.SettingJson = Newtonsoft.Json.JsonConvert.SerializeObject(newapiUrlSettingObj);
                if (_apiUrlSetting.Id == 0)
                {
                    BigEntityTableEngine.LocalEngine.Insert(nameof(ApiUrlSetting), _apiUrlSetting);
                }
                else
                {
                    BigEntityTableEngine.LocalEngine.Update(nameof(ApiUrlSetting), _apiUrlSetting);
                }

                _apiUrlSettingObj = newapiUrlSettingObj;

            }
            catch (Exception ex)
            {
                _apiUrlSetting.SettingJson = Newtonsoft.Json.JsonConvert.SerializeObject(_apiUrlSettingObj);
                Util.SendMsg(this, ex.Message);
            }

        }
    }
}
