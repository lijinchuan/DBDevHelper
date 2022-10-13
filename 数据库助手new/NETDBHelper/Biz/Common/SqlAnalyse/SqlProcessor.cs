using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SqlProcessor : ISqlProcessor
    {
        private readonly Dictionary<string, Func<ISqlAnalyser>> SqlAnalyserMapper = new Dictionary<string, Func<ISqlAnalyser>>();

        private readonly string _sql;
        private readonly SqlReader _sqlReader = null;
        private readonly Stack<ISqlAnalyser> sqlAnalysersStacks = null;

        private ISqlExpress[] CurrentSqlExpressArray = null;
        private volatile int CurrentSqlExpressListReadPostion = 0;

        private static HashSet<string> KeysAfterBegin = new HashSet<string> { SqlAnalyser.keyTransaction };

        public SqlProcessor(string sql)
        {
            _sql = sql;
            //分析时全部转为小写
            _sqlReader = new SqlReader(sql.ToLower());
            sqlAnalysersStacks = new Stack<ISqlAnalyser>();

            SqlAnalyserMapper.Add(SqlAnalyser.keySelect, () => new SelectAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyUpdate, () => new UpdateAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyTruncate, () => new TruncateAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyInsert, () => new InsertAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyDelete, () => new DeleteAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyCreate, () => new CreateAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyAlter, () => new AlterAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyDrop, () => new DropAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyExec, () => new ExecAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyExecute, () => new ExecuteAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyser.keyIf, () => new IfAnalyser());
            //SqlAnalyserMapper.Add(SqlAnalyser.keyBegin, () => new BeginAnalyser());
        }

        public ISqlAnalyser GetSqlAnalyser(string token)
        {
            if (SqlAnalyserMapper.TryGetValue(token, out Func<ISqlAnalyser> a))
            {
                return a();
            }

            return null;
        }

        public List<ISqlAnalyser> Handle()
        {
            Stack<ISqlExpress> bracketStack = new Stack<ISqlExpress>();
            Stack<ISqlExpress> beginEndStack = new Stack<ISqlExpress>();
            int currentDeep = 0;
            int deep = 0;
            ISqlAnalyser currentAnalyser = null;

            var sqlExpresses = new List<ISqlExpress>();
            var token = _sqlReader.ReadNext();
            while (token != null)
            {
                sqlExpresses.Add(token);
                token = _sqlReader.ReadNext();
            }

            CurrentSqlExpressArray= sqlExpresses.ToArray();
            CurrentSqlExpressListReadPostion = 0;

            for (; CurrentSqlExpressListReadPostion < CurrentSqlExpressArray.Length; CurrentSqlExpressListReadPostion++)
            {
                var currentSqlExpress = CurrentSqlExpressArray[CurrentSqlExpressListReadPostion];

                if (currentSqlExpress.ExpressType == SqlExpressType.BracketEnd)
                {
                    if (bracketStack.Any())
                    {
                        bracketStack.Pop();
                        deep--;
                    }
                }
                else if (currentSqlExpress.ExpressType == SqlExpressType.End)
                {
                    if (beginEndStack.Any())
                    {
                        beginEndStack.Pop();
                        deep--;
                    }
                }

                currentSqlExpress.Deep = deep;

                var iskey = SqlAnalyserMapper.ContainsKey(currentSqlExpress.Val);
                if (iskey)
                {
                    currentSqlExpress.AnalyseType = AnalyseType.Key;
                }
                if (currentAnalyser == null || !currentAnalyser.Accept(this, currentSqlExpress, iskey))
                {
                    var analyser = GetSqlAnalyser(currentSqlExpress.Val);
                    if (analyser != null)
                    {
                        analyser.Deep = currentSqlExpress.Deep;
                        sqlAnalysersStacks.Push(analyser);

                        analyser.Accept(this, currentSqlExpress, iskey);
                        currentAnalyser = analyser;
                        currentDeep = currentSqlExpress.Deep;
                    }
                    else
                    {
                        currentAnalyser = null;
                        if (currentSqlExpress.Deep < currentDeep)
                        {
                            //变浅了
                            if (sqlAnalysersStacks.Count > 0)
                            {
                                var temp = new Queue<ISqlAnalyser>();
                                ISqlAnalyser faterAnalyser = null;
                                while (sqlAnalysersStacks.Any())
                                {
                                    faterAnalyser = sqlAnalysersStacks.Pop();
                                    temp.Enqueue(faterAnalyser);
                                    if (faterAnalyser.Deep <= currentSqlExpress.Deep)
                                    {
                                        currentAnalyser = faterAnalyser;
                                        currentAnalyser.Accept(this, currentSqlExpress, false);
                                        break;
                                    }
                                }

                                while (temp.Any())
                                {
                                    sqlAnalysersStacks.Push(temp.Dequeue());
                                }

                            }

                        }

                    }
                }
                if (currentDeep != currentSqlExpress.Deep)
                {
                    currentDeep = currentSqlExpress.Deep;
                    if (currentAnalyser == null)
                    {
                        currentAnalyser = new DefaultAnalyser();
                        currentAnalyser.Deep = currentSqlExpress.Deep;
                        sqlAnalysersStacks.Push(currentAnalyser);

                        currentAnalyser.Accept(this, currentSqlExpress, iskey);
                    }
                }

                //要放在后面处理
                if (currentSqlExpress.ExpressType == SqlExpressType.Bracket)
                {
                    bracketStack.Push(currentSqlExpress);
                    deep++;
                }
                else if (currentSqlExpress.ExpressType == SqlExpressType.Begin)
                {
                    if (!KeysAfterBegin.Contains(GetNext()?.Val ?? string.Empty))
                    {
                        beginEndStack.Push(currentSqlExpress);
                        deep++;
                    }
                }
            }

            //这里也要考虑层级，先只解析简单的情况
            var ret = PopAnalyser();

            return ret;

            List<ISqlAnalyser> PopAnalyser()
            {
                var list = sqlAnalysersStacks.OrderBy(p => p.GetStartPos()).ThenByDescending(p => p.GetEndPos()).ToList();
                ISqlAnalyser parent = null;
                Stack<ISqlAnalyser> parentsAnalyser = new Stack<ISqlAnalyser>();
                foreach (var item in list)
                {
                    if (parent == null)
                    {
                        parent = item;
                    }
                    else
                    {
                        if (parent.GetStartPos() <= item.GetStartPos() && parent.GetEndPos() >= item.GetEndPos())
                        {
                            parent.NestAnalyser.Add(item);
                            parentsAnalyser.Push(parent);
                            item.ParentAnalyser = parent;
                            parent = item;
                        }
                        else
                        {
                            var find = false;
                            while (parentsAnalyser.Any())
                            {
                                var pt = parentsAnalyser.Pop();
                                if (pt.GetStartPos() <= item.GetStartPos() && pt.GetEndPos() >= item.GetEndPos())
                                {
                                    find = true;
                                    pt.NestAnalyser.Add(item);
                                    item.ParentAnalyser = pt;
                                    parent = pt;
                                    break;
                                }
                            }

                            if (!find)
                            {
                                parent = null;
                            }
                        }
                    }
                }

                return list.Where(p => p.ParentAnalyser == null).ToList();
            }

        }

        public List<string> FindTables(List<ISqlAnalyser> sqlAnalysers, int pos)
        {
            foreach (var analyser in sqlAnalysers)
            {
                var express = analyser.FindByPos(pos);
                if (express != null)
                {
                    return analyser.FindTables(express);
                }
            }

            return new List<string>();
        }

        public ISqlExpress GetNext(int offset = 0)
        {
            if (offset < 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (CurrentSqlExpressArray == null || CurrentSqlExpressArray.Length <= CurrentSqlExpressListReadPostion + 1 + offset)
            {
                return null;
            }
            return CurrentSqlExpressArray[CurrentSqlExpressListReadPostion + 1 + offset];
        }
    }
}
