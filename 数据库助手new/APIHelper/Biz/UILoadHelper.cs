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

                newNode.ImageIndex = newNode.SelectedImageIndex = 21;

                newNode.Tag = item;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));

        }
    }
}
