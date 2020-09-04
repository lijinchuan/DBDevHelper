using LJC.FrameWork.LogManager;
using NPOI;
using NPOI.OpenXmlFormats.Vml;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NETDBHelper.Drawing
{
    public class StepSelector
    {
        private Stack<Step> steps = new Stack<Step>();
        private static int defaultStepLen = 5;
        private Func<Point, Point,bool, bool> checkStepHasConflict = null;

        private Point startPoint, secDestPoint,destPoint;
        private StepDirection destDirection;
        private StepDirection firstDirection;

        public StepSelector(Point start,Point dest, Func<Point, Point,bool, bool> funcCheckStep, 
            StepDirection _firstDirection=StepDirection.none,
            StepDirection _destDirection = StepDirection.none)
        {
            startPoint = start;
            destPoint = dest;
            firstDirection = _firstDirection;

            checkStepHasConflict = funcCheckStep;
            destDirection = _destDirection;
        }

        private bool Check(Point p1,Point p2,bool isline)
        {
            if (!isline)
            {
                if ((Math.Abs(p1.X - startPoint.X) < 10 && Math.Abs(p2.X - destPoint.X) < 10)
                    || (Math.Abs(p1.X - startPoint.X) < 10 && Math.Abs(p2.X - destPoint.X) < 10))
                {
                    return false;
                }
            }

            return checkStepHasConflict(p1, p2, isline);
        }

        private void Pepare()
        {
            steps.Clear();
            var step = new Step
            {
                Pos = startPoint,
                chooseDirection = firstDirection
            };
            steps.Push(step);

            if (destDirection != StepDirection.none)
            {
                switch (destDirection)
                {
                    case StepDirection.left:
                        {
                            secDestPoint = new Point(destPoint.X + 20, destPoint.Y);
                            while (secDestPoint.X>destPoint.X)
                            {
                                if (!Check(secDestPoint, destPoint,false))
                                {
                                    break;
                                }
                                secDestPoint.X -= 3;
                            }
                            break;
                        }
                    case StepDirection.right:
                        {
                            secDestPoint = new Point(destPoint.X - 20, destPoint.Y);
                            while (secDestPoint.X < destPoint.X)
                            {
                                if (!Check(secDestPoint, destPoint,false))
                                {
                                    break;
                                }
                                secDestPoint.X += 3;
                            }
                            break;
                        }
                    default:
                        {
                            secDestPoint = destPoint;
                            break;
                        }
                }
            }
            else
            {
                secDestPoint = destPoint;
            }
        }

        private bool ChooseDirection(Step current)
        {
            if (current.Directions.Count == 0)
            {
                return false;
            }

            if (current.chooseDirection != StepDirection.none)
            {
                return true;
            }

            var seldirect = StepDirection.none;
            if (secDestPoint.X == current.Pos.X && secDestPoint.Y == current.Pos.Y)
            {
                seldirect = StepDirection.same;
            }
            #region 基本转向
            else if (secDestPoint.X > current.Pos.X && current.Directions.Contains(StepDirection.right))
            {
                seldirect = StepDirection.right;
            }
            else if (secDestPoint.X < current.Pos.X && current.Directions.Contains(StepDirection.left))
            {
                seldirect = StepDirection.left;
            }
            else if (secDestPoint.Y > current.Pos.Y && current.Directions.Contains(StepDirection.down))
            {
                seldirect = StepDirection.down;
            }
            else if (secDestPoint.Y < current.Pos.Y && current.Directions.Contains(StepDirection.up))
            {
                seldirect = StepDirection.up;
            }
            #endregion
            #region 右转时优先上下方向 ，防止左转
            else if (secDestPoint.X > current.Pos.X && (current.Directions.Contains(StepDirection.up)
                || current.Directions.Contains(StepDirection.down)))
            {
                if (secDestPoint.Y > current.Pos.Y && current.Directions.Contains(StepDirection.down))
                {
                    seldirect = StepDirection.down;
                }
                else if (secDestPoint.Y < current.Pos.Y && current.Directions.Contains(StepDirection.up))
                {
                    seldirect = StepDirection.up;
                }
            }
            #endregion
            #region 左转优先上下，防止右转
            else if (secDestPoint.X < current.Pos.X && (current.Directions.Contains(StepDirection.down)
                || current.Directions.Contains(StepDirection.up)))
            {
                if (secDestPoint.Y > current.Pos.Y && current.Directions.Contains(StepDirection.down))
                {
                    seldirect = StepDirection.down;
                }
                else if (secDestPoint.Y < current.Pos.Y && current.Directions.Contains(StepDirection.up))
                {
                    seldirect = StepDirection.up;
                }
            }
            #endregion
            #region 下转防止向上
            else if (secDestPoint.Y > current.Pos.Y && (current.Directions.Contains(StepDirection.left)
                || current.Directions.Contains(StepDirection.right)))
            {
                if (secDestPoint.X < current.Pos.X && current.Directions.Contains(StepDirection.left))
                {
                    seldirect = StepDirection.left;
                }
                else if (secDestPoint.X > current.Pos.X && current.Directions.Contains(StepDirection.right))
                {
                    seldirect = StepDirection.right;
                }
            }
            #endregion
            #region  上转防止向下
            else if (secDestPoint.Y < current.Pos.Y && (current.Directions.Contains(StepDirection.left)
                || current.Directions.Contains(StepDirection.right)))
            {
                if (secDestPoint.X < current.Pos.X && current.Directions.Contains(StepDirection.left))
                {
                    seldirect = StepDirection.left;
                }
                else if (secDestPoint.X > current.Pos.X && current.Directions.Contains(StepDirection.right))
                {
                    seldirect = StepDirection.right;
                }
            }
            #endregion

            if (seldirect == StepDirection.none)
            {
                seldirect = current.Directions.First();
            }
            current.chooseDirection = seldirect;
            return true;
        }

        public List<Point> Select()
        {
            this.Pepare();
            StepSelectorTrace trace = new StepSelectorTrace(this.startPoint, this.secDestPoint);
            while (true)
            {
                if (steps.Count > 10000)
                {
                    steps.Clear();
                    break;
                }
                if (steps.Count == 0)
                {
                    break;
                }
                var currstep = steps.Peek();
                if (currstep == null || currstep.Directions.Count == 0)
                {
                    break;
                }

                var boo = ChooseDirection(currstep);
                trace.Select();
                if (!boo)
                {
                    steps.Pop();
                }

                if (currstep.chooseDirection == StepDirection.same)
                {
                    break;
                }

                var nextstep = new Step()
                {
                    Pos = currstep.Pos
                };
                nextstep.SetFirst(currstep.chooseDirection);
                int stepLen = defaultStepLen;
                switch (currstep.chooseDirection)
                {
                    case StepDirection.left:
                        {
                            nextstep.Directions.Remove(StepDirection.right);
                            var offsetx = nextstep.Pos.X - secDestPoint.X;
                            if (offsetx > 0 && offsetx < stepLen)
                            {
                                stepLen = offsetx;
                            }
                            nextstep.Offset(-stepLen, 0);
                            break;
                        }
                    case StepDirection.right:
                        {
                            nextstep.Directions.Remove(StepDirection.left);
                            var offsetx = secDestPoint.X - nextstep.Pos.X;
                            if (offsetx > 0 && offsetx < stepLen)
                            {
                                stepLen = offsetx;
                            }
                            nextstep.Offset(stepLen, 0);
                            break;
                        }
                    case StepDirection.up:
                        {
                            nextstep.Directions.Remove(StepDirection.down);
                            var offsety = nextstep.Pos.Y - secDestPoint.Y;
                            if (offsety > 0 && offsety < stepLen)
                            {
                                stepLen = offsety;
                            }
                            nextstep.Offset(0, -stepLen);
                            break;
                        }
                    case StepDirection.down:
                        {
                            nextstep.Directions.Remove(StepDirection.up);
                            var offsety = secDestPoint.Y - nextstep.Pos.Y;
                            if (offsety > 0 && offsety < stepLen)
                            {
                                stepLen = offsety;
                            }
                            nextstep.Offset(0, stepLen);
                            break;
                        }
                }

                if (!Check(currstep.Pos, nextstep.Pos,true)
                    || (Math.Abs(nextstep.Pos.X - startPoint.X) <= 0 && Math.Abs(nextstep.Pos.Y - startPoint.Y) <= 5)
                    || (Math.Abs(nextstep.Pos.X - startPoint.X) <= 5 && Math.Abs(nextstep.Pos.Y - startPoint.Y) <= 0)
                    || (Math.Abs(nextstep.Pos.X - secDestPoint.X)<=0 && Math.Abs(nextstep.Pos.Y - secDestPoint.Y)<=5)
                    || (Math.Abs(nextstep.Pos.X - secDestPoint.X) <= 5 && Math.Abs(nextstep.Pos.Y - secDestPoint.Y) <= 0))
                {
                    steps.Push(nextstep);
                }
                else
                {
                    currstep.Directions.Remove(currstep.chooseDirection);
                    currstep.chooseDirection = StepDirection.none;
                    if (currstep.Directions.Count == 0)
                    {
                        steps.Pop();
                        while (steps.Count > 0)
                        {
                            var father = steps.Peek();
                            father.Directions.Remove(father.chooseDirection);
                            father.chooseDirection = StepDirection.none;
                            if (father.Directions.Count > 0)
                            {
                                break;
                            }
                            else
                            {
                                steps.Pop();
                            }
                        }
                    }
                }

            }

            List<Step> steplist = new List<Step>();
            while (steps.Count > 0)
            {
                steplist.Add(steps.Pop());
            }
            var orgstepcount = steplist.Count;
            if (steplist.Count > 1)
            {
                steplist.Reverse();
                steplist = CutSteps(steplist);
            }

            List<Point> result = steplist.Select(p => p.Pos).ToList();

            if (secDestPoint.X != destPoint.X && result.Count > 0)
            {
                result.Add(destPoint);
            }

            result = CutResult(result);

            trace.Result(result.Count);

            LogHelper.Instance.Debug(trace.ToString());

            return result;
        }

        private StepDirection GetOpDirection(StepDirection direction)
        {
            var opDirection = StepDirection.none;
            switch (direction)
            {
                case StepDirection.left:
                    {
                        opDirection = StepDirection.right;
                        break;
                    }
                case StepDirection.right:
                    {
                        opDirection = StepDirection.left;
                        break;
                    }
                case StepDirection.up:
                    {
                        opDirection = StepDirection.down;
                        break;
                    }
                case StepDirection.down:
                    {
                        opDirection = StepDirection.up;
                        break;
                    }
            }

            return opDirection;
        }

        private List<Step> CutSteps(List<Step> steps)
        {
            if (steps.Count > 1)
            {
                //去掉有回路的情况
                var steparray = steps.ToArray();

                //相同方向的点只取第一个点和最后一个点，大大减少点数，提高绘图效率
                List<Step> steps2 = new List<Step>();
                steps2.Add(steparray[0]);
                for (int i = 1; i < steparray.Length; i++)
                {
                    if (steparray[i].chooseDirection != steparray[i - 1].chooseDirection)
                    {
                        if (i > 1 && i < steparray.Length - 1)
                        {
                            steps2.Add(steparray[i - 1]);
                        }
                        steps2.Add(steparray[i]);
                    }
                }

                steparray = steps2.ToArray();

                //将线条撸直
                var prestep = steparray.First();
                for (var i = 1; i < steparray.Length; i++)
                {
                    if (steparray[i].chooseDirection == prestep.chooseDirection)
                    {
                        continue;
                    }

                    //i是最后一个与起始方向一致的点的后一个点

                    for (int j = i + 1; j < steparray.Length; j++)
                    {
                        if (steparray[j].chooseDirection == prestep.chooseDirection)
                        {
                            //j是后面与主方向一致的点
                            for (int k = j + 1; k < steparray.Length; k++)
                            {
                                if (steparray[k].chooseDirection != prestep.chooseDirection)
                                {
                                    //k是后面与主方向不一致的点
                                    Step joinstep = new Step();
                                    if (prestep.chooseDirection == StepDirection.left || prestep.chooseDirection == StepDirection.right)
                                    {
                                        joinstep.Pos = new Point(steparray[k].Pos.X, steparray[i - 1].Pos.Y);

                                        if (steparray[i - 1].Pos.X < steparray[k].Pos.X)
                                        {
                                            joinstep.chooseDirection = StepDirection.right;
                                        }
                                        else
                                        {
                                            joinstep.chooseDirection = StepDirection.left;
                                        }
                                    }
                                    else
                                    {
                                        joinstep.Pos = new Point(steparray[i - 1].Pos.X, steparray[k].Pos.Y);

                                        if (steparray[i - 1].Pos.Y < steparray[k].Pos.Y)
                                        {
                                            joinstep.chooseDirection = StepDirection.down;
                                        }
                                        else
                                        {
                                            joinstep.chooseDirection = StepDirection.up;
                                        }
                                    }

                                    if (!Check(steparray[i - 1].Pos, joinstep.Pos, true)
                                            && !Check(joinstep.Pos, steparray[k].Pos, true))
                                    {
                                        List<Step> list = new List<Step>();
                                        for (var m = 0; m < i; m++)
                                        {
                                            list.Add(steparray[m]);
                                        }
                                        list.Add(joinstep);

                                        for (var m = k; m < steparray.Length; m++)
                                        {
                                            var s = steparray[m];
                                            list.Add(s);
                                        }

                                        steparray = list.ToArray();

                                    }
                                }
                            }
                        }
                    }

                    prestep = steparray[i];
                }

                //反向简化处理
                var preDirection = steparray.First().chooseDirection;
                for (var i = 1; i < steparray.Length; i++)
                {
                    if (steparray[i].chooseDirection == preDirection)
                    {
                        continue;
                    }
                    if (steparray[i].chooseDirection != preDirection)
                    {
                        var opDirection = GetOpDirection(preDirection);
                        if (opDirection == StepDirection.none)
                        {
                            continue;
                        }
                        for (var j = i + 1; j < steparray.Length; j++)
                        {
                            if (steparray[j].chooseDirection == opDirection)
                            {
                                for (var k = j + 1; k < steparray.Length; k++)
                                {
                                    if (steparray[k].chooseDirection != opDirection)
                                    {
                                        Step joinstep = new Step();
                                        if (preDirection == StepDirection.left || preDirection == StepDirection.right)
                                        {
                                            joinstep.Pos = new Point(steparray[k].Pos.X, steparray[i].Pos.Y);
                                            if (steparray[i].Pos.Y < steparray[k].Pos.Y)
                                            {
                                                joinstep.chooseDirection = StepDirection.down;
                                            }
                                            else
                                            {
                                                joinstep.chooseDirection = StepDirection.up;
                                            }
                                        }
                                        else
                                        {
                                            joinstep.Pos = new Point(steparray[i].Pos.X, steparray[k].Pos.Y);
                                            if (steparray[i].Pos.X < steparray[k].Pos.X)
                                            {
                                                joinstep.chooseDirection = StepDirection.right;
                                            }
                                            else
                                            {
                                                joinstep.chooseDirection = StepDirection.left;
                                            }
                                        }

                                        if (!Check(steparray[i - 1].Pos, joinstep.Pos, true) && !Check(joinstep.Pos, steparray[k].Pos, true))
                                        {
                                            var list = new List<Step>();
                                            for (var m = 0; m < i - 1; m++)
                                            {
                                                list.Add(steparray[m]);
                                            }
                                            list.Add(joinstep);
                                            for (var m = k; m < steparray.Length; m++)
                                            {
                                                var s = steparray[m];
                                                list.Add(s);
                                            }

                                            steparray = list.ToArray();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    preDirection = steparray[i].chooseDirection;
                }

                if (steparray.Length != steps.Count)
                {
                    steps = steparray.ToList();
                }

            }

            return steps;
        }

        private List<Point> CutResult(List<Point> points)
        {
            if (points.Count > 0)
            {
                var ps = points.ToArray();
                //T形处理
                for (var i = 1; i < ps.Length - 1; i++)
                {
                    if ((ps[i].X == ps[i - 1].X && ps[i].X == ps[i + 1].X
                        && ps[i].Y > ps[i - 1].Y == ps[i].Y > ps[i + 1].Y)
                        || (ps[i].Y == ps[i - 1].Y && ps[i].Y == ps[i + 1].Y
                       && ps[i].X > ps[i - 1].X == ps[i].X > ps[i + 1].X))
                    {
                        var li = ps.ToList();
                        li.RemoveAt(i);
                        ps = li.ToArray();
                        i--;
                    }
                }

                if (ps.Length != points.Count)
                {
                    points = ps.ToList();
                }
            }
            return points;
        }
    }
}
