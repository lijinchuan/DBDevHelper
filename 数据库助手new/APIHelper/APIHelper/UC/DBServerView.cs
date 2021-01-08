using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using System.Text.RegularExpressions;
using APIHelper.SubForm;
using Biz.Common;
using LJC.FrameWorkV3.Data.EntityDataBase;
using Biz;
using APIHelper.UC;
using System.Threading;

namespace APIHelper
{
    public partial class DBServerView : UserControl
    {
        public Action<string, string> OnCreateSelectSql;
        public Action<string, LogicMap> OnDeleteLogicMap;

        public DBServerView()
        {
            InitializeComponent();
            ts_serchKey.Height = 20;
            tv_DBServers.ImageList = new ImageList();
            
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ForderClose);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ForderDB);
            tv_DBServers.ImageList.Images.Add(Resources.Resource1.ForderOpen);
            tv_DBServers.ImageList.Images.Add("LOADING", Resources.Resource1.loading);
            tv_DBServers.ImageList.Images.Add("API", Resources.Resource1.DB7);
            tv_DBServers.ImageList.Images.Add("COL", Resources.Resource1.DB6);
            tv_DBServers.ImageList.Images.Add("COLQ", Resources.Resource1.ColQ);
            tv_DBServers.ImageList.Images.Add("LOGIC", Resources.Resource1.logic);
            tv_DBServers.ImageList.Images.Add("DOC", Resources.Resource1.Index);

            tv_DBServers.Nodes.Add(new TreeNodeEx("资源管理器", 0, 1));
            tv_DBServers.BeforeExpand += Tv_DBServers_BeforeExpand;
            tv_DBServers.BeforeCollapse += Tv_DBServers_BeforeCollapse;
            tv_DBServers.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_DBServers_NodeMouseClick);
            tv_DBServers.NodeMouseDoubleClick += Tv_DBServers_NodeMouseDoubleClick;
            
            tv_DBServers.HideSelection = false;
            this.DBServerviewContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(OnMenuStrip_ItemClicked);
        }

        private void Tv_DBServers_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Node is Biz.TreeNodeEx)
            {
                e.Node.ImageIndex= e.Node.SelectedImageIndex = (e.Node as Biz.TreeNodeEx).CollapseImgIndex;
            }
        }

        private void Tv_DBServers_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is Biz.TreeNodeEx)
            {
                e.Node.ImageIndex=e.Node.SelectedImageIndex = (e.Node as Biz.TreeNodeEx).ExpandImgIndex;
            }
        }

        T FindParentNode<T>(TreeNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if(parent.Tag is T)
                {
                    return (T)parent.Tag;
                }
                parent = parent.Parent;
            }

            return default(T);
        }

        TreeNode FindParentNodeNode<T>(TreeNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (parent.Tag is T)
                {
                    return parent;
                }
                parent = parent.Parent;
            }

            return null;
        }

        TreeNode FindParentNode(TreeNode node, NodeContentType nodeContentType)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (parent.Tag is INodeContents && (parent.Tag as INodeContents).GetNodeContentType() == nodeContentType)
                {
                    return parent;
                }
                parent = parent.Parent;
            }

            return null;
        }


        void ReLoadDBObj(TreeNode selNode,bool loadall=false)
        {
            //TreeNode selNode = tv_DBServers.SelectedNode;
            if (selNode == null)
                return;
            AsyncCallback callback = null;
            if (loadall)
            {
                callback = new AsyncCallback((o) =>
                  {
                      var node = selNode;
                      foreach(TreeNode c in node.Nodes)
                      {
                          this.BeginInvoke(new Action(() =>
                          {
                              ReLoadDBObj(c, loadall);
                          }));
                      }
                  });
            }
            if (selNode.Level == 0)
            {

                Biz.UILoadHelper.LoadApiResurceAsync(this.ParentForm, selNode, callback, selNode);
            }
            else if (selNode.Tag is APISource)
            {
                if (loadall)
                {
                    callback(null);
                }
            }
            else if (selNode.Tag is NodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.APIPARENT)
            {
                var sid = (selNode.Parent.Tag as APISource).Id;
                Biz.UILoadHelper.LoadApiAsync(this.ParentForm, selNode, sid, callback, selNode);
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.LOGICMAPParent)
            {
                Biz.UILoadHelper.LoadLogicMapsAnsy(this.ParentForm, selNode, FindParentNode<APISource>(selNode).Id, callback, selNode);
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.ENVPARENT)
            {
                var sid = (selNode.Parent.Tag as APISource).Id;
                Biz.UILoadHelper.LoadApiEnvAsync(this.ParentForm, selNode, sid, callback, selNode);
            }
            else if (selNode.Tag is APIEnv)
            {
                var sid = FindParentNode<APISource>(selNode).Id;
                var envid = (selNode.Tag as APIEnv).Id;
                Biz.UILoadHelper.LoadApiEnvParamsAsync(this.ParentForm, selNode, sid, envid, callback, selNode);
            }
            else if (selNode.Tag is INodeContents && (selNode.Tag as INodeContents).GetNodeContentType() == NodeContentType.DOCPARENT)
            {
                var sid = FindParentNode<APISource>(selNode).Id;
                Biz.UILoadHelper.LoadApiDocsAsync(this.ParentForm, selNode, sid, callback, selNode);
            }
        }

        void OnMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                DBServerviewContextMenuStrip.Visible = false;
                var selnode = tv_DBServers.SelectedNode;
                if (selnode == null)
                {
                    return;
                }
                switch (e.ClickedItem.Text)
                {
                    case "添加API资源":
                        {
                            if(new SubForm.AddAPISource().ShowDialog() == DialogResult.OK)
                            {
                                Bind();
                            }
                            break;
                        }
                    case "编辑":
                        {
                            if (selnode.Tag is APISource)
                            {
                                if (new SubForm.AddAPISource((selnode.Tag as APISource).Id).ShowDialog() == DialogResult.OK)
                                {
                                    Bind();
                                }
                            }
                            else if (selnode.Tag is APIUrl)
                            {
                                var apiurl = selnode.Tag as APIUrl;
                                var souceid = FindParentNode<APISource>(selnode).Id;
                                if (apiurl.BodyDataType == BodyDataType.wcf)
                                {
                                    if (new SubForm.AddWCFApiDlg(souceid, apiurl).ShowDialog() == DialogResult.OK)
                                    {
                                        if (!BigEntityTableEngine.LocalEngine.Find<APIDoc>(nameof(APIDoc), "APIId", new object[] { apiurl.Id }).Any())
                                        {
                                            //创建文档
                                            BigEntityTableEngine.LocalEngine.Insert(nameof(APIDoc), new APIDoc
                                            {
                                                APISourceId = souceid,
                                                APIId = apiurl.Id,
                                                Mark = apiurl.Desc
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    if (new SubForm.AddAPIStep1Dlg(souceid, apiurl).ShowDialog() == DialogResult.OK)
                                    {
                                        if (!BigEntityTableEngine.LocalEngine.Find<APIDoc>(nameof(APIDoc), "APIId", new object[] { apiurl.Id }).Any())
                                        {
                                            //创建文档
                                            BigEntityTableEngine.LocalEngine.Insert(nameof(APIDoc), new APIDoc
                                            {
                                                APISourceId = souceid,
                                                APIId = apiurl.Id,
                                                Mark = apiurl.Desc
                                            });
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "删除":
                        {
                            if (selnode.Tag is APISource && MessageBox.Show("要删除吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                var sourceid = (selnode.Tag as APISource).Id;
                                //如果有API则不能删除
                                if (BigEntityTableEngine.LocalEngine.Count(nameof(APIUrl), "SourceId", new object[] { sourceid }) > 0)
                                {
                                    MessageBox.Show("删除失败，请先删除接口");
                                    return;
                                }
                                if (BigEntityTableEngine.LocalEngine.Scan<LogicMap>(nameof(LogicMap), "APISourceId_LogicName", new object[] { sourceid, Consts.STRINGCOMPAIRMIN }, new object[] { sourceid, Consts.STRINGCOMPAIRMAX }, 1, 1).Any())
                                {
                                    MessageBox.Show("删除失败，请先删除逻辑图关联");
                                    return;
                                }
                                if (BigEntityTableEngine.LocalEngine.Delete<APISource>(nameof(APISource), sourceid))
                                {
                                    Bind();
                                }
                            }
                            else if (selnode.Tag is APIEnv && MessageBox.Show("要删除吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                var apienv = selnode.Tag as APIEnv;
                                var apienvparams = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId", new object[] { apienv.SourceId })
                                    .ToList();
                                //如果有环境变量，则不能删除
                                foreach (var p in apienvparams.Where(p => p.EnvId == apienv.Id))
                                {
                                    BigEntityTableEngine.LocalEngine.Delete<APIEnvParam>(nameof(APIEnvParam), p.Id);
                                }
                                BigEntityTableEngine.LocalEngine.Delete<APIEnv>(nameof(APIEnv), apienv.Id);
                                ReLoadDBObj(FindParentNode(selnode, NodeContentType.ENVPARENT));
                            }
                            else if (selnode.Tag is APIEnvParam && MessageBox.Show("会删除所有环境下的此变量，要删除吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                var apienvparam = selnode.Tag as APIEnvParam;
                                var apienvparams = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_Name", new object[] { apienvparam.APISourceId, apienvparam.Name })
                                    .ToList();
                                foreach (var p in apienvparams)
                                {
                                    BigEntityTableEngine.LocalEngine.Delete<APIEnvParam>(nameof(APIEnvParam), p.Id);
                                }
                                ReLoadDBObj(FindParentNode(selnode, NodeContentType.ENVPARENT));
                            }
                            else if (selnode.Tag is APIUrl)
                            {
                                var souceid = FindParentNode<APISource>(selnode).Id;
                                var apiurlid = (selnode.Tag as APIUrl).Id;
                                //如果API
                                if (BigEntityTableEngine.LocalEngine.Find<LogicMapRelColumn>(nameof(LogicMapRelColumn), c =>
                                 {
                                     return c.APIId == apiurlid || c.RelAPIId == apiurlid;
                                 }).Count() > 0)
                                {
                                    MessageBox.Show("删除失败，请先删除逻辑图字段关联关系");
                                    return;
                                }
                                if (MessageBox.Show("要删除吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                {
                                    return;
                                }
                                //删除APIDATA
                                var apidatalist = BigEntityTableEngine.LocalEngine.Find<APIData>(nameof(APIData), "ApiId", new object[] { apiurlid }).ToList();
                                foreach (var apidata in apidatalist)
                                {
                                    BigEntityTableEngine.LocalEngine.Delete<APIData>(nameof(APIData), apidata.Id);
                                }
                                var apiparamlist = BigEntityTableEngine.LocalEngine.Find<APIParam>(nameof(APIParam), "APIId", new object[] { apiurlid }).ToList();
                                foreach (var apiparam in apiparamlist)
                                {
                                    BigEntityTableEngine.LocalEngine.Delete<APIParam>(nameof(APIParam), apiparam.Id);
                                }
                                var apiDocExamplelist = BigEntityTableEngine.LocalEngine.Find<APIDocExample>(nameof(APIDocExample), "ApiId", new object[] { apiurlid }).ToList();
                                foreach (var apidocexample in apiDocExamplelist)
                                {
                                    BigEntityTableEngine.LocalEngine.Delete<APIDocExample>(nameof(APIDocExample), apidocexample.Id);
                                }
                                var apidoclist = BigEntityTableEngine.LocalEngine.Find<APIDoc>(nameof(APIDoc), "APIId", new object[] { apiurlid }).ToList();
                                foreach (var apidoc in apidoclist)
                                {
                                    BigEntityTableEngine.LocalEngine.Delete<APIDoc>(nameof(APIDoc), apidoc.Id);
                                }
                                BigEntityTableEngine.LocalEngine.Delete<APIUrl>(nameof(APIUrl), apiurlid);
                                ReLoadDBObj(FindParentNode(selnode, NodeContentType.APISOURCE), true);
                            }
                            break;
                        }
                    case "添加API":
                        {
                            var apisource = selnode.Parent.Tag as APISource;
                            var sourceid= apisource.Id;
                            var step1dlg = new SubForm.AddAPIStep1Dlg(sourceid);
                            if (step1dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.ReLoadDBObj(selnode);
                                Util.AddToMainTab(this,$"[{apisource.SourceName}]{step1dlg.APIUrl.APIName}", new UC.UCAddAPI(step1dlg.APIUrl));

                                //创建文档
                                BigEntityTableEngine.LocalEngine.Insert(nameof(APIDoc), new APIDoc
                                {
                                    APISourceId=apisource.Id,
                                    APIId = step1dlg.APIUrl.Id,
                                    Mark = step1dlg.APIUrl.Desc
                                });
                            }
                            break;
                        }
                    case "添加环境":
                        {
                            var apisource = selnode.Parent.Tag as APISource;
                            var sourceid = apisource.Id;
                            var dlg = new SubForm.AddEnvDlg(sourceid);
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.ReLoadDBObj(selnode);
                            }
                            break;
                        }
                    case "添加环境变量":
                        {
                            var apisource = selnode.Parent.Tag as APISource;
                            var sourceid = apisource.Id;
                            var dlg = new SubForm.AddAPIEnvParamDlg(sourceid,0);
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.ReLoadDBObj(selnode);
                            }
                            break;
                        }
                    case "参数定义":
                        {
                            var apidoc = selnode.Tag as APIDoc;
                            var apisource = FindParentNode<APISource>(selnode);
                            var page = new UC.UCAddAPIParam(apidoc.APISourceId, apidoc.APIId);
                            Util.AddToMainTab(this, $"{apisource.SourceName}.{selnode.Text}参数定义", page);
                            break;
                        }
                    case "刷新":
                        {
                            ReLoadDBObj(tv_DBServers.SelectedNode);
                            break;
                        }
                    case "新增逻辑关系图":
                        {
                            AddNewLogicMap();
                            break;
                        }
                    case "删除逻辑关系图":
                        {
                            DelLogicMap();
                            break;
                        }
                    case "复制对象名":
                        {
                            Clipboard.SetText(selnode.Text);
                            Util.SendMsg(this, "已复制到剪贴板");
                            break;
                        }
                    case "添加WCF接口":
                        {
                            var apisource = selnode.Parent.Tag as APISource;
                            var sourceid = apisource.Id;
                            var step1dlg = new SubForm.AddWCFApiDlg(sourceid);
                            if (step1dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.ReLoadDBObj(selnode);
                                Util.AddToMainTab(this, $"[{apisource.SourceName}]{step1dlg.APIUrl.APIName}", new UC.UCAddAPI(step1dlg.APIUrl));

                                //创建文档
                                BigEntityTableEngine.LocalEngine.Insert(nameof(APIDoc), new APIDoc
                                {
                                    APISourceId = apisource.Id,
                                    APIId = step1dlg.APIUrl.Id,
                                    Mark = step1dlg.APIUrl.Desc
                                });
                            }
                            break;
                        }
                    case "如何使用":
                        {
                            if (selnode.Tag is APIEnvParam)
                            {
                                var page = new UC.DocPage();
                                Util.AddToMainTab(this, $"帮助文档-环境变量", page);
                                page.InitDoc(Application.StartupPath + "\\help.html#envparam", null);

                            }
                            else if ((selnode.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.DOCPARENT)
                            {
                                var page = new UC.DocPage();
                                Util.AddToMainTab(this, $"帮助文档-接口文档", page);
                                page.InitDoc(Application.StartupPath + "\\help.html#doc", null);
                            }
                            break;
                        }
                    default:
                        {
                            MessageBox.Show(e.ClickedItem.Text);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private string GetDBName(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Level < 1)
                return null;
            if (node.Level == 2)
                return node.Text;
            return GetDBName(node.Parent);
        }

        void tv_DBServers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {

                ReLoadDBObj(e.Node);
            }
        }


        private void Tv_DBServers_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is APIUrl)
            {
                var apiurlid = (e.Node.Tag as APIUrl).Id;
                var apiurl = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), apiurlid);
                if (apiurl != null)
                {
                    var source = e.Node.Parent.Parent.Tag as APISource;

                    Util.AddToMainTab(this, $"[{source.SourceName}]{apiurl.APIName}", new UC.UCAddAPI(apiurl));
                }
            }
            else if (e.Node.Tag is APIEnvParam)
            {
                var apisource = FindParentNode<APISource>(e.Node);
                var envparam = e.Node.Tag as APIEnvParam;
                if (envparam.Id == 0)
                {
                    envparam = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_Name", new object[] { apisource.Id, envparam.Name }).FirstOrDefault();
                }
                var dlg = new SubForm.AddAPIEnvParamDlg(apisource.Id, envparam.Id);
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                }
            }
            else if (e.Node.Tag is LogicMap)
            {
                var apisouce = FindParentNode<APISource>(e.Node);
                var logicmap = (e.Node.Tag as LogicMap);
                var title = $"{apisouce.SourceName}逻辑关系图{logicmap.LogicName}";
                UC.UCLogicMap panel = new UCLogicMap(apisouce, logicmap.ID);
                panel.Load();
                Util.AddToMainTab(this, title, panel);
            }
            else if (e.Node.Tag is APIDoc)
            {
                var apisouce = FindParentNode<APISource>(e.Node);
                var doc = e.Node.Tag as APIDoc;
                var apiurl = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), doc.APIId);
                if (apiurl != null)
                {
                    var page = new UC.DocPage();
                    Util.AddToMainTab(this, $"[文档]{apisouce.SourceName}.{apiurl.APIName}", page);
                    page.InitDoc(apiurl, null);
                }
            }
        }

        public void Bind()
        {
            ReLoadDBObj(tv_DBServers.Nodes[0]);
        }

        private void DBServerView_Load(object sender, EventArgs e)
        {
            ReLoadDBObj(tv_DBServers.Nodes[0], true);
        }

        public string DisConnectSelectDBServer()
        {
            if (this.tv_DBServers.SelectedNode == null || this.tv_DBServers.SelectedNode.Level != 1)
                return null;
            var server = this.tv_DBServers.SelectedNode.Text;
            DisConnectServer(server);
            return server;
        }

        private void DisConnectServer(string serverName)
        {

        }

        private void tv_DBServers_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node = tv_DBServers.SelectedNode;
                this.tv_DBServers.ContextMenuStrip = this.DBServerviewContextMenuStrip;

                添加API资源ToolStripMenuItem.Visible = node.Level == 0;

                添加APIToolStripMenuItem.Visible = (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.APIPARENT;
                添加WCF接口ToolStripMenuItem.Visible = (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.APIPARENT;

                修改ToolStripMenuItem.Visible = node.Tag is APISource
                    ||node.Tag is APIUrl;
                删除ToolStripMenuItem.Visible = node.Tag is APISource
                    ||node.Tag is APIEnv
                    ||node.Tag is APIEnvParam
                    ||node.Tag is APIUrl;

                添加环境ToolStripMenuItem.Visible= (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.ENVPARENT;
                添加环境变量ToolStripMenuItem.Visible= (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.ENVPARENT;

                参数定义ToolStripMenuItem.Visible = (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.DOC;

                新增逻辑关系图ToolStripMenuItem.Visible= (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.LOGICMAPParent;
                删除逻辑关系图ToolStripMenuItem.Visible = (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.LOGICMAP;

                如何使用ToolStripMenuItem.Visible = (node.Tag as INodeContents)?.GetNodeContentType() == NodeContentType.DOCPARENT
                    || node.Tag is APIEnvParam;
            }

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            string serchkey = ts_serchKey.Text;

            bool matchall = serchkey.StartsWith("'");
            if (matchall)
            {
                serchkey = serchkey.Trim('\'');
            }
            if (!ts_serchKey.Items.Contains(serchkey))
            {
                ts_serchKey.Items.Add(serchkey);
            }
            if (tv_DBServers.SelectedNode == null)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }

            if (!tv_DBServers.Focused)
            {
                this.tv_DBServers.Focus();
            }

            bool boo = false;
            if (tv_DBServers.SelectedNode.Nodes.Count > 0)
                boo = SearchNode(tv_DBServers.SelectedNode.Nodes[0], serchkey, matchall, true);
            else if (tv_DBServers.SelectedNode.NextNode != null)
                boo = SearchNode(tv_DBServers.SelectedNode.NextNode, serchkey, matchall, true);
            else
            {
                var parent = tv_DBServers.SelectedNode.Parent;
                while (parent != null && parent.NextNode == null)
                {
                    parent = parent.Parent;
                }
                if (parent != null)
                {
                    if (parent.NextNode != null)
                    {
                        boo = SearchNode(parent.NextNode, serchkey, matchall, true);
                    }
                }
            }

            if (!boo)
            {
                tv_DBServers.SelectedNode = tv_DBServers.Nodes[0];
            }

        }

        private bool SearchNode(TreeNode nodeStart, string txt, bool matchall, bool maxsearch)
        {
            if (nodeStart == null)
            {
                return false;
            }
            var text = nodeStart.Text;
            var find = matchall ? text.Equals(txt, StringComparison.OrdinalIgnoreCase) : text.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1;
            if (!find)
            {
                if(nodeStart.Tag is APIUrl)
                {
                    var apiurl = nodeStart.Tag as APIUrl;
                    text = apiurl.Path;
                    find = matchall ? text.Equals(txt, StringComparison.OrdinalIgnoreCase) : text.IndexOf(txt, StringComparison.OrdinalIgnoreCase) > -1;
                }
            }
            if (find)
            {
                tv_DBServers.SelectedNode = nodeStart;
                return true;
            }
            if (nodeStart.Nodes.Count > 0)
            {
                foreach (TreeNode node in nodeStart.Nodes)
                {
                    if (SearchNode(node, txt, matchall, false))
                        return true;
                }
            }

            if (maxsearch)
            {
                if (nodeStart.NextNode != null)
                {
                    return SearchNode(nodeStart.NextNode, txt, matchall, true);
                }
                else
                {
                    if (maxsearch)
                    {
                        var parent = nodeStart.Parent;
                        while (parent != null && parent.NextNode == null)
                        {
                            parent = parent.Parent;
                        }
                        if (parent != null)
                        {
                            return SearchNode(parent.NextNode, txt, matchall, true);
                        }
                    }
                }
            }

            return false;
        }

        private void ts_serchKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                tv_DBServers.Focus();
                toolStripDropDownButton1_Click(null, null);
            }
        }

        public TreeNode FindNode(string serverName, string dbName = null, string tbName = null)
        {
            foreach (TreeNode node in tv_DBServers.Nodes[0].Nodes)
            {
                if (node.Text.Equals(serverName, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(dbName))
                        return node;
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        if (subNode.Text.Equals(dbName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (string.IsNullOrWhiteSpace(tbName))
                                return subNode;
                            foreach (TreeNode subSubNode in subNode.Nodes)
                            {
                                if (subSubNode.Text.Equals(tbName, StringComparison.OrdinalIgnoreCase))
                                    return subSubNode;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void SubMenuItem_Insert_Click(object sender, EventArgs e)
        {

        }

        private void Mark_Local()
        {
            

        }

        private void 备注本地ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mark_Local();
        }

        private void MarkResource()
        {
           
        }

        private void ClearMarkResource()
        {
           
        }

        private void AddNewLogicMap()
        {
            var currnode = this.tv_DBServers.SelectedNode;
            if (currnode == null)
            {
                return;
            }

            while (true)
            {
                SubForm.InputStringDlg dlg = new InputStringDlg("输入逻辑关系图名称");
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var apisouce = FindParentNode<APISource>(currnode);
                    var name = dlg.InputString.ToUpper();
                    long total = 0;
                    if (BigEntityTableEngine.LocalEngine.Scan<LogicMap>(nameof(LogicMap), "APISourceId_LogicName",
                        new object[] { apisouce.Id, name }, new object[] { apisouce.Id, name }, 1, 1, ref total).Count > 0)
                    {
                        MessageBox.Show("名称不能重复");
                    }
                    else
                    {
                        var logicmap = new LogicMap
                        {
                            APISourceId=apisouce.Id,
                            LogicName=dlg.InputString,
                            SourceName=apisouce.SourceName
                        };
                        BigEntityTableEngine.LocalEngine.Insert<LogicMap>(nameof(LogicMap), logicmap);
                        ReLoadDBObj(currnode);

                        var title = $"{apisouce.SourceName}逻辑关系图{logicmap.LogicName}";
                        UC.UCLogicMap panel = new UCLogicMap(apisouce, logicmap.ID);
                        panel.Load();
                        Util.AddToMainTab(this, title, panel);

                        break;
                    }
                }
                else
                {
                    break;
                }

            }
        }

        private void DelLogicMap()
        {
            var currnode = this.tv_DBServers.SelectedNode;
            if(currnode!=null&&currnode.Tag is LogicMap)
            {
                var logicmap = currnode.Tag as LogicMap;
                if (MessageBox.Show($"要删除逻辑关系图【{logicmap.LogicName}】吗？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //先删除相关字段
                    var logicMapRelColumnList = BigEntityTableEngine.LocalEngine.Find<LogicMapRelColumn>(nameof(LogicMapRelColumn), "LogicID", new object[] { logicmap.ID }).ToList();
                    foreach(var col in logicMapRelColumnList)
                    {
                        BigEntityTableEngine.LocalEngine.Delete<LogicMapRelColumn>(nameof(LogicMapRelColumn), col.ID);
                    }
                    var logicTableList = BigEntityTableEngine.LocalEngine.Find<LogicMapTable>(nameof(LogicMapTable), "LogicID", new object[] { logicmap.ID }).ToList();
                    foreach(var tb in logicTableList)
                    {
                        BigEntityTableEngine.LocalEngine.Delete<LogicMapTable>(nameof(LogicMapTable), tb.ID);
                    }
                    BigEntityTableEngine.LocalEngine.Delete<LogicMap>(nameof(LogicMap), logicmap.ID);
                    if (this.OnDeleteLogicMap != null)
                    {
                        this.OnDeleteLogicMap(GetDBName(currnode), logicmap);
                    }
                    ReLoadDBObj(currnode.Parent);
                }
            }
        }

        private void TTSM_CreateIndex_Click(object sender, EventArgs e)
        {
            var _node = tv_DBServers.SelectedNode;
        }

        private void TTSM_DelIndex_Click(object sender, EventArgs e)
        {
        }
    }
}
