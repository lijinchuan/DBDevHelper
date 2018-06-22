using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedisHelperUI
{
    public partial class SubInsertForm : Form
    {
        public RedisType RedisType
        {
            get;
            set;
        }

        public RedisHelper.Model.RedisServerEntity RedisServer
        {
            get;
            set;
        }

        private string _key;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                this.TBKey.Text = value;
            }
        }

        public string SubKey
        {
            get;
            private set;
        }

        public RedisValue Val
        {
            get;
            private set;
        }

        public SubInsertForm()
        {
            InitializeComponent();
        }

        private Func<bool> IsValid
        {
            get;
            set;
        }

        public Action<string> OnAdd
        {
            get;
            private set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.TBKey.Enabled = false;
            switch (RedisType)
            {
                case RedisType.Hash:
                    {
                        Label labmembertext = new Label();
                        labmembertext.Text = "member:";
                        this.Controls.Add(labmembertext);
                        labmembertext.TextAlign = ContentAlignment.MiddleCenter;
                        labmembertext.Width = LabKey.Width;
                        labmembertext.Left = LabKey.Left;
                        labmembertext.Top = LabKey.Top + LabKey.Height + 19;

                        TextBox tbmebmerbox = new TextBox();
                        this.Controls.Add(tbmebmerbox);
                        tbmebmerbox.Width = TBKey.Width;
                        tbmebmerbox.Left = TBKey.Left;
                        tbmebmerbox.Top = TBKey.Top + TBKey.Height + 15;

                        tbmebmerbox.TextChanged += (o, ev) =>
                            {
                                this.SubKey = tbmebmerbox.Text.Trim();
                            };

                        Label labvaluetext = new Label();
                        labvaluetext.Text = "value:";
                        this.Controls.Add(labvaluetext);
                        labvaluetext.Width = labmembertext.Width;
                        labvaluetext.Left = labmembertext.Left;
                        labvaluetext.Top = labmembertext.Top + labmembertext.Height + 19;

                        TextBox tbvalbox = new TextBox();
                        this.Controls.Add(tbvalbox);
                        tbvalbox.Width = tbmebmerbox.Width;
                        tbvalbox.Left = tbmebmerbox.Left;
                        tbvalbox.Top = tbmebmerbox.Top + tbmebmerbox.Height + 15;

                        CheckBox cb = new CheckBox();
                        cb.Text = "数字";
                        cb.Checked = false;
                        this.Controls.Add(cb);
                        cb.Left = tbvalbox.Left;
                        cb.Top = tbvalbox.Top + tbvalbox.Height + 15;

                        IsValid = () =>
                            {
                                if (string.IsNullOrWhiteSpace(tbmebmerbox.Text))
                                {
                                    MessageBox.Show("member不能为空");
                                    return false;
                                }

                                if (cb.Checked)
                                {
                                    RedisValue rv;
                                    if (RedisUtil.TryParseNumber(tbvalbox.Text, out rv))
                                    {
                                        this.Val = rv;
                                    }
                                    else
                                    {
                                        MessageBox.Show("格式错误");
                                        return false;
                                    }
                                }
                                else
                                {
                                    this.Val = tbvalbox.Text;
                                }

                                OnAdd = (connstr) =>
                                {
                                    RedisUtil.Execute(connstr, db =>
                                    {
                                        db.HashSet(this.Key, this.SubKey, this.Val);
                                        MessageBox.Show("添加成功");
                                    }, ex =>
                                    {
                                        MessageBox.Show(ex.Message);
                                    });
                                };

                                return true;
                            };

                        break;
                    }
                case RedisType.List:
                    {
                        Label labitemtext = new Label();
                        labitemtext.Text = "item:";
                        this.Controls.Add(labitemtext);
                        labitemtext.Width = LabKey.Width;
                        labitemtext.Left = LabKey.Left;
                        labitemtext.Top = LabKey.Top + LabKey.Height + 19;

                        TextBox tbitembox = new TextBox();
                        this.Controls.Add(tbitembox);
                        tbitembox.Width = TBKey.Width;
                        tbitembox.Left = TBKey.Left;
                        tbitembox.Top = TBKey.Top + TBKey.Height + 15;

                        CheckBox cb = new CheckBox();
                        cb.Text = "数字";
                        cb.Checked = false;
                        this.Controls.Add(cb);
                        cb.Left = tbitembox.Left;
                        cb.Top = tbitembox.Top + tbitembox.Height + 15;

                        IsValid = () =>
                        {
                            if (string.IsNullOrWhiteSpace(tbitembox.Text))
                            {
                                MessageBox.Show("item不能为空");
                                return false;
                            }

                            if (cb.Checked)
                            {
                                RedisValue rv;
                                if (RedisUtil.TryParseNumber(tbitembox.Text, out rv))
                                {
                                    this.Val = rv;
                                }
                                else
                                {
                                    MessageBox.Show("格式错误");
                                    return false;
                                }
                            }
                            else
                            {
                                this.Val = tbitembox.Text;
                            }

                            OnAdd = (connstr) =>
                                {
                                    RedisUtil.Execute(connstr, db =>
                                        {
                                            db.ListLeftPush(this.Key, this.Val);
                                            MessageBox.Show("添加成功");
                                        }, ex =>
                                        {
                                            MessageBox.Show(ex.Message);
                                        });
                                };

                            return true;
                        };

                        break;
                    }
                case RedisType.Set:
                    {
                        Label labitemtext = new Label();
                        labitemtext.Text = "item:";
                        this.Controls.Add(labitemtext);
                        labitemtext.Width = LabKey.Width;
                        labitemtext.Left = LabKey.Left;
                        labitemtext.Top = LabKey.Top + LabKey.Height + 19;

                        TextBox tbitembox = new TextBox();
                        this.Controls.Add(tbitembox);
                        tbitembox.Width = TBKey.Width;
                        tbitembox.Left = TBKey.Left;
                        tbitembox.Top = TBKey.Top + TBKey.Height + 15;

                        CheckBox cb = new CheckBox();
                        cb.Text = "数字";
                        cb.Checked = false;
                        this.Controls.Add(cb);
                        cb.Left = tbitembox.Left;
                        cb.Top = tbitembox.Top + tbitembox.Height + 15;

                        IsValid = () =>
                        {
                            if (string.IsNullOrWhiteSpace(tbitembox.Text))
                            {
                                MessageBox.Show("item不能为空");
                                return false;
                            }

                            if (cb.Checked)
                            {
                                RedisValue rv;
                                if (RedisUtil.TryParseNumber(tbitembox.Text, out rv))
                                {
                                    this.Val = rv;
                                }
                                else
                                {
                                    MessageBox.Show("格式错误");
                                    return false;
                                }
                            }
                            else
                            {
                                this.Val = tbitembox.Text;
                            }

                            OnAdd = (connstr) =>
                            {
                                RedisUtil.Execute(connstr, db =>
                                {
                                    db.SetAdd(this.Key, this.Val);
                                    MessageBox.Show("添加成功");
                                }, ex =>
                                {
                                    MessageBox.Show(ex.Message);
                                });
                            };

                            return true;
                        };

                        break;
                    }
                case RedisType.SortedSet:
                    {
                        Label labmembertext = new Label();
                        labmembertext.Text = "member:";
                        this.Controls.Add(labmembertext);
                        labmembertext.Width = LabKey.Width;
                        labmembertext.Left = LabKey.Left;
                        labmembertext.Top = LabKey.Top + LabKey.Height + 19;

                        TextBox tbmebmerbox = new TextBox();
                        this.Controls.Add(tbmebmerbox);
                        tbmebmerbox.Width = TBKey.Width;
                        tbmebmerbox.Left = TBKey.Left;
                        tbmebmerbox.Top = TBKey.Top + TBKey.Height + 15;

                        Label labvaluetext = new Label();
                        labvaluetext.Text = "score:";
                        this.Controls.Add(labvaluetext);
                        labvaluetext.Width = labmembertext.Width;
                        labvaluetext.Left = labmembertext.Left;
                        labvaluetext.Top = labmembertext.Top + labmembertext.Height + 19;

                        TextBox tbvalbox = new TextBox();
                        this.Controls.Add(tbvalbox);
                        tbvalbox.Width = tbmebmerbox.Width;
                        tbvalbox.Left = tbmebmerbox.Left;
                        tbvalbox.Top = tbmebmerbox.Top + tbmebmerbox.Height + 15;

                        IsValid = () =>
                        {
                            if (string.IsNullOrWhiteSpace(tbmebmerbox.Text))
                            {
                                MessageBox.Show("member不能为空");
                                return false;
                            }

                            RedisValue subkey;
                            if (!RedisUtil.TryParseNumber(tbmebmerbox.Text, out subkey))
                            {
                                subkey = tbmebmerbox.Text;
                            }

                            RedisValue rv;
                            if (RedisUtil.TryParseNumber(tbvalbox.Text, out rv))
                            {
                                this.Val = rv;
                            }
                            else
                            {
                                MessageBox.Show("格式错误");
                                return false;
                            }

                            OnAdd = (connstr) =>
                            {
                                RedisUtil.Execute(connstr, db =>
                                {
                                    db.SortedSetAdd(this.Key, subkey, (double)this.Val);
                                    MessageBox.Show("添加成功");
                                }, ex =>
                                {
                                    MessageBox.Show(ex.Message);
                                });
                            };

                            return true;
                        };

                        break;
                    }
                default:
                    {
                        //this.DialogResult = DialogResult.Cancel;
                        this.TBKey.Enabled = true;
                        this.TBKey.ReadOnly = false;
                        Label labitemtext = new Label();
                        labitemtext.Text = "item:";
                        this.Controls.Add(labitemtext);
                        labitemtext.Width = LabKey.Width;
                        labitemtext.Left = LabKey.Left;
                        labitemtext.Top = LabKey.Top + LabKey.Height + 19;

                        TextBox tbitembox = new TextBox();
                        this.Controls.Add(tbitembox);
                        tbitembox.Width = TBKey.Width;
                        tbitembox.Left = TBKey.Left;
                        tbitembox.Top = TBKey.Top + TBKey.Height + 15;

                        ComboBox cbtype = new ComboBox();
                        cbtype.Items.AddRange(new[] { "string", "set" });
                        cbtype.SelectedText = "string";
                        this.Controls.Add(cbtype);
                        cbtype.Left = tbitembox.Left;
                        cbtype.Top = tbitembox.Top + tbitembox.Height + 15;

                        CheckBox cb = new CheckBox();
                        cb.Text = "数字";
                        cb.Checked = false;
                        this.Controls.Add(cb);
                        cb.Left = tbitembox.Left;
                        cb.Top = cbtype.Top + cbtype.Height + 15;

                        IsValid = () =>
                        {
                            if (string.IsNullOrWhiteSpace(TBKey.Text))
                            {
                                MessageBox.Show("key不能为空");
                                return false;
                            }

                            if (string.IsNullOrWhiteSpace(tbitembox.Text))
                            {
                                MessageBox.Show("item不能为空");
                                return false;
                            }

                            if (cbtype.SelectedText == "set")
                            {
                                if (cb.Checked)
                                {
                                    RedisValue rv;
                                    if (RedisUtil.TryParseNumber(tbitembox.Text, out rv))
                                    {
                                        this.Val = rv;
                                    }
                                    else
                                    {
                                        MessageBox.Show("格式错误");
                                        return false;
                                    }
                                }
                                else
                                {
                                    this.Val = tbitembox.Text;
                                }

                                OnAdd = (connstr) =>
                                {
                                    RedisUtil.Execute(connstr, db =>
                                    {
                                        db.SetAdd(this.TBKey.Text, this.Val);
                                        MessageBox.Show("添加成功");
                                    }, ex =>
                                    {
                                        MessageBox.Show(ex.Message);
                                    });
                                };
                            }
                            else
                            {
                                OnAdd = (connstr) =>
                                {
                                    RedisUtil.Execute(connstr, db =>
                                    {
                                        db.StringSet(this.TBKey.Text, this.Val.ToString());
                                        MessageBox.Show("添加成功");
                                    }, ex =>
                                    {
                                        MessageBox.Show(ex.Message);
                                    });
                                };
                            }

                            return true;
                        };

                        break;
                    }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (this.RedisServer == null)
            {
                return;
            }

            if (IsValid == null)
            {
                return;
            }

            if (!IsValid())
            {
                return;
            }

            if (MessageBox.Show("确定插入吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            this.OnAdd(this.RedisServer.ConnStr);

            this.DialogResult = DialogResult.Yes;
        }
    }
}
