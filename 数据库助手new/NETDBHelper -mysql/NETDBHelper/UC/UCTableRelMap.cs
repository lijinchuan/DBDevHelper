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
using LJC.FrameWorkV3.Comm;
using NETDBHelper.Drawing;
using LJC.FrameWork.Data.EntityDataBase;
using NPOI.SS.Formula;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace NETDBHelper.UC
{
    public partial class UCTableRelMap : TabPage
    {
        DBSource DBSource = null;
        string _DBName = null;
        string Tbname = null;

        static Color[] colors = new Color[] { Color.LightBlue, Color.Red, Color.Green, Color.Gold, Color.BurlyWood,
            Color.GreenYellow,Color.Black,Color.Brown,Color.Chocolate};

        private List<UCTableView2> ucTableViews = new List<UCTableView2>();

        Dictionary<string, List<string>> tableColumnList = null;

        List<RelColumnEx> relColumnIces = null;

        bool hashotline = false;

        public UCTableRelMap()
        {
            InitializeComponent();
        }

        public UCTableRelMap(DBSource dbSource, string dbname, string tbname)
        {
            InitializeComponent();
            this.DBSource = dbSource;
            this._DBName = dbname;
            this.Tbname = tbname;

            this.PanelMap.ContextMenuStrip = this.CMSOpMenu;
            this.PanelMap.AutoScroll = true;
            this.PanelMap.ControlAdded += PanelMap_ControlAdded;
            this.PanelMap.ControlRemoved += PanelMap_ControlRemoved;
            this.PanelMap.MouseClick += PanelMap_MouseClick;
            this.PanelMap.MouseDoubleClick += PanelMap_MouseDoubleClick;

            tableColumnList = new Dictionary<string, List<string>>();

            var tbs = MySQLHelper.GetTBs(this.DBSource, dbname);
            tableColumnList.Add(dbname.ToLower(), tbs.AsEnumerable().Select(p => p.Field<string>("name").ToLower()).ToList());

            this.CMSOpMenu.VisibleChanged += CMSOpMenu_VisibleChanged;
            TSMDelRelColumn.DropDownItemClicked += TSMDelRelColumn_DropDownItemClicked;
        }

        private void PanelMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (hashotline)
            {
                var pt = e.Location;
                RelColumnEx relColumnEx = FindHotLine(pt);
                if (relColumnEx != null)
                {
                    SubForm.InputStringDlg dlg = new SubForm.InputStringDlg($"输入{relColumnEx.RelColumn.TBName}.{relColumnEx.RelColumn.ColName}和{relColumnEx.RelColumn.RelTBName}.{relColumnEx.RelColumn.RelColName}关系描述", relColumnEx.RelColumn.Desc ?? string.Empty);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        relColumnEx.RelColumn.Desc = dlg.InputString;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Update<RelColumn>(nameof(RelColumn), relColumnEx.RelColumn);
                        this.PanelMap.Invalidate();
                    }
                }
            }
        }

        private RelColumnEx FindHotLine(Point ptOnPanelMap)
        {
            var pt = ptOnPanelMap;
            RelColumnEx relColumnEx = null;
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

            return relColumnEx;
        }

        private void PanelMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (relColumnIces != null)
            {
                var pt = e.Location;
                RelColumnEx relColumnEx = FindHotLine(pt);

                if (relColumnEx != null)
                {
                    hashotline = true;
                    //MessageBox.Show(string.Join(",", relColumnEx.LinkLines.Select(p => p.X + " " + p.Y)));
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
                TSMDelRelColumn.Visible = false;
                if (hashotline)
                {
                    var location = new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top);
                    var relColumnEx = this.FindHotLine(this.PanelMap.PointToClient(location));
                    if (relColumnEx != null)
                    {
                        var ts = new ToolStripMenuItem();
                        ts.Text = $"{relColumnEx.RelColumn.RelDBName}.{relColumnEx.RelColumn.RelTBName}.{relColumnEx.RelColumn.RelColName}";
                        ts.Tag = relColumnEx.RelColumn;
                        TSMDelRelColumn.DropDownItems.Add(ts);
                    }
                    TSMDelRelColumn.Visible = TSMDelRelColumn.DropDownItems.Count > 0;
                }
            }
            else
            {
                TSMDelRelColumn.DropDownItems.Clear();

            }
        }

        private void TSMDelRelColumn_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Tag is RelColumn)
            {
                var rc = e.ClickedItem.Tag as RelColumn;
                if (MessageBox.Show("是否要删除关联", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if(LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Delete<RelColumn>(nameof(RelColumn),
                        rc.Id))
                    {
                        TSMDelRelColumn.DropDownItems.Remove(e.ClickedItem);
                        this.relColumnIces.RemoveAll(p => p.RelColumn.Id == ((RelColumn)e.ClickedItem.Tag).Id);
                    }
                    this.PanelMap.Invalidate();
                }
            }
        }

        public void Load()
        {
            var dbs = MySQLHelper.GetDBs(this.DBSource);
            foreach (DataRow r in dbs.Select())
            {
                添加表ToolStripMenuItem.DropDownItems.Add((string)r["name"]);
            }
            添加表ToolStripMenuItem.DropDownItemClicked += 添加表ToolStripMenuItem_DropDownItemClicked;
            添加表ToolStripMenuItem.Click += 添加表ToolStripMenuItem_Click;

            int pointstartx = 50;
            int margin = 100;
            var location = new Point(pointstartx, 30);
            List<Tuple<string, string>> reltblist = new List<Tuple<string, string>>();
            reltblist.Add(new Tuple<string, string>(this._DBName.ToLower(), this.Tbname.ToLower()));

            var list = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelTable>(nameof(RelTable),
                p =>
                {
                    return p.DBName.Equals(this._DBName, StringComparison.OrdinalIgnoreCase)
                            && ((p.TBName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)
                            || p.RelTBName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)));
                }).ToList();
            reltblist.AddRange(list.Select(p => new Tuple<string, string>(p.RelDBName, p.RelTBName)));
            reltblist.AddRange(list.Select(p => new Tuple<string, string>(p.DBName, p.TBName)));

            var allrelcolumnlist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelColumn>(nameof(RelColumn), r =>
            {
                return (this._DBName.Equals(r.DBName, StringComparison.OrdinalIgnoreCase)
                && this.Tbname.Equals(r.TBName, StringComparison.OrdinalIgnoreCase))
                || (this._DBName.Equals(r.RelDBName, StringComparison.OrdinalIgnoreCase)
                && this.Tbname.Equals(r.RelTBName, StringComparison.OrdinalIgnoreCase));
            }).ToList();

            reltblist.AddRange(allrelcolumnlist.Select(p => new Tuple<string, string>(p.DBName, p.TBName)));
            reltblist.AddRange(allrelcolumnlist.Select(p => new Tuple<string, string>(p.RelDBName, p.RelTBName)));

            reltblist = reltblist.Distinct().ToList();

            int maxy = 0;
            foreach (var item in reltblist)
            {
                UCTableView2 tv = new UCTableView2(DBSource, this._DBName.Equals(item.Item1, StringComparison.OrdinalIgnoreCase), item.Item1, item.Item2, () =>
                  {
                      var tblist = new List<Tuple<string, string>>();
                      foreach (var tb in ucTableViews)
                      {
                          if (!string.IsNullOrWhiteSpace(tb.TableName))
                          {
                              tblist.Add(Tuple.Create<string, string>(tb.DataBaseName.ToLower(), tb.TableName.ToLower()));
                          }
                      }

                      if (!tableColumnList.ContainsKey(item.Item1))
                      {
                          var tbs = MySQLHelper.GetTBs(this.DBSource, item.Item1);
                          tableColumnList.Add(item.Item1, tbs.AsEnumerable().Select(p => p.Field<string>("name").ToLower()).ToList());
                      }

                      return tableColumnList[item.Item1].Select(p => new Tuple<string, string>(item.Item1, p)).Where(p => !tblist.Contains(p)).OrderBy(p => p).ToList();
                  }, v =>
                  {
                      AdjustLoaction(v);
                  },
                  Check);
                tv.OnAddNewRelColumn = c =>
                {
                    this.relColumnIces.Add(new RelColumnEx() { RelColumn = c });
                    this.PanelMap.Invalidate();
                };
                tv.Location = location;
                this.PanelMap.Controls.Add(tv);

                if (tv.Location.Y + tv.Height > maxy)
                {
                    maxy = tv.Location.Y + tv.Height;
                }

                if (location.X + 2 * tv.Width + margin >= this.PanelMap.Width)
                {
                    location = new Point(pointstartx, maxy + margin);
                }
                else
                {
                    location = new Point(location.X + tv.Width + margin, location.Y);
                }
            }
            this.DoubleBuffered = true;
            this.PanelMap.Paint += PanelMap_Paint;

        }

        private void 添加表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CMSOpMenu.Visible = false;
            AddTable(this._DBName);
        }

        private void 添加表ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
            AddTable(e.ClickedItem.Text);
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
                        relColumnIces = new List<RelColumnEx>();
                        var othertables = ucTableViews.Where(p => !string.IsNullOrEmpty(p.TableName)).Select(p => new Tuple<string, string>(p.DataBaseName, p.TableName)).Distinct().ToList();
                        var allrelcolumnlist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelColumn>(nameof(RelColumn), r =>
                         {
                             return (r.DBName.Equals(this._DBName, StringComparison.OrdinalIgnoreCase) || r.RelDBName.Equals(this._DBName, StringComparison.OrdinalIgnoreCase))
                             && (othertables.Any(p => p.Item1.Equals(r.DBName, StringComparison.OrdinalIgnoreCase) && p.Item2.Equals(r.TBName, StringComparison.OrdinalIgnoreCase))
                             || othertables.Any(p => p.Item1.Equals(r.RelDBName, StringComparison.OrdinalIgnoreCase) && p.Item2.Equals(r.RelTBName, StringComparison.OrdinalIgnoreCase)));
                         }).ToList();

                        foreach (var rc in allrelcolumnlist)
                        {
                            var startpt = this.FindColumnScreenStartPoint(rc.DBName, rc.TBName, rc.ColName);
                            var destpt = this.FindColumnScreenEndPoint(rc.RelDBName, rc.RelTBName, rc.RelColName);
                            if (!startpt.IsEmpty && !destpt.IsEmpty)
                            {
                                var p1 = parent.PointToClient(startpt);
                                var p2 = parent.PointToClient(destpt);
                                var ptlist = new StepSelector(GetDrawWidth(),
                                    GetDrawHeight(), p1, p2,
                                    Check, StepDirection.right, StepDirection.right).Select();
                                relColumnIces.Add(new RelColumnEx
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
                                relColumnIces.Add(new RelColumnEx
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
                    var col = FindColumn(oldstartpos);
                    bool haschange = false;
                    if (col == null || item.Start.IsEmpty || !col.Name.Equals(item.RelColumn.ColName, StringComparison.OrdinalIgnoreCase)
                        || !col.TBName.Equals(item.RelColumn.TBName, StringComparison.OrdinalIgnoreCase))
                    {
                        startpt = this.FindColumnScreenStartPoint(item.RelColumn.DBName, item.RelColumn.TBName, item.RelColumn.ColName);
                        if (!startpt.IsEmpty)
                        {
                            item.Start = startpt;
                        }
                        haschange = true;
                    }

                    col = FindColumn(oldendpos);
                    if (col == null || item.Dest.IsEmpty || !col.Name.Equals(item.RelColumn.RelColName, StringComparison.OrdinalIgnoreCase)
                        || !col.TBName.Equals(item.RelColumn.RelTBName, StringComparison.OrdinalIgnoreCase))
                    {
                        destpt = this.FindColumnScreenEndPoint(item.RelColumn.RelDBName, item.RelColumn.RelTBName, item.RelColumn.RelColName);
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
                    using (var p = new Pen(colors[colori++], 1))
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
            if (e.Control is UCTableView2)
            {
                this.ucTableViews.Remove((UCTableView2)e.Control);
            }
        }

        private void PanelMap_ControlAdded(object sender, ControlEventArgs e)
        {
            if(e.Control is UCTableView2)
            {
                this.ucTableViews.Add((UCTableView2)e.Control);
            }
        }

        public Point FindColumnScreenStartPoint(string dbname,string tbname, string colname)
        {
            foreach (var v in this.ucTableViews)
            {
                if (v.DataBaseName.Equals(dbname, StringComparison.OrdinalIgnoreCase) && v.TableName?.Equals(tbname, StringComparison.OrdinalIgnoreCase) == true)
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

        public Point FindColumnScreenEndPoint(string dbname, string tbname, string colname)
        {
            foreach (var v in this.ucTableViews)
            {
                if (v.DataBaseName.Equals(dbname, StringComparison.OrdinalIgnoreCase) && v.TableName?.Equals(tbname, StringComparison.OrdinalIgnoreCase) == true)
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

        public TBColumn FindColumn(Point screenpt)
        {
            screenpt.Offset(this.PanelMap.AutoScrollPosition.X, this.PanelMap.AutoScrollPosition.Y);
            foreach (var v in this.ucTableViews)
            {
                var panel = v.Controls.Find("ColumnsPanel", false).FirstOrDefault();
                if (panel != null)
                {
                    var ct = panel.GetChildAtPoint(panel.PointToClient(screenpt));
                    if (ct is Label && ct.Tag is TBColumn)
                    {
                        var col = (ct.Tag as TBColumn);
                        //MessageBox.Show(col.Name);
                        return col;
                    }
                }
            }

            return null;
        }


        private void AdjustLoaction(UCTableView2 view)
        {
            //原则 调下不调上，调右不调左
            List<UCTableView2> adjustlist = new List<UCTableView2>();
            int paddingx = 20, paddingy = 30;
            var rect = new Rectangle(view.Location, view.Size);
            foreach(var v in ucTableViews)
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

            foreach(var v in adjustlist)
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

        private void AddTable(string db)
        {
            var location = this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top));
            UCTableView2 tv = new UCTableView2(DBSource, _DBName.Equals(db, StringComparison.OrdinalIgnoreCase), db, null, () =>
                {
                    var list = new List<Tuple<string, string>>();
                    foreach (var tb in ucTableViews)
                    {
                        list.Add(new Tuple<string, string>(tb.DataBaseName.ToLower(), tb.TableName?.ToLower()));
                    }
                    if (!tableColumnList.ContainsKey(db.ToLower()))
                    {
                        var tbs = MySQLHelper.GetTBs(this.DBSource, db);
                        tableColumnList.Add(db.ToLower(), tbs.AsEnumerable().Select(p => p.Field<string>("name").ToLower()).ToList());
                    }
                    return tableColumnList[db.ToLower()].Select(p => new Tuple<string, string>(db.ToLower(), p)).Where(p => !list.Contains(p)).OrderBy(p => p).ToList();
                }, v =>
                {

                    if (!string.IsNullOrWhiteSpace(v.TableName) && (!v.DataBaseName.Equals(_DBName, StringComparison.OrdinalIgnoreCase) || !v.TableName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)))
                    {
                        var newreltable = new RelTable
                        {
                            ServerName = DBSource.ServerName.ToLower(),
                            DBName = this._DBName.ToLower(),
                            TBName = Tbname.ToLower(),
                            RelDBName = v.DataBaseName.ToLower(),
                            RelTBName = v.TableName.ToLower()
                        };
                        var boo = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelTable>(nameof(RelTable),
                                p => p.DBName.Equals(newreltable.DBName, StringComparison.OrdinalIgnoreCase)
                                    && p.TBName.Equals(newreltable.TBName, StringComparison.OrdinalIgnoreCase)
                                    && p.RelDBName.Equals(newreltable.RelDBName, StringComparison.OrdinalIgnoreCase)
                                    && p.RelTBName.Equals(newreltable.RelTBName, StringComparison.OrdinalIgnoreCase)).Any();
                        if (!boo)
                        {
                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<RelTable>(nameof(RelTable), newreltable);
                        }
                    }

                    v.Location = location;
                    AdjustLoaction(v);
                },
              Check);
            tv.OnAddNewRelColumn = c =>
            {
                this.relColumnIces.Add(new RelColumnEx() { RelColumn = c });
                this.PanelMap.Invalidate();
            };
            tv.Location = location;
            this.PanelMap.Controls.Add(tv);
        }

        private void delStripMenuItem_Click(object sender, EventArgs e)
        {
            var ct = this.PanelMap.GetChildAtPoint(this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top)));
            if (ct is UCTableView2)
            {
                var view = ((UCTableView2)ct);
                if (view.TableName != this.Tbname)
                {
                    if (string.IsNullOrEmpty(view.TableName) || MessageBox.Show($"要删除和表{view.TableName}关联关系吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var v = view;
                        if (!string.IsNullOrWhiteSpace(v.TableName) && !v.TableName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var item in LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelTable>(nameof(RelTable),
                                    p => p.DBName.Equals(this._DBName, StringComparison.OrdinalIgnoreCase)
                                        && p.TBName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)
                                        && p.RelDBName.Equals(v.DataBaseName, StringComparison.OrdinalIgnoreCase)
                                        && p.RelTBName.Equals(v.TableName, StringComparison.OrdinalIgnoreCase)).ToList())
                            {
                                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Delete<RelTable>(nameof(RelTable), item.Id);
                            }
                        }
                        this.PanelMap.Controls.Remove(ct);
                    }
                }
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
                    if (System.IO.File.Exists(filename))
                    {
                        MessageBox.Show("文件名已存在");
                        return;
                    }
                    
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
