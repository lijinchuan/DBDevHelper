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

namespace NETDBHelper.UC
{
    public partial class LogViewTab : TabPage
    {
        int pageSize = 20;

        public LogViewTab()
        {
            InitializeComponent();
            this.GVLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            PageIndex = 1;
            this.bindingNavigatorMoveLastItem.Click += BindingNavigatorMoveLastItem_Click;
            this.bindingNavigatorMoveNextItem.Click += BindingNavigatorMoveNextItem_Click;
            this.bindingNavigatorMoveFirstItem.Click += BindingNavigatorMoveFirstItem_Click;
            this.bindingNavigatorMovePreviousItem.Click += BindingNavigatorMovePreviousItem_Click;

            this.GVLog.ContextMenuStrip = new ContextMenuStrip();
            this.GVLog.ContextMenuStrip.Items.Add("复制");
            this.GVLog.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "复制")
            {
                var cells = GVLog.SelectedCells;
                if (cells.Count > 0)
                {
                    List<string> list = new List<string>();
                    foreach (DataGridViewCell cell in cells)
                    {
                        list.Add(cell.Value?.ToString());
                    }
                    Clipboard.SetText(string.Join("\t", list));
                }
            }
        }


        private void BindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (Total == 0)
            {
                PageIndex = 1;
            }
            else
            {
                if (PageIndex > 1)
                {
                    PageIndex -= 1;
                }
            }

            BindData();
        }

        private void BindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            PageIndex = 1;

            BindData();
        }

        private void BindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (Total == 0)
            {
                PageIndex = 1;
            }
            else
            {
                var lastpage = (int)Math.Ceiling(Total * 1.0 / pageSize);
                if (PageIndex < lastpage)
                {
                    PageIndex += 1;
                }
            }

            BindData();
        }

        private int PageIndex
        {
            get;
            set;
        }

        private int Total
        {
            get;
            set;
        }

        private void BindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            if (Total == 0)
            {
                PageIndex = 1;
            }
            else
            {
                var lastpage = (int)Math.Ceiling(Total * 1.0 / pageSize);
                PageIndex = lastpage;
            }

            BindData();

        }

        public void BindData()
        {

            long total = 0;
            var logs = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<HLogEntity>("HLog", "LogTime",
                new object[] { DateTime.MinValue }, new object[] { DateTime.Now }, PageIndex == 0 ? 1 : PageIndex, pageSize, ref total).Select(p=>new
                {
                    编号=p.ID,
                    日志时间=p.LogTime,
                    服务器=p.Sever,
                    库=p.DB,
                    对象类型 = p.LogType,
                    对象名称 =p.TypeName,
                    信息=p.Info
                }).ToList();
            this.Total = (int)total;
            this.GVLog.DataSource = logs;
            var totalpage = (int)Math.Ceiling(total * 1.0 / pageSize);
            this.bindingNavigatorCountItem.Text = totalpage.ToString();
            this.bindingNavigatorPositionItem.Text = PageIndex.ToString();

            this.bindingNavigatorMoveNextItem.Enabled = PageIndex < totalpage;
            this.bindingNavigatorMoveFirstItem.Enabled = PageIndex > 1;
            this.bindingNavigatorMoveLastItem.Enabled = PageIndex < totalpage;
            this.bindingNavigatorMovePreviousItem.Enabled = PageIndex > 1;
        }

    }
}
