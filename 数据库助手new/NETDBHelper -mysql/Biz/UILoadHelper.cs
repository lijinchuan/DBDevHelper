using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Data;

namespace Biz
{
    public static class UILoadHelper
    {
        private static DataTable SQLServersTB;
        private static void LoadServer(Form parentForm, Action<DataTable> onLoadComplete)
        {
            if (onLoadComplete != null)
            {
                //if (SQLServersTB == null)
                //{
                //    SQLServersTB = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers();
                //}
                DataTable tb = SQLServersTB;
                if (!parentForm.IsDisposed)
                    parentForm.Invoke(onLoadComplete, tb);
            }

        }
        public static void LoadSqlServer(Form parentForm, Action<DataTable> onLoadComplete)
        {
            new Action<Form, Action<DataTable>>(LoadServer).BeginInvoke(parentForm, onLoadComplete, null, null);
        }

        public static void LoadDBsAnsy(Form parent, TreeNode serverNode, DBSource server, AsyncCallback callback, object @object)
        {
            serverNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            serverNode.Expand();
            new Action<Form, TreeNode, DBSource>(LoadDBs).BeginInvoke(parent, serverNode, server, callback, @object);
        }

        private static void LoadDBs(Form parent, TreeNode serverNode, DBSource server)
        {
            var tb = Biz.Common.Data.MySQLHelper.GetDBs(server);

            parent.Invoke(new Action(() =>
                {
                    serverNode.Nodes.Clear();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        DBInfo dbInfo = new DBInfo { DBSource = server, Name = tb.Rows[i]["Name"].ToString() };
                        TreeNode dbNode = new TreeNode(dbInfo.Name, 2, 2);
                        var item = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<MarkObjectInfo>("MarkObjectInfo", "keys", new[] { dbInfo.Name.ToUpper(), string.Empty, string.Empty }).FirstOrDefault();
                        if (item != null)
                        {
                            dbNode.ToolTipText = item.MarkInfo;
                        }
                        dbNode.Tag = dbInfo;
                        serverNode.Nodes.Add(dbNode);

                        var tbnode = new TreeNode("表", 1, 1);
                        tbnode.Tag = new NodeContents(NodeContentType.TBParent);
                        dbNode.Nodes.Add(tbnode);
                        var viewnode = new TreeNode("视图", 1, 1);
                        viewnode.Tag = new NodeContents(NodeContentType.VIEWParent);
                        dbNode.Nodes.Add(viewnode);
                        var procnode = new TreeNode("存储过程", 1, 1);
                        procnode.Tag = new NodeContents(NodeContentType.PROCParent);
                        dbNode.Nodes.Add(procnode);
                        var funnode = new TreeNode("函数", 1, 1);
                        funnode.Tag = new NodeContents(NodeContentType.FUNPARENT);
                        dbNode.Nodes.Add(funnode);
                        var logicmapnode = new TreeNode("逻辑关系图", 1, 1);
                        logicmapnode.Tag = new NodeContents(NodeContentType.LOGICMAPParent);
                        dbNode.Nodes.Add(logicmapnode);

                        serverNode.Expand();
                    }
                }));
        }

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server, string dbname,Func<string, string> gettip, AsyncCallback callback, object @object)
        {
            dbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            dbNode.Expand();
            new Action<Form, TreeNode, DBSource, string, Func<string, string>>(LoadTBs).BeginInvoke(parent, dbNode, server, dbname, gettip, callback, @object);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn, string> gettip)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, Func<TBColumn, string>>(LoadColumns).BeginInvoke(parent, tbNode, server,gettip, null, null);
        }

        public static void LoadProcedureAnsy(Form parent,TreeNode procedureNode,DBSource server)
        {
            procedureNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            procedureNode.Expand();
            new Action<Form, TreeNode, DBSource>(LoadProcedure).BeginInvoke(parent, procedureNode, server, null, null);
        }
        public static void LoadFunctionsAnsy(Form parent, TreeNode functionNode, DBSource server, Func<string, string> gettip)
        {
            functionNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            functionNode.Expand();
            new Action<Form, TreeNode, DBSource, Func<string, string>>(LoadFunctions).BeginInvoke(parent, functionNode, server, gettip, null, null);
        }

        private static void LoadFunctions(Form parent, TreeNode tbNode, DBSource server, Func<string, string> gettip)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (var kv in Biz.Common.Data.MySQLHelper.GetFunctionsWithParams(server, tbNode.Parent.Text).AsEnumerable().GroupBy(p => p.Field<string>("name")))
            {

                //int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(kv.Key, 13, 14);
                FunInfo funInfo = new FunInfo
                {
                    Name = kv.Key,
                    IsScalar = false,
                    IsTableValue = false,
                    FuncParamInfos = new List<FunParamInfo>()
                };
                newNode.Tag = funInfo;
                foreach (var row in kv)
                {
                    if (!row.IsNull("name"))
                    {
                        var paramliststr = Encoding.UTF8.GetString((byte[])row["param_list"]);
                        if (!string.IsNullOrEmpty(paramliststr))
                        {
                            var paramlist = paramliststr.Split(',');
                            foreach (var p in paramlist)
                            {
                                var funParamInfo = new FunParamInfo();
                                var arr = p.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                //funParamInfo.Len = row.IsNull("length") ? -1 : row.Field<Int16>("length");
                                //funParamInfo.HasDefaultValue = row.Field<int>("has_default_value") == 1;
                                //funParamInfo.IsOutparam = row.Field<bool>("isoutparam");
                                funParamInfo.Name = arr[0];
                                funParamInfo.TypeName = arr[1];
                                var node = new TreeNode($"{funParamInfo.Name}({funParamInfo.TypeName})",11,11);
                                node.Tag = funParamInfo;
                                newNode.Nodes.Add(node);
                                funInfo.FuncParamInfos.Add(funParamInfo);
                            }
                        }
                    }
                }

                if (gettip != null)
                {
                    var tiptext = gettip(kv.Key);
                    if (string.IsNullOrWhiteSpace(tiptext))
                    {
                        newNode.ImageIndex = 19;
                        newNode.SelectedImageIndex = 20;
                    }
                    else
                    {
                        newNode.ToolTipText = tiptext;
                    }
                }

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        public static void LoadIndexAnsy(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, string>(LoadIndexs).BeginInvoke(parent, tbNode, server, dbname, null, null);
        }

        private static void LoadIndexs(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.MySQLHelper.GetIndexs(server, dbname, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item.IndexName, item.Cols.Select(p =>
                {
                    var node = new TreeNode
                    {
                        Text = p.Col
                    };
                    if (p.IsInclude)
                    {
                        node.ImageKey = node.SelectedImageKey = "plugin";
                    }
                    else if (item.IndexName.Equals("primary", StringComparison.OrdinalIgnoreCase))
                    {
                        node.ImageIndex = node.SelectedImageIndex = 8;
                    }
                    else if (p.IsDesc)
                    {
                        node.ImageKey = node.SelectedImageKey = "DESC";
                    }
                    else
                    {
                        node.ImageKey = node.SelectedImageKey = "ASC";
                    }


                    node.Tag = new IndexColumnInfo
                    {
                        Name = p.Col
                    };

                    return node;

                }).ToArray());

                newNode.Tag = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 6;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        public static void LoadTriggersAnsy(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, string>(LoadTriggers).BeginInvoke(parent, tbNode, server, dbname, null, null);
        }

        private static void LoadTriggers(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.MySQLHelper.GetTriggers(server, dbname, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode($"{item.TriggerName}");

                newNode.Tag = item;

                if (item.ExecIsTriggerDisabled)
                {
                    newNode.ImageIndex = newNode.SelectedImageIndex = 30;
                }
                else
                {
                    newNode.ImageIndex = newNode.SelectedImageIndex = 29;
                }

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        public static void LoadViewsAnsy(Form parent, TreeNode tbNode, DBSource server, Func<ViewInfo, string> getviewtip, Func<ViewColumn, string> gettip)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, Func<ViewInfo, string>, Func<ViewColumn, string>>(LoadViews).BeginInvoke(parent, tbNode, server, getviewtip, gettip, null, null);
        }

        private static void LoadViews(Form parent, TreeNode tbNode, DBSource server, Func<ViewInfo, string> getviewtip, Func<ViewColumn, string> gettip)
        {
            if (server == null)
            {
                return;
            }

            var list = Common.Data.MySQLHelper.GetViews(server, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item.Key, item.Value.Select(p => {
                    var node = new TreeNode
                    {
                        Text = p.Name + "(" + p.TypeName + (p.Length == -1 ? "" : ("(" + p.Length + ")")) + ")",
                        ImageIndex = 5,
                        SelectedImageIndex = 5,
                        Tag = p
                    };

                    node.ToolTipText = gettip(p);
                    if (string.IsNullOrWhiteSpace(node.ToolTipText))
                    {
                        node.ImageIndex = node.SelectedImageIndex = 18;
                    }

                    return node;
                }).ToArray());

                newNode.ImageIndex = newNode.SelectedImageIndex = 15;
                var viewInfo = new ViewInfo
                {
                    DBName = tbNode.Parent.Text,
                    Name = item.Key
                };
                newNode.Tag = viewInfo;
                newNode.ToolTipText = getviewtip(viewInfo);
                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        private static void InsertRange(TreeNode node, IEnumerable<TreeNode> nodes)
        {
            if (nodes.Count() == 0)
            {
                return;
            }

            for (int i = 0; i < nodes.Count(); i++)
            {
                node.Nodes.Insert(i, nodes.ElementAt(i));
            }
        }

        private static void LoadColumns(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn, string> gettip)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            var tb = tbNode.Tag as TableInfo;
            foreach (TBColumn col in Biz.Common.Data.MySQLHelper.GetColumns(server, tb.DBName, tb.TBName))
            {
                int imgIdx = (col.IsID && col.IsKey) ? 9 : (col.IsKey ? 4 : (col.IsID ? 10 : 5));
                TreeNode newNode = new TreeNode(string.Concat(col.Name, "(", col.TypeName, ")"), imgIdx, imgIdx);
                newNode.Tag = col;
                newNode.ToolTipText = gettip(col);
                if (string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ToolTipText = col.Description;
                }
                if (!col.IsKey && string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ImageIndex = newNode.SelectedImageIndex = 18;
                }
                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() =>
            {
                tbNode.Nodes.Clear(); 
                InsertRange(tbNode, treeNodes.ToArray());
                var indexnode = new TreeNode("索引", 1, 1);
                indexnode.Tag = new NodeContents(NodeContentType.INDEXParent);
                tbNode.Nodes.Add(indexnode);
                var triggernode = new TreeNode("触发器", 1, 1);
                triggernode.Tag = new NodeContents(NodeContentType.TRIGGERPARENT);
                tbNode.Nodes.Add(triggernode);
                tbNode.Expand();
            }));
        }

        private static void LoadProcedure(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (string col in Biz.Common.Data.MySQLHelper.GetProcedures(server, tbNode.Parent.Text).ToList())
            {
                //int imgIdx = col.IsKey ? 4 : 5;
                ProcInfo procInfo = new ProcInfo { Name = col, ProcParamInfos = new List<ProcParamInfo>() };
                TreeNode newNode = new TreeNode(col, 13, 14);
                newNode.Tag = procInfo;
                treeNodes.Add(newNode);
            }
            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        private static void LoadTBs(Form parent, TreeNode serverNode, DBSource server, string dbname, Func<string, string> gettip)
        {
            //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Text));
            if (server == null)
                return;

            long total = 0;

            List<TreeNode> treeNodes = new List<TreeNode>();
            DataTable tb = Biz.Common.Data.MySQLHelper.GetTBs(server, dbname);
            var y = from x in tb.AsEnumerable()
                    orderby x["name"]
                    select x;
            if (y.Count() == 0)
            {
                parent.Invoke(new Action(() =>
                {
                    serverNode.Nodes.Clear();
                }));
                return;
            }
            var tb2 = y.CopyToDataTable();
            for (int i = 0; i < tb2.Rows.Count; i++)
            {
                var tbinfo = new TableInfo
                {
                    DBName = dbname,
                    TBName = tb2.Rows[i]["name"].ToString()
                };
                var ex = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<RelTable>(nameof(RelTable), "SDT", new[] { dbname.ToLower(), tbinfo.TBName.ToLower() }, new[] { dbname.ToLower(), tbinfo.TBName.ToLower() }, 1, 1, ref total).FirstOrDefault() != null
                         || LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Scan<RelTable>(nameof(RelTable), "SDRT", new[] { dbname.ToLower(), tbinfo.TBName.ToLower() }, new[] { dbname.ToLower(), tbinfo.TBName.ToLower() }, 1, 1, ref total).FirstOrDefault() != null;

                TreeNode newNode = null;
                if (ex)
                {
                    newNode = new TreeNode(tbinfo.TBName, 32, 32);
                }
                else
                {
                    newNode = new TreeNode(tbinfo.TBName, 31, 31);
                }
                newNode.Name = tbinfo.TBName;
                newNode.Tag = tbinfo;
                if (gettip != null)
                {
                    newNode.ToolTipText = gettip(tbinfo.TBName);
                }
                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() =>
            {
                serverNode.Nodes.Clear();
                serverNode.Nodes.AddRange(treeNodes.ToArray());
                serverNode.Expand();
            }));

        }


        public static void LoadLogicMapsAnsy(Form parent, TreeNode tbNode, string dbname)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, string>(LoadLogicMaps).BeginInvoke(parent, tbNode, dbname, null, null);
        }

        public static void LoadLogicMaps(Form parent, TreeNode tbNode, string dbname)
        {
            var logicmaplist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<LogicMap>(nameof(LogicMap),
                p => p.DBName.Equals(dbname, StringComparison.OrdinalIgnoreCase)).ToList();


            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in logicmaplist)
            {
                TreeNode newNode = new TreeNode(item.LogicName);

                newNode.ImageIndex = newNode.SelectedImageIndex = 25;

                newNode.Tag = item;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));

        }
    }
}
