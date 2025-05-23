﻿using LJC.FrameWork.LogManager;
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

namespace APIHelper.Drawing
{
    public class StepSelector
    {
        private Stack<Step> steps = new Stack<Step>();
        HashSet<Point> stephash = new HashSet<Point>();
        //最大只能是20
        private static List<int> defaultStepLens = new List<int> { 20, 10, 5, 1 };
        static int paddingmax = defaultStepLens.Max();
        static int defalutStep = 5;
        private Func<Point, Point,bool, bool> checkStepHasConflict = null;

        private Point startPoint, secStartPoint, secDestPoint, destPoint;
        private StepDirection destDirection;
        private StepDirection firstDirection;

        private int width = 1000, height = 1000;


        public StepSelector(int _widht,int _height,Point start,Point dest, Func<Point, Point,bool, bool> funcCheckStep, 
            StepDirection _firstDirection=StepDirection.none,
            StepDirection _destDirection = StepDirection.none)
        {
            width = _widht;
            height = _height;

            startPoint = start;
            destPoint = dest;
            firstDirection = _firstDirection;

            checkStepHasConflict = funcCheckStep;
            destDirection = _destDirection;
        }

        private bool Check(Point p1,Point p2,bool isline)
        {
            if (p1.X < 0 || p1.Y < 0 || p1.X > width || p1.Y > height)
            {
                return true;
            }

            if (p2.X < 0 || p2.Y < 0 || p2.X > width || p2.Y > height)
            {
                return true;
            }

            if (!isline)
            {
                if ((Math.Abs(p1.X - startPoint.X) < 10 && Math.Abs(p2.X - destPoint.X) < 10)
                    || (Math.Abs(p1.X - startPoint.X) < 10 && Math.Abs(p2.X - destPoint.X) < 10))
                {
                    return false;
                }
            }
            else
            {
                if (p1.X != p2.X && p1.Y != p2.Y)
                {
                    return true;
                }
            }

            return checkStepHasConflict(p1, p2, isline);
        }

        private void Pepare()
        {
            steps.Clear();
            stephash.Clear();

            if (firstDirection != StepDirection.none)
            {
                switch (firstDirection)
                {
                    case StepDirection.left:
                        {
                            secStartPoint = new Point(startPoint.X - paddingmax, startPoint.Y);
                            while (secStartPoint.X < startPoint.X - 5)
                            {
                                if (!Check(secStartPoint, startPoint, false))
                                {
                                    break;
                                }
                                secStartPoint.X += 1;
                            }
                            break;
                        }
                    case StepDirection.right:
                        {
                            secStartPoint = new Point(startPoint.X + paddingmax, startPoint.Y);
                            while (secStartPoint.X > startPoint.X + 5)
                            {
                                if (!Check(secStartPoint, startPoint, false))
                                {
                                    break;
                                }
                                secStartPoint.X -= 1;
                            }
                            break;
                        }
                    default:
                        {
                            secStartPoint = startPoint;
                            break;
                        }
                }
            }
            else
            {
                secStartPoint = startPoint;
            }

            if (destDirection != StepDirection.none)
            {
                switch (destDirection)
                {
                    case StepDirection.left:
                        {
                            secDestPoint = new Point(destPoint.X + paddingmax, destPoint.Y);
                            while (secDestPoint.X>destPoint.X)
                            {
                                if (!Check(secDestPoint, destPoint,false))
                                {
                                    break;
                                }
                                secDestPoint.X -= 1;
                            }
                            break;
                        }
                    case StepDirection.right:
                        {
                            secDestPoint = new Point(destPoint.X - paddingmax, destPoint.Y);
                            while (secDestPoint.X < destPoint.X)
                            {
                                if (!Check(secDestPoint, destPoint,false))
                                {
                                    break;
                                }
                                secDestPoint.X += 1;
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

            var step = new Step
            {
                Pos = secStartPoint,
                chooseDirection = firstDirection,
                StepLens = defaultStepLens.Select(p => p).ToList()
            };
            steps.Push(step);
        }

        private StepDirection GetLastDirection()
        {
            if (steps.Count > 1)
            {
                var laststep = steps.Pop();
                var direction = steps.Peek().chooseDirection;
                steps.Push(laststep);
                return direction;
            }

            return StepDirection.none;
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
                var lastdirection = GetLastDirection();
                if (steps.Count > 0 && current.Directions.Contains(lastdirection))
                {
                    seldirect = lastdirection;
                }

                if (seldirect == StepDirection.none)
                {
                    seldirect = current.Directions.First();
                }
            }

            #region
            if (seldirect != StepDirection.same)
            {
                if (secDestPoint.Y == current.Pos.Y && Math.Abs(secStartPoint.X - current.Pos.X) >= paddingmax)
                {
                    if ((secDestPoint.X > current.Pos.X && seldirect == StepDirection.left)
                        || (secDestPoint.X < current.Pos.X && seldirect == StepDirection.right))
                    {
                        if (current.Directions.Contains(StepDirection.up))
                            seldirect = StepDirection.up;
                        else if (current.Directions.Contains(StepDirection.down))
                            seldirect = StepDirection.down;
                    }
                }
                else if (secDestPoint.X == current.Pos.X && Math.Abs(secStartPoint.Y - current.Pos.Y) >= paddingmax)
                {
                    if ((secDestPoint.Y > current.Pos.Y && seldirect == StepDirection.up)
                        || (secDestPoint.Y < current.Pos.Y && seldirect == StepDirection.down))
                    {
                        if (current.Directions.Contains(StepDirection.right))
                            seldirect = StepDirection.right;
                        else if (current.Directions.Contains(StepDirection.left))
                            seldirect = StepDirection.left;
                    }
                }
            }
            #endregion

            current.chooseDirection = seldirect;
            return true;
        }

        private void ResetTopStep()
        {
            if (steps.Count > 0)
            {
                var step = steps.Peek();
                if (step.StepLens.Count > 0)
                {
                    step.StepLens.RemoveAt(0);
                }

                if (step.StepLens.Count == 0)
                {
                    step.Directions.Remove(step.chooseDirection);

                    step.chooseDirection = StepDirection.none;
                    if (step.Directions.Count == 0)
                    {
                        stephash.Remove(steps.Pop().Pos);
                        while (steps.Count > 0)
                        {
                            var father = steps.Peek();
                            if (father.StepLens.Count > 0)
                            {
                                father.StepLens.RemoveAt(0);
                            }
                            if (father.StepLens.Count == 0)
                            {
                                father.Directions.Remove(father.chooseDirection);
                                father.chooseDirection = StepDirection.none;
                                if (father.Directions.Count > 0)
                                {
                                    break;
                                }
                                else
                                {
                                    stephash.Remove(steps.Pop().Pos);
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<Point> Select()
        {
            this.Pepare();
            StepSelectorTrace trace = new StepSelectorTrace(this.secStartPoint, this.secDestPoint);
            
            while (true)
            {
                if (steps.Count > 10000)
                {
                    trace.FailSteps = steps.Select(p => p).Reverse().ToList();
                    steps.Clear();
                    break;
                }
                if (trace.IsTimeOut())
                {
                    trace.FailSteps = steps.Select(p => p).Reverse().ToList();
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
                    ResetTopStep();
                    continue;
                }

                if (currstep.chooseDirection == StepDirection.same)
                {
                    break;
                }

                var nextstep = new Step()
                {
                    Pos = currstep.Pos
                };

                if (Math.Abs(nextstep.Pos.X - secDestPoint.X) < 30 || Math.Abs(nextstep.Pos.Y - secDestPoint.Y) < 30)
                {
                    nextstep.StepLens = new List<int> { defalutStep };
                }
                else
                {
                    nextstep.StepLens = defaultStepLens.Select(p => p).ToList();
                }

                nextstep.SetFirst(currstep.chooseDirection);


                int stepLen = nextstep.StepLens.First();
                
                switch (currstep.chooseDirection)
                {
                    case StepDirection.left:
                        {
                            nextstep.Directions.Remove(StepDirection.right);
                            var offsetx = nextstep.Pos.X - secDestPoint.X;
                            if (offsetx > 0 && offsetx < stepLen)
                            {
                                nextstep.StepLens.RemoveAll(p => p > offsetx);
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
                                nextstep.StepLens.RemoveAll(p => p > offsetx);
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
                                nextstep.StepLens.RemoveAll(p => p > offsety);
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
                                nextstep.StepLens.RemoveAll(p => p > offsety);
                                stepLen = offsety;
                            }
                            nextstep.Offset(0, stepLen);
                            break;
                        }
                }

                if (!Check(currstep.Pos, nextstep.Pos, true)
                    || (Math.Abs(nextstep.Pos.X - secStartPoint .X) <= 0 && Math.Abs(nextstep.Pos.Y - secStartPoint.Y) <= paddingmax)
                    || (Math.Abs(nextstep.Pos.X - secStartPoint.X) <= paddingmax && Math.Abs(nextstep.Pos.Y - secStartPoint.Y) <= 0)
                    || (Math.Abs(nextstep.Pos.X - secDestPoint.X) <= 0 && Math.Abs(nextstep.Pos.Y - secDestPoint.Y) <= paddingmax)
                    || (Math.Abs(nextstep.Pos.X - secDestPoint.X) <= paddingmax && Math.Abs(nextstep.Pos.Y - secDestPoint.Y) <= 0))
                {
                    if (!stephash.Contains(nextstep.Pos))
                    {
                        steps.Push(nextstep);
                        stephash.Add(nextstep.Pos);
                    }
                    else
                    {
                        while (true)
                        {
                            if (steps.Count == 0)
                            {
                                break;
                            }
                            if (steps.Peek().Pos == nextstep.Pos)
                            {
                                ResetTopStep();
                                break;
                            }
                            else
                            {
                                stephash.Remove(steps.Pop().Pos);
                            }
                        }
                    }
                }
                else
                {
                    ResetTopStep();
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

            if (secStartPoint.X != startPoint.X)
            {
                result.Insert(0, startPoint);
            }

            if (secDestPoint.X != destPoint.X && result.Count > 0)
            {
                result.Add(destPoint);
            }

            result = CutResult(result);

            trace.Result(result.Count);

            LogHelper.Instance.Debug(trace.ToString());

            return result;
        }

        private static StepDirection GetOpDirection(StepDirection direction)
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

        private void AdjustFirstDirection(Step stepfirst,Step steplast)
        {
            if (stepfirst.Pos.X == steplast.Pos.X)
            {
                if (stepfirst.Pos.Y < steplast.Pos.Y)
                {
                    stepfirst.chooseDirection = StepDirection.down;
                }
                else
                {
                    stepfirst.chooseDirection = StepDirection.up;
                }
            }
            else
            {
                if (stepfirst.Pos.X < steplast.Pos.X)
                {
                    stepfirst.chooseDirection = StepDirection.right;
                }
                else
                {
                    stepfirst.chooseDirection = StepDirection.left;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steparray"></param>
        /// <param name="fine">高质量</param>
        /// <returns></returns>
        private Step[] Merge(Step[] steparray,bool fine)
        {
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
                                    AdjustFirstDirection(steparray[i - 1], joinstep);
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
                                    if (fine)
                                    {
                                        i = 2;
                                    }

                                }
                            }
                        }
                    }
                }

                prestep = steparray[i];
            }

            return steparray;
        }

        /// <summary>
        /// 处理回路
        /// </summary>
        /// <param name="steparray"></param>
        /// <param name="fine">是否精细处理</param>
        /// <returns></returns>
        public Step[] CutCircuit(Step[] steparray, bool fine)
        {
            //反向简化处理
            int istart = 0;
            var preDirection = steparray.First().chooseDirection;
            for (var i = istart + 1; i < steparray.Length; i++)
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
                                        AdjustFirstDirection(steparray[i - 1], joinstep);

                                        var list = new List<Step>();
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
                                        i = istart + 1;
                                    }
                                }
                            }
                        }
                    }
                }

                preDirection = steparray[i].chooseDirection;
            }
            return steparray;
        }

        private List<Step> CutSteps(List<Step> steps)
        {
            if (steps.Count > 1)
            {
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
                steparray = Merge(steparray, false);
                steparray = Merge(steparray, true);

                //反向简化处理
                steparray = CutCircuit(steparray, false);

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
                        && ps[i].Y >= ps[i - 1].Y == ps[i].Y >= ps[i + 1].Y)
                        || (ps[i].Y == ps[i - 1].Y && ps[i].Y == ps[i + 1].Y
                       && ps[i].X >= ps[i - 1].X == ps[i].X >= ps[i + 1].X)
                       || (ps[i].X == ps[i + 1].X && ps[i].Y == ps[i + 1].Y))
                    {
                        var li = ps.ToList();
                        li.RemoveAt(i);
                        ps = li.ToArray();
                        i = 1;
                    }
                }

                //起点处理
                if (ps.Length > 2)
                {
                    if (firstDirection == StepDirection.right)
                    {
                        if (ps[1].X < ps[0].X && ps[2].Y != ps[1].Y)
                        {
                            var temp = ps[0];
                            ps[0] = ps[1];
                            for(var i = 1; i < ps.Length; i++)
                            {
                                if (ps[i].X == ps[0].X)
                                {
                                    ps[i].X = temp.X;
                                }
                            }
                        }
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
