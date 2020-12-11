using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.UC.Auth
{
    public partial class UCApiKey : UserControl
    {
        private static string[] ADDTOITMES = new[] { "header", "query params" };
        public UCApiKey()
        {
            InitializeComponent();
            this.CBAddTo.Items.AddRange(ADDTOITMES);
            this.CBAddTo.SelectedItem = ADDTOITMES[0];
        }

        public string Key
        {
            get
            {
                return TBKey.Text;
            }
            set
            {
                TBKey.Text = value;
            }
        }

        public string Val
        {
            get
            {
                return TBValue.Text;
            }
            set
            {
                TBValue.Text = value;
            }
        }

        /// <summary>
        /// 0-添加到头，2添加到URL
        /// </summary>
        /// <returns></returns>
        public int AddTo
        {
            get
            {
                for (int i = 0; i < ADDTOITMES.Length; i++)
                {
                    if (CBAddTo.SelectedItem.Equals(ADDTOITMES[i]))
                    {
                        return i;
                    }

                }
                return 0;
            }
            set
            {
                if (value < ADDTOITMES.Length)
                {
                    this.CBAddTo.SelectedItem = ADDTOITMES[value];
                }
                else
                {
                    this.CBAddTo.SelectedItem = ADDTOITMES[0];
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
        }
    }
}
