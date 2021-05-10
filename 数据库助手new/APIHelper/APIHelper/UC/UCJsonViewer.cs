using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace APIHelper.UC
{
    public partial class UCJsonViewer : UserControl
    {
        private static ImageList IMGS = null;
        private bool sourcechanged = false;
        public UCJsonViewer()
        {
            InitializeComponent();

            this.DTV.LineColor = Color.LightBlue;

            if (IMGS == null)
            {
                IMGS = new ImageList();
                IMGS.Images.Add(Resources.Resource1.arr);
                IMGS.Images.Add(Resources.Resource1.obj);
                IMGS.Images.Add(Resources.Resource1.param);
            }
            this.DTV.ImageList = IMGS;
        }

        private string _datasource = null;
        public string DataSource
        {
            get
            {
                return _datasource;
            }
            set
            {
                if (_datasource != value)
                {
                    _datasource = value;
                    sourcechanged = true;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!string.IsNullOrWhiteSpace(DataSource)&&sourcechanged)
            {
                BindTreeView(this.DTV, DataSource);
                sourcechanged = false;
            }
        }

        public void BindDataSource()
        {
            if (!string.IsNullOrWhiteSpace(DataSource)&&sourcechanged)
            {
                BindTreeView(this.DTV, DataSource);
                sourcechanged = false;
            }
        }

        /// <summary>
        /// 绑定树形控件
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="strJson"></param>
        private void BindTreeView(TreeView treeView, string strJson)
        {
            treeView.Nodes.Clear();

            //string strJson = "1";//抛异常
            //string strJson = "{}";
            //string strJson = "{\"errcode\":0,\"errmsg\":\"查询成功\",\"datas\":[{\"c_ResourceType\":\"BootLogo\",\"c_Url\":\"/Upload/Magazine/4e09315d-7d92-4e6a-984d-80f684a24da8.jpg\"}]}";
            //string strJson = "[{\"DeviceCode\":\"430BE-B3C6A-4E953-9F972-FC741\",\"RoomNum\":\"777\"},{\"DeviceCode\":\"BF79F -09807-EEA31-2499E-31A98\",\"RoomNum\":\"888\"}]";

            if (IsJOjbect(strJson))
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);

                foreach (var item in jo)
                {
                    TreeNode tree;
                    if (item.Value.GetType() == typeof(JObject))
                    {
                        tree = new TreeNode(item.Key);
                        tree.ImageIndex=tree.SelectedImageIndex = 1;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        treeView.Nodes.Add(tree);
                    }
                    else if (item.Value.GetType() == typeof(JArray))
                    {
                        tree = new TreeNode(item.Key);
                        tree.ImageIndex = tree.SelectedImageIndex = 0;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        treeView.Nodes.Add(tree);
                    }
                    else
                    {
                        tree = new TreeNode(item.Key + ":" + item.Value.ToString());
                        tree.ImageIndex = tree.SelectedImageIndex = 2;
                        treeView.Nodes.Add(tree);
                    }
                }
            }
            if (IsJArray(strJson))
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(strJson);
                int i = 0;
                foreach (JObject item in ja)
                {
                    TreeNode tree = new TreeNode("Array [" + (i++) + "]");
                    tree.ImageIndex = tree.SelectedImageIndex = 1;
                    foreach (var itemOb in item)
                    {
                        TreeNode treeOb;
                        if (itemOb.Value.GetType() == typeof(JObject))
                        {
                            treeOb = new TreeNode(itemOb.Key);
                            treeOb.ImageIndex = treeOb.SelectedImageIndex = 1;
                            AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                            tree.Nodes.Add(treeOb);

                        }
                        else if (itemOb.Value.GetType() == typeof(JArray))
                        {
                            treeOb = new TreeNode(itemOb.Key);
                            treeOb.ImageIndex = treeOb.SelectedImageIndex = 0;
                            AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                            tree.Nodes.Add(treeOb);
                        }
                        else
                        {
                            treeOb = new TreeNode(itemOb.Key + ":" + itemOb.Value.ToString());
                            treeOb.ImageIndex = treeOb.SelectedImageIndex = 2;
                            tree.Nodes.Add(treeOb);
                        }
                    }
                    treeView.Nodes.Add(tree);
                }
            }
            treeView.ExpandAll();
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="parantNode"></param>
        /// <param name="value"></param>
        public void AddTreeChildNode(ref TreeNode parantNode, string value)
        {
            if (IsJOjbect(value))
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(value);
                foreach (var item in jo)
                {
                    TreeNode tree;
                    if (item.Value.GetType() == typeof(JObject))
                    {
                        tree = new TreeNode(item.Key);
                        tree.ImageIndex = tree.SelectedImageIndex = 1;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        parantNode.Nodes.Add(tree);
                    }
                    else if (item.Value.GetType() == typeof(JArray))
                    {
                        tree = new TreeNode(item.Key);
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        tree.ImageIndex = tree.SelectedImageIndex = 0;
                        parantNode.Nodes.Add(tree);
                    }
                    else
                    {
                        tree = new TreeNode(item.Key + ":" + item.Value.ToString());
                        tree.ImageIndex = tree.SelectedImageIndex = 2;
                        parantNode.Nodes.Add(tree);
                    }
                }
            }
            if (IsJArray(value))
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(value);
                int i = 0;
                foreach (JObject item in ja)
                {
                    TreeNode tree = new TreeNode("Array [" + (i++) + "]");
                    tree.SelectedImageIndex = tree.ImageIndex = 1;
                    parantNode.Nodes.Add(tree);
                    foreach (var itemOb in item)
                    {
                        TreeNode treeOb;
                        if (itemOb.Value.GetType() == typeof(JObject))
                        {
                            treeOb = new TreeNode(itemOb.Key);
                            treeOb.ImageIndex = treeOb.SelectedImageIndex = 1;
                            AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                            tree.Nodes.Add(treeOb);

                        }
                        else if (itemOb.Value.GetType() == typeof(JArray))
                        {
                            treeOb = new TreeNode(itemOb.Key);
                            treeOb.ImageIndex = treeOb.SelectedImageIndex = 0;
                            AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                            tree.Nodes.Add(treeOb);
                        }
                        else
                        {
                            treeOb = new TreeNode(itemOb.Key + ":" + itemOb.Value.ToString());
                            tree.Nodes.Add(treeOb);
                            treeOb.ImageIndex = treeOb.SelectedImageIndex = 2;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否JOjbect类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsJOjbect(string value)
        {
            try
            {
                JObject ja = JObject.Parse(value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否JArray类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsJArray(string value)
        {
            try
            {
                JArray ja = JArray.Parse(value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void TSMCopy_Click(object sender, EventArgs e)
        {
            if (DTV.SelectedNode != null)
            {
                Clipboard.SetText(DTV.SelectedNode.Text);
                Util.SendMsg(this, "复制成功");
            }
        }

        private void TSMView_Click(object sender, EventArgs e)
        {
            if (DTV.SelectedNode != null)
            {
                SubForm.TextBoxWin textBoxWin=new SubForm.TextBoxWin("查看",DTV.SelectedNode.Text);
                textBoxWin.Show();
            }
        }
    }
}
