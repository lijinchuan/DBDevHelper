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
    public partial class EntityCodePanel : TabPage
    {
        public string EntityBody
        {
            get
            {
                return this.TBCode.Text;
            }
            set
            {
                this.TBCode.Text = value;
                //TBCode.MarkKeyWords();
            }
        }
        public EntityCodePanel()
        {
            InitializeComponent();

            TBCode.KeyWords.AddKeyWord("public", Color.Blue);
            TBCode.KeyWords.AddKeyWord("static", Color.Blue);
            TBCode.KeyWords.AddKeyWord("const", Color.Blue);
            TBCode.KeyWords.AddKeyWord("class", Color.Blue);
            TBCode.KeyWords.AddKeyWord("namespace", Color.Blue);
            TBCode.KeyWords.AddKeyWord("private", Color.Blue);
            TBCode.KeyWords.AddKeyWord("internel", Color.Blue);
            TBCode.KeyWords.AddKeyWord("get", Color.Blue);
            TBCode.KeyWords.AddKeyWord("set", Color.Blue);
            TBCode.KeyWords.AddKeyWord("return", Color.Blue);
            TBCode.KeyWords.AddKeyWord("datetime", Color.Green);
            TBCode.KeyWords.AddKeyWord("decimal", Color.Green);
            TBCode.KeyWords.AddKeyWord("string", Color.Green);
            TBCode.KeyWords.AddKeyWord("float", Color.Green);
            TBCode.KeyWords.AddKeyWord("bool", Color.Green);
            TBCode.KeyWords.AddKeyWord("int", Color.Green);
            TBCode.KeyWords.AddKeyWord("Int16", Color.Green);
            TBCode.KeyWords.AddKeyWord("Int32", Color.Green);
            TBCode.KeyWords.AddKeyWord("Int64", Color.Green);
            TBCode.KeyWords.AddKeyWord("long", Color.Green);
            TBCode.KeyWords.AddKeyWord("DateTime", Color.Green);
            TBCode.KeyWords.AddKeyWord("value", Color.Blue);
            TBCode.KeyWords.AddKeyWord("throw", Color.Blue);
            TBCode.KeyWords.AddKeyWord("new", Color.Blue);
            TBCode.KeyWords.AddKeyWord("Display", Color.Red);
            TBCode.KeyWords.AddKeyWord("ProtoContract", Color.Red);
            TBCode.KeyWords.AddKeyWord("DataBaseMapperAttr", Color.Red);
            TBCode.KeyWords.AddKeyWord("ProtoMember", Color.Red);
            TBCode.KeyWords.AddKeyWord("JsonProperty", Color.Red);
            TBCode.KeyWords.AddKeyWord("PropertyDescriptionAttr", Color.Red);
        }
    }
}
