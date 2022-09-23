using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Common
{
    public static class SQLCodeHelper
    {
        public enum AnalyseType
        {
            /// <summary>
            /// 字符串
            /// </summary>
            String,
            Bracket,
            /// <summary>
            /// 注释
            /// </summary>
            Annotation,
            Token
        }

        public class AnalyseInfo
        {
            public AnalyseType Type
            {
                get;
                set;
            }

            public int StartLine
            {
                get;
                set;
            }

            public int StartIndex
            {
                get;
                set;
            }

            public int StartInineIndex
            {
                get;
                set;
            }

            public int EndLine
            {
                get;
                set;
            }

            public int EndIndex
            {
                get;
                set;
            }

            public int EndInineIndex
            {
                get;
                set;
            }

            public string AnalyseError
            {
                get;
                set;
            }
        }

        private static bool IsNumber(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private static bool IsLetter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == '_' || ch == '@';
        }

        public static List<AnalyseInfo> Analyse(char[] codes, int posStart)
        {
            var line = 0;
            var lineIndex = 0;
            AnalyseInfo tokenInfo = null;
            Stack<AnalyseInfo> stringStack = new Stack<AnalyseInfo>();
            Stack<AnalyseInfo> bracketStack = new Stack<AnalyseInfo>();
            Stack<AnalyseInfo> annotationStack = new Stack<AnalyseInfo>();

            List<AnalyseInfo> analyseInfos = new List<AnalyseInfo>();
            var i = posStart;
            for (; i < codes.Length; i++)
            {
                var lastch = lastCh(i);
                var ch = codes[i];
                var isletter = IsLetter(ch);
                var isnumber = IsNumber(ch);

                if ((isletter || isnumber) && stringStack.Count == 0 && annotationStack.Count == 0)
                {
                    if (tokenInfo == null)
                    {
                        tokenInfo = CrateStart(AnalyseType.Token);
                    }
                }
                else
                {
                    if (!isletter && !isnumber && tokenInfo != null)
                    {
                        FillEnd(tokenInfo, false);
                        analyseInfos.Add(tokenInfo);
                        tokenInfo = null;
                    }

                    if (ch == '\'' && annotationStack.Count == 0)
                    {
                        if (stringStack.Count == 0)
                        {
                            stringStack.Push(CrateStart(AnalyseType.String));
                        }
                        else if (lastch != '\'' && stringStack.Count > 0)
                        {
                            var analyseInfo = stringStack.Pop();
                            analyseInfos.Add(FillEnd(analyseInfo, true));
                        }
                    }
                    else if (ch == '*' && lastch == '/' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        annotationStack.Push(CrateStart(AnalyseType.Annotation));
                    }
                    else if (ch == '-' && lastch == '-' && stringStack.Count == 0 && analyseInfos.Count == 0)
                    {
                        var analyseInfo = CrateStart(AnalyseType.Annotation);
                        for (i += 1; i < codes.Length; i++)
                        {
                            if (codes[i] == '\n')
                            {
                                break;
                            }
                        }
                        FillEnd(analyseInfo, false);
                        analyseInfos.Add(analyseInfo);
                    }
                    else if (ch == '/' && lastch == '*' && stringStack.Count == 0 && annotationStack.Count > 0)
                    {
                        var analyseInfo = annotationStack.Pop();
                        analyseInfos.Add(FillEnd(analyseInfo, true));
                    }
                    else if (ch == '(' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        bracketStack.Push(CrateStart(AnalyseType.Bracket));
                        //进入子分析
                        var subAnalyseInfos = Analyse(codes, i + 1);
                        if (subAnalyseInfos.Any())
                        {
                            analyseInfos.AddRange(Analyse(codes, i + 1));
                            i = subAnalyseInfos.Max(p => p.EndIndex);
                        }
                    }
                    else if (ch == ')' && stringStack.Count == 0 && annotationStack.Count == 0)
                    {
                        if (bracketStack.Count > 0)
                        {
                            bracketStack.Pop();
                        }
                    }
                }

                if (ch == '\n')
                {
                    line++;
                    lineIndex = 0;
                }
                else
                {
                    lineIndex++;
                }
            }

            if (tokenInfo != null)
            {
                analyseInfos.Add(FillEnd(tokenInfo, true));
            }
            else if (stringStack.Count > 0)
            {
                analyseInfos.Add(FillEnd(stringStack.Pop(), true));
            }
            else if (annotationStack.Count > 0)
            {
                analyseInfos.Add(FillEnd(annotationStack.Pop(), true));
            }
            else if (bracketStack.Count > 0)
            {
                analyseInfos.AddRange(Analyse(codes, bracketStack.Pop().StartIndex + 1));
            }

            return analyseInfos;

            AnalyseInfo CrateStart(AnalyseType analyseType)
            {
                AnalyseInfo analyseInfo = new AnalyseInfo();
                analyseInfo.StartLine = line;
                analyseInfo.StartIndex = i;
                analyseInfo.StartInineIndex = lineIndex;
                analyseInfo.Type = analyseType;
                return analyseInfo;
            }

            AnalyseInfo FillEnd(AnalyseInfo analyseInfo, bool isIncludeCurrentChar)
            {
                analyseInfo.EndLine = line;
                analyseInfo.EndIndex = i;
                if (!isIncludeCurrentChar)
                {
                    analyseInfo.EndIndex--;
                }
                analyseInfo.EndInineIndex = lineIndex;
                return analyseInfo;
            }

            char lastCh(int idx)
            {
                if (idx == 0)
                {
                    return '\0';
                }
                return codes[idx - 1];
            }
        }
    }
}
