using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public enum EnumAdjust
    {
        Nothing = 0,
        Size = 1
    }


    public partial class AdjustTextBox : TextBox
    {
        public AdjustTextBox():base()
        {
            InitCtrl();
        }


        public AdjustTextBox(IContainer container) : base()
        {
            container.Add(this);
            InitCtrl();
        }


        private bool m_isMoving = false;
        private Point m_offset = new Point(0, 0);
        private Size m_intSize = new Size(0, 0);
        private EnumAdjust m_adjust = EnumAdjust.Nothing;
        private int m_x, m_y, m_w, m_h = 0;

        private const int WM_PAINT = 0x000F;
        private const int WM_SETFOCUS = 0x0007;

        private const int WM_KILLFOCUS = 0x0008;

        private Rectangle RecAdjustSize
        {
            get { return new Rectangle(this.Width - 15, this.Height - 15, 15, 15); }
        }
        private Rectangle RecAdjustPostion
        {
            get { return new Rectangle(0, 0, this.Width, this.Height); }
        }

        private Color _promptColor = Color.Gray;
        private bool _drawPrompt = true;

        public string DrawPrompt { get; set; }


        private void InitCtrl()
        {
            //this.Text = "";
            //this.Multiline = true;
            this.BorderStyle = BorderStyle.FixedSingle;
        }
        #region 鼠标事件
        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_isMoving = true;
            m_offset = new Point(e.X, e.Y);
            m_intSize.Width = this.Width;
            m_intSize.Height = this.Height;


            if (RecAdjustSize.Contains(e.X, e.Y)) //调整大小
            {
                this.Cursor = Cursors.SizeNWSE;
                m_adjust = EnumAdjust.Size;
            }
            else
            {
                m_adjust = EnumAdjust.Nothing;
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            Cursor = Cursors.Default;
            m_isMoving = false;

            base.OnMouseUp(mevent);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            CursorCtrl(e.X, e.Y);
            m_x = this.Left;
            m_y = this.Top;
            m_w = this.Width;
            m_h = this.Height;
            if (m_isMoving)
            {
                switch (m_adjust)
                {
                    case EnumAdjust.Size:
                        m_w = m_intSize.Width + (e.X - m_offset.X);
                        m_h = m_intSize.Height + (e.Y - m_offset.Y);
                        break;
                }
                this.SetBounds(m_x, m_y, m_w, m_h);
            }
            base.OnMouseMove(e);
        }
        #endregion



        private void CursorCtrl(int x, int y)
        {
            if (!m_isMoving && RecAdjustSize.Contains(x, y))
            {
                this.Cursor = Cursors.SizeNWSE;
                return;
            }
            this.Cursor = Cursors.Default;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WM_SETFOCUS:
                    _drawPrompt = false;
                    break;

                case WM_KILLFOCUS:
                    _drawPrompt = true;
                    break;
            }

            base.WndProc(ref m);

            // Only draw the prompt on the WM_PAINT event
            // and when the Text property is empty
            if (m.Msg == WM_PAINT && _drawPrompt && this.Text.Length == 0 &&
                              !this.GetStyle(ControlStyles.UserPaint))
                DrawTextPrompt(this.CreateGraphics());
        }

        protected virtual void DrawTextPrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding |
              TextFormatFlags.Top | TextFormatFlags.EndEllipsis;
            Rectangle rect = this.ClientRectangle;

            // Offset the rectangle based on the HorizontalAlignment, 
            // otherwise the display looks a little strange
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags = flags | TextFormatFlags.HorizontalCenter;
                    rect.Offset(0, 1);
                    break;

                case HorizontalAlignment.Left:
                    flags = flags | TextFormatFlags.Left;
                    rect.Offset(1, 1);
                    break;

                case HorizontalAlignment.Right:
                    flags = flags | TextFormatFlags.Right;
                    rect.Offset(0, 1);
                    break;
            }

            // Draw the prompt text using TextRenderer
            TextRenderer.DrawText(g, DrawPrompt, this.Font, rect,
                              _promptColor, this.BackColor, flags);
        }
    }
}
