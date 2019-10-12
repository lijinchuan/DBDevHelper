using RedisHelper.Model;
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
    public partial class AddNewForm : Form
    {
        public int? Defaultdb
        {
            get;
            set;
        }

        public string NewKey
        {
            get;
            private set;
        }

        public RedisServerEntity RediServer
        {
            get;
            set;
        }

        public AddNewForm()
        {
            InitializeComponent();

            this.CBType.SelectedIndexChanged += CBType_SelectedIndexChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CBType.SelectedIndex = 0;
        }

        void CBType_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*String
Set
Hash
List
SortedSet*/

            CBSecKey.Visible = LBSecKey.Visible = TBSecKey.Visible = false;
            CBThridKey.Visible = LBThridKey.Visible = TBThridKey.Visible = false;

            switch ((string)CBType.SelectedItem)
            {
                case "String":
                    {
                        CBSecKey.Visible = LBSecKey.Visible = TBSecKey.Visible = true;
                        LBSecKey.Text = "Value";
                        CBSecKey.Visible = false;
                        LBThridKey.Visible = TBThridKey.Visible = CBThridKey.Visible = false;
                        break;
                    }
                case "Set":
                    {
                        CBSecKey.Visible = LBSecKey.Visible = TBSecKey.Visible = true;
                        LBSecKey.Text = "Value";
                        CBSecKey.Visible = true;
                        break;
                    }
                case "List":
                    {
                        CBSecKey.Visible = LBSecKey.Visible = TBSecKey.Visible = true;
                        LBSecKey.Text = "Value";
                        CBSecKey.Visible = true;
                        break;
                    }
                case "Hash":
                    {
                        CBSecKey.Visible = LBSecKey.Visible = TBSecKey.Visible = true;
                        CBThridKey.Visible = LBThridKey.Visible = TBThridKey.Visible = true;
                        LBSecKey.Text = "member";
                        CBSecKey.Visible = false;
                        LBThridKey.Text = "value";
                        CBThridKey.Visible = true;
                        break;
                    }
                case "SortedSet":
                    {
                        LBSecKey.Visible = TBSecKey.Visible = true;
                        LBThridKey.Visible = TBThridKey.Visible = true;
                        LBSecKey.Text = "item";
                        LBThridKey.Text = "score";
                        break;
                    }
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var firstkey = TB_FirstKey.Text;
            if (string.IsNullOrWhiteSpace(firstkey))
            {
                MessageBox.Show("key不能为空");
                return;
            }

            bool issucc = false;

            if (RediServer != null)
            {
                switch ((string)CBType.SelectedItem)
                {
                    case "String":
                        {
                            RedisUtil.Execute(this.RediServer.ConnStr,this.Defaultdb, (db) =>
                            {
                                if (db.StringSet(firstkey, TBSecKey.Text))
                                {
                                    issucc = true;
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                            break;
                        }
                    case "Set":
                        {
                            RedisUtil.Execute(this.RediServer.ConnStr,this.Defaultdb, (db) =>
                            {
                                RedisValue val = TBSecKey.Text;
                               
                                if (CBSecKey.Checked)
                                {
                                    RedisValue rv;
                                    if (RedisUtil.TryParseNumber(TBSecKey.Text, out rv))
                                    {
                                        val = rv;
                                    }
                                    else
                                    {
                                        MessageBox.Show(TBSecKey.Text + "不是数字");
                                        return;
                                    }
                                }
                                if (db.SetAdd(firstkey, val))
                                {
                                    issucc = true;
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                            break;
                        }
                    case "List":
                        {
                            RedisUtil.Execute(this.RediServer.ConnStr, this.Defaultdb,(db) =>
                            {
                                RedisValue val = TBSecKey.Text;

                                if (CBSecKey.Checked)
                                {
                                    RedisValue rv;
                                    if (RedisUtil.TryParseNumber(TBSecKey.Text, out rv))
                                    {
                                        val = rv;
                                    }
                                    else
                                    {
                                        MessageBox.Show(TBSecKey.Text + "不是数字");
                                        return;
                                    }
                                }
                                if (db.ListRightPush(firstkey, val) > 0)
                                {
                                    issucc = true;
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                            break;
                        }
                    case "Hash":
                        {
                            RedisUtil.Execute(this.RediServer.ConnStr,this.Defaultdb, (db) =>
                            {
                                RedisValue seckey = TBSecKey.Text;
                                RedisValue val = TBThridKey.Text;
                                if (CBThridKey.Checked)
                                {
                                    RedisValue rv;
                                    if (RedisUtil.TryParseNumber(TBThridKey.Text, out rv))
                                    {
                                        seckey = rv;
                                    }
                                    else
                                    {
                                        MessageBox.Show(TBThridKey.Text + "不是数字");
                                        return;
                                    }
                                }
                                if (db.HashSet(firstkey,seckey, val))
                                {
                                    issucc = true;
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                            break;
                        }
                    case "SortedSet":
                        {
                            RedisUtil.Execute(this.RediServer.ConnStr,this.Defaultdb, (db) =>
                            {
                                RedisValue seckey = TBSecKey.Text;
                                double val = 0;
                                if (!double.TryParse(TBThridKey.Text, out val))
                                {
                                    MessageBox.Show(TBThridKey.Text + "不是数字");
                                    return;
                                }
                                if (db.SortedSetAdd(firstkey, seckey, val))
                                {
                                    issucc = true;
                                    MessageBox.Show("success");
                                }
                                else
                                {
                                    MessageBox.Show("fail");
                                }
                            }, (ex) =>
                            {
                                MessageBox.Show(ex.Message);
                            });
                            break;
                        }
                }

                if (issucc)
                {
                    this.NewKey = TB_FirstKey.Text;
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
