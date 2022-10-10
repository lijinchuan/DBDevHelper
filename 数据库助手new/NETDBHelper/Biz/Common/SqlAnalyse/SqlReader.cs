using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common.SqlAnalyse
{
    public class SqlReader:ISqlReader
    {
        private StringReader reader = null;

        private int CurrentLine = 0;

        private int CurrentIndex = 0;

        private int CurrentLineIndex = 0;

        private int CurrentDeep = 0;

        private readonly string Sql;

        private int lastch = -1;
        private int leftch = -1;

        readonly Stack<SqlExpress> stringStack = new Stack<SqlExpress>();
        readonly Stack<SqlExpress> bracketStack = new Stack<SqlExpress>();
        readonly Stack<SqlExpress> beginEndStack = new Stack<SqlExpress>();
        readonly Stack<SqlExpress> annotationStack = new Stack<SqlExpress>();

        private static bool IsNumber(int ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private static bool IsLetter(int ch)
        {
            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || (ch >= '\u4e00' && ch <= '\u9fa5') || ch == '_' || ch == '@' || ch == '[' || ch == ']' || ch == '.' || ch == '#' || ch == '$';
        }

        SqlExpress CrateStart(SqlExpressType analyseType)
        {
            SqlExpress analyseInfo = new SqlExpress();
            analyseInfo.StartLine = CurrentLine;
            analyseInfo.StartIndex = CurrentIndex;
            analyseInfo.StartInineIndex = CurrentLineIndex;
            analyseInfo.ExpressType = analyseType;
            analyseInfo.Deep = CurrentDeep;
            return analyseInfo;
        }

        SqlExpress FillEnd(SqlExpress analyseInfo, bool isIncludeCurrentChar)
        {
            analyseInfo.EndLine = CurrentLine;
            analyseInfo.EndIndex = CurrentIndex;
            if (!isIncludeCurrentChar)
            {
                analyseInfo.EndIndex--;
            }
            analyseInfo.EndInineIndex = CurrentLineIndex;
            return analyseInfo;
        }

        public SqlReader(string sql)
        {
            Sql = sql;
        }

        public ISqlExpress ReadNext()
        {
            if (reader == null)
            {
                reader = new StringReader(Sql);
            }
            SqlExpress tokenInfo = null;
            //指示是否是数字
            var isNumberic = true;
            var isVar = false;
            while (true)
            {
                var val = leftch == -1 ? reader.Read() : leftch;
                if (val == -1)
                {
                    break;
                }
                leftch = -1;

                var ch = val;
                var isletter = IsLetter(ch);
                var isnumber = IsNumber(ch);

                if ((isletter || isnumber) && stringStack.Count == 0 && annotationStack.Count == 0)
                {
                    if (tokenInfo == null)
                    {
                        if (ch == '@')
                        {
                            isVar = true;
                        }
                        tokenInfo = CrateStart(isVar ? SqlExpressType.Var : SqlExpressType.Token);
                    }
                    tokenInfo.Val += (char)ch;
                    if (isletter && ch != '.')
                    {
                        isNumberic = false;
                    }
                }
                else
                {
                    if (!isletter && !isnumber && tokenInfo != null)
                    {
                        FillEnd(tokenInfo, false);
                        leftch = ch;
                        if (isNumberic)
                        {
                            tokenInfo.ExpressType = SqlExpressType.Numric;
                        }
                        tokenInfo.NextChar = ch;

                        if (tokenInfo.Val == "begin")
                        {
                            tokenInfo.ExpressType = SqlExpressType.Begin;
                            beginEndStack.Push(tokenInfo);

                            CurrentDeep++;

                            return tokenInfo;
                        }
                        else if (tokenInfo.Val == "end")
                        {
                            tokenInfo.ExpressType = SqlExpressType.End;
                            if (beginEndStack.Count > 0)
                            {
                                beginEndStack.Pop();
                                CurrentDeep--;
                                tokenInfo.Deep = CurrentDeep;
                                return tokenInfo;
                            }
                        }
                        else
                        {
                            return tokenInfo;
                        }
                    }

                    if (ch == '\'' && annotationStack.Count == 0)
                    {
                        if (stringStack.Count == 0)
                        {
                            stringStack.Push(CrateStart(SqlExpressType.String));
                        }
                        else if (stringStack.Count > 0 && ((lastch != '\'' && stringStack.Peek().StartIndex != CurrentIndex - 1) ||
                            (lastch == '\'' && stringStack.Peek().StartIndex == CurrentIndex - 1)))
                        {
                            var analyseInfo = stringStack.Pop();
                            var ret = FillEnd(analyseInfo, true);
                            CurrentIndex++;
                            lastch = ch;
                            return ret;
                        }
                    }
                    else if (ch == '*' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        if (lastch == '/')
                        {
                            annotationStack.Push(CrateStart(SqlExpressType.Annotation));
                        }
                        else
                        {
                            var sqlExpress = CrateStart(SqlExpressType.Star);
                            FillEnd(sqlExpress, true);
                            CurrentIndex++;
                            lastch = ch;
                            return sqlExpress;
                        }
                    }
                    else if (ch == '-' && lastch == '-' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        var analyseInfo = CrateStart(SqlExpressType.Annotation);
                        CurrentIndex++;
                        var nextCh = reader.Read();
                        while (nextCh != -1)
                        {
                            CurrentIndex++;
                            if (nextCh == '\n')
                            {
                                break;
                            }
                            nextCh = reader.Read();
                        }
                        FillEnd(analyseInfo, false);
                        lastch = nextCh;
                        CurrentLine++;
                        CurrentLineIndex = 0;
                        return analyseInfo;
                    }
                    else if (ch == '/' && lastch == '*' && stringStack.Count == 0 && annotationStack.Count > 0)
                    {
                        var analyseInfo = annotationStack.Pop();

                        var ret = FillEnd(analyseInfo, true);
                        lastch = ch;
                        CurrentIndex++;
                        return ret;
                    }
                    else if (ch == '(' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        bracketStack.Push(CrateStart(SqlExpressType.Bracket));

                        var sqlExpress = CrateStart(SqlExpressType.Bracket);
                        FillEnd(sqlExpress, true);

                        //进入子分析
                        lastch = ch;
                        CurrentIndex++;
                        CurrentDeep++;

                        return sqlExpress;
                    }
                    else if (ch == ')' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        if (bracketStack.Count > 0)
                        {
                            bracketStack.Pop();
                            CurrentDeep--;

                            var sqlExpress = CrateStart(SqlExpressType.BracketEnd);
                            FillEnd(sqlExpress, true);

                            lastch = ch;
                            CurrentIndex++;
                            return sqlExpress;
                        }
                    }
                    else if (ch == ',' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        var sqlExpress = CrateStart(SqlExpressType.Comma);
                        FillEnd(sqlExpress, true);
                        CurrentIndex++;
                        lastch = ch;
                        return sqlExpress;
                    }
                }

                if (ch == '\n')
                {
                    CurrentLine++;
                    CurrentLineIndex = 0;
                }
                else
                {
                    CurrentLineIndex++;
                }
                lastch = ch;
                CurrentIndex++;
            }

            if (tokenInfo != null)
            {
                return FillEnd(tokenInfo, false);
            }
            else if (stringStack.Count > 0)
            {
                return FillEnd(stringStack.Pop(), false);
            }
            else if (annotationStack.Count > 0)
            {
                return FillEnd(annotationStack.Pop(), false) ;
            }
            else if (bracketStack.Count > 0)
            {
                if (bracketStack.Count > 0)
                {
                    bracketStack.Pop();
                    CurrentDeep--;
                }
            }

            return null;
        }
    }
}
