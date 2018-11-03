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
            new Action<Form, TreeNode, DBSource>(LoadDBs).BeginInvoke(parent, serverNode, server, null, null);
        }

        private static void LoadDBs(Form parent, TreeNode serverNode, DBSource server)
        {
            var tb= Biz.Common.Data.MySQLHelper.GetDBs(server);
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() =>
                    {
                        serverNode.Nodes.Clear();
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            TreeNode tbNode = new TreeNode(tb.Rows[i]["Name"].ToString(), 2, 2);
                            serverNode.Nodes.Add(tbNode);
                        }
                        
                        
                    }));
            }
            else
            {
                serverNode.Nodes.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    TreeNode tbNode = new TreeNode(tb.Rows[i]["Name"].ToString(), 2, 2);
                    serverNode.Nodes.Add(tbNode);
                }
            }
        }

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server)
        {
            dbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadTBs).BeginInvoke(parent, dbNode, server, null, null);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadColumns).BeginInvoke(parent, tbNode, server, null, null);
        }

        public static void LoadProcedureAnsy(Form parent,TreeNode procedureNode,DBSource server)
        {
            procedureNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadProcedure).BeginInvoke(parent, procedureNode, server, null, null);
        }

        public static void LoadIndexAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中..."));
            Thread.Sleep(100);
            new Action<Form, TreeNode, DBSource>(LoadIndexs).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadIndexs(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.OracleHelper.GetIndexs(server, tbNode.Parent.Parent.Name, tbNode.Parent.Name);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                var imageindex = item.IndexName.Equals("primary", StringComparison.OrdinalIgnoreCase) ? 8 : 7;
                TreeNode newNode = new TreeNode(item.IndexName.ToLower(), item.Cols.Select(p => new TreeNode
                {
                    Text = p.ToLower(),
                    ImageIndex = imageindex,
                    SelectedImageIndex = imageindex,
                    Name=p
                }).ToArray());
                newNode.Name = item.IndexName;

                newNode.ImageIndex = newNode.SelectedImageIndex = 6;

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

        private static void LoadColumns(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (TBColumn col in Biz.Common.Data.OracleHelper.GetColumns(server, tbNode.Parent.Name, tbNode.Name))
            {
                int imgIdx = (col.IsID && col.IsKey) ? 9 : (col.IsKey ? 4 : (col.IsID ? 10 : 5));
                TreeNode newNode = new TreeNode(string.Concat(col.Name.ToLower(), "(",Common.Data.Common.OracleTypeToNetType(col.TypeName), ")"), imgIdx, imgIdx);
                newNode.Name = col.Name;
                newNode.Tag = col;
                treeNodes.Add(newNode);
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() =>
                {
                    tbNode.Nodes.Clear(); InsertRange(tbNode, treeNodes.ToArray());
                    if (tbNode.Level == 3)
                    {
                        tbNode.Nodes.Add("INDEXS", "索引", 1, 1);
                        tbNode.Nodes.Add("TRIGGER", "触发器", 1, 1);
                    }
                    tbNode.Expand();
                }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                if (tbNode.Level == 3)
                {
                    tbNode.Nodes.Add("INDEXS", "索引", 1, 1);
                    tbNode.Nodes.Add("TRIGGER", "触发器", 1, 1);
                }
                tbNode.Expand();
            }
        }

        private static void LoadProcedure(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (string col in Biz.Common.Data.OracleHelper.GetProcList(server).ToList())
            {
                //int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(col, 12, 12);
                newNode.Tag = col;
                newNode.Name = col;
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

        private static void LoadTBs(Form parent, TreeNode serverNode, DBSource server)
        {
            //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Text));
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            DataTable tb = Biz.Common.Data.OracleHelper.GetTBs(server, serverNode.Text);
            var y = from x in tb.AsEnumerable()
                    orderby x["name"]
                    select x;
            if (y.Count() > 0)
            {
                var tb2 = y.CopyToDataTable();
                for (int i = 0; i < tb2.Rows.Count; i++)
                {
                    TreeNode newNode = new TreeNode(tb2.Rows[i]["name"].ToString().ToLower(), 3, 3);
                    newNode.Name = tb2.Rows[i]["name"].ToString();
                    treeNodes.Add(newNode);
                }
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { serverNode.Nodes.Clear(); serverNode.Nodes.AddRange(treeNodes.ToArray());
                    serverNode.Nodes.Insert(0,"PROCEDURE", "存储过程", 1, 1);
                    serverNode.Nodes.Insert(1,"VIEW", "视图", 1, 1);
                    serverNode.Nodes.Insert(2,"MVIEW", "物化视图", 1, 1);
                    serverNode.Nodes.Insert(3,"JOBS", "作业", 1, 1);
                    serverNode.Nodes.Insert(4, "SEQUENCE", "序列", 1, 1);
                    serverNode.Expand(); }));
            }
            else
            {
                serverNode.Nodes.Clear();
                serverNode.Nodes.AddRange(treeNodes.ToArray());
                serverNode.Nodes.Insert(0, "PROCEDURE", "存储过程", 1, 1);
                serverNode.Nodes.Insert(1, "VIEW", "视图", 1, 1);
                serverNode.Nodes.Insert(2, "MVIEW", "物化视图", 1, 1);
                serverNode.Nodes.Insert(3, "JOBS", "作业", 1, 1);
                serverNode.Nodes.Insert(4, "SEQUENCE", "序列", 1, 1);
                serverNode.Expand();
            }
        }
    }
}
