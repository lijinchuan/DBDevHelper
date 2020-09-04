using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETDBHelper.Drawing
{
    public class StepSelectorTrace
    {
        private Point startPoint, destPoint;

        public StepSelectorTrace(Point start,Point dest)
        {
            this.startPoint = start;
            this.destPoint = dest;
            Start = DateTime.Now;
        }

        private int SelectCount
        {
            get;
            set;
        }

        private DateTime Start
        {
            get;
            set;
        }

        private DateTime End
        {
            get;
            set;
        }

        private int ResultCount
        {
            get;
            set;
        }

        public void StartTrace()
        {
            Start = DateTime.Now;
        }

        public void StopTrace()
        {
            End = DateTime.Now;
        }

        public void Select()
        {
            SelectCount++;
        }

        public void Result(int resultcount)
        {
            ResultCount = resultcount;
        }

        public override string ToString()
        {
            if (End == DateTime.MinValue)
            {
                End = DateTime.Now;
            }
            return $"规划路线，用时：{End.Subtract(Start).TotalMilliseconds}ms,start{startPoint.X},{startPoint.Y},end:{destPoint.X},{destPoint.Y},规划成功:{ResultCount > 1},原始步数:{SelectCount},整合输出步数:{ResultCount}";

        }
    }
}
