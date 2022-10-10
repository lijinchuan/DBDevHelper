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
        }

        public ISqlAnalyser GetSqlAnalyser(string token)
        {
            if(SqlAnalyserMapper.TryGetValue(token,out Func<ISqlAnalyser> a))
            {
                return a();
            }

            return null;
        }

        public List<ISqlAnalyser> Handle()
        {
            List<ISqlAnalyser> ret = new List<ISqlAnalyser>();
            var next = _sqlReader.ReadNext();

            var currentDeep = 0;
            ISqlAnalyser currentAnalyser = null;
            //在有上级分析器的情况下，保存多个同级的分析器
            while (next != null)
            {
                var iskey = SqlAnalyserMapper.ContainsKey(next.Val);
                if (iskey)
                {
                    next.AnalyseType = AnalyseType.Key;
                }
                if (currentAnalyser == null || !currentAnalyser.Accept(next, iskey))
                {
                    var analyser = GetSqlAnalyser(next.Val);
                    if (analyser != null)
                    {
                        analyser.Deep = next.Deep;
                        sqlAnalysersStacks.Push(analyser);

                        analyser.Accept(next, iskey);
                        currentAnalyser = analyser;
                        currentDeep = next.Deep;
                    }
                    else
                    {
                        currentAnalyser = null;
                        if (next.Deep < currentDeep)
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
                                    if (faterAnalyser.Deep <= next.Deep)
                                    {
                                        currentAnalyser = faterAnalyser;
                                        currentAnalyser.Accept(next, false);
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
                currentDeep = next.Deep;
                next = _sqlReader.ReadNext();
            }

            //这里也要考虑层级，先只解析简单的情况
            ret = PopAnalyser();

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


        public List<string> FindTables(List<ISqlAnalyser> sqlAnalysers,int pos)
        {
            foreach(var analyser in sqlAnalysers)
            {
                var express = analyser.FindByPos(pos);
                if (express != null)
                {
                    return analyser.FindTables(express);
                }
            }

            return new List<string>();
        }
    }
}
