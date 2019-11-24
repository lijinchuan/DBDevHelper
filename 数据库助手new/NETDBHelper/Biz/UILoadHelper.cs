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

        public static void LoadTBsAnsy(Form parent, TreeNode dbNode, DBSource server,Func<string,string> gettip)
        {
            dbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            dbNode.Expand();
            new Action<Form, TreeNode, DBSource,Func<string,string>>(LoadTBs).BeginInvoke(parent, dbNode, server,gettip, null, null);
        }

        public static void LoadColumnsAnsy(Form parent, TreeNode tbNode, DBSource server, Func<TBColumn,string> gettip)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource, Func<TBColumn,string>>(LoadColumns).BeginInvoke(parent, tbNode, server,gettip, null, null);
        }

        private static void LoadColumns(Form parent, TreeNode tbNode, DBSource server,Func<TBColumn,string> gettip)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            
            foreach (TBColumn col in Biz.Common.Data.SQLHelper.GetColumns(server, tbNode.Parent.Text, tbNode.Name, tbNode.Text))
            {
                int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(string.Concat(col.Name, "(", col.TypeName, ")"), imgIdx, imgIdx);
                newNode.Tag = col;

                newNode.ToolTipText = gettip(col);
                if (string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ToolTipText = col.Description;
                }
                if (!col.IsKey&& string.IsNullOrWhiteSpace(newNode.ToolTipText))
                {
                    newNode.ImageIndex = newNode.SelectedImageIndex = 18;
                }
                treeNodes.Add(newNode);
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Nodes.Add("INDEXS", "索引", 1, 1); tbNode.Expand(); tbNode.Expand(); }));
            }
            else
            {
                tbNode.Nodes.Clear();
                tbNode.Nodes.AddRange(treeNodes.ToArray());
                tbNode.Nodes.Add("INDEXS", "索引", 1, 1);
                tbNode.Expand();
            }
        }

        private static void LoadTBs(Form parent, TreeNode serverNode, DBSource server,Func<string,string> gettip)
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

                newNode.Tag = new TableInfo
                {
                    DBName=serverNode.Text,
                    TBId= tb2.Rows[i]["id"].ToString(),
                    TBName=tb2.Rows[i]["name"].ToString()
                };
                if (gettip != null)
                {
                    newNode.ToolTipText = gettip(tb2.Rows[i]["name"].ToString());
                }
                treeNodes.Add(newNode);
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => 
                { serverNode.Nodes.Clear();
                    serverNode.Nodes.AddRange(treeNodes.ToArray());
                    serverNode.Nodes.Add("VIEW", "视图", 1, 1);
                    serverNode.Nodes.Add("PROCEDURE", "存储过程", 1, 1);
                    serverNode.Expand(); }));
            }
            else
            {
                serverNode.Nodes.Clear();
                serverNode.Nodes.AddRange(treeNodes.ToArray());
                serverNode.Nodes.Add("VIEW", "视图", 1, 1);
                serverNode.Nodes.Add("PROCEDURE", "存储过程", 1, 1);
                serverNode.Expand();
            }
        }

        private static void LoadProcedure(Form parent, TreeNode tbNode, DBSource server,Func<string,string> gettip)
        {
            if (server == null)
                return;
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (var kv in Biz.Common.Data.SQLHelper.GetProceduresWithParams(server, tbNode.Parent.Text).AsEnumerable().GroupBy(p=>p.Field<string>("name")))
            {
                
                //int imgIdx = col.IsKey ? 4 : 5;
                TreeNode newNode = new TreeNode(kv.Key, 13, 14);
                newNode.Tag = kv;
                var pname = string.Empty;
                foreach(var row in kv)
                {
                    if (!row.IsNull("pname"))
                    {
                        var len = row.IsNull("length") ? -1 : row.Field<Int16>("length");
                        var isnullable = row.Field<int>("isnullable") == 1;
                        var isoutparam = row.Field<int>("isoutparam") == 1;
                        newNode.Nodes.Add(row.Field<string>("pname"), $"{row.Field<string>("pname")}({row.Field<string>("tpname")}{(len == -1 ? string.Empty : "(" + len.ToString() + ")")}{(isnullable ? " null" : "")}{(isoutparam ? " output" : "")})", isoutparam ? 12 : 11, isoutparam ? 12 : 11);
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

        public static void LoadProcedureAnsy(Form parent, TreeNode procedureNode, DBSource server,Func<string,string> gettip)
        {
            procedureNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            procedureNode.Expand();
            new Action<Form, TreeNode, DBSource,Func<string,string>>(LoadProcedure).BeginInvoke(parent, procedureNode, server, gettip,null, null);
        }

        public static void LoadIndexAnsy(Form parent, TreeNode tbNode, DBSource server)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();
            new Action<Form, TreeNode, DBSource>(LoadIndexs).BeginInvoke(parent, tbNode, server, null, null);
        }

        private static void LoadIndexs(Form parent, TreeNode tbNode, DBSource server)
        {
            if (server == null)
            {
                return;
            }

            var list = Biz.Common.Data.SQLHelper.GetIndexs(server, tbNode.Parent.Parent.Text, tbNode.Parent.Text);
            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in list)
            {
                var imageindex = item.IsPri ? 8 : 7;
                TreeNode newNode = new TreeNode($"{item.IndexName}{(item.IsClustered?"(聚集)":"")}", item.Cols.Select(p => new TreeNode
                {
                    Text = $"{p.Col}{(p.IsDesc?"(倒序)":"")}{(p.IsInclude?"(包含)":"")}",
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
                    Text = p.Name+"("+p.TypeName+(p.Length==-1?"":("("+p.Length+")"))+")",
                    ImageIndex = 5,
                    SelectedImageIndex = 5,
                    Tag=p
                }).ToArray());

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
    }
}
