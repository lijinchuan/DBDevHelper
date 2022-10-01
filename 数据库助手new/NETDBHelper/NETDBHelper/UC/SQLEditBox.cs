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

            foreach(var sQLKeyWord in SQLKeyWordHelper.GetSqlKeywordList())
            {
                if (!dic.ContainsKey(sQLKeyWord.KeyWord.ToLower()))
                {
                    var color =Color.FromName(sQLKeyWord.HighColor);
                    this.KeyWords.AddKeyWord(sQLKeyWord.KeyWord.ToLower(), color);
                    this.KeyWords.AddKeyWord(sQLKeyWord.KeyWord.ToUpper(), color);
                }
            }
        }
    }
}
