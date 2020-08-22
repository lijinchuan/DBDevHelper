using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Func<Step, Step, bool> checkStepHasConflict = null;

        private Point startPoint,destPoint;
        private StepDirection fstDirection;

        public StepSelector(Point start,Point dest, Func<Step, Step, bool> funcCheckStep, StepDirection firstDirection=StepDirection.none)
        {
            startPoint = start;
            destPoint = dest;
            fstDirection = firstDirection;

            checkStepHasConflict = funcCheckStep;
        }

        private void Pepare()
        {
            steps.Clear();
            var step = new Step
            {
                Pos = startPoint,
                chooseDirection = fstDirection
            };
            steps.Push(step);
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
            if (destPoint.X == current.Pos.X && destPoint.Y == current.Pos.Y)
            {
                seldirect = StepDirection.same;
            }
            #region 基本转向
            else if (destPoint.X > current.Pos.X && current.Directions.Contains(StepDirection.right))
            {
                seldirect = StepDirection.right;
            }
            else if (destPoint.X < current.Pos.X && current.Directions.Contains(StepDirection.left))
            {
                seldirect = StepDirection.left;
            }
            else if (destPoint.Y > current.Pos.Y && current.Directions.Contains(StepDirection.down))
            {
                seldirect = StepDirection.down;
            }
            else if (destPoint.Y < current.Pos.Y && current.Directions.Contains(StepDirection.up))
            {
                seldirect = StepDirection.up;
            }
            #endregion
            #region 右转时优先上下方向 ，防止左转
            else if (destPoint.X > current.Pos.X && (current.Directions.Contains(StepDirection.up)
                || current.Directions.Contains(StepDirection.down)))
            {
                if (destPoint.Y > current.Pos.Y && current.Directions.Contains(StepDirection.down))
                {
                    seldirect = StepDirection.down;
                }
                else if (destPoint.Y < current.Pos.Y && current.Directions.Contains(StepDirection.up))
                {
                    seldirect = StepDirection.up;
                }
            }
            #endregion
            #region 左转优先上下，防止右转
            else if (destPoint.X < current.Pos.X && (current.Directions.Contains(StepDirection.down)
                || current.Directions.Contains(StepDirection.up)))
            {
                if (destPoint.Y > current.Pos.Y && current.Directions.Contains(StepDirection.down))
                {
                    seldirect = StepDirection.down;
                }
                else if (destPoint.Y < current.Pos.Y && current.Directions.Contains(StepDirection.up))
                {
                    seldirect = StepDirection.up;
                }
            }
            #endregion
            #region 下转防止向上
            else if (destPoint.Y > current.Pos.Y && (current.Directions.Contains(StepDirection.left)
                || current.Directions.Contains(StepDirection.right)))
            {
                if (destPoint.X < current.Pos.X && current.Directions.Contains(StepDirection.left))
                {
                    seldirect = StepDirection.left;
                }
                else if (destPoint.X > current.Pos.X && current.Directions.Contains(StepDirection.right))
                {
                    seldirect = StepDirection.right;
                }
            }
            #endregion
            #region  上转防止向下
            else if (destPoint.Y < current.Pos.Y && (current.Directions.Contains(StepDirection.left)
                || current.Directions.Contains(StepDirection.right)))
            {
                if (destPoint.X < current.Pos.X && current.Directions.Contains(StepDirection.left))
                {
                    seldirect = StepDirection.left;
                }
                else if (destPoint.X > current.Pos.X && current.Directions.Contains(StepDirection.right))
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
            while (true)
            {
                if (steps.Count > 100000)
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
                            var offsetx = nextstep.Pos.X - destPoint.X;
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
                            var offsetx = destPoint.X - nextstep.Pos.X;
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
                            var offsety = nextstep.Pos.Y - destPoint.Y;
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
                            var offsety = destPoint.Y - nextstep.Pos.Y;
                            if (offsety > 0 && offsety < stepLen)
                            {
                                stepLen = offsety;
                            }
                            nextstep.Offset(0, stepLen);
                            break;
                        }
                }

                if (nextstep.Pos.X > 0 && nextstep.Pos.Y > 0 && (!checkStepHasConflict(currstep, nextstep)
                    || (Math.Abs(nextstep.Pos.X - startPoint.X) <= 0 && Math.Abs(nextstep.Pos.Y - startPoint.Y) <= 30)
                    || (Math.Abs(nextstep.Pos.X - startPoint.X) <= 30 && Math.Abs(nextstep.Pos.Y - startPoint.Y) <= 0)
                    || (Math.Abs(nextstep.Pos.X - destPoint.X)<=0 && Math.Abs(nextstep.Pos.Y - destPoint.Y)<=30)
                    || (Math.Abs(nextstep.Pos.X - destPoint.X) <= 30 && Math.Abs(nextstep.Pos.Y - destPoint.Y) <= 0)))
                {
                    steps.Push(nextstep);
                    //if (nextstep.Pos.X != destPoint.X && nextstep.Pos.Y != destPoint.Y)
                     //   nextstep.chooseDirection = currstep.chooseDirection;
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

            if (steplist.Count > 1)
            {
                steplist.Reverse();
                steplist = CutSteps(steplist);
            }

            List<Point> result = steplist.Select(p => p.Pos).ToList();
            result = CutResult(result);
            return result;
        }

        private List<Step> CutSteps(List<Step> steps)
        {
            if (steps.Count > 1)
            {
                var d = steps.First().chooseDirection;
                var steparray = steps.ToArray();
                for (int i = 1; i < steparray.Length; i++)
                {
                    if (steparray[i].chooseDirection == d)
                    {
                        continue;
                    }
                    if (i < steparray.Length - 2)
                    {
                        var nextstep = steparray[i + 1];
                        StepDirection opdirection = StepDirection.none;
                        if (nextstep.chooseDirection == StepDirection.left)
                        {
                            opdirection = StepDirection.right;
                        }
                        if (nextstep.chooseDirection == StepDirection.right)
                        {
                            opdirection = StepDirection.left;
                        }
                        if (nextstep.chooseDirection == StepDirection.up)
                        {
                            opdirection = StepDirection.down;
                        }
                        if (nextstep.chooseDirection == StepDirection.down)
                        {
                            opdirection = StepDirection.up;
                        }
                        for (var j = i + 2; j < steparray.Length; j++)
                        {
                            if (steparray[j].chooseDirection == opdirection
                                && ((steparray[j].Pos.X == nextstep.Pos.X && Math.Abs(steparray[j].Pos.Y - nextstep.Pos.Y) <= 5)
                                || (steparray[j].Pos.Y == nextstep.Pos.Y && Math.Abs(steparray[j].Pos.X - nextstep.Pos.X) <= 5)))
                            {
                                int k = -1;
                                d = steparray[j].chooseDirection;
                                steparray = steps.Where(p =>
                                {
                                    k++;
                                    return k <= i || k > j;
                                }).ToArray();

                            }
                        }
                    }
                }

                List<Step> steps2 = new List<Step>();
                steps2.Add(steparray[0]);
                for (int i = 1; i < steparray.Length - 1; i++)
                {
                    if (steparray[i].chooseDirection != steparray[i - 1].chooseDirection)
                    {
                        if (i > 1)
                        {
                            steps2.Add(steparray[i - 1]);
                        }
                        steps2.Add(steparray[i]);
                    }
                }
                if (steparray.Length > 2)
                {
                    steps2.Add(steparray[steparray.Length - 1]);
                }
                if (steparray.Length != steps2.Count)
                {
                    steparray = steps2.ToArray();
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
                var gp = points.GroupBy(p => p);
                int minfirst = points.Count, maxlast = -1;
                foreach (var kv in gp.Where(p => p.Count() > 1))
                {
                    int first = 0, last = 0;
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].X == kv.Key.X && points[i].Y == kv.Key.Y)
                        {
                            if (first == 0)
                            {
                                first = i;
                            }
                            else
                            {
                                last = i;
                            }
                        }
                    }
                    if (first <= minfirst && last >= maxlast)
                    {
                        minfirst = first;
                        maxlast = last;
                    }
                }

                if (maxlast > minfirst)
                {
                    int i = -1;
                    points = points.Where(p =>
                    {
                        i++;
                        return i <= minfirst || i > maxlast;
                    }).ToList();
                }
            }
            return points;
        }
    }
}
