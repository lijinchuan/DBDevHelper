using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETDBHelper.Drawing
{
    public class Line
    {
        public Point Start
        {
            get;
            set;
        }

        public Point End
        {
            get;
            set;
        }

        public Line(Point p1,Point p2)
        {
            this.Start = p1;
            this.End = p2;
        }

        public void Change()
        {
            var end = this.End;
            this.End = this.Start;
            this.Start = end;
        }
    }
}
