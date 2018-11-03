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
            MenuItem_FixColumn.Click += MenuItem_FixColumn_Click;

            this.dv_Data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            
            dv_Data.CellContextMenuStripNeeded += dv_Data_CellContextMenuStripNeeded;
        }

        void dv_Data_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                var columname = dv_Data.Columns[e.ColumnIndex].Name;
                if (dv_Data.Columns[e.ColumnIndex].Frozen)
                {
                    MenuItem_FixColumn.Text = "解锁列[" + columname + "]";
                }
                else
                {
                    MenuItem_FixColumn.Text = "锁定列[" + columname + "]";
                }
                MenuItem_FixColumn.Tag = e.ColumnIndex;
                MenuItem_FixColumn.Enabled = e.ColumnIndex < 3;
                MenuItem_FixColumn.Visible = true;
            }
            else
            {
                MenuItem_FixColumn.Visible = false;
                
            }
        }

        void MenuItem_FixColumn_Click(object sender, EventArgs e)
        {
            var tag = MenuItem_FixColumn.Tag;
            if (tag == null)
            {
                return;
            }
            var columnindex=(int)tag;
            dv_Data.Columns[columnindex].Frozen = !dv_Data.Columns[columnindex].Frozen;

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
            get;
            set;
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
                        Biz.Common.Data.OracleHelper.ExecuteNoQuery(DBSource, DBName, value);
                    }
                    else
                    {
                        var tb=Biz.Common.Data.OracleHelper.ExecuteDBTable(DBSource, DBName, value);
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
            this.SQLString = this.tb_sql.Text;
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
            var cols = Biz.Common.Data.OracleHelper.GetColumns(this.DBSource, this.DBName, this.TBName).Where(p=>p.IsKey).ToList();
            if (cols.Count == 0)
            {
                MessageBox.Show("删除失败，没有主键");
                return;
            }
            
            foreach (DataGridViewRow selRow in dv_Data.SelectedRows)
            {
                List<KeyValuePair<string,object>> kvs=new List<KeyValuePair<string,object>>();
                foreach (var col in cols)
                {
                    kvs.Add(new KeyValuePair<string,object>(col.Name,selRow.Cells[col.Name].Value));
                }
                try
                {
                    Biz.Common.Data.OracleHelper.DeleteItem(DBSource, DBName, TBName, kvs);
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
