﻿using System;
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
            this.tb_sql.KeyWords.AddKeyWord("select", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("*", Color.Gray);
            this.tb_sql.KeyWords.AddKeyWord("from", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("delete", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("where", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("distinct", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("top", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("nolock", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("with", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("order", Color.Green);
            this.tb_sql.KeyWords.AddKeyWord("by", Color.Green);
            this.tb_sql.KeyWords.AddKeyWord("between", Color.Green);
            this.tb_sql.KeyWords.AddKeyWord("and", Color.Green);
            this.tb_sql.KeyWords.AddKeyWord("or", Color.Green);
            this.tb_sql.KeyWords.AddKeyWord("not", Color.Green);
            this.tb_sql.KeyWords.AddKeyWord("null", Color.Gray);
            this.tb_sql.KeyWords.AddKeyWord("isnull", Color.Red);
            this.tb_sql.KeyWords.AddKeyWord("getdate", Color.Red);
            this.tb_sql.KeyWords.AddKeyWord("cast", Color.Red);
            this.tb_sql.KeyWords.AddKeyWord("as", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("convert", Color.Red);
            this.tb_sql.KeyWords.AddKeyWord("case", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("when", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("then", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("else", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("end", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("if", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("asc", Color.Blue);
            this.tb_sql.KeyWords.AddKeyWord("desc", Color.Blue);

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

            Regex rg = new Regex(@"create\s+(PROCEDURE|Table)\s",RegexOptions.IgnoreCase);
            if (rg.IsMatch(sql))
            {
                return true;
            }

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
                        Biz.Common.Data.MySQLHelper.ExecuteNoQuery(DBSource, DBName, value);
                    }
                    else
                    {
                        var tb=Biz.Common.Data.MySQLHelper.ExecuteDBTable(DBSource, DBName, value);
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

        private void MenuItem_DelItem_Click(object sender, EventArgs e)
        {
            if (this.dv_Data.SelectedRows.Count == 0)
                return;

            if (this.TBName == null)
            {
                MessageBox.Show("删除失败，未接受表名");
                return;
            }

            if (MessageBox.Show("删除记录吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;
            this.tb_Msg.Text = "";
            var tb = Biz.Common.Data.MySQLHelper.GetKeys(this.DBSource, this.DBName, this.TBName);
            if (tb.Rows.Count == 0)
            {
                //MessageBox.Show("查找不到主键，不能删除记录！");
                //return;
                foreach (DataGridViewColumn column in dv_Data.Columns)
                {
                    var row=tb.NewRow();
                    row[0] = column.Name;
                    tb.Rows.Add(row);
                }
            }
            
            foreach (DataGridViewRow selRow in dv_Data.SelectedRows)
            {
                List<KeyValuePair<string,object>> kvs=new List<KeyValuePair<string,object>>();
                foreach (DataRow row in tb.Rows)
                {
                    kvs.Add(new KeyValuePair<string,object>(row[0].ToString(),selRow.Cells[row[0].ToString()].Value));
                }
                try
                {
                    Biz.Common.Data.MySQLHelper.DeleteItem(DBSource, DBName, TBName, kvs);
                }
                catch (Exception ex)
                {
                    this.tb_Msg.Text += ex.Message + "\r\n";
                    this.tabControl1.SelectedTab = tabPage2;
                }
            }
            Execute();
        }
    }
}
