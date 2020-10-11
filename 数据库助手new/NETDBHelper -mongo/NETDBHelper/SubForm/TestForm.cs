using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class TestForm : Form
    {
        public class Persion
        {
            public int No
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public bool Sex
            {
                get;
                set;
            }
        }

        public TestForm()
        {
            InitializeComponent();

            this.multSelectCombox1.DataSource = new List<string> { "男", "女", "1000", "2000", "3000", "4000" };
            this.multSelectCombox1.Text = "sfasdasfasdfasdf";

            List<Persion> persions = new List<Persion>();
            persions.Add(new Persion
            {
                No=1,
                Name="persion1",
                Sex=true
            });
            persions.Add(new Persion
            {
                No = 2,
                Name = "persion2",
                Sex = true
            });
            persions.Add(new Persion
            {
                No = 2,
                Name = "persion3",
                Sex = false
            });
            this.tableCombox1.DataSource = persions;

            this.dataGridView1.DataSource = persions;
        }

        
    }
}
