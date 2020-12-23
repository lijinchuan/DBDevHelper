using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.UC
{
    public partial class UCAPIExampleTabPage : TabPage
    {
        UCAPIExample UCAPIExample = null;
        public UCAPIExampleTabPage()
        {
            InitializeComponent();
        }

        public UCAPIExampleTabPage(int apiid,Entity.APIInvokeLog log)
        {
            InitializeComponent();
            UCAPIExample = new UC.UCAPIExample(apiid,log);
            UCAPIExample.Dock = DockStyle.Fill;
            this.Controls.Add(UCAPIExample);
        }
    }
}
