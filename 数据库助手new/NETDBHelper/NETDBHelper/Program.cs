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

            BigEntityTableEngine.LocalEngine.CreateTable("SPInfo", "ID", true, typeof(SPInfo), new IndexInfo[]
            {
                new IndexInfo
                {
                    IndexName="DBName_SPName",
                    Indexs=new IndexItem[]
                    {
                        new IndexItem
                        {
                            Field="DBName",
                            FieldType=EntityType.STRING,
                        },
                        new IndexItem
                        {
                            Field="SPName",
                            FieldType=EntityType.STRING,
                        }
                    }
                },
                new IndexInfo
                {
                    IndexName="SPName",
                    Indexs=new IndexItem[]
                    {
                        new IndexItem
                        {
                            Field="SPName",
                            FieldType=EntityType.STRING,
                        }
                    }
                }
            });

            BigEntityTableEngine.LocalEngine.CreateTable("SPContent", "ID", true, typeof(SPContent),new IndexInfo[]{
                  new IndexInfo
                  {
                      IndexName="SPName",
                      Indexs=new IndexItem[]
                      {
                          new IndexItem
                          {
                              Field="SPName",
                              FieldType=EntityType.STRING
                          }
                      }
                  }
                });


            BigEntityTableEngine.LocalEngine.CreateTable("FunContent", "ID", true, typeof(FunContent), new IndexInfo[]{
                  new IndexInfo
                  {
                      IndexName="FunName",
                      Indexs=new IndexItem[]
                      {
                          new IndexItem
                          {
                              Field="FunName",
                              FieldType=EntityType.STRING
                          }
                      }
                  }
                });

            //TBSearchColumn
            BigEntityTableEngine.LocalEngine.CreateTable("TBSearchColumn", "ID", true, typeof(TBSearchColumn), new IndexInfo[]{
                  new IndexInfo
                  {
                      IndexName="DBName_TBName",
                      Indexs=new IndexItem[]
                      {
                          new IndexItem
                          {
                              Field="DBName",
                              FieldType=EntityType.STRING
                          },
                          new IndexItem
                          {
                              Field="TBName",
                              FieldType=EntityType.STRING
                          }
                      }
                  }
                });

            BigEntityTableEngine.LocalEngine.CreateTable<RelTable>(p => p.Id, b =>
            {
                b.AddIndex("SDT", p => p.Asc(q => q.DBName).Asc(q => q.TBName));
                b.AddIndex("SDRT", p => p.Asc(q => q.RelDBName).Asc(q => q.RelTBName));
            });

            BigEntityTableEngine.LocalEngine.CreateTable<RelColumn>(p => p.Id, b =>
            {
                b.AddIndex("SDTC", p => p.Asc(q => q.DBName).Asc(q => q.TBName));
                b.AddIndex("SDRTC", p => p.Asc(q => q.RelDBName).Asc(q => q.RelTBName));
            });

            BigEntityTableEngine.LocalEngine.CreateTable<LogicMap>(p => p.ID, p => p.AddIndex("DB_LogicName", m => m.Asc(f => f.DBName).Asc(f => f.LogicName)));
            BigEntityTableEngine.LocalEngine.CreateTable<LogicMapTable>(p => p.ID, p => p.AddIndex("LogicID", m => m.Asc(f => f.LogicID)));
            BigEntityTableEngine.LocalEngine.CreateTable<LogicMapRelColumn>(p => p.ID, p => p.AddIndex("LogicID", m => m.Asc(f => f.LogicID))
            .AddIndex("LSDTC", m => m.Asc(f => f.LogicID).Asc(f => f.DBName).Asc(f => f.TBName))
            .AddIndex("LSDRTC", m => m.Asc(f => f.LogicID).Asc(f => f.RelDBName).Asc(f => f.RelTBName)));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LJC.FrameWork.SOA.ESBClientPoolManager.MAXCLIENTCOUNT = 1;
            Application.Run(new MainFrm());

            BigEntityTableEngine.LocalEngine.ShutDown();
            try
            {
                LJC.FrameWork.SOA.ESBClient.Close();
            }
            catch
            {

            }
        }
    }
}
