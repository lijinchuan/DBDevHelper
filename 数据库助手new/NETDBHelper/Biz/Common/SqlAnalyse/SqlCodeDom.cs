using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SqlCodeDom : ISqlProcessor
    {
        private readonly static Dictionary<string, Func<ISqlAnalyser>> SqlAnalyserMapper = new Dictionary<string, Func<ISqlAnalyser>>();

        private string _sql;
        //private readonly SqlReader _sqlReader = null;
        //private readonly Stack<ISqlAnalyser> sqlAnalysersStacks = null;

        private List<ISqlAnalyser> sqlAnalysers = null;

        private ISqlExpress[] CurrentSqlExpressArray = null;
        private volatile int CurrentSqlExpressListReadPostion = 0;

        private static HashSet<string> KeysAfterBegin = new HashSet<string> { SqlAnalyse.SqlAnalyser.keyTransaction };

        static SqlCodeDom()
        {
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keySelect, () => new SelectAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyUpdate, () => new UpdateAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyTruncate, () => new TruncateAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyInsert, () => new InsertAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyDelete, () => new DeleteAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyCreate, () => new CreateAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyAlter, () => new AlterAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyDrop, () => new DropAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyExec, () => new ExecAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyExecute, () => new ExecuteAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyIf, () => new IfAnalyser());
            SqlAnalyserMapper.Add(SqlAnalyse.SqlAnalyser.keyCase, () => new CaseAnalyser());
            //SqlAnalyserMapper.Add(SqlAnalyser.keyBegin, () => new BeginAnalyser());
        }

        public SqlCodeDom(string sql)
        {
            _sql = sql;
        }

        public ISqlAnalyser GetSqlAnalyser(string token)
        {
            if (SqlAnalyserMapper.TryGetValue(token, out Func<ISqlAnalyser> a))
            {
                return a();
            }

            return null;
        }

        public List<ISqlAnalyser> Analyse()
        {
            if (sqlAnalysers != null)
            {
                return sqlAnalysers;
            }

            if (string.IsNullOrWhiteSpace(_sql))
            {
                sqlAnalysers = new List<ISqlAnalyser>();
                return sqlAnalysers;
            }

            //分析时全部转为小写
            var sqlReader = new SqlReader(_sql.ToLower());
            var sqlAnalysersStacks = new Stack<ISqlAnalyser>();

            Stack<ISqlExpress> bracketStack = new Stack<ISqlExpress>();
            Stack<ISqlExpress> beginEndStack = new Stack<ISqlExpress>();
            Stack<ISqlExpress> acceptDeepStack = new Stack<ISqlExpress>();
            int currentDeep = 0;
            int deep = 0;
            ISqlAnalyser currentAnalyser = null;

            var sqlExpresses = new List<ISqlExpress>();
            var token = sqlReader.ReadNext();
            while (token != null)
            {
                sqlExpresses.Add(token);
                token = sqlReader.ReadNext();
            }

            CurrentSqlExpressArray= sqlExpresses.ToArray();
            CurrentSqlExpressListReadPostion = 0;

            for (; CurrentSqlExpressListReadPostion < CurrentSqlExpressArray.Length; CurrentSqlExpressListReadPostion++)
            {
                var currentSqlExpress = CurrentSqlExpressArray[CurrentSqlExpressListReadPostion];

                if (currentSqlExpress.Val == SqlAnalyse.SqlAnalyser.keyCase)
                {
                    beginEndStack.Push(currentSqlExpress);
                    deep++;
                }

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
                    if (beginEndStack.Any() && beginEndStack.Peek().Val == SqlAnalyse.SqlAnalyser.keyBegin)
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
                var analyseAccept = currentAnalyser?.Accept(this, currentSqlExpress, iskey);
                if (currentAnalyser == null || analyseAccept == AnalyseAccept.Reject)
                {
                    if (currentAnalyser != null && currentAnalyser.Deep == currentSqlExpress.Deep)
                    {
                        var tempAnalyser = currentAnalyser;
                        while (acceptDeepStack.Any())
                        {
                            if (acceptDeepStack.Peek() == tempAnalyser.GetAcceptSqlExpressList().FirstOrDefault())
                            {
                                acceptDeepStack.Pop();
                                deep--;
                                currentSqlExpress.Deep--;
                                tempAnalyser.ParentAnalyser.AddAcceptSqlExpress(tempAnalyser.GetAcceptSqlExpressList().LastOrDefault());
                                tempAnalyser = tempAnalyser.ParentAnalyser;
                                tempAnalyser.ParentAnalyser = null;

                            }
                            else
                            {
                                break;
                            }
                        }
                    }

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
                else if (analyseAccept == AnalyseAccept.AcceptDeeper)
                {
                    deep++;
                    currentSqlExpress.Deep = deep;
                    var analyser = GetSqlAnalyser(currentSqlExpress.Val);
                    analyser.ParentAnalyser = currentAnalyser;
                    analyser.Deep = currentSqlExpress.Deep;
                    sqlAnalysersStacks.Push(analyser);
                    acceptDeepStack.Push(currentSqlExpress);

                    analyser.Accept(this, currentSqlExpress, iskey);
                    currentAnalyser = analyser;
                    currentDeep = currentSqlExpress.Deep;
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

                if (currentSqlExpress.ExpressType == SqlExpressType.End)
                {
                    if (beginEndStack.Any() && beginEndStack.Peek().Val == SqlAnalyse.SqlAnalyser.keyCase)
                    {
                        beginEndStack.Pop();
                        deep--;
                    }
                }
            }

            var curr = currentAnalyser;
            while (acceptDeepStack.Any())
            {
                var sqlExpress = acceptDeepStack.Pop();
                if (sqlExpress == curr.GetAcceptSqlExpressList().FirstOrDefault())
                {
                    curr.ParentAnalyser.AddAcceptSqlExpress(curr.GetAcceptSqlExpressList().LastOrDefault());
                    curr = curr.ParentAnalyser;
                }
            }

            //这里也要考虑层级，先只解析简单的情况
            sqlAnalysers = PopAnalyser();

            return sqlAnalysers;

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
                                    parent = item;
                                    parentsAnalyser.Push(pt);
                                    break;
                                }
                            }

                            if (!find)
                            {
                                parent = item;
                            }
                        }
                    }
                }

                return list.Where(p => p.ParentAnalyser == null).ToList();
            }

        }

        public List<string> FindTables(int pos)
        {
            foreach (var analyser in Analyse())
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

        public ISqlExpress FindExpress(int pos)
        {
            foreach (var analyser in Analyse())
            {
                var express = analyser.FindByPos(pos);
                if (express != null)
                {
                    return express;
                }
            }

            return null;
        }

        public void SetSql(string sql)
        {
            if (sql != _sql)
            {
                _sql = sql;
                sqlAnalysers = null;
            }
        }
    }
}
