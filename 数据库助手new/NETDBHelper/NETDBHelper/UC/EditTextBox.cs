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
using Entity;
using LJC.FrameWork.Data.EntityDataBase;

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

        private DataGridView view = new DataGridView();

        public KeyWordManager KeyWords
        {
            get
            {
                return _keyWords;
            }
        }

        public string DBName
        {
            get;
            set;
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

            view.Visible = false;
            view.MouseLeave += View_MouseLeave;
            view.BorderStyle = BorderStyle.None;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //view.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            view.AllowUserToAddRows = false;
            view.RowHeadersVisible = false;
            view.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            view.BackgroundColor = Color.White;
            view.GridColor = Color.LightGreen;
            
            this.ParentChanged += EditTextBox_ParentChanged;

            this.RichText.ImeMode = ImeMode.On;

            this.RichText.HideSelection = false;
        }

        private void EditTextBox_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null&&!this.Parent.Controls.Contains(view))
            {
                this.Parent.Controls.Add(view);
            }
        }

        private void View_MouseLeave(object sender, EventArgs e)
        {
            view.Visible = false;
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

            if (this.RichText.Lines.Length == 0)
            {
                return;
            }

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
                    if (i == line1)
                    {
                        continue;
                    }
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
            if (RichText.SelectionLength == 0)
            {
                var currline = RichText.GetLineFromCharIndex(RichText.SelectionStart);

                if (currline > CurrentClientScreentEndLine && currline + 1 != RichText.Lines.Length)
                {
                    RichText.SelectionStart = RichText.GetFirstCharIndexFromLine(CurrentClientScreentEndLine);
                }
                else if (currline < CurrentClientScreenStartLine)
                {
                    RichText.SelectionStart = RichText.GetFirstCharIndexFromLine(CurrentClientScreenStartLine);
                }
            }
            _timer.SetTimeOutCallBack(() =>
                {
                    this.Invoke(new Action<bool>(MarkKeyWords), true);
                    this.Invoke(new Action(() => {
                        
                    }));
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

        public string SelectedText
        {
            get
            {
                return this.RichText.SelectedText;
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
                //if (oldStart < totalIndex)
                //{
                //    oldStart = totalIndex + RichText.Lines[line1].Length + 1;
                //    oldSelectLen = 0;
                //}
                //if (oldStart > this.RichText.GetFirstCharIndexFromLine(line2 - 1) + this.RichText.Lines[line2 - 1].Length)
                //{
                //    oldStart = this.RichText.GetFirstCharIndexFromLine(line2 - 1);
                //    oldSelectLen = 0;
                //}

                DataTable tb = Biz.Common.Data.DataHelper.CreateFatTable("pos", "len", "color");
                
                var linesLen = this.RichText.Lines.Length;
                for (int l = line1; l <= line2 && l < linesLen; l++)
                {
                    totalIndex = this.RichText.GetFirstCharIndexFromLine(l);
                    string express = RichText.Lines[l]+" ";
                    
                    if (!_markedLines.Contains(l))
                    {
                        _markedLines.Add(l);

                        foreach (var m in this.KeyWords.MatchKeyWord(express.ToLower()))
                        {
                            if ((m.PostionStart == 0 || "[]{},|%#!<>=();+-*/\r\n 　".IndexOf(express[m.PostionStart-1])>-1)
                                && (m.PostionEnd == express.Length - 1 || "[]{},|%#!<>=();+-*/\r\n 　".IndexOf(express[m.PostionEnd + 1])>-1))
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

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RichText.SelectAll();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.RichText.SelectedText);
        }

        private void RichText_MouseHover(object sender, EventArgs e)
        {
            
            

        }

        private Tuple<string, string> GetTableName(string s, string defalutdb)
        {
            var arr = s.Split('.');
            if (arr.Length == 1 || (arr[arr.Length - 2].Equals("dbo", StringComparison.OrdinalIgnoreCase) && arr.Length == 2))
            {
                return new Tuple<string, string>(defalutdb.ToUpper(), arr.Last().Trim('[', ']').Trim().ToUpper());
            }
            else if (arr.Length == 3)
            {
                return new Tuple<string, string>(arr.First().Trim('[',']').Trim().ToUpper(), arr.Last().Trim('[', ']').Trim().ToUpper());
            }
            return new Tuple<string, string>(arr.First().Trim('[', ']').Trim().ToUpper(), 
                arr.Last().Trim('[', ']').Trim().ToUpper());
        }

        private void RichText_SelectionChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DBName))
            {
                return;
            }

            var seltext = this.RichText.SelectedText;
            if (string.IsNullOrWhiteSpace(seltext) || seltext.IndexOf('\n') > -1 || seltext.Length > 30)
            {
                return;
            }

            seltext = seltext.Trim().ToUpper();
            var subtexts = seltext.Split('.').Select(p => p.Trim('[', ']').Trim()).ToArray();
            List<string[]> keys = new List<string[]>();
            if (subtexts.Length > 2)
            {
                keys.Add(new string[] { subtexts[subtexts.Length - 3], subtexts[subtexts.Length - 2], subtexts.Last() });
            }
            else if (subtexts.Length > 1)
            {
                keys.Add(new string[] { DBName.ToUpper(), subtexts[subtexts.Length - 2], subtexts.Last() });
            }
            else
            {
                //[\s\n]+from[\s\r\n]+(?:(?:[\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:\w+)?(?:\,(?:[\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:\s+\w+)?)*)
                //[\s\n]+from[\s\r\n]+((?:[\w\.\[\]]{1,}(?:\s?=\w+)?(?:\,?=[\w\.\[\]]{1,}(?:\s?=\w+)?))*)|[\s\n]+join[\s\n]+([\w\.\[\]]{1,})|(?:^?|\s+)update|insert\s+([\w\.\[\]]+)
                HashSet<Tuple<string, string>> tablenamehash = new HashSet<Tuple<string, string>>();
                foreach (Match m in Regex.Matches(this.RichText.Text, @"[\s\r\n]+from[\s\r\n]+(?:([\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:\w+)?(?:\,[\r\n\s]*(?:[\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:[\s\r\n]+\w+)?)*)|[\s\n\r]+join[\s\n\r]+([\w\.\[\]]{1,})|(?:^?|\s+)update[\s\r\n]+([\w\.\[\]]{1,})|insert[\s\r\n]+into[\s\r\n]+([\w\.\[\]]+)|delete[\s\r\n]+from[\s\r\n]+([\w\.\[\]]+)", 
                    RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    if (!string.IsNullOrWhiteSpace(m.Groups[0].Value))
                    {
                        foreach (Match n in Regex.Matches(m.Groups[0].Value, @",[\s\r\n]*([\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:[\s\r\n]+\w+)?", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        {
                            var t = GetTableName(n.Groups[1].Value,DBName);

                            if (!tablenamehash.Contains(t))
                            {
                                tablenamehash.Add(t);
                            }
                        }
                    }

                    //select
                    if (!string.IsNullOrEmpty(m.Groups[1].Value))
                    {
                        var t1 =GetTableName(m.Groups[1].Value,DBName);

                        if (!tablenamehash.Contains(t1))
                        {
                            tablenamehash.Add(t1);
                        }

                    }

                    //join
                    if (!string.IsNullOrEmpty(m.Groups[2].Value))
                    {
                        var t2 =GetTableName(m.Groups[2].Value,DBName);

                        if (!tablenamehash.Contains(t2))
                        {
                            tablenamehash.Add(t2);
                        }

                    }

                    //update
                    if (!string.IsNullOrEmpty(m.Groups[3].Value))
                    {
                        var t = GetTableName(m.Groups[3].Value, DBName);

                        if (!tablenamehash.Contains(t))
                        {
                            tablenamehash.Add(t);
                        }

                    }

                    //insert
                    if (!string.IsNullOrEmpty(m.Groups[4].Value))
                    {
                        var t = GetTableName(m.Groups[4].Value,DBName);

                        if (!tablenamehash.Contains(t))
                        {
                            tablenamehash.Add(t);
                        }

                    }
                    //delete
                    if (!string.IsNullOrEmpty(m.Groups[5].Value))
                    {
                        var t = GetTableName(m.Groups[5].Value,DBName);

                        if (!tablenamehash.Contains(t))
                        {
                            tablenamehash.Add(t);
                        }

                    }
                }

                if (tablenamehash.Count > 0)
                {
                    foreach(var it in tablenamehash)
                    {
                        keys.Add(new string[] { it.Item1, it.Item2, subtexts.Last() });
                    }
                }

            }

            if (keys.Count>0)
            {
                var marklist = new List<MarkColumnInfo>();
                foreach(var key in keys)
                {
                    var findresult= LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkColumnInfo>("MarkColumnInfo", "keys", key).FirstOrDefault();
                    if (findresult != null)
                    {
                        marklist.Add(findresult);
                    }
                    
                }
                if (marklist.Count > 0)
                {
                    view.DataSource = marklist.Select(p => new
                    {
                        提示 = p.DBName.ToLower() + "." + p.TBName.ToLower() + "." + p.ColumnName.ToLower() + ":" + p.MarkInfo
                    }).ToList();
                    view.Visible = true;
                    
                    view.BringToFront();
                    view.Height = (view.Rows.GetRowsHeight(DataGridViewElementStates.Visible) / marklist.Count) * marklist.Count + view.ColumnHeadersHeight;

                    view.Location = PointToClient(Control.MousePosition);
                }
            }
        }

        private void 搜索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.FindDlg dlg = new SubForm.FindDlg();
            //dlg.FindLast += (s, i) =>
            //{
            //    var pos = this.RichText.Find(s, i,RichTextBoxFinds.NoHighlight);
            //    return pos;
            //};
            dlg.FindNext += (s, i) =>
            {
                var pos = this.RichText.Find(s, i, RichTextBoxFinds.NoHighlight);
                if (pos != -1)
                {
                    this.RichText.Select(pos, s.Length);
                    this.RichText.ScrollToCaret();
                    this.RichText.Focus();
                    return pos + s.Length;
                }
                else
                {
                    return 0;
                }

            };

            dlg.Show();
        }

        private void TSMI_Save_Click(object sender, EventArgs e)
        {
            var sql = this.RichText.Text;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }

            var nameinput = new SubForm.InputStringDlg("备注");
            if (nameinput.ShowDialog() == DialogResult.OK)
            {
                if (LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<SqlSaveEntity>("SqlSave", new SqlSaveEntity
                {
                    Desc = nameinput.InputString,
                    MDate = DateTime.Now,
                    Sql = sql
                }))
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }
    }
}
