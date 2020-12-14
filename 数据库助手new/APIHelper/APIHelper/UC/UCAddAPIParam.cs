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
    public partial class UCAddAPIParam : TabPage
    {

        private int _apisourceid = 0;
        private int _apiid = 0;

        private void Init()
        {
            List<APIParam> requstParams = new List<APIParam>();
            List<APIParam> respParams = new List<APIParam>();
            if (_apiid > 0)
            {
                var paramslist = BigEntityTableEngine.LocalEngine.Find<APIParam>(nameof(APIParam), "APIId", new object[] { _apiid }).ToList();

                requstParams = paramslist.Where(p => p.Type == 0).OrderBy(p => p.Sort).ToList();
                respParams = paramslist.Where(p => p.Type == 1).OrderBy(p => p.Sort).ToList();
            }

            if (requstParams.Count == 0)
            {
                requstParams.Add(new APIParam
                {
                    Sort = 1,
                    APIId = _apiid,
                    APISourceId = _apisourceid,

                });
            }

            if (respParams.Count == 0)
            {
                respParams.Add(new APIParam
                {
                    Sort = 1,
                    APIId = _apiid,
                    APISourceId = _apisourceid,

                });
            }

            TBRQParams.Init(0, requstParams, _apisourceid, _apiid);
            TBRESPParams.Init(1, respParams, _apisourceid, _apiid);
        }

        public UCAddAPIParam()
        {
            InitializeComponent();

            Init();
        }

        public UCAddAPIParam(int apisourceid, int apiid)
        {
            _apisourceid = apisourceid;
            _apiid = apiid;
            InitializeComponent();

            Init();
        }



        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this._apisourceid == 0)
            {
                Util.SendMsg(this, "不能保存，没有绑定API资源ID");
                return;
            }

            var requstParams = TBRQParams.RequstParams;
            var respParams = TBRESPParams.RequstParams;

            if (requstParams.GroupBy(p => p.Name).Any(p => p.Count() > 1))
            {
                Util.SendMsg(this, "不能保存，请求参数有重复字段");
                return;
            }

            if (requstParams.GroupBy(p => p.Sort).Any(p => p.Count() > 1))
            {
                Util.SendMsg(this, "不能保存,请求参数排序重复");
                return;
            }

            if (respParams.GroupBy(p => p.Name).Any(p => p.Count() > 1))
            {
                Util.SendMsg(this, "保存失败，应答参数有重复字段");
                return;
            }

            if (respParams.GroupBy(p => p.Sort).Any(p => p.Count() > 1))
            {
                Util.SendMsg(this, "保存失败，应答参数排序重复");
                return;
            }

            var paramslist = BigEntityTableEngine.LocalEngine.Find<APIParam>(nameof(APIParam), "APIId", new object[] { _apiid }).ToList();
            //删除
            foreach (var p in paramslist.Where(w => w.Type == 0))
            {
                if (!requstParams.Any(q => q.Id == p.Id))
                {
                    BigEntityTableEngine.LocalEngine.Delete<APIParam>(nameof(APIParam), p.Id);
                }
            }

            foreach (var p in paramslist.Where(w => w.Type == 1))
            {
                if (!respParams.Any(q => q.Id == p.Id))
                {
                    BigEntityTableEngine.LocalEngine.Delete<APIParam>(nameof(APIParam), p.Id);
                }
            }

            foreach (var item in requstParams)
            {
                if (item.Id == 0)
                {
                    BigEntityTableEngine.LocalEngine.Insert(nameof(APIParam), item);
                }
                else
                {
                    var pa = paramslist.Find(p => p.Id == item.Id);
                    if (!pa.Equals(item))
                    {
                        BigEntityTableEngine.LocalEngine.Update(nameof(APIParam), item);
                    }
                }
            }

            foreach (var item in respParams)
            {
                if (item.Id == 0)
                {
                    BigEntityTableEngine.LocalEngine.Insert(nameof(APIParam), item);
                }
                else
                {
                    var pa = paramslist.Find(p => p.Id == item.Id);
                    if (!pa.Equals(item))
                    {
                        BigEntityTableEngine.LocalEngine.Update(nameof(APIParam), item);
                    }
                }
            }
        }
    }
}
