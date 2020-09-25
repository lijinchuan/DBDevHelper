using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Data;
using System.Threading;

namespace Biz
{
    public static class UILoadHelper
    {
        private static DataTable SQLServersTB;
        private static void LoadServer(Form parentForm, Action<DataTable> onLoadComplete)
        {
            if (onLoadComplete != null)
            {
                if (SQLServersTB == null)
                {
                    SQLServersTB = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers();
                }
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
            var tb = Biz.Common.Data.SQLHelper.GetDBs(server);

            parent.Invoke(new Action(() =>
            {
                serverNode.Nodes.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DBInfo dbInfo = new DBInfo { DBSource = server, Name = tb.Rows[i]["Name"].ToString() };
                    TreeNode dbNode = new TreeNode(dbInfo.Name, 2, 2);
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

                    serverNode.Expand();
                }
            }));

        }

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server, string dbname, Func<string, string> gettip)
        {
            dbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            dbNode.Expand();
            new Action<Form, TreeNode, DBSource, string, Func<string, string>>(LoadTBs).BeginInvoke(parent, dbNode, server, dbname, gettip, null, null);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn,string> gettip)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, Func<TBColumn,string>>(LoadColumns).BeginInvoke(parent, tbNode, server,gettip, null, null);
        }

        private static void LoadColumns(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn, string> gettip)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            var tb = tbNode.Tag as TableInfo;
            foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(server, tb.DBName, tb.TBId, tb.TBName))
            {
                int imgIdx = col.IsKey ? 4 : 5;
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
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                var indexnode = new TreeNode("索引", 1, 1);
                indexnode.Tag = new NodeContents(NodeContentType.INDEXParent);
                tbNode.Nodes.Add(indexnode);
                tbNode.Expand();
            }));

        }

        private static void LoadTBs(Form parent, TreeNode serverNode, DBSource server,string dbname, Func<string, string> gettip)
        {
            //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Text));
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            DataTable tb = Biz.Common.Data.SQLHelper.GetTBs(server, dbname);
            var y = from x in tb.AsEnumerable()
                    orderby x["name"]
                    select x;
            if (y.Count() == 0)
            {
                parent.Invoke(new Action(() => serverNode.Nodes.Clear()));
                return;
            }
            var tb2 = y.CopyToDataTable();
            for (int i = 0; i < tb2.Rows.Count; i++)
            {
                var tbinfo = new TableInfo
                {
                    DBName = dbname,
                    TBId = tb2.Rows[i]["id"].ToString(),
                    TBName = tb2.Rows[i]["name"].ToString()
                };
                TreeNode newNode = new TreeNode(tbinfo.TBName, 3, 3);

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

        private static void LoadProcedure(Form parent, TreeNode tbNode, DBSource server, Func<string, string> gettip)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (var kv in Biz.Common.Data.SQLHelper.GetProceduresWithParams(server, tbNode.Parent.Text).AsEnumerable().GroupBy(p => p.Field<string>("name")))
            {

                //int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(kv.Key, 13, 14);
                ProcInfo procInfo = new ProcInfo { Name = kv.Key, ProcParamInfos = new List<ProcParamInfo>() };
                newNode.Tag = procInfo;
                var pname = string.Empty;
                foreach (var row in kv)
                {
                    if (!row.IsNull("pname"))
                    {
                        var procParamInfo = new ProcParamInfo();
                        procParamInfo.Len = row.IsNull("length") ? -1 : row.Field<Int16>("length");
                        procParamInfo.IsNullable = row.Field<int>("isnullable") == 1;
                        procParamInfo.IsOutparam = row.Field<int>("isoutparam") == 1;
                        procParamInfo.Name = row.Field<string>("pname");
                        procParamInfo.TypeName = row.Field<string>("tpname");
                        var node = new TreeNode($"{procParamInfo.Name}({row.Field<string>("tpname")}{(procParamInfo.Len == -1 ? string.Empty : "(" + procParamInfo.Len.ToString() + ")")}{(procParamInfo.IsNullable ? " null" : "")}{(procParamInfo.IsOutparam ? " output" : "")})", procParamInfo.IsOutparam ? 12 : 11, procParamInfo.IsOutparam ? 12 : 11);
                        node.Tag = procParamInfo;
                        newNode.Nodes.Add(node);
                        procInfo.ProcParamInfos.Add(procParamInfo);
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

        public static void LoadProcedureAnsy(Form parent, TreeNode procedureNode, DBSource server,Func<string,string> gettip)
        {
            procedureNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            procedureNode.Expand();
            new Action<Form, TreeNode, DBSource,Func<string,string>>(LoadProcedure).BeginInvoke(parent, procedureNode, server, gettip,null, null);
        }

        public static void LoadIndexAnsy(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, string>(LoadIndexs).BeginInvoke(parent, tbNode, server, dbname, null, null);
        }

        private static void LoadIndexs(Form parent, TreeNode tbNode,DBSource server,string dbname)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.SQLHelper.GetIndexs(server, dbname, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                var imageindex = item.IsPri ? 8 : 7;
                TreeNode newNode = new TreeNode($"{item.IndexName}{(item.IsClustered ? "(聚集)" : "")}", item.Cols.Select(p =>
                {
                    var node = new TreeNode
                    {
                        Text = $"{p.Col}{(p.IsDesc ? "(倒序)" : "")}{(p.IsInclude ? "(包含)" : "")}",
                        ImageIndex = imageindex,
                        SelectedImageIndex = imageindex
                    };

                    node.Tag = p;

                    return node;
                }).ToArray());

                newNode.Tag = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 6;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        public static void LoadViewsAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource>(LoadViews).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadViews(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.SQLHelper.GetViews(server, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item.Key, item.Value.Select(p => new TreeNode
                {
                    Text = p.Name + "(" + p.TypeName + (p.Length == -1 ? "" : ("(" + p.Length + ")")) + ")",
                    ImageIndex = 5,
                    SelectedImageIndex = 5,
                    Tag = p
                }).ToArray());

                newNode.ImageIndex = newNode.SelectedImageIndex = 15;

                newNode.Tag = new ViewInfo
                {
                    DBName = tbNode.Parent.Text,
                    Name = item.Key
                };

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }
    }
}
