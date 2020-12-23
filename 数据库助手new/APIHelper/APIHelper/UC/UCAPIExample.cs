using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LJC.FrameWorkV3.Data.EntityDataBase;
using Entity;

namespace APIHelper.UC
{
    public partial class UCAPIExample : UserControl,ISaveAble
    {
        private int apiid = 0;
        private APIInvokeLog apiInvokeLog;
        public UCAPIExample()
        {
            InitializeComponent();
        }

        public UCAPIExample(int apiurlid,APIInvokeLog log)
        {
            InitializeComponent();

            this.apiid = apiurlid;
            this.apiInvokeLog = log;

            Bind();
        }

        public void Bind()
        {
            
            TBid.Visible = false;
            this.DGVList.MultiSelect = false;
            this.DGVList.AllowUserToAddRows = false;
            this.DGVList.DoubleClick += DGVList_DoubleClick; ;
            

            if (apiInvokeLog != null)
            {
                this.TBReq.Text = apiInvokeLog.GetRequestBody().ToString();
                this.TBResp.Text = apiInvokeLog.GetRespBody().ToString();
            }
        }

        private void DGVList_DoubleClick(object sender, EventArgs e)
        {
            var currrow = DGVList.CurrentRow;
            if (currrow != null)
            {
                var example = (APIDocExample)currrow.DataBoundItem;

                this.TBid.Text = example.Id.ToString();
                this.TBExamName.Text = example.Name;
                this.TBReq.Text = example.Req;
                this.TBResp.Text = example.Resp;
            }
        }

        private void BindData()
        {
            if (apiid > 0)
            {
                var examples = BigEntityTableEngine.LocalEngine.Find<APIDocExample>(nameof(APIDocExample), "ApiId", new object[] { apiid }).ToList();
                this.DGVList.DataSource = null;
                this.DGVList.DataSource = examples;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BindData();
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(TBExamName.Text))
            {
                Util.SendMsg(this, "示例场景不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(TBReq.Text) && string.IsNullOrWhiteSpace(TBResp.Text))
            {
                Util.SendMsg(this, "请求和返回不能都为空");
                return;
            }

            var id = int.Parse(TBid.Text);
            if (id == 0)
            {
                BigEntityTableEngine.LocalEngine.Insert(nameof(APIDocExample), new APIDocExample
                {
                    ApiId = apiid,
                    Name = TBExamName.Text.Trim(),
                    Req = TBReq.Text,
                    Resp = TBResp.Text
                });
                BindData();
                Util.SendMsg(this, "新增成功");
                
            }
            else
            {
                BigEntityTableEngine.LocalEngine.Update(nameof(APIDocExample), new APIDocExample
                {
                    Id=id,
                    ApiId = apiid,
                    Name = TBExamName.Text.Trim(),
                    Req = TBReq.Text,
                    Resp = TBResp.Text
                });
                BindData();
                Util.SendMsg(this, "修改成功");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            var id = int.Parse(TBid.Text);
            if (id > 0)
            {
                var delitem = (DGVList.DataSource as List<APIDocExample>).Find(p => p.Id == id);
                if (delitem != null&&MessageBox.Show("确定要删除"+delitem.Name,"提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    BigEntityTableEngine.LocalEngine.Delete<APIDocExample>(nameof(APIDocExample), delitem.Id);
                    BindData();
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.TBid.Text = "0";
            this.TBReq.Text = "";
            this.TBExamName.Text = "";
            this.TBResp.Text = "";
        }
    }
}
