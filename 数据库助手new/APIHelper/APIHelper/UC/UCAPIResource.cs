using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.UC
{
    public partial class UCAPIResource : UserControl
    {
        private int _resourceId = 0;

        public UCAPIResource()
        {
            InitializeComponent();

            PBButton.Image = Resources.Resource1.add;
            PBButton.Width = Resources.Resource1.add.Width;
            PBButton.Height = Resources.Resource1.add.Height;
            Resize += UCAPIResource_Resize;
            TBFilePath.ReadOnly = true;
            PBButton.Visible = false;
        }

        private void UCAPIResource_Resize(object sender, EventArgs e)
        {
            PBButton.Location = new Point((Width - Resources.Resource1.add.Width) / 2, PBButton.Location.Y);
        }

        public int ResourceId
        {
            get
            {
                return _resourceId;
            }
            set
            {
                if (value == 0)
                {
                    _resourceId = 0;
                    TBFilePath.Text = string.Empty;
                }
                else if (value > 0 && value != _resourceId)
                {
                    _resourceId = value;
                    var resource = BigEntityTableEngine.LocalEngine.Find<APIResource>(nameof(APIResource), _resourceId);
                    if (resource != null)
                    {
                        TBFilePath.Text = resource.FileName;
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileName == TBFilePath.Text)
                {
                    return;
                }
                //if (new FileInfo(dlg.FileName).Length > 9 * 1024 * 1024)
                //{
                //    LBMsg.Text = "文件最大9M";
                //    return;
                //}
                LBMsg.Text = "文件未保存";
                TBFilePath.Text = dlg.FileName;
                PBButton.Visible = true;
            }
        }

        private void PBButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(TBFilePath.Text))
            {
                return;
            }

            byte[] bytes=new byte[0];
            string md5 = string.Empty;
            if (new FileInfo(TBFilePath.Text).Length <= 9 * 1024 * 1024)
            {
                bytes = File.ReadAllBytes(TBFilePath.Text);
                md5 = LJC.FrameWorkV3.Comm.HashEncrypt.MD5_JS(bytes);
            }
            else
            {
                StringBuilder sb=new StringBuilder();
                var readBuffer = new byte[1024000];
                var readLen = 0;
                using (var fs = new FileStream(TBFilePath.Text, FileMode.Open, FileAccess.Read))
                {
                    while (true)
                    {
                        readLen = fs.Read(readBuffer, 0, readBuffer.Length);
                        if (readLen == 0)
                        {
                            break;
                        }
                        var isend = readLen < readBuffer.Length;
                        if (isend)
                        {
                            readBuffer = readBuffer.Take(readLen).ToArray();
                        }
                        var tempMd5 = LJC.FrameWorkV3.Comm.HashEncrypt.MD5_JS(readBuffer);
                        sb.Append(tempMd5);
                        if (isend)
                        {
                            break;
                        }
                    }
                    md5 = LJC.FrameWorkV3.Comm.HashEncrypt.MD5_JS(Encoding.UTF8.GetBytes(sb.ToString()));
                }
            }

            var resource = BigEntityTableEngine.LocalEngine.Find<APIResource>(nameof(APIResource), nameof(APIResource.MD5), new object[] { md5 }).FirstOrDefault();

            if (resource == null)
            {
                resource = new APIResource
                {
                    FileName = TBFilePath.Text,
                    MD5 = md5,
                    ResourceData = bytes
                };

                BigEntityTableEngine.LocalEngine.Insert(nameof(APIResource), resource);
                _resourceId = resource.Id;
            }
            else
            {
                _resourceId = resource.Id;
            }

            LBMsg.Text = "文件保存成功";
            PBButton.Visible = false;
        }
    }
}
