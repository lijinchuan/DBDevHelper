using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public class SqlProceCodePanel:TabPage
    {
        private SQLEditBox sqlEditBox1;

        internal string UseCode
        {
            get
            {
                return @"<pre>
            Dictionary<string,object> retDic=new Dictionary<string,object>();
            retDic.Add('@recordCount', 0);
            
            string gubacode='000001',datetimetype='90day',bandtype='1';
            int pagesize=100,pageindex=1;
              
            var outputparam=new System.Data.SqlClient.SqlParameter('@recordCount',System.Data.SqlDbType.Int);
            outputparam.Direction = System.Data.ParameterDirection.Output;

            var list = LJC.FrameWork.Data.QuickDataBase.DataContextMoudelFactory<GubaBandResultEntity>
                .GetDataContext('ConndbDB$GubaData')
                .ExecuteProc('GubaBandResultList',
                new DbParameter[]{
                   new System.Data.SqlClient.SqlParameter('@GubaCode',gubacode),
                   new System.Data.SqlClient.SqlParameter('@DateType',datetimetype),
                   new System.Data.SqlClient.SqlParameter('@pageSize',pagesize),
                   new System.Data.SqlClient.SqlParameter('@pageIndex',pageindex),
                   new System.Data.SqlClient.SqlParameter('@OrderBy',bandtype+' desc'),
                   outputparam,
                }
                , ref retDic);

            int total=(int)retDic.First().Value;
            return list;
            </pre>
            ".Replace("'", "\"");
            }
        }

        public string Code
        {
            get
            {
                return this.sqlEditBox1.Text;
            }
            set
            {
                this.sqlEditBox1.Text = value;
                //TBCode.MarkKeyWords();
            }
        }

        public SqlProceCodePanel()
        {
            InitializeComponent();

            if(this.sqlEditBox1.ContextMenuStrip!=null)
            {
                this.sqlEditBox1.ContextMenuStrip.Items.Add("调用代码", null, new EventHandler((o, p) =>
                    {
                        SubForm.WinWebBroswer webb = new SubForm.WinWebBroswer();
                        webb.SetTitle("使用示例");
                        webb.SetBody(UseCode);
                        webb.ShowDialog();
                    }));
            }
        }

        private void InitializeComponent()
        {
            sqlEditBox1 = new SQLEditBox();
            this.SuspendLayout();
            // 
            // sqlEditBox1
            // 
            this.Controls.Add(sqlEditBox1);
            this.sqlEditBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            
            this.sqlEditBox1.Location = new System.Drawing.Point(0, 0);
            this.sqlEditBox1.Name = "sqlEditBox1";
            this.sqlEditBox1.Size = new System.Drawing.Size(674, 448);
            this.sqlEditBox1.TabIndex = 0;
            this.ResumeLayout(false);
        }
    }
}
