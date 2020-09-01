using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NETDBHelper.Drawing
{
    public static class DrawingUtil
    {
        /// <summary>
        /// 判断两条线是否相交
        /// </summary>
        /// <param name="a">线段1起点坐标</param>
        /// <param name="b">线段1终点坐标</param>
        /// <param name="c">线段2起点坐标</param>
        /// <param name="d">线段2终点坐标</param>
        /// <param name="intersection">相交点坐标</param>
        /// <returns>是否相交 0:两线平行  -1:不平行且未相交  1:两线相交</returns>
        public static int GetIntersection(Point a, Point b, Point c, Point d, ref Point intersection)
        {
            //判断异常
            if (Math.Abs(b.X - a.Y) + Math.Abs(b.X - a.X) + Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                if (c.X - a.X == 0)
                {
                    //ABCD是同一个点！;
                }
                else
                {
                    //AB是一个点，CD是一个点，且AC不同！;
                }
                return 0;
            }

            if (Math.Abs(b.Y - a.Y) + Math.Abs(b.X - a.X) == 0)
            {
                if ((a.X - d.X) * (c.Y - d.Y) - (a.Y - d.Y) * (c.X - d.X) == 0)
                {
                    //"A、B是一个点，且在CD线段上！;
                }
                else
                {
                    //A、B是一个点，且不在CD线段上！;
                }
                return 0;
            }
            if (Math.Abs(d.Y - c.Y) + Math.Abs(d.X - c.X) == 0)
            {
                if ((d.X - b.X) * (a.Y - b.Y) - (d.Y - b.Y) * (a.X - b.X) == 0)
                {
                    //C、D是一个点，且在AB线段上！;
                }
                else
                {
                    //C、D是一个点，且不在AB线段上;
                }
            }

            if ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y) == 0)
            {
                return 0;
            }

            intersection.X = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            intersection.Y = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));

            if ((intersection.X - a.X) * (intersection.X - b.X) <= 0 && (intersection.X - c.X) * (intersection.X - d.X) <= 0 && (intersection.Y - a.Y) * (intersection.Y - b.Y) <= 0 && (intersection.Y - c.Y) * (intersection.Y - d.Y) <= 0)
            {
                return 1; //'相交
            }
            else
            {
                return -1; //'相交但不在线段上
            }
        }

        private static bool HasIntersect(Line line1, Line line2)
        {
            Point ret = Point.Empty;
            var retval = GetIntersection(line1.Start, line1.End, line2.Start, line2.End, ref ret);
            return retval == 1;
        }

        public static bool HasIntersect(Rectangle rectangle, Line line)
        {
            var p1 = new Point(rectangle.X, rectangle.Y);
            var p2 = new Point(rectangle.X, rectangle.Y + rectangle.Height);
            var p3 = new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            var p4 = new Point(rectangle.X + rectangle.Width, rectangle.Y);
            return HasIntersect(new Line(p1, p2), line) || HasIntersect(new Line(p2, p3), line)
                || HasIntersect(new Line(p3, p4), line) || HasIntersect(new Line(p1, p4), line);
        }

        public static bool Isoverlap(Line line1, Line line2)
        {
            if (line1.Start.X == line1.End.X)
            {
                if (line2.Start.X != line2.End.X || line2.Start.X != line1.Start.X)
                {
                    return false;
                }

                return line2.Start.Y <= line1.End.Y && line2.End.Y >= line1.Start.Y;
            }
            else if (line1.Start.Y == line1.End.Y)
            {
                if (line2.Start.Y != line2.End.Y || line2.Start.Y != line1.Start.Y)
                {
                    return false;
                }

                return line2.Start.X <= line1.End.X && line2.End.X >= line1.Start.X;
            }

            return false;
        }

    }
}
