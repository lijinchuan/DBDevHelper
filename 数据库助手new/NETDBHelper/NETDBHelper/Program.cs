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
        static void Main()
        {
            BigEntityTableEngine.LocalEngine.CreateTable("MarkColumnInfo", "ID",true, typeof(MarkColumnInfo), new IndexInfo[]
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
    }
}
