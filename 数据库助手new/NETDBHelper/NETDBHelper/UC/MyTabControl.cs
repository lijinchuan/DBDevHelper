using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NETDBHelper.UC
{
    public partial class MyTabControl :TabControl
    {
        const int CLOSE_SIZE = 15;
        private static int DEF_START_POS_Default = 20;
        int DEF_START_POS = DEF_START_POS_Default;
        int mtop = 3;
        private static Font defaultFont = new Font("Tahoma", 8.25f, FontStyle.Regular);
        private ToolStripProfessionalRenderer renderer = new ToolStripProfessionalRenderer();

        private Dictionary<int, TabTableTabEx> tabExDic = new Dictionary<int, TabTableTabEx>();

        private Rectangle moreTabRect = Rectangle.Empty;

        private ListBox morelistbox = null;
        private List<TabTableTabEx> moretabtablelist = new List<TabTableTabEx>();

        private ContextMenuStrip TagPageContextMenuStrip = new ContextMenuStrip();

        public MyTabControl()
        {
            InitializeComponent();
            
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.DrawItem += new DrawItemEventHandler(MyTabControl_DrawItem);
            this.SizeMode = TabSizeMode.Fixed;
            
            this.ItemSize = new Size { Width = 0,Height=18 };
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            morelistbox = new ListBox();
            morelistbox.BorderStyle = BorderStyle.FixedSingle;
            morelistbox.Visible = false;
            morelistbox.BackColor = Color.LightYellow;
            
            morelistbox.ItemHeight = 22;

            morelistbox.DoubleClick += Morelistbox_DoubleClick;
            morelistbox.MouseLeave += Morelistbox_MouseLeave;


            TagPageContextMenuStrip.Items.Add("关闭其它");
            TagPageContextMenuStrip.Items.Add("重命名");
            TagPageContextMenuStrip.ItemClicked += TagPageContextMenuStrip_ItemClicked;

        }

        private void TagPageContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TagPageContextMenuStrip.Visible = false;
            switch (e.ClickedItem.Text)
            {
                case "关闭其它":
                    {
                        if (MessageBox.Show("确认要关闭其它选项页吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            break;
                        }
                        List<TabPage> tbpages = new List<TabPage>();
                        foreach (var tab in tabExDic)
                        {
                            if (this.SelectedIndex != tab.Key)
                            {
                                tbpages.Add(tab.Value.TabPage);
                            }
                        }
                        moretabtablelist.Clear();
                        morelistbox.DataSource = moretabtablelist;
                        //morelistbox.Visible = false;
                        foreach (var tab in tbpages)
                        {
                            this.TabPages.Remove(tab);
                        }
                        break;
                    }
                case "重命名":
                    {
                        SubForm.InputStringDlg inputStringDlg = new SubForm.InputStringDlg("重命名",this.SelectedTab.Text);
                        if (inputStringDlg.ShowDialog() == DialogResult.OK)
                        {
                            if (!string.IsNullOrWhiteSpace(inputStringDlg.InputString))
                            {
                                this.SelectedTab.Text = inputStringDlg.InputString;
                                this.Invalidate();
                            }
                        }
                        break;
                    }
            }
        }

        private void Morelistbox_MouseLeave(object sender, EventArgs e)
        {
            morelistbox.Visible = false;
        }

        private void Morelistbox_DoubleClick(object sender, EventArgs e)
        {
            var selitem = (TabTableTabEx)morelistbox.SelectedItem;
            this.morelistbox.Visible = false;
            this.morelistbox.DataSource = null;

            if (selitem != null)
            {
                var pos = Math.Max(this.TabPages.Count - this.moretabtablelist.Count - 1, 0);

                for(var i = pos; i >= 0; i--)
                {
                    this.tabExDic[i].TabIndex--;
                }

                var lasttabex = this.tabExDic.AsEnumerable().ToArray()[pos].Value;
                selitem.TabIndex = pos;

                selitem.TabIndex = pos;
                if (pos != 0)
                {
                    tabExDic[0].TabIndex = tabExDic.Count;
                }
                var tabs = tabExDic.AsEnumerable().OrderBy(p => p.Value.TabIndex).Select(p => p.Value.TabPage).ToArray();
                this.tabExDic.Clear();
                this.TabPages.Clear();
                this.TabPages.AddRange(tabs);
                this.SelectedTab = selitem.TabPage;

                this.Invalidate();
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.Parent.MouseClick += Parent_MouseClick;
            if(!this.Parent.Controls.Contains(morelistbox))
            {
                this.Parent.Controls.Add(morelistbox);
            }
        }

        void Parent_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (moreTabRect.Contains(e.X, e.Y))
                {
                    if (morelistbox.Visible)
                    {
                        morelistbox.Visible = false;
                        morelistbox.DataSource = null;
                    }
                    else
                    {

                        morelistbox.DataSource = moretabtablelist;
                        var maxwidth = 0;
                        foreach (var item in moretabtablelist)
                        {
                            var w = this.CreateGraphics().MeasureString(item.Text, morelistbox.Font);
                            if (w.Width > maxwidth)
                            {
                                maxwidth = (int)w.Width;
                            }
                        }
                        morelistbox.Width = maxwidth;
                        morelistbox.DisplayMember = "Text";

                        morelistbox.Location = new Point(moreTabRect.X - morelistbox.Width - 1, moreTabRect.Height * 2);
                        morelistbox.Visible = true;
                        morelistbox.BringToFront();
                    }
                }
                else
                {

                    foreach (var tab in tabExDic)
                    {
                        if (tab.Value.StripRect.Contains(e.X, e.Y))
                        {
                            if (this.SelectedIndex != tab.Key)
                            {
                                this.SelectedIndex = tab.Key;
                                this.Invalidate();
                            }
                            else if (tab.Value.CloseButtonBand.Contains(e.X, e.Y))
                            {
                                this.TabPages.Remove(tab.Value.TabPage);
                                break;
                            }
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                foreach (var tab in tabExDic)
                {
                    if (this.SelectedIndex == tab.Key)
                    {
                        if (tab.Value.StripRect.Contains(e.X, e.Y))
                        {
                            this.TagPageContextMenuStrip.Visible = true;
                            var pt=PointToScreen(new Point(e.X, e.Y));
                            this.TagPageContextMenuStrip.Left = pt.X;
                            this.TagPageContextMenuStrip.Top = pt.Y;
                            break;
                        }
                    }
                }
            }
        }

        protected override void OnSelected(TabControlEventArgs e)
        {

            base.OnSelected(e);

            this.Invalidate();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            SizeF textSize = this.CreateGraphics().MeasureString(e.Control.Text, defaultFont, new SizeF(200, 10), StringFormat.GenericDefault);
            textSize.Width += 35;
            
            if (moretabtablelist.Count > 0 || (this.tabExDic.Count > 0 && this.Width - this.tabExDic.Last().Value.StripRect.Location.X - this.tabExDic.Last().Value.StripRect.Width < 20 + textSize.Width))
            {
                var tabpage = (TabPage)e.Control;
                var list = new List<TabPage>();
                var items = this.tabExDic.Select(p => p.Value.TabPage).ToList();
                if (items.Count > 1)
                {
                    items.Add(items.First());
                    items = items.Skip(1).ToList();
                }
                list.AddRange(items);
                list.Insert(Math.Max(0, this.tabExDic.Count - this.moretabtablelist.Count - 1), tabpage);

                this.SuspendLayout();
                tabExDic.Clear();
                this.TabPages.Clear();
                moretabtablelist.Clear();
                this.TabPages.AddRange(list.ToArray());
                this.SelectedTab = tabpage;
                this.ResumeLayout();
            }
            else
            {
                if (e.Control is TabPage)
                {
                    tabExDic.Add(TabCount - 1, new TabTableTabEx((TabPage)e.Control, TabCount - 1));
                }
            }

            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            //tabExDic.Clear();

            int index = int.MaxValue;
            var newdic = new Dictionary<int, TabTableTabEx>();
            foreach(var tab in tabExDic)
            {
                if (tab.Value.TabPage == e.Control)
                {
                    index = tab.Key;
                }
                if(tab.Key<index)
                {
                    newdic.Add(tab.Key, new TabTableTabEx(tab.Value.TabPage, tab.Key));
                }
                if(tab.Key>index)
                {
                    newdic.Add(tab.Key - 1, new TabTableTabEx(tab.Value.TabPage, tab.Key - 1));
                }
            }
            tabExDic.Clear();
            tabExDic = newdic;
            DEF_START_POS = DEF_START_POS_Default;
            base.OnControlRemoved(e);
        }

        
        private void DrawCross(Rectangle crossRect, bool isMouseOver, Graphics g)
        {
            if (isMouseOver)
            {
                Color fill = renderer.ColorTable.ButtonSelectedHighlight;

                g.FillRectangle(new SolidBrush(fill), crossRect);

                Rectangle borderRect = crossRect;

                borderRect.Width--;
                borderRect.Height--;

                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }

            using (Pen pen = new Pen(Color.Black, 1.6f))
            {
                g.DrawLine(pen, crossRect.Left + 3, crossRect.Top + 3,
                    crossRect.Right - 5, crossRect.Bottom - 4);

                g.DrawLine(pen, crossRect.Right - 5, crossRect.Top + 3,
                    crossRect.Left + 3, crossRect.Bottom - 4);
            }
        }

        private void OnCalcTabPage(Graphics g, TabPage currentItem,StringFormat sf,ref RectangleF stripRect)
        {
            Font currentFont = defaultFont;
            //if (currentItem == SelectedTab)
            //    currentFont = new Font(Font, FontStyle.Bold);

            SizeF textSize = g.MeasureString(currentItem.Text, currentFont, new SizeF(200, 10), sf);
            textSize.Width += 35;

            if (RightToLeft == RightToLeft.No)
            {
                RectangleF buttonRect = new RectangleF(DEF_START_POS, mtop, textSize.Width, 17);
                stripRect = buttonRect;
                DEF_START_POS += (int)textSize.Width;
            }
            else
            {
                RectangleF buttonRect = new RectangleF(DEF_START_POS - textSize.Width + 1, mtop, textSize.Width - 1, 17);
                stripRect = buttonRect;
                DEF_START_POS -= (int)textSize.Width;
            }
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float currwidth = 0;
            moretabtablelist.Clear();
            var isfull = false;
            foreach (var item in this.tabExDic)
            {

                if (!isfull)
                {
                    currwidth += item.Value.StripRect.Width;
                    if (currwidth > this.Width - 20)
                    {
                        isfull = true;
                    }
                }
                if (isfull)
                {
                    moretabtablelist.Add(item.Value);
                }
                else
                {
                    MyTabControl_DrawItem_Tab(item.Key, e.Graphics);
                }
            }
            if (isfull)
            {
                var point = new Point(this.Width - 15, 5);
                moreTabRect = new Rectangle(point, new Size(Resources.Resource1.bullet_eject.Width,
                    Resources.Resource1.bullet_eject.Height));
                e.Graphics.DrawImage(Resources.Resource1.bullet_eject, point);
            }
            else
            {
                if (this.morelistbox.Visible)
                {
                    this.morelistbox.Visible = false;
                    this.morelistbox.DataSource = null;
                }
            }
        }

        void MyTabControl_DrawItem_Tab(int tabindex,Graphics g)
        {
            var currentItem=this.TabPages[tabindex];
            var SelectedItem=this.SelectedTab;
            
            Font currentFont = defaultFont;
            var sf=StringFormat.GenericDefault;
            RectangleF buttonRect = tabExDic[tabindex].StripRect;
            
            if (buttonRect.Size == SizeF.Empty)
            {
                OnCalcTabPage(g, currentItem,sf, ref buttonRect);
                tabExDic[tabindex].StripRect = buttonRect;
            }

            currentItem.BorderStyle = BorderStyle.None;
            bool isFirstTab = buttonRect.X < 50;

            var txt = currentItem.Text;
            SizeF textSize = g.MeasureString(currentItem.Text, currentFont, new SizeF(200, 10), sf);
            textSize.Width += 20;
            var maxw = currentItem == SelectedItem ? 190 : 200;
            if (textSize.Width > maxw)
            {
                while (g.MeasureString(txt = txt.Substring(0, txt.Length - 1), currentFont, new SizeF(200, 10), sf).Width > maxw) ;
                txt += "...";
            }

            GraphicsPath path = new GraphicsPath();
            LinearGradientBrush brush;

            #region Draw Not Right-To-Left Tab

            if (RightToLeft == RightToLeft.No)
            {
                if (currentItem == SelectedItem || isFirstTab)
                {
                    path.AddLine(buttonRect.Left - 10, buttonRect.Bottom - 1,
                                 buttonRect.Left + (buttonRect.Height / 2) - 4, mtop + 4);
                }
                else
                {
                    path.AddLine(buttonRect.Left, buttonRect.Bottom - 1, buttonRect.Left,
                                 buttonRect.Bottom - (buttonRect.Height / 2) - 2);
                    path.AddLine(buttonRect.Left, buttonRect.Bottom - (buttonRect.Height / 2) - 3,
                                 buttonRect.Left + (buttonRect.Height / 2) - 4, mtop + 3);
                }

                path.AddLine(buttonRect.Left + (buttonRect.Height / 2) + 2, mtop, buttonRect.Right - 3, mtop);
                path.AddLine(buttonRect.Right, mtop + 2, buttonRect.Right, buttonRect.Bottom - 1);
                path.AddLine(buttonRect.Right - 4, buttonRect.Bottom - 1, buttonRect.Left, buttonRect.Bottom - 1);
                path.CloseFigure();
                
                if (currentItem == SelectedItem)
                {
                    brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Window, LinearGradientMode.Vertical);
                }
                else
                {
                    brush = new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical);
                }

                g.FillPath(brush, path);
                g.DrawPath(SystemPens.ControlDark, path);

                if (currentItem == SelectedItem)
                {
                    g.DrawLine(new Pen(brush), buttonRect.Left - 9, buttonRect.Height + 2,
                               buttonRect.Left + buttonRect.Width - 1, buttonRect.Height + 2);
                }

                PointF textLoc = new PointF(buttonRect.Left + buttonRect.Height - 4, buttonRect.Top + (buttonRect.Height / 2) - (textSize.Height / 2) - 3);
                RectangleF textRect = buttonRect;
                textRect.Location = textLoc;
                textRect.Width = buttonRect.Width - (textRect.Left - buttonRect.Left) - 4;
                textRect.Height = textSize.Height + currentFont.Size / 2;

                if (currentItem == SelectedItem)
                {
                    //textRect.Y -= 2;
                    g.DrawString(txt, currentFont, new SolidBrush(Color.LightSeaGreen), textRect, sf);
                }
                else
                {
                    g.DrawString(txt, currentFont, new SolidBrush(ForeColor), textRect, sf);
                }
            }

            #endregion

            #region Draw Right-To-Left Tab

            if (RightToLeft == RightToLeft.Yes)
            {
                if (currentItem == SelectedItem || isFirstTab)
                {
                    path.AddLine(buttonRect.Right + 10, buttonRect.Bottom - 1,
                                 buttonRect.Right - (buttonRect.Height / 2) + 4, mtop + 4);
                }
                else
                {
                    path.AddLine(buttonRect.Right, buttonRect.Bottom - 1, buttonRect.Right,
                                 buttonRect.Bottom - (buttonRect.Height / 2) - 2);
                    path.AddLine(buttonRect.Right, buttonRect.Bottom - (buttonRect.Height / 2) - 3,
                                 buttonRect.Right - (buttonRect.Height / 2) + 4, mtop + 3);
                }

                path.AddLine(buttonRect.Right - (buttonRect.Height / 2) - 2, mtop, buttonRect.Left + 3, mtop);
                path.AddLine(buttonRect.Left, mtop + 2, buttonRect.Left, buttonRect.Bottom - 1);
                path.AddLine(buttonRect.Left + 4, buttonRect.Bottom - 1, buttonRect.Right, buttonRect.Bottom - 1);
                path.CloseFigure();

                if (currentItem == SelectedItem)
                {
                    brush =
                        new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Window,
                                                LinearGradientMode.Vertical);
                }
                else
                {
                    brush =
                        new LinearGradientBrush(buttonRect, SystemColors.ControlLightLight, SystemColors.Control,
                                                LinearGradientMode.Vertical);
                }

                g.FillPath(brush, path);
                g.DrawPath(SystemPens.ControlDark, path);

                if (currentItem == SelectedItem)
                {
                    g.DrawLine(new Pen(brush), buttonRect.Right + 9, buttonRect.Height + 2,
                               buttonRect.Right - buttonRect.Width + 1, buttonRect.Height + 2);
                }

                PointF textLoc = new PointF(buttonRect.Left + 2, buttonRect.Top + (buttonRect.Height / 2) - (textSize.Height / 2) - 2);
                RectangleF textRect = buttonRect;
                textRect.Location = textLoc;
                textRect.Width = buttonRect.Width - (textRect.Left - buttonRect.Left) - 10;
                textRect.Height = textSize.Height + currentFont.Size / 2;

                if (currentItem == SelectedItem)
                {
                    textRect.Y -= 1;
                    g.DrawString(currentItem.Text, currentFont, new SolidBrush(ForeColor), textRect, sf);
                }
                else
                {
                    g.DrawString(currentItem.Text, currentFont, new SolidBrush(ForeColor), textRect, sf);
                }

                //g.FillRectangle(Brushes.Red, textRect);
            }
            #endregion

            //关闭符号
            if (currentItem == SelectedTab)
            {
                var closeButtonBounds = tabExDic[tabindex].CloseButtonBand;
                if (closeButtonBounds == Rectangle.Empty)
                {
                    tabExDic[tabindex].CloseButtonBand =
                        closeButtonBounds = new Rectangle((int)buttonRect.Right - 20, mtop, 16, 16);
                }
                DrawCross(closeButtonBounds, false, g);
            }

            
        }

        void MyTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                MyTabControl_DrawItem_Tab(e.Index, e.Graphics);

                e.Graphics.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }
}
