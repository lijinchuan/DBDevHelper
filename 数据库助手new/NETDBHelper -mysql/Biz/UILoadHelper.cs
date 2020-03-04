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

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server, Func<string, string> gettip)
        {
            new Action<Form, TreeNode, DBSource, Func<string, string>>(LoadTBs).BeginInvoke(parent, dbNode, server,gettip, null, null);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn, string> gettip)
        {
            new Action<Form, TreeNode, DBSource, Func<TBColumn, string>>(LoadColumns).BeginInvoke(parent, tbNode, server,gettip, null, null);
        }

        public static void LoadProcedureAnsy(Form parent,TreeNode procedureNode,DBSource server)
        {
            new Action<Form, TreeNode, DBSource>(LoadProcedure).BeginInvoke(parent, procedureNode, server, null, null);
        }

        public static void LoadIndexAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            new Action<Form, TreeNode, DBSource>(LoadIndexs).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadIndexs(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.MySQLHelper.GetIndexs(server, tbNode.Parent.Parent.Text, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                var imageindex = item.IndexName.Equals("primary", StringComparison.OrdinalIgnoreCase) ? 8 : 7;
                TreeNode newNode = new TreeNode(item.IndexName, item.Cols.Select(p => new TreeNode
                {
                    Text = p,
                    ImageIndex = imageindex,
                    SelectedImageIndex = imageindex
                }).ToArray());

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
            foreach (TBColumn col in Biz.Common.Data.MySQLHelper.GetColumns(server, tbNode.Parent.Text, tbNode.Text))
            {
                int imgIdx = (col.IsID && col.IsKey) ? 9 : (col.IsKey ? 4 : (col.IsID ? 10 : 5));
                TreeNode newNode = new TreeNode(string.Concat(col.Name, "(", col.TypeName, ")"), imgIdx, imgIdx);
                newNode.Tag = col;
                newNode.ToolTipText = string.IsNullOrWhiteSpace(col.Description) ? gettip(col) : col.Description;
                if (!col.IsKey && string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ImageIndex = newNode.SelectedImageIndex = 16;
                }
                treeNodes.Add(newNode);
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() =>
                {
                    tbNode.Nodes.Clear(); InsertRange(tbNode, treeNodes.ToArray());
                    tbNode.Nodes.Add("INDEXS", "索引", 1, 1); tbNode.Expand();
                }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Nodes.Add("INDEXS", "索引", 1, 1);
                tbNode.Expand();
            }
        }

        private static void LoadProcedure(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (string col in Biz.Common.Data.MySQLHelper.GetProcedures(server, tbNode.Parent.Text).ToList())
            {
                //int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(col, 13, 14);
                newNode.Tag = col;
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

        private static void LoadTBs(Form parent, TreeNode serverNode, DBSource server, Func<string, string> gettip)
        {
            //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Text));
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            DataTable tb = Biz.Common.Data.MySQLHelper.GetTBs(server, serverNode.Text);
            var y = from x in tb.AsEnumerable()
                    orderby x["name"]
                    select x;
            if (y.Count() == 0)
                return;
            var tb2= y.CopyToDataTable();
            for (int i = 0; i < tb2.Rows.Count; i++)
            {
                TreeNode newNode = new TreeNode(tb2.Rows[i]["name"].ToString(), 3, 3);
                newNode.Name = tb2.Rows[i]["name"].ToString();
                newNode.Tag = new TableInfo
                {
                    DBName = serverNode.Text,
                    //TBId = tb2.Rows[i]["id"].ToString(),
                    TBName = tb2.Rows[i]["name"].ToString()
                };
                if (gettip != null)
                {
                    newNode.ToolTipText = gettip(tb2.Rows[i]["name"].ToString());
                }
                treeNodes.Add(newNode);
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { serverNode.Nodes.Clear(); serverNode.Nodes.AddRange(treeNodes.ToArray()); serverNode.Nodes.Add("PROCEDURE", "存储过程", 1, 1); serverNode.Expand(); }));
            }
            else
            {
                serverNode.Nodes.Clear();
                serverNode.Nodes.AddRange(treeNodes.ToArray());
                serverNode.Nodes.Add("PROCEDURE", "存储过程", 1, 1);
                serverNode.Expand();
            }
        }
    }
}
