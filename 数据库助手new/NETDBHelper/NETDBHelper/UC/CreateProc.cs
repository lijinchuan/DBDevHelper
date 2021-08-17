using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;

namespace NETDBHelper.UC
{
    public partial class CreateProc : TabPage
    {
        private DBSource dbSource
        {
            get;
            set;
        }

        private string connDB
        {
            get;
            set;
        }

        private string table
        {
            get;
            set;
        }

        private string tableID
        {
            get;
            set;
        }

        public CreateProceEnum proceType
        {
            get;
            set;
        }

        public CreateProc(DBSource dbSource,string connDB,string table,string tableID,CreateProceEnum proceType)
            :base()
        {
            InitializeComponent();
            this.dbSource = dbSource;
            this.connDB = connDB;
            this.editTextBox1.DBName = connDB;
            this.editTextBox1.DBServer = dbSource;
            this.table = table;
            this.tableID = tableID;
            this.proceType = proceType;
        }

        public void Create()
        {
            if (this.proceType == CreateProceEnum.Insert)
            {
                this.Text = "Insert[" + table + "]";
                this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetInsertProcSql(dbSource, connDB, tableID, table);
                this.editTextBox1.MarkKeyWords(true);
                Clipboard.SetText(this.editTextBox1.Text);
                MainFrm.SendMsg(string.Format("sql语句已经复制到剪贴板。"));
            }
            else if (this.proceType == CreateProceEnum.Update)
            {
                this.Text = "Update[" + table + "]";
                this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetUpdateProcSql(dbSource, connDB, tableID, table);
                this.editTextBox1.MarkKeyWords(true);
                Clipboard.SetText(this.editTextBox1.Text);
                MainFrm.SendMsg(string.Format("sql语句已经复制到剪贴板。"));
            }
            else if (this.proceType == CreateProceEnum.Delete)
            {
                this.Text = "Delete[" + table + "]";
                this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetDeleteSql(dbSource, connDB, tableID, table);
                this.editTextBox1.MarkKeyWords(true);
                Clipboard.SetText(this.editTextBox1.Text);
                MainFrm.SendMsg(string.Format("sql语句已经复制到剪贴板。"));
            }
            else if (this.proceType == CreateProceEnum.Upsert)
            {
                this.Text = "Upsert[" + table + "]";
                this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetUpsertProcsql(dbSource, connDB, tableID, table);
                this.editTextBox1.MarkKeyWords(true);
                Clipboard.SetText(this.editTextBox1.Text);
                MainFrm.SendMsg(string.Format("sql语句已经复制到剪贴板。"));
            }
            else if (this.proceType == CreateProceEnum.BatchInsert)
            {
                this.Text = "bulkcopy[" + table + "]";
                this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetBatInsertProcSql(dbSource, connDB, tableID, table);
                this.editTextBox1.MarkKeyWords(true);
                Clipboard.SetText(this.editTextBox1.Text);
                MainFrm.SendMsg(string.Format("sql语句已经复制到剪贴板。"));
            }
            
        }
    }
}
