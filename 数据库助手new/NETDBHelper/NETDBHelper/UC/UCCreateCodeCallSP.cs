using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace NETDBHelper.UC
{
    public partial class UCCreateCodeCallSP : TabPage
    {
        public UCCreateCodeCallSP()
        {
            InitializeComponent();
            Dictionary<int, string> typedic = new Dictionary<int, string>();
            typedic.Add(1, "ADO.NET");
            typedic.Add(2, "MS ENTERPRICE LIB");
            CBType.DataSource = typedic.Select(p => new
            {
                p.Key,
                p.Value
            }).ToList();
            CBType.ValueMember = "Key";
            CBType.DisplayMember = "Value";
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

            if (CBType.SelectedValue.Equals(2))
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder outputsb = new StringBuilder();
                sb.AppendLine("var db = DatabaseFactory.CreateDatabase();");
                sb.AppendFormat("var cmd=db.GetStoredProcCommand(\"{0}\");", tb_spName.Text);
                sb.AppendLine();
                string[] array = tb_Params.Text.Split(',');
                Regex rg = new Regex(@"(@\w+)\s+([\w\(\)]+)=?\s{0,}(\w{0,})\,?\r?\n?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection matchs = rg.Matches(tb_Params.Text);
                //string outPutParam = string.Empty;
                foreach (Match m in matchs)
                {
                    if (m.Groups.Count > 3 && m.Groups[3].Value.Trim().Equals("output", StringComparison.OrdinalIgnoreCase))
                    {
                        var outPutParam = m.Groups[1].Value;
                        sb.AppendFormat("db.AddOutParameter(cmd, \"{0}\",{1}, 18);",
                            m.Groups[1].Value, Biz.Common.Data.Common.SqlTypeToDatadbType(m.Groups[2].Value));

                        outputsb.AppendFormat("object op_"+ outPutParam.Trim('@') + " = db.GetParameterValue(cmd, \"{0}\");", outPutParam);
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
                sb.AppendLine();
                sb.AppendLine(outputsb.ToString());
                tb_code.Text = sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SqlParameter[] parameters = new SqlParameter[] {");
                sb.AppendLine();
                string[] array = tb_Params.Text.Split(',');
                Regex rg = new Regex(@"(@\w+)\s+([\w\(\)]+)=?\s{0,}(\w{0,})\,?\r?\n?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection matchs = rg.Matches(tb_Params.Text);
                string outPutParam = string.Empty;

                StringBuilder sboutput = new StringBuilder();
                int i = 0;
                foreach (Match m in matchs)
                {
                    if (m.Groups.Count > 3 && m.Groups[3].Value.Trim().Equals("output", StringComparison.OrdinalIgnoreCase))
                    {
                        outPutParam = m.Groups[1].Value;
                        
                        sb.Append($@"new SqlParameter(""{outPutParam}"",{Biz.Common.Data.Common.SqlTypeToDatadbType(m.Groups[2].Value)}){{Value={tb_Entity.Text}.{outPutParam.Trim('@')},Direction=ParameterDirection.Output}},");
                        sboutput.AppendLine($"object op_{outPutParam.Trim('@')}=parameters[{i}].Value;");
                    }
                    else
                    {
                        sb.Append($@"new SqlParameter(""{m.Groups[1].Value}"",{Biz.Common.Data.Common.SqlTypeToDatadbType(m.Groups[2].Value)}){{Value={tb_Entity.Text}.{m.Groups[1].Value.Trim('@')}}},");
                    }
                    i++;
                    sb.AppendLine();
                }
                sb.AppendLine("}");
                sb.AppendLine(sboutput.ToString());

                tb_code.Text = sb.ToString();
            }
        }
    }
}
