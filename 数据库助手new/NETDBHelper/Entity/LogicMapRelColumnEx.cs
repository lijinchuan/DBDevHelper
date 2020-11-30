using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Entity
{
    public class LogicMapRelColumnEx
    {
        public LogicMapRelColumn RelColumn
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

        public Color LineColor
        {
            get;
            set;
        }

        /// <summary>
        /// 描述框架位置
        /// </summary>
        public Rectangle DescRect
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是热点
        /// </summary>
        public bool IsHotLine
        {
            get;
            set;
        }
    }
}
