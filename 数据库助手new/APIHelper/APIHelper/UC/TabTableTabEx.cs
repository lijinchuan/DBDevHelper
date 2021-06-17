using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace APIHelper.UC
{
    internal class TabTableTabEx 
    {
        private RectangleF _stripRect;
        public RectangleF StripRect
        {
            get
            {
                return _stripRect;
            }
            set
            {
                _stripRect = value;
                if (value != RectangleF.Empty)
                {
                    _tabwidth = value.Width;
                }
            }
        }

        private float _tabwidth = 120;

        public float TabWidth
        {
            get
            {
                return _tabwidth;
            }
        }

        public int TabIndex
        {
            get;
            set;
        }

        public string Text
        {
            get
            {
                return TabPage.Text;
            }
        }

        public TabPage TabPage
        {
            get;
            set;
        }

        public Rectangle CloseButtonBand
        {
            get;
            set;
        }

        public TabTableTabEx(TabPage page,int tabindex)
        {
            this.TabPage = page;
            this.TabIndex = tabindex;
        }

        public void ClearRect()
        {
            StripRect = RectangleF.Empty;
            CloseButtonBand = Rectangle.Empty;
        }
    }
}
