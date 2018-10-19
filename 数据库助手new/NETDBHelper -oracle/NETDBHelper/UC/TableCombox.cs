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
    public partial class TableCombox : UserControl
    {
        public TableCombox()
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.None;
        }

        private Point mousePoint = new Point(0,0);
        DataGridView gridview = null;

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

            if (gridview == null)
            {
                this.gridview = new DataGridView();
                gridview.MouseLeave += panel1_MouseLeave;
                this.gridview.Visible = false;
                this.gridview.AutoSize = true;
                //this.gridview.Width = comboBox1.Width;
                this.gridview.BorderStyle = BorderStyle.None;
                this.gridview.BackColor = Color.White;
                this.gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                this.gridview.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                this.gridview.AllowUserToAddRows = false;
                this.gridview.AllowUserToDeleteRows = false;
                this.gridview.MouseDown += MultSelectCombox_MouseMove;
                
                this.gridview.Location = new Point(this.comboBox1.Location.X, this.comboBox1.Height + 3);
                this.Controls.Add(gridview);

                this.gridview.DataSource = Items;

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

        public TableCombox(List<object> items)
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
            if (this.gridview != null)
            {
                var location = this.PointToScreen(this.gridview.Location);
                var mouselocation = mousePoint;
                if (location.X > mousePoint.X || location.Y > mouselocation.Y
                    || location.X + this.gridview.Width < mousePoint.X || location.Y + this.gridview.Height < mouselocation.Y)
                {
                    gridview.Visible = false;
                }
            }
        }

        void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (this.gridview != null)
            {
                var location = this.PointToScreen(this.gridview.Location);
                var mouselocation = mousePoint;
                if (location.X > mousePoint.X || location.Y > mouselocation.Y
                    || location.X + this.gridview.Width < mousePoint.X || location.Y + this.gridview.Height < mouselocation.Y)
                {
                    gridview.Visible = false;
                }
            }
        }

        void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (this.gridview != null)
            {
                gridview.Visible = true;
                this.BringToFront();
            }
            
        }
    }
}
