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
using APIHelper.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace APIHelper.UC
{
    public partial class UCLogicMap : TabPage
    {
        private int _logicMapId = 0;
        APISource APISource = null;
        string _DBName = null;

        static Color[] colors = new Color[] { Color.FromArgb(255,0x19,0x7f,0x96),
            Color.Red, Color.Green, Color.FromArgb(255,0x9d,0x1d,0x60),
            Color.FromArgb(255,0x07,0x52,0x96),
            Color.FromArgb(255,0x41,0x1f,0x98),
            Color.Black,Color.Brown,Color.Chocolate};

        private List<UCLogicTableView> ucTableViews = new List<UCLogicTableView>();

        Dictionary<int, List<APIUrl>> tableColumnList = null;

        List<LogicMapRelColumnEx> relColumnIces = null;

        bool hashotline = false;

        public UCLogicMap()
        {
            InitializeComponent();
        }

        public UCLogicMap(APISource apisource, int logicMapId)
        {
            var logicmap = BigEntityTableEngine.LocalEngine.Find<LogicMap>(nameof(LogicMap), logicMapId);
            if (logicmap == null)
            {

                return;
            }

            InitializeComponent();
            this.APISource = apisource;
            this._DBName = logicmap.SourceName;

            this._logicMapId = logicMapId;

            this.PanelMap.ContextMenuStrip = this.CMSOpMenu;
            this.PanelMap.AutoScroll = true;
            this.PanelMap.ControlAdded += PanelMap_ControlAdded;
            this.PanelMap.ControlRemoved += PanelMap_ControlRemoved;
            this.PanelMap.MouseClick += PanelMap_MouseClick;
            this.PanelMap.MouseDoubleClick += PanelMap_MouseDoubleClick;

            tableColumnList = new Dictionary<int, List<APIUrl>>();

            var tbs= BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), "SourceId", new object[] { this.APISource.Id});
            //var tbs = SQLHelper.GetTBs(this.DBSource, logicmap.DBName);
            tableColumnList.Add(logicmap.APISourceId, tbs.Select(p => p).ToList());

            this.CMSOpMenu.VisibleChanged += CMSOpMenu_VisibleChanged;
            TSMDelRelColumn.DropDownItemClicked += TSMDelRelColumn_DropDownItemClicked;
            TSMDelRelColumn.Click += TSMDelRelColumn_Click;
            TSMI_Export.Click += TSMI_Export_Click;
            delStripMenuItem.Click += delStripMenuItem_Click;
            TSMI_CopyTableName.Click += TSMI_CopyTableName_Click;
            TSMI_CopyColName.Click += TSMI_CopyColName_Click;
            TSMI_CopyTable.Click += TSMI_CopyTable_Click;
        }

        private void TSMI_CopyTable_Click(object sender, EventArgs e)
        {
            var ct = this.PanelMap.GetChildAtPoint(this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top)));
            if (ct is UCLogicTableView)
            {
                var view = ((UCLogicTableView)ct);
                if (!string.IsNullOrWhiteSpace(view.RpTableName))
                {
                    string name2 = string.Empty;
                    int i = 2;
                    for(; i < 1000; i++)
                    {
                        name2 = view.RpTableName + "*" + i;
                        if(ucTableViews.Any(p=>p.APISouceId==view.APISouceId
                        && p.APIName.Equals(name2, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (i < 1000)
                    {
                        var apiurl = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), view.APIId);
                        apiurl.APIName = name2;
                        var source = BigEntityTableEngine.LocalEngine.Find<APISource>(nameof(APISource), view.APISouceId);
                        AddTable(source, apiurl, false);
                    }
                }
            }
        }

        private void PanelMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (hashotline)
            {
                var pt = e.Location;
                var relColumnEx = FindHotLine(pt);
                if (relColumnEx != null)
                {
                    //Util.SendMsg(this, $"[{relColumnEx.RelColumn.DBName}].[{relColumnEx.RelColumn.TBName}].[{relColumnEx.RelColumn.ColName}] -> [{relColumnEx.RelColumn.RelDBName}].[{relColumnEx.RelColumn.RelTBName}].[{relColumnEx.RelColumn.RelColName}]:{relColumnEx.RelColumn.Desc}");
                    SubForm.InputStringDlg dlg = new SubForm.InputStringDlg($"输入{relColumnEx.RelColumn.APIName.Split('*')[0]}.{relColumnEx.RelColumn.APIParamName}和{relColumnEx.RelColumn.RelAPIName.Split('*')[0]}.{relColumnEx.RelColumn.RelAPIParamName}关系描述", relColumnEx.RelColumn.Desc ?? string.Empty);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        relColumnEx.RelColumn.Desc = dlg.InputString;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Update<LogicMapRelColumn>(nameof(LogicMapRelColumn), relColumnEx.RelColumn);
                        this.PanelMap.Invalidate();
                    }
                }
            }
        }

        private LogicMapRelColumnEx FindHotLine(Point ptOnPanelMap)
        {
            var pt = ptOnPanelMap;
            LogicMapRelColumnEx relColumnEx = null;
            pt.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
            foreach (var item in relColumnIces)
            {

                if (item.LinkLines != null && item.LinkLines.Length > 0)
                {
                    for (var i = 1; i < item.LinkLines.Length - 1; i++)
                    {
                        if (DrawingUtil.HasIntersect(new Rectangle(pt.X - 1, pt.Y - 1, 3, 3), new Line(item.LinkLines[i - 1], item.LinkLines[i])))
                        {
                            //points = item.LinkLines;
                            relColumnEx = item;
                            break;
                        }
                    }
                }
                if (relColumnEx != null)
                {
                    break;
                }
            }

            foreach (var item in relColumnIces)
            {
                item.IsHotLine = false;
            }

            if (relColumnEx != null)
            {
                relColumnEx.IsHotLine = true;
            }

            return relColumnEx;
        }

        private void PanelMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (relColumnIces != null)
            {
                var pt = e.Location;
                LogicMapRelColumnEx relColumnEx = FindHotLine(pt);

                if (relColumnEx != null)
                {
                    relColumnEx.IsHotLine = true;
                    hashotline = true;
                    //Util.SendMsg(this, string.Join(",", relColumnEx.LinkLines.Select(p => p.X + " " + p.Y)));
                    Util.SendMsg(this, $"[{relColumnEx.RelColumn.SourceName}].[{relColumnEx.RelColumn.APIName}].[{relColumnEx.RelColumn.APIParamName}] -> [{relColumnEx.RelColumn.RelAPIResourceName}].[{relColumnEx.RelColumn.RelAPIName}].[{relColumnEx.RelColumn.RelAPIParamName}]:{relColumnEx.RelColumn.Desc}");

                    using (var g = this.PanelMap.CreateGraphics())
                    {
                        g.TranslateTransform(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                        using (var p = new Pen(relColumnEx.LineColor, 2))
                        {
                            var points = relColumnEx.LinkLines;
                            for (var i = 1; i < points.Length; i++)
                            {
                                if (i != points.Length - 1)
                                {
                                    //g.DrawPie(p, new Rectangle(points[i].X, points[i].Y, 3, 3), 0, 360);
                                }
                                else
                                {
                                    AdjustableArrowCap arrowCap = new AdjustableArrowCap(p.Width * 2 + 1, p.Width + 2 + 1, true);
                                    p.CustomEndCap = arrowCap;
                                }
                                g.DrawLine(p, points[i - 1], points[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (hashotline)
                    {
                        hashotline = false;
                        this.PanelMap.Invalidate();
                    }
                }
            }

        }

        private int GetDrawWidth()
        {
            return Math.Max(this.PanelMap.Width, this.PanelMap.HorizontalScroll.Maximum);
        }

        private int GetDrawHeight()
        {
            return Math.Max(this.PanelMap.Height, this.PanelMap.VerticalScroll.Maximum);
        }

        private void CMSOpMenu_VisibleChanged(object sender, EventArgs e)
        {

            if (CMSOpMenu.Visible)
            {
                TSMDelRelColumn.Enabled = false;
                var location = new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top);
                if (hashotline)
                {
                    var relColumnEx = this.FindHotLine(this.PanelMap.PointToClient(location));
                    if (relColumnEx != null)
                    {
                        var ts = new ToolStripMenuItem();
                        ts.Text = $"{relColumnEx.RelColumn.RelAPIResourceName}.{relColumnEx.RelColumn.RelAPIName}.{relColumnEx.RelColumn.RelAPIParamName}";
                        ts.Tag = relColumnEx.RelColumn;
                        TSMDelRelColumn.DropDownItems.Add(ts);
                    }
                    TSMDelRelColumn.Enabled = TSMDelRelColumn.DropDownItems.Count > 0;
                }
                else
                {
                    //location.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
                    var col = this.FindColumn(location);
                    if (col != null)
                    {
                        TSMDelRelColumn.Enabled = true;
                    }
                }


                var ct = this.PanelMap.GetChildAtPoint(this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top)));
                delStripMenuItem.Enabled = TSMI_CopyTableName.Enabled = TSMI_CopyTable.Enabled = ct is UCLogicTableView;
                TSMI_CopyColName.Enabled = FindColumn(location) != null;
                添加表ToolStripMenuItem.Enabled = !TSMI_CopyTableName.Enabled;
            }
            else
            {
                TSMDelRelColumn.DropDownItems.Clear();
            }
        }

        private void TSMDelRelColumn_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag is LogicMapRelColumn)
            {
                var rc = e.ClickedItem.Tag as LogicMapRelColumn;
                if (MessageBox.Show("是否要删除关联", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (BigEntityTableEngine.LocalEngine.Delete<LogicMapRelColumn>(nameof(LogicMapRelColumn),
                        rc.ID))
                    {
                        TSMDelRelColumn.DropDownItems.Remove(e.ClickedItem);
                        this.relColumnIces.RemoveAll(p => p.RelColumn.ID == ((LogicMapRelColumn)e.ClickedItem.Tag).ID);
                    }
                    this.PanelMap.Invalidate();
                }
            }
        }

        private void TSMDelRelColumn_Click(object sender, EventArgs e)
        {
            if (TSMDelRelColumn.DropDownItems.Count > 0)
            {
                return;
            }

            var location = new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top);
            //location.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);

            var col = this.FindColumn(location);
            if (col != null)
            {
                this.CMSOpMenu.Visible = false;
                if (MessageBox.Show($"要删除字段【{col.Name}】吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    long total = 0;
                    var logiccollist= BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID",
                        new object[] { _logicMapId }, new object[] { _logicMapId }, 1, int.MaxValue, ref total).ToList();
                    var logiccol = logiccollist.FirstOrDefault(p => p.APISourceId==col.APISourceId
                      && p.APIId==col.APIId
                      && p.APIParamId==col.Id && p.RelAPIParamName == string.Empty);
                    if (logiccol != null)
                    {
                        BigEntityTableEngine.LocalEngine.Delete<LogicMapRelColumn>(nameof(LogicMapRelColumn), logiccol.ID);
                        Util.SendMsg(this, $"操作成功");
                    }
                    else
                    {
                        Util.SendMsg(this, $"未作任何操作");
                    }
                }
            }
        }

        public void Load()
        {
            var dbs = BigEntityTableEngine.LocalEngine.List<APISource>(nameof(APISource), 1, int.MaxValue).ToList();
            //var dbs = SQLHelper.GetDBs(this.DBSource);
            foreach (var db in dbs)
            {
                var ts = 添加表ToolStripMenuItem.DropDownItems.Add(db.SourceName);
                ts.Tag = db;
            }
            添加表ToolStripMenuItem.DropDownItemClicked += 添加表ToolStripMenuItem_DropDownItemClicked;
            添加表ToolStripMenuItem.Click += 添加表ToolStripMenuItem_Click;

            List<Tuple<int, int>> reltblist = new List<Tuple<int, int>>();

            var list = BigEntityTableEngine.LocalEngine.Find<LogicMapTable>(nameof(LogicMapTable),
                p =>
                {
                    return p.LogicID == this._logicMapId;
                }).ToList();
            reltblist.AddRange(list.Select(p => new Tuple<int, int>(p.APIResourceId, p.APIId)));

            var allrelcolumnlist = BigEntityTableEngine.LocalEngine.Find<LogicMapRelColumn>(nameof(LogicMapRelColumn), r =>
            {
                return r.LogicID == this._logicMapId;
            }).ToList();

            reltblist.AddRange(allrelcolumnlist.Select(p => new Tuple<int, int>(p.APISourceId, p.APIId))) ;
            reltblist.AddRange(allrelcolumnlist.Where(p => !string.IsNullOrEmpty(p.RelAPIParamName)).Select(p => new Tuple<int, int>(p.RelAPIResourceId, p.RelAPIId)));

            reltblist = reltblist.Distinct().ToList();

            foreach (var item in reltblist)
            {
                var tbleinfo = list.Find(p => p.APIResourceId==item.Item1 && p.APIId==item.Item2);
                if (tbleinfo == null)
                {
                    continue;
                }
                var apiurl=BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), item.Item2);
                UCLogicTableView tv = new UCLogicTableView(dbs.Find(p=>p.Id==item.Item1), this.APISource.Id==item.Item1, apiurl,this._logicMapId, () =>
                {
                    var tblist = new List<Tuple<int, int>>();
                    foreach (var tb in ucTableViews)
                    {
                        if (tb.APIId!=0)
                        {
                            tblist.Add(Tuple.Create<int, int>(tb.APISouceId, tb.APIId));
                        }
                    }

                    if (!tableColumnList.ContainsKey(item.Item1))
                    {
                        //var tbs = SQLHelper.GetTBs(this.DBSource, item.Item1);
                        var tbs = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), "SourceId", new object[] { this.APISource.Id }).ToList();
                        tableColumnList.Add(item.Item1, tbs);
                    }

                    return tableColumnList[item.Item1].Select(p => new Tuple<int, APIUrl>(item.Item1, p)).Where(p => !tblist.Contains(new Tuple<int,int>(p.Item1,p.Item2.Id))).OrderBy(p => p).ToList();
                }, v =>
                {
                    AdjustLoaction(v);
                },
                  Check);
                tv.OnAddNewRelColumn = c =>
                {
                    this.relColumnIces.Add(new LogicMapRelColumnEx() { RelColumn = c });
                    this.PanelMap.Invalidate();
                };
                tv.Location = new Point(tbleinfo.Posx, tbleinfo.Posy);
                tv.LogicMapTableId = tbleinfo.ID;
                this.PanelMap.Controls.Add(tv);
            }
            this.DoubleBuffered = true;
            this.PanelMap.Paint += PanelMap_Paint;

        }

        private void 添加表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CMSOpMenu.Visible = false;
            AddTable(this.APISource,null);
        }

        private void 添加表ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            AddTable(e.ClickedItem.Tag as APISource, null);
        }

        private void PanelMap_Paint(object sender, PaintEventArgs e)
        {
            DrawLinkLine(this.PanelMap, e.Graphics);
        }

        private Point GetMax(Point p1, Point p2)
        {
            if (p1.X < p2.X)
            {
                return p2;
            }
            if (p1.X > p2.X)
            {
                return p1;
            }
            if (p1.Y < p2.Y)
            {
                return p2;
            }
            return p1;
        }

        private Point GetMin(Point p1, Point p2)
        {
            if (p1.X > p2.X)
            {
                return p2;
            }
            if (p1.X < p2.X)
            {
                return p1;
            }
            if (p1.Y > p2.Y)
            {
                return p2;
            }
            return p1;
        }

        private void DrawLinkLine(Control parent, Graphics g)
        {
            //g.TranslateTransform(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
            bool haserror = false;
            if (relColumnIces == null)
            {
                lock (this)
                {
                    if (relColumnIces == null)
                    {
                        relColumnIces = new List<LogicMapRelColumnEx>();
                        var othertables = ucTableViews.Where(p => p.APIId>0).Select(p => new Tuple<int, int>(p.APISouceId, p.APIId)).Distinct().ToList();
                        var allrelcolumnlist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID", new object[] { this._logicMapId },
                            new object[] { this._logicMapId }, 1, int.MaxValue);

                        foreach (var rc in allrelcolumnlist)
                        {
                            var startpt = this.FindColumnScreenStartPoint(rc.APISourceId, rc.APIId, rc.APIParamName);
                            var destpt = this.FindColumnScreenEndPoint(rc.RelAPIResourceId, rc.RelAPIId, rc.RelAPIParamName);
                            if (!startpt.IsEmpty && !destpt.IsEmpty)
                            {
                                var p1 = parent.PointToClient(startpt);
                                var p2 = parent.PointToClient(destpt);
                                var ptlist = new StepSelector(GetDrawWidth(),
                                    GetDrawHeight(), p1, p2,
                                    Check, StepDirection.right, StepDirection.right).Select();
                                relColumnIces.Add(new LogicMapRelColumnEx
                                {
                                    RelColumn = rc,
                                    Dest = destpt,
                                    Start = startpt,
                                    LinkLines = ptlist.ToArray()
                                });

                                haserror = ptlist.Count == 0;
                            }
                            else
                            {
                                relColumnIces.Add(new LogicMapRelColumnEx
                                {
                                    RelColumn = rc,
                                    Dest = destpt,
                                    Start = startpt,
                                    LinkLines = new Point[0]
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                //更新下
                foreach (var item in relColumnIces)
                {
                    var oldstartpos = item.Start;
                    var oldendpos = item.Dest;
                    oldstartpos.Offset(-20, 0);
                    oldendpos.Offset(20, 0);
                    Point startpt = Point.Empty, destpt = Point.Empty;
                    oldstartpos.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                    var col = FindColumn(oldstartpos);
                    bool haschange = false;
                    if (col == null || item.Start.IsEmpty || col.Id!=item.RelColumn.APIParamId
                        || col.APIId!=item.RelColumn.APIId)
                    {
                        startpt = this.FindColumnScreenStartPoint(item.RelColumn.APISourceId, item.RelColumn.APIId, item.RelColumn.APIParamName);
                        if (!startpt.IsEmpty)
                        {
                            item.Start = startpt;
                        }
                        haschange = true;
                    }
                    oldendpos.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                    col = FindColumn(oldendpos);
                    if (col == null || item.Dest.IsEmpty || col.Id!=item.RelColumn.RelAPIParamId
                        || col.APIId!=item.RelColumn.RelAPIId)
                    {
                        destpt = this.FindColumnScreenEndPoint(item.RelColumn.RelAPIResourceId, item.RelColumn.RelAPIId, item.RelColumn.RelAPIParamName);
                        if (!destpt.IsEmpty)
                        {
                            item.Dest = destpt;
                        }
                        haschange = true;
                    }

                    if (!haschange && item.LinkLines.Length < 2)
                    {
                        haschange = true;
                    }

                    if (haschange)
                    {
                        if (!item.Start.IsEmpty && !item.Dest.IsEmpty)
                        {
                            var p1 = parent.PointToClient(item.Start);
                            var p2 = parent.PointToClient(item.Dest);
                            var ptlist = new StepSelector(GetDrawWidth(),
                                    GetDrawHeight(),
                                    p1, p2,
                                   Check, StepDirection.right, StepDirection.right).Select();
                            item.LinkLines = ptlist.ToArray();

                            haserror = ptlist.Count == 0;
                        }
                        else
                        {
                            item.LinkLines = new Point[0];
                        }
                    }
                }
            }


            int colori = 0;
            foreach (var item in relColumnIces)
            {
                if (item.LinkLines.Length > 0)
                {
                    if (colori == colors.Length)
                    {
                        colori = 0;
                    }
                    item.LineColor = colors[colori];
                    using (var p = new Pen(colors[colori++], item.IsHotLine ? 2 : 1))
                    {
                        var points = item.LinkLines;

                        for (int i = 1; i <= points.Length - 1; i++)
                        {
                            if (i == points.Length - 1)
                            {
                                AdjustableArrowCap arrowCap = new AdjustableArrowCap(p.Width * 2 + 1, p.Width + 2 + 1, true);
                                p.CustomEndCap = arrowCap;
                            }
                            var p1 = points[i - 1];
                            p1.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                            var p2 = points[i];
                            p2.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                            g.DrawLine(p, p1, p2);
                            //g.DrawPie(p, new RectangleF(points[i], new SizeF(5, 5)), 0, 360);
                        }


                        //找一个好的点添加描述文字
                        if (!string.IsNullOrWhiteSpace(item.RelColumn.Desc))
                        {
                            List<Tuple<Point, Point>> linelist = new List<Tuple<Point, Point>>();
                            for (int i = 1; i <= points.Length - 1; i++)
                            {
                                linelist.Add(Tuple.Create<Point, Point>(points[i - 1], points[i]));
                            }
                            linelist = linelist.OrderByDescending(q => Math.Max(Math.Abs(q.Item1.X - q.Item2.X), Math.Abs(q.Item1.Y - q.Item2.Y))).ToList();
                            for (var x = 0; x < linelist.Count; x++)
                            {
                                var line = linelist[x];
                                var p1 = line.Item1;
                                var p2 = line.Item2;

                                var p3 = Point.Empty;
                                var p4 = Point.Empty;
                                //找最近与他平行的线
                                foreach (var item2 in relColumnIces)
                                {
                                    if (item2 == item || item2.LinkLines == null || item2.LinkLines.Length == 0)
                                    {
                                        continue;
                                    }

                                    for (int j = 1; j < item2.LinkLines.Length - 1; j++)
                                    {
                                        if (p1.X == p2.X)
                                        {
                                            if (item2.LinkLines[j - 1].X != item2.LinkLines[j].X
                                                || Math.Abs(item2.LinkLines[j - 1].X - p1.X) > 10)
                                            {
                                                continue;
                                            }
                                            var minpoint = GetMin(item2.LinkLines[j - 1], item2.LinkLines[j]);
                                            var maxpoint = GetMax(item2.LinkLines[j - 1], item2.LinkLines[j]);
                                            if (maxpoint.Y >= GetMin(p1, p2).Y && minpoint.Y <= GetMax(p1, p2).Y)
                                            {
                                                if (p3 == Point.Empty || p3.Y > minpoint.Y)
                                                {
                                                    p3 = minpoint;
                                                }
                                                if (p4 == Point.Empty || p4.Y < maxpoint.Y)
                                                {
                                                    p4 = maxpoint;
                                                }
                                            }

                                        }
                                        else if (p1.Y == p2.Y)
                                        {
                                            if (item2.LinkLines[j - 1].Y != item2.LinkLines[j].Y
                                                || Math.Abs(item2.LinkLines[j - 1].Y - p1.Y) > 10)
                                            {
                                                continue;
                                            }
                                            var minpoint = GetMin(item2.LinkLines[j - 1], item2.LinkLines[j]);
                                            var maxpoint = GetMax(item2.LinkLines[j - 1], item2.LinkLines[j]);
                                            if (maxpoint.X >= GetMin(p1, p2).X && minpoint.X <= GetMax(p1, p2).X)
                                            {
                                                if (p3 == Point.Empty || p3.X > minpoint.X)
                                                {
                                                    p3 = minpoint;
                                                }
                                                if (p4 == Point.Empty || p4.X < maxpoint.X)
                                                {
                                                    p4 = maxpoint;
                                                }
                                            }
                                        }
                                    }

                                    foreach (var tb in ucTableViews)
                                    {
                                        var rect = tb.Bounds;
                                        var newrect = rect;//new Rectangle(rect.X - 15, rect.Y - 15, rect.Width + 30, rect.Height + 30);
                                        newrect.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
                                        if (p1.X == p2.X)
                                        {
                                            if (Math.Abs(newrect.X - p1.X) < 10)
                                            {
                                                var minpoint = new Point(newrect.X, newrect.Y);
                                                var maxpoint = new Point(newrect.X, newrect.Y + newrect.Height);
                                                if (maxpoint.Y >= GetMin(p1, p2).Y && minpoint.Y <= GetMax(p1, p2).Y)
                                                {
                                                    if (p3 == Point.Empty || p3.Y > minpoint.Y)
                                                    {
                                                        p3 = minpoint;
                                                    }
                                                    if (p4 == Point.Empty || p4.Y < maxpoint.Y)
                                                    {
                                                        p4 = maxpoint;
                                                    }
                                                }
                                            }

                                            if (Math.Abs(newrect.X + newrect.Width - p1.X) < 10)
                                            {
                                                var minpoint = new Point(newrect.X + newrect.Width, newrect.Y);
                                                var maxpoint = new Point(newrect.X + newrect.Width, newrect.Y + newrect.Height);
                                                if (maxpoint.Y >= GetMin(p1, p2).Y && minpoint.Y <= GetMax(p1, p2).Y)
                                                {
                                                    if (p3 == Point.Empty || p3.Y > minpoint.Y)
                                                    {
                                                        p3 = minpoint;
                                                    }
                                                    if (p4 == Point.Empty || p4.Y < maxpoint.Y)
                                                    {
                                                        p4 = maxpoint;
                                                    }
                                                }
                                            }
                                        }
                                        else if (p1.Y == p2.Y)
                                        {
                                            if (Math.Abs(newrect.Y - p1.Y) <= 10)
                                            {
                                                var minpoint = new Point(newrect.X, newrect.Y);
                                                var maxpoint = new Point(newrect.X + newrect.Width, newrect.Y);
                                                if (maxpoint.X >= GetMin(p1, p2).X && minpoint.X <= GetMax(p1, p2).X)
                                                {
                                                    if (p3 == Point.Empty || p3.X > minpoint.X)
                                                    {
                                                        p3 = minpoint;
                                                    }
                                                    if (p4 == Point.Empty || p4.X < maxpoint.X)
                                                    {
                                                        p4 = maxpoint;
                                                    }
                                                }
                                            }

                                            if (Math.Abs(newrect.Y + newrect.Height - p1.Y) <= 10)
                                            {
                                                var minpoint = new Point(newrect.X + newrect.Width, newrect.Y);
                                                var maxpoint = new Point(newrect.X + newrect.Width, newrect.Y + newrect.Height);
                                                if (maxpoint.X >= GetMin(p1, p2).X && minpoint.X <= GetMax(p1, p2).X)
                                                {
                                                    if (p3 == Point.Empty || p3.X > minpoint.X)
                                                    {
                                                        p3 = minpoint;
                                                    }
                                                    if (p4 == Point.Empty || p4.X < maxpoint.X)
                                                    {
                                                        p4 = maxpoint;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                using (var f = new Font("宋体", 9))
                                {
                                    if (p1.X == p2.X)
                                    {
                                        var desc = item.RelColumn.Desc;
                                        StringFormat strF = new StringFormat();
                                        strF.FormatFlags = StringFormatFlags.DirectionVertical;
                                        var sf = g.MeasureString(desc, f, 20, strF);

                                        var ystart = Math.Min(p1.Y, p2.Y);
                                        var yend = Math.Max(p1.Y, p2.Y);
                                        if (p3 != Point.Empty)
                                        {
                                            if (Math.Abs(p1.X - p3.X) < sf.Width / 2)
                                            {
                                                var ystart2 = Math.Min(p3.Y, p4.Y);
                                                var yend2 = Math.Max(p3.Y, p4.Y);
                                                if (ystart2 <= ystart && yend2 >= yend)
                                                {
                                                    if (x == linelist.Count - 1)
                                                    {
                                                        ystart = Math.Min(linelist.First().Item1.Y, linelist.First().Item2.Y);
                                                        yend = Math.Max(linelist.First().Item1.Y, linelist.First().Item2.Y);
                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }
                                                }
                                                if (yend2 >= ystart && ystart2 <= yend)
                                                {
                                                    if (ystart2 - ystart > yend - yend2 && ystart2 - ystart > sf.Height)
                                                    {
                                                        yend = ystart2;
                                                    }
                                                    else if (yend - yend2 > ystart2 - ystart)
                                                    {
                                                        ystart = yend2;
                                                    }
                                                }
                                            }
                                        }

                                        var rect = new Rectangle(p1.X, ystart, (int)sf.Width + 5, (int)sf.Height + 20);
                                        rect.Offset(-(int)(sf.Width / 2), (int)(Math.Abs(yend - ystart) / 2 - sf.Height / 2));
                                        rect.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                                        item.DescRect = rect;
                                        g.DrawString(desc, f, new SolidBrush(p.Color), item.DescRect, strF);
                                    }
                                    else if (p1.Y == p2.Y)
                                    {
                                        var sf = g.MeasureString(item.RelColumn.Desc, f);

                                        var xstart = Math.Min(p1.X, p2.X);
                                        var xend = Math.Max(p1.X, p2.X);
                                        if (p3 != Point.Empty)
                                        {
                                            if (Math.Abs(p1.Y - p3.Y) < sf.Height / 2)
                                            {
                                                var xstart2 = Math.Min(p3.X, p4.X);
                                                var xend2 = Math.Max(p3.X, p4.X);
                                                if (xstart2 <= xstart && xend2 >= xend)
                                                {
                                                    if (x == linelist.Count - 1)
                                                    {
                                                        xstart = Math.Min(linelist.First().Item1.X, linelist.First().Item2.X);
                                                        xend = Math.Max(linelist.First().Item1.X, linelist.First().Item2.X);
                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }
                                                }
                                                if (xend2 >= xstart && xstart2 <= xend)
                                                {
                                                    if (xstart2 - xstart > xend - xend2 && xstart2 - xstart > sf.Width)
                                                    {
                                                        xend = xstart2;
                                                    }
                                                    else if (xend - xend2 > xstart2 - xstart)
                                                    {
                                                        xstart = xend2;
                                                    }
                                                }
                                            }
                                        }


                                        var rect = new Rectangle(xstart, p1.Y, (int)sf.Width + 20, (int)sf.Height);
                                        rect.Offset((int)(Math.Abs(xend - xstart) / 2 - sf.Width / 2), -(int)(sf.Height / 2));
                                        rect.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                                        item.DescRect = rect;
                                        g.DrawString(item.RelColumn.Desc, f, new SolidBrush(p.Color), item.DescRect);
                                    }
                                }

                                break;

                            }
                        }
                    }
                }
            }

            if (haserror)
            {
                Util.SendMsg(this, "uctablerelmap", "绘制关系图出错，请尝试拖动表格重新绘制。");

            }
            else
            {
                Util.ClearMsg(this, "uctablerelmap");
            }
        }

        private void PanelMap_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is UCLogicTableView)
            {
                this.ucTableViews.Remove((UCLogicTableView)e.Control);
            }
        }

        private void PanelMap_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is UCLogicTableView)
            {
                this.ucTableViews.Add((UCLogicTableView)e.Control);
            }
        }

        public Point FindColumnScreenStartPoint(int apisourceid, int apiid, string colname)
        {
            foreach (var v in this.ucTableViews)
            {
                if (v.APISouceId == apisourceid && v.APIId == apiid)
                {
                    var rect = v.FindTBColumnScreenRect(colname);
                    if (!rect.IsEmpty)
                    {
                        var pt = rect.Location;
                        pt.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
                        var offsetx = new Random(Guid.NewGuid().GetHashCode()).Next(2, rect.Height - 2);
                        pt.Offset(rect.Width + 6, offsetx);
                        return pt;
                    }
                }
            }

            return Point.Empty;
        }

        public Point FindColumnScreenEndPoint(int souceid, int apiid, string colname)
        {
            foreach (var v in this.ucTableViews)
            {
                if (v.APISouceId == souceid && v.APIId == apiid)
                {
                    var rect = v.FindTBColumnScreenRect(colname);
                    if (!rect.IsEmpty)
                    {
                        var pt = rect.Location;
                        pt.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
                        var offsetx = new Random(Guid.NewGuid().GetHashCode()).Next(2, rect.Height - 2);
                        //pt.Offset(-10, rect.Height / 2);
                        pt.Offset(-10, offsetx);
                        return pt;
                    }
                }
            }

            return Point.Empty;
        }

        public APIParamEx FindColumn(Point screenpt)
        {
            foreach (var v in this.ucTableViews)
            {
                var panel = v.Controls.Find("ColumnsPanel", false).FirstOrDefault();
                if (panel != null)
                {
                    var ct = panel.GetChildAtPoint(panel.PointToClient(screenpt));
                    if (ct is Label && ct.Tag is APIParamEx)
                    {
                        var col = (ct.Tag as APIParamEx);
                        //MessageBox.Show(col.Name);
                        return col;
                    }
                }
            }

            return null;
        }


        private void AdjustLoaction(UCLogicTableView view)
        {
            //原则 调下不调上，调右不调左
            List<UCLogicTableView> adjustlist = new List<UCLogicTableView>();
            int paddingx = 20, paddingy = 30;
            var rect = new Rectangle(view.Location, view.Size);
            foreach (var v in ucTableViews)
            {
                if (v.Equals(view))
                {
                    continue;
                }
                var rect2 = new Rectangle(v.Location, v.Size);
                //rect2.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
                if (rect.IntersectsWith(rect2))
                {
                    if (Math.Abs(view.Location.X - v.Location.X) > view.Width / 2)
                    {
                        v.Location = new Point(view.Location.X + view.Width + paddingx,
                            v.Location.Y);
                    }
                    else
                    {
                        v.Location = new Point(v.Location.X,
                            view.Location.Y + view.Height + paddingy);
                    }
                    adjustlist.Add(v);
                }
            }

            foreach (var v in adjustlist)
            {
                AdjustLoaction(v);
            }

        }

        private bool Check(Point p1, Point p2, bool isline)
        {
            //if (!this.Bounds.Contains(p1) || !this.Bounds.Contains(p2))
            //{
            //    return true;
            //}

            var line = new Line(p1, p2);

            //var boo = isline && DrawingUtil.HasIntersect(Bounds, line);
            //if (boo)
            //{
            //    return true;
            //}


            foreach (var tb in ucTableViews)
            {
                var rect = tb.Bounds;
                var newrect = rect;//new Rectangle(rect.X - 15, rect.Y - 15, rect.Width + 30, rect.Height + 30);
                newrect.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
                if (newrect.Contains(p1) || newrect.Contains(p2))
                {
                    return true;
                }

                var boo = isline && DrawingUtil.HasIntersect(newrect, line);
                if (boo)
                {
                    return true;
                }
            }

            if (relColumnIces != null)
            {
                foreach (var item in this.relColumnIces)
                {
                    for (int i = 1; i < item.LinkLines.Length; i++)
                    {
                        var subline = new Line(item.LinkLines[i - 1], item.LinkLines[i]);
                        if (DrawingUtil.Isoverlap(line, subline))
                        {
                            return true;
                        }
                    }

                    if (item.DescRect != Rectangle.Empty)
                    {
                        var boo = isline && DrawingUtil.HasIntersect(item.DescRect, line);
                        if (boo)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void AddTable(APISource source,APIUrl apiurl,bool adjustLoaction=true)
        {
            //var =BigEntityTableEngine.LocalEngine.Find<APISource>(nameof(APISource), sourceid);
            var location = this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top));
            UCLogicTableView tv = new UCLogicTableView(source, this.APISource.Id==source.Id, apiurl,this._logicMapId, () =>
            {
                var list = new List<Tuple<int, int>>();
                foreach (var tb in ucTableViews)
                {
                    list.Add(new Tuple<int, int>(tb.APISouceId, tb.APIId));
                }
                if (!tableColumnList.ContainsKey(source.Id))
                {
                    //var tbs = SQLHelper.GetTBs(this.DBSource, db);
                    var tbs = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), "SourceId", new object[] { source.Id }).ToList();
                    tableColumnList.Add(source.Id, tbs);
                }
                return tableColumnList[source.Id].Select(p => new Tuple<int, APIUrl>(source.Id, p)).Where(p => !list.Contains(new Tuple<int, int>(p.Item1,p.Item2.Id))).OrderBy(p => p).ToList();
            }, v =>
            {

                if (v.APIId>0)
                {
                    var pt = v.Location;
                    pt.Offset(-this.PanelMap.AutoScrollPosition.X, -this.PanelMap.AutoScrollPosition.Y);
                    var newreltable = new LogicMapTable
                    {
                        LogicID = this._logicMapId,
                        APIResourceId=v.APISouceId,
                        APIResourceName = v.APISourceName.ToLower(),
                        APIId=v.APIId,
                        APIName = v.APIName.ToLower(),
                        Posx = pt.X,
                        Posy = pt.Y
                    };
                    var boo = BigEntityTableEngine.LocalEngine.Find<LogicMapTable>(nameof(LogicMapTable),
                            p => p.LogicID == this._logicMapId && p.APIResourceId==newreltable.APIResourceId
                                && p.APIId==newreltable.APIId).Any();
                    if (!boo)
                    {
                        BigEntityTableEngine.LocalEngine.Insert(nameof(LogicMapTable), newreltable);
                        v.LogicMapTableId = newreltable.ID;
                    }
                }
                this.PanelMap.Invalidate();
                v.Location = location;
                if (adjustLoaction)
                {
                    AdjustLoaction(v);
                }
                
            },
              Check);
            tv.OnAddNewRelColumn = c =>
            {
                this.relColumnIces.Add(new LogicMapRelColumnEx() { RelColumn = c });
                this.PanelMap.Invalidate();
            };
            tv.Location = location;
            this.PanelMap.Controls.Add(tv);
        }

        private void delStripMenuItem_Click(object sender, EventArgs e)
        {
            var ct = this.PanelMap.GetChildAtPoint(this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top)));
            if (ct is UCLogicTableView)
            {
                var view = ((UCLogicTableView)ct);
                if (!string.IsNullOrWhiteSpace(view.RpTableName))
                {
                    if (string.IsNullOrEmpty(view.APIName) || MessageBox.Show($"要删除和表{view.RpTableName}关联关系吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var v = view;

                        foreach (var item in BigEntityTableEngine.LocalEngine.Find<LogicMapTable>(nameof(LogicMapTable),
                                p => p.LogicID == this._logicMapId && p.APIResourceId == v.APISouceId
                                    && p.APIId == v.APIId).ToList())
                        {
                            BigEntityTableEngine.LocalEngine.Delete<LogicMapTable>(nameof(LogicMapTable), item.ID);
                        }
                        this.PanelMap.Controls.Remove(ct);
                    }
                }
            }
        }

        private void TSMI_CopyTableName_Click(object sender, EventArgs e)
        {
            var ct = this.PanelMap.GetChildAtPoint(this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top)));
            if (ct is UCLogicTableView)
            {
                var view = ((UCLogicTableView)ct);
                if (!string.IsNullOrWhiteSpace(view.RpTableName))
                {
                    Clipboard.SetText(view.RpTableName);
                    Util.SendMsg(this, "已复制表名");
                }
            }
        }

        private void TSMI_CopyColName_Click(object sender, EventArgs e)
        {
            var col = FindColumn(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top));
            if (col != null)
            {
                Clipboard.SetText(col.Name);
                Util.SendMsg(this, "已复制字段名");
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void TSMI_Export_Click(object sender, EventArgs e)
        {
            SubForm.InputStringDlg dlg = new SubForm.InputStringDlg("导出名称");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(dlg.InputString))
                {
                    var dir = Application.StartupPath + "\\temp\\";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var filename = $"{dir}\\{dlg.InputString}.png";

                    var sx = PanelMap.HorizontalScroll.Value;
                    var sy = PanelMap.VerticalScroll.Value;

                    var w = Math.Max(PanelMap.Width, PanelMap.HorizontalScroll.Maximum);
                    var h = Math.Max(PanelMap.Height, PanelMap.VerticalScroll.Maximum);
                    bool hashbar = w > PanelMap.Width, hasvbar = h > PanelMap.Height;
                    using (var bm = new Bitmap(w, h))
                    {
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            int y = 0;
                            PanelMap.VerticalScroll.Value = PanelMap.VerticalScroll.Minimum;
                            while (y < h)
                            {
                                int x = 0;
                                var yleft = Math.Min(PanelMap.Height, h - y);
                                if (yleft == 0)
                                {
                                    break;
                                }

                                while (x < w)
                                {
                                    var xleft = Math.Min(PanelMap.Width, w - x);
                                    if (xleft == 0)
                                    {
                                        break;
                                    }
                                    using (var bm1 = new Bitmap(PanelMap.Width, PanelMap.Height))
                                    {
                                        this.PanelMap.DrawToBitmap(bm1, new Rectangle(0, 0, bm1.Width, bm1.Height));
                                        g.DrawImage(bm1, x, y, new Rectangle(bm1.Width - xleft, bm1.Height - yleft, xleft, yleft), GraphicsUnit.Pixel);
                                    }
                                    x += xleft;
                                    if (x < PanelMap.HorizontalScroll.Maximum)
                                    {
                                        PanelMap.HorizontalScroll.Value = x;
                                    }
                                }


                                y += yleft;
                                if (y < PanelMap.VerticalScroll.Maximum)
                                {
                                    PanelMap.VerticalScroll.Value = y;
                                }
                            }
                            if (hashbar || hasvbar)
                            {
                                Rectangle rect = new Rectangle(0, 0, w - (hasvbar ? 25 : 0), h - (hashbar ? 25 : 0));
                                using (var bmcopy = bm.Clone(rect, PixelFormat.Format32bppArgb))
                                {
                                    bmcopy.Save(filename, ImageFormat.Png);
                                }
                            }
                            else
                            {
                                bm.Save(filename, ImageFormat.Png);
                            }
                            Util.SendMsg(this, $"文件已保存:{filename}");
                            System.Diagnostics.Process.Start("explorer.exe", dir);
                        }
                    }
                    PanelMap.HorizontalScroll.Value = sx;
                    PanelMap.VerticalScroll.Value = sy;
                }
            }
        }
    }
}
