﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace NETDBHelper.UC
{
    public partial class LoadingBox : UserControl
    {
        #region 兼容旧的
        public object tag
        {
            get;
            set;
        }
        public event Action<object> OnStop;
        #endregion

        private Thread TaskThread
        {
            get;
            set;
        }

        private Action beforeCancel;

        public LoadingBox()
        {
            InitializeComponent();
        }

        public string Msg
        {
            set
            {
                this.lb1.Text = value;
            }
        }

        private void LoadingBox_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Resources.Resource1.loading2;
            if (this.TaskThread == null)
            {
                this.LlbStop.Visible = false;
            }
        }

        private void LlbStop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("停止吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (TaskThread != null)
                {
                    try
                    {
                        if (beforeCancel != null)
                        {
                            beforeCancel();
                        }
                        TaskThread.Abort();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        TaskThread = null;
                    }
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        public void Waiting(Control parent, Action act, Action cancel = null)
        {
            if (TaskThread != null)
            {
                return;
            }

            this.beforeCancel = cancel;
            this.Location = new Point((parent.Width - this.Width) / 2, (parent.Height - this.Height) / 2);
            parent.BeginInvoke(new Action(() =>
            {
                parent.Controls.Add(this);
                this.BringToFront();
            }));
            var thd = new Thread(new ThreadStart(() =>
            {
                try
                {
                    act();
                }
                catch (Exception ex)
                {
                    Util.SendMsg(this, ex.Message);
                }
                finally
                {
                    if (!parent.IsDisposed)
                    {
                        parent.BeginInvoke(new Action(() => { parent.Controls.Remove(this); parent.Invalidate(); }));
                    }
                    TaskThread = null;
                }
            }), 10240000);
            this.TaskThread = thd;

            thd.Start();
        }

        public void Waiting(Control parent, Action<object> act, object val, Action cancel)
        {
            if (TaskThread != null)
            {
                return;
            }
            this.Location = new Point((parent.Width - this.Width) / 2, (parent.Height - this.Height) / 2);
            parent.BeginInvoke(new Action(() =>
            {
                parent.Controls.Add(this);
                this.BringToFront();
            }));
            this.beforeCancel = cancel;
            var thd = new Thread(new ThreadStart(() =>
            {
                try
                {
                    act(val);
                }
                catch (Exception ex)
                {
                    Util.SendMsg(this, ex.Message);
                }
                finally
                {
                    if (!parent.IsDisposed)
                    {
                        parent.BeginInvoke(new Action(() => { parent.Controls.Remove(this); parent.Invalidate(); }));
                    }
                    TaskThread = null;
                }
            }));
            this.TaskThread = thd;

            thd.Start();
        }
    }
}
