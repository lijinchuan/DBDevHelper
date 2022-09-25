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
            _sqlReader = new SqlReader(sql);
            sqlAnalysersStacks = new Stack<ISqlAnalyser>();

            SqlAnalyserMapper.Add("select", () => new SelectAnalyser());
        }

        public ISqlAnalyser GetSqlAnalyser(string token)
        {
            if(SqlAnalyserMapper.TryGetValue(token.ToLower(),out Func<ISqlAnalyser> a))
            {
                return a();
            }

            return null;
        }

        public void Handle()
        {
            var next = _sqlReader.ReadNext();
            
            var currentDeep = 0;
            ISqlAnalyser currentAnalyser = null;
            List<ISqlAnalyser> sqlAnalysers = new List<ISqlAnalyser>();
            while (next != null)
            {
                var analyser = GetSqlAnalyser(next.Val);
                if (analyser != null)
                {
                    analyser.Deep = next.Deep;
                    if (currentAnalyser != null)
                    {
                        if (currentDeep > next.Deep)
                        {
                            sqlAnalysersStacks.Push(currentAnalyser);
                            currentAnalyser = analyser;
                            currentDeep = next.Deep;
                        }
                        else if (currentDeep == next.Deep)
                        {
                            if (sqlAnalysersStacks.Count == 0)
                            {
                                //把前一个语句的解析结果返回

                                currentAnalyser = analyser;
                            }
                            else
                            {
                                sqlAnalysers.Add(currentAnalyser);
                                currentAnalyser = analyser;
                            }
                        }
                        else if (currentDeep < next.Deep)
                        {
                            if (sqlAnalysersStacks.Count == 0)
                            {
                                throw new Exception("解析出错，找不上父级");
                            }

                            var faterAnalyser = sqlAnalysersStacks.Pop();
                            if (faterAnalyser.Deep != next.Deep)
                            {
                                throw new Exception("解析出错，深度不正确");
                            }

                            sqlAnalysers.Add(currentAnalyser);
                            faterAnalyser.NestAnalyser = sqlAnalysers;
                            sqlAnalysers = new List<ISqlAnalyser>();

                            if (sqlAnalysersStacks.Count > 0)
                            {
                                sqlAnalysers.Add(faterAnalyser);
                            }
                            else
                            {
                                //把前一个语句的解析结果返回

                            }

                            currentAnalyser = analyser;
                            currentDeep = next.Deep;
                        }
                    }
                    analyser.Accept(next);
                }

                next = _sqlReader.ReadNext();
            }
        }
    }
}
