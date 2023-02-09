using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.UC
{
    public partial class UCSimulateResponse : UserControl
    {
        private static readonly string[] ResponseContentTypes = new[] { "文本","图片","文件"};

        public UCSimulateResponse()
        {
            InitializeComponent();

            CBContentType.DataSource = Biz.Common.WebUtil.ContentTypes;
            CBCharset.DataSource = Biz.Common.WebUtil.Charsets;
            CBResponseContentType.DropDownStyle = ComboBoxStyle.DropDownList;
            CBResponseContentType.DataSource = ResponseContentTypes;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
        }
    }
}
