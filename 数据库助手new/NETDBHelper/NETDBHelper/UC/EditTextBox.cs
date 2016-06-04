using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Biz.Common;
using LJC.FrameWork.CodeExpression.KeyWordMatch;

namespace NETDBHelper.UC
{
    public partial class EditTextBox : UserControl
    {
        private Color defaultSelectionColor;
        //internal KeyWordManager keywordman = new KeyWordManager();
        private KeyWordManager _keyWords=new KeyWordManager();
        private WatchTimer _timer = new WatchTimer(3);
        private List<int> _markedLines = new List<int>();
        private int _lastMarketedLines = -1;
        private int _lastInputChar = '\0';

        public KeyWordManager KeyWords
        {
            get
            {
                return _keyWords;
            }
        }
        public EditTextBox()
        {
            InitializeComponent();
            this.RichText.WordWrap = false;
            this.RichText.ScrollBars = RichTextBoxScrollBars.Both;
            this.RichText.ContextMenuStrip = this.contextMenuStrip1;
            this.RichText.VScroll += new EventHandler(RichText_VScroll);
            this.ScaleNos.Font = new Font(RichText.Font.FontFamily, RichText.Font.Size + 1.019f);
            this.RichText.KeyUp += new KeyEventHandler(RichText_KeyUp);
            
            this.RichText.TextChanged+=new EventHandler(RichText_TextChanged);
            defaultSelectionColor = this.RichText.SelectionColor;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _lastMarketedLines = -1;
            MarkKeyWords(true);
        }

        void RichText_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.RichText.Focus())
                return;

            if (e.Control)
                return;
            if (e.Alt)
                return;
            if (e.Shift)
                return;
            if (e.KeyData == (Keys.LButton|Keys.ShiftKey))
            {
                return;
            }

            _lastInputChar = e.KeyValue;
        }

        void RichText_TextChanged(object sender, EventArgs e)
        {
            if (_lastInputChar == '\0')
                return;

            int line = this.RichText.GetLineFromCharIndex(this.RichText.GetFirstCharIndexOfCurrentLine());
            if (_lastInputChar == '\r' || _lastInputChar == '\n')
            {
                _markedLines.RemoveAll(p => p == line || p == line - 1);
                SetLineNo(true);
            }
            else if (_lastInputChar == '\b')
            {
                _markedLines.RemoveAll(p => p == line);
                SetLineNo();
            }
            else
            {
                _markedLines.RemoveAll(p => p == line);
            }
            _lastMarketedLines = -1;
            _lastInputChar='\0';

            this.RichText.LockPaint = true;
            var oldstart = this.RichText.SelectionStart;
            var oldlen = this.RichText.SelectionLength;
            this.RichText.SelectionStart = this.RichText.GetFirstCharIndexOfCurrentLine();
            this.RichText.SelectionLength = this.RichText.Lines[line].Length;
            this.RichText.SelectionColor = Color.Black;
            this.RichText.SelectionStart = oldstart;
            this.RichText.SelectionLength = oldlen;
            this.RichText.LockPaint = false;
            MarkKeyWords(false);
        }

        void SetLineNo(bool addNewLine = false)
        {
            int line1 = CurrentClientScreenStartLine + 1;
            int line2 = CurrentClientScreentEndLine + 1;
            if (addNewLine)
                line2 += 1;
            //if (line1 == this.ScaleNos.FirstLine && line2 == this.ScaleNos.LastLine)
            //    return;
            Dictionary<int, Point> nos = new Dictionary<int, Point>();
            int offset = 0;
            int strLen = this.RichText.GetCharIndexFromPosition(new Point(0, 0)) + 1;
            int linesLen = RichText.Lines.Length;
            for (int i = line1; i <= line2 && i <= linesLen; i++)
            {
                //要算上一个换行符
                var curlen = RichText.Lines[i - 1].Length;
                if (curlen == 0)
                {
                    Point p = new Point(2, 0);
                    p.Y = offset + (offset==0?0:this.Font.Height) + 1;
                    offset = p.Y;
                    nos.Add(i, p);
                    strLen += 1;
                }
                else
                {
                    Point p = this.RichText.GetPositionFromCharIndex(strLen);
                    offset = p.Y;
                    p.X = 2;
                    nos.Add(i, p);
                    strLen += RichText.Lines[i - 1].Length + 1;
                }
            }
            this.ScaleNos.LineNos = nos;
        }

        void RichText_VScroll(object sender, EventArgs e)
        {
            _timer.SetTimeOutCallBack(() =>
                {
                    this.Invoke(new Action<bool>(MarkKeyWords),true);
                });
        }

        public override string Text
        {
            get
            {
                return RichText.Text;
            }
            set
            {
                this.RichText.Text = value;
                this.SetLineNo();
            }
        }

        public void AppendText(string text)
        {
            this.RichText.AppendText(text);
        }

        /// <summary>
        /// 当前屏幕开始行
        /// </summary>
        private int CurrentClientScreenStartLine
        {
            get
            {
                return this.RichText.GetLineFromCharIndex(this.RichText.GetCharIndexFromPosition(new Point(0, 0)));
            }
        }

        private int CurrentClientScreentEndLine
        {
            get
            {
               return this.RichText.GetLineFromCharIndex(this.RichText.GetCharIndexFromPosition(new Point(0, this.RichText.Height)));
            }
        }

        /// <summary>
        /// 分解过程
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public void MarkKeyWords(bool reSetLineNo)
        {
            try
            {
                if (this.RichText.Lines.Length == 0)
                    return;
                int line1 = CurrentClientScreenStartLine;
                if (_lastMarketedLines == line1)
                    return;

                int line2 = CurrentClientScreentEndLine+1;
                if (line2 == 1)
                {
                    return;
                }
                
                int oldStart = this.RichText.SelectionStart;
                int oldSelectLen = this.RichText.SelectionLength;

                int totalIndex = this.RichText.GetCharIndexFromPosition(new Point(0, 0));
                if (oldStart < totalIndex)
                {
                    oldStart = totalIndex + RichText.Lines[line1].Length+1;
                    oldSelectLen = 0;
                }
                if (oldStart > this.RichText.GetFirstCharIndexFromLine(line2-1)+this.RichText.Lines[line2-1].Length)
                {
                    oldStart = this.RichText.GetFirstCharIndexFromLine(line2-1);
                    oldSelectLen = 0;
                }
   
                DataTable tb = Biz.Common.Data.DataHelper.CreateFatTable("pos", "len", "color");
                
                var linesLen = this.RichText.Lines.Length;
                for (int l = line1; l <= line2 && l < linesLen; l++)
                {
                    totalIndex = this.RichText.GetFirstCharIndexFromLine(l);
                    string express = RichText.Lines[l]+" ";
                    
                    if (!_markedLines.Contains(l))
                    {
                        _markedLines.Add(l);

                        foreach (var m in this.KeyWords.MatchKeyWord(express))
                        {
                            if ((m.PostionStart == 0 || "[]{},|%#@!<>=();\r\n 　".IndexOf(express[m.PostionStart-1])>-1)
                                && (m.PostionEnd == express.Length - 1 || "[]{},|%#@!<>=();\r\n 　".IndexOf(express[m.PostionEnd + 1])>-1))
                            {
                                DataRow row = tb.NewRow();
                                row[0] = totalIndex + m.PostionStart;
                                row[1] = m.KeyWordMatched.Length;
                                row[2] = m.Tag;
                                tb.Rows.Add(row);
                            }
                        }
                    }
                }

                this.RichText.LockPaint = true;
                foreach (DataRow row in tb.Rows)
                {
                    this.RichText.SelectionStart = (int)row[0];
                    this.RichText.SelectionLength = (int)row[1];
                    this.RichText.SelectionColor = (Color)row[2];
                }
                this.RichText.SelectionStart = oldStart;
                this.RichText.SelectionLength = oldSelectLen;
                //this.RichText.SelectionColor = oldSelectColor;
                _lastMarketedLines = line1;
                
            }
            finally
            {
                this.RichText.LockPaint = false;
                if (reSetLineNo)
                    SetLineNo();
            }
            
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RichText.Paste();
            MarkKeyWords(true);
        }

        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return contextMenuStrip1;
            }
        }
    }
}
