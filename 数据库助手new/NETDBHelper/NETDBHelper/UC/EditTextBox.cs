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
using System.Threading;
using System.IO;
using LJC.FrameWorkV3.Data.EntityDataBase;
using LJC.FrameWorkV3.Comm;
using LJC.FrameWorkV3.LogManager;

namespace NETDBHelper.UC
{
    public partial class EditTextBox : UserControl
    {
        public class ThinkInfo
        {
            /// <summary>
            /// 0-关键字 1-表 2-字段
            /// </summary>
            public int Type
            {
                get;
                set;
            }

            /// <summary>
            /// 对象名
            /// </summary>
            public string ObjectName
            {
                get;
                set;
            }

            /// <summary>
            /// 关联内容
            /// </summary>
            public object Tag
            {
                get;
                set;
            }

            /// <summary>
            /// 描述
            /// </summary>
            public string Desc
            {
                get;
                set;
            }

            /// <summary>
            /// 匹配打分
            /// </summary>
            public double Score
            {
                get;
                set;
            }
        }

        public class ViewContext
        {
            /// <summary>
            ///0-空 1-字段提示 2-联想
            /// </summary>
            public byte DataType
            {
                get;
                set;
            }
        }

        private Color defaultSelectionColor;
        //internal KeyWordManager keywordman = new KeyWordManager();
        private KeyWordManager _keyWords=new KeyWordManager();
        private LJC.FrameWork.Comm.WatchTimer _timer = new LJC.FrameWork.Comm.WatchTimer(6);
        private System.Threading.Timer backtimer = null;
        private HashSet<int> _markedLines = new HashSet<int>();
        private int _lastMarketedLines = -1;
        private int _lastInputChar = '\0';
        private GrammarAnalysisResult _lastGrammarAnalysisResult = null;
        private Point _currpt = Point.Empty;
        private DateTime _pointtiptime = DateTime.MaxValue;

        private DataGridView view = new DataGridView();

        /// <summary>
        /// 备选库
        /// </summary>
        private List<ThinkInfo> ThinkInfoLib = null;
        private List<SubSearchSourceItem> sourceList = null;
        private HashSet<string> TableSet = new HashSet<string>();
        private object GetObjects(string keys, ref int count)
        {
            if (string.IsNullOrWhiteSpace(_dbname))
            {
                return null;
            }

            ProcessTraceUtil.StartTrace();

            if (sourceList == null)
            {
                var markColumnInfoList = BigEntityTableRemotingEngine.List<MarkObjectInfo>("MarkObjectInfo", 1, int.MaxValue).ToList();
                ProcessTraceUtil.Trace("BigEntityTableRemotingEngine.List<MarkObjectInfo>");
                ThinkInfoLib = new List<ThinkInfo>();

                foreach (var o in SQLKeyWordHelper.GetKeyWordList())
                {
                    ThinkInfoLib.Add(new ThinkInfo
                    {
                        Type = 0,
                        Desc = o.Desc,
                        ObjectName = o.KeyWord
                    });
                }

                count = 0;

                HashSet<string> hash = new HashSet<string>();
                var exdbhash = new HashSet<string>(DBServer.ExDBList?.Select(p => p.ToUpper()) ?? new List<string>());
                var extbhash = new HashSet<string>(DBServer.ExTBList?.Select(p => p.ToUpper()) ?? new List<string>());
                var tbHash = new HashSet<string>();

                var tbs= markColumnInfoList.Where(p => !string.IsNullOrWhiteSpace(p.ColumnName)).GroupBy(p=>new { p.DBName,p.TBName,p.Servername}).Select(p => p.First()).ToList();
                var tbnamemanger = new LJC.FrameWorkV3.CodeExpression.KeyWordMatch.KeyWordManager();
                foreach(var tb in tbs)
                {
                    tbnamemanger.AddKeyWord($"{tb.TBName.ToUpper()}",tb);
                }
                var matchresult = tbnamemanger.MatchKeyWord(RichText.Text.ToUpper());
                foreach (var m in matchresult)
                {
                    if (m.PostionStart > 0)
                    {
                        var lastch = RichText.Text[m.PostionStart - 1];
                        if (lastch != ' ' && lastch != '[' && lastch != '\t' && lastch != '\r' && lastch != '\n')
                        {
                            continue;
                        }
                    }
                    if (m.PostionEnd < RichText.Text.Length - 1)
                    {
                        var nextch = RichText.Text[m.PostionEnd + 1];
                        if (nextch != ' ' && nextch != ']' && nextch != '\t' && nextch != '\r' && nextch != '\n')
                        {
                            continue;
                        }
                    }
                    var tag = m.Tag as MarkObjectInfo;
                    var key = (1 + "_" + tag.DBName + tag.TBName).ToUpper();
                    if (!hash.Contains(key))
                    {
                        if (!TableSet.Contains(tag.TBName))
                        {
                            TableSet.Add(tag.TBName);
                        }
                        var thinkinfo = new ThinkInfo
                        {
                            Type = 1,
                            ObjectName = tag.TBName.ToLower(),
                            Tag = new MarkObjectInfo
                            {
                                DBName = tag.DBName,
                                Servername = tag.Servername,
                                TBName = tag.TBName
                            },
                            Desc = string.Empty
                        };
                        ThinkInfoLib.Add(thinkinfo);
                        hash.Add(key);
                    }
                }

                foreach (var m in markColumnInfoList)
                {
                    if (/*m.Servername != DBServer.ServerName ||*/ exdbhash.Contains(m.DBName) || extbhash.Contains(m.DBName + "," + m.TBName?.Split('.').Last()))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(m.ColumnName))
                    {
                        var key = $"{m.TBName.ToUpper()}";
                        //ThinkInfoLib.RemoveAll(p => p.Type == 1 && ((MarkObjectInfo)p.Tag)?.DBName.Equals(m.DBName, StringComparison.OrdinalIgnoreCase) == true && p.ObjectName.Equals(m.TBName, StringComparison.OrdinalIgnoreCase));
                        if (!tbHash.Contains(key))
                        {
                            ThinkInfoLib.Add(new ThinkInfo { Type = 1, ObjectName = m.TBName.ToLower(), Tag = m, Desc = m.MarkInfo });
                            tbHash.Add(key);
                        }
                    }
                    else
                    {
                        string desc = m.ColumnType;
                        if (!string.IsNullOrWhiteSpace(m.MarkInfo))
                        {
                            desc += $",{m.MarkInfo}";
                        }
                        ThinkInfoLib.Add(new ThinkInfo { Type = 2, Desc = desc, ObjectName = m.ColumnName.ToLower(), Tag = m });
                    }

                    if (count == 20)
                    {
                        break;
                    }
                }

                sourceList = ThinkInfoLib.Select(p => new SubSearchSourceItem(p.ObjectName.ToUpper(), p)).ToList();
                sourceList.AddRange(ThinkInfoLib.Where(p => !string.IsNullOrWhiteSpace(p.Desc)).Select(p => new SubSearchSourceItem(p.Desc.ToUpper(), p)).ToList());
            }

            var searchtable = string.Empty;
            var searchKeys = keys;
            if (searchKeys.IndexOf('.') > -1)
            {
                var keyarr = searchKeys.Split('.');
                searchtable = keyarr[keyarr.Length - 2];
                searchKeys = keyarr.Last();
            }

            List<ThinkInfo> thinkresut = new List<ThinkInfo>();

            ProcessTraceUtil.Trace("start search");
            ThinkInfoLib.ForEach(p => p.Score = 0);
            ProcessTraceUtil.Trace("start subsearch");

            var tableKeyManager = new LJC.FrameWorkV3.CodeExpression.KeyWordMatch.KeyWordManager();
            foreach(var item in TableSet)
            {
                tableKeyManager.AddKeyWord(item.ToUpper(), item);
            }
            TableSet.Clear();
            foreach (var item in tableKeyManager.MatchKeyWord(RichText.Text.ToUpper()))
            {
                TableSet.Add(item.Tag.ToString());
            }
            ProcessTraceUtil.Trace("重新检查表:" + TableSet.Count + "个");

            if (!string.IsNullOrWhiteSpace(searchKeys))
            {
                thinkresut = LJC.FrameWorkV3.Comm.StringHelper.SubSearch(sourceList.Where(p =>
                {
                    var to = (ThinkInfo)p.Tag;
                    if (!string.IsNullOrEmpty(searchtable) && (to.Type != 2 || !searchtable.Equals((to.Tag as MarkObjectInfo).TBName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }
                    if (to.Type == 2)
                    {
                        var markcolumn = (MarkObjectInfo)to.Tag;
                        var tablename = string.Empty;
                        if (!markcolumn.DBName.Equals(_dbname, StringComparison.OrdinalIgnoreCase))
                        {
                            tablename = $"{markcolumn.DBName.ToLower()}.dbo.{markcolumn.TBName}";
                        }
                        else
                        {
                            tablename = markcolumn.TBName;
                        }
                        if (!TableSet.Contains(tablename, StringComparer.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                    return true;
                }), searchKeys.ToUpper(), 3, 1000)
                    .Select(p => (ThinkInfo)p).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(searchtable))
            {
                thinkresut = ThinkInfoLib.Where(p => (p.Tag as MarkObjectInfo)?.TBName?.Equals(searchtable, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }

            ProcessTraceUtil.Trace("SubSearch");

            int score = 0;
            ThinkInfo lastItem = null;
            foreach (var item in thinkresut)
            {
                if (lastItem != null && !item.ObjectName.Equals(lastItem.ObjectName, StringComparison.OrdinalIgnoreCase))
                {
                    score++;
                }
                item.Score = score;
                lastItem = item;
            }

            //foreach (var item in ThinkInfoLib)
            //{
            //    if (!string.IsNullOrEmpty(searchtable) && (item.Type != 2 || !searchtable.Equals((item.Tag as MarkObjectInfo).TBName, StringComparison.OrdinalIgnoreCase)))
            //    {
            //        continue;
            //    }

            //    var tagobj = item.Tag as MarkObjectInfo;
            //    var desc = tagobj?.MarkInfo??item.Desc;
            //    var fullobjectname = item.ObjectName;
            //    if (item.Type == 1)
            //    {
            //        if (!tagobj.DBName.Equals(_dbname))
            //        {
            //            fullobjectname = $"{tagobj.DBName}.{item.ObjectName}";
            //        }
            //    }
            //    else if (item.Type == 2)
            //    {
            //        if (!tagobj.DBName.Equals(_dbname))
            //        {
            //            fullobjectname = $"{tagobj.DBName}.{item.ObjectName}";
            //        }
            //    }
            //    if (item.ObjectName.Equals(keys, StringComparison.OrdinalIgnoreCase)
            //        || (item.ObjectName.Equals(keys, StringComparison.OrdinalIgnoreCase) == true))
            //    {
            //        item.Score = (byte)(byte.MaxValue - (byte)fullobjectname.IndexOf(keys, StringComparison.OrdinalIgnoreCase));
            //        thinkresut.Add(item);
            //        continue;
            //    }

            //    if (item.ObjectName.StartsWith(keys, StringComparison.OrdinalIgnoreCase)
            //        || (desc?.StartsWith(keys, StringComparison.OrdinalIgnoreCase)) == true)
            //    {
            //        item.Score = (byte)(byte.MaxValue - 1 - (byte)fullobjectname.Length);
            //        thinkresut.Add(item);
            //        continue;
            //    }

            //    int pos = fullobjectname.IndexOf(keys, StringComparison.OrdinalIgnoreCase);
            //    if (pos > -1)
            //    {
            //        item.Score = Math.Max((byte)(byte.MaxValue - (byte)fullobjectname.Length - (byte)pos), (byte)0);
            //        thinkresut.Add(item);
            //        continue;
            //    }
            //    else
            //    {
            //        pos = desc?.IndexOf(keys, StringComparison.OrdinalIgnoreCase) ?? -1;
            //        if (pos > -1)
            //        {
            //            item.Score = Math.Max((byte)(byte.MaxValue - fullobjectname.Length - (byte)item.Desc.Length - (byte)pos), (byte)0);
            //            thinkresut.Add(item);
            //            continue;
            //        }
            //    }
            //}

            

            //thinkresut = thinkresut.Where(p =>
            //{
            //    if (!string.IsNullOrEmpty(searchtable) && (p.Type != 2 || !searchtable.Equals((p.Tag as MarkObjectInfo).TBName, StringComparison.OrdinalIgnoreCase)))
            //    {
            //        return false;
            //    }
            //    if (p.Type == 2)
            //    {
            //        var markcolumn = (MarkObjectInfo)p.Tag;
            //        var tablename = string.Empty;
            //        if (!markcolumn.DBName.Equals(_dbname, StringComparison.OrdinalIgnoreCase))
            //        {
            //            tablename = $"{markcolumn.DBName.ToLower()}.dbo.{markcolumn.TBName}";
            //        }
            //        else
            //        {
            //            tablename = markcolumn.TBName;
            //        }
            //        if (!TableSet.Contains(tablename, StringComparer.OrdinalIgnoreCase))
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //}).ToList();

            ProcessTraceUtil.Trace("处理结果");

            foreach (var p in thinkresut)
            {
                if (p.Type == 1)
                {
                    var markcolumn = (MarkObjectInfo)p.Tag;
                    var tablename = string.Empty;
                    if (!markcolumn.DBName.Equals(_dbname, StringComparison.OrdinalIgnoreCase))
                    {
                        tablename = $"{markcolumn.DBName.ToLower()}.dbo.{ p.ObjectName}";
                        p.Score++;
                    }
                    else
                    {
                        tablename = p.ObjectName;
                    }
                    if (!TableSet.Contains(tablename, StringComparer.OrdinalIgnoreCase))
                    {
                        TableSet.Add(tablename);
                    }
                }
            }

            count = thinkresut.Count;
            var ret= thinkresut.OrderBy(p => p.Score).Select(p =>
            {
                string objectname = null;
                string replaceobjectname = null;
                bool issamedb = true;
                if (p.Type == 2)
                {
                    var markcolumn = (p.Tag as MarkObjectInfo);
                    issamedb = markcolumn.DBName.Equals(_dbname, StringComparison.OrdinalIgnoreCase);
                    if (issamedb)
                    {
                        objectname = $"{markcolumn.TBName.ToLower()}.{p.ObjectName}";
                    }
                    else
                    {
                        objectname = $"{markcolumn.DBName.ToLower()}.dbo.{markcolumn.TBName.ToLower()}.{p.ObjectName}";
                    }
                    replaceobjectname = $"{markcolumn.TBName.ToLower()}.{p.ObjectName}";
                }
                else if (p.Type == 1)
                {
                    var markcolumn = (p.Tag as MarkObjectInfo);
                    issamedb = markcolumn.DBName.Equals(_dbname, StringComparison.OrdinalIgnoreCase);
                    if (issamedb)
                    {
                        objectname = p.ObjectName;
                        replaceobjectname = p.ObjectName;
                    }
                    else
                    {
                        objectname = $"{markcolumn.DBName.ToLower()}.dbo.{p.ObjectName}";
                        replaceobjectname = $"{markcolumn.DBName.ToLower()}.dbo.{p.ObjectName}";
                    }
                }
                else
                {
                    objectname = p.ObjectName;
                    replaceobjectname = p.ObjectName;
                }
                return new
                {
                    建议 = objectname,
                    说明 = p.Desc,
                    p.Type,
                    Issamedb = issamedb,
                    replaceobjectname
                };
            }).Take(200).ToList();

            ProcessTraceUtil.Trace("排序结果");

            LogHelper.Instance.Debug(ProcessTraceUtil.PrintTrace(100));

            return ret;
        }

        public DBSource DBServer
        {
            get;
            set;
        }

        private string _dbname;
        public string DBName
        {
            get
            {
                return _dbname;
            }
            set
            {
                _dbname = value;
            }
        }

        public KeyWordManager KeyWords
        {
            get
            {
                return _keyWords;
            }
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

        public int SelectionLength
        {
            get
            {
                return this.RichText.SelectionLength;
            }
        }

        public string SelectedText
        {
            get
            {
                return this.RichText.SelectedText;
            }
            set
            {
                this.RichText.Text = value;
            }
        }

        public EditTextBox()
        {
            InitializeComponent();
            this.RichText.WordWrap = false;
            this.DoubleBuffered = true;
            this.RichText.ScrollBars = RichTextBoxScrollBars.Both;
            this.RichText.ContextMenuStrip = this.contextMenuStrip1;
            this.RichText.VScroll += new EventHandler(RichText_VScroll);
            this.ScaleNos.Font = new Font(RichText.Font.FontFamily, RichText.Font.Size + 1.019f);
            this.RichText.KeyUp += new KeyEventHandler(RichText_KeyUp);
            this.RichText.KeyDown += RichText_KeyDown;
            this.RichText.TextChanged+=new EventHandler(RichText_TextChanged);
            this.RichText.MouseClick += RichText_MouseClick;
            this.RichText.MouseMove += RichText_MouseMove;
            this.RichText.MouseLeave += RichText_MouseLeave;
            this.RichText.DoubleClick += RichText_DoubleClick;
            contextMenuStrip1.VisibleChanged += ContextMenuStrip1_VisibleChanged;

            defaultSelectionColor = this.RichText.SelectionColor;

            view.Visible = false;
            view.MouseLeave += View_MouseLeave;
            view.BorderStyle = BorderStyle.None;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            view.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            view.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            view.AllowUserToAddRows = false;
            view.RowHeadersVisible = false;
            view.KeyUp += View_KeyUp;
            view.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            view.BackgroundColor = Color.White;
            view.GridColor = Color.LightGreen;
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            view.CellClick += View_CellClick;
            view.RowPostPaint += View_RowPostPaint;
            view.DataBindingComplete += View_DataBindingComplete;

            剪切ToolStripMenuItem.Enabled = false;
            
            view.Tag = new ViewContext
            {
                DataType=0
            };
            
            this.ParentChanged += EditTextBox_ParentChanged;

            this.RichText.ImeMode = ImeMode.Close;

            this.RichText.HideSelection = false;

            backtimer = new System.Threading.Timer(new System.Threading.TimerCallback((o) =>
              {
                  if (this.Visible&& !view.Visible && _currpt != Point.Empty && DateTime.Now.Subtract(_pointtiptime).TotalMilliseconds >= 1000)
                  {
                      _pointtiptime = DateTime.MaxValue;
                      backtimer.Change(0, Timeout.Infinite);
                      this.BeginInvoke(new Action(() =>
                      {
                          try
                          {
                              ShowTip();
                          }
                          catch(Exception ex)
                          {
                              Util.SendMsg(this, ex.ToString());
                          }
                          finally
                          {
                              backtimer.Change(0, 100);
                          }
                      }));
                  }
              }), null, 0, 100);
        }

        private void ContextMenuStrip1_VisibleChanged(object sender, EventArgs e)
        {
            this.重做ToolStripMenuItem.Enabled = this.RichText.CanRedo;
            this.撤消ToolStripMenuItem.Enabled = this.RichText.CanUndo;
        }

        private void RichText_DoubleClick(object sender, EventArgs e)
        {
            int st;
            var seltext = GetTipCurrWord(false, out st);
            if (string.IsNullOrWhiteSpace(seltext) || seltext.IndexOf('\n') > -1)
            {
                return;
            }
            this.RichText.Select(st-seltext.Length, seltext.Length);
        }

        private void ShowTip()
        {
            if (string.IsNullOrWhiteSpace(DBName))
            {
                return;
            }
            int st;
            var seltext = GetTipCurrWord(true,out st);
            if (string.IsNullOrWhiteSpace(seltext) || seltext.IndexOf('\n') > -1)
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
                var alltext = RichText.Text;
                var diff = int.MaxValue;
                Match match = null;
                foreach (Match m in Regex.Matches(alltext, $@"[\r\n\t\s]+\[?([\w_]+)\]?(?:[\r\n\t\s]+as)?[\r\n\s\t]+{subtexts[subtexts.Length - 2]}[\r\n\s\t]+", RegexOptions.IgnoreCase))
                {
                    if (Math.Abs(m.Index - st) < diff)
                    {
                        match = m;
                        diff = Math.Abs(m.Index - st);
                    }
                    else
                    {
                        match = m;
                        break;
                    }
                }
                if (match != null)
                {
                    keys.Add(new string[] { DBName.ToUpper(), match.Groups[1].Value.ToUpper(), subtexts.Last() });
                }
                else
                {
                    keys.Add(new string[] { DBName.ToUpper(), subtexts[subtexts.Length - 2], subtexts.Last() });
                }
            }
            else
            {
                //[\s\n]+from[\s\r\n]+(?:(?:[\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:\w+)?(?:\,(?:[\w\.\[\]]{1,})[\s\r\n]+(?:as)?(?:\s+\w+)?)*)
                //[\s\n]+from[\s\r\n]+((?:[\w\.\[\]]{1,}(?:\s?=\w+)?(?:\,?=[\w\.\[\]]{1,}(?:\s?=\w+)?))*)|[\s\n]+join[\s\n]+([\w\.\[\]]{1,})|(?:^?|\s+)update|insert\s+([\w\.\[\]]+)
                HashSet<Tuple<string, string>> tablenamehash = new HashSet<Tuple<string, string>>();
                foreach (Match m in Regex.Matches(this.RichText.Text, @"[\s\r\n]+from[\s\r\n]+(?:([\w\.\[\]]{1,})[\s\r\n]+)|(?:[\s\n\r]+|^)join[\s\n\r]+([\w\.\[\]]{1,})|(?:^?|\s+)update[\s\r\n]+([\w\.\[\]]{1,})|insert[\s\r\n]+into[\s\r\n]+([\w\.\[\]]+)|delete[\s\r\n]+from[\s\r\n]+([\w\.\[\]]+)",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    if (!string.IsNullOrWhiteSpace(m.Groups[0].Value))
                    {
                        foreach (Match n in Regex.Matches(m.Groups[0].Value, @",[\s\r\n]*([\w\.\[\]]{1,})[\s\r\n]+", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        {
                            var t = GetTableName(n.Groups[1].Value, DBName);

                            if (!tablenamehash.Contains(t))
                            {
                                tablenamehash.Add(t);
                            }
                        }
                    }

                    //select
                    if (!string.IsNullOrEmpty(m.Groups[1].Value))
                    {
                        var t1 = GetTableName(m.Groups[1].Value, DBName);

                        if (!tablenamehash.Contains(t1))
                        {
                            tablenamehash.Add(t1);
                        }

                    }

                    //join
                    if (!string.IsNullOrEmpty(m.Groups[2].Value))
                    {
                        var t2 = GetTableName(m.Groups[2].Value, DBName);

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
                        var t = GetTableName(m.Groups[4].Value, DBName);

                        if (!tablenamehash.Contains(t))
                        {
                            tablenamehash.Add(t);
                        }

                    }
                    //delete
                    if (!string.IsNullOrEmpty(m.Groups[5].Value))
                    {
                        var t = GetTableName(m.Groups[5].Value, DBName);

                        if (!tablenamehash.Contains(t))
                        {
                            tablenamehash.Add(t);
                        }

                    }
                }

                if (tablenamehash.Count > 0)
                {
                    foreach (var it in tablenamehash)
                    {
                        keys.Add(new string[] { it.Item1, it.Item2, subtexts.Last() });
                    }
                }

            }

            if (keys.Count > 0)
            {
                var marklist = new List<MarkObjectInfo>();
                foreach (var key in keys)
                {
                    var findresult = BigEntityTableRemotingEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", key).FirstOrDefault();
                    if (findresult != null)
                    {
                        marklist.Add(findresult);
                    }

                }
                if (marklist.Count > 0)
                {
                    (view.Tag as ViewContext).DataType = 1;
                    view.DataSource = marklist.Select(p => {
                        if (p.DBName.Equals(_dbname, StringComparison.OrdinalIgnoreCase))
                        {
                            return new
                            {
                                提示 = $"{p.TBName.ToLower()}.{p.ColumnName.ToLower()}({p.ColumnType}):{p.MarkInfo}"
                            };
                        }
                        else
                        {
                            return new
                            {
                                提示 = $"{p.DBName.ToLower()}.{p.TBName.ToLower()}.{p.ColumnName.ToLower()}({p.ColumnType}):{p.MarkInfo}"
                            };
                        }
                    }).ToList();
                    
                    var padding = view.Columns[0].DefaultCellStyle.Padding;
                    padding.Left = 1;
                    view.Columns[0].DefaultCellStyle.Padding = padding;
                    
                    view.Visible = true;

                    view.BringToFront();
                    
                    var postion = PointToClient(Control.MousePosition);
                    var parentpostion = this.Location;
                    var offsety = 5;
                    if (postion.X + view.Width > parentpostion.X + this.Width)
                    {
                        postion.Offset(parentpostion.X + this.Width - postion.X - view.Width, offsety);
                    }
                    else
                    {
                        postion.Offset(0, offsety);
                    }
                    view.Location = postion;
                }
            }
        }

        private void RichText_MouseLeave(object sender, EventArgs e)
        {
            this._pointtiptime = DateTime.MaxValue;
            this._currpt = Point.Empty;
        }

        private void RichText_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currpt != e.Location)
            {
                _currpt = e.Location;
                _pointtiptime = DateTime.Now;
            }
        }

        private void RichText_MouseClick(object sender, MouseEventArgs e)
        {
            if (view.Visible)
            {
                var tippt = this.RichText.GetPositionFromCharIndex(this.RichText.SelectionIndent);
                if (Math.Abs(e.X - tippt.X) > 10 && Math.Abs(e.Y - tippt.Y) > 10)
                {
                    view.Visible = false;
                }
            }
        }

        private void View_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if ((view.Tag as ViewContext).DataType == 2)
            {
                view.Columns["Type"].Visible = false;
                view.Columns["Issamedb"].Visible = false;
                view.Columns["replaceobjectname"].Visible = false;
            }

            var ajustviewwith = 0;
            int icount = 0;
            List<int> maxwidthlist = new List<int>();
            foreach (DataGridViewColumn col in view.Columns)
            {
                if (!col.Visible)
                {
                    continue;
                }

                int maxwidth = 0;
                foreach (DataGridViewRow row in view.Rows)
                {
                    using (var g = view.CreateGraphics())
                    {
                        var mwidth = col.DefaultCellStyle.Padding.Left + (int)g.MeasureString(row.Cells[col.Name].Value.ToString() + col.Name, view.Font).Width + 20;
                        if (mwidth > maxwidth)
                        {
                            maxwidth = mwidth;
                        }
                    }
                }
                ajustviewwith += maxwidth;
                if (icount < view.DisplayedColumnCount(false))
                {
                    maxwidthlist.Add(maxwidth);
                }
                icount++;
            }

            var limitwidth = (int)(this.Width * 0.8);
            var width = Math.Min(ajustviewwith, limitwidth);

            view.Width = width;

            var rate = width < ajustviewwith ? (width*1.0/ajustviewwith): 1.0;
            icount = 0;
            foreach (DataGridViewColumn col in view.Columns)
            {
                if (!col.Visible)
                {
                    continue;
                }
                col.Width = (int)(maxwidthlist[icount]*rate);
                icount++;
            }

            var height = view.ColumnHeadersHeight;
            for (var i = 0; i < Math.Min(view.Rows.Count, 10); i++)
            {
                height += view.Rows[i].Height;
            }
            view.Height = height;
        }

        private void View_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if ((view.Tag as ViewContext).DataType == 2)
            {
                Bitmap rowIcon = null;

                var tp = (int)view.Rows[e.RowIndex].Cells["Type"].Value;
                if (tp == 1)
                {
                    rowIcon = Resources.Resource1.table;
                }
                else if (tp == 2)
                {
                    rowIcon = Resources.Resource1.DB6;
                }

                if (rowIcon != null)
                    e.Graphics.DrawImage(rowIcon, e.RowBounds.Left + 4, Convert.ToInt16((e.RowBounds.Top + e.RowBounds.Bottom) / 2 - 8), 16, 16);
            }

        }

        private void RichText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyData == (Keys.Control | Keys.Z))
            {
                e.Handled = true;
                RichText.Undo();
                return;
            }

            if (e.Control && e.KeyData == (Keys.Control | Keys.R))
            {
                e.Handled = true;
                RichText.Redo();
                return;
            }

            if (e.KeyCode == Keys.Down
                || e.KeyCode == Keys.Up)
            {
                if (view.Visible)
                {
                    View_KeyUp(this, new KeyEventArgs(e.KeyCode));
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Enter
                || e.KeyCode == Keys.Space
                || e.KeyCode == Keys.Right)
            {
                if (view.Visible)
                {
                    int i = 0;
                    for (; i < view.Rows.Count; i++)
                    {
                        if (view.Rows[i].Selected)
                        {
                            view.Rows[i].Selected = false;
                            break;
                        }
                    }
                    if (i < view.Rows.Count)
                    {
                        View_CellClick(e.KeyCode, new DataGridViewCellEventArgs(0, i));
                    }
                    else
                    {
                        view.Visible = false;
                    }
                    e.Handled = true;

                }
            }
            else if (Keys.Oem7 == e.KeyCode)
            {
                var infos = GrammarAnalysis();
                var currentIndex = RichText.SelectionStart;
                var currentLine=RichText.GetLineFromCharIndex(currentIndex);
                var currentPos = currentIndex - RichText.GetFirstCharIndexOfCurrentLine();
                _lastGrammarAnalysisResult = infos;
                if (!infos.AnnotationInfos.Any(p => p.Contains(currentLine, currentPos))
                    && !infos.StringInfos.Any(p => p.Contains(currentLine, currentPos)))
                {
                    _lastInputChar = '\0';
                    var old = Clipboard.GetText();
                    Clipboard.SetText("'");
                    RichText.Paste();
                    if (!string.IsNullOrEmpty(old))
                    {
                        Clipboard.SetText(old);
                    }
                    _lastInputChar = '\'';
                }
                
            }
        }

        private string GetTipCurrWord(bool includedot, out int start)
        {
            start = -1;
            var curindex = this.RichText.GetCharIndexFromPosition(_currpt);
            var realpt = this.RichText.GetPositionFromCharIndex(curindex);
            if (_currpt.X - realpt.X < -10 || _currpt.X - realpt.X > 15)
            {
                return string.Empty;
            }
            if (_currpt.Y - realpt.Y < -10 || _currpt.Y - realpt.Y > 15)
            {
                return string.Empty;
            }
            var currline = this.RichText.GetLineFromCharIndex(curindex);

            var charstartindex = this.RichText.GetFirstCharIndexFromLine(currline);
            var tippt = this.RichText.GetPositionFromCharIndex(curindex);
            tippt.Offset(0, 20);
            string pre = "", last = "";
            int pi = curindex - charstartindex - 1;

            var currLineText = RichText.Lines[currline];
            //判断是否是注释部分
            var nodeindex = currLineText?.IndexOf("--");
            if (nodeindex > -1 && pi >= nodeindex)
            {
                start = -1;
                return string.Empty;
            }

            var linesLen = RichText.Lines.Length;
            while (pi >= 0)
            {

                var ch = currLineText[pi];

                if ((ch >= 'A' && ch <= 'Z') || (ch >= 48 && ch <= 57) || (ch >= 'a' && ch <= 'z')
                    || ch == '_' || ch == '@' || (includedot && ch == '.')
                    || (ch >= '\u4E00' && ch <= '\u9FA5'))
                {
                    pre = ch + pre;
                    pi--;
                }
                else
                {
                    break;
                }
            }
            pi = curindex - charstartindex;
            if (linesLen > currline)
            {
                while (pi < currLineText.Length)
                {
                    var ch = currLineText[pi];

                    if ((ch >= 'A' && ch <= 'Z') || (ch >= 48 && ch <= 57) || (ch >= 'a' && ch <= 'z')
                        || ch == '_' || ch == '@' || (includedot && ch == '.')
                        || (ch >= '\u4E00' && ch <= '\u9FA5'))
                    {
                        last += ch;
                        pi++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var keyword = pre + last;
            start = pi + charstartindex;
            return keyword;
        }

        private int GetCurrWord(out string word)
        {
            var curindex = this.RichText.SelectionStart;
            var currline = this.RichText.GetLineFromCharIndex(curindex);
            //var charstartindex = this.RichText.GetFirstCharIndexOfCurrentLine();
            var charstartindex = this.RichText.GetFirstCharIndexFromLine(currline);
            var tippt = this.RichText.GetPositionFromCharIndex(curindex);
            tippt.Offset(0, 20);
            string pre = "", last = "";
            int pi = curindex - charstartindex - 1;

            var currLineText = RichText.Lines[currline];
            var linesLen = RichText.Lines.Length;
            //判断是否是注释部分
            var nodeindex = currLineText?.IndexOf("--");
            if (nodeindex > -1 && pi >= nodeindex)
            {
                word = string.Empty;
                return -1;
            }

            while (pi >= 0)
            {

                var ch = currLineText[pi];

                if ((ch >= 'A' && ch <= 'Z') || (ch >= 48 && ch <= 57) || (ch >= 'a' && ch <= 'z') 
                    || ch == '_' || ch == '.' || ch == '@'
                    || (ch>= '\u4E00'&&ch<='\u9FA5'))
                {
                    pre = ch + pre;
                    pi--;
                }
                else
                {
                    break;
                }
            }
            pi = curindex - charstartindex;
            if (linesLen > currline)
            {
                while (pi < currLineText.Length)
                {
                    var ch = currLineText[pi];

                    if ((ch >= 'A' && ch <= 'Z') || (ch >= 48 && ch <= 57) || (ch >= 'a' && ch <= 'z')
                        || ch == '_' || ch == '.' || ch == '@'
                        || (ch >= '\u4E00' && ch <= '\u9FA5'))
                    {
                        last += ch;
                        pi++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var keyword = pre + last;
            word = keyword;
            return pi + charstartindex;
        }

        private void View_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if ((view.Tag as ViewContext).DataType == 2)
            {
                var val = view.Rows[e.RowIndex].Cells["replaceobjectname"].Value.ToString();
                var Issamedb = (bool)view.Rows[e.RowIndex].Cells["Issamedb"].Value;
                string keyword;
                var keywordindex = GetCurrWord(out keyword);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    this.RichText.LockPaint = true;
                    //this.RichText.Select(this.RichText.SelectionStart - keyword.Length, keyword.Length);
                    this.RichText.Select(keywordindex - keyword.Length, keyword.Length);
                    //this.RichText.Text.Remove(this.RichText.SelectionStart - keyword.Length, keyword.Length);

                    if (keyword.IndexOf('.') > -1 || sender?.Equals(Keys.Right) == true || !Issamedb)
                    {
                        this.RichText.SelectedText = val;
                    }
                    else
                    {
                        this.RichText.SelectedText = val.Split('.').Last();
                    }
                    //this.RichText.SelectionStart += val.Length - keyword.Length;
                    view.Visible = false;
                    this.RichText.LockPaint = false;
                    this.RichText.Focus();
                }
            }
        }

        private void View_KeyUp(object sender, KeyEventArgs e)
        {
            int i = 0;
            for(; i < view.Rows.Count; i++)
            {
                if (view.Rows[i].Selected)
                {
                    view.Rows[i].Selected = false;
                    break;
                }
            }
            if (i == view.Rows.Count)
            {
                i = -1;
            }
            if (e.KeyCode == Keys.Up)
            {
                if (i < 0)
                {
                    view.Rows[view.Rows.Count - 1].Selected = true;
                    view.CurrentCell = view.Rows[view.Rows.Count - 1].Cells[0];
                }
                else if (i == 0)
                {
                    view.ClearSelection();
                }
                else
                {
                    view.Rows[i - 1].Selected = true;
                    view.CurrentCell = view.Rows[i - 1].Cells[0];
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (i == view.Rows.Count - 1)
                {
                    view.Rows[0].Selected = true;
                    view.CurrentCell = view.Rows[0].Cells[0];
                }
                else
                {
                    view.Rows[i + 1].Selected = true;
                    view.CurrentCell = view.Rows[i + 1].Cells[0];
                }
            }
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
            if ((view.Tag as ViewContext).DataType == 2)
            {
                return;
            }
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
            //if (e.Shift)
            //    return;
            if (e.KeyData == (Keys.LButton|Keys.ShiftKey))
            {
                return;
            }
            _lastInputChar = e.KeyValue;
        }

        private void ClearMarkedLine(int[] lines)
        {
            if (lines.Length == 0)
            {
                return;
            }
            lines = lines.OrderBy(p => p).ToArray();
            this.RichText.LockPaint = true;
            var oldstart = this.RichText.SelectionStart;
            var oldlen = this.RichText.SelectionLength;
            int selectionStart = RichText.GetFirstCharIndexFromLine(lines.First());

            for (var i = 1; i < lines.Length; i++)
            {
                if (lines[i] - lines[i - 1] != 1)
                {
                    this.RichText.SelectionStart = selectionStart;
                    this.RichText.SelectionLength = RichText.GetFirstCharIndexFromLine(lines[i]) - selectionStart + RichText.Lines[lines[i]].Length;
                    this.RichText.SelectionColor = Color.Black;
                    selectionStart = RichText.GetFirstCharIndexFromLine(lines[i]);
                }
            }

            if (selectionStart != -1)
            {
                this.RichText.SelectionStart = selectionStart;
                this.RichText.SelectionLength = RichText.GetFirstCharIndexFromLine(lines[lines.Length - 1]) - selectionStart + RichText.Lines[lines[lines.Length - 1]].Length;
                this.RichText.SelectionColor = Color.Black;

                this.RichText.SelectionStart = oldstart;
                this.RichText.SelectionLength = oldlen;
                this.RichText.LockPaint = false;
            }
        }

        void RichText_TextChanged(object sender, EventArgs e)
        {
            if (_lastInputChar == '\0')
                return;
            if (this.RichText.Lines.Length == 0)
            {
                return;
            }

            #region 联想
            string keyword;
            var keywordindex = GetCurrWord(out keyword);

            if (!string.IsNullOrEmpty(keyword))
            {
                int count = 0;
                var obj = GetObjects(keyword, ref count);
                if (obj != null && count > 0)
                {
                    (view.Tag as ViewContext).DataType = 2;
                    view.DataSource = obj;
                    
                    var padding = view.Columns[0].DefaultCellStyle.Padding;
                    padding.Left = 20;
                    view.Columns[0].DefaultCellStyle.Padding = padding;
                    view.Visible = true;

                    view.ClearSelection();
                    view.BringToFront();

                    var curindex = this.RichText.SelectionStart;
                    var tippt = this.RichText.GetPositionFromCharIndex(curindex);
                    tippt.Offset(RichText.Location.X, 20);
                    var morewidth = tippt.X + view.Width - view.Parent.Location.X - view.Parent.Width;
                    if (morewidth > 0)
                    {
                        tippt.Offset(-morewidth, 0);
                    }
                    if (view.Height + tippt.Y + 30 > this.Parent.Location.Y + this.Parent.Height)
                    {
                        tippt.Offset(0, -view.Height - 20);
                    }
                    view.ScrollBars = ScrollBars.Vertical;
                    view.Location = tippt;

                }
                else
                {
                    (view.Tag as ViewContext).DataType = 0;
                    view.DataSource = null;
                    view.Visible = false;
                }
            }
            else
            {
                (view.Tag as ViewContext).DataType = 0;
                view.DataSource = null;
                view.Visible = false;
            }

            #endregion

            int line = this.RichText.GetLineFromCharIndex(this.RichText.GetFirstCharIndexOfCurrentLine());
            
            if (_lastInputChar == '\r' || _lastInputChar == '\n')
            {
                _markedLines.RemoveWhere(p => p == line || p == line - 1);
                SetLineNo(true);
            }
            else if (_lastInputChar == '\b')
            {
                _markedLines.RemoveWhere(p => p == line);
                SetLineNo();
            }
            else
            {
                _markedLines.RemoveWhere(p => p == line);
            }
            //if (_lastGrammarAnalysisResult != null)
            //{
            //    foreach (var g in _lastGrammarAnalysisResult?.AnnotationInfos.Where(p => p.StartLine <= line && p.EndLine >= line))
            //    {
            //        _markedLines.RemoveWhere(p => p >= g.StartLine && p <= g.EndLine);
            //    }
            //    foreach (var g in _lastGrammarAnalysisResult?.StringInfos.Where(p => p.StartLine <= line && p.EndLine >= line))
            //    {
            //        _markedLines.RemoveWhere(p => p >= g.StartLine && p <= g.EndLine);
            //    }
            //}
            _lastMarketedLines = -1;
            _lastInputChar='\0';

            ClearMarkedLine(new[] { line });

            MarkKeyWords(false);
        }

        void SetLineNo(bool addNewLine = false)
        {
            ProcessTraceUtil.Trace("StartSetLineNo");
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
            var lineHeight = 0;
            var linesIsEmpty = RichText.Lines.Skip(line1 - 1).Take(line2 - line1 + 1).Select(p => p.FirstOrDefault() == default(char)).ToArray();
            for (int i = line1; i <= line2 && i <= linesLen; i++)
            {
                //要算上一个换行符
                var isEmpty = linesIsEmpty[i - line1];
                if (isEmpty)
                {
                    if (i == line1)
                    {
                        continue;
                    }
                    Point p = new Point(2, 0);
                    p.Y = offset + (offset == 0 ? 0 : this.Font.Height) + 1;
                    offset = p.Y;
                    nos.Add(i, p);
                    strLen += 1;
                }
                else
                {
                    Point p;
                    if (lineHeight == 0)
                    {
                        p = this.RichText.GetPositionFromCharIndex(strLen);

                        if (nos.Count > 0)
                        {
                            lineHeight = p.Y - offset;
                        }

                        strLen += RichText.Lines[i - 1].Length + 1;

                    }
                    else
                    {
                        p = new Point(2, offset + lineHeight);
                    }

                    offset = p.Y;
                    p.X = 2;

                    nos.Add(i, p);
                }
            }
            ProcessTraceUtil.Trace("nos");

            this.ScaleNos.LineNos = nos;

            ProcessTraceUtil.Trace("SetLineNo");
        }

        int LastScrollVPos = 0;

        void RichText_VScroll(object sender, EventArgs e)
        {
            var isDown = true;
            var vpos = RichText.VerticalPosition;
            isDown = vpos > LastScrollVPos;
            LastScrollVPos = vpos;
            //if (RichText.SelectionLength == 0)
            //{
            //    var currline = RichText.GetLineFromCharIndex(RichText.SelectionStart);

            //    if (currline > CurrentClientScreentEndLine && currline + 1 != RichText.Lines.Length)
            //    {
            //        RichText.SelectionStart = RichText.GetFirstCharIndexFromLine(CurrentClientScreentEndLine);
            //    }
            //    else if (currline < CurrentClientScreenStartLine)
            //    {
            //        RichText.SelectionStart = RichText.GetFirstCharIndexFromLine(CurrentClientScreenStartLine);
            //    }
            //}
            _timer.ReSetTimeOutCallBack(() =>
                {
                    this.Invoke(new Action<bool, bool>(MarkKeyWords), true, isDown);
                    this.Invoke(new Action(() =>
                    {

                    }));
                });
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
        /// 注释信息
        /// </summary>
        /// <returns></returns>
        private GrammarAnalysisResult GrammarAnalysis()
        {
            var result = new GrammarAnalysisResult
            {
                AnnotationInfos=new List<GrammarInfo>(),
                StringInfos=new List<GrammarInfo>()
            };
            var texts = RichText.Lines;
            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            var len = 0;
            int strPos = -1, strStartLine = -1;
            for (var l = 0; l < texts.Length; l++)
            {
                var text = texts[l];
                var textLen = 0;
                if (text != null)
                {
                    textLen = text.Length;
                    for (var i = 0; i < text.Length; i++)
                    {
                        if (strPos == -1 && i > 0 && text[i] == '*' && text[i - 1] == '/')
                        {
                            if (stack.Count == 0)
                            {
                                len = -(i - 1);
                            }
                            stack.Push(new Tuple<int, int>(i - 1, l));
                        }
                        else if (strPos == -1 && i > 0 && text[i] == '/' && text[i - 1] == '*')
                        {
                            if (stack.Count > 1)
                            {
                                stack.Pop();
                            }
                            else if (stack.Count == 1)
                            {
                                var item = stack.Pop();
                                len += i + 1;
                                result.AnnotationInfos.Add(new GrammarInfo
                                {
                                    Start = item.Item1,
                                    StartLine = item.Item2,
                                    End = i,
                                    EndLine = l,
                                    Len = len
                                });
                            }
                        }
                        else if (stack.Count == 0 && text[i] == '\'')
                        {
                            if (strPos == -1)
                            {
                                strPos = i;
                                strStartLine = l;
                                len = -i;
                            }
                            else if (i == textLen - 1 || text[i + 1] != '\'')
                            {
                                len += i + 1;
                                result.StringInfos.Add(new GrammarInfo
                                {
                                    Start = strPos,
                                    StartLine = strStartLine,
                                    End = i,
                                    EndLine = l,
                                    Len = len
                                });
                                strPos = -1;
                            }
                            else if (text[i + 1] == '\'')
                            {
                                i++;
                            }
                        }
                        else if (stack.Count == 0 && strPos == -1 && text[i] == '-' && textLen - 1 > i && text[i + 1] == '-')
                        {
                            result.AnnotationInfos.Add(new GrammarInfo
                            {
                                Start = i,
                                StartLine = l,
                                End = textLen - 1,
                                EndLine = l,
                                Len = textLen - i
                            });
                            break;
                        }
                    }
                }
                if (stack.Count > 0
                    || strPos > -1)
                {
                    len += textLen + 1;//加上换行符号
                }
            }

            if (stack.Count > 0)
            {
                var item = stack.First();
                result.AnnotationInfos.Add(new GrammarInfo
                {
                    Start = item.Item1,
                    StartLine = item.Item2,
                    End = texts.Last().Length,
                    EndLine = texts.Length - 1,
                    Len = len
                });
            }
            else if (strPos > -1)
            {
                result.StringInfos.Add(new GrammarInfo
                {
                    Start = strPos,
                    StartLine = strStartLine,
                    End = texts.Last().Length,
                    EndLine = texts.Length - 1,
                    Len = len
                });
            }

            return result;
        }

        /// <summary>
        /// 分解过程
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public void MarkKeyWords(bool reSetLineNo, bool isDown = true)
        {
            ProcessTraceUtil.StartTrace();

            if (reSetLineNo)
                SetLineNo();

            int oldVPos = RichText.VerticalPosition;
            int oldHPos = RichText.HorizontalPosition;
            try
            {
                this.RichText.SelectionChanged -= RichText_SelectionChanged;
                if (this.RichText.Lines.Length == 0)
                    return;
                int line1 = CurrentClientScreenStartLine;
                if (_lastMarketedLines == line1)
                    return;

                int line2 = CurrentClientScreentEndLine + 1;

                if (!isDown)
                {
                    line1 = Math.Max(0, line1 - (int)((line2 - line1) * 0.5));
                }
                else
                {
                    line2 = (int)(line2 * 1.5);
                }
                //if (line2 == 1)
                //{
                //    return;
                //}

                int oldStart = this.RichText.SelectionStart;
                int oldSelectLen = this.RichText.SelectionLength;

                ProcessTraceUtil.Trace("totalIndex:");
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

                var grammarAnalysisResult = new Lazy<GrammarAnalysisResult>(() => GrammarAnalysis());
                if (_lastGrammarAnalysisResult != null)
                {
                    var lines = new List<int>();
                    //B有A没有，清除B
                    //A有B没有，清除A
                    var exstringInfos = _lastGrammarAnalysisResult.StringInfos.Except(grammarAnalysisResult.Value.StringInfos, new GrammarInfoComparer()).ToList();
                    if (exstringInfos.Any())
                    {

                        foreach (var info in exstringInfos)
                        {
                            for (var i = info.StartLine; i <= info.EndLine; i++)
                            {
                                lines.Add(i);
                                _markedLines.Remove(i);
                            }
                        }
                    }
                    exstringInfos = _lastGrammarAnalysisResult.AnnotationInfos.Except(grammarAnalysisResult.Value.AnnotationInfos, new GrammarInfoComparer()).ToList();
                    if (exstringInfos.Any())
                    {

                        foreach (var info in exstringInfos)
                        {
                            for (var i = info.StartLine; i <= info.EndLine; i++)
                            {
                                lines.Add(i);
                                _markedLines.Remove(i);
                            }
                        }
                    }
                    ClearMarkedLine(lines.Distinct().ToArray());
                }
                _lastGrammarAnalysisResult = grammarAnalysisResult.Value;

                var annotationInfos = new Lazy<List<GrammarInfo>>(() => grammarAnalysisResult.Value.AnnotationInfos.Where(p => p.EndLine >= line1 && p.StartLine <= line2).ToList());
                var stringInfos= new Lazy<List<GrammarInfo>>(() => grammarAnalysisResult.Value.StringInfos.Where(p => p.EndLine >= line1 && p.StartLine <= line2).ToList());

                foreach (var a in annotationInfos.Value)
                {

                    for (var l = a.StartLine; l <= a.EndLine; l++)
                    {
                        if (!_markedLines.Contains(l))
                        {
                            var totalIndex = RichText.GetFirstCharIndexFromLine(a.StartLine);
                            DataRow row = tb.NewRow();
                            row[0] = totalIndex + a.Start;
                            row[1] = a.Len;
                            row[2] = Color.Gray;
                            tb.Rows.Add(row);

                            if (a.Start == 0)
                            {
                                _markedLines.Add(a.StartLine);
                            }

                            if (a.End == RichText.Lines[a.EndLine].Length)
                            {
                                _markedLines.Add(a.EndLine);
                            }

                            for (var k = a.StartLine + 1; k <= a.EndLine - 1; k++)
                            {
                                _markedLines.Add(k);
                            }

                            break;
                        }
                    }
                }

                foreach (var a in stringInfos.Value)
                {
                    for (var l = a.StartLine; l <= a.EndLine; l++)
                    {
                        if (!_markedLines.Contains(l))
                        {
                            var totalIndex = RichText.GetFirstCharIndexFromLine(a.StartLine);
                            DataRow row = tb.NewRow();
                            row[0] = totalIndex + a.Start;
                            row[1] = a.Len;
                            row[2] = Color.DarkRed;
                            tb.Rows.Add(row);

                            if (a.Start == 0)
                            {
                                _markedLines.Add(a.StartLine);
                            }

                            if (a.End == RichText.Lines[a.EndLine].Length)
                            {
                                _markedLines.Add(a.EndLine);
                            }

                            for (var k = a.StartLine + 1; k <= a.EndLine - 1; k++)
                            {
                                _markedLines.Add(k);
                            }

                            break;
                        }
                    }
                }

                var linesLen = this.RichText.Lines.Length;
                for (int l = line1; l <= line2 && l < linesLen; l++)
                {
                    if (!_markedLines.Contains(l))
                    {
                        _markedLines.Add(l);

                        var totalIndex = RichText.GetFirstCharIndexFromLine(l);
                        string express = RichText.Lines[l] + " ";

                        foreach (var m in this.KeyWords.MatchKeyWord(express.ToLower()))
                        {
                            if (annotationInfos.Value.Any(p =>p.Contains(l,m.PostionStart)))
                            {
                                continue;
                            }
                            if (stringInfos.Value.Any(p =>p.Contains(l,m.PostionStart)))
                            {
                                continue;
                            }
                            if ((m.PostionStart == 0 || "[]{},|%#!<>=();+-*/\r\n 　".IndexOf(express[m.PostionStart - 1]) > -1)
                                && (m.PostionEnd == express.Length - 1 || "[]{},|%#!<>=();+-*/\r\n 　".IndexOf(express[m.PostionEnd + 1]) > -1))
                            {
                                DataRow row = tb.NewRow();
                                row[0] = totalIndex + m.PostionStart;
                                row[1] = m.KeyWordMatched.Length;
                                row[2] = m.Tag;
                                tb.Rows.Add(row);
                            }
                        }

                        ProcessTraceUtil.Trace("MatchKeyWord:" + l);
                    }
                }

                this.RichText.LockPaint = true;
                foreach (DataRow row in tb.Rows)
                {
                    this.RichText.SelectionStart = (int)row[0];
                    this.RichText.SelectionLength = (int)row[1];
                    this.RichText.SelectionColor = (Color)row[2];
                }

                ProcessTraceUtil.Trace("setColor:" + tb.Rows.Count);
                if (this.RichText.SelectionStart != oldStart)
                {
                    this.RichText.SelectionStart = oldStart;
                }
                if (this.RichText.SelectionLength != oldSelectLen)
                {
                    this.RichText.SelectionLength = oldSelectLen;
                }

                this.RichText.HorizontalPosition = oldHPos;
                this.RichText.VerticalPosition = oldVPos;
                RichText.LockPaint = false;
                _lastMarketedLines = line1;

                ProcessTraceUtil.Trace("resetPostion");

            }
            finally
            {

                this.RichText.SelectionChanged += RichText_SelectionChanged;

                LogHelper.Instance.Debug(ProcessTraceUtil.PrintTrace(100));
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
            this.剪切ToolStripMenuItem.Enabled = this.SelectionLength > 0;
        }

        private void 搜索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubForm.FindDlg dlg = new SubForm.FindDlg();

            dlg.FindLast += (s, i) =>
            {
                var pos = this.RichText.Find(s, 0, i, RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight);
                if (pos != -1)
                {
                    this.RichText.Select(pos, s.Length);
                    this.RichText.ScrollToCaret();
                    this.RichText.Focus();
                    return pos;
                }
                return 0;
            };
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

            dlg.ShowMe(Util.FindParent<TabPage>(this));
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
                if (BigEntityTableRemotingEngine.Insert("SqlSave", new SqlSaveEntity
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

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SelectionLength > 0)
            {
                Clipboard.SetData(DataFormats.Rtf,this.RichText.SelectedRtf);
                this.RichText.SelectedRtf = string.Empty;
            }
        }

        private void 撤消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.RichText.CanUndo)
            {
                this.RichText.Undo();
            }
        }

        private void 重做ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.RichText.CanRedo)
            {
                this.RichText.Redo();
            }
        }

        private void TSMI_SaveAsFile_Click(object sender, EventArgs e)
        {
            var sql = this.RichText.Text;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }

            var nameinput = new SubForm.InputStringDlg("导出文件名");
            if (nameinput.ShowDialog() == DialogResult.OK)
            {
                var dir = Application.StartupPath + "\\temp\\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var filename = $"{dir}\\{nameinput.InputString}.sql";
                using (StreamWriter fs = new StreamWriter(filename, false, Encoding.UTF8))
                {

                    var str = sql;
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        fs.Write(str);
                    }
                }
                Util.SendMsg(this, $"文件已保存:{filename}");
                System.Diagnostics.Process.Start("explorer.exe", dir);
            }
        }
    }
}
