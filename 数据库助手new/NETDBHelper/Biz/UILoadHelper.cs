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
            var tb= Biz.Common.Data.SQLHelper.GetDBs(server);
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
            new Action<Form, TreeNode, DBSource>(LoadTBs).BeginInvoke(parent, dbNode, server, null, null);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            new Action<Form, TreeNode, DBSource>(LoadColumns).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadColumns(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(server, tbNode.Parent.Text, tbNode.Name, tbNode.Text))
            {
                int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(string.Concat(col.Name, "(", col.TypeName, ")"), imgIdx, imgIdx);
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

        private static void LoadTBs(Form parent, TreeNode serverNode, DBSource server)
        {
            //var server = DBServers.FirstOrDefault(p => p.ServerName.Equals(e.Node.Parent.Text));
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            DataTable tb = Biz.Common.Data.SQLHelper.GetTBs(server, serverNode.Text);
            var y = from x in tb.AsEnumerable()
                    orderby x["name"]
                    select x;
            if (y.Count() == 0)
                return;
            var tb2= y.CopyToDataTable();
            for (int i = 0; i < tb2.Rows.Count; i++)
            {
                TreeNode newNode = new TreeNode(tb2.Rows[i]["name"].ToString(), 3, 3);
                newNode.Name = tb2.Rows[i]["id"].ToString();
                treeNodes.Add(newNode);
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { serverNode.Nodes.Clear(); serverNode.Nodes.AddRange(treeNodes.ToArray()); serverNode.Expand(); }));
            }
            else
            {
                serverNode.Nodes.Clear();
                serverNode.Nodes.AddRange(treeNodes.ToArray());
                serverNode.Expand();
            }
        }
    }
}
