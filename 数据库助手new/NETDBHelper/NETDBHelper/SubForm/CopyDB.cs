using Biz.Common.Data;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class CopyDB : Form
    {
        private volatile Task<int> saveTask = null;

        public CopyDB()
        {
            InitializeComponent();
            this.CLBDBs.Items.Clear();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.OwnerCtl != null)
            {
                var pt = this.OwnerCtl.PointToScreen(this.OwnerCtl.Location);
                pt.Offset(this.OwnerCtl.Width / 2 - this.Width, this.OwnerCtl.Height / 2 - this.Height);
                this.Location = pt;
            }
        }

        private Control OwnerCtl = null;
        public void ShowMe(Control owner)
        {
            this.OwnerCtl = owner;
            this.Show();
        }

        public DBSource DBSource { get; private set; }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                this.Close();
            }
        }

        private async Task<int> Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in CLBDBs.CheckedItems)
                {
                    //创建库
                    var db = item.ToString();
                    sb.AppendLine("create database[" + db + "];");
                    sb.AppendLine("GO");

                    var tbs = SQLHelper.GetTBs(this.DBSource, db);

                    //创建表
                    foreach (var tb in tbs.AsEnumerable())
                    {
                        var tbinfo = new TableInfo
                        {
                            DBName = item.ToString(),
                            Schema = tb["schema"].ToString(),
                            TBId = tb["id"].ToString(),
                            TBName = tb["name"].ToString()
                        };

                        var cols = SQLHelper.GetColumns(this.DBSource, tbinfo.DBName, tbinfo.TBId, tbinfo.TBName).ToList();

                        sb.AppendLine();
                        sb.AppendLine("--" + tbinfo.TBName);
                        sb.AppendLine(DataHelper.GetCreateTableSQL(tbinfo, cols));
                        sb.AppendLine();
                        sb.AppendLine("GO");
                        //索引
                        foreach (var idx in SQLHelper.GetIndexs(DBSource, db, tbinfo.TBName))
                        {
                            //SQLHelper.CreateIndex()
                        }
                        
                        Util.SendMsg(this.OwnerCtl ?? this, "导出库" + db + ",表:" + tbinfo.TBName);
                    }

                    //视图
                    foreach (var v in SQLHelper.GetViews(DBSource, db))
                    {
                        var vsql = SQLHelper.GetViewCreateSql(DBSource, db, v.Key);

                        sb.AppendLine(vsql);
                        sb.AppendLine("GO");
                        Util.SendMsg(this.OwnerCtl ?? this, "导出库" + db + ",视图:" + v.Key);
                    }

                    //存储过程
                    var proclist = SQLHelper.GetProcedures(DBSource, db).ToList();
                    foreach (var proc in proclist)
                    {
                        var body = SQLHelper.GetProcedureBody(DBSource, db, proc);
                        sb.AppendLine(body);
                        sb.AppendLine("GO");
                        Util.SendMsg(this.OwnerCtl ?? this, "导出库" + db + ",存储过程:" + proc);
                    }

                    //函数
                    var functionlist = SQLHelper.GetFunctions(DBSource, db);
                    foreach (var r in functionlist.AsEnumerable())
                    {
                        var body = SQLHelper.GetFunctionBody(DBSource, db, r["name"].ToString());
                        sb.AppendLine(body);
                        sb.AppendLine("GO");
                        Util.SendMsg(this.OwnerCtl ?? this, "导出库" + db + ",函数:" + r["name"].ToString());
                    }
                }

                if (sb.Length > 0)
                {
                    var dir = Application.StartupPath + "\\temp\\";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var filename = Path.Combine(dir, "createdb_" + DateTime.Now.ToString("yyyyyMMddHHmmss") + ".sql");

                    File.WriteAllText(filename, sb.ToString(), Encoding.UTF8);

                    Util.SendMsg(this.OwnerCtl ?? this, "保存成功:" + filename);
                }

                return await Task.FromResult(1);
            }
            catch (Exception ex)
            {
                Util.SendMsg(this.OwnerCtl ?? this, ex.Message);

                return await Task.FromResult(0);
            }
            finally
            {
                
                this.BeginInvoke(new Action(()=>{
                    this.saveTask = null;
                    if (this.Modal)
                    {
                        //Biz.Common.Data.SQLHelper.

                        this.DialogResult = DialogResult.OK;

                    }
                    else
                    {
                        this.Close();
                    }
                }));

                
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (saveTask != null)
            {
                e.Cancel = true;
                Util.SendMsg(this.OwnerCtl ?? this, "正在处理中，不能关闭");
                return;
            }
            base.OnClosing(e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.BtnOk.Enabled = false;
            this.BtnCancel.Enabled = false;
            saveTask = Task.Run(() => Save());


        }

        private void BtnSelectServer_Click(object sender, EventArgs e)
        {
            var connsqlserver = new ConnSQLServer();
            if (connsqlserver.ShowDialog() == DialogResult.OK)
            {
                this.DBSource = connsqlserver.DBSource;
                try
                {
                    this.CLBDBs.Items.Clear();
                    var dbs = Biz.Common.Data.SQLHelper.GetDBs(DBSource);
                    this.CLBDBs.Items.AddRange(dbs.AsEnumerable().Select(p => (object)p.Field<string>("name")).OrderBy(p => p.ToString()).ToArray());
                }
                catch(Exception ex)
                {
                    Util.SendMsg(this.OwnerCtl ?? this, ex.Message);
                }
            }
        }

        private void BtnSelAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < this.CLBDBs.Items.Count; i++)
            {
                this.CLBDBs.SetItemChecked(i, true);
            }
        }
    }
}
