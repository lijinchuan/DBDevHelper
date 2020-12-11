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
    public partial class UCBearToken : UserControl
    {
        public UCBearToken()
        {
            InitializeComponent();
        }

        public string Token
        {
            get
            {
                return TBToken.Text;
            }
            set
            {
                TBToken.Text = value;
            }
        }
    }
}
