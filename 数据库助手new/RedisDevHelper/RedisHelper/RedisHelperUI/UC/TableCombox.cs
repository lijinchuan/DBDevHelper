using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RedisHelperUI.UC
{
    public partial class TableCombox : UserControl
    {
        public event Action<object> TextChanged;

        public TableCombox()
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.None;

            //this.comboBox1.TextChanged+=comboBox1_TextChanged;
        }

        void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.gridview != null && this.gridview.CurrentRow != null)
            {
                if (this.TextChanged != null)
                {
                    this.TextChanged(this.gridview.CurrentRow.DataBoundItem);
                }
            }
        }

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

        public override string Text
        {
            get
            {
                return comboBox1.Text;
            }
            set
            {
                comboBox1.Text = value;
            }
        }

        private void BindData()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            if (Items == null)
            {
                return;
            }

            if (gridview == null)
            {
                this.gridview = new DataGridView();
                gridview.MouseLeave += panel1_MouseLeave;
                this.gridview.Visible = false;
                this.gridview.AutoSize = false;
                //this.gridview.Width = comboBox1.Width;
                this.gridview.BorderStyle = BorderStyle.None;
                this.gridview.BackColor = Color.White;
                this.gridview.BackgroundColor = Color.White;
                this.gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.gridview.Height = 300;
                this.gridview.ScrollBars = ScrollBars.Vertical;
                this.gridview.AllowUserToAddRows = false;
                this.gridview.AllowUserToDeleteRows = false;
                this.gridview.MultiSelect = false;
                this.gridview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                this.gridview.RowsAdded += gridview_RowsAdded;
                this.gridview.DataBindingComplete += gridview_DataBindingComplete;

                this.gridview.DoubleClick += gridview_DoubleClick;

                this.Parent.Controls.Add(gridview);

                var pt = this.Parent.PointToClient(this.PointToScreen(this.comboBox1.Location));
                this.gridview.Location = new Point(pt.X, pt.Y + this.comboBox1.Height + 2);

                comboBox1.DropDown += comboBox1_DropDown;
                comboBox1.DropDownClosed += comboBox1_DropDownClosed;
            }

            this.gridview.DataSource = Items;
            this.gridview.BringToFront();
        }

        void gridview_DoubleClick(object sender, EventArgs e)
        {
            var selrow = this.gridview.CurrentRow;
            this.Text = selrow.DataBoundItem.ToString();
            this.gridview.Visible = false;

            if (TextChanged != null)
            {
                TextChanged(selrow.DataBoundItem);
            }
        }

        void gridview_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int newheight = 0;
            int cnt = 0;
            foreach (DataGridViewRow row in gridview.Rows)
            {
                newheight += row.Height;
                cnt++;
                if (cnt > 10)
                {
                    break;
                }
            }
            //if (newheight < gridview.Height)
            {
                gridview.Height = newheight+23;
            }
        }

        void gridview_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
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
            
            this.comboBox1.DropDownHeight = 1;
           
            SelectedValues = new List<object>();

            BindData();
        }

        void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if (this.gridview != null)
            {
            }
        }

        void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (this.gridview != null)
            {
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
