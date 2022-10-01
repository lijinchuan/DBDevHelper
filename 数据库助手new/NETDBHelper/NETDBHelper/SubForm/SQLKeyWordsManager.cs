using Biz.Common;
using Entity;
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

namespace NETDBHelper.SubForm
{
    public partial class SQLKeyWordsManager : SubBaseDlg
    {
        private Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Pink,Color.DeepPink, Color.Gray, Color.Green };

        public SQLKeyWordsManager()
        {
            InitializeComponent();

            this.GVKeyWordList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            this.GVKeyWordList.ContextMenuStrip = new ContextMenuStrip();
            this.GVKeyWordList.ContextMenuStrip.Items.Add("删除");
            this.GVKeyWordList.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
            this.GVKeyWordList.CellDoubleClick += GVLog_CellDoubleClick;
            this.GVKeyWordList.BorderStyle = BorderStyle.None;
            this.GVKeyWordList.GridColor = Color.LightBlue;
            GVKeyWordList.BackgroundColor = Color.White;
            GVKeyWordList.RowHeadersVisible = false;

            this.GVKeyWordList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.GVKeyWordList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.GVKeyWordList.AllowUserToResizeRows = true;

            CBKeyType.SelectedIndexChanged += CBKeyType_SelectedIndexChanged;

            LBHidId.Text = "0";
        }

        private void CBKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = (SqlKeyWordType)CBKeyType.SelectedItem;
            CBColor.SelectedItem = SQLKeyWordHelper.GetColorByType(type);
        }

        private void GVLog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = GVKeyWordList["关键字", e.RowIndex];
            var currentKeyWord = BigEntityTableEngine.LocalEngine.Find<SqlKeyword>(nameof(SqlKeyword), nameof(SqlKeyword.KeyWord), new[] { cell.Value }).FirstOrDefault();
            if (currentKeyWord != null)
            {
                TbKeyWord.Text = currentKeyWord.KeyWord;
                CBKeyType.SelectedItem = currentKeyWord.SqlKeyWordType;
                CBColor.SelectedItem = Color.FromName(currentKeyWord.HighColor);
                TBDesc.Text = currentKeyWord.Desc;
                LBHidId.Text = currentKeyWord.ID.ToString();
                BtnAdd.Text = "修改";
            }
        }

        private void Bind()
        {
            var list = BigEntityTableEngine.LocalEngine.List<SqlKeyword>(nameof(SqlKeyword), 1, int.MaxValue)
                .Select(p => new
                {
                    关键字 = p.KeyWord,
                    关键字类型 = p.SqlKeyWordType,
                    说明 = p.Desc
                }).ToList();
            if (LBNavList.SelectedItem != null && LBNavList.SelectedItem.ToString() != "ALL")
            {
                list = list.Where(p => p.关键字.StartsWith(LBNavList.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
            }
            GVKeyWordList.DataSource = list;
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var cells = GVKeyWordList.SelectedCells;
            if (cells.Count == 0)
            {
                return;
            }
            var itemText = e.ClickedItem.Text;
            this.GVKeyWordList.ContextMenuStrip.Hide();
            if (itemText == "删除")
            {
                
                var delCell = cells[0];
                var keyword = GVKeyWordList["关键字", delCell.RowIndex].Value;
                var currentKeyWord = BigEntityTableEngine.LocalEngine.Find<SqlKeyword>(nameof(SqlKeyword), nameof(SqlKeyword.KeyWord), new[] { keyword }).FirstOrDefault();
                if (currentKeyWord != null)
                {
                    if (currentKeyWord.IsProtect)
                    {
                        MessageBox.Show("不能删除关键字");
                        return;
                    }
                    if (MessageBox.Show("删除【"+currentKeyWord.KeyWord+"】？", "删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BigEntityTableEngine.LocalEngine.Delete<SqlKeyword>(nameof(SqlKeyword), currentKeyWord.ID);
                        Bind();
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //设置手动绘制
            CBColor.DrawMode = DrawMode.OwnerDrawFixed;
            CBColor.DropDownStyle = ComboBoxStyle.DropDownList;
            CBColor.DrawItem += CBColor_DrawItem;

            CBColor.DataSource = colors;

            //选择第一个
            CBColor.SelectedIndex = 0;
            LBNavList.Items.Add(string.Empty);
            LBNavList.Items.Add("@");
            for(var i = 'A'; i <= 'Z'; i++)
            {
                LBNavList.Items.Add(i);
            }
            LBNavList.SelectedIndexChanged += LBNavList_SelectedIndexChanged;


            CBKeyType.DataSource = new List<SqlKeyWordType> { SqlKeyWordType.DataType,
            SqlKeyWordType.Function,
            SqlKeyWordType.GlobVar,
            SqlKeyWordType.KeyWord,
            SqlKeyWordType.Other};

            Bind();
        }

        private void LBNavList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        private string ColorToChineseString(Color color)
        {
            string str = string.Empty;

            if (color == Color.Red)
            {
                str = "红色";
            }
            else if (color == Color.Green)
            {
                str = "绿色";
            }
            else if (color == Color.Blue)
            {
                str = "蓝色";
            }
            else
            {
                str = color.Name;
            }

            return str;
        }

        private void CBColor_DrawItem(object sender, DrawItemEventArgs e)
        {

            //选中项
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index < 0)
                return;

            ComboBox cmb = (ComboBox)sender;

            Color color = (Color)cmb.Items[e.Index];
            //画笔
            SolidBrush brush = new SolidBrush(color);

            Graphics g = e.Graphics;

            Rectangle rect = e.Bounds;

            rect.Inflate(-2, -2);

            Rectangle rectColor = new Rectangle(rect.Location, new Size(20, rect.Height));
            //绘制边框
            g.DrawRectangle(new Pen(e.ForeColor), rectColor);
            //填充颜色
            g.FillRectangle(brush, rectColor);
            //绘制文本
            g.DrawString(ColorToChineseString(color), new Font("宋体", 9), new SolidBrush(e.ForeColor), (rect.X + 22), rect.Y);


        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbKeyWord.Text))
            {
                MessageBox.Show("关键字不能为空");
                return;
            }
            var id = int.Parse(LBHidId.Text);

            var keyword = TbKeyWord.Text.Trim();
            var item = new SqlKeyword
            {
                Desc = TBDesc.Text,
                HighColor = ((Color)CBColor.SelectedItem).Name,
                IsProtect = false,
                KeyWord = keyword,
                SqlKeyWordType = (SqlKeyWordType)CBKeyType.SelectedItem
            };

            var currentKeyWord = BigEntityTableEngine.LocalEngine.Find<SqlKeyword>(nameof(SqlKeyword),id);
            if (currentKeyWord != null)
            {
                
                item.ID = currentKeyWord.ID;
                if (currentKeyWord.IsProtect)
                {
                    if (currentKeyWord.KeyWord != item.KeyWord)
                    {
                        MessageBox.Show("不能修改名称");
                        return;
                    }
                    item.IsProtect = currentKeyWord.IsProtect;
                    item.SqlKeyWordType = currentKeyWord.SqlKeyWordType;
                }
                BigEntityTableEngine.LocalEngine.Update(nameof(SqlKeyword),item);
            }
            else
            {
                BigEntityTableEngine.LocalEngine.Insert(nameof(SqlKeyword), item);
            }
            Bind();
            Reset();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbKeyWord.Text))
            {
                MessageBox.Show("关键字不能为空");
                return;
            }
            var keyword = TbKeyWord.Text.Trim();
            GVKeyWordList.DataSource = BigEntityTableEngine.LocalEngine.Find<SqlKeyword>(nameof(SqlKeyword), nameof(SqlKeyword.KeyWord), new[] { keyword }).Select(p => new
            {
                关键字 = p.KeyWord,
                关键字类型 = p.SqlKeyWordType,
                说明 = p.Desc
            }).ToList();
        }

        private void Reset()
        {
            TBDesc.Text = string.Empty;
            TbKeyWord.Text = string.Empty;
            CBKeyType.SelectedItem = SqlKeyWordType.KeyWord;
            BtnAdd.Text = "添加";
            LBHidId.Text = "0";

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
