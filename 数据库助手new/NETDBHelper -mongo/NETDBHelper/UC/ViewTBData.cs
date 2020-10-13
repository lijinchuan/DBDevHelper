using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Text.RegularExpressions;

namespace NETDBHelper.UC
{
    public partial class ViewTBData :TabPage
    {
        public ViewTBData()
        {
            InitializeComponent();
            dv_Data.DataError += dv_Data_DataError;
            dv_Data.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dv_Data.ContextMenuStrip = this.contextMenuStrip1;

            MenuItem_CopyValue.Click += MenuItem1_CopValue_Click;
            MenuItem_CopyColumnName.Click += MenuItem_CopyColumnName_Click;
        }

        void MenuItem_CopyColumnName_Click(object sender, EventArgs e)
        {
            if (dv_Data.CurrentCell != null)
            {
               Clipboard.SetText(dv_Data.CurrentCell.OwningColumn.Name);
            }
        }

        void MenuItem1_CopValue_Click(object sender, EventArgs e)
        {
            var cell=dv_Data.CurrentCell;
            if (cell != null)
            {
                var val = cell.Value == null ? "" : cell.Value.ToString();
                Clipboard.SetText(val);
            }
        }

        void dv_Data_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public DBSource DBSource
        {
            get;
            set;
        }

        public string DBName
        {
            get
            {
                return this.tb_sql.DBName;
            }
            set
            {
                this.tb_sql.DBName = value;
            }
        }

        public string TBName
        {
            get;
            set;
        }

        private bool IsNoTable(string sql)
        {
            return false;
        }

        public string SQLString
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(tb_sql.Text))
                {
                    this.tb_sql.Text = value;
                }
                tb_sql.MarkKeyWords(true);

                //string rgstr = @"select[\s\r\n]{1,}[\s\w\*\,\.\[\]]+[\s\r\n]{1,}from[\s\r\n]{1,}([\w]+)";
                //Regex rg = new Regex(rgstr);
                //Match m=rg.Match(value);
                //if (!m.Success)
                //{
                //    tb_Msg.Text = value + " 语法错误！";
                //    this.tabControl1.SelectedTab = tabPage2;
                //    return;
                //}
                try
                {
                    //this.Text = "数据查询[" + DBName + "]";
                    DateTime now = DateTime.Now;
                    if (IsNoTable(value))
                    {
                        Biz.Common.Data.MongoDBHelper.ExecuteNoQuery(DBSource, DBName, value);
                    }
                    else
                    {
                        var tb=Biz.Common.Data.MongoDBHelper.ExecuteDBTable(DBSource, DBName, value);
                        this.dv_Data.DataSource = tb;
                        foreach(DataGridViewColumn col in this.dv_Data.Columns){
                            if (col.ValueType == typeof(DateTime))
                            {
                                col.DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            }
                        }
                    }

                    this.tb_Msg.Text = "执行完成:" + DateTime.Now.Subtract(now).TotalMilliseconds.ToString("f4")+"ms";
                    this.tabControl1.SelectedTab = tabPage1;
                }
                catch (Exception e)
                {
                    this.tb_Msg.Text = e.Message;
                    this.tabControl1.SelectedTab = tabPage2;
                }
            }
        }

        public void Execute()
        {
            if (string.IsNullOrEmpty(this.tb_sql.SelectedText))
            {
                this.SQLString = this.tb_sql.Text;
            }
            else
            {
                this.SQLString = this.tb_sql.SelectedText;
            }
        }
    }
}
