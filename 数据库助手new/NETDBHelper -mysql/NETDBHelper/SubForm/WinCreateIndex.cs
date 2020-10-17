using Entity;
using NETDBHelper.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class WinCreateIndex : Form
    {
        private List<TBColumn> TBColumnList = new List<TBColumn>();
        public List<IndexTBColumn> IndexColumns = new List<IndexTBColumn>();
        private ImageList imageList = new ImageList();

        public WinCreateIndex()
        {
            InitializeComponent();
        }

        public WinCreateIndex(List<TBColumn> columns)
        {
            InitializeComponent();

            this.TBColumnList = columns;

            LbxColumn.Font = new Font("宋体", 12);
            LbxColumn.DrawMode = DrawMode.OwnerDrawVariable;
            LbxColumn.MeasureItem += LbxColumn_MeasureItem;
            LbxColumn.DrawItem += LbxColumn_DrawItem;
            imageList.Images.Add(Resources.Resource1.ASC);
            imageList.Images.Add(Resources.Resource1.DESC);
            imageList.Images.Add(Resources.Resource1.plugin);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitCheckBoxGroup(this.panItmes);
        }

        public string IndexName
        {
            get
            {
                return TBIndexName.Text.Trim();
            }

        }

        private string GetIndexName()
        {
            if (IndexColumns.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var idx in this.IndexColumns)
            {
                if (!idx.Include)
                {
                    sb.AppendFormat("_{0}_{1}", idx.Name, idx.Order == -1 ? 2 : idx.Order);
                }
            }
            if (sb.Length == 0)
            {
                return string.Empty;
            }
            return "ix_" + sb.Remove(0, 1).ToString();
        }

        public bool IsUnique()
        {
            return CBUnique.Checked;
        }

        public bool IsPrimaryKey()
        {
            return CBKey.Checked;
        }

        public bool IsAutoIncr()
        {
            return CB_AutoIncr.Checked;
        }

        public bool IsClustered()
        {
            return CBClustered.Checked;
        }

        private void InitCheckBoxGroup(Control ctlContainer)
        {

            int linewidth = 20;
            int lineIndex = 0;
            int margintop = 5;
            int marginright = 10;
            var items=new[] { "不选", "顺序", "倒序","包含" };

            for (int i = 0; i < TBColumnList.Count; i++)
            {
                var col = TBColumnList[i];

                LabCombox cb = new LabCombox(col.Name,items);
                cb.Width = 200;
                cb.SelectedIndex = 0;
                //colContainer.Add(col);
                cb.Tag = col;
                cb.SelectedIndexChanged += (o, ex) =>
                {
                    var idxcol = IndexColumns.Find(p => p.Name.Equals(col.Name, StringComparison.OrdinalIgnoreCase));
                    if (cb.SelectedIndex==1)
                    {
                        if (idxcol == null)
                        {
                            IndexColumns.Add(new IndexTBColumn
                            {
                                Name = col.Name,
                                TypeName = col.TypeName,
                                Order = 1,
                                IsID=col.IsID,
                                IsKey=col.IsKey
                            });
                        }
                        else
                        {
                            idxcol.Order = 1;
                            idxcol.Include = false;
                        }
                    }
                    else if (cb.SelectedIndex == 2)
                    {
                        if (idxcol == null)
                        {
                            IndexColumns.Add(new IndexTBColumn
                            {
                                Name = col.Name,
                                TypeName = col.TypeName,
                                Order = -1,
                                IsID = col.IsID,
                                IsKey = col.IsKey
                            });
                        }
                        else
                        {
                            idxcol.Order = -1;
                            idxcol.Include = false;
                        }
                    }
                    else if(cb.SelectedIndex==3)
                    {
                        if (idxcol == null)
                        {
                            IndexColumns.Add(new IndexTBColumn
                            {
                                Name = col.Name,
                                TypeName = col.TypeName,
                                Include = true,
                                IsID = col.IsID,
                                IsKey = col.IsKey
                            });
                        }
                        else
                        {
                            idxcol.Order = 0;
                            idxcol.Include = true;
                        }
                    }
                    else
                    {
                        IndexColumns.Remove(idxcol);
                    }
                    
                    BindIndexCol();
                    
                };
                if (linewidth + cb.Width > ctlContainer.Width)
                {
                    lineIndex++;
                    linewidth = 20;
                }
                cb.Location = new Point(linewidth, lineIndex * cb.Height + margintop + 15);
                linewidth += cb.Width + marginright;
                ctlContainer.Controls.Add(cb);
            }
        }

        private void BindIndexCol()
        {
            var selitem = LbxColumn.SelectedItem;

            this.TBIndexName.Text = GetIndexName();

            IndexColumns = IndexColumns.OrderBy(p =>
            {
                if (p.Include)
                {
                    return 1;
                }
                return 0;
            }).ToList();

            this.LbxColumn.DataSource = IndexColumns;
            this.LbxColumn.DisplayMember = "Name";
            this.LbxColumn.ValueMember = "Name";

            if (selitem != null && LbxColumn.Items.Contains(selitem))
            {
                LbxColumn.SelectedItem = selitem;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (this.IndexColumns.Count == 0)
            {
                MessageBox.Show("请选择索引列");
                return;
            }

            if (IsPrimaryKey() && IndexColumns.Count != 1)
            {
                MessageBox.Show("只能选择一列");
                return;
            }

            if (string.IsNullOrEmpty(IndexName))
            {
                MessageBox.Show("索引名称不能为空");
                return;
            }

            if (IsAutoIncr())
            {
                if (this.IndexColumns.Count != 1)
                {
                    MessageBox.Show("只能选择一个自增长键");
                    return;
                }

                if (!this.IndexColumns[0].TypeName.Equals("int", StringComparison.OrdinalIgnoreCase) && !this.IndexColumns[0].TypeName.Equals("bigint", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("自增长键必须是数字类型");
                    return;
                }

                if (this.IndexColumns[0].IsID)
                {
                    MessageBox.Show("已经是自增主键");
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void LbxColumn_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 20;
        }

        private void LbxColumn_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }
            Brush myBrush = Brushes.Black;
            Color RowBackColorSel = Color.FromArgb(150, 200, 250);//选择项目颜色
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(RowBackColorSel);
            }
            else
            {
                myBrush = new SolidBrush(Color.White);
            }
            e.Graphics.FillRectangle(myBrush, e.Bounds);
            e.DrawFocusRectangle();//焦点框

            //绘制图标
            var col = this.IndexColumns[e.Index];
            var imgindex = 2;
            if (col.Order == 1)
            {
                imgindex = 0;
            }
            else if (col.Order == -1)
            {
                imgindex = 1;
            }
            Image image = imageList.Images[imgindex];
            Graphics graphics = e.Graphics;
            Rectangle bound = e.Bounds;
            Rectangle imgRec = new Rectangle(
                bound.X,
                bound.Y,
                bound.Height,
                bound.Height);
            Rectangle textRec = new Rectangle(
                imgRec.Right,
                bound.Y,
                bound.Width - imgRec.Right,
                bound.Height);
            if (image != null)
            {
                e.Graphics.DrawImage(
                    image,
                    imgRec,
                    0,
                    0,
                    image.Width,
                    image.Height,
                    GraphicsUnit.Pixel);
                //绘制字体
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(col.Name, e.Font, new SolidBrush(Color.Black), textRec, stringFormat);
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (LbxColumn.SelectedItem != null)
            {
                var idxcol = LbxColumn.SelectedItem as IndexTBColumn;
                
                var colindex = IndexColumns.FindIndex(p => p.Name.Equals(idxcol.Name, StringComparison.OrdinalIgnoreCase));

                if (colindex < IndexColumns.Count - 1)
                {
                    if (!idxcol.Include && IndexColumns[colindex + 1].Include)
                    {
                        return;
                    }
                    var nextcol = IndexColumns[colindex + 1];
                    IndexColumns[colindex + 1] = IndexColumns[colindex];
                    IndexColumns[colindex] = nextcol;

                    BindIndexCol();

                    LbxColumn.SelectedIndex = colindex + 1;
                }
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (LbxColumn.SelectedItem != null)
            {
                var idxcol = LbxColumn.SelectedItem as IndexTBColumn;

                var colindex = IndexColumns.FindIndex(p => p.Name.Equals(idxcol.Name, StringComparison.OrdinalIgnoreCase));

                if (colindex > 0)
                {
                    if (idxcol.Include && !IndexColumns[colindex - 1].Include)
                    {
                        return;
                    }
                    var nextcol = IndexColumns[colindex - 1];
                    IndexColumns[colindex - 1] = IndexColumns[colindex];
                    IndexColumns[colindex] = nextcol;

                    

                    BindIndexCol();

                    LbxColumn.SelectedIndex = colindex - 1;
                }

            }
        }
    }
}
