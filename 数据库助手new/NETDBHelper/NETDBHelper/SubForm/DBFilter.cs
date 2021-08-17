using Biz.Common.Data;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class DBFilter : Form
    {
        private CheckedListBox mainCLB = null;

        /// <summary>
        /// 正向 1 反选 0
        /// </summary>
        private int _chooseStyle=0;

        private bool _canChooseTB = true;

        public DBFilter()
        {
            InitializeComponent();
        }

        private Control OwnerCtl = null;
        public void ShowMe(Control owner)
        {
            this.OwnerCtl = owner;
            this.Show();
        }

        public DBSource DBSource { get; private set; }

        public DBFilter SetDBSource(DBSource source)
        {
            this.DBSource = source;
            BtnSelectServer.Enabled = false;

            try
            {
                ChooseDB();
            }
            catch (Exception ex)
            {
                Util.SendMsg(this.OwnerCtl ?? this, ex.Message);
            }

            return this;
        }

        public DBFilter Init(int chooseStyle,bool canChooseTB)
        {
            _chooseStyle = chooseStyle;
            _canChooseTB = canChooseTB;
            if (!_canChooseTB)
            {
                CLBTBS.Enabled = false;
            }
            return this;
        }

        private void ChooseDB()
        {
            this.CLBDBs.Items.Clear();
            var dbs = Biz.Common.Data.SQLHelper.GetDBs(DBSource);
            var dic = new Dictionary<string, List<StringAndBool>>();
            
            this.CLBDBs.Items.AddRange(dbs.AsEnumerable().Select(p => (object)p.Field<string>("name")).OrderBy(p => p.ToString()).ToArray());
            
            for (var i = 0; i < this.CLBDBs.Items.Count; i++)
            {
                var db = CLBDBs.Items[i].ToString();
                var check = DBSource.ExDBList?.Contains(db) == true;
                CLBDBs.SetItemChecked(i, check);
            }

            if (DBSource.ExTBList != null)
            {
                foreach (var item in DBSource.ExTBList)
                {
                    var arr = item.Split(',');
                    if (!dic.ContainsKey(arr[0]))
                    {
                        dic.Add(arr[0], new List<StringAndBool>());
                    }

                    dic[arr[0]].Add(new StringAndBool(arr[1], true));
                }
            }
            BtnSelAll.Text = "全消";

            this.CLBDBs.Tag = dic;
        }

        private void BtnSelectServer_Click(object sender, EventArgs e)
        {
            var connsqlserver = new ConnSQLServer();
            if (connsqlserver.ShowDialog() == DialogResult.OK)
            {
                this.DBSource = connsqlserver.DBSource;
                try
                {
                    ChooseDB();
                }
                catch (Exception ex)
                {
                    Util.SendMsg(this.OwnerCtl ?? this, ex.Message);
                }
            }
        }

        private void CLBDBs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
             base.OnLoad(e);

            if (this.OwnerCtl != null)
            {
                var pt = this.OwnerCtl.PointToScreen(this.OwnerCtl.Location);
                pt.Offset(this.OwnerCtl.Width / 2 - this.Width / 2, this.OwnerCtl.Height / 2 - this.Height / 2);
                this.Location = pt;
            }
        }

        private void CLBDBs_Click(object sender, EventArgs e)
        {
            mainCLB = CLBDBs;
            if (CLBDBs.CheckedItems.Count < CLBDBs.Items.Count)
            {
                BtnSelAll.Text = "全选";
            }
            else
            {
                BtnSelAll.Text = "全消";
            }

            CLBTBS.ItemCheck -= CLBTBS_ItemCheck;
            this.CLBTBS.Items.Clear();

            var db = (string)CLBDBs.SelectedItem;

            if (CLBDBs.Tag != null)
            {
                var dic = (Dictionary<string, List<StringAndBool>>)CLBDBs.Tag;

                if (!dic.ContainsKey(db))
                {
                    var tbs = SQLHelper.GetTBs(DBSource, db);

                    var list = new List<StringAndBool>();
                    foreach (var row in tbs.AsEnumerable())
                    {
                        list.Add(new StringAndBool(row.Field<string>("name"), false));
                    }
                    dic.Add(db, list);
                }

                var i = 0;
                foreach (var item in dic[db])
                {
                    this.CLBTBS.Items.Add(item.Str);
                    this.CLBTBS.SetItemChecked(i, item.Boo);
                    i++;
                }
                this.CLBTBS.Tag = dic[db];
                CLBTBS.ItemCheck += CLBTBS_ItemCheck;
            }
        }

        private void CLBDBs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void CLBTBS_Click(object sender, EventArgs e)
        {
            mainCLB = CLBTBS;
            if (CLBTBS.CheckedItems.Count < CLBTBS.Items.Count)
            {
                BtnSelAll.Text = "全选";
            }
            else
            {
                BtnSelAll.Text = "全消";
            }
        }

        private void CLBTBS_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<StringAndBool> tuples = (List<StringAndBool>)CLBTBS.Tag;

            var tp = tuples.Find(p => p.Str == CLBTBS.Items[e.Index].ToString());
            tp.Boo = e.NewValue == CheckState.Checked;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("放弃吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            
            if (this.Modal)
            {
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                this.Close();
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (this.DBSource == null)
            {
                return;
            }

            var dbdic = (Dictionary<string, List<StringAndBool>>)this.CLBDBs.Tag;

            var exDBList = new List<string>();

            var exTBList = new Dictionary<string, List<string>>();

            foreach (var item in CLBDBs.CheckedItems)
            {
                var db = item.ToString();
                if (!exDBList.Contains(db))
                {
                    exDBList.Add(db);
                }

                if (dbdic.ContainsKey(db))
                {
                    if (!exTBList.ContainsKey(db))
                    {
                        exTBList.Add(db, new List<string>());
                    }
                    foreach (var tb in dbdic[db])
                    {
                        if (tb.Boo && exTBList[db].Contains(tb.Str))
                        {
                            exTBList[db].Add(tb.Str);
                        }
                    }
                }
            }

            DBSource.ExDBList = exDBList;
            DBSource.ExTBList = exTBList.SelectMany(q => q.Value.Select(f => q.Key + "," + f)).ToList();

            if (!CBIsTemp.Checked)
            {
                var allDBs = Biz.Common.XMLHelper.DeSerializeFromFile<DBSourceCollection>(Resources.Resource1.DbServersFile) ?? new DBSourceCollection();
                var dbs = allDBs.Upsert(DBSource, false);
                Biz.Common.XMLHelper.Serialize(allDBs, Resources.Resource1.DbServersFile);
            }

            if (this.Modal)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.Close();
            }

            Util.SendMsg(this.OwnerCtl, "操作成功，重新操作生效。");
        }

        private void BtnSelAll_Click(object sender, EventArgs e)
        {
            if (BtnSelAll.Text == "全选")
            {
                for (int i = 0; i < this.mainCLB.Items.Count; i++)
                {
                    this.mainCLB.SetItemChecked(i, true);
                }
                BtnSelAll.Text = "全消";
            }
            else
            {
                for (int i = 0; i < this.mainCLB.Items.Count; i++)
                {
                    this.mainCLB.SetItemChecked(i, false);
                }
                BtnSelAll.Text = "全选";
            }
        }
    }
}
