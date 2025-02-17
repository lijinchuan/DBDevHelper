﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class WatingDlg : Form
    {
        public WatingDlg()
        {
            InitializeComponent();
            WatingDlg.CheckForIllegalCrossThreadCalls = false;

            this.loadingBox1.Location = new Point(0, 0);
            this.Width = this.loadingBox1.Width;
            this.Height = this.loadingBox1.Height;
        }

        public void Show(string msg)
        {
            this.Msg = msg;
            new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(200);
                this.DialogResult = DialogResult.Cancel;
                this.Visible = false;
                Thread.Sleep(200);
                this.ShowDialog();

            })).Start();
            this.ShowDialog();
            
        }

        public new void Hide()
        {
            Thread.Sleep(500);
            this.DialogResult = DialogResult.None;
            this.Visible = false;
        }

        public string Msg
        {
            set
            {
                this.loadingBox1.Msg = value;
            }
        }
    }
}
