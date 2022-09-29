using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SqlProcessor : ISqlProcessor
    {
        private Dictionary<string, Func<ISqlAnalyser>> SqlAnalyserMapper = new Dictionary<string, Func<ISqlAnalyser>>();

        private string _sql = string.Empty;
        private SqlReader _sqlReader = null;
        private Stack<ISqlAnalyser> sqlAnalysersStacks = null;

        public SqlProcessor(string sql)
        {
            _sql = sql;
            //分析时全部转为小写
            _sqlReader = new SqlReader(sql.ToLower());
            sqlAnalysersStacks = new Stack<ISqlAnalyser>();

            SqlAnalyserMapper.Add("select", () => new SelectAnalyser());
            SqlAnalyserMapper.Add("exec", () => null);
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
            List<ISqlAnalyser> sqlAnalysers = new List<ISqlAnalyser>();
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
                        if (currentAnalyser != null)
                        {
                            if (next.Deep > currentDeep)
                            {
                                sqlAnalysersStacks.Push(currentAnalyser);
                                currentDeep = next.Deep;
                            }
                            else if (currentDeep == next.Deep)
                            {
                                if (sqlAnalysersStacks.Count == 0)
                                {
                                    //把前一个语句的解析结果返回
                                    ret.Add(currentAnalyser);
                                }
                                else
                                {
                                    sqlAnalysers.Add(currentAnalyser);
                                }
                            }
                            else if (currentDeep > next.Deep)
                            {
                                //变浅了
                                if (sqlAnalysersStacks.Count > 0)
                                {
                                    var faterAnalyser = sqlAnalysersStacks.Pop();
                                    if (faterAnalyser.Deep != next.Deep)
                                    {
                                        sqlAnalysersStacks.Push(faterAnalyser);
                                    }
                                    else
                                    {
                                        sqlAnalysers.Add(currentAnalyser);
                                        faterAnalyser.NestAnalyser.AddRange(sqlAnalysers);
                                        sqlAnalysers = new List<ISqlAnalyser>();

                                        if (sqlAnalysersStacks.Count > 0)
                                        {
                                            sqlAnalysers.Add(faterAnalyser);
                                        }
                                        else
                                        {
                                            //把前一个语句的解析结果返回
                                            ret.Add(faterAnalyser);
                                        }
                                    }
                                }
                                currentDeep = next.Deep;

                            }
                        }
                        analyser.Accept(next, iskey);
                        currentAnalyser = analyser;
                    }
                    else
                    {
                        //前一个结果返回
                        if (next.Deep > currentDeep)
                        {
                            currentDeep = next.Deep;
                            if (currentAnalyser != null)
                            {
                                sqlAnalysersStacks.Push(currentAnalyser);
                            }
                            currentAnalyser = null;
                        }
                        else if (currentDeep == next.Deep)
                        {
                            if (currentAnalyser != null)
                            {
                                if (sqlAnalysersStacks.Count == 0)
                                {
                                    //把前一个语句的解析结果返回
                                    ret.Add(currentAnalyser);
                                }
                                else
                                {
                                    sqlAnalysers.Add(currentAnalyser);
                                }
                            }
                            currentAnalyser = null;
                        }
                        else if (next.Deep < currentDeep)
                        {
                            //变浅了
                            if (sqlAnalysersStacks.Count > 0)
                            {
                                var faterAnalyser = sqlAnalysersStacks.Pop();
                                if (faterAnalyser.Deep != next.Deep)
                                {
                                    sqlAnalysersStacks.Push(faterAnalyser);
                                }
                                else
                                {
                                    if (currentAnalyser != null)
                                    {
                                        sqlAnalysers.Add(currentAnalyser);
                                    }
                                    faterAnalyser.NestAnalyser.AddRange(sqlAnalysers);
                                    sqlAnalysers = new List<ISqlAnalyser>();

                                    currentAnalyser = faterAnalyser;
                                    currentAnalyser.Accept(next, false);
                                }
                            }
                            currentDeep = next.Deep;
                        }
                        
                    }
                }

                next = _sqlReader.ReadNext();
            }

            //这里也要考虑层级，先只解析简单的情况
            PopAnalyser();

            return ret;

            void PopAnalyser()
            {
                if (sqlAnalysersStacks.Count > 0)
                {
                    if (currentAnalyser != null)
                    {
                        sqlAnalysers.Add(currentAnalyser);
                    }
                    while (sqlAnalysersStacks.Count > 0)
                    {
                        currentAnalyser = sqlAnalysersStacks.Pop();
                        currentAnalyser.NestAnalyser = sqlAnalysers;
                        sqlAnalysers = new List<ISqlAnalyser>();
                        if (sqlAnalysersStacks.Count > 0)
                        {
                            sqlAnalysers.Add(currentAnalyser);
                        }
                    }
                    ret.Add(currentAnalyser);
                }
                else if (currentAnalyser != null)
                {
                    ret.Add(currentAnalyser);
                }
            }
        }
    }
}
