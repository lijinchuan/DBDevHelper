using Biz.Common;
using Biz.Common.Data;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class SyncDataWin : Form
    {
        private const string GetTableColumnsMetaDataSQL = @"select [syscolumns].name
                                           ,[systypes].name type
                                           ,[syscolumns].length
                                           ,[syscolumns].isnullable
                                           ,[syscolumns].prec
                                           ,[syscolumns].scale
                                           FROM [syscolumns](nolock)
                                           left join [systypes](nolock)
                                           on [syscolumns].xusertype = [systypes].xusertype
                                           left join [sysobjects](nolock) on [syscolumns].[id]=[sysobjects].id
                                           where [sysobjects].type='U'
                                           and [systypes].name<>'sysname'
                                           and [sysobjects].name=@name";

        private const string GetKeyColumnsSql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE(nolock) WHERE TABLE_NAME=@TABLE_NAME";

        public SyncDataWin()
        {
            InitializeComponent();
        }

        private DBSource DBSource
        {
            get;
            set;
        }

        private string ConnDB
        {
            get;
            set;
        }

        public SyncDataWin(DBSource dbsource,string conndb,string sourcetable)
        {
            InitializeComponent();

            this.DBSource = dbsource;
            this.ConnDB = conndb;
            this.TBSourceConnStr.Text = SQLHelper.GetConnstringFromDBSource(dbsource,conndb);
            this.TBSource.Text = sourcetable;

            this.TBSourceConnStr.ReadOnly = this.TBSource.ReadOnly = true;

            this.CBDestDB.SelectedIndexChanged += CBDestDB_SelectedIndexChanged;

            BtnLoadFields_Click(null, null);

        }

        private void CBDestDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBDestDB.SelectedIndex == -1)
            {
                TBDestConnStr.Text = string.Empty;
            }
            else
            {
                TBDestConnStr.Text = Biz.Common.Data.SQLHelper.GetConnstringFromDBSource((DBSource)CBDestDB.Tag, CBDestDB.SelectedValue.ToString());
            }
        }

        private string GetSourceConnStr
        {
            get
            {
                var val = TBSourceConnStr.Text.Trim();
                if (string.IsNullOrEmpty(val))
                {
                    throw new Exception("请填写源表连接串");
                }

                return val;
            }
        }

        private string GetDestConnStr
        {
            get
            {

                var val = TBDestConnStr.Text.Trim();
                if (string.IsNullOrEmpty(val))
                {
                    throw new Exception("请填写目标表连接串");
                }

                return val;
            }
        }

        private string GetSourceTBName
        {
            get
            {
                var val = TBSource.Text.Trim();
                if (string.IsNullOrEmpty(val))
                {
                    throw new Exception("请填写源表名称");
                }

                return val;
            }
        }

        private string GetDestTBName
        {
            get
            {

                var val = TBDest.Text.Trim();
                if (string.IsNullOrEmpty(val))
                {
                    throw new Exception("请填写目标表名称");
                }

                return val;
            }
        }

        private string GetKeyField
        {
            get
            {
                if (CBKey.SelectedIndex == -1)
                {
                    throw new Exception("请选择主键");
                }

                return CBKey.SelectedItem.ToString();
            }
        }

        private TBColumn GetByCol
        {
            get
            {
                
                if (CBByCol.SelectedIndex == -1)
                {
                    throw new Exception("请选择主键");
                }

                return SQLHelper.GetColumns(DBSource, ConnDB, GetSourceTBName, null).AsEnumerable().First(p => p.Name == CBByCol.SelectedItem.ToString());
            }
        }

        private string[] GetDestFields
        {
            get
            {
                List<string> fields = new List<string>();
                foreach (var item in CBFields.CheckedItems)
                {
                    fields.Add(item.ToString());
                }

                if (!fields.Contains(GetKeyField))
                {
                    fields.Add(GetKeyField);
                    for(int i = 0; i < CBFields.Items.Count; i++)
                    {
                        if (CBFields.Items[i].ToString().Equals(GetKeyField))
                        {
                            CBFields.SetItemChecked(i, true);
                            break;
                        }
                    }
                }

                if (fields.Count == 0)
                {
                    throw new Exception("请选择同步字段");
                }

                return fields.ToArray();
            }
        }


        private void BtnSync_Click(object sender, EventArgs e)
        {
            string sourceconnstr = GetSourceConnStr;
            string destconnstr = GetDestConnStr;
            string soucretable = GetSourceTBName;
            string[] fields = GetDestFields;
            string keyfield = GetKeyField;
            //prefix = "";

            string desttable = GetDestTBName;

            //DataSyncHelper.Clear(sourceconnstr, soucretable, destconnstr,desttable,redisconnstr);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetSourceConnStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = GetKeyColumnsSql;
                        cmd.Parameters.Add(new SqlParameter("@TABLE_NAME", GetSourceTBName));
                        using (var reader = cmd.ExecuteReader())
                        {
                            int readcont = 0;
                            while (reader.Read())
                            {
                                var colname = (string)reader.GetValue(0);
                                if (colname != GetKeyField)
                                {
                                    throw new Exception($"{GetKeyField}不是主键");
                                }
                                readcont++;
                            }
                            if (readcont == 0)
                            {
                                throw new Exception($"{GetKeyField}不是主键");
                            }
                        }
                    }
                }

                var col = GetByCol;

                if (col.TypeName.IndexOf("timestamp", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var boo = DataSyncHelper.SyncSQLDBDataByTimeStamp(sourceconnstr, soucretable, fields, col.Name
                       , keyfield, destconnstr, desttable, 0);
                }
                else if (col.TypeName.IndexOf("datetime", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var boo = DataSyncHelper.SyncSQLDBDataByTime(sourceconnstr, soucretable, fields, col.Name
                       , keyfield, destconnstr, desttable, 0);
                }
                else if (col.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var boo = DataSyncHelper.SyncSQLDBDataByNumberCol(sourceconnstr, soucretable, fields, col.Name
                       , keyfield, destconnstr, desttable, 0);
                }
                else
                {
                    var boo = DataSyncHelper.SyncSQLDBDataByStrCol(sourceconnstr, soucretable, fields, col.Name
                       , keyfield, destconnstr, desttable, 0);
                }

                MessageBox.Show("同步成功");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                MessageBox.Show("出错:" + ex.Message);
            }

            sw.Stop();
        }

        private void BtnLoadFields_Click(object sender, EventArgs e)
        {
            try
            {
                var cols = SQLHelper.GetColumns(DBSource, ConnDB, GetSourceTBName, null);

                this.CBFields.Items.Clear();
                CBKey.Items.Clear();
                foreach (TBColumn col in cols)
                {
                    this.CBFields.Items.Add(col.Name);
                    this.CBKey.Items.Add(col.Name);
                    this.CBByCol.Items.Add(col.Name);
                }

                var idcol = cols.First(p => p.IsID);
                if (idcol != null)
                {
                    CBKey.SelectedItem = idcol.Name;
                }
                else
                {
                    var keycols = cols.Where(p => p.IsKey).ToArray();
                    if (keycols.Length == 1)
                    {
                        if (keycols != null)
                        {
                            CBKey.SelectedItem = keycols[0].Name;
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCheckDest_Click(object sender, EventArgs e)
        {
            try
            {
                var fields = GetDestFields;
                StringBuilder sb = new StringBuilder($"if object_id('{GetDestTBName}') is null");
                sb.AppendLine();
                sb.AppendLine("begin");
                sb.AppendLine($"CREATE TABLE [{GetDestTBName}](");
                using (SqlConnection destconn = new SqlConnection(GetDestConnStr))
                {
                    bool exist = false;
                    destconn.Open();
                    using (var cmd0 = destconn.CreateCommand())
                    {
                        cmd0.CommandType = CommandType.Text;

                        cmd0.CommandText = $@"if object_id('{GetDestTBName}') is not null
                                           select 0
                                             else 
                                          select 1";

                        exist = cmd0.ExecuteScalar().ToString()=="0";
                    }
                    using (SqlConnection conn = new SqlConnection(GetSourceConnStr))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                           
                            if (!exist)
                            {
                                cmd.CommandText = GetTableColumnsMetaDataSQL; //$"select top 1 {string.Join(",",GetDestFields)} from {GetSourceTBName}";
                                cmd.Parameters.Add(new SqlParameter("@name", GetSourceTBName));

                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                DataTable tb = new DataTable();
                                adp.Fill(tb);

                                for (int i = 0; i < tb.Rows.Count; i++)
                                {
                                    var name = tb.Rows[i]["name"].ToString();
                                    if (!fields.Contains(name))
                                    {
                                        continue;
                                    }

                                    var col = new TBColumn
                                    {
                                        IsKey = name == GetKeyField,
                                        Length = int.Parse(tb.Rows[i]["length"].ToString()),
                                        Name = tb.Rows[i]["name"].ToString(),
                                        TypeName = tb.Rows[i]["type"].ToString(),
                                        IsNullAble = tb.Rows[i]["isnullable"].ToString().Equals("1"),
                                        prec = tb.Rows[i]["prec"].CovertToInt(),
                                        scale = tb.Rows[i]["scale"].CovertToInt()
                                    };

                                    sb.AppendFormat("[{0}] {1} {2} ,", col.Name, col.ToDBType(), (col.IsID || col.IsKey) ? "NOT NULL" : (col.IsNullAble ? "NULL" : "NOT NULL"));
                                    sb.AppendLine();

                                }

                                sb.AppendLine(")");
                                sb.AppendLine($"alter table {GetDestTBName} add constraint PK_{GetDestTBName}_1 primary key({GetKeyField})");
                                sb.AppendLine("end");

                                using (var cmd1 = destconn.CreateCommand())
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.CommandText = sb.ToString();

                                    var ret = cmd1.ExecuteNonQuery();

                                    MessageBox.Show("创建成功");
                                }
                            }
                            else
                            {
                                var sql = $"select top 0 * from {GetDestTBName}";

                                using(var cmd2 = destconn.CreateCommand())
                                {
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = sql;
                                    var table = new DataTable();
                                    new SqlDataAdapter(cmd2).Fill(table);

                                    for(int i = 0; i < CBFields.Items.Count; i++)
                                    {
                                        
                                        CBFields.SetItemChecked(i, table.Columns.Contains(CBFields.Items[i].ToString()));
                                    }
                                    
                                    
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSelectServer_Click(object sender, EventArgs e)
        {
            var connsqlserver = new ConnSQLServer();
            if (connsqlserver.ShowDialog() == DialogResult.OK)
            {
                this.DBSource = connsqlserver.DBSource;

                this.CBDestDB.DataSource = SQLHelper.GetDBs(connsqlserver.DBSource);
                this.CBDestDB.Tag = connsqlserver.DBSource;
                this.CBDestDB.DisplayMember = "name";
                this.CBDestDB.ValueMember = "name";
                this.CBDestDB.SelectedIndex = -1;
            }
        }
    }
}
