using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Data;
using System.Threading;
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace Biz
{
    public static class UILoadHelper
    {
        public static void LoadApiAsync(Form parent, TreeNode tbNode, int apiResourceId)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();

            new Action<Form, TreeNode, int>(LoadApi).BeginInvoke(parent, tbNode, apiResourceId, null, null);
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

        public static void LoadApi(Form parent, TreeNode tbNode,int apiSourceId)
        {

            var apilist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl),
                "SourceId", new object[] { apiSourceId }).ToList();

            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in apilist)
            {
                TreeNode newNode = new TreeNode(item.APIName);

                newNode.ImageIndex = newNode.SelectedImageIndex = 21;

                newNode.Tag = item;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
        }

        public static void LoadApiResurceAsync(Form parent, TreeNode tbNode)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 17, 17));
            tbNode.Expand();

            new Action<Form, TreeNode>(LoadApiResurce).BeginInvoke(parent, tbNode, null, null);
        }

        public static void LoadApiResurce(Form parent,TreeNode pnode)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            var aPISources = BigEntityTableEngine.LocalEngine.List<APISource>(nameof(APISource), 1, int.MaxValue);
            foreach (var s in aPISources)
            {
                TreeNode node = new TreeNodeEx(s.SourceName, 0, 2,0,2);
                var serverinfo = s;
                node.Tag = serverinfo;

                node.Nodes.Add(new TreeNodeEx
                {
                   Text="接口",
                   Tag=new NodeContents(NodeContentType.APIPARENT),
                   ImageIndex=0,
                   SelectedImageIndex=2,
                   CollapseImgIndex=0,
                   ExpandImgIndex=2
                });

                node.Nodes.Add(new TreeNodeEx
                {
                    Text = "环境",
                    Tag = new NodeContents(NodeContentType.ENVPARENT),
                    ImageIndex = 0,
                    SelectedImageIndex = 2,
                    CollapseImgIndex = 0,
                    ExpandImgIndex = 2
                });

                treeNodes.Add(node);
            }
            parent.Invoke(new Action(() => { pnode.Nodes.Clear(); pnode.Nodes.AddRange(treeNodes.ToArray()); pnode.Expand(); }));
        }
    }
}
