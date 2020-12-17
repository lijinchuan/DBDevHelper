using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class ParseSwaggerDocDlg : Form
    {
        public ParseSwaggerDocDlg()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            
            var isResp = CBIsResp.Checked;
            var Result = Regex.Replace(TBInput.Text, isResp ? @"(\w+)\s+([\w\[\]]+)[^\r\n]*[\r\n]+(?:nullable: true[\r\n]+)?(([^\r\n]+)[\r\n]+)?" : @"(\w+)(\*?)\s+([\w\]\[]+)[^\r\n]*[\r\n]+(?:nullable: true[\r\n]+)?(([^\r\n]+)[\r\n]+)?", isResp ? "|$1|$2  |$4 |\r\n" : "|$1|$2  |$3|$5   |\r\n", RegexOptions.IgnoreCase);
            this.TBOutPut.Text = Result;
        }
    }
}
