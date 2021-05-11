using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Biz.Common.Data;
using Entity;

namespace NETDBHelper.UC
{
    public partial class UCAddTableByEntity : TabPage//UserControl// 
    {
        private DBSource DB
        {
            get;
            set;
        }
        private string DBName
        {
            get;
            set;
        }
        public Action<DBSource, string> OnNewTableAdd;
        public UCAddTableByEntity(DBSource db,string dbName)
            : base()
        {
            InitializeComponent();
            //etbCode.KeyWords.Clear();
            //etbSQL.KeyWords.Clear();
            etbSQL.KeyWords.AddKeyWord("use", Color.Blue);
            etbSQL.KeyWords.AddKeyWord("create", Color.Blue);
            etbSQL.KeyWords.AddKeyWord("table", Color.Gray);
            etbSQL.KeyWords.AddKeyWord("EXEC", Color.Red);
            etbSQL.KeyWords.AddKeyWord("SET", Color.Blue);
            etbSQL.KeyWords.AddKeyWord("go", Color.Blue);
            this.DB = db;
            this.DBName = dbName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(etbCode.Text))
            {
                this.labErr.Text = "请在上面方框内粘入实体代码！";
                return;
            }
            try
            {
                DataTable tb= DataHelper.GetEntityFieldTable(etbCode.Text).FirstOrDefault();
                if (tb != null)
                {
                    var inputdlg = new SubForm.InputStringDlg("备注表说明");
                    if (inputdlg.ShowDialog() != DialogResult.OK)
                    {
                        Util.SendMsg(this, "必须要填写表备注");
                        return;
                    }
                    string s = DataHelper.GetCreateTableSQL(this.DBName, inputdlg.InputString, tb);
                    this.etbSQL.Text = s;
                    this.etbSQL.MarkKeyWords(true);
                    Biz.Common.Data.MySQLHelper.ExecuteNoQuery(this.DB, this.DBName, s, null);
                    this.labErr.Text= "执行成功";
                    if (OnNewTableAdd != null)
                    {
                        OnNewTableAdd(this.DB, this.DBName);
                    }
                }
            }
            catch (DBHelperException ex)
            {
                this.labErr.Text ="编绎错误："+ex.Message;
            }
            catch (Exception ex)
            {
                this.labErr.Text = ex.Message;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SubForm.WinWebBroswer web = new SubForm.WinWebBroswer();

            string example = @"
<span style='color:red;'>using System;</span>
<span style='color:red;'>//引用Entity，设置主键什么的</span>
<span style='color:red;'>using Entity;</span>
namespace Nonamespace
{
    public class NewsEntity
    {
        private int _id;
        <span style='color:blue;'>//设置自增长键和主键</span>
        <span style='color:blue;'>[DataBaseMapperAttr(IsKey=true,IsIdentity=true,Nullable=false,Description=" + "\"自增主键\"" + @")]</span>
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id=value;
            }
        }

        private DateTime _cdate;
        public DateTime Cdate
        {
            get
            {
                return _cdate;
            }
            set
            {
                _cdate=value;
            }
        }
        private DateTime _mdate;
        public DateTime Mdate
        {
            get
            {
                return _mdate;
            }
            set
            {
                _mdate=value;
            }
        }
        private string _title;
        <span style='color:blue;'>[DataBaseMapperAttr(Len=30)]</span>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title=value;
            }
        }
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content=value;
            }
        }
        private string _class;
        public string Class
        {
            get
            {
                return _class;
            }
            set
            {
                _class=value;
            }
        }
        private string _source;
        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source=value;
            }
        }
        private string _formurl;
        public string Formurl
        {
            get
            {
                return _formurl;
            }
            set
            {
                _formurl=value;
            }
        }
        private string _keywords;

        public string Keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                _keywords=value;
            }
        }
        private DateTime _newsDate;

        public DateTime NewsDate
        {
            get
            {
                return _newsDate;
            }
            set
            {
                _newsDate=value;
            }
        }
        private long _isImp;
        <span style='color:blue;'>[DataBaseMapperAttr(Len=0)]</span>
        public long IsImp
        {
            get
            {
                return _isImp;
            }
            set
            {
                _isImp=value;
            }
        }
        private bool _isvalid;
        public bool Isvalid
        {
            get
            {
                return _isvalid;
            }
            set
            {
                _isvalid=value;
            }
        }
        private string _conkeywords;

        public string Conkeywords
        {
            get
            {
                return _conkeywords;
            }
            set
            {
                _conkeywords=value;
            }
        }
        private bool _isList;

        public bool IsList
        {
            get
            {
                return _isList;
            }
            set
            {
                _isList=value;
            }
        }
        private bool _isRead;

        public bool IsRead
        {
            get
            {
                return _isRead;
            }
            set
            {
                _isRead=value;
            }
        }
        private bool _isReqest;

        public bool IsReqest
        {
            get
            {
                return _isReqest;
            }
            set
            {
                _isReqest=value;
            }
        }
        private string _newsWriter;

        public string NewsWriter
        {
            get
            {
                return _newsWriter;
            }
            set
            {
                _newsWriter=value;
            }
        }
        private string _path;

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path=value;
            }
        }
        private int _power;

        public int Power
        {
            get
            {
                return _power;
            }
            set
            {
                _power=value;
            }
        }
        private int _clicktime;

        public int Clicktime
        {
            get
            {
                return _clicktime;
            }
            set
            {
                _clicktime=value;
            }
        }
        private bool _isHtmlMaked;

        public bool IsHtmlMaked
        {
            get
            {
                return _isHtmlMaked;
            }
            set
            {
                _isHtmlMaked=value;
            }
        }
        private bool _isNewsKeysCollected;

        public bool IsNewsKeysCollected
        {
            get
            {
                return _isNewsKeysCollected;
            }
            set
            {
                _isNewsKeysCollected=value;
            }
        }
    }
}

";

            web.SetHtml(string.Format("<html><head><title>{0}</title></head><body><pre>{1}</pre></body></html>", "根据模型生成表样例",
                example));

            web.ShowDialog();
        }
    }
}
