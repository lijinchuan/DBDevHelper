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
    public partial class UCBasicAuth : UserControl
    {
        public UCBasicAuth()
        {
            InitializeComponent();
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
    }
}
