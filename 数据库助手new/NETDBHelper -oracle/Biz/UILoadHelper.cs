using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Data;
using System.Threading;
using Biz.Common.Data;

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
            var tb = OracleHelper.GetDBs(server);
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

                        var seqparentnode = new TreeNode("序列", 1, 1);
                        seqparentnode.Tag = new NodeContents(NodeContentType.SEQUENCEParent);
                        serverNode.Nodes.Insert(4, seqparentnode);

                        var viewparentnode = new TreeNode("视图", 1, 1);
                        viewparentnode.Tag = new NodeContents(NodeContentType.VIEWParent);
                        serverNode.Nodes.Insert(1, viewparentnode);

                        var rviewparentnode = new TreeNode("物化视图", 1, 1);
                        rviewparentnode.Tag = new NodeContents(NodeContentType.RVIEWParent);
                        serverNode.Nodes.Insert(2, rviewparentnode);

                        var procparentnode = new TreeNode("存储过程", 1, 1);
                        procparentnode.Tag = new NodeContents(NodeContentType.PROCParent);
                        serverNode.Nodes.Insert(0, procparentnode);

                        var jobparentnode = new TreeNode("作业", 1, 1);
                        jobparentnode.Tag = new NodeContents(NodeContentType.JOBParent);
                        serverNode.Nodes.Insert(3, jobparentnode);

                        var userparentnode = new TreeNode("用户", 1, 1);
                        userparentnode.Tag = new NodeContents(NodeContentType.USERParent);
                        serverNode.Nodes.Insert(5, userparentnode);

                        serverNode.Expand();
                    }
                }));
        }

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server, string dbname, Func<string, string> gettip)
        {
            dbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource, string, Func<string, string>>(LoadTBs).BeginInvoke(parent, dbNode, server, dbname, gettip, null, null);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn, string> gettip)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource, Func<TBColumn, string>>(LoadColumns).BeginInvoke(parent, tbNode, server, gettip, null, null);
        }

        public static void LoadProcedureAnsy(Form parent,TreeNode procedureNode,DBSource server)
        {
            procedureNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadProcedure).BeginInvoke(parent, procedureNode, server, null, null);
        }

        public static void LoadIndexAnsy(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource,string>(LoadIndexs).BeginInvoke(parent, tbNode, server,dbname, null, null);
        }

        private static void LoadIndexs(Form parent, TreeNode tbNode, DBSource server, string dbname)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetIndexs(server, dbname, tbNode.Parent.Name);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                var imageindex = item.IndexName.Equals("primary", StringComparison.OrdinalIgnoreCase) ? 8 : 7;
                TreeNode newNode = new TreeNode(item.IndexName.ToLower(), item.Cols.Select(p =>
                {
                    var node = new TreeNode
                    {
                        Text = p.Col.ToLower(),
                        ImageIndex = imageindex,
                        SelectedImageIndex = imageindex,
                        Name = p.Col
                    };
                    node.Tag = new IndexColumnInfo
                    {
                        Name = p.Col
                    };
                    return node;
                }).ToArray());
                newNode.Name = item.IndexName;
                newNode.Tag = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 6;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        public static void LoadTriggersAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadTriggers).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadTriggers(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetTriggers(server, tbNode.Parent.Parent.Name, tbNode.Parent.Name);
            List<TreeNode> treeNodes = new List<TreeNode>();
            //MessageBox.Show(list.Count().ToString());
            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item);
                newNode.Name = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 11;

                treeNodes.Add(newNode);
            }

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Expand();
            }
        }

        public static void LoadViewAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadView).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadView(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetViews(server, tbNode.Parent.Parent.Name, tbNode.Parent.Name);
            List<TreeNode> treeNodes = new List<TreeNode>();
            //MessageBox.Show(list.Count().ToString());
            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item);
                newNode.Name = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 3;

                treeNodes.Add(newNode);
            }

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Expand();
            }
        }

        public static void LoadMViewAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadMView).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadMView(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetMViews(server, tbNode.Parent.Parent.Name, tbNode.Parent.Name);
            List<TreeNode> treeNodes = new List<TreeNode>();
            //MessageBox.Show(list.Count().ToString());
            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item);
                newNode.Name = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 3;

                treeNodes.Add(newNode);
            }

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Expand();
            }
        }

        public static void LoadJobsAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadJobs).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadJobs(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetJobs(server);
            List<TreeNode> treeNodes = new List<TreeNode>();
            //MessageBox.Show(list.Count().ToString());
            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item);
                newNode.Name = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 13;

                treeNodes.Add(newNode);
            }

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Expand();
            }
        }

        public static void LoadSeqsAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadSeqs).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadSeqs(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetSeqs(server);
            List<TreeNode> treeNodes = new List<TreeNode>();
            //MessageBox.Show(list.Count().ToString());
            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item);
                newNode.Name = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 14;

                treeNodes.Add(newNode);
            }

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Expand();
            }
        }

        public static void LoadUsersAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadUsers).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadUsers(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetUsers(server).ToList();
            List<TreeNode> treeNodes = new List<TreeNode>();
            //MessageBox.Show(list.Count().ToString());
            foreach (var item in list)
            {
                TreeNode newNode = new TreeNode(item);
                newNode.Name = item;

                newNode.ImageIndex = newNode.SelectedImageIndex = 15;

                treeNodes.Add(newNode);
            }

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Expand();
            }
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
            foreach (TBColumn col in Biz.Common.Data.OracleHelper.GetColumns(server, tb.DBName, tb.TBName))
            {
                int imgIdx = (col.IsID && col.IsKey) ? 9 : (col.IsKey ? 4 : (col.IsID ? 10 : 5));
                TreeNode newNode = new TreeNode(string.Concat(col.Name.ToLower(), "(", Common.Data.Common.OracleTypeToNetType(col.TypeName), ")"), imgIdx, imgIdx);
                newNode.Name = col.Name;
                newNode.Tag = col;
                newNode.ToolTipText = gettip?.Invoke(col);
                if (string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ToolTipText = col.Description;
                }
                if (!col.IsKey && string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ImageIndex = newNode.SelectedImageIndex = 23;
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
                triggernode.Tag = new NodeContents(NodeContentType.TRIGGERParent);
                tbNode.Nodes.Add(triggernode);
                tbNode.Expand();
            }));

        }

        private static void LoadProcedure(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (string col in Biz.Common.Data.OracleHelper.GetProcList(server).ToList())
            {
                //int imgIdx = col.IsKey ? 4 : 5;
                ProcInfo procInfo = new ProcInfo { Name = col, ProcParamInfos = new List<ProcParamInfo>() };
                TreeNode newNode = new TreeNode(col, 12, 12);
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
            DataTable tb = Biz.Common.Data.OracleHelper.GetTBs(server, dbname);
            var y = from x in tb.AsEnumerable()
                    orderby x["name"]
                    select x;
            if (y.Count() > 0)
            {
                var tb2 = y.CopyToDataTable();
                for (int i = 0; i < tb2.Rows.Count; i++)
                {
                    var tbinfo = new TableInfo
                    {
                        DBName = dbname,
                        TBName = tb2.Rows[i]["name"].ToString()
                    };

                    TreeNode newNode = new TreeNode(tbinfo.TBName.ToLower(), 3, 3);
                    newNode.Tag = tbinfo;
                    if (gettip != null)
                    {
                        newNode.ToolTipText = gettip(tbinfo.TBName);
                    }
                    treeNodes.Add(newNode);
                }
            }

            parent.Invoke(new Action(() =>
            {
                serverNode.Nodes.Clear(); 
                serverNode.Nodes.AddRange(treeNodes.ToArray());

                serverNode.Expand();
            }));

        }
    }
}
