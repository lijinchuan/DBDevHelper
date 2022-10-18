using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public interface ISqlAnalyser
    {
        /// <summary>
        /// 解析深度
        /// </summary>
        int Deep { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        string GetPrimaryKey();

        HashSet<string> GetKeys();

        AnalyseAccept Accept(ISqlProcessor sqlProcessor,ISqlExpress sqlExpress,bool isKey);

        List<ISqlAnalyser> NestAnalyser { get; set; }

        void Print(string sql);

        HashSet<ISqlExpress> GetTables();
        /// <summary>
        /// 别名表
        /// </summary>
        /// <returns></returns>
        HashSet<ISqlExpress> GetAliasTables();

        HashSet<ISqlExpress> GetColumns();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlExpress"></param>
        /// <returns></returns>
        List<string> FindTables(ISqlExpress sqlExpress);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        ISqlExpress FindByPos(int pos);
        /// <summary>
        /// 添加Key
        /// </summary>
        /// <param name="key"></param>
        void AddAcceptKey(string key);
        /// <summary>
        /// 添加表达式
        /// </summary>
        /// <param name="sqlExpress"></param>

        void AddAcceptSqlExpress(ISqlExpress sqlExpress);
        /// <summary>
        /// 解析开始位置
        /// </summary>
        /// <returns></returns>
        int GetStartPos();
        /// <summary>
        /// 解析结束位置
        /// </summary>
        /// <returns></returns>
        int GetEndPos();
        /// <summary>
        /// 父解析器
        /// </summary>
        ISqlAnalyser ParentAnalyser { get; set; }
    }
}
