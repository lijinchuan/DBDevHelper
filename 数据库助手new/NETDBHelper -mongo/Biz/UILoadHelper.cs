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

        public static void LoadDBsAnsy(Form parent, TreeNode serverNode, DBSource server)
        {
            serverNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            serverNode.Expand();
            new Action<Form, TreeNode, DBSource>(LoadDBs).BeginInvoke(parent, serverNode, server, null, null);
        }

        private static void LoadDBs(Form parent, TreeNode serverNode, DBSource server)
        {
            var tb = Biz.Common.Data.MongoDBHelper.GetDBs(server);

            parent.Invoke(new Action(() =>
                {
                    serverNode.Nodes.Clear();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        DBInfo dbInfo = new DBInfo { DBSource = server, Name = tb.Rows[i]["Name"].ToString() };
                        TreeNode dbNode = new TreeNode(dbInfo.Name, 2, 2);
                        dbNode.Tag = dbInfo;
                        serverNode.Nodes.Add(dbNode);

                        var tbnode = new TreeNode("集合", 1, 1);
                        tbnode.Tag = new NodeContents(NodeContentType.TBParent);
                        dbNode.Nodes.Add(tbnode);

                        var logicmapnode = new TreeNode("逻辑关系图", 1, 1);
                        logicmapnode.Tag = new NodeContents(NodeContentType.LOGICMAPParent);
                        dbNode.Nodes.Add(logicmapnode);

                        serverNode.Expand();
                    }
                }));
        }

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server, string dbname,Func<string, string> gettip)
        {
            dbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            dbNode.Expand();
            new Action<Form, TreeNode, DBSource, string, Func<string, string>>(LoadTBs).BeginInvoke(parent, dbNode, server, dbname, gettip, null, null);
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

            var list = Biz.Common.Data.MongoDBHelper.GetIndexs(server, dbname, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                var imageindex = item.IndexName.Equals("primary", StringComparison.OrdinalIgnoreCase) ? 8 : 7;

                TreeNode newNode = new TreeNode(item.IndexName, item.Cols.Select(p =>
                {
                    var node = new TreeNode
                    {
                        Text = p.Col,
                        ImageIndex = imageindex,
                        SelectedImageIndex = imageindex
                    };

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
            foreach (TBColumn col in Biz.Common.Data.MongoDBHelper.GetColumns(server, tb.DBName, tb.TBName))
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
            List<TreeNode> treeNodes = new List<TreeNode>();
            DataTable tb = Biz.Common.Data.MongoDBHelper.GetTBs(server, dbname);
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
                TreeNode newNode = new TreeNode(tbinfo.TBName, 3, 3);
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
