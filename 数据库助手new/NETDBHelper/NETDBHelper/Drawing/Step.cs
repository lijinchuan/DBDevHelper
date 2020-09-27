using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETDBHelper.Drawing
{
    public class Step
    {
        public Point Pos
        {
            get;
            set;
        }

        /// <summary>
        /// 可供选择的方向
        /// </summary>
        public List<StepDirection> Directions
        {
            get;
            set;
        } = new List<StepDirection>
        {
            StepDirection.left,
            StepDirection.right,
            StepDirection.up,
            StepDirection.down
        };

        public void SetFirst(StepDirection stepDirection)
        {
            if (Directions.Contains(stepDirection))
            {
                Directions.Remove(stepDirection);
                Directions.Insert(0, stepDirection);
            }
        }

        /// <summary>
        /// 步长
        /// </summary>
        public List<int> StepLens
        {
            get;
            set;
        }

        public StepDirection chooseDirection
        {
            get;
            set;
        } = StepDirection.none;

        public void Offset(int dx, int dy)
        {
            Pos = new Point(Pos.X + dx, Pos.Y + dy);
        }
    }
}
