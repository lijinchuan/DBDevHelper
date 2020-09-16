using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using LJC.FrameWorkV3.EntityBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NETDBHelper
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
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

            BigEntityTableEngine.LocalEngine.CreateTable("SPContent", "ID", true, typeof(SPContent), new IndexInfo[]{
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


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
            //Application.Run(new SubForm.TestForm());
        }
    }
}
