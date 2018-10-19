using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class SQLEditBox : EditTextBox
    {
        public SQLEditBox()
            : base()
        {
            InitializeComponent();
            this.KeyWords.AddKeyWord("use", Color.Blue);
            this.KeyWords.AddKeyWord("table", Color.Blue);
            this.KeyWords.AddKeyWord("column", Color.Blue);
            this.KeyWords.AddKeyWord("SET", Color.Blue);
            this.KeyWords.AddKeyWord("ANSI_NULLS", Color.Pink);
            this.KeyWords.AddKeyWord("CREATE", Color.Blue);
            this.KeyWords.AddKeyWord("@@ERROR", Color.Pink);
            this.KeyWords.AddKeyWord("@@ROWCOUNT", Color.Pink);
            this.KeyWords.AddKeyWord("select", Color.Blue);
            this.KeyWords.AddKeyWord("*", Color.Gray);
            this.KeyWords.AddKeyWord("from", Color.Blue);
            this.KeyWords.AddKeyWord("delete", Color.Blue);
            this.KeyWords.AddKeyWord("where", Color.Blue);
            this.KeyWords.AddKeyWord("distinct", Color.Blue);
            this.KeyWords.AddKeyWord("top", Color.Blue);
            this.KeyWords.AddKeyWord("nolock", Color.Blue);
            this.KeyWords.AddKeyWord("with", Color.Blue);
            this.KeyWords.AddKeyWord("order", Color.Green);
            this.KeyWords.AddKeyWord("by", Color.Green);
            this.KeyWords.AddKeyWord("between", Color.Green);
            this.KeyWords.AddKeyWord("and", Color.Green);
            this.KeyWords.AddKeyWord("or", Color.Green);
            this.KeyWords.AddKeyWord("not", Color.Green);
            this.KeyWords.AddKeyWord("null", Color.Gray);
            this.KeyWords.AddKeyWord("isnull", Color.Red);
            this.KeyWords.AddKeyWord("getdate", Color.Red);
            this.KeyWords.AddKeyWord("cast", Color.Red);
            this.KeyWords.AddKeyWord("as", Color.Blue);
            this.KeyWords.AddKeyWord("convert", Color.Red);
            this.KeyWords.AddKeyWord("case", Color.Blue);
            this.KeyWords.AddKeyWord("when", Color.Blue);
            this.KeyWords.AddKeyWord("then", Color.Blue);
            this.KeyWords.AddKeyWord("else", Color.Blue);
            this.KeyWords.AddKeyWord("end", Color.Blue);
            this.KeyWords.AddKeyWord("if", Color.Blue);
            this.KeyWords.AddKeyWord(",", Color.Green);
            this.KeyWords.AddKeyWord("[", Color.Gray);
            this.KeyWords.AddKeyWord("]", Color.Gray);
        }
    }
}
