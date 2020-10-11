using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace NETDBHelper.UC
{
    public partial class UCCreateCodeCallSP : TabPage
    {
        public UCCreateCodeCallSP()
        {
            InitializeComponent();
        }

        public UCCreateCodeCallSP(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_Params.Text))
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("var db = DatabaseFactory.CreateDatabase();");
            sb.AppendFormat("var cmd=db.GetStoredProcCommand({0});",tb_spName.Text);
            sb.AppendLine();
            string[] array = tb_Params.Text.Split(',');
            Regex rg = new Regex(@"(@\w+)\s+([\w\(\)]+)=?\s{0,}(\w{0,})\,?\r?\n?",RegexOptions.IgnoreCase|RegexOptions.Multiline);
            MatchCollection matchs= rg.Matches(tb_Params.Text);
            string outPutParam = string.Empty;
            foreach (Match m in matchs)
            {
                if (m.Groups.Count > 3 && m.Groups[3].Value.Trim().Equals("output", StringComparison.OrdinalIgnoreCase))
                {
                    outPutParam = m.Groups[1].Value;
                    sb.AppendFormat("db.AddOutParameter(cmd, \"{0}\",{1}, 18);",
                        m.Groups[1].Value,Biz.Common.Data.Common.SqlTypeToDatadbType(m.Groups[2].Value));
                }
                else
                {
                    sb.AppendFormat("db.AddInParameter(cmd, \"{0}\",{1},{2}.{3});",
                        m.Groups[1].Value, Biz.Common.Data.Common.SqlTypeToDatadbType(m.Groups[2].Value),
                        tb_Entity.Text, m.Groups[1].Value.Trim('@'));
                }
                sb.AppendLine();
            }
            sb.AppendLine("db.ExecuteNonQuery(cmd);");
            if (!string.IsNullOrWhiteSpace(outPutParam))
                sb.AppendFormat("object o = db.GetParameterValue(cmd, \"{0}\");", outPutParam);
            tb_code.Text = sb.ToString();
        }
    }
}
