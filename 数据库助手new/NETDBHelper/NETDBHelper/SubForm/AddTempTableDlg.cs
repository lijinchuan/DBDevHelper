using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
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
    public partial class AddTempTableDlg : SubBaseDlg
    {
        private DBSource dbSource;
        private TempTB tempTB = null;
        private List<TempTBColumn> tempTBColumns = new List<TempTBColumn>();
        private string tbName;

        public AddTempTableDlg()
        {
            InitializeComponent();
        }

        public TempTB TempTB
        {
            get
            {
                return tempTB;
            }
        }

        public List<TempTBColumn> TempTBColumns
        {
            get
            {
                return tempTBColumns;
            }
        }

        public AddTempTableDlg(DBSource dbSource,string tbname)
        {
            InitializeComponent();
            this.dbSource = dbSource;
            tbName = tbname;
            
        }

        int colPanelCount =1;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PBAdd.Parent.Tag = 0;
            PBAdd.Image = Resources.Resource1.bullet_plus;
            PBRem.Image = Resources.Resource1.bullet_minus;

            PBAdd.Click += PBAdd_Click;
            PBRem.Click += PBRem_Click;

            if (string.IsNullOrWhiteSpace(tbName))
            {
                tbName = "$" + Guid.NewGuid().ToString("N");
            }
            else
            {
                var temptb = BigEntityTableRemotingEngine.Find<TempTB>(nameof(TempTB), TempTB.INDEX_DB_TB, new object[] { tbName }).FirstOrDefault();
                if (temptb == null)
                {
                    MessageBox.Show("临时表不存在");
                    Close();
                    return;
                }
                tempTB = temptb;

                TBDisplayTableName.Text = tempTB.DisplayName;

                CBType.DataSource = Enum.GetNames(typeof(MSSQLTypeEnum));

                var cols = BigEntityTableRemotingEngine.Find<TempTBColumn>(nameof(TempTBColumn), nameof(TempTBColumn.TBId), new object[] { tempTB.Id }).ToList();
                if (cols.Count > 0)
                {
                    CBType.SelectedItem = cols[0].TypeName;
                    TBColName.Text = cols[0].Name;
                    CBType.Enabled = false;
                    TBColName.Enabled = false;
                }

                for(var i = 1; i < cols.Count; i++)
                {
                    CopyCloumnBox(cols[i]);
                }
            }

            if (tempTB == null)
            {
                tempTB = new TempTB
                {
                    TBName = tbName
                };
            }
        }

        private void PBRem_Click(object sender, EventArgs e)
        {
            if (colPanelCount == 1)
            {
                return;
            }
            var pb = (sender as PictureBox);
            if (pb.Tag is TempTBColumn)
            {
                var col = pb.Tag as TempTBColumn;
                BigEntityTableRemotingEngine.Delete<TempTBColumn>(nameof(TempTBColumn), col.Id);
                var ctl = pb.Parent;
                if (ctl.Tag is int)
                {
                    PannelColumns.Controls.Remove(ctl);
                    AdjustColumn((int)ctl.Tag);
                    colPanelCount--;
                }
            }
        }

        private Panel GetLastColumnBox()
        {
            Panel panelLast = null;
            foreach (Control ctl in PannelColumns.Controls)
            {
                if (ctl is Panel && ctl.Tag is int)
                {
                    var panel = ctl as Panel;
                    if (panelLast == null || (int)panel.Tag > (int)panelLast.Tag)
                    {
                        panelLast = panel;
                    }
                }
            }

            return panelLast;
        }

        private IEnumerable<TempTBColumn> GetColumns()
        {
            
            foreach (Control ctl in PannelColumns.Controls)
            {
                if (ctl is Panel && ctl.Tag is int)
                {
                    TempTBColumn col = new TempTBColumn();
                    var panel = ctl as Panel;
                    var ignore = false;
                    foreach(Control c in panel.Controls)
                    {
                        if (!c.Enabled)
                        {
                            ignore = true;
                        }
                        if(c is ComboBox)
                        {
                            col.TypeName = (c as ComboBox).SelectedItem.ToString();
                        }

                        if(c is TextBox)
                        {
                            col.Name = (c as TextBox).Text.Trim();
                        }
                    }
                    if (!ignore)
                    {
                        yield return col;
                    }
                }
            }
        }

        private void AdjustColumn(int remId)
        {
            foreach(Control ctl in PannelColumns.Controls)
            {
                if(ctl is Panel&&ctl.Tag is int)
                {
                    var id = (int)ctl.Tag;
                    if (id > remId)
                    {
                        var location = ctl.Location;
                        location.Offset(0, -ctl.Height);
                        ctl.Location = location;
                        ctl.Tag = id - 1;
                    }
                }
            }
        }

        private bool IsEques(Image img1,Image img2)
        {
            var bytes1=LJC.FrameWorkV3.Comm.ImageHelper.GetBytes(img1);
            var bytes2 = LJC.FrameWorkV3.Comm.ImageHelper.GetBytes(img2);
            if (bytes1.Length != bytes2.Length)
            {
                return false;
            }
            for(var i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void CopyCloumnBox(TempTBColumn tempTBColumn)
        {
            var panelLast = GetLastColumnBox();
            var newPanel = new Panel();
            newPanel.Width = panelLast.Width;
            newPanel.Height = panelLast.Height;

            foreach (Control ctl in panelLast.Controls)
            {
                if (ctl is ComboBox)
                {
                    var cbType = ctl as ComboBox;
                    ComboBox cb = new ComboBox();
                    newPanel.Controls.Add(cb);
                    cb.Items.AddRange(Enum.GetNames(typeof(MSSQLTypeEnum)));
                    cb.Width = cbType.Width;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.Height = cbType.Height;
                    cb.Location = cbType.Location;
                    if (tempTBColumn != null)
                    {
                        cb.SelectedItem = tempTBColumn.TypeName;
                        cb.Enabled = false;
                    }
                }
                else if (ctl is TextBox)
                {
                    var tbColName = ctl as TextBox;
                    TextBox tb = new TextBox();
                    tb.Width = tbColName.Width;
                    tb.Height = tbColName.Height;
                    tb.Location = tbColName.Location;
                    if (tempTBColumn != null)
                    {
                        tb.Text = tempTBColumn.Name;
                        tb.Enabled = false;
                    }
                    newPanel.Controls.Add(tb);
                }
                else if (ctl is PictureBox)
                {
                    var pb = ctl as PictureBox;
                    PictureBox pbnew = new PictureBox();
                    pbnew.Tag = tempTBColumn;
                    pbnew.Height = pb.Height;
                    pbnew.Width = pb.Width;
                    pbnew.Image = pb.Image;
                    pbnew.Location = pb.Location;
                    if (IsEques(pb.Image, Resources.Resource1.bullet_plus))
                    {
                        pbnew.Click += PBAdd_Click;
                    }
                    else
                    {
                        pbnew.Click += PBRem_Click;
                    }

                    newPanel.Controls.Add(pbnew);
                }
            }


            newPanel.Tag = (int)panelLast.Tag + 1;
            colPanelCount++;

            var location = panelLast.Location;
            location.Offset(0, panelLast.Height);
            newPanel.Location = location;

            PannelColumns.Controls.Add(newPanel);
        }

        private void PBAdd_Click(object sender, EventArgs e)
        {
            CopyCloumnBox(null);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBDisplayTableName.Text))
            {
                MessageBox.Show("名称不能为空");
                return;
            }

            tempTB.DisplayName = TBDisplayTableName.Text.Trim();
            if (BigEntityTableRemotingEngine.Upsert(nameof(TempTB), tempTB))
            {
                var columns = GetColumns().ToList();
                if (columns.Count > 0)
                {

                    if (columns.Select(p => p.Name.ToUpper()).GroupBy(p => p).Any(p => p.Count() > 1))
                    {
                        MessageBox.Show("字段不能重复");
                        return;
                    }
                    var cols = BigEntityTableRemotingEngine.Find<TempTBColumn>(nameof(TempTBColumn), nameof(TempTBColumn.TBId), new object[] { tempTB.Id }).ToList();
                    if (cols.Count > 0)
                    {
                        var exCols = cols.Where(p => columns.Any(q => q.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase))).ToList();
                        if (exCols.Count > 0)
                        {
                            MessageBox.Show("字段不能重复:" + string.Join(" ", exCols.Select(p => p.Name)));
                            return;
                        }
                    }
                    foreach (var col in columns)
                    {
                        col.TBId = tempTB.Id;
                    }

                    BigEntityTableRemotingEngine.InsertBatch(nameof(TempTBColumn), columns);
                }
            }
            tempTBColumns = BigEntityTableRemotingEngine.Find<TempTBColumn>(nameof(TempTBColumn), nameof(TempTBColumn.TBId), new object[] { tempTB.Id }).ToList();
            this.DialogResult = DialogResult.OK;
        }
    }
}
