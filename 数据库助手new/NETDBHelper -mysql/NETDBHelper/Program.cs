using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using LJC.FrameWorkV3.EntityBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NETDBHelper
{
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Process[] pa = Process.GetProcesses();//获取当前进程数组。
            var currprocess = Process.GetCurrentProcess();
            foreach (Process p in pa)
            {
                if (p.ProcessName == currprocess.ProcessName && p.Id != currprocess.Id && p.MainModule.FileName == currprocess.MainModule.FileName)
                {
                    //MessageBox.Show("另一个进程正在运行，无法启动。");
                    SwitchToThisWindow(p.MainWindowHandle, true);
                    return;
                }
            }

            BigEntityTableEngine.LocalEngine.CreateTable("MarkColumnInfo", "ID", true, typeof(MarkColumnInfo), new IndexInfo[]
            {
                new IndexInfo
                {
                    IndexName="keys",
                    Indexs=new IndexItem[]
                    {
                        new IndexItem
                        {
                            Field="DBName",
                            FieldType=EntityType.STRING,
                        },
                        new IndexItem
                        {
                            Field="TBName",
                            FieldType=EntityType.STRING,
                        },
                        new IndexItem
                        {
                            Field="ColumnName",
                            FieldType=EntityType.STRING,
                        }
                    }
                }
            });

            BigEntityTableEngine.LocalEngine.CreateTable("ColumnMarkSyncRecord", "ID", true, typeof(ColumnMarkSyncRecord), new IndexInfo[]
            {
                new IndexInfo
                {
                    IndexName="keys",
                    Indexs=new IndexItem[]
                    {
                        new IndexItem
                        {
                            Field="DBName",
                            FieldType=EntityType.STRING,
                        },
                        new IndexItem
                        {
                            Field="TBName",
                            FieldType=EntityType.STRING,
                        }
                    }
                }
            });

            BigEntityTableEngine.LocalEngine.CreateTable("HLog", "ID", true, typeof(HLogEntity), new IndexInfo[]{
                 new IndexInfo
                 {
                     IndexName="LogTime",
                     Indexs=new IndexItem[]
                     {
                         new IndexItem
                         {
                             Field="LogTime",
                             FieldType=EntityType.DATETIME,
                         }
                     }
                 }
                });


            BigEntityTableEngine.LocalEngine.CreateTable("SqlSave", "ID", true, typeof(SqlSaveEntity));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
            //Application.Run(new SubForm.TestForm());
        }
    }
}
