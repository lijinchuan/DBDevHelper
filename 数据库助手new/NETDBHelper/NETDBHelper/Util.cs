﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NETDBHelper.SubForm;
using System.Runtime.CompilerServices;
using Entity;
using System.Reflection;

namespace NETDBHelper
{
    public static class Util
    {
        private static Dictionary<int, PopMessageDlg> PopDlgDic = new Dictionary<int, PopMessageDlg>();

        private static string MsgType = "";

        private static LoginUser CurrentLoginUser = null;

        public static event Action<LoginUser> OnUserLogin = null;

        public static event Action<LoginUser> OnUserLoginOut = null;

        public static void SendMsg(Control ctl,string msg)
        {
            var parent = ctl;
            while (parent != null)
            {
                if(parent is MainFrm)
                {
                    ((MainFrm)parent).SetMsg(msg);
                    break;
                }

                parent = parent.Parent;
            }
        }

        public static void SendMsg(Control ctl,string msgtype, string msg)
        {
            var parent = ctl;
            while (parent != null)
            {
                if (parent is MainFrm)
                {
                    MsgType = msgtype;
                    ((MainFrm)parent).SetMsg(msg);
                    break;
                }

                parent = parent.Parent;
            }
        }

        public static void ClearMsg(Control ctl, string msgtype)
        {
            if (msgtype == MsgType || string.IsNullOrWhiteSpace(MsgType))
            {
                var parent = ctl;
                while (parent != null)
                {
                    if (parent is MainFrm)
                    {
                        ((MainFrm)parent).SetMsg("");
                        break;
                    }

                    parent = parent.Parent;
                }
            }
        }


        public static void ClosePopMsg(int msgid)
        {
            PopMessageDlg dlg = null;
            if (PopDlgDic.TryGetValue(msgid, out dlg))
            {
                dlg.Close();
            }
        }

        public static T FindParent<T>(Control ctl) where T : Control
        {
            if (ctl == null)
            {
                return null;
            }

            while (ctl.Parent != null)
            {
                if (ctl.Parent != null && ctl.Parent is T)
                {
                    return (T)ctl.Parent;
                }
                ctl = ctl.Parent;
            }

            return null;
        }

        public static void PopMsg(int msgid,string title,string content)
        {
            PopMessageDlg dlg = null;
            var cnt = 0;
            lock (PopDlgDic)
            {
                if (!PopDlgDic.TryGetValue(msgid, out dlg))
                {
                    dlg = new PopMessageDlg();
                    dlg.Text = title;
                    PopDlgDic.Add(msgid, dlg);
                    dlg.FormClosed += (s,e)=>
                    {
                        lock (PopDlgDic)
                        {
                            PopDlgDic.Remove(msgid);
                        }
                    };
                }

                foreach(var item in PopDlgDic)
                {
                    cnt++;
                    if (item.Value.Equals(dlg))
                    {
                        break;
                    }
                }
            }

            if (dlg.GetMsg() != content || dlg.Text != title)
            {
                dlg.SetMsg(title, content);
                dlg.PopShow(cnt);
            }
        }

        public static void UserLogin(string userName,int level)
        {
            CurrentLoginUser = new LoginUser
            {
                UserLevel=level,
                UserName=userName
            };

            if (OnUserLogin != null)
            {
                OnUserLogin(CurrentLoginUser);
            }
        }

        public static int LoginUserLevel()
        {
            if (CurrentLoginUser == null)
            {
                return 0;
            }

            return CurrentLoginUser.UserLevel;
        }

        public static void LoginOut()
        {
            var oldLoinUser = CurrentLoginUser;
            CurrentLoginUser = null;

            if (OnUserLoginOut != null)
            {
                OnUserLoginOut(oldLoinUser);
            }
        }

        public static bool IsNoteTable(string tableName)
        {
            return tableName?.StartsWith("#note_", StringComparison.OrdinalIgnoreCase) == true;

        }

        public static bool IsTempTable(string tableName)
        {
            return tableName?.StartsWith("$", StringComparison.OrdinalIgnoreCase) == true;
        }

        public static string NameNoteTalbe()
        {
            return ("#note_" + Guid.NewGuid().ToString("N")).ToUpper();
        }

        public static string NameTempTable()
        {
            return "$" + Guid.NewGuid().ToString("N");
        }

        //使用扩展方法
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
