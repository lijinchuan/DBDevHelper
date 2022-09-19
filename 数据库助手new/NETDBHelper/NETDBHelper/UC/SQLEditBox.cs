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
            dic.Add("set", Color.Blue);
            dic.Add("table", Color.Blue);
            dic.Add("transaction", Color.Blue);
            dic.Add("rollback", Color.Blue);
            dic.Add("return", Color.Blue);
            dic.Add("on", Color.Blue);
            dic.Add("view", Color.Blue);
            dic.Add("ansi_nulls", Color.DeepPink);
            dic.Add("quoted_identifier", Color.DeepPink);
            dic.Add("create", Color.Blue);
            dic.Add("cross apply", Color.Blue);
            dic.Add("outer apply", Color.Blue);
            dic.Add("@@error", Color.DeepPink);
            dic.Add("@@rowcount", Color.DeepPink);
            dic.Add("@@fetch_status", Color.DeepPink);
            
            dic.Add("select", Color.Blue);
            dic.Add("*", Color.Gray);
            dic.Add("from", Color.Blue);
            dic.Add("delete", Color.Blue);
            dic.Add("update", Color.Blue);
            dic.Add("insert", Color.Blue);
            dic.Add("into", Color.Blue);
            dic.Add("values", Color.Blue);
            dic.Add("where", Color.Blue);
            dic.Add("distinct", Color.Blue);
            dic.Add("top", Color.Blue);
            dic.Add("nolock", Color.Blue);
            dic.Add("with", Color.Blue);
            dic.Add("like", Color.Blue);
            dic.Add("order", Color.Blue);
            dic.Add("by", Color.Blue);
            dic.Add("desc", Color.Blue);
            dic.Add("asc", Color.Blue);
            dic.Add("between", Color.Blue);
            dic.Add("and", Color.Blue);
            dic.Add("or", Color.Blue);
            dic.Add("not", Color.Blue);
            dic.Add("null", Color.Gray);
            dic.Add("isnull", Color.Red);
            dic.Add("getdate", Color.Red);
            dic.Add("goto", Color.Blue);
            dic.Add("year", Color.Red);
            dic.Add("month", Color.Red);
            dic.Add("day", Color.Red);
            dic.Add("cast", Color.Red);
            dic.Add("as", Color.Blue);
            dic.Add("convert", Color.Red);
            dic.Add("case", Color.Blue);
            dic.Add("when", Color.Blue);
            dic.Add("then", Color.Blue);
            dic.Add("else", Color.Blue);
            dic.Add("end", Color.Blue);
            dic.Add("if", Color.Blue);
            dic.Add("is", Color.Blue);
            dic.Add("begin", Color.Blue);
            dic.Add("exec", Color.Blue);
            dic.Add("execute", Color.Blue);
            dic.Add("sp_executesql", Color.Blue);
            dic.Add("proc", Color.Blue);
            dic.Add("procedure", Color.Blue);
            dic.Add("declare", Color.Blue);
            dic.Add("while", Color.Blue);
            dic.Add("join", Color.Blue);
            dic.Add("for", Color.Blue);
            dic.Add("in", Color.Blue);
            dic.Add("inner", Color.Blue);
            dic.Add("outer", Color.Blue);
            dic.Add("hash", Color.Blue);
            dic.Add("group", Color.Blue);
            dic.Add("output", Color.Blue);
            dic.Add("option", Color.Blue);
            dic.Add("recompile", Color.Blue);
            dic.Add("commit", Color.Blue);
            dic.Add("nocount", Color.Blue);

            dic.Add("count", Color.Red);
            dic.Add("sum", Color.Red);
            dic.Add("max", Color.Red);
            dic.Add("min", Color.Red);
            dic.Add("avg", Color.Red);
            dic.Add("exists", Color.Red);
            dic.Add("having", Color.Red);
            dic.Add("mid", Color.Red);
            dic.Add(",", Color.Gray);
            dic.Add("[", Color.Gray);
            dic.Add("]", Color.Gray);
            dic.Add("print", Color.Red);
            dic.Add("charindex", Color.Red);
            dic.Add("left", Color.Red);
            dic.Add("right", Color.Red);
            dic.Add("stuff", Color.Red);
            dic.Add("len", Color.Red);
            dic.Add("round", Color.Red);
            dic.Add("difference", Color.Red);
            dic.Add("soundex", Color.Red);
            dic.Add("lower", Color.Red);
            dic.Add("upper", Color.Red);
            dic.Add("ltrim", Color.Red);
            dic.Add("rtrim", Color.Red);
            dic.Add("replace", Color.Red);
            dic.Add("space", Color.Red);
            dic.Add("reverse", Color.Red);
            dic.Add("replicate", Color.Red);
            dic.Add("quotename", Color.Red);
            dic.Add("patindex", Color.Red);
            dic.Add("parsename", Color.Red);
            dic.Add("isdate", Color.Red);
            dic.Add("datename", Color.Red);
            dic.Add("datepart", Color.Red);
            dic.Add("coalesce", Color.Red);
            dic.Add("open", Color.Red);
            dic.Add("fetch", Color.Red);
            dic.Add("close", Color.Red);
            dic.Add("deallocate", Color.Red);

            dic.Add("char", Color.Green);
            dic.Add("nchar", Color.Green);
            dic.Add("varchar", Color.Green);
            dic.Add("nvarchar", Color.Green);
            dic.Add("datetime", Color.Green);
            dic.Add("float", Color.Green);
            dic.Add("text", Color.Green);
            dic.Add("ntext", Color.Green);
            dic.Add("bit", Color.Green);
            dic.Add("binary", Color.Green);
            dic.Add("varbinary", Color.Green);
            dic.Add("int", Color.Green);
            dic.Add("tinyint", Color.Green);
            dic.Add("smallint", Color.Green);
            dic.Add("bigint", Color.Green);
            dic.Add("decimal", Color.Green);
            dic.Add("numeric", Color.Green);
            dic.Add("smallmoney", Color.Green);
            dic.Add("money", Color.Green);
            dic.Add("real", Color.Green);
            dic.Add("datetime2", Color.Green);
            dic.Add("smalldatetime", Color.Green);
            dic.Add("date", Color.Green);
            dic.Add("time", Color.Green);
            dic.Add("datetimeoffset", Color.Green);
            dic.Add("timestamp", Color.Green);
            dic.Add("sql_variant", Color.Green);
            dic.Add("uniqueidentifier", Color.Green);
            dic.Add("xml", Color.Green);
            dic.Add("cursor", Color.Green);

            for(int i = 0; i < 10; i++)
            {
                dic.Add(i.ToString(), Color.Yellow);
            }

            foreach (var kv in dic)
            {

                this.KeyWords.AddKeyWord(kv.Key, kv.Value);

                if (kv.Key.Length>1 && !dic.ContainsKey(kv.Key.ToUpper()))
                {
                    this.KeyWords.AddKeyWord(kv.Key.ToUpper(), kv.Value);
                }
            }

            foreach(var sQLKeyWord in SQLKeyWordHelper.GetKeyWordList())
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
