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

namespace NETDBHelper.UC
{
    public partial class UCTableRelMap : TabPage
    {
        Point lastLocation = Point.Empty;
        DBSource DBSource = null;
        string DBName = null;

        private List<UCTableView> ucTableViews = new List<UCTableView>();

        private Lazy<List<string>> lzTableList = null;

        public UCTableRelMap()
        {
            InitializeComponent();
        }

        public UCTableRelMap(DBSource dbSource,string dbname)
        {
            InitializeComponent();
            this.DBSource = dbSource;
            this.DBName = dbname;
            this.PanelMap.MouseMove += UCTableRelMap_MouseMove;
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

        private void PanelMap_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is UCTableView)
            {
                this.ucTableViews.Remove((UCTableView)e.Control);
            }
        }

        private void PanelMap_ControlAdded(object sender, ControlEventArgs e)
        {
            if(e.Control is UCTableView)
            {
                this.ucTableViews.Add((UCTableView)e.Control);
            }
        }

        private void UCTableRelMap_MouseMove(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
        }


        private void AdjustLoaction(UCTableView view)
        {
            //原则 调下不调上，调右不调左
            int offsetx = 0, offsety = 0;
            int paddingx = 20, paddingy = 30;
            var rect = new Rectangle(view.Location, view.Size);
            foreach(var v in ucTableViews)
            {
                if (v.Equals(view))
                {
                    continue;
                }
                var rect2 = new Rectangle(v.Location, v.Size);

                if (rect.IntersectsWith(rect2) || rect2.IntersectsWith(rect))
                {
                    if (view.Location.X >= v.Location.X && view.Location.X <= v.Location.X + v.Width)
                    {
                        offsetx = Math.Max(offsetx, v.Location.X + v.Width - view.Location.X + paddingx);
                    }

                    if (view.Location.Y >= v.Location.Y && view.Location.Y <= v.Location.Y + v.Height)
                    {
                        offsety = Math.Max(offsety, v.Location.Y + v.Height - view.Location.Y + paddingy);
                    }
                }
            }

            if (offsetx > 0 || offsety > 0)
            {
                foreach (var v in ucTableViews)
                {
                    var dx = v.Location.X >= view.Location.X ? offsetx : 0;
                    var dy = v.Location.Y >= view.Location.Y ? offsety : 0;
                    if (dx > 0 || dy > 0)
                    {
                        var loc = v.Location;
                        loc.Offset(dx, dy);
                        v.Location = loc;
                    }
                }

                if (!ucTableViews.Contains(view))
                {
                    var loc = view.Location;
                    loc.Offset(offsetx, offsety);
                    view.Location = loc;
                }
            }

        }

        private void 添加表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var location = lastLocation;
            UCTableView tv = new UCTableView(DBSource, DBName, null, () =>
              {
                  var list = new List<string>();
                  foreach (var tb in ucTableViews)
                  {
                      list.Add(tb.TableName);
                  }
                  return lzTableList.Value.Where(p => !list.Contains(p)).ToList();
              }, v => { v.Location = location; AdjustLoaction(v); },
              (p1,p2,b)=>
              {

                  if (!this.Bounds.Contains(p1) || !this.Bounds.Contains(p2))
                  {
                      return true;
                  }

                  var line = new Line(p1, p2);

                  var boo =b&& DrawingUtil.HasIntersect(ClientRectangle, line);
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

                      boo = b&&DrawingUtil.HasIntersect(newrect, line);
                      if (boo)
                      {
                          return true;
                      }
                  }
                  return false;
              });
            tv.Location = location;
            this.PanelMap.Controls.Add(tv);
        }
    }
}
