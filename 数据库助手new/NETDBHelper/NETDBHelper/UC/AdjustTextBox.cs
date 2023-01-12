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


        private Rectangle RecAdjustSize
        {
            get { return new Rectangle(this.Width - 15, this.Height - 15, 15, 15); }
        }
        private Rectangle RecAdjustPostion
        {
            get { return new Rectangle(0, 0, this.Width, this.Height); }
        }


        private void InitCtrl()
        {
            this.Text = "";
            this.Multiline = true;
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
    }
}
