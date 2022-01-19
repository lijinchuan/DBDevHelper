using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
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

        private TextBox NoteTextBox = null;

        private Func<Point, Point, bool, bool> onCheckConflict;

        public Action<LogicMapRelColumn> OnAddNewRelColumn;

        public UCLogicTableView()
        {
            InitializeComponent();
        }

        public bool IsNoteTable
        {
            get
            {
                return TBName?.StartsWith("#note_",StringComparison.Ordinal)==true;
            }
        }

        public bool IsTempTable
        {
            get
            {
                return TBName?.StartsWith("$", StringComparison.Ordinal) == true;
            }
        }

        public void Check()
        {
            if (!string.IsNullOrWhiteSpace(LBTabname.Text))
            {
                var panels = new[] { ColumnsPanel,ColumnsPanelOutPut};
                var isoverflow = false;
                var maxwidth = Width * 1f;
                using (var g = CreateGraphics())
                {
                    var size = g.MeasureString(LBTabname.Text, this.LBTabname.Font);
                    if (size.Width > this.LBTabname.Width)
                    {
                        isoverflow = true;
                        maxwidth = size.Width;
                    }
                    foreach (var pannel in panels)
                    {
                        foreach (var ctl in pannel.Controls)
                        {
                            if (ctl is Label)
                            {
                                var lb = ctl as Label;
                                var size2 = g.MeasureString(lb.Text, lb.Font);
                                if (size2.Width > lb.Width)
                                {
                                    isoverflow = true;
                                    if (size2.Width > maxwidth)
                                    {
                                        maxwidth = size2.Width;
                                    }
                                }
                            }
                        }
                    }
                }
                if (isoverflow)
                {
                    maxwidth += 1;
                    this.Width = (int)maxwidth + 2;
                    this.LBTabname.Width = (int)maxwidth;
                    this.ColumnsPanel.Width = (int)maxwidth;
                    this.CBCoumns.Width = (int)maxwidth - 2;
                    this.ColumnsPanelOutPut.Width = (int)maxwidth;
                    this.CBCoumnsOutput.Width = (int)maxwidth - 2;
                    this.groupBox1.Width = this.Width;
                    foreach (var pannel in panels)
                    {
                        foreach (var ctl in pannel.Controls)
                        {
                            if (ctl is Label)
                            {
                                (ctl as Label).Width = (int)maxwidth;
                            }
                        }
                    }
                }
            }
        }

        public UCLogicTableView(DBSource dbSource, bool samedb, string dbname, string tbname, int logicMapId, Func<List<Tuple<string, string>>> funcLoadTables, Action<UCLogicTableView> onComplete,
            Func<Point, Point, bool, bool> checkConflict)
        {
            InitializeComponent();
            //this.AutoSize = true;

            LBTabname.TextChanged += (s, e) => Check();

            DBSource = dbSource;
            DBName = dbname;
            TBName = tbname;
            issamedb = samedb;

            _logicMapId = logicMapId;

            LBTabname.BackColor = Color.LightBlue;
            LBTabname.Visible = false;
            //LBTabname.AutoSize = false;
            LBTabname.Height = 20;
            LBTabname.ForeColor = Color.Blue;
            
            ColumnsPanel.Location = new Point(1, LBTabname.Height + 1);
            ColumnsPanel.Width = LBTabname.Width - 2;
            //this.ColumnsPanel.Height = Height - ColumnsPanelOutPut.Height - LBTabname.Height - 2;

            CBTables.Height = 20;
            FunLoadTables = funcLoadTables;

            OnComplete = onComplete;
            BorderStyle = BorderStyle.None;

            onCheckConflict = checkConflict;
        }


        private void CBTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBTables.SelectedIndex != -1)
            {
                TBName = CBTables.Text;
                LBTabname.Text = issamedb ? $"{TBName}" : $"[{DBName}].{TBName}";

                LBTabname.Visible = true;
                LBTabname.Location = new Point(1, 1);
                LBTabname.Width = this.Width - 2;
                CBTables.Visible = false;

                ColumnsList = SQLHelper.GetColumns(DBSource, DBName, CBTables.SelectedItem.ToString()).ToList();
                ColumnsList.ForEach(p => p.TBName = TrimTableName(TBName));
                BindColumns();
            }
        }

        public string TableName
        {
            get
            {
                return TBName;
            }
        }

        public string RpTableName
        {
            get
            {
                if (IsTempTable)
                {
                    return "临时表";
                }

                if (IsNoteTable)
                {
                    return "备注";
                }
                return TBName?.Split('*')[0];
            }
        }

        public string DataBaseName
        {
            get
            {
                return this.DBName;
            }
        }

        public string TrimTableName(string tbname)
        {
            return tbname.Split('*')[0];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(Pens.DarkOliveGreen, 0, 0, this.Width - 1, this.Height - 1);
        }

        private void BindInputColumns(IEnumerable<LogicMapRelColumn> collist,IEnumerable<LogicMapRelColumn> relcollist)
        {
            this.CBCoumns.Items.Clear();

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
                        AddColumnLable(ColumnsPanel,CBCoumns,col, ref logicMapRelColumns);
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
                        AddColumnLable(ColumnsPanel, CBCoumns, col, ref logicMapRelColumns);
                    }
                }

            }
        }

        private void BindOutputColumns(IEnumerable<LogicMapRelColumn> collist, IEnumerable<LogicMapRelColumn> relcollist)
        {
            this.CBCoumnsOutput.Items.Clear();

            this.CBCoumnsOutput.Items.AddRange(ColumnsList.Where(p => !collist.Any(q => q.ColName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
            && !relcollist.Any(q =>q.RelColName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))).ToArray());

            this.CBCoumnsOutput.SelectedIndex = -1;
            this.CBCoumnsOutput.Visible = true;
            this.CBCoumnsOutput.DisplayMember = "Name";
            this.CBCoumnsOutput.SelectedIndexChanged += CBCoumnsOutput_SelectedIndexChanged; ;

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
                        AddColumnLable(ColumnsPanelOutPut,CBCoumnsOutput,col, ref logicMapRelColumns);
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
                        AddColumnLable(ColumnsPanelOutPut, CBCoumnsOutput, col, ref logicMapRelColumns);
                    }
                }

            }
        }

        private void CBCoumnsOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBCoumnsOutput.SelectedIndex != -1)
            {
                List<LogicMapRelColumn> logicMapRelColumns = null;
                AddColumnLable(ColumnsPanelOutPut, CBCoumnsOutput, (TBColumn)CBCoumnsOutput.SelectedItem, ref logicMapRelColumns);
                var selectedindex = CBCoumnsOutput.SelectedIndex;
                CBCoumnsOutput.SelectedIndex = -1;
                CBCoumnsOutput.Items.RemoveAt(selectedindex);
            }
        }

        private void BindColumns()
        {
            if(IsNoteTable)
            {
                DrawAble(NoteTextBox, ColumnsPanel, false);
            }
            else
            {
                var collist = BigEntityTableRemotingEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn),
                   "LSDTC", new object[] { this._logicMapId, DBName.ToLower(), this.TBName.ToLower() },
                   new object[] { this._logicMapId, DBName.ToLower(), this.TBName.ToLower() }, 1, int.MaxValue);

                var relcollist = BigEntityTableRemotingEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn),
                    "LSDRTC", new object[] { this._logicMapId, this.DBName.ToLower(), this.TBName.ToLower() },
                    new object[] { this._logicMapId, this.DBName.ToLower(), this.TBName.ToLower() }, 1, int.MaxValue);

                BindInputColumns(collist.Where(p => !p.IsOutPut), relcollist.Where(p => !p.ReIsOutPut));

                BindOutputColumns(collist.Where(p => p.IsOutPut), relcollist.Where(p => p.ReIsOutPut));
            }

            if (OnComplete != null)
            {
                OnComplete(this);
            }
        }

        public Rectangle FindTBColumnScreenRect(string colname,bool isOutPut)
        {
            if (IsNoteTable)
            {
                if (colname.Equals("Text", StringComparison.OrdinalIgnoreCase))
                {
                    return NoteTextBox.Parent.RectangleToScreen(NoteTextBox.Bounds);
                }
                return Rectangle.Empty;
            }
            var pannel = isOutPut ? ColumnsPanelOutPut : ColumnsPanel;
            foreach (Control lb in pannel.Controls)
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

        private void DrawAble(Control lb,Panel panel,bool isOutPut)
        {
            bool isDraging = false;
            Point dragStart = Point.Empty;
            Point dragEnd = Point.Empty;
            List<Point> points = null;
            lb.MouseMove += (s, ee) =>
            {
                if (ee.Button == MouseButtons.Left)
                {
                    var parentpannel = this.Parent as Panel;
                    var pp = parentpannel.PointToClient(lb.PointToScreen(ee.Location));
                    pp.Offset(-parentpannel.AutoScrollPosition.X, -parentpannel.AutoScrollPosition.Y);

                    //Util.SendMsg(this, pp.Y + " " + parentpannel.AutoScrollPosition.Y);
                    if (pp.Y + parentpannel.AutoScrollPosition.Y <= 0)
                    {
                        if (parentpannel.VerticalScroll.Value + pp.Y + parentpannel.AutoScrollPosition.Y > parentpannel.VerticalScroll.Minimum)
                        {
                            parentpannel.VerticalScroll.Value += pp.Y + parentpannel.AutoScrollPosition.Y;
                        }
                    }

                    if (pp.Y >= parentpannel.Height - parentpannel.AutoScrollPosition.Y)
                    {
                        if (parentpannel.VerticalScroll.Value < parentpannel.VerticalScroll.Maximum)
                        {
                            parentpannel.VerticalScroll.Value += 50;
                        }
                    }

                    if (pp.X >= parentpannel.Width - parentpannel.AutoScrollPosition.X)
                    {
                        if (parentpannel.HorizontalScroll.Value < parentpannel.HorizontalScroll.Maximum)
                        {
                            parentpannel.HorizontalScroll.Value += 50;
                        }
                    }

                    //Util.SendMsg(this, pp.X + " " + parentpannel.AutoScrollPosition.X);
                    if (pp.X + parentpannel.AutoScrollPosition.X <= 0)
                    {
                        if (parentpannel.HorizontalScroll.Value + pp.X + parentpannel.AutoScrollPosition.X > parentpannel.HorizontalScroll.Minimum)
                        {
                            parentpannel.HorizontalScroll.Value += pp.X + parentpannel.AutoScrollPosition.X;
                        }
                    }


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
                        Point start = this.Parent.PointToClient(panel.PointToScreen(new Point(lb.Location.X + lb.Width, lb.Location.Y + lb.Height / 2)));
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
                                LogicID = _logicMapId,
                                IsOutPut = isOutPut,
                                ReIsOutPut = col.IsOutPut
                            };
                            var relcollist = BigEntityTableRemotingEngine
                            .Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LSDTC", new object[] { newrelcolumn.LogicID, newrelcolumn.DBName, newrelcolumn.TBName },
                            new object[] { newrelcolumn.LogicID, newrelcolumn.DBName, newrelcolumn.TBName }, 1, int.MaxValue);

                            if (!relcollist.Any(p => p.IsOutPut == newrelcolumn.IsOutPut && p.ReIsOutPut == newrelcolumn.ReIsOutPut && p.RelDBName.ToLower() == newrelcolumn.RelDBName && p.RelTBName.ToLower() == newrelcolumn.RelTBName && p.RelColName.ToLower() == newrelcolumn.RelColName))
                            {
                                BigEntityTableRemotingEngine.Insert(nameof(LogicMapRelColumn), newrelcolumn);
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
                                using (var g = Parent.CreateGraphics())
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

                        Parent.Invalidate();

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

        private void ClearColumnLable()
        {
            List<Control> remlist = new List<Control>();
            foreach (Control ctl in ColumnsPanel.Controls)
            {
                if (ctl.Tag is TBColumn)
                {
                    remlist.Add(ctl);
                }
            }
            if (remlist.Count > 0)
            {
                CBCoumns.Location = remlist[0].Location;
            }
            foreach (var ctl in remlist)
            {
                ColumnsPanel.Controls.Remove(ctl);
            }

            remlist.Clear();
            foreach (Control ctl in ColumnsPanelOutPut.Controls)
            {
                if (ctl.Tag is TBColumn)
                {
                    remlist.Add(ctl);
                }
            }
            if (remlist.Count > 0)
            {
                CBCoumnsOutput.Location = remlist[0].Location;
            }
            foreach (var ctl in remlist)
            {
                ColumnsPanelOutPut.Controls.Remove(ctl);
            }
        }

        private void AddColumnLable(Panel panel,ComboBox cbCol, TBColumn tbcolumn, ref List<LogicMapRelColumn> logicMapRelColumns)
        {
            var tbcol = tbcolumn.Clone() as TBColumn;
            var isOutPut = panel == ColumnsPanelOutPut;
            tbcol.IsOutPut = isOutPut;

            if (panel != ColumnsPanelOutPut && panel.Location.Y + panel.Height >= ColumnsPanelOutPut.Location.Y)
            {
                var newPos = ColumnsPanelOutPut.Location;
                newPos.Offset(0, panel.Location.Y + panel.Height - ColumnsPanelOutPut.Location.Y + 10);
                this.groupBox1.Location = new Point(0, newPos.Y - 5);
                ColumnsPanelOutPut.Location = newPos;
                this.Height = ColumnsPanelOutPut.Location.Y + ColumnsPanelOutPut.Height + 1;
            }

            var lbIdx = 0;
            foreach(Control ctl in panel.Controls)
            {
                if(ctl.Tag is TBColumn)
                {
                    lbIdx++;
                }
            }

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
            else if (tbcol.IsUnique())
            {
                lb.Image = SQLTypeRs.UNIQ;
            }
            lb.UseMnemonic = true;

            if (lbIdx % 2 == 1)
            {
                lb.BackColor = Color.LightYellow;
            }

            lb.Text = "   " + tbcol.Name;
            lb.Location = cbCol.Location;
            lb.Tag = tbcol;
            lb.Height = 20;
            var loc = lb.Location;
            loc.Offset(0, lb.Height);
            cbCol.Location = loc;
            lb.Width = this.Width - 2;
            panel.Controls.Add(lb);

            if (panel.Height < cbCol.Location.Y + cbCol.Height + 10)
            {
                panel.Height = cbCol.Location.Y + cbCol.Height + 10;

                //using (var g = this.CreateGraphics())
                //{
                //    using (var p = new Pen(this.BackColor, 1))
                //    {
                //        g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
                //    }
                //}

                if (!isOutPut && panel.Location.Y + panel.Height >= ColumnsPanelOutPut.Location.Y)
                {
                    var newPos = ColumnsPanelOutPut.Location;
                    newPos.Offset(0, panel.Location.Y + panel.Height - ColumnsPanelOutPut.Location.Y + 10);
                    ColumnsPanelOutPut.Location = newPos;
                    this.groupBox1.Location = new Point(0, newPos.Y - 5);
                }
                this.Height = ColumnsPanelOutPut.Location.Y + ColumnsPanelOutPut.Height + 1;
                this.Invalidate();
            }

            //加到库
            if (_logicMapId > 0)
            {
                long total = 0;
                if (logicMapRelColumns == null)
                {
                    logicMapRelColumns = BigEntityTableRemotingEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID",
                        new object[] { _logicMapId }, new object[] { this._logicMapId }, 1, int.MaxValue, ref total);
                }
                if (!logicMapRelColumns.Any(p => p.IsOutPut == isOutPut && p.DBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase) && p.TBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.ColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase))
                    && !logicMapRelColumns.Any(p => p.IsOutPut == isOutPut && p.RelDBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase) && p.RelTBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.RelColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    BigEntityTableRemotingEngine.Insert(nameof(LogicMapRelColumn), new LogicMapRelColumn
                    {
                        LogicID = _logicMapId,
                        ColName = tbcol.Name.ToLower(),
                        DBName = tbcol.DBName.ToLower(),
                        TBName = tbcol.TBName.ToLower(),
                        RelColName = string.Empty,
                        RelDBName = string.Empty,
                        RelTBName = string.Empty,
                        IsOutPut = isOutPut
                    });
                }
                else
                {
                    var logiccol = logicMapRelColumns.FirstOrDefault(p => p.IsOutPut == isOutPut && p.DBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase)
                      && p.TBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.ColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase)
                      && p.RelColName == string.Empty);

                    if (logiccol != null && logiccol.Desc != null)
                    {
                        lb.Text = $"   {tbcol.Name}({logiccol.Desc})";
                    }
                }
            }

            DrawAble(lb, panel, isOutPut);


            lb.DoubleClick += (s, ee) =>
            {
                if (_logicMapId > 0)
                {
                    long total = 0;
                    var allcols = BigEntityTableRemotingEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID",
                        new object[] { this._logicMapId }, new object[] { this._logicMapId }, 1, int.MaxValue, ref total);
                    var logiccol = allcols.FirstOrDefault(p => p.IsOutPut == (panel == ColumnsPanelOutPut) && p.DBName.Equals(tbcol.DBName, StringComparison.OrdinalIgnoreCase)
                      && p.TBName.Equals(tbcol.TBName, StringComparison.OrdinalIgnoreCase) && p.ColName.Equals(tbcol.Name, StringComparison.OrdinalIgnoreCase)
                      && p.RelColName == string.Empty);
                    var logiccoldesc = logiccol?.Desc ?? string.Empty;
                    var dlg = new SubForm.InputStringDlg($"逻辑备注{TrimTableName(tbcol.TBName)}.{tbcol.Name}", logiccoldesc);
                    dlg.DlgResult += () =>
                    {
                        if (logiccol != null)
                        {
                            logiccol.Desc = dlg.InputString;
                            BigEntityTableRemotingEngine.Update(nameof(LogicMapRelColumn), logiccol);
                        }
                        else
                        {
                            BigEntityTableRemotingEngine.Insert(nameof(LogicMapRelColumn), new LogicMapRelColumn
                            {
                                LogicID = _logicMapId,
                                ColName = tbcol.Name.ToLower(),
                                DBName = tbcol.DBName.ToLower(),
                                TBName = tbcol.TBName.ToLower(),
                                RelColName = string.Empty,
                                RelDBName = string.Empty,
                                RelTBName = string.Empty,
                                Desc = dlg.InputString,
                                IsOutPut=panel==ColumnsPanelOutPut
                            });
                        }
                        lb.Text = $"   {tbcol.Name}({dlg.InputString})";
                    };
                    
                    dlg.ShowMe(Util.FindParent<TabPage>(this));
                }
            };

            lb.TextChanged += (s, e) => Check();

            lb.MouseHover += (s, e) =>
            {
                var col = (lb.Tag as TBColumn);
                var desc = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys",
                    new[] { col.DBName.ToUpper(),TrimTableName(col.TBName).ToUpper(), col.Name.ToUpper() }).FirstOrDefault();
                if (desc != null)
                {
                    Util.SendMsg(this, desc.MarkInfo);
                }
                else
                {
                    Util.SendMsg(this, string.Empty);
                }
            };

            Check();
        }

        private void CBCoumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBCoumns.SelectedIndex != -1)
            {
                List<LogicMapRelColumn> logicMapRelColumns = null;
                AddColumnLable(ColumnsPanel,CBCoumns,(TBColumn)CBCoumns.SelectedItem,ref logicMapRelColumns);
                var selectedindex = CBCoumns.SelectedIndex;
                CBCoumns.SelectedIndex = -1;
                CBCoumns.Items.RemoveAt(selectedindex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsNoteTable)
            {
                LBTabname.Visible = true;
                LBTabname.Text = "说明";
                LBTabname.Location = new Point(1, 1);
                LBTabname.Width = Width - 2;
                groupBox1.Visible = false;
                ColumnsPanelOutPut.Visible = false;
                CBTables.Visible = false;
                CBCoumns.Visible = false;
                NoteTextBox = new TextBox();
                NoteTextBox.Location = new Point(1, 1);
                NoteTextBox.Width = Width - 2;
                NoteTextBox.Multiline = true;
                ColumnsPanel.Controls.Add(NoteTextBox);
                NoteTextBox.Dock = DockStyle.Fill;
                NoteTextBox.BorderStyle = BorderStyle.None;

                var colum = new TBColumn
                {
                    DBName = DBName,
                    Name = "Text",
                    TBName = TBName,
                    TypeName = "Varchar",
                    Length = -1
                };
                NoteTextBox.Tag = colum;

                ColumnsList = new List<TBColumn> { colum };

                var noteTable = BigEntityTableRemotingEngine.Find<TempNotesTable>(nameof(TempNotesTable), nameof(TempNotesTable.TBName), new object[] { TBName.ToUpper() }).FirstOrDefault();
                if (noteTable != null)
                {
                    NoteTextBox.Text = noteTable.Text;
                }

                NoteTextBox.LostFocus += (s, ee) =>
                {
                    noteTable = BigEntityTableRemotingEngine.Find<TempNotesTable>(nameof(TempNotesTable), nameof(TempNotesTable.TBName), new object[] { TBName.ToUpper() }).FirstOrDefault();
                    if (noteTable == null)
                    {
                        noteTable = new TempNotesTable
                        {
                            DBName = DBName.ToUpper(),
                            TBName = TBName.ToUpper(),
                            Text = NoteTextBox.Text
                        };
                        BigEntityTableRemotingEngine.Insert(nameof(TempNotesTable), noteTable);
                    }
                    else
                    {
                        if (noteTable.Text != NoteTextBox.Text)
                        {
                            noteTable.Text = NoteTextBox.Text;
                            BigEntityTableRemotingEngine.Update(nameof(TempNotesTable), noteTable);
                        }
                    }
                };

                this.LBTabname.MouseMove += OnLBTabnameMouseMove;
                this.LBTabname.MouseDown += OnLBTabnameMouseDown;
                this.LBTabname.MouseUp += OnLBTabnameMouseUp;

                BindColumns();
                this.Height = ColumnsPanel.Location.Y + ColumnsPanel.Height + 1;
            }
            else if (IsTempTable)
            {
                var temptb = BigEntityTableRemotingEngine.Find<TempTB>(nameof(TempTB), TempTB.INDEX_DB_TB, new object[] { TBName }).FirstOrDefault();
                if (temptb == null)
                {
                    return;
                }
                LBTabname.Text = $"#{temptb.DisplayName}";
                LBTabname.Visible = true;
                LBTabname.Location = new Point(1, 1);
                LBTabname.Width = Width - 2;
                CBTables.Visible = false;
                var cols = BigEntityTableRemotingEngine.Find<TempTBColumn>(nameof(TempTBColumn), nameof(TempTBColumn.TBId), new object[] { temptb.Id }).ToList();

                ColumnsList = cols.Select(p => new TBColumn
                {
                    DBName=DBName,
                    TBName=TBName,
                    Name=p.Name,
                    TypeName=p.TypeName
                }).ToList();
                BindColumns();

                this.LBTabname.MouseMove += OnLBTabnameMouseMove;
                this.LBTabname.MouseDown += OnLBTabnameMouseDown;
                this.LBTabname.MouseUp += OnLBTabnameMouseUp;
                this.LBTabname.DoubleClick += LBTabname_DoubleClick;
                this.LBTabname.MouseHover += (ss, ee) =>
                {
                    if (!string.IsNullOrWhiteSpace(DBName) && !string.IsNullOrWhiteSpace(TBName))
                    {
                        Util.SendMsg(this, "临时表");
                    }
                };

                ColumnsPanel.DoubleClick += ColumnsPanel_DoubleClick;
                CBCoumns.Visible = false;

                ColumnsPanelOutPut.DoubleClick += ColumnsPanelOutPut_DoubleClick;
                CBCoumnsOutput.Visible = false;
            }
            else
            {

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
                    var realtbname = TrimTableName(TBName);
                    LBTabname.Text = issamedb ? $"{realtbname}" : $"[{DBName}].{realtbname}";
                    LBTabname.Visible = true;
                    LBTabname.Location = new Point(1, 1);
                    LBTabname.Width = Width - 2;
                    CBTables.Visible = false;
                    ColumnsList = SQLHelper.GetTBOrViewColumns(DBSource, DBName, realtbname).ToList();
                    if (TBName.IndexOf('*') > -1)
                    {
                        ColumnsList.ForEach(p => p.TBName = this.TBName);
                    }
                    BindColumns();
                }

                this.LBTabname.MouseMove += OnLBTabnameMouseMove;
                this.LBTabname.MouseDown += OnLBTabnameMouseDown;
                this.LBTabname.MouseUp += OnLBTabnameMouseUp;
                this.LBTabname.DoubleClick += LBTabname_DoubleClick;
                this.LBTabname.MouseHover += (ss, ee) =>
                {
                    if (!string.IsNullOrWhiteSpace(DBName) && !string.IsNullOrWhiteSpace(TBName))
                    {
                        var desc = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { DBName.ToUpper(),TrimTableName(TBName).ToUpper(), string.Empty }).FirstOrDefault();
                        if (desc != null)
                        {
                            Util.SendMsg(this, desc.MarkInfo);
                        }
                        else
                        {
                            Util.SendMsg(this, string.Empty);
                        }
                    }
                };

                ColumnsPanel.DoubleClick += ColumnsPanel_DoubleClick;
                CBCoumns.Visible = false;

                ColumnsPanelOutPut.DoubleClick += ColumnsPanelOutPut_DoubleClick;
                CBCoumnsOutput.Visible = false;
            }
        }

        private void ColumnsPanelOutPut_DoubleClick(object sender, EventArgs e)
        {
            CBCoumnsOutput.Visible = !CBCoumnsOutput.Visible;
        }

        private void ColumnsPanel_DoubleClick(object sender, EventArgs e)
        {
            CBCoumns.Visible = !CBCoumns.Visible;
        }

        private void LBTabname_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TBName))
            {
                this.LBTabname.Visible = false;
                this.CBTables.Visible = true;
            }

            if (IsTempTable)
            {
                var dlg = new SubForm.AddTempTableDlg(DBSource, TBName);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ColumnsList = dlg.TempTBColumns.Select(p => new TBColumn
                    {
                        DBName = DBName,
                        TBName = TBName,
                        Name = p.Name,
                        TypeName = p.TypeName
                    }).ToList();
                    LBTabname.Text = "#"+dlg.TempTB.DisplayName;
                    ClearColumnLable();
                    BindColumns();
                }
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

                LogicMapTable tb = BigEntityTableRemotingEngine.Find<LogicMapTable>(nameof(LogicMapTable), this.LogicMapTableId);
                
                tb.Posx = pt.X;
                tb.Posy = pt.Y;

                BigEntityTableRemotingEngine.Update(nameof(LogicMapTable), tb);
            }
        }
    }
}
