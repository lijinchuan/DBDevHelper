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
    public partial class MyTabControl : TabControl
    {
        const int CLOSE_SIZE = 15;
        private static int DEF_START_POS_Default = 20;
        int DEF_START_POS = DEF_START_POS_Default;
        int mtop = 3;
        private static Font defaultFont = new Font("Tahoma", 8.25f, FontStyle.Regular);
        private ToolStripProfessionalRenderer renderer = new ToolStripProfessionalRenderer();

        private Dictionary<int, TabTableTabEx> tabExDic = new Dictionary<int, TabTableTabEx>();

        public MyTabControl()
        {
            InitializeComponent();

            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            
            this.DrawItem += new DrawItemEventHandler(MyTabControl_DrawItem);
            this.SizeMode = TabSizeMode.Fixed;

            this.ItemSize = new Size { Width = 0, Height = 18 };

            this.SetStyle(ControlStyles.UserPaint, true);

        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.Parent.MouseClick += Parent_MouseClick;
        }

        void Parent_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                tabExDic.Add(TabCount - 1, new TabTableTabEx((TabPage)e.Control, TabCount - 1));
                this.SelectedTab = (e.Control as TabPage);
            }

            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            //tabExDic.Clear();

            int index = int.MaxValue;
            var newdic = new Dictionary<int, TabTableTabEx>();
            foreach (var tab in tabExDic)
            {
                if (tab.Value.TabPage == e.Control)
                {
                    index = tab.Key;
                }
                if (tab.Key < index)
                {
                    newdic.Add(tab.Key, new TabTableTabEx(tab.Value.TabPage, tab.Key));
                }
                if (tab.Key > index)
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

        private void OnCalcTabPage(Graphics g, TabPage currentItem, StringFormat sf, ref RectangleF stripRect)
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
            foreach (var item in this.tabExDic)
            {
                MyTabControl_DrawItem_Tab(item.Key, e.Graphics);
            }
        }

        void MyTabControl_DrawItem_Tab(int tabindex, Graphics g)
        {
            var currentItem = this.TabPages[tabindex];
            var SelectedItem = this.SelectedTab;

            Font currentFont = defaultFont;
            var sf = StringFormat.GenericDefault;
            RectangleF buttonRect = tabExDic[tabindex].StripRect;

            if (buttonRect.Size == SizeF.Empty)
            {
                OnCalcTabPage(g, currentItem, sf, ref buttonRect);
                tabExDic[tabindex].StripRect = buttonRect;
            }

            currentItem.BorderStyle = BorderStyle.None;
            bool isFirstTab = buttonRect.X < 50;

            SizeF textSize = g.MeasureString(currentItem.Text, currentFont, new SizeF(200, 10), sf);
            textSize.Width += 20;

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
                    g.DrawString(currentItem.Text, currentFont, new SolidBrush(Color.LightSeaGreen), textRect, sf);
                }
                else
                {
                    g.DrawString(currentItem.Text, currentFont, new SolidBrush(ForeColor), textRect, sf);
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
