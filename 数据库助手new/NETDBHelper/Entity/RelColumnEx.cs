using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Entity
{
    public class RelColumnEx
    {
        public RelColumn RelColumn
        {
            get;
            set;
        }

        public Point Start
        {
            get;
            set;
        }

        public Point Dest
        {
            get;
            set;
        }

        public Point[] LinkLines
        {
            get;
            set;
        } = new Point[0];
    }
}
