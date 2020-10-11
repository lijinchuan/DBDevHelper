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
            this.Text = string.Format("{0}-{1}",table,proceType);
            this.dbSource = dbSource;
            this.connDB = connDB;
            this.editTextBox1.DBName = connDB;
            this.table = table;
            this.tableID = tableID;
            this.proceType = proceType;
        }

        public void Create()
        {
            try
            {
                switch (proceType)
                {
                    case CreateProceEnum.InsertOrUpdate:
                        {
                            this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetInsertProcSql(dbSource, connDB, tableID, table);
                            this.editTextBox1.MarkKeyWords(true);
                            break;
                        }
                    case CreateProceEnum.Delete:
                        {
                            this.editTextBox1.Text = Biz.Common.Data.DataHelper.GetDeleteProcSql(dbSource, connDB, tableID, table);
                            this.editTextBox1.MarkKeyWords(true);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
