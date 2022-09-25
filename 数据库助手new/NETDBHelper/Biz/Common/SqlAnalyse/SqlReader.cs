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

        private string Sql;

        private int lastch = -1;

        Stack<SqlExpress> stringStack = new Stack<SqlExpress>();
        Stack<SqlExpress> bracketStack = new Stack<SqlExpress>();
        Stack<SqlExpress> annotationStack = new Stack<SqlExpress>();

        private static bool IsNumber(int ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private static bool IsLetter(int ch)
        {
            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == '_' || ch == '@' || ch == '[' || ch == ']' || ch == '.';
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
            while (true)
            {
                var val = reader.Read();
                if (val == -1)
                {
                    break;
                }
                var ch = val;
                var isletter = IsLetter(ch);
                var isnumber = IsNumber(ch);

                if ((isletter || isnumber) && stringStack.Count == 0 && annotationStack.Count == 0)
                {
                    if (tokenInfo == null)
                    {
                        tokenInfo = CrateStart(SqlExpressType.Token);
                    }
                    tokenInfo.Val += (char)ch;
                }
                else
                {
                    if (!isletter && !isnumber && tokenInfo != null)
                    {
                        FillEnd(tokenInfo, false);
                        lastch = ch;
                        CurrentIndex++;
                        return tokenInfo;
                    }

                    if (ch == '\'' && annotationStack.Count == 0)
                    {
                        if (stringStack.Count == 0)
                        {
                            stringStack.Push(CrateStart(SqlExpressType.String));
                        }
                        else if (lastch != '\'' && stringStack.Count > 0)
                        {
                            var analyseInfo = stringStack.Pop();
                            var ret = FillEnd(analyseInfo, true);
                            CurrentIndex++;
                            lastch = ch;
                            return ret;
                        }
                    }
                    else if (ch == '*' && lastch == '/' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        annotationStack.Push(CrateStart(SqlExpressType.Annotation));
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
                        //进入子分析
                        lastch = ch;
                        CurrentIndex++;
                        CurrentDeep++;
                        var ret = ReadNext();

                        return ret;
                    }
                    else if (ch == ')' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        if (bracketStack.Count > 0)
                        {
                            bracketStack.Pop();
                            CurrentDeep--;
                        }
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
                return FillEnd(tokenInfo, true);
            }
            else if (stringStack.Count > 0)
            {
                return FillEnd(stringStack.Pop(), true);
            }
            else if (annotationStack.Count > 0)
            {
                return FillEnd(annotationStack.Pop(), true) ;
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
