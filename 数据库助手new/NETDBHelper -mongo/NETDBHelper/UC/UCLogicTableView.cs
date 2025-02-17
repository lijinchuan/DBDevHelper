﻿using System;
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
using Biz.Common;
using NETDBHelper.Resources;
using System.Drawing.Drawing2D;
using NETDBHelper.Drawing;
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace NETDBHelper.UC
{
    public partial class UCLogicTableView : UserControl
    {
        private int _logicMapId = 0;
        DBSource DBSource = null;
        string DBName = null;
        string TBName = null;
        bool issamedb = true;

        public int LogicMapTableId
        {
            get;
            set;
        }

        private List<TBColumn> ColumnsList = null;

        private Func<List<Tuple<string, string>>> FunLoadTables = null;

        private Action<UCLogicTableView> OnComplete;

        private bool IsTitleDraging = false;
        private Point TitleDragStart = Point.Empty;
        private Point TitleDragEnd = Point.Empty;

        private Func<Point, Point, bool, bool> onCheckConflict;

        public Action<LogicMapRelColumn> OnAddNewRelColumn;

        private int LbIdx = 0;

        public UCLogicTableView()
        {
            InitializeComponent();
        }

        public UCLogicTableView(DBSource dbSource, bool samedb, string dbname, string tbname,int logicMapId, Func<List<Tuple<string, string>>> funcLoadTables, Action<UCLogicTableView> onComplete,
            Func<Point, Point, bool, bool> checkConflict)
        {
            InitializeComponent();
            //this.AutoSize = true;

            DBSource = dbSource;
            DBName = dbname;
            TBName = tbname;
            issamedb = samedb;

            this._logicMapId = logicMapId;

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

                ColumnsList = MongoDBHelper.GetColumns(DBSource, DBName, CBTables.SelectedItem.ToString()).ToList();
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

            var collist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn),
                "LSDTC", new object[] { this._logicMapId, DBName.ToLower(), this.TBName.ToLower() },
                new object[] { this._logicMapId, DBName.ToLower(), this.TBName.ToLower() }, 1, int.MaxValue);

            var relcollist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn),
                "LSDRTC", new object[] { this._logicMapId, this.DBName.ToLower(), this.TBName.ToLower() },
                new object[] { this._logicMapId, this.DBName.ToLower(), this.TBName.ToLower() }, 1, int.MaxValue);

            this.CBCoumns.Items.AddRange(ColumnsList.Where(p => !collist.Any(q => q.ColName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
            && !relcollist.Any(q => q.RelColName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))).ToArray());

            this.CBCoumns.SelectedIndex = -1;
            this.CBCoumns.Visible = true;
            this.CBCoumns.DisplayMember = "Name";
            this.CBCoumns.SelectedIndexChanged += CBCoumns_SelectedIndexChanged;

            HashSet<string> ha = new HashSet<string>();
            List<LogicMapRelColumn> logicMapRelColumns = null;
            foreach (var item in collist)
            {
                if (!ha.Contains(item.ColName))
                {
                    ha.Add(item.ColName);

                    var col = ColumnsList.FirstOrDefault(p => p.Name.Equals(item.ColName, StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        AddColumnLable(col,ref logicMapRelColumns);
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
                        AddColumnLable(col,ref logicMapRelColumns);
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

        private void AddColumnLable(TBColumn tbcol,ref List<LogicMapRelColumn> logicMapRelColumns)
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

            if (LbIdx % 2 == 1)
            {
                lb.BackColor = Color.LightYellow;
            }
            LbIdx++;

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

            //加到库
            if (_logicMapId > 0)
            {
                long total = 0;
                if (logicMapRelColumns == null)
                {
                    logicMapRelColumns = BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID",
                        new object[] { this._logicMapId }, new object[] { this._logicMapId }, 1, int.MaxValue, ref total);
                }
                if (!logicMapRelColumns.Any(p => p.DBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase) && p.TBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.ColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase))
                    && !logicMapRelColumns.Any(p => p.RelDBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase) && p.RelTBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.RelColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapRelColumn), new LogicMapRelColumn
                    {
                        LogicID = _logicMapId,
                        ColName = tbcol.Name.ToLower(),
                        DBName = tbcol.DBName.ToLower(),
                        TBName = tbcol.TBName.ToLower(),
                        RelColName = string.Empty,
                        RelDBName = string.Empty,
                        RelTBName = string.Empty
                    });
                }
                else
                {
                    var logiccol = logicMapRelColumns.FirstOrDefault(p => p.DBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase)
                      && p.TBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.ColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase)
                      && p.RelColName == string.Empty);

                    if (logiccol != null && logiccol.Desc != null)
                    {
                        lb.Text = $"   {tbcol.Name}({logiccol.Desc})";
                    }
                }
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
                                        var pt1 = points[i - 1];
                                        var pt2 = points[i];
                                        pt1.Offset((this.Parent as Panel).AutoScrollPosition.X, (this.Parent as Panel).AutoScrollPosition.Y);
                                        pt2.Offset((this.Parent as Panel).AutoScrollPosition.X, (this.Parent as Panel).AutoScrollPosition.Y);
                                        g.DrawLine(p, pt1, pt2);
                                        //g.DrawPie(p, new RectangleF(points[i], new SizeF(5, 5)), 0, 360);
                                    }
                                }
                            }
                        }

                        //处理
                        Point start = this.Parent.PointToClient(this.ColumnsPanel.PointToScreen(new Point(lb.Location.X + lb.Width, lb.Location.Y + lb.Height / 2)));
                        Point dest = this.Parent.PointToClient(lb.PointToScreen(dragEnd));
                        start.Offset(-(this.Parent as Panel).AutoScrollPosition.X, -(this.Parent as Panel).AutoScrollPosition.Y);
                        dest.Offset(-(this.Parent as Panel).AutoScrollPosition.X, -(this.Parent as Panel).AutoScrollPosition.Y);
                        points = new StepSelector(Math.Max(this.Parent.Width, (this.Parent as Panel).HorizontalScroll.Maximum),
                                    Math.Max(this.Parent.Height, (this.Parent as Panel).VerticalScroll.Maximum), start, dest, (s1, s2, b) => onCheckConflict(s1, s2, b), _firstDirection: StepDirection.right, _destDirection: StepDirection.right).Select();


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
                                    var pt1 = points[i - 1];
                                    var pt2 = points[i];
                                    pt1.Offset((this.Parent as Panel).AutoScrollPosition.X, (this.Parent as Panel).AutoScrollPosition.Y);
                                    pt2.Offset((this.Parent as Panel).AutoScrollPosition.X, (this.Parent as Panel).AutoScrollPosition.Y);
                                    g.DrawLine(p, pt1, pt2);
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
                        var col = (Util.FindParent<UCLogicMap>(this))?.FindColumn(lb.PointToScreen(pt));
                        if (col != null)
                        {
                            var newrelcolumn = new LogicMapRelColumn
                            {
                                ColName = (lb.Tag as TBColumn).Name.ToLower(),
                                DBName = this.DBName.ToLower(),
                                RelColName = col.Name.ToLower(),
                                RelDBName = col.DBName.ToLower(),
                                RelTBName = col.TBName.ToLower(),
                                TBName = this.TBName.ToLower(),
                                LogicID=_logicMapId
                            };
                            var relcollist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine
                            .Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LSDTC", new object[] { newrelcolumn.LogicID, newrelcolumn.DBName, newrelcolumn.TBName },
                            new object[] { newrelcolumn.LogicID, newrelcolumn.DBName, newrelcolumn.TBName }, 1, int.MaxValue);

                            if (!relcollist.Any(p => p.RelDBName.ToLower() == newrelcolumn.RelDBName && p.RelTBName.ToLower() == newrelcolumn.RelTBName && p.RelColName.ToLower() == newrelcolumn.RelColName))
                            {
                                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<LogicMapRelColumn>(nameof(LogicMapRelColumn), newrelcolumn);
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
                                            var pt1 = points[i - 1];
                                            var pt2 = points[i];
                                            pt1.Offset((this.Parent as Panel).AutoScrollPosition.X, (this.Parent as Panel).AutoScrollPosition.Y);
                                            pt2.Offset((this.Parent as Panel).AutoScrollPosition.X, (this.Parent as Panel).AutoScrollPosition.Y);
                                            g.DrawLine(p, pt1, pt2);
                                            //g.DrawPie(p, new RectangleF(points[i], new SizeF(5, 5)), 0, 360);
                                        }
                                    }
                                }
                            }
                        }

                        this.Parent.Invalidate();

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


            lb.DoubleClick += (s,ee)=>
            {
                if (_logicMapId > 0)
                {
                    long total = 0;
                    var allcols = BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID",
                        new object[] { this._logicMapId }, new object[] { this._logicMapId }, 1, int.MaxValue, ref total);
                    var logiccol = allcols.FirstOrDefault(p => p.DBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase)
                      && p.TBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.ColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase)
                      && p.RelColName == string.Empty);
                    var logiccoldesc = logiccol?.Desc ?? string.Empty;
                    var dlg = new SubForm.InputStringDlg($"逻辑备注{tbcol.TBName}.{tbcol.Name}", logiccoldesc);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (logiccol != null)
                        {
                            logiccol.Desc = dlg.InputString;
                            BigEntityTableEngine.LocalEngine.Update<LogicMapRelColumn>(nameof(LogicMapRelColumn), logiccol);
                        }
                        else
                        {
                            BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapRelColumn), new LogicMapRelColumn
                            {
                                LogicID = _logicMapId,
                                ColName = tbcol.Name.ToLower(),
                                DBName = tbcol.DBName.ToLower(),
                                TBName = tbcol.TBName.ToLower(),
                                RelColName = string.Empty,
                                RelDBName = string.Empty,
                                RelTBName = string.Empty,
                                Desc=dlg.InputString
                            });
                        }
                        lb.Text = $"   {tbcol.Name}({dlg.InputString})";
                    }
                }
            };
        }

        private void CBCoumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBCoumns.SelectedIndex != -1)
            {
                List<LogicMapRelColumn> logicMapRelColumns = null;
                AddColumnLable((TBColumn)CBCoumns.SelectedItem,ref logicMapRelColumns);
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
                ColumnsList = MongoDBHelper.GetColumns(DBSource, DBName, TBName).ToList();
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

            if (this.LogicMapTableId > 0 && this.Parent != null)
            {
                
                var pt = this.Location;
                pt.Offset(-(this.Parent as Panel).AutoScrollPosition.X, -(this.Parent as Panel).AutoScrollPosition.Y);

                LogicMapTable tb = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<LogicMapTable>(nameof(LogicMapTable), this.LogicMapTableId);
                
                tb.Posx = pt.X;
                tb.Posy = pt.Y;

                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Update(nameof(LogicMapTable), tb);
            }
        }
    }
}
