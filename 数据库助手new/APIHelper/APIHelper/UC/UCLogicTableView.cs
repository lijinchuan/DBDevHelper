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
using Biz.Common;
using APIHelper.Resources;
using System.Drawing.Drawing2D;
using APIHelper.Drawing;
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace APIHelper.UC
{
    public partial class UCLogicTableView : UserControl
    {
        private int _logicMapId = 0;
        APISource APISource = null;
        APIUrl API = null;
        bool issamedb = true;

        public int LogicMapTableId
        {
            get;
            set;
        }

        private List<APIParamEx> ColumnsList = null;

        private Func<List<Tuple<int, APIUrl>>> FunLoadTables = null;

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

        public void Check()
        {
            if (!string.IsNullOrWhiteSpace(LBTabname.Text))
            {
                var isoverflow = false;
                var maxwidth = this.Width * 1f;
                using (var g = this.CreateGraphics())
                {
                    var size = g.MeasureString(LBTabname.Text, this.LBTabname.Font);
                    if (size.Width > this.LBTabname.Width)
                    {
                        isoverflow = true;
                        maxwidth = size.Width;
                    }
                    foreach (var ctl in this.ColumnsPanel.Controls)
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
                if (isoverflow)
                {
                    maxwidth += 1;
                    this.Width = (int)maxwidth + 2;
                    this.LBTabname.Width = (int)maxwidth;
                    this.ColumnsPanel.Width = (int)maxwidth;
                    this.CBCoumns.Width = (int)maxwidth - 2;
                    foreach(var ctl in this.ColumnsPanel.Controls)
                    {
                        if(ctl is Label)
                        {
                            (ctl as Label).Width = (int)maxwidth;
                        }
                    }
                }
            }
        }

        public UCLogicTableView(APISource apiSource, bool samedb, APIUrl api, int logicMapId, Func<List<Tuple<int, APIUrl>>> funcLoadTables, Action<UCLogicTableView> onComplete,
            Func<Point, Point, bool, bool> checkConflict)
        {
            InitializeComponent();
            //this.AutoSize = true;

            LBTabname.TextChanged += (s, e) => Check();

            APISource = apiSource;
            API = api;
            issamedb = samedb;

            this._logicMapId = logicMapId;

            LBTabname.BackColor = Color.LightBlue;
            LBTabname.Visible = false;
            //LBTabname.AutoSize = false;
            LBTabname.Height = 20;
            LBTabname.ForeColor = Color.Blue;
            
            this.ColumnsPanel.Location = new Point(1, LBTabname.Height + 1);
            this.ColumnsPanel.Width = LBTabname.Width - 2;
            this.ColumnsPanel.Height = this.Height - this.LBTabname.Height - 2;

            this.CBTables.Height = 20;
            this.FunLoadTables = funcLoadTables;

            this.OnComplete = onComplete;
            this.BorderStyle = BorderStyle.None;

            onCheckConflict = checkConflict;

            if (api != null)
            {
                LBTabname.Text = issamedb ? $"{api.APIName}" : $"[{this.APISource.SourceName}].{api.APIName}";
            }

            CBCoumns.Visible = false;
        }


        private void CBTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBTables.SelectedIndex != -1)
            {
                var apiurl = CBTables.SelectedItem as APIUrl;
                this.API = apiurl;
                LBTabname.Text = issamedb ? $"{API.APIName}" : $"[{APISource.SourceName}].{API.APIName}";

                this.LBTabname.Visible = true;
                this.LBTabname.Location = new Point(1, 1);
                this.LBTabname.Width = this.Width - 2;
                this.CBTables.Visible = false;

                ColumnsList = BigEntityTableEngine.LocalEngine.Find<APIParam>(nameof(APIParam), "APIId", new object[] { apiurl.Id })
                    .Select(p=>
                    {
                        var item = new APIParamEx(p);
                        item.APIId = this.APIId;
                        item.APIName = this.APIName;
                        item.APISourceId = this.APISouceId;
                        item.SourceName = this.APISourceName;
                        return item;
                    }).ToList();
                //ColumnsList = SQLHelper.GetColumns(DBSource, DBName, CBTables.SelectedItem.ToString()).ToList();
                if (this.API.APIName.IndexOf('*') > -1)
                {
                    ColumnsList.ForEach(p => p.APIName = this.API.APIName);
                }
                BindColumns();
            }
        }

        public int APIId
        {
            get
            {
                if (API == null)
                {
                    return 0;
                }
                return this.API.Id;
            }
        }

        public string RpTableName
        {
            get
            {
                return this.API?.APIName.Split('*')[0];
            }
        }

        public int APISouceId
        {
            get
            {
                return this.APISource.Id;
            }
        }

        public string APISourceName
        {
            get
            {
                return this.APISource.SourceName;
            }
        }

        public string APIName
        {
            get
            {
                return this.API?.APIName;
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
                "LSDTC", new object[] { this._logicMapId, APISource.Id, this.API.Id },
                new object[] { this._logicMapId, APISource.Id, API.Id }, 1, int.MaxValue);

            var relcollist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn),
                "LSDRTC", new object[] { this._logicMapId, APISource.Id, API.Id },
                new object[] { this._logicMapId, APISource.Id, API.Id }, 1, int.MaxValue);

            this.CBCoumns.Items.AddRange(ColumnsList.Where(p => !collist.Any(q => q.APIParamName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))
            && !relcollist.Any(q => q.RelAPIParamName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))).ToArray());

            this.CBCoumns.SelectedIndex = -1;
            this.CBCoumns.Visible = true;
            this.CBCoumns.DisplayMember = "Name";
            this.CBCoumns.SelectedIndexChanged += CBCoumns_SelectedIndexChanged;

            HashSet<string> ha = new HashSet<string>();
            List<LogicMapRelColumn> logicMapRelColumns = null;
            foreach (var item in collist)
            {
                if (!ha.Contains(item.APIParamName))
                {
                    ha.Add(item.APIParamName);

                    var col = ColumnsList.FirstOrDefault(p => p.Name.Equals(item.APIParamName, StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        AddColumnLable(col,ref logicMapRelColumns);
                    }
                }

            }

            foreach (var item in relcollist)
            {
                if (!ha.Contains(item.RelAPIParamName))
                {
                    ha.Add(item.RelAPIParamName);

                    var col = ColumnsList.FirstOrDefault(p => p.Name.Equals(item.RelAPIParamName, StringComparison.OrdinalIgnoreCase));
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
                    if (tag is APIParamEx)
                    {
                        if ((tag as APIParamEx).Name.Equals(colname, StringComparison.OrdinalIgnoreCase))
                        {
                            return lb.Parent.RectangleToScreen(lb.Bounds);
                        }
                    }
                }
            }

            return Rectangle.Empty;
        }

        private void AddColumnLable(APIParamEx tbcol, ref List<LogicMapRelColumn> logicMapRelColumns)
        {
            var lb = new Label();
            lb.AutoSize = false;
            lb.ImageAlign = ContentAlignment.MiddleLeft;
            lb.Image = SQLTypeRs.CHAR;
            //if (tbcol.IsString())
            //{
            //    lb.Image = SQLTypeRs.CHAR;
            //}
            //else if (tbcol.IsDateTime())
            //{
            //    lb.Image = SQLTypeRs.DATE;
            //}
            //else if (tbcol.IsNumber())
            //{
            //    lb.Image = SQLTypeRs.NUMBER;
            //}
            //else if (tbcol.IsBoolean())
            //{
            //    lb.Image = SQLTypeRs.BOOL;
            //}
            //else if (tbcol.IsUnique())
            //{
            //    lb.Image = SQLTypeRs.UNIQ;
            //}
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
                if (!logicMapRelColumns.Any(p => p.APISourceId==tbcol.APISourceId && p.APIId==tbcol.APIId && p.APIParamId==tbcol.Id)
                    && !logicMapRelColumns.Any(p => p.RelAPIResourceId==tbcol.APISourceId && p.RelAPIId==tbcol.APIId && p.RelAPIParamId==tbcol.Id))
                {
                    BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapRelColumn), new LogicMapRelColumn
                    {
                        LogicID = _logicMapId,
                        APIParamId=tbcol.Id,
                        APIParamName = tbcol.Name.ToLower(),
                        APISourceId=tbcol.APISourceId,
                        SourceName=tbcol.SourceName.ToLower(),
                        APIId=tbcol.APIId,
                        APIName=tbcol.APIName.ToLower(),
                        RelAPIParamId=0,
                        RelAPIParamName=string.Empty,
                        RelAPIResourceName = string.Empty,
                        RelAPIName = string.Empty
                    });
                }
                else
                {
                    var logiccol = logicMapRelColumns.FirstOrDefault(p => p.APISourceId==tbcol.APISourceId
                      && p.APIId==tbcol.APIId&& p.APIParamId==tbcol.Id
                      && p.RelAPIParamName == string.Empty);

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
                                APIParamId=(lb.Tag as APIParamEx).Id,
                                APIParamName = (lb.Tag as APIParamEx).Name.ToLower(),
                                APISourceId=this.APISource.Id,
                                SourceName = this.APISource.SourceName.ToLower(),
                                RelAPIParamId=col.Id,
                                RelAPIParamName = col.Name.ToLower(),
                                RelAPIResourceId=col.APISourceId,
                                RelAPIResourceName = col.SourceName.ToLower(),
                                RelAPIId=col.APIId,
                                RelAPIName = col.APIName.ToLower(),
                                APIId=this.API.Id,
                                APIName = this.API.APIName.ToLower(),
                                LogicID = _logicMapId
                            };
                            var relcollist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine
                            .Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LSDTC", new object[] { newrelcolumn.LogicID, newrelcolumn.APISourceId, newrelcolumn.APIId },
                            new object[] { newrelcolumn.LogicID, newrelcolumn.APISourceId, newrelcolumn.APIId }, 1, int.MaxValue);

                            if (!relcollist.Any(p => p.RelAPIResourceId == newrelcolumn.RelAPIResourceId && p.RelAPIId == newrelcolumn.RelAPIId && p.RelAPIParamId == newrelcolumn.RelAPIParamId))
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


            lb.DoubleClick += (s, ee) =>
            {
                if (_logicMapId > 0)
                {
                    long total = 0;
                    var allcols = BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID",
                        new object[] { this._logicMapId }, new object[] { this._logicMapId }, 1, int.MaxValue, ref total);
                    var logiccol = allcols.FirstOrDefault(p => p.APISourceId==tbcol.APISourceId
                      && p.APIId==tbcol.APIId && p.APIParamId==tbcol.Id
                      && p.RelAPIParamId == 0);
                    var logiccoldesc = logiccol?.Desc ?? string.Empty;
                    var dlg = new SubForm.InputStringDlg($"逻辑备注{tbcol.APIName.Split('*')[0]}.{tbcol.Name}", logiccoldesc);
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
                                APIParamId=tbcol.Id,
                                APIParamName = tbcol.Name.ToLower(),
                                APISourceId=tbcol.APISourceId,
                                SourceName = tbcol.SourceName.ToLower(),
                                APIId=tbcol.APIId,
                                APIName = tbcol.Name.ToLower(),
                                RelAPIParamName = string.Empty,
                                RelAPIResourceName = string.Empty,
                                RelAPIName = string.Empty,
                                Desc = dlg.InputString
                            });
                        }
                        lb.Text = $"   {tbcol.Name}({dlg.InputString})";
                    }
                }
            };

            lb.TextChanged += (s, e) => Check();

            lb.MouseHover += (s, e) =>
            {
                var col = (lb.Tag as APIParamEx);
                var desc = col.Desc;
                if (desc != null)
                {
                    Util.SendMsg(this, desc);
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
                AddColumnLable((APIParamEx)CBCoumns.SelectedItem,ref logicMapRelColumns);
                var selectedindex = CBCoumns.SelectedIndex;
                CBCoumns.SelectedIndex = -1;
                CBCoumns.Items.RemoveAt(selectedindex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (API==null)
            {
                CBTables.DataSource = FunLoadTables().Select(p => p.Item2).OrderBy(p => p).ToList();
                CBTables.DisplayMember = "APIName";
                CBTables.ValueMember = "Id";
                CBTables.SelectedIndex = -1;
                this.LBTabname.Visible = false;
                this.CBTables.Visible = true;
                this.CBTables.Location = new Point(1, 1);
                this.CBTables.Width = this.Width - 2;
                this.CBTables.SelectedIndexChanged += CBTables_SelectedIndexChanged;
            }
            else
            {
                var realtbname = API.APIName.Split('*')[0];
                this.LBTabname.Text = issamedb ? $"{realtbname}" : $"[{APISource.SourceName}].{realtbname}";
                this.LBTabname.Visible = true;
                this.LBTabname.Location = new Point(1, 1);
                this.LBTabname.Width = this.Width - 2;
                this.CBTables.Visible = false;
                ColumnsList = BigEntityTableEngine.LocalEngine.Find<APIParam>(nameof(APIParam), "APIId", new object[] { API.Id })
                    .Select(p =>
                    {
                        var item = new APIParamEx(p);
                        item.APIId = this.APIId;
                        item.APIName = this.APIName;
                        item.APISourceId = this.APISouceId;
                        item.SourceName = this.APISourceName;
                        return item;
                    }).ToList();
                //ColumnsList = SQLHelper.GetColumns(DBSource, DBName, CBTables.SelectedItem.ToString()).ToList();
                if (this.API.APIName.IndexOf('*') > -1)
                {
                    ColumnsList.ForEach(p => p.APIName = this.API.APIName);
                }
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
            if (API==null)
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
