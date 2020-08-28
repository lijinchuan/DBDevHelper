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

namespace NETDBHelper.UC
{
    public partial class UCTableRelMap : TabPage
    {
        DBSource DBSource = null;
        string DBName = null;
        string Tbname = null;

        private List<UCTableView2> ucTableViews = new List<UCTableView2>();

        private Lazy<List<string>> lzTableList = null;

        public UCTableRelMap()
        {
            InitializeComponent();
        }

        public UCTableRelMap(DBSource dbSource,string dbname,string tbname)
        {
            InitializeComponent();
            this.DBSource = dbSource;
            this.DBName = dbname;
            this.Tbname = tbname;

            this.PanelMap.ContextMenuStrip = this.CMSOpMenu;
            this.PanelMap.AutoScroll = true;
            this.PanelMap.ControlAdded += PanelMap_ControlAdded;
            this.PanelMap.ControlRemoved += PanelMap_ControlRemoved;

            lzTableList = new Lazy<List<string>>(() =>
             {
                 var tbs = SQLHelper.GetTBs(this.DBSource, DBName);
                 List<string> tablelist = new List<string>();
                 tablelist.AddRange(tbs.AsEnumerable().Select(p => p.Field<string>("name")));
                 return tablelist;
             });
        }

        public void Load()
        {
            int pointstartx = 50;
            int margin = 100;
            var location = new Point(pointstartx, 30);
            List<string> reltblist = new List<string>();
            reltblist.Add(this.Tbname.ToLower());

            var list = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelTable>(nameof(RelTable),
                p =>
                {
                    return p.ServerName.Equals(this.DBSource.ServerName, StringComparison.OrdinalIgnoreCase)
                            && p.DBName.Equals(this.DBName, StringComparison.OrdinalIgnoreCase)
                            && ((p.TBName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)
                            || p.RelTBName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)));
                }).ToList();
            reltblist.AddRange(list.Select(p => p.RelTBName));
            reltblist.AddRange(list.Select(p => p.TBName));
            reltblist = reltblist.Distinct().ToList();

            int maxy = 0;
            foreach (var item in reltblist)
            {
                UCTableView2 tv = new UCTableView2(DBSource, DBName, item, () =>
                {
                    var tblist = new List<string>();
                    foreach (var tb in ucTableViews)
                    {
                        tblist.Add(tb.TableName?.ToLower());
                    }
                    return lzTableList.Value.Where(p => !tblist.Contains(p.ToLower())).OrderBy(p => p).ToList();
                }, v =>
                {
                    AdjustLoaction(v);
                },
                  Check);
                tv.Location = location;
                this.PanelMap.Controls.Add(tv);

                if (tv.Location.Y + tv.Height > maxy)
                {
                    maxy = tv.Location.Y + tv.Height;
                }

                if (location.X + tv.Width >= this.Width)
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

        private void PanelMap_Paint(object sender, PaintEventArgs e)
        {
            foreach (var v in this.ucTableViews)
            {
                v.LinkRelCols(this.PanelMap, e.Graphics);
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

        public Point FindColumnScreenPoint(string tbname, string colname)
        {
            foreach (var v in this.ucTableViews)
            {
                if (v.TableName.Equals(tbname, StringComparison.OrdinalIgnoreCase))
                {
                    var pt = v.FindTBColumnScreenPos(colname);
                    pt.Offset(-10, -5);
                    return pt;
                }
            }

            return Point.Empty;
        }

        public TBColumn FindColumn(Point screenpt)
        {
            foreach (var v in this.ucTableViews)
            {
                screenpt.Offset(5, 0);
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

        private bool Check(Point p1,Point p2,bool isline)
        {
            if (!this.Bounds.Contains(p1) || !this.Bounds.Contains(p2))
            {
                return true;
            }

            var line = new Line(p1, p2);

            var boo = isline && DrawingUtil.HasIntersect(ClientRectangle, line);
            if (boo)
            {
                return true;
            }


            foreach (var tb in ucTableViews)
            {
                var rect = tb.Bounds;
                var newrect = rect;//new Rectangle(rect.X - 15, rect.Y - 15, rect.Width + 30, rect.Height + 30);

                if (newrect.Contains(p1) || newrect.Contains(p2))
                {
                    return true;
                }

                boo = isline && DrawingUtil.HasIntersect(newrect, line);
                if (boo)
                {
                    return true;
                }
            }
            return false;
        }

        private void 添加表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var location = this.PanelMap.PointToClient(new Point(this.CMSOpMenu.Left, this.CMSOpMenu.Top));
            UCTableView2 tv = new UCTableView2(DBSource, DBName, null, () =>
              {
                  var list = new List<string>();
                  foreach (var tb in ucTableViews)
                  {
                      list.Add(tb.TableName?.ToLower());
                  }
                  return lzTableList.Value.Where(p => !list.Contains(p.ToLower())).OrderBy(p => p).ToList();
              }, v => { 
                  
                  if (!string.IsNullOrWhiteSpace(v.TableName) && !v.TableName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase))
                  {
                      var newreltable = new RelTable
                      {
                          ServerName = DBSource.ServerName.ToLower(),
                          DBName = DBName.ToLower(),
                          TBName = Tbname.ToLower(),
                          RelTBName = v.TableName.ToLower()
                      };
                      var boo = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<RelTable>(nameof(RelTable),
                              p => p.ServerName.Equals(newreltable.ServerName, StringComparison.OrdinalIgnoreCase)
                                  && p.DBName.Equals(newreltable.DBName, StringComparison.OrdinalIgnoreCase)
                                  && p.TBName.Equals(newreltable.TBName, StringComparison.OrdinalIgnoreCase)
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
                                    p => p.ServerName.Equals(this.DBSource.ServerName, StringComparison.OrdinalIgnoreCase)
                                        && p.DBName.Equals(this.DBName, StringComparison.OrdinalIgnoreCase)
                                        && p.TBName.Equals(this.Tbname, StringComparison.OrdinalIgnoreCase)
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
    }
}
