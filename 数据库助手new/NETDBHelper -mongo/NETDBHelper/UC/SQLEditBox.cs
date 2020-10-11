using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Biz.Common;

namespace NETDBHelper.UC
{
    public partial class SQLEditBox : EditTextBox
    {
        public SQLEditBox()
            : base()
        {
            InitializeComponent();

            Dictionary<string, Color> dic = new Dictionary<string, Color>();
            dic.Add("use", Color.Blue);
            dic.Add("table", Color.Blue);
            dic.Add("on", Color.Blue);
            dic.Add("column", Color.Blue);
            dic.Add("SET", Color.Blue);
            dic.Add("ANSI_NULLS", Color.Pink);
            dic.Add("CREATE", Color.Blue);
            dic.Add("@@ERROR", Color.Pink);
            dic.Add("@@ROWCOUNT", Color.Pink);
            dic.Add("select", Color.Blue);
            dic.Add("*", Color.Gray);
            dic.Add("from", Color.Blue);
            dic.Add("delete", Color.Blue);
            dic.Add("update", Color.Blue);
            dic.Add("insert", Color.Blue);
            dic.Add("where", Color.Blue);
            dic.Add("distinct", Color.Blue);
            dic.Add("top", Color.Blue);
            dic.Add("nolock", Color.Blue);
            dic.Add("with", Color.Blue);
            dic.Add("like", Color.Blue);
            dic.Add("order", Color.Green);
            dic.Add("by", Color.Green);
            dic.Add("desc", Color.Blue);
            dic.Add("asc", Color.Blue);
            dic.Add("between", Color.Green);
            dic.Add("and", Color.Green);
            dic.Add("or", Color.Green);
            dic.Add("not", Color.Green);
            dic.Add("null", Color.Gray);
            dic.Add("isnull", Color.Red);
            dic.Add("getdate", Color.Red);
            dic.Add("cast", Color.Red);
            dic.Add("as", Color.Blue);
            dic.Add("convert", Color.Red);
            dic.Add("case", Color.Blue);
            dic.Add("when", Color.Blue);
            dic.Add("then", Color.Blue);
            dic.Add("else", Color.Blue);
            dic.Add("end", Color.Blue);
            dic.Add("if", Color.Blue);
            dic.Add("procedure", Color.Blue);
            dic.Add("declare", Color.Blue);
            dic.Add("count", Color.Blue);
            dic.Add(",", Color.Green);
            dic.Add("[", Color.Gray);
            dic.Add("]", Color.Gray);

            foreach (var kv in dic)
            {
                this.KeyWords.AddKeyWord(kv.Key, kv.Value);

                if (kv.Key.Length > 1 && !dic.ContainsKey(kv.Key.ToUpper()))
                {
                    this.KeyWords.AddKeyWord(kv.Key.ToUpper(), kv.Value);
                }
            }

            foreach (var sQLKeyWord in SQLKeyWordHelper.GetKeyWordList())
            {
                if (!dic.ContainsKey(sQLKeyWord.KeyWord.ToLower()))
                {
                    var color = sQLKeyWord.HighColor;
                    if (color == Color.Empty)
                    {
                        color = Color.Blue;
                    }
                    this.KeyWords.AddKeyWord(sQLKeyWord.KeyWord.ToLower(), color);
                    this.KeyWords.AddKeyWord(sQLKeyWord.KeyWord.ToUpper(), color);
                }
            }
        }
    }
}
