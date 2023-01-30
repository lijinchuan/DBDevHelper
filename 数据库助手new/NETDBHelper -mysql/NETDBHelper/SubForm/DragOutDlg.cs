using NETDBHelper.UC;
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
    public partial class DragOutDlg : SubBaseDlg
    {
        private Control dargSource;
        private TabPage tabPage;
        public DragOutDlg()
        {
            InitializeComponent();
        }

        public DragOutDlg(Control dargSource,TabPage tabPage)
        {
            InitializeComponent();
            Tabs.TabPages.Clear();
            Tabs.CanDragOut = false;
            this.dargSource = dargSource;
            this.tabPage = tabPage;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.MaximizeBox = true;
            Tabs.TabPages.Add(tabPage);
            Tabs.SelectedTab = tabPage;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Tabs.TabPages.Clear();
            (this.dargSource as MyTabControl).TabPages.Add(tabPage);
            (this.dargSource as MyTabControl).SelectedTab = tabPage;
        }
    }
}
