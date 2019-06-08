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

        public SyncDataWin(string sourcestr,string sourcetable)
        {
            InitializeComponent();

            this.TBSourceConnStr.Text = sourcestr;
            this.TBSource.Text = sourcetable;

            this.TBSourceConnStr.ReadOnly = this.TBSource.ReadOnly = true;

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

                var boo = DataSyncHelper.SyncSQLDBData(sourceconnstr, soucretable, fields
                   , keyfield, destconnstr, desttable, 0);

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

                using (SqlConnection conn = new SqlConnection(GetSourceConnStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"select top 0 * from {GetSourceTBName}";

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adp.Fill(table);

                        this.CBFields.Items.Clear();
                        CBKey.Items.Clear();
                        foreach (DataColumn col in table.Columns)
                        {
                            this.CBFields.Items.Add(col.ColumnName);
                            this.CBKey.Items.Add(col.ColumnName);
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
                    using (SqlConnection conn = new SqlConnection(GetSourceConnStr))
                    {
                        using (var cmd = conn.CreateCommand())
                        {

                            cmd.CommandType = CommandType.Text;
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
                        }

                        using (var cmd = destconn.CreateCommand())
                        {
                            destconn.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sb.ToString();

                            var ret = cmd.ExecuteNonQuery();

                            MessageBox.Show("检查完成");
                        }
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
