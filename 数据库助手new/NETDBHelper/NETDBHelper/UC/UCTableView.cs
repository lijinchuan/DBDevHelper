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
using Biz.Common.Data;
using NPOI.SS.Formula.Functions;
using NETDBHelper.Drawing;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

namespace NETDBHelper.UC
{
    public partial class UCTableView : UserControl
    {
        DBSource DBSource = null;
        string DBName = null;
        string TBName = null;

        private Func<List<string>> FunLoadTables = null;

        private Action<UCTableView> OnComplete;

        private bool IsTitleDraging = false;
        private Point TitleDragStart = Point.Empty;
        private Point TitleDragEnd = Point.Empty;

        private Func<Point, Point,bool, bool> onCheckConflict;

        public UCTableView()
        {
            InitializeComponent();
        }

        public UCTableView(DBSource dbSource, string dbname, string tbname, Func<List<string>> funcLoadTables, Action<UCTableView> onComplete,
            Func<Point,Point,bool,bool> checkConflict)
        {
            InitializeComponent();
            this.AutoSize = true;
            this.DGVColumns.ColumnHeadersVisible = false;
            this.DGVColumns.BackgroundColor = Color.LightBlue;
            this.DGVColumns.RowHeadersVisible = true;
            this.DGVColumns.AllowUserToAddRows = false;

            DBSource = dbSource;
            DBName = dbname;

            this.CBTables.SelectedIndexChanged += CBTables_SelectedIndexChanged;
            this.FunLoadTables = funcLoadTables;
            CBTables.DataSource = FunLoadTables();
            TBName = tbname;
            
            this.DGVColumns.BindingContextChanged += DGVColumns_BindingContextChanged;
            this.DGVColumns.DataBindingComplete += DGVColumns_DataBindingComplete;
            this.DGVColumns.CellClick += DGVColumns_CellClick;

            

            this.OnComplete = onComplete;
            this.BackColor = LBTabname.BackColor = Color.LightBlue;
            LBTabname.AutoSize = false;
            LBTabname.Height = 24;
            LBTabname.ForeColor = Color.Blue;
            LBTabname.Text = tbname;
            this.BorderStyle = BorderStyle.FixedSingle;
            onCheckConflict = checkConflict;

        }

        private void DGVColumns_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var colname = DGVColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                var rect = DGVColumns.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                var ctl = this.DGVColumns.Controls.Find("pic_" + colname, false).FirstOrDefault();
                if (ctl==null)
                {
                    var pic = new PictureBox();
                    pic.Image = Resources.Resource1.link;
                    pic.Location = new Point(rect.Right, rect.Y + rect.Height / 2 - 8);
                    pic.Name = "pic_" + colname;
                    
                    this.DGVColumns.Controls.Add(pic);
                    pic.BringToFront();

                    bool isDraging = false;
                    Point dragStart = Point.Empty;
                    Point dragEnd = Point.Empty;
                    List<Point> points = null;
                    pic.MouseMove += (s, ee) =>
                    {
                        if (ee.Button == MouseButtons.Left)
                        {
                            isDraging = Math.Abs(dragEnd.X - dragStart.X) > 10 || Math.Abs(dragEnd.Y - dragEnd.Y) > 10;

                            dragEnd = new Point(ee.X, ee.Y);
                            if (isDraging)
                            {
                                if (points != null)
                                {
                                    using (var g = this.Parent.CreateGraphics())
                                    {
                                        g.SmoothingMode = SmoothingMode.AntiAlias;
                                        using (var p = new Pen(this.Parent.BackColor, 2))
                                        {
                                            //p.StartCap = LineCap.Round;
                                            for (int i = 1; i <= points.Count - 1; i++)
                                            {
                                                if (i == points.Count - 1)
                                                {
                                                    AdjustableArrowCap arrowCap = new AdjustableArrowCap(p.Width * 2 + 1, p.Width + 2 + 1, true);
                                                    p.CustomEndCap = arrowCap;
                                                }
                                                g.DrawLine(p, points[i - 1], points[i]);
                                                //g.DrawPie(p, new RectangleF(points[i], new SizeF(5, 5)), 0, 360);
                                            }
                                        }
                                    }
                                }

                                //处理
                                Point start = this.Parent.PointToClient(this.DGVColumns.PointToScreen(new Point(pic.Location.X, pic.Location.Y)));
                                Point dest = this.Parent.PointToClient(pic.PointToScreen(dragEnd));
                                points = new StepSelector(start, dest, (s1, s2,b) => onCheckConflict(s1, s2,b), _destDirection: StepDirection.right).Select();

                                using (var g = this.Parent.CreateGraphics())
                                {
                                    using (var p = new Pen(Color.Gray, 2))
                                    {
                                        //p.StartCap = LineCap.Round;
                                        g.SmoothingMode = SmoothingMode.AntiAlias;
                                        for (int i = 1; i <= points.Count - 1; i++)
                                        {
                                            if (i == points.Count - 1)
                                            {
                                                AdjustableArrowCap arrowCap = new AdjustableArrowCap(p.Width * 2 + 1, p.Width + 2 + 1, true);
                                                p.CustomEndCap = arrowCap;
                                            }
                                            g.DrawLine(p, points[i - 1], points[i]);
                                            //g.DrawPie(p, new RectangleF(points[i], new SizeF(5, 5)), 0, 360);
                                        }
                                    }
                                }

                                this.Invalidate();
                            }
                        }
                    };

                    pic.MouseUp += (s, ee) =>
                    {
                        bool isdragendevent = false;
                        if (ee.Button == MouseButtons.Left)
                        {
                            if (isDraging)
                            {
                                isdragendevent = true;
                            }

                            if (isdragendevent)
                            {
                                isDraging = false;
                                //处理结束事件
                            }

                            if (isDraging)
                            {
                                isDraging = false;
                                this.Invalidate();
                            }
                        }

                        dragStart = Point.Empty;
                        dragEnd = Point.Empty;
                    };

                    pic.MouseDown += (s, ee) =>
                    {
                        if (ee.Button == MouseButtons.Left)
                        {
                            dragStart = new Point(ee.X, ee.Y);
                            dragEnd = new Point(ee.X, ee.Y);
                        }
                    };


                }
                else
                {
                    ctl.BringToFront();
                }
            }
        }

        private void DGVColumns_BindingContextChanged(object sender, EventArgs e)
        {
        }

        private void UCTableView_MouseHover(object sender, EventArgs e)
        {
            MessageBox.Show("hover");
        }

        private void DGVColumns_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.DGVColumns.Height = this.DGVColumns.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + 5;
            InitLocLayout();
            if (OnComplete != null)
            {
                OnComplete(this);
            }
        }

        private void CBTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBTables.SelectedIndex != 1)
            {
                CBTables.Visible = false;
                LBTabname.Text = CBTables.Text;
                LBTabname.Visible = true;
                this.TBName = CBTables.Text;

                var columns = SQLHelper.GetColumns(DBSource, DBName, CBTables.SelectedItem.ToString()).ToList();
                DGVColumns.DataSource = columns.Select(p => new
                {
                    p.Name
                }).ToList();
                
                InitLocLayout();
            }
        }

        public string TableName
        {
            get
            {
                return this.TBName;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DBSource != null && DBName != null)
            {
                if (!string.IsNullOrWhiteSpace(TBName))
                {
                    this.CBTables.SelectedItem = TBName;
                }
            }

            this.LBTabname.MouseMove += OnLBTabnameMouseMove;
            this.LBTabname.MouseDown += OnLBTabnameMouseDown;
            this.LBTabname.MouseUp += OnLBTabnameMouseUp;
            this.LBTabname.DoubleClick += LBTabname_DoubleClick;
            //this.CBTables.MouseHover += UCTableView_MouseHover;
        }

        private void InitLocLayout()
        {
            if (this.LBTabname.Visible)
            {
                this.LBTabname.Location = new Point(0, 0);
                this.LBTabname.Width = this.Width;
                this.DGVColumns.Location = new Point(0, LBTabname.Height);
                this.Height = this.LBTabname.Height + this.DGVColumns.Height;
                this.DGVColumns.Width = this.Width;
            }
            else
            {
                this.CBTables.Location = new Point(0, 0);
                this.CBTables.Width = this.Width;
                this.DGVColumns.Location = new Point(0, CBTables.Height);
                this.Height = this.CBTables.Height + this.DGVColumns.Height;
                this.DGVColumns.Width = this.Width;
            }
        }

        private void LBTabname_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TBName))
            {
                this.LBTabname.Visible = false;
                this.CBTables.Visible = true;
                InitLocLayout();
            }
        }

        protected void OnLBTabnameMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //var rect = new Rectangle(this.Location, this.Size);
                //if (rect.Contains(e.Location))
                {
                    IsTitleDraging = Math.Abs(TitleDragEnd.X - TitleDragStart.X) > 10 || Math.Abs(TitleDragEnd.Y - TitleDragEnd.Y) > 10; //&& ((DragEnd.X > DragStart.X && dragSource != tabExDic.Last().Value) || (DragEnd.X < DragStart.X && dragSource != tabExDic.First().Value));

                    TitleDragEnd = new Point(e.X, e.Y);
                    if (IsTitleDraging)
                    {
                        OnTabDragOver(null);
                    }
                }

            }
        }

        protected void OnLBTabnameMouseUp(object sender, MouseEventArgs e)
        {
            bool isdragendevent = false;
            if (e.Button == MouseButtons.Left)
            {
                if (IsTitleDraging)
                {
                    isdragendevent = true;
                }

                if (isdragendevent)
                {
                    IsTitleDraging = false;
                    OnTabDragEnd(null);
                }

                if (IsTitleDraging)
                {
                    IsTitleDraging = false;
                    this.Invalidate();
                }
            }

            TitleDragStart = Point.Empty;
            TitleDragEnd = Point.Empty;
        }

        protected void OnLBTabnameMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //IsDraging = true;
                TitleDragStart = new Point(e.X, e.Y);
                TitleDragEnd = new Point(e.X, e.Y);
            }
        }

        private void OnTabDragOver(DragEventArgs drgevent)
        {
            //foreach (var item in this.tabExDic)
            //{
            //    if (item.Value.StripRect.Contains(DragStart))
            //    {
            //        item.Value.TabPage.Invalidate();
            //        break;
            //    }
            //}

            this.Invalidate();

            var loc = this.Location;
            loc.Offset(TitleDragEnd.X - TitleDragStart.X, TitleDragEnd.Y - TitleDragStart.Y);
            this.Location = loc;
        }

        private void OnTabDragEnd(DragEventArgs drgevent)
        {
        }
    }
}
