using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class UCTextBox : UserControl
    {
        private int oldWidth = 0;
        private int oldHeight = 0;

        public UCTextBox()
        {
            InitializeComponent();
            TBValue.Enabled = false;
            CBNull.Checked = false;
            this.CBNull.CheckedChanged += CBNull_CheckedChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.TBValue.SizeChanged += TBValue_SizeChanged;
        }

        private void TBValue_SizeChanged(object sender, EventArgs e)
        {
            if (oldHeight == 0 || oldWidth == 0)
            {
                oldHeight = TBValue.Height;
                oldWidth = TBValue.Width;
                return;
            }

            this.Width += TBValue.Width - oldWidth;
            this.Height += TBValue.Height - oldHeight;

            oldHeight = TBValue.Height;
            oldWidth = TBValue.Width;
        }

        public bool ShowCheckBox
        {
            get
            {
                return CBNull.Visible;
            }
            set
            {
                CBNull.Visible = value;
                if (!value)
                {
                    TBValue.Width = Width - 2;
                    TBValue.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                    TBValue.Enabled = true;
                }
                else
                {
                    TBValue.Dock = DockStyle.None;
                    TBValue.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                }
            }
        }

        private void CBNull_CheckedChanged(object sender, EventArgs e)
        {
            TBValue.Enabled = CBNull.Checked;
        }

        public override string Text
        {
            get
            {
                if (CBNull.Visible && !CBNull.Checked)
                {
                    return null;
                }

                return TBValue.Text;
            }
            set
            {
                if (value == null)
                {
                    if (!CBNull.Visible)
                    {
                        throw new NotSupportedException("不支持NULL值");
                    }
                    CBNull.Checked = false;
                }
                else
                {
                    TBValue.Text = value;
                    if (CBNull.Visible && !CBNull.Checked)
                    {
                        CBNull.Checked = true;
                    }
                }
            }
        }

        public bool Multiline
        {
            get
            {
                return TBValue.Multiline;
            }
            set
            {
                TBValue.Multiline = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return TBValue.BackColor;
            }
            set
            {
                TBValue.BackColor = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return TBValue.ReadOnly;
            }
            set
            {
                TBValue.ReadOnly = value;
            }
        }

        public string DrawPrompt
        {
            get
            {
                return TBValue.DrawPrompt;
            }
            set
            {
                TBValue.DrawPrompt = value;
            }
        }
    }
}
