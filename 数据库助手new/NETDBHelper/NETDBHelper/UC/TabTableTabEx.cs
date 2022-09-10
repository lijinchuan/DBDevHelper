using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    internal class TabTableTabEx 
    {
        public RectangleF StripRect
        {
            get;
            set;
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

        public bool Visible
        {
            get;
            set;
        }

        public bool IsHidTitle
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
