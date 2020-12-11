using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using LJC.FrameWorkV3.EntityBuf;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace APIHelper
{
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
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

            BigEntityTableEngine.LocalEngine.CreateTable<APISource>(p => p.Id, null);

            BigEntityTableEngine.LocalEngine.CreateTable<APIUrl>(p => p.Id, b => b.AddIndex("SourceId", p => p.Asc(q => q.SourceId))
            .AddIndex("SourceId_APIName", p => p.Asc(q => q.SourceId).Asc(q => q.APIName)));

            BigEntityTableEngine.LocalEngine.CreateTable<APIEnv>(p => p.Id, b => b.AddIndex("SourceId", p => p.Asc(q => q.SourceId)));

            //BigEntityTableEngine.LocalEngine.CreateTable("MarkColumnInfo", "ID",true, typeof(MarkColumnInfo), new IndexInfo[]
            //{
            //    new IndexInfo
            //    {
            //        IndexName="keys",
            //        Indexs=new IndexItem[]
            //        {
            //            new IndexItem
            //            {
            //                Field="DBName",
            //                FieldType=EntityType.STRING,
            //            },
            //            new IndexItem
            //            {
            //                Field="TBName",
            //                FieldType=EntityType.STRING,
            //            },
            //            new IndexItem
            //            {
            //                Field="ColumnName",
            //                FieldType=EntityType.STRING,
            //            }
            //        }
            //    }
            //});

            BigEntityTableEngine.LocalEngine.CreateTable("MarkObjectInfo", "ID", true, typeof(MarkObjectInfo), new IndexInfo[]
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


            BigEntityTableEngine.LocalEngine.CreateTable<LogicMap>(p => p.ID, p => p.AddIndex("DB_LogicName", m => m.Asc(f => f.DBName).Asc(f => f.LogicName)));
            BigEntityTableEngine.LocalEngine.CreateTable<LogicMapTable>(p => p.ID, p => p.AddIndex("LogicID", m => m.Asc(f => f.LogicID)));
            BigEntityTableEngine.LocalEngine.CreateTable<LogicMapRelColumn>(p => p.ID, p => p.AddIndex("LogicID", m => m.Asc(f => f.LogicID))
            .AddIndex("LSDTC", m => m.Asc(f => f.LogicID).Asc(f => f.DBName).Asc(f => f.TBName))
            .AddIndex("LSDRTC", m => m.Asc(f => f.LogicID).Asc(f => f.RelDBName).Asc(f => f.RelTBName)));


            BigEntityTableEngine.LocalEngine.CreateTable<APIData>(p => p.Id, p => p.AddIndex("ApiId", q => q.Asc(m => m.ApiId)));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());

            BigEntityTableEngine.LocalEngine.ShutDown();
        }
    }
}
