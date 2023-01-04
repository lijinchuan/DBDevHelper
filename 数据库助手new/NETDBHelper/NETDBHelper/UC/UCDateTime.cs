using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace NETDBHelper.UC
{
    public partial class UCDateTime: DateTimePicker
    {
        private Color _backDisabledColor;

        public UCDateTime() : base()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            _backDisabledColor = Color.FromKnownColor(KnownColor.Control);

            base.ValueChanged += UCDateTime_ValueChanged;
        }

        private void UCDateTime_ValueChanged(object sender, EventArgs e)
        {
            this._val = base.Value;
        }

        /// <summary>
        ///     Gets or sets the background color of the control
        /// </summary>
        [Browsable(true)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private DateTime? _val = null;
        public new DateTime? Value
        {
            get
            {
                return _val;
            }
            set
            {
                _val = value;
                if (value != null)
                {
                    base.Value = value.Value;
                }
                else
                {
                    base.Value = DateTime.MinValue;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the background color of the control when disabled
        /// </summary>
        [Category("Appearance"), Description("The background color of the component when disabled")]
        [Browsable(true)]
        public Color BackDisabledColor
        {
            get { return _backDisabledColor; }
            set { _backDisabledColor = value; }
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            //Graphics g = e.Graphics;

            //The dropDownRectangle defines position and size of dropdownbutton block, 
            //the width is fixed to 17 and height to 16. The dropdownbutton is aligned to right
            Rectangle dropDownRectangle = new Rectangle(ClientRectangle.Width - 17, 0, 17, 16);
            Brush bkgBrush;
            ComboBoxState visualState;

            //When the control is enabled the brush is set to Backcolor, 
            //otherwise to color stored in _backDisabledColor
            if (this.Enabled)
            {
                bkgBrush = new SolidBrush(this.BackColor);
                visualState = ComboBoxState.Normal;
            }
            else
            {
                bkgBrush = new SolidBrush(this._backDisabledColor);
                visualState = ComboBoxState.Disabled;
            }

            // Painting...in action

            //Filling the background
            g.FillRectangle(bkgBrush, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            //Drawing the datetime text
            g.DrawString(this.Text, this.Font, Brushes.Black, 0, 2);

            //Drawing the dropdownbutton using ComboBoxRenderer
            ComboBoxRenderer.DrawDropDownButton(g, dropDownRectangle, visualState);

            g.Dispose();
            bkgBrush.Dispose();
        }
    }
}
