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
using NETDBHelper.SubForm;
using Biz.Common;
using Biz.Common.Data;

namespace NETDBHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string,string> OnCreateEntity;
        public Action<DBSource,string,string, string> OnShowTableData;
        public Action<DBSource,string> OnAddEntityTB;
        public Action<string, string> OnCreateSelectSql;
        public Action<DBSource, string, string, string,CreateProceEnum> OnCreatePorcSQL;
        public Action<DBSource,string,string> OnAddSqlExecuter;
        public Action<DBSource, string, string, string> OnShowProc;
        public Action<DBSource, string, string, string> OnShowDataDic;
        public Action<string, string> OnViewTable;
        private DBSourceCollection _dbServers;
        /// <summary>
        /// 实体命名空间
        /// </summary>
        private static string DefaultEntityNamespace = "Nonamespace";
        public DBSourceCollection DBServers
        {
            get
            {
                return _dbServers;
            }
        }
        public DBServerView()
        {
            InitializeComponent();
            ts_serchKey.Height = 20;
            _dbServers = new DBSourceCollection();
            tv_DBServers.ImageList = new ImageList();
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB1);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB2);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB3);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB4);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB5);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB6);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB7);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB8);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB9);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB10);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB11);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.param);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.paramout);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code); //13
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB16);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB161); //16
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.loading);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ColQ); //18
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_no);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.script_code_red_no);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB16);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.DB161);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.loading);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ColQ); //18
            tv_DBServers.Nodes.Add("0", "资源管理器", 0);
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);

            this.DBServerviewContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuStrip_ItemClicked);
            
            this.CommMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(CommMenuStrip_ItemClicked);
        }

        void CommMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "刷新":
                        ReLoadDBObj(tv_DBServers.SelectedNode);
                        break;
                    case "复制对象名":
                        if (this.tv_DBServers.SelectedNode != null)
                        {
                            string s = tv_DBServers.SelectedNode.Text;
                            if (s.IndexOf('(') > -1)
                                Clipboard.SetText(s.Substring(0, s.IndexOf('(')));
                            else
                                Clipboard.SetText(s);
                        }
                        break;
                    case "添加实体映射表":
                        if (OnAddEntityTB != null)
                        {
                            var node = tv_DBServers.SelectedNode;
                            if (node == null)
                                return;
                            OnAddEntityTB(GetDBSource(node), node.Text);
                        }
                        break;
                    case "删除对象":
                        if (MessageBox.Show("确认要删除数据库" + tv_DBServers.SelectedNode.Text + "吗？", "询问",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            var node = tv_DBServers.SelectedNode;
                            Biz.Common.Data.MySQLHelper.DeleteDataBase(GetDBSource(node), node.Text);
                            ReLoadDBObj(node.Parent);


                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = node.Text,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.db,
                                DB = node.Text,
                                Sever = GetDBSource(node).ServerName,
                                Info = "删除",
                                Valid = true
                            });
                        }
                        break;
                    case "新增对象":
                        var selnode = tv_DBServers.SelectedNode;
                        var dlg = new SubForm.InputStringDlg("请输入库名：");
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            Biz.Common.Data.MySQLHelper.CreateDataBase(GetDBSource(selnode),selnode.FirstNode.Text, dlg.InputString);
                            ReLoadDBObj(selnode);
                        }


                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                        {
                            TypeName = dlg.InputString,
                            LogTime = DateTime.Now,
                            LogType = LogTypeEnum.db,
                            DB = dlg.InputString,
                            Sever = GetDBSource(selnode).ServerName,
                            Info = "新增",
                            Valid = true
                        });
                        break;
                    case "查看表":
                        {
                            OnViewTables();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        void ReLoadDBObj(TreeNode selNode)
        {
            //TreeNode selNode = tv_DBServers.SelectedNode;
            if (selNode == null)
                return;
            if (selNode.Level == 1)
            {
                Biz.UILoadHelper.LoadDBsAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 2)
            {
                var dbname = GetDBName(selNode).ToUpper();
                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, selNode, GetDBSource(selNode), name =>
                {
                    var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                 [] { dbname, name.ToUpper(), string.Empty }).FirstOrDefault();
                    return mark == null ? string.Empty : mark.MarkInfo;
                });
            }
            else if (selNode.Level == 3 && !selNode.Text.Equals("存储过程"))
            {
                var dbname = GetDBName(selNode).ToUpper();
                var dbsource = GetDBSource(selNode);
                var synccolumnmark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.
                    Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { dbname, selNode.Text.ToUpper() }).FirstOrDefault() != null;
                Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, selNode, dbsource, (col) =>
                {
                    var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { dbname, selNode.Text.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();

                    if (mark == null && !synccolumnmark && !string.IsNullOrWhiteSpace(col.Description))
                    {
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<MarkObjectInfo>("MarkObjectInfo", new MarkObjectInfo
                        {
                            DBName = dbname.ToUpper(),
                            ColumnName = col.Name.ToUpper(),
                            Servername = dbsource.ServerName,
                            TBName = selNode.Text.ToUpper(),
                            ColumnType=col.TypeToString(),
                            MarkInfo = col.Description
                        });
                    }

                    return mark == null ? string.Empty : mark.MarkInfo;
                });
                if (!synccolumnmark)
                {
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<ColumnMarkSyncRecord>("ColumnMarkSyncRecord",
                        new ColumnMarkSyncRecord
                        {
                            DBName = dbname.ToUpper(),
                            SyncDate = DateTime.Now,
                            TBName = selNode.Text.ToUpper()
                        });
                }

                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = selNode.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.table,
                    DB = dbname,
                    Sever = GetDBSource(selNode).ServerName,
                    Valid = true
                });
            }
            else if (selNode.Level == 3 && selNode.Text.Equals("存储过程"))
            {
                Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
            else if (selNode.Level == 4 && selNode.Text.Equals("索引"))
            {
                Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, selNode, GetDBSource(selNode));
            }
        }

        void OnMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                DBServerviewContextMenuStrip.Visible = false;
                switch (e.ClickedItem.Text)
                {
                    case "生成实体类":
                        CreateEntityClass();
                        break;
                    case "显示前100条数据":
                        ShowTop100Data();
                        break;
                    case "复制对象名":
                        if (this.tv_DBServers.SelectedNode != null)
                        {
                            Clipboard.SetText(tv_DBServers.SelectedNode.Text);
                        }
                        break;
                    case "删除表":
                        if (MessageBox.Show("确认要删除表" + tv_DBServers.SelectedNode.Text + "吗？", "询问",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            var node = tv_DBServers.SelectedNode;

                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = node.Text,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.table,
                                DB = node.Parent.Text,
                                Sever = GetDBSource(node).ServerName,
                                Info = "删除",
                                Valid = true
                            });

                            Biz.Common.Data.MySQLHelper.DeleteTable(GetDBSource(node), node.Parent.Text, node.Text);
                            ReLoadDBObj(node.Parent);
                        }
                        break;
                    case "刷新":
                        ReLoadDBObj(tv_DBServers.SelectedNode);
                        break;
                    case "修改表名":
                        var _node=tv_DBServers.SelectedNode;
                        var oldname = _node.Text;
                        var dlg=new SubForm.InputStringDlg("修改表名:", _node.Text);
                        if ( dlg.ShowDialog()== DialogResult.OK)
                        {
                            if (string.Equals(dlg.InputString, oldname, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                            Biz.Common.Data.MySQLHelper.ReNameTableName(GetDBSource(_node), _node.Parent.Text,
                                oldname, dlg.InputString);
                            ReLoadDBObj(_node.Parent);

                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                            {
                                TypeName = dlg.InputString,
                                LogTime = DateTime.Now,
                                LogType = LogTypeEnum.table,
                                DB = _node.Parent.Text,
                                Sever = GetDBSource(_node).ServerName,
                                Info = string.Format("重命名：{0}-{1}", oldname, dlg.InputString),
                                Valid = true
                            });
                        }
                        break;
                    case "InsertOrUpdate":
                        _node=tv_DBServers.SelectedNode;
                        if (this.OnCreatePorcSQL != null)
                        {
                            this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text,CreateProceEnum.InsertOrUpdate);
                        }
                        break;
                    case "Delete":
                        _node = tv_DBServers.SelectedNode;
                        if (this.OnCreatePorcSQL != null)
                        {
                            this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.Delete);
                        }
                        break;
                    case "Select":

                        break;
                    case "创建语句":
                        MessageBox.Show("Create");
                        break;
                    case "备注":
                        {
                            MarkResource();
                            break;
                        }
                    case "清理无效字段":
                        {
                            ClearMarkResource();
                            break;
                        }
                    default:
                        _node = tv_DBServers.SelectedNode;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        void ShowTop100Data()
        {
            if (tv_DBServers.SelectedNode != null && tv_DBServers.SelectedNode.Level == 3)
            {
                List<KeyValuePair<string, bool>> cols = new List<KeyValuePair<string, bool>>();
                foreach (TreeNode node in tv_DBServers.SelectedNode.Nodes)
                {
                    if (node.Text == "索引" && node == tv_DBServers.SelectedNode.LastNode)
                    {
                        continue;
                    }
                    cols.Add(new KeyValuePair<string, bool>(node.Text.Substring(0,node.Text.IndexOf('(')), (node.Tag as TBColumn).IsKey));
                }
                StringBuilder sb = new StringBuilder("SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ;");
                sb.AppendLine();
                sb.AppendLine("select");
                sb.Append(string.Join(",\r\n", cols.Select(p =>"["+p.Key+"]")));
                sb.AppendLine("");
                sb.Append(" from ");
                sb.Append(tv_DBServers.SelectedNode.Text);
                sb.Append(" limit 0,100;");
                sb.AppendLine();
                sb.AppendLine("SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ ;");
                if (this.OnShowTableData != null)
                {
                    OnShowTableData(this.tv_DBServers.SelectedNode.Parent.Parent.Tag as DBSource,this.tv_DBServers.SelectedNode.Parent.Text,this.tv_DBServers.SelectedNode.Text, sb.ToString());
                }
            }
        }

        void CreateEntityClass()
        {
            if (tv_DBServers.SelectedNode != null && tv_DBServers.SelectedNode.Level == 3)
            {
                bool hasKey = false;
                var tbDesc = Biz.Common.Data.MySQLHelper.GetTableColsDescription(GetDBSource(tv_DBServers.SelectedNode), tv_DBServers.SelectedNode.Parent.Text,
                    tv_DBServers.SelectedNode.Text);

                Regex rg = new Regex(@"(\w+)\s*\((\w+)\)");
                string format = @"        {4}public {0} {1}
        {{
            get
            {{
                return {2};
            }}
            set
            {{
                {3}=value;
            }}
        }}";
                //命名空间
                SubForm.CreateEntityNavDlg dlg = new CreateEntityNavDlg("请输入实体命名空间", DefaultEntityNamespace);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DefaultEntityNamespace = dlg.InputString;
                }

                StringBuilder sb = new StringBuilder(string.Format("namespace {0}\r\n", DefaultEntityNamespace));
                sb.AppendLine("{");
                sb.AppendLine("    [Serializable]");
                if (dlg.SupportProtobuf)
                    sb.AppendLine("    [ProtoContract]");
                if (dlg.SupportDBMapperAttr)
                    sb.AppendLine("    [DataBaseMapperAttr(TableName=\"" + tv_DBServers.SelectedNode.Text + "\")]");
                sb.Append("    public class ");
                sb.Append(Biz.Common.StringHelper.FirstToUpper(tv_DBServers.SelectedNode.Text));
                sb.Append("Entity");
                sb.Append("\r\n    {\r\n");
                if (dlg.SupportDBMapperAttr)
                {
                    sb.AppendLine("        //表名");
                    sb.AppendLine(string.Format("        public const string TbName=\"{0}.{1}\";", tv_DBServers.SelectedNode.Parent.Text, tv_DBServers.SelectedNode.Text));
                }
                sb.AppendLine();
                sb.AppendLine(@"
        //分表
        public string SplitTbName
        {
            get
            {
                throw new NotImplementedException(); 
            }
        }");
                sb.AppendLine();
                TreeNode selNode = tv_DBServers.SelectedNode;
                int idx = 1;
                foreach (TreeNode node in selNode.Nodes)
                {
                    if (node.Text == "索引" && node == selNode.LastNode)
                    {
                        continue;
                    }
                    Match m = rg.Match(node.Text);
                    if (m.Success)
                    {
                        var y = (from x in tbDesc.AsEnumerable()
                                 where string.Equals((string)x["ColumnName"], m.Groups[1].Value, StringComparison.OrdinalIgnoreCase)
                                 select x["Description"]).FirstOrDefault();

                        string desc = y == DBNull.Value ? string.Empty : (string)y;

                        string privateAttr = string.Concat("_" + Biz.Common.StringHelper.FirstToLower(m.Groups[1].Value));
                        sb.AppendFormat("        private {0} {1};", Biz.Common.Data.Common.DbTypeToNetType(m.Groups[2].Value), privateAttr);
                        sb.AppendLine();
                        if (dlg.SupportProtobuf)
                        {
                            sb.AppendLine(string.Format("        [ProtoMember({0})]", idx++));
                        }
                        bool iskey = false;
                        if (node.Tag != null && node.Tag is TBColumn)
                        {
                            iskey = (node.Tag as TBColumn).IsID||(node.Tag as TBColumn).IsKey;
                        }

                        if (dlg.SupportDBMapperAttr)
                        {
                            if (iskey)
                            {
                                sb.AppendLine("        [DataBaseMapperAttr(Column=\"" + m.Groups[1].Value + "\",isKey=true)]");
                                hasKey = true;
                            }
                            else
                            {
                                sb.AppendLine("        [DataBaseMapperAttr(Column=\"" + m.Groups[1].Value + "\")]");
                            }
                        }


                        if (dlg.SupportJsonproterty)
                        {
                            sb.AppendLine("        [JsonProperty(\"" + m.Groups[1].Value.ToLower() + "\")]");
                            sb.AppendLine("        [PropertyDescriptionAttr(\"" + desc + "\")]");
                        }

                        sb.AppendFormat(format, Biz.Common.Data.Common.DbTypeToNetType(m.Groups[2].Value), Biz.Common.StringHelper.FirstToUpper(m.Groups[1].Value),
                            privateAttr, privateAttr, dlg.SupportMvcDisplay ? string.Format("[Display(Name = \"{0}\")]\r\n        ", desc) : string.Empty);
                        sb.AppendLine();
                    }
                    else
                    {
                        MessageBox.Show("生成实体类错误：" + node.Text);
                        break;
                    }
                }
                sb.AppendLine("    }");
                sb.AppendLine("}");
                if (OnCreateEntity != null)
                {
                    OnCreateEntity("实体类" + selNode.Text, sb.ToString());
                }
                Clipboard.SetText(sb.ToString());
                MainFrm.SendMsg(string.Format("实体代码已经复制到剪贴板,{0}", hasKey ? "" : "警告：表没有自增主键。"));
            }
        }

        private DBSource GetDBSource(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Level < 1)
                return null;
            if (node.Level == 1)
                return DBServers.FirstOrDefault(p => p.ServerName.Equals(node.Text));
            return GetDBSource(node.Parent);
        }

        void tv_DBServers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Node.Level == 2)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                //var server=DBServers.FirstOrDefault(p=>p.ServerName.Equals(e.Node.Parent.Text));
                //if (server == null)
                //    return;
                //DataTable tb= Biz.Common.Data.SQLHelper.GetTBs(server, e.Node.Text);
                //for (int i = 0; i < tb.Rows.Count; i++)
                //{
                //    TreeNode newNode = new TreeNode(tb.Rows[i]["name"].ToString(),3, 3);
                //    newNode.Name=tb.Rows[i]["id"].ToString();
                //    e.Node.Nodes.Add(newNode);
                //}
                //e.Node.Expand();
                Biz.UILoadHelper.LoadTBsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), name =>
                {
                    var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                 [] { GetDBName(e.Node).ToUpper(), name.ToUpper(), string.Empty }).FirstOrDefault();
                    return mark == null ? string.Empty : mark.MarkInfo;
                });


                LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                {
                    TypeName = e.Node.Text,
                    LogTime = DateTime.Now,
                    LogType = LogTypeEnum.db,
                    DB = e.Node.Text,
                    Sever = GetDBSource(e.Node).ServerName,
                    Valid = true
                });
            }
            if (e.Node.Level == 3)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
                //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Parent.Text));
                //if (server == null)
                //    return;
                //foreach(TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(server,e.Node.Parent.Text,e.Node.Name))
                //{
                //    int imgIdx = col.IsKey ? 4 : 5;
                //    TreeNode newNode = new TreeNode(string.Concat(col.Name,"(",col.TypeName,")"),imgIdx, imgIdx);
                //    newNode.Tag = col;
                //    e.Node.Nodes.Add(newNode);
                //}
                //e.Node.Expand();

                if (e.Node.Text.Equals("存储过程"))
                {
                    Biz.UILoadHelper.LoadProcedureAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));

                }
                else
                {
                    var dbname = GetDBName(e.Node).ToUpper();
                    var dbsource = GetDBSource(e.Node);
                    var synccolumnmark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.
                       Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { dbname, e.Node.Text.ToUpper() }).FirstOrDefault() != null;
                    Biz.UILoadHelper.LoadColumnsAnsy(this.ParentForm, e.Node, GetDBSource(e.Node), (col) =>
                    {
                        var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                    [] { dbname, e.Node.Text.ToUpper(), col.Name.ToUpper() }).FirstOrDefault();
                        if (mark == null && !synccolumnmark && !string.IsNullOrWhiteSpace(col.Description))
                        {
                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<MarkObjectInfo>("MarkObjectInfo", new MarkObjectInfo
                            {
                                DBName = dbname.ToUpper(),
                                ColumnName = col.Name.ToUpper(),
                                Servername = dbsource.ServerName,
                                TBName = e.Node.Text.ToUpper(),
                                ColumnType=col.TypeToString(),
                                MarkInfo = col.Description
                            });
                        }
                        return mark == null ? string.Empty : mark.MarkInfo;
                    });

                    if (!synccolumnmark)
                    {
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<ColumnMarkSyncRecord>("ColumnMarkSyncRecord",
                            new ColumnMarkSyncRecord
                            {
                                DBName = dbname.ToUpper(),
                                SyncDate = DateTime.Now,
                                TBName = e.Node.Text.ToUpper()
                            });
                    }

                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                    {
                        TypeName = e.Node.Text,
                        LogTime = DateTime.Now,
                        LogType = LogTypeEnum.table,
                        DB = dbname,
                        Sever = GetDBSource(e.Node).ServerName,
                        Valid = true
                    });
                }
            }
            else if (e.Node.Level == 4)
            {
                if (e.Node.Nodes.Count > 0)
                    return;
               
                if (e.Node.Text.Equals("索引"))
                {
                    Biz.UILoadHelper.LoadIndexAnsy(this.ParentForm, e.Node, GetDBSource(e.Node));
                }
            }

        }
        public void Bind()
        {
            if (DBServers == null)
                return;
            foreach (DBSource s in DBServers)
            {
                bool isAdd = false;
                foreach (TreeNode n in tv_DBServers.Nodes[0].Nodes)
                {
                    if (n.Text.Equals(s.ServerName))
                    {
                        isAdd = true;
                        break;
                    }
                }
                if (isAdd)
                    continue;
                if (tv_DBServers.Nodes[0].Nodes.ContainsKey(s.ServerName))
                    continue;
               TreeNode node = new TreeNode(s.ServerName,1,1);
               node.Tag = s;
               tv_DBServers.Nodes[0].Nodes.Add(node);
               DataTable table= Biz.Common.Data.MySQLHelper.GetDBs(s);
               for (int i = 0; i < table.Rows.Count; i++)
               {
                   TreeNode tbNode = new TreeNode(table.Rows[i]["Name"].ToString(), 2, 2);
                   node.Nodes.Add(tbNode);
               }
            }
        }

        private void DBServerView_Load(object sender, EventArgs e)
        {
            Bind();
        }

        public void DisConnectSelectDBServer()
        {
            if (this.tv_DBServers.SelectedNode==null||this.tv_DBServers.SelectedNode.Level != 1)
                return;
            DisConnectServer(this.tv_DBServers.SelectedNode.Text);
        }

        private void DisConnectServer(string serverName)
        {
            this.DBServers.Remove(this.DBServers.FirstOrDefault(p=>p.ServerName.Equals(serverName)));
            foreach (TreeNode node in tv_DBServers.Nodes[0].Nodes)
            {
                if (node.Text.Equals(serverName))
                {
                    tv_DBServers.Nodes[0].Nodes.Remove(node);
                    break;
                }
            }
        }

        private void tv_DBServers_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node=tv_DBServers.SelectedNode;
                if ( node!= null)
                {
                    if ((tv_DBServers.SelectedNode.Level == 3 && !tv_DBServers.SelectedNode.Text.Equals("存储过程"))
                        || (tv_DBServers.SelectedNode.Level == 4 && tv_DBServers.SelectedNode.Parent.Text.Equals("存储过程"))
                        || (tv_DBServers.SelectedNode.Level == 5 && tv_DBServers.SelectedNode.Parent.Text.Equals("索引")))
                    {
                        this.tv_DBServers.ContextMenuStrip = this.DBServerviewContextMenuStrip;
                        if (tv_DBServers.SelectedNode.Parent.Text.Equals("存储过程"))
                        {
                            foreach(ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = false;
                            }

                            导出ToolStripMenuItem.Visible = true;
                        }
                        else if (tv_DBServers.SelectedNode.Parent.Text.Equals("索引"))
                        {
                            foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = false;
                            }
                            TSM_ManIndex.Visible = true;
                        }
                        else
                        {
                            foreach (ToolStripItem item in tv_DBServers.ContextMenuStrip.Items)
                            {
                                item.Visible = true;
                                ExpdataToolStripMenuItem.Visible = false;
                            }
                        }
                        TTSM_CreateIndex.Visible = node.Level == 3;
                        TTSM_DelIndex.Visible = node.Level == 5 && node.Parent.Text.Equals("索引");

                        ExpdataToolStripMenuItem.Visible = node.Level == 3;
                    }
                    else
                    {
                        this.tv_DBServers.ContextMenuStrip = this.CommMenuStrip;
                        subMenuItemAddEntityTB.Visible = node.Level == 2;
                        CommSubMenuItem_Delete.Visible = node.Level == 2;
                        TSMI_ViewTableList.Visible = node.Level == 2;
                        CommSubMenuitem_add.Visible = node.Level == 1;
                        CommSubMenuitem_ViewConnsql.Visible = node.Level == 2;
                        性能分析工具ToolStripMenuItem.Visible = node.Level == 1;
                        SqlExecuterToolStripMenuItem.Visible = node.Level == 2;
                        备注本地ToolStripMenuItem.Visible = node.Level == 4;
                    }
                }
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            string serchkey = ts_serchKey.Text;

            bool matchall = serchkey.StartsWith("'");
            if (matchall)
            {
                serchkey = serchkey.Trim('\'');
            }
            if (!ts_serchKey.Items.Contains(serchkey))
            {
                ts_serchKey.Items.Add(serchkey);
            }
            if (tv_DBServers.SelectedNode == null)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }
            if (!tv_DBServers.Focused)
            {
                this.tv_DBServers.Focus();
            }
            bool boo = false;
            if (tv_DBServers.SelectedNode.Nodes.Count > 0)
                boo=SearchNode(tv_DBServers.SelectedNode.Nodes[0], serchkey, matchall, true);
            else if (tv_DBServers.SelectedNode.NextNode != null)
                boo=SearchNode(tv_DBServers.SelectedNode.NextNode, serchkey, matchall, true);
            else
            {
                var parent = tv_DBServers.SelectedNode.Parent;
                while (parent != null && parent.NextNode == null)
                {
                    parent = parent.Parent;
                }
                if (parent != null)
                {
                    if (parent.NextNode != null)
                    {
                        boo = SearchNode(parent.NextNode, serchkey, matchall, true);
                    }
                }
            }
            if (!boo)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }
        }

        private bool SearchNode(TreeNode nodeStart, string txt, bool matchall, bool maxsearch)
        {
            if (nodeStart == null)
            {
                return false;
            }
            var find = matchall ? nodeStart.Text.Equals(txt, StringComparison.OrdinalIgnoreCase) : nodeStart.Text.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1;
            if (find)
            {
                tv_DBServers.SelectedNode = nodeStart;
                return true;
            }
            if (nodeStart.Nodes.Count > 0)
            {
                foreach (TreeNode node in nodeStart.Nodes)
                {
                    if (SearchNode(node, txt, matchall, false))
                        return true;
                }
            }
            if (maxsearch)
            {
                if (nodeStart.NextNode != null)
                {
                    return SearchNode(nodeStart.NextNode, txt, matchall, true);
                }
                else
                {
                    if (maxsearch)
                    {
                        var parent = nodeStart.Parent;
                        while (parent != null && parent.NextNode == null)
                        {
                            parent = parent.Parent;
                        }
                        if (parent != null)
                        {
                            return SearchNode(parent.NextNode, txt, matchall, true);
                        }
                    }
                }
            }

            return false;
        }

        private void ts_serchKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                tv_DBServers.Focus();
                toolStripDropDownButton1_Click(null, null);
            }
        }

        public TreeNode FindNode(string serverName, string dbName=null, string tbName=null)
        {
            foreach (TreeNode node in tv_DBServers.Nodes[0].Nodes)
            {
                if (node.Text.Equals(serverName, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(dbName))
                        return node;
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        if (subNode.Text.Equals(dbName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (string.IsNullOrWhiteSpace(tbName))
                                return subNode;
                            foreach (TreeNode subSubNode in subNode.Nodes)
                            {
                                if (subSubNode.Text.Equals(tbName, StringComparison.OrdinalIgnoreCase))
                                    return subSubNode;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void SubMenuItem_Insert_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.InsertOrUpdate);
            }
        }

        private void 创建语句ToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var node = this.tv_DBServers.SelectedNode;
            if (node != null && node.Level == 3)
            {
                StringBuilder sb = new StringBuilder(string.Format("CREATE TABLE `{0}`(", node.Text));
                sb.AppendLine();
                foreach (TBColumn col in Biz.Common.Data.MySQLHelper.GetColumns(GetDBSource(node), node.Parent.Text, node.Text))
                {
                    sb.AppendFormat("`{0}` {1} {2} {3},", col.Name, Biz.Common.Data.Common.GetDBType(col), (col.IsID || col.IsKey) ? "NOT NULL" : (col.IsNullAble ? "NULL" : "NOT NULL"), col.IsID ? "AUTO_INCREMENT" : "");
                    if (col.IsID)
                    {
                        sb.AppendLine();
                        sb.AppendFormat("PRIMARY KEY (`{0}`),", col.Name);
                    }
                    sb.AppendLine();
                }
                sb.AppendLine("`last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
                sb.AppendLine(")ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=utf8;");
                sb.AppendLine("//注意：bit类型要手工改成TINYINT(1)。");
                TextBoxWin win = new TextBoxWin("创建表" + node.Text, sb.ToString());
                win.ShowDialog();
            }
            else if (node != null && node.Level == 4 && node.Parent.Text.Equals("存储过程"))
            {
                if (OnShowProc != null)
                {
                    var body = Biz.Common.Data.MySQLHelper.GetProcedureBody(GetDBSource(node), node.Parent.Parent.Text, node.Text);
                    //TextBoxWin win = new TextBoxWin("存储过程[" + node.Text + "]", "drop PROCEDURE if exists " + node.Text + ";\r\n\r\n" + body.Replace("\n","\r\n"));
                    //win.ShowDialog();
                    OnShowProc(GetDBSource(node), node.Parent.Parent.Text, node.Text, body);

                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Insert<HLogEntity>("HLog", new HLogEntity
                    {
                        TypeName = node.Text,
                        LogTime = DateTime.Now,
                        LogType = LogTypeEnum.proc,
                        DB = node.Parent.Parent.Text,
                        Sever = GetDBSource(node).ServerName,
                        Valid = true
                    });
                }
            }
        }

        private void ExpdataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || node.Level != 3)
            {
                return;
            }
            var cols = Biz.Common.Data.MySQLHelper.GetColumns(GetDBSource(node), node.Parent.Text, node.Text)
                .Where(p => !p.IsID).ToList();
            string sqltext = string.Format("select {0} from {1}", string.Join(",", cols.Select(p => string.Concat("[", p.Name, "]"))), string.Concat("[", node.Text, "]"));
            var datas = Biz.Common.Data.MySQLHelper.ExecuteDBTable(GetDBSource(node), node.Parent.Text, sqltext, null);
            StringBuilder sb = new StringBuilder(string.Format("Insert into {0} ({1}) values", string.Concat("`", node.Text, "`"), string.Join(",", cols.Select(p => string.Concat("`", p.Name, "`")))));
            foreach (DataRow row in datas.Rows)
            {
                StringBuilder sb1 = new StringBuilder();
                foreach (var column in cols)
                {
                    object data = row[column.Name];
                    if (data == DBNull.Value)
                    {
                        sb1.Append("NULL,");
                    }
                    else
                    {
                        if (column.TypeName.IndexOf("int", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.IndexOf("float", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("bit", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.Equals("real", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                            || column.TypeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
                            || column.TypeName.IndexOf("money", StringComparison.OrdinalIgnoreCase) > -1
                        )
                        {
                            sb1.AppendFormat("{0},", data);
                        }
                        else if (column.TypeName.Equals("boolean", StringComparison.OrdinalIgnoreCase)
                               || column.TypeName.Equals("bool", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("{0},", data.Equals(true) ? 1 : 0);
                        }
                        else if (column.TypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                        {
                            sb1.AppendFormat("'{0}',", ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            sb1.Append(string.Concat("'", data, "',"));
                        }
                    }
                }
                if (sb1.Length > 0)
                    sb1.Remove(sb1.Length - 1, 1);
                sb.AppendFormat("({0}),", sb1.ToString());
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            TextBoxWin win = new TextBoxWin("导出数据", sb.ToString());
            win.ShowDialog();
        }

        private void SubMenuItem_Select_Click(object sender, EventArgs e)
        {
            var node = this.tv_DBServers.SelectedNode;
            if (node == null || node.Level != 3)
            {
                return;
            }

            DBSource dbsource = GetDBSource(node);
            string dbname = node.Parent.Text,
                tbname = node.Text, tid = node.Name;

            var cols = Biz.Common.Data.MySQLHelper.GetColumns(dbsource, dbname, tbname).ToList();

            SubForm.WinCreateSelectSpNav nav = new WinCreateSelectSpNav(cols);

            if (nav.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            var conditioncols = nav.ConditionColumns;
            var outputcols = nav.OutPutColumns;

            string codes = MySQLHelper.CreateSelectSql(dbname, tbname, nav.Editer, nav.SPAbout, cols, conditioncols, outputcols);

            if (OnCreateSelectSql != null)
            {
                OnCreateSelectSql(string.Format("查询[{0}.{1}]", dbname, tbname), codes);
            }
        }

        private void CommSubMenuitem_ViewConnsql_Click(object sender, EventArgs e)
        {
            var node = tv_DBServers.SelectedNode;
            if (node == null)
                return;

            string conndb = string.Empty;
            if (node.Level < 2)
                return;

            if (node.Level == 2)
            {
                conndb = node.Text;
            }
            else
            {
                var pnode = node.Parent;
                while (pnode.Level != 2)
                {
                    pnode = pnode.Parent;
                }
                conndb = pnode.Text;
            }
            
            var connsql = MySQLHelper.GetConnstringFromDBSource(GetDBSource(node), conndb);
            SubForm.WinWebBroswer web = new WinWebBroswer();
            web.SetHtml(string.Format("<html><head><title>连接串_{1}</title></head><body><br/>&lt;add name=\"ConndbDB${1}\" connectionString=\"{0}\" providerName=\"MySql.Data.MySqlClient\"/&gt;</body></html>", connsql, conndb));
            web.Show();
        }

        private void SqlExecuterToolStripMenuItem_Click(object sender, EventArgs e)
        {
             var node = tv_DBServers.SelectedNode;
            if (node == null)
                return;
            if (OnAddSqlExecuter != null)
            {
                OnAddSqlExecuter(GetDBSource(node),node.Text,null);
            }
        }

        private void 生成数据字典ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selnode = tv_DBServers.SelectedNode;
            if (selnode != null && selnode.Level == 3)
            {
                //库名
                string tbname = string.Format("[{0}].[{1}]", selnode.Parent.Text, selnode.Text);

                var tbclumns = Biz.Common.Data.MySQLHelper.GetColumns(this.GetDBSource(selnode), selnode.Parent.Text, selnode.Text).ToList();
                var tbmark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                  [] { selnode.Parent.Text.ToUpper(), selnode.Text.ToUpper(), string.Empty }).FirstOrDefault();
                var tbdesc = tbmark == null ? selnode.Text : tbmark.MarkInfo;

                DataTable resulttb = new DataTable();
                resulttb.Columns.AddRange(new string[][] { 
                    new []{"line","行号"},
                    new []{"name","列名"},
                    new []{"iskey","是否主键"},
                    new []{"null","可空"},
                    new []{"type","类型"},
                    new []{"len","长度"},
                    new []{"desc","说明"} }.Select(s => new DataColumn
                    {
                        ColumnName = s[0],
                        Caption = s[1],
                    }).ToArray());

                var tbDesc = Biz.Common.Data.MySQLHelper.GetTableColsDescription(GetDBSource(tv_DBServers.SelectedNode), tv_DBServers.SelectedNode.Parent.Text,
                    tv_DBServers.SelectedNode.Text);

                Regex rg = new Regex(@"(\w+)\s*\((\w+)\)");

                TreeNode selNode = tv_DBServers.SelectedNode;
                int idx = 1;
                foreach (TreeNode node in selNode.Nodes)
                {
                    if (node.Text.Equals("索引") && node == selnode.LastNode)
                    {
                        continue;
                    }
                    var newrow = resulttb.NewRow();
                    newrow["line"] = idx++;
                    Match m = rg.Match(node.Text);
                    if (m.Success)
                    {
                        var y = (from x in tbDesc.AsEnumerable()
                                 where string.Equals((string)x["ColumnName"], m.Groups[1].Value, StringComparison.OrdinalIgnoreCase)
                                 select x["Description"]).FirstOrDefault();

                        if (y == null || string.IsNullOrEmpty(y.ToString()))
                        {
                            var mark = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new
                                [] { GetDBName(selnode).ToUpper(), GetTBName(selnode).ToUpper(), m.Groups[1].Value.ToUpper() }).FirstOrDefault();
                            if (mark != null)
                            {
                                y = mark.MarkInfo;
                            }
                        }

                        //字段描述
                        string desc = y == DBNull.Value ? "&nbsp;" : (string)y;
                        newrow["desc"] = string.IsNullOrEmpty(desc) ? "&nbsp;" : desc;
                        newrow["name"] = m.Groups[1].Value;
                        newrow["type"] = m.Groups[2].Value;

                        bool iskey = false;
                        if (node.Tag != null && node.Tag is TBColumn)
                        {
                            iskey = (node.Tag as TBColumn).IsKey;
                        }
                        newrow["iskey"] = iskey ? "√" : "✕";

                        var col = tbclumns.Find(p => p.Name.Equals(m.Groups[1].Value, StringComparison.OrdinalIgnoreCase));
                        if (col != null)
                        {
                            newrow["len"] = col.prec != 0 ? col.prec.ToString() : (col.Length>0?col.Length.ToString():"&nbsp;");
                            newrow["null"] = col.IsNullAble ? "√" : "✕";
                        }

                        resulttb.Rows.Add(newrow);
                    }
                    else
                    {
                        MessageBox.Show("生成数据字典错误：" + node.Text);
                        break;
                    }

                }

                //生成HTML
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<html><head><title>数据字典-{0}</title><style>
p{{font-size:11px;}}
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style></head><body><p>表名：{0}</p><p>表说明：{1}</p><table cellpadding='0' cellspacing='0' border='1'>", tbname, tbdesc);
                sb.Append("</tr>");
                foreach (DataColumn col in resulttb.Columns)
                {
                    sb.AppendFormat("<th>{0}</th>", col.Caption);
                }
                sb.Append("</tr>");

                foreach (DataRow row in resulttb.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn col in resulttb.Columns)
                    {
                        sb.AppendFormat("<td>{0}</td>", row[col.ColumnName]);
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</table></body></html>");

                if (OnShowDataDic != null)
                {
                    OnShowDataDic(GetDBSource(selnode), GetDBName(selnode), selnode.Text, sb.ToString());
                }

            }
        }

        private void 性能分析工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selnode = this.tv_DBServers.SelectedNode;
            if (selnode == null)
                return;
            var db = this.GetDBSource(selnode);
            if (db == null)
                return;
            new SubForm.SubFrmPerformAnalysis(db).Show();
        }

        private void SubMenuItem_Delete_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (this.OnCreatePorcSQL != null)
            {
                this.OnCreatePorcSQL(GetDBSource(_node), _node.Parent.Text, _node.Name, _node.Text, CreateProceEnum.Delete);
            }
        }

        private void TTSM_CreateIndex_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            var ds = GetDBSource(_node);
            var db = _node.Parent.Text;
            var tb = _node.Text;
            var cols = Biz.Common.Data.MySQLHelper.GetColumns(ds, db, tb).ToList();
            WinCreateIndex win = new WinCreateIndex(cols);
            if (win.ShowDialog() == DialogResult.OK && MessageBox.Show("要创建索引吗？") == DialogResult.OK)
            {
                try
                {
                    Biz.Common.Data.MySQLHelper.CreateIndex(ds, db, tb, win.IndexName, win.IsUnique(), win.IsPrimaryKey(),win.IsAutoIncr(), win.IndexColumns);
                    MessageBox.Show("创建索引成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "创建索引出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TTSM_DelIndex_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
            if (_node.Level == 5 && _node.Parent.Text.Equals("索引"))
            {
                var indexname = _node.Text;
                if (MessageBox.Show("要删除索引\"" + indexname + "\"吗?", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var ds = GetDBSource(_node);
                    try
                    {
                        Biz.Common.Data.MySQLHelper.DropIndex(ds, _node.Parent.Parent.Parent.Text, _node.Parent.Parent.Text, indexname.Equals("primary", StringComparison.OrdinalIgnoreCase), indexname);
                        MessageBox.Show("删除成功");
                        ReLoadDBObj(_node.Parent);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "删除索引失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string GetTBName(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Level < 2)
                return null;
            if (node.Level == 3)
                return node.Text;
            return GetTBName(node.Parent);
        }

        private string GetDBName(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Level < 1)
                return null;
            if (node.Level == 2)
                return node.Text;
            return GetDBName(node.Parent);
        }

        private void Mark_Local()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (selnode != null && selnode.Level == 4)
            {
                var col = ((TBColumn)selnode.Tag).Name;
                var tbname = GetTBName(selnode);
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), tbname.ToUpper(), col.ToUpper() }).FirstOrDefault();

                if (item == null)
                {
                    item = new MarkObjectInfo { ColumnName = col.ToUpper(), DBName = dbname.ToUpper(), TBName = tbname.ToUpper(), Servername = servername };
                }
                InputStringDlg dlg = new InputStringDlg($"备注字段[{tbname}.{col}]", item.MarkInfo);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (selnode.ImageIndex == 18)
                    {
                        selnode.ImageIndex = selnode.SelectedImageIndex = 5;
                    }
                    item.MarkInfo = dlg.InputString;
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                    selnode.ToolTipText = item.MarkInfo;
                    MessageBox.Show("备注成功");
                }
            }
            else if (selnode.Parent.Text.Equals("存储过程"))
            {
                var servername = GetDBSource(selnode).ServerName;
                var dbname = GetDBName(selnode);
                var spname = selnode.Text;
                var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<SPInfo>("SPInfo", "DBName_SPName", new[] { dbname.ToUpper(), spname.ToUpper() }).FirstOrDefault();

                if (item == null)
                {
                    item = new SPInfo { Servername = servername, DBName = dbname, SPName = spname, Mark = "" };
                }
                InputStringDlg dlg = new InputStringDlg($"备注字段[{dbname}.{spname}]", item.Mark);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (selnode.ImageIndex == 16)
                    {
                        selnode.ImageIndex = selnode.SelectedImageIndex = 5;
                    }
                    item.Mark = dlg.InputString;
                    LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<SPInfo>("SPInfo", item);
                    selnode.ToolTipText = item.Mark;
                    selnode.ImageIndex = 13;
                    selnode.SelectedImageIndex = 14;
                    MessageBox.Show("备注成功");
                }
            }

        }

        private void 备注本地ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mark_Local();
        }

        private void MarkResource()
        {
            var currnode = tv_DBServers.SelectedNode;
            if (currnode != null)
            {
                if (currnode.Tag != null && currnode.Tag is TableInfo)
                {
                    var tb = (TableInfo)currnode.Tag;
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), string.Empty }).FirstOrDefault();

                    if (item == null)
                    {
                        item = new MarkObjectInfo { ColumnName = string.Empty, DBName = tb.DBName.ToUpper(), TBName = tb.TBName.ToUpper(), Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                    }
                    InputStringDlg dlg = new InputStringDlg($"备注:{tb.TBName}", item.MarkInfo);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        item.MarkInfo = dlg.InputString;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                        currnode.ToolTipText = item.MarkInfo;
                        MessageBox.Show("备注成功");
                    }
                }
            }
        }


        private void TSMI_MulMarkLocal_Click(object sender, EventArgs e)
        {
            var currnode = tv_DBServers.SelectedNode;
            if (currnode == null)
            {
                return;
            }
            MultiInputDlg dlg = new MultiInputDlg();
            dlg.Text = "批量注释";
            dlg.Moke = "每行一个，示例：列名#####说明文字";
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var tbname = GetTBName(currnode)?.ToUpper();
            if (string.IsNullOrWhiteSpace(tbname))
            {
                return;
            }

            var dbname = GetDBName(currnode).ToUpper();

            var allnodes = currnode.Parent.Nodes;
            var dic = new Dictionary<string, TreeNode>();
            foreach (TreeNode node in allnodes)
            {
                if (node.Tag is TBColumn)
                {
                    if (string.IsNullOrWhiteSpace(node.ToolTipText))
                    {
                        var colinfo = (TBColumn)node.Tag;
                        dic.Add(colinfo.Name.ToUpper(), node);
                    }
                }
            }

            int scount = 0;

            foreach (var ln in dlg.InputString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var arr = ln.Split(new[] { "#####" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 1)
                {
                    var mark = arr[1];
                    if (string.IsNullOrWhiteSpace(arr[1]))
                    {
                        continue;
                    }
                    var column = arr[0].Trim().ToUpper();

                    if (dic.ContainsKey(column))
                    {
                        var tb = (TBColumn)dic[column].Tag;
                        var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname, tbname, column }).FirstOrDefault();

                        if (item == null)
                        {
                            item = new MarkObjectInfo { ColumnName = column, DBName = dbname, TBName = tbname, Servername = GetDBSource(currnode).ServerName, MarkInfo = string.Empty };
                        }

                        item.MarkInfo = mark;
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Upsert<MarkObjectInfo>("MarkObjectInfo", item);
                        dic[column].ToolTipText = mark;

                        if (dic[column].ImageIndex == 18)
                        {
                            dic[column].ImageIndex = dic[column].SelectedImageIndex = 5;
                        }

                        scount++;
                        //MessageBox.Show("备注成功");

                    }
                }
            }


            MessageBox.Show($"备注成功{scount}条");
        }

        private void ClearMarkResource()
        {
            var currnode = tv_DBServers.SelectedNode;
            if (currnode != null)
            {
                if (currnode.Tag != null && currnode.Tag is TableInfo)
                {
                    if (MessageBox.Show("要清理无效字段吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                    var cols = MySQLHelper.GetColumns(GetDBSource(currnode), currnode.Parent.Text, currnode.Name).ToList();
                    var tb = (TableInfo)currnode.Tag;
                    var markedcolumns = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), LJC.FrameWorkV3.Data.EntityDataBase.Consts.STRINGCOMPAIRMIN },
                        new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper(), LJC.FrameWorkV3.Data.EntityDataBase.Consts.STRINGCOMPAIRMAX }, 1, int.MaxValue);

                    foreach (var col in markedcolumns)
                    {
                        if (!cols.Any(p => p.Name.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase)))
                        {
                            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Delete<MarkObjectInfo>("MarkObjectInfo", col.ID);
                        }
                    }
                    var columnMarkSyncRecorditem = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", "keys", new[] { tb.DBName.ToUpper(), tb.TBName.ToUpper() }).ToList();
                    foreach (var rec in columnMarkSyncRecorditem)
                    {
                        LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Delete<ColumnMarkSyncRecord>("ColumnMarkSyncRecord", rec.ID);
                    }

                    MessageBox.Show("清理成功");
                }
            }
        }


        private void OnViewTables()
        {
            var selnode = tv_DBServers.SelectedNode;
            if (this.OnViewTable != null && selnode != null)
            {
                var dbname = GetDBName(selnode);
                DataTable tb = Biz.Common.Data.MySQLHelper.GetTBs(GetDBSource(selnode), dbname);

                StringBuilder sb = new StringBuilder("<html>");
                sb.Append("<head>");
                sb.Append($"<title>查看{dbname}的库表</title>");
                sb.Append(@"<style>
 table {{
width:98%;
font-family: verdana,arial,sans-serif;
font-size:11px;
color:#333333;
border-width: 1px;
border-color: #666666;
border-collapse: collapse;
}}
table th {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #dedede;
}}
table td {{
border-width: 1px;
padding: 8px;
border-style: solid;
border-color: #666666;
background-color: #ffffff;
}}</style>");
                sb.Append("</head>");
                sb.Append(@"<body>
                  <script>
                      function k(){
                          if (event.keyCode == 13) s();
                      }
                      function s(){
                       var w=document.getElementById('w').value
                       if(/^\s*$/.test(w)){
                           var idx=1
                           var trs= document.getElementsByTagName('tr');
                           for(var i=0;i<trs.length;i++){
                               trs[i].style.display=''
                               if(trs[i].firstChild.tagName=='TD')
                                   trs[i].firstChild.innerText=idx++
                            }
                           return
                       }
                       var idx=1;
                       var tds= document.getElementsByTagName('td');
                       w=w.toUpperCase();
                       for(var i=0;i<tds.length;i+=3){
                           var boo=tds[i+1].innerText.toUpperCase().indexOf(w)>-1||tds[i+2].innerText.toUpperCase().indexOf(w)>-1
                           tds[i].parentNode.style.display=boo?'':'none'
                           if(boo) tds[i].innerText=idx++
                       }
                   }
                  </script>");
                sb.Append("<input id='w' type='text' style='height:23px; line-height:23px;' onkeypress='k()' value=''/><input type='button' style='font-size:12px; height:23px; line-height:18px;' value='搜索'  onclick='s()'/>");
                sb.Append("<p/>");
                sb.Append("<table>");
                sb.Append("<tr><th>序号</th><th>表名</th><th>描述</th></tr>");
                int i = 1;
                foreach (DataRow row in tb.Rows)
                {
                    var name = (string)row["name"];
                    var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbname.ToUpper(), name.ToUpper(), string.Empty }).FirstOrDefault();

                    sb.Append($"<tr><td>{i++}</td><td>{name}</td><td>{(item == null ? string.Empty : item.MarkInfo)}</td></tr>");
                }
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                this.OnViewTable(dbname, sb.ToString());
            }

        }
    }
}
