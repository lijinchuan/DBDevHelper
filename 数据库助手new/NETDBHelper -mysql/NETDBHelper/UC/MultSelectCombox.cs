using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class MultSelectCombox : UserControl
    {
        private Point mousePoint = new Point(0,0);
        Panel panel1 = null;

        protected IEnumerable<object> Items
        {
            get;
            set;
        }

        public List<object> SelectedValues
        {
            get;
            set;
        }

        public new string Text
        {
            get;
            set;
        }

        public IEnumerable<object> DataSource
        {
            get
            {
                return Items;
            }
            set
            {
                Items = value;
                if (value != null)
                {
                    BindData();
                }
            }
        }

        private void BindData()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            if (panel1 == null)
            {
                this.panel1 = new Panel();
                panel1.MouseLeave += panel1_MouseLeave;
                this.panel1.Visible = false;
                this.panel1.Width = comboBox1.Width;
                this.panel1.BorderStyle = BorderStyle.FixedSingle;
                this.panel1.AutoScroll = true;
                this.panel1.BackColor = Color.White;
                this.panel1.MouseDown += MultSelectCombox_MouseMove;
                this.panel1.Location = new Point(this.comboBox1.Location.X, this.comboBox1.Height + 2);
                this.Controls.Add(panel1);
            }

            this.panel1.Controls.Clear();
            SelectedValues.Clear();

            if (Items != null && Items.Count() > 0)
            {
                int y = 2;
                int offsetX = 5;
                int offsetY = y;
                foreach (var item in Items)
                {
                    CheckBox cb = new CheckBox();
                    cb.Text = item.ToString();
                    cb.Tag = item;
                    cb.MouseMove += MultSelectCombox_MouseMove;

                    cb.Location = new Point(offsetX, offsetY);
                    offsetY += cb.Height + y;

                    cb.CheckedChanged += cb_CheckedChanged;

                    panel1.Controls.Add(cb);
                }

                if (Items.Count() <= 5)
                {
                    panel1.Height = offsetY + y;
                }
                else
                {
                    panel1.Height = 5 * (offsetY) / Items.Count();
                    
                }
                
                comboBox1.DropDown += comboBox1_DropDown;
                comboBox1.DropDownClosed += comboBox1_DropDownClosed;
            }
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (sender as CheckBox);
            
            if (cb.Checked)
            {
                if (!SelectedValues.Contains(cb.Tag))
                {
                    SelectedValues.Add(cb.Tag);
                }
            }
            else
            {
                SelectedValues.Remove(cb.Tag);
            }

            var vals = string.Join("、", SelectedValues);
            if (vals.Length > 15)
            {
                vals = vals.Substring(0, 15) + "...";
            }

            comboBox1.Text = vals;
        }

        public MultSelectCombox()
        {
            InitializeComponent();

            this.Width = this.comboBox1.Width;
            this.Height = this.comboBox1.Height;
            
        }

        public MultSelectCombox(List<object> items)
            : this()
        {
            Items = items;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            this.MouseMove += MultSelectCombox_MouseMove;
            
            this.comboBox1.DropDownHeight = 1;
           
            SelectedValues = new List<object>();

            BindData();
        }

        void MultSelectCombox_MouseMove(object sender, MouseEventArgs e)
        {
            this.mousePoint =(sender as Control).PointToScreen(e.Location);
        }

        void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if (this.panel1 != null)
            {
                var location = this.PointToScreen(this.panel1.Location);
                var mouselocation = mousePoint;
                if (location.X > mousePoint.X || location.Y > mouselocation.Y
                    || location.X + this.panel1.Width < mousePoint.X || location.Y + this.panel1.Height < mouselocation.Y)
                {
                    panel1.Visible = false;
                }
            }
        }

        void panel1_MouseLeave(object sender, EventArgs e)
        {
        }

        void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (this.panel1 != null)
            {
                panel1.Visible = true;
                this.BringToFront();;
            }
            
        }

    }
}
