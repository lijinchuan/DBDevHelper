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
        public static void LoadApiAsync(Form parent, TreeNode tbNode, int apiResourceId, AsyncCallback callback, object @object)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 3, 3));
            tbNode.Expand();

            new Action<Form, TreeNode, int>(LoadApi).BeginInvoke(parent, tbNode, apiResourceId, callback, @object);
        }

        public static void LoadLogicMapsAnsy(Form parent, TreeNode tbNode, int sourceid,AsyncCallback callback, object @object)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 3, 3));
            tbNode.Expand();
            new Action<Form, TreeNode, int>(LoadLogicMaps).BeginInvoke(parent, tbNode, sourceid, callback, @object);
        }

        public static void LoadLogicMaps(Form parent, TreeNode tbNode, int sourceid)
        {
            var logicmaplist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<LogicMap>(nameof(LogicMap),
                p => p.APISourceId == sourceid).ToList();


            List<TreeNode> treeNodes = new List<TreeNode>();

            foreach (var item in logicmaplist)
            {
                TreeNode newNode = new TreeNode(item.LogicName);

                newNode.ImageKey = newNode.SelectedImageKey = "LOGIC";

                newNode.Tag = item;

                treeNodes.Add(newNode);
            }

            parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));

        }

        public static void LoadApi(Form parent, TreeNode tbNode, int apiSourceId)
        {
            try
            {
                var apilist = LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl),
                    "SourceId", new object[] { apiSourceId }).ToList();

                List<TreeNode> treeNodes = new List<TreeNode>();

                foreach (var item in apilist.OrderBy(p => p.APIName))
                {
                    TreeNode newNode = new TreeNode(item.APIName);

                    newNode.ImageKey = newNode.SelectedImageKey = "API";

                    newNode.Tag = item;

                    treeNodes.Add(newNode);
                }

                parent.Invoke(new Action(() => { tbNode.Nodes.Clear(); tbNode.Nodes.AddRange(treeNodes.ToArray()); tbNode.Expand(); }));
            }
            catch (Exception ex)
            {

            }
        }

        public static void LoadApiResurceAsync(Form parent, TreeNode tbNode,AsyncCallback callback,object @object)
        {
            tbNode.Nodes.Add(new TreeNode("加载中...", 3, 3));
            tbNode.Expand();

            new Action<Form, TreeNode>(LoadApiResurce).BeginInvoke(parent, tbNode,callback, @object);
        }

        public static void LoadApiResurce(Form parent, TreeNode pnode)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            var aPISources = BigEntityTableEngine.LocalEngine.List<APISource>(nameof(APISource), 1, int.MaxValue);
            foreach (var s in aPISources)
            {
                TreeNode node = new TreeNodeEx(s.SourceName, 0, 2, 0, 2);
                var serverinfo = s;
                node.Tag = serverinfo;

                node.Nodes.Add(new TreeNodeEx
                {
                    Text = "接口",
                    Tag = new NodeContents(NodeContentType.APIPARENT),
                    ImageIndex = 0,
                    SelectedImageIndex = 2,
                    CollapseImgIndex = 0,
                    ExpandImgIndex = 2
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

                node.Nodes.Add(new TreeNodeEx
                {
                    Text = "文档",
                    Tag = new NodeContents(NodeContentType.DOCPARENT),
                    ImageIndex = 0,
                    SelectedImageIndex = 2,
                    CollapseImgIndex = 0,
                    ExpandImgIndex = 2
                });

                node.Nodes.Add(new TreeNodeEx
                {
                    Text = "逻辑图",
                    Tag = new NodeContents(NodeContentType.LOGICMAPParent),
                    ImageIndex = 0,
                    SelectedImageIndex = 2,
                    CollapseImgIndex = 0,
                    ExpandImgIndex = 2
                });

                treeNodes.Add(node);
            }
            parent.Invoke(new Action(() => { pnode.Nodes.Clear(); pnode.Nodes.AddRange(treeNodes.ToArray()); pnode.Expand(); }));
        }

        public static void LoadApiEnvAsync(Form parent, TreeNode pnode, int sourceid, AsyncCallback callback, object @object)
        {
            pnode.Nodes.Add(new TreeNode("加载中...", 3, 3));
            pnode.Expand();

            new Action<Form, TreeNode, int>(LoadApiEnv).BeginInvoke(parent, pnode, sourceid, callback, @object);
        }

        public static void LoadApiEnv(Form parent, TreeNode pnode, int sourceid)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            var envlist = BigEntityTableEngine.LocalEngine.Find<APIEnv>(nameof(APIEnv), "SourceId", new object[] { sourceid }).ToList();
            foreach (var s in envlist)
            {
                TreeNode node = new TreeNodeEx(s.EnvName, 0, 2, 0, 2);
                node.Tag = s;

                treeNodes.Add(node);
            }
            parent.Invoke(new Action(() => { pnode.Nodes.Clear(); pnode.Nodes.AddRange(treeNodes.ToArray()); pnode.Expand(); }));
        }

        public static void LoadApiEnvParamsAsync(Form parent, TreeNode pnode, int sourceid,int envid, AsyncCallback callback, object @object)
        {
            pnode.Nodes.Add(new TreeNode("加载中...", 3, 3));
            pnode.Expand();

            new Action<Form, TreeNode, int,int>(LoadApiEnvParams).BeginInvoke(parent, pnode, sourceid,envid, callback, @object);
        }

        public static void LoadApiEnvParams(Form parent, TreeNode pnode, int sourceid, int envid)
        {
            try
            {
                List<TreeNode> treeNodes = new List<TreeNode>();
                var allenvparamslist = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId", new object[] { sourceid }).ToList();
                var allparamnames = allenvparamslist.Select(p => p.Name).Distinct();
                foreach (var s in allparamnames)
                {
                    var param = allenvparamslist.Find(p => p.EnvId == envid && p.Name == s);

                    TreeNode node = new TreeNode(s);
                    if (param == null)
                    {
                        node.Tag = new APIEnvParam
                        {
                            EnvId = envid,
                            APISourceId = sourceid,
                            Name = s
                        };
                        node.ImageKey = node.SelectedImageKey = "COLQ";

                    }
                    else
                    {
                        node.Tag = param;
                        node.ImageKey = node.SelectedImageKey = "COL";
                    }
                    treeNodes.Add(node);

                }
                parent.Invoke(new Action(() => { pnode.Nodes.Clear(); pnode.Nodes.AddRange(treeNodes.ToArray()); pnode.Expand(); }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadApiDocsAsync(Form parent, TreeNode pnode, int sourceid, AsyncCallback callback, object @object)
        {
            pnode.Nodes.Add(new TreeNode("加载中...", 3, 3));
            pnode.Expand();

            new Action<Form, TreeNode, int>(LoadApiDocs).BeginInvoke(parent, pnode, sourceid, callback, @object);
        }

        public static void LoadApiDocs(Form parent, TreeNode pnode, int sourceid)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            
            var doclist = BigEntityTableEngine.LocalEngine.Find<APIDoc>(nameof(APIDoc), "APISourceId", new object[] { sourceid }).ToList();
            if (doclist.Count > 0)
            {
                var apiurllist = BigEntityTableEngine.LocalEngine.FindBatch<APIUrl>(nameof(APIUrl), doclist.Select(p => (object)p.APIId)).ToList();
                foreach (var s in doclist)
                {
                    var apiurl = apiurllist.Find(p => p.Id == s.APIId);
                    if (apiurl != null)
                    {
                        TreeNode node = new TreeNode(apiurl.APIName);

                        node.Tag = s;
                        node.ImageKey = node.SelectedImageKey = "DOC";
                        treeNodes.Add(node);
                    }

                }
            }
            parent.Invoke(new Action(() => { pnode.Nodes.Clear(); pnode.Nodes.AddRange(treeNodes.ToArray()); pnode.Expand(); }));
        }
    }
}
