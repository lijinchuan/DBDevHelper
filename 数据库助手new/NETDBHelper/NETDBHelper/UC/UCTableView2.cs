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
using System.Drawing.Drawing2D;
using NETDBHelper.Drawing;
using LJC.FrameWork.CodeExpression;
using System.Security.Policy;
using Biz.Common;
using NETDBHelper.Resources;

namespace NETDBHelper.UC
{
    public partial class UCTableView2 : UserControl
    {
        DBSource DBSource = null;
        string DBName = null;
        string TBName = null;
        bool issamedb = true;

        private List<TBColumn> ColumnsList = null;

        private Func<List<Tuple<string, string>>> FunLoadTables = null;

        private Action<UCTableView2> OnComplete;

        private bool IsTitleDraging = false;
        private Point TitleDragStart = Point.Empty;
        private Point TitleDragEnd = Point.Empty;

        private Func<Point, Point, bool, bool> onCheckConflict;

        public Action<RelColumn> OnAddNewRelColumn;

        private List<List<Point>> relLineList = null;

        public UCTableView2()
        {
            InitializeComponent();
        }

        public UCTableView2(DBSource dbSource,bool samedb, string dbname, string tbname, Func<List<Tuple<string,string>>> funcLoadTables, Action<UCTableView2> onComplete,
            Func<Point, Point, bool, bool> checkConflict)
        {
            InitializeComponent();
            //this.AutoSize = true;

            DBSource = dbSource;
            DBName = dbname;
            TBName = tbname;
            issamedb = samedb;

            LBTabname.BackColor = Color.LightBlue;
            LBTabname.Visible = false;
            LBTabname.AutoSize = false;
            LBTabname.Height = 20;
            LBTabname.ForeColor = Color.Blue;
            if (!string.IsNullOrWhiteSpace(tbname))
            {
                LBTabname.Text = issamedb ? $"{tbname}" : $"[{dbname}].{tbname}";
            }
            this.ColumnsPanel.Location = new Point(1, LBTabname.Height + 1);
            this.ColumnsPanel.Width = LBTabname.Width - 2;
            this.ColumnsPanel.Height = this.Height - this.LBTabname.Height - 2;

            this.CBTables.Height = 20;
            this.FunLoadTables = funcLoadTables;

            this.OnComplete = onComplete;
            this.BorderStyle = BorderStyle.None;

            onCheckConflict = checkConflict;

            CBCoumns.Visible = false;

        }


        private void CBTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBTables.SelectedIndex != -1)
            {
                this.TBName = CBTables.Text;
                LBTabname.Text = issamedb ? $"{TBName}" : $"[{DBName}].{TBName}";

                this.LBTabname.Visible = true;
                this.LBTabname.Location = new Point(1, 1);
                this.LBTabname.Width = this.Width - 2;
                this.CBTables.Visible = false;

                ColumnsList = SQLHelper.GetColumns(DBSource, DBName, CBTables.SelectedItem.ToString()).ToList();
                BindColumns();
            }
        }

        public string TableName
        {
            get
            {
                return this.TBName;
            }
        }

        public string DataBaseName
        {
            get
            {
                return this.DBName;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(Pens.DarkOliveGreen, 0, 0, this.Width - 1, this.Height - 1);
        }

        private void BindColumns()
        {

            var collist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<RelColumn>(nameof(RelColumn),
                "SDTC", new[] { DBName.ToLower(), this.TBName.ToLower() },
                new[] { DBName.ToLower(), this.TBName.ToLower() }, 1, int.MaxValue);

            var relcollist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<RelColumn>(nameof(RelColumn),
                "SDRTC", new[] { this.DBName.ToLower(), this.TBName.ToLower() },
                new[] { this.DBName.ToLower(), this.TBName.ToLower() }, 1, int.MaxValue);

            this.CBCoumns.Items.AddRange(ColumnsList.Where(p => !collist.Any(q => q.ColName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
            && !relcollist.Any(q => q.RelColName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))).ToArray());

            this.CBCoumns.SelectedIndex = -1;
            this.CBCoumns.Visible = true;
            this.CBCoumns.DisplayMember = "Name";
            this.CBCoumns.SelectedIndexChanged += CBCoumns_SelectedIndexChanged;

            HashSet<string> ha = new HashSet<string>();
            foreach (var item in collist)
            {
                if (!ha.Contains(item.ColName))
                {
                    ha.Add(item.ColName);

                    var col = ColumnsList.FirstOrDefault(p => p.Name.Equals(item.ColName, StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        AddColumnLable(col);
                    }
                }

            }

            foreach (var item in relcollist)
            {
                if (!ha.Contains(item.RelColName))
                {
                    ha.Add(item.RelColName);

                    var col = ColumnsList.FirstOrDefault(p => p.Name.Equals(item.RelColName, StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        AddColumnLable(col);
                    }
                }

            }

            if (this.OnComplete != null)
            {
                this.OnComplete(this);
            }
        }

        public Rectangle FindTBColumnScreenRect(string colname)
        {
            foreach (Control lb in this.ColumnsPanel.Controls)
            {
                if (lb is Label)
                {
                    var tag = (lb as Label).Tag;
                    if (tag is TBColumn)
                    {
                        if ((tag as TBColumn).Name.Equals(colname, StringComparison.OrdinalIgnoreCase))
                        {
                            return lb.Parent.RectangleToScreen(lb.Bounds);
                        }
                    }
                }
            }

            return Rectangle.Empty;
        }

        private void AddColumnLable(TBColumn tbcol)
        {
            var lb = new Label();
            lb.AutoSize = false;
            lb.ImageAlign = ContentAlignment.MiddleLeft;
            if (tbcol.IsString())
            {
                lb.Image = SQLTypeRs.CHAR;
            }
            else if (tbcol.IsDateTime())
            {
                lb.Image = SQLTypeRs.DATE;
            }
            else if (tbcol.IsNumber())
            {
                lb.Image = SQLTypeRs.NUMBER;
            }
            else if (tbcol.IsBoolean())
            {
                lb.Image = SQLTypeRs.BOOL;
            }
            lb.UseMnemonic = true;

            lb.Text = "   " + tbcol.Name;
            lb.Location = CBCoumns.Location;
            lb.Tag = tbcol;
            lb.Height = 20;
            var loc = lb.Location;
            loc.Offset(0, lb.Height);
            CBCoumns.Location = loc;
            lb.Width = this.Width - 2;
            ColumnsPanel.Controls.Add(lb);

            if (this.ColumnsPanel.Height < this.CBCoumns.Location.Y + this.CBCoumns.Height + 10)
            {
                this.ColumnsPanel.Height = this.CBCoumns.Location.Y + this.CBCoumns.Height + 10;

                //using (var g = this.CreateGraphics())
                //{
                //    using (var p = new Pen(this.BackColor, 1))
                //    {
                //        g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
                //    }
                //}

                this.Height = this.ColumnsPanel.Location.Y + this.ColumnsPanel.Height + 1;
                this.Invalidate();
            }

            bool isDraging = false;
            Point dragStart = Point.Empty;
            Point dragEnd = Point.Empty;
            List<Point> points = null;
            lb.MouseMove += (s, ee) =>
            {
                if (ee.Button == MouseButtons.Left)
                {
                    isDraging = Math.Abs(dragEnd.X - dragStart.X) > 10 || Math.Abs(dragEnd.Y - dragStart.Y) > 10;

                    dragEnd = new Point(ee.X, ee.Y);
                    if (isDraging)
                    {
                        if (points != null)
                        {
                            using (var g = this.Parent.CreateGraphics())
                            {
                                //g.SmoothingMode = SmoothingMode.AntiAlias;
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
                        Point start = this.Parent.PointToClient(this.ColumnsPanel.PointToScreen(new Point(lb.Location.X + lb.Width, lb.Location.Y + lb.Height / 2)));
                        Point dest = this.Parent.PointToClient(lb.PointToScreen(dragEnd));
                        points = new StepSelector(Math.Max(this.Parent.Width, (this.Parent as Panel).HorizontalScroll.Maximum),
                                    Math.Max(this.Parent.Height, (this.Parent as Panel).VerticalScroll.Maximum), start, dest, (s1, s2, b) => onCheckConflict(s1, s2, b),_firstDirection:StepDirection.right, _destDirection: StepDirection.right).Select();


                        using (var g = this.Parent.CreateGraphics())
                        {
                            using (var p = new Pen(Color.Gray, 2))
                            {
                                //p.StartCap = LineCap.Round;
                                //g.SmoothingMode = SmoothingMode.AntiAlias;
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

            lb.MouseUp += (s, ee) =>
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
                        var pt = dragEnd;
                        pt.Offset(20, 0);
                        var col = (Util.FindParent<UCTableRelMap>(this))?.FindColumn(lb.PointToScreen(pt));
                        if (col != null)
                        {
                            var newrelcolumn = new RelColumn
                            {
                                ColName = (lb.Tag as TBColumn).Name.ToLower(),
                                DBName = this.DBName.ToLower(),
                                RelColName = col.Name.ToLower(),
                                RelDBName = col.DBName.ToLower(),
                                RelTBName = col.TBName.ToLower(),
                                ServerName = DBSource.ServerName.ToLower(),
                                TBName = this.TBName.ToLower()
                            };
                            var relcollist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine
                            .Scan<RelColumn>(nameof(RelColumn), "SDTC", new[] { newrelcolumn.DBName, newrelcolumn.TBName },
                            new[] { newrelcolumn.DBName, newrelcolumn.TBName }, 1, int.MaxValue);

                            if (!relcollist.Any(p => p.RelDBName.ToLower() == newrelcolumn.RelDBName && p.RelTBName.ToLower() == newrelcolumn.RelTBName && p.RelColName.ToLower() == newrelcolumn.RelColName))
                            {
                                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<RelColumn>(nameof(RelColumn), newrelcolumn);
                                //通知父控件
                                if (OnAddNewRelColumn != null)
                                {
                                    OnAddNewRelColumn(newrelcolumn);
                                }
                            }
                        }
                        else
                        {
                            if (points != null)
                            {
                                using (var g = this.Parent.CreateGraphics())
                                {
                                    //g.SmoothingMode = SmoothingMode.AntiAlias;
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
                        }

                        this.Invalidate();

                        isDraging = false;
                        //处理结束事件
                    }
                }

                dragStart = Point.Empty;
                dragEnd = Point.Empty;
            };

            lb.MouseDown += (s, ee) =>
            {
                if (ee.Button == MouseButtons.Left)
                {
                    dragStart = new Point(ee.X, ee.Y);
                    dragEnd = new Point(ee.X, ee.Y);
                }
            };
        }

        private void CBCoumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBCoumns.SelectedIndex != -1)
            {
                AddColumnLable((TBColumn)CBCoumns.SelectedItem);
                var selectedindex = CBCoumns.SelectedIndex;
                CBCoumns.SelectedIndex = -1;
                CBCoumns.Items.RemoveAt(selectedindex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrWhiteSpace(TBName))
            {
                CBTables.DataSource = FunLoadTables().Select(p => p.Item2).OrderBy(p => p).ToList();
                CBTables.SelectedIndex = -1;
                this.LBTabname.Visible = false;
                this.CBTables.Visible = true;
                this.CBTables.Location = new Point(1, 1);
                this.CBTables.Width = this.Width - 2;
                this.CBTables.SelectedIndexChanged += CBTables_SelectedIndexChanged;
            }
            else
            {
                this.LBTabname.Text = issamedb ? $"{TBName}" : $"[{DBName}].{TBName}";
                this.LBTabname.Visible = true;
                this.LBTabname.Location = new Point(1, 1);
                this.LBTabname.Width = this.Width - 2;
                this.CBTables.Visible = false;
                ColumnsList = SQLHelper.GetColumns(DBSource, DBName, TBName).ToList();
                BindColumns();

            }

            this.LBTabname.MouseMove += OnLBTabnameMouseMove;
            this.LBTabname.MouseDown += OnLBTabnameMouseDown;
            this.LBTabname.MouseUp += OnLBTabnameMouseUp;
            this.LBTabname.DoubleClick += LBTabname_DoubleClick;

            ColumnsPanel.DoubleClick += ColumnsPanel_DoubleClick;
            this.CBCoumns.Visible = false;
        }

        private void ColumnsPanel_DoubleClick(object sender, EventArgs e)
        {
            this.CBCoumns.Visible = !this.CBCoumns.Visible;
        }

        private void LBTabname_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TBName))
            {
                this.LBTabname.Visible = false;
                this.CBTables.Visible = true;
            }
        }

        protected void OnLBTabnameMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //var rect = new Rectangle(this.Location, this.Size);
                //if (rect.Contains(e.Location))
                {
                    var pt = this.PointToScreen(e.Location);
                    IsTitleDraging = Math.Abs(pt.X - TitleDragEnd.X) > 2 || Math.Abs(pt.Y - TitleDragEnd.Y) > 2; //&& ((DragEnd.X > DragStart.X && dragSource != tabExDic.Last().Value) || (DragEnd.X < DragStart.X && dragSource != tabExDic.First().Value));
                    if (IsTitleDraging)
                    {
                        TitleDragStart = TitleDragEnd;
                        TitleDragEnd = pt;
                    }

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
                if (!IsTitleDraging && TitleDragStart != Point.Empty)
                {
                    var pt = this.PointToScreen(e.Location);
                    IsTitleDraging = Math.Abs(pt.X - TitleDragStart.X) > 0 || Math.Abs(pt.Y - TitleDragStart.Y) > 0; //&& ((DragEnd.X > DragStart.X && dragSource != tabExDic.Last().Value) || (DragEnd.X < DragStart.X && dragSource != tabExDic.First().Value));
                }

                if (IsTitleDraging)
                {
                    isdragendevent = true;

                }

                if (isdragendevent)
                {
                    OnTabDragEnd(null);
                }

                if (IsTitleDraging)
                {

                    IsTitleDraging = false;
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
                TitleDragStart = this.PointToScreen(new Point(e.X, e.Y));
                TitleDragEnd = this.PointToScreen(new Point(e.X, e.Y));
            }
        }

        private void OnTabDragOver(DragEventArgs drgevent)
        {
            var loc = this.Location;
            loc.Offset(TitleDragEnd.X - TitleDragStart.X, TitleDragEnd.Y - TitleDragStart.Y);
            this.Location = loc;

            this.Invalidate();
            //this.Parent.Invalidate(true);
        }

        private void OnTabDragEnd(DragEventArgs drgevent)
        {
            this.Parent.Invalidate(true);
        }
    }
}
