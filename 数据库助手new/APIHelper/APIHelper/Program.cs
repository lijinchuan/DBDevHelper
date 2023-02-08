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
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("程序启动，检查进程");

            Process[] pa = Process.GetProcesses();//获取当前进程数组。
            var currprocess = Process.GetCurrentProcess();
            foreach (Process p in pa)
            {
                if (p.ProcessName == currprocess.ProcessName && p.Id != currprocess.Id && p.MainModule.FileName == currprocess.MainModule.FileName)
                {
                    //MessageBox.Show("另一个进程正在运行，无法启动。");
                    LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("检查到已启动进程，切换到进程");
                    SwitchToThisWindow(p.MainWindowHandle, true);
                    return;
                }
            }

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("检查进程完毕");

            BigEntityTableEngine.LocalEngine.CreateTable<APISource>(p => p.Id, null);

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APISource");

            BigEntityTableEngine.LocalEngine.CreateTable<APIUrl>(p => p.Id, b => b.AddIndex("SourceId", p => p.Asc(q => q.SourceId))
            .AddIndex("SourceId_APIName", p => p.Asc(q => q.SourceId).Asc(q => q.APIName)));

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIUrl");

            BigEntityTableEngine.LocalEngine.CreateTable<APIEnv>(p => p.Id, b => b.AddIndex("SourceId", p => p.Asc(q => q.SourceId)));

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIEnv");

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

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 MarkObjectInfo");


            BigEntityTableEngine.LocalEngine.CreateTable<LogicMap>(p => p.ID, p => p.AddIndex("APISourceId_LogicName", m => m.Asc(f => f.APISourceId).Asc(f => f.LogicName)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 LogicMap");
            BigEntityTableEngine.LocalEngine.CreateTable<LogicMapTable>(p => p.ID, p => p.AddIndex("LogicID", m => m.Asc(f => f.LogicID)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 LogicMapTable");
            BigEntityTableEngine.LocalEngine.CreateTable<LogicMapRelColumn>(p => p.ID, p => p.AddIndex("LogicID", m => m.Asc(f => f.LogicID))
            .AddIndex("LSDTC", m => m.Asc(f => f.LogicID).Asc(f => f.APISourceId).Asc(f => f.APIId))
            .AddIndex("LSDRTC", m => m.Asc(f => f.LogicID).Asc(f => f.RelAPIResourceId).Asc(f => f.RelAPIId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 LogicMapRelColumn");

            BigEntityTableEngine.LocalEngine.Upgrade<Entity.OldVesion.APIData, APIData>(nameof(APIData), old =>
            {
                if (old == null)
                {
                    return null;
                }
                return new APIData
                {
                    ApiId = old.ApiId,
                    ApiKeyAddTo = old.ApiKeyAddTo,
                    ApiKeyName = old.ApiKeyName,
                    ApiKeyValue = old.ApiKeyValue,
                    BearToken = old.BearToken,
                    Cookies = old.Cookies,
                    FormDatas = old.FormDatas,
                    Headers = old.Headers,
                    Multipart_form_data = new List<ParamInfo>(),
                    Params = old.Params,
                    RawText = old.RawText,
                    XWWWFormUrlEncoded = old.XWWWFormUrlEncoded
                };

            }, "Id", true, new IndexBuilder<APIData>().AddIndex("ApiId", q => q.Asc(m => m.ApiId)).Build());
            //BigEntityTableEngine.LocalEngine.CreateTable<APIData>(p => p.Id, p => p.AddIndex("ApiId", q => q.Asc(m => m.ApiId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIData");

            BigEntityTableEngine.LocalEngine.CreateTable<APIEnv>(p => p.Id, p => p.AddIndex("SourceId", q => q.Asc(m => m.SourceId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIEnv");

            BigEntityTableEngine.LocalEngine.CreateTable<APIEnvParam>(p => p.Id, p => p.AddIndex("APISourceId", q => q.Asc(m => m.APISourceId))
            .AddIndex("APISourceId_EnvId", q=>q.Asc(m=>m.APISourceId).Asc(m=>m.EnvId))
            .AddIndex("APISourceId_Name", q => q.Asc(m => m.APISourceId).Asc(m => m.Name)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIEnvParam");

           //日志
           // BigEntityTableEngine.LocalEngine.Upgrade<Entity.OldVesion.V2.APIInvokeLog, Entity.APIInvokeLog>(nameof(APIInvokeLog), old =>
           //   {
           //       return new APIInvokeLog
           //       {
           //           APIData=new APIData
           //           {
           //               ApiId=old.APIData.ApiId,
           //               ApiKeyAddTo=old.APIData.ApiKeyAddTo,
           //               ApiKeyName=old.APIData.ApiKeyName,
           //               ApiKeyValue=old.APIData.ApiKeyValue,
           //               BasicUserName=string.Empty,
           //               BasicUserPwd=string.Empty,
           //               BearToken=old.APIData.BearToken,
           //               Cookies=old.APIData.Cookies,
           //               FormDatas=old.APIData.FormDatas,
           //               Headers=old.APIData.Headers,
           //               Id=old.APIData.Id,
           //               Multipart_form_data=old.APIData.Multipart_form_data,
           //               Params=old.APIData.Params,
           //               RawText=old.APIData.RawText,
           //               XWWWFormUrlEncoded=old.APIData.XWWWFormUrlEncoded
           //           },
           //           OrgAPIData = new APIData
           //           {
           //               ApiId = old.APIData.ApiId,
           //               ApiKeyAddTo = old.APIData.ApiKeyAddTo,
           //               ApiKeyName = old.APIData.ApiKeyName,
           //               ApiKeyValue = old.APIData.ApiKeyValue,
           //               BasicUserName = string.Empty,
           //               BasicUserPwd = string.Empty,
           //               BearToken = old.APIData.BearToken,
           //               Cookies = old.APIData.Cookies,
           //               FormDatas = old.APIData.FormDatas,
           //               Headers = old.APIData.Headers,
           //               Id = old.APIData.Id,
           //               Multipart_form_data = old.APIData.Multipart_form_data,
           //               Params = old.APIData.Params,
           //               RawText = old.APIData.RawText,
           //               XWWWFormUrlEncoded = old.APIData.XWWWFormUrlEncoded
           //           },
           //           ApiEnvId =old.ApiEnvId,
           //           AuthType=old.AuthType,
           //           APIId=old.APIId,
           //           APIMethod=old.APIMethod,
           //           APIName=old.APIName,
           //           APIResonseResult=old.APIResonseResult,
           //           ApplicationType=old.ApplicationType,
           //           BodyDataType=old.BodyDataType,
           //           CDate=old.CDate,
           //           Ms=old.Ms,
           //           Path=old.Path,
           //           OrgPath=old.Path,
           //           RespMsg=old.RespMsg,
           //           ResponseText=old.ResponseText,
           //           RespSize=old.RespSize,
           //           SourceId=old.SourceId,
           //           StatusCode=old.StatusCode
           //       };
           //   }, "Id", true, new IndexBuilder<APIInvokeLog>().AddIndex("APIId_CDate", m => m.Asc(s => s.APIId).Desc(s => s.CDate))
           //.AddIndex("APIId_ApiEnvId_CDate", m => m.Asc(s => s.APIId).Asc(s => s.ApiEnvId).Desc(s => s.CDate))
           //.AddIndex("CDate", m => m.Desc(s => s.CDate)).Build());

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIInvokeLog");

            BigEntityTableEngine.LocalEngine.CreateTable<APIInvokeLog>(p => p.Id, p => p.AddIndex("APIId_CDate", m => m.Asc(s => s.APIId).Desc(s => s.CDate))
            .AddIndex("APIId_ApiEnvId_CDate", m => m.Asc(s => s.APIId).Asc(s => s.ApiEnvId).Desc(s => s.CDate))
            .AddIndex("CDate", m => m.Desc(s => s.CDate)));

            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("EnsureIndex:APIInvokeLog");

            //参数
            BigEntityTableEngine.LocalEngine.CreateTable<APIParam>(p => p.Id, p => p.AddIndex("APIId", m => m.Asc(s => s.APIId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIParam");
            //文档
            BigEntityTableEngine.LocalEngine.CreateTable<APIDoc>(p => p.Id, p => p.AddIndex("APISourceId", m => m.Asc(s => s.APISourceId)).AddIndex("APIId", m => m.Asc(s => s.APIId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIDoc");
            //文档示例
            BigEntityTableEngine.LocalEngine.CreateTable<APIDocExample>(p => p.Id, p => p.AddIndex("ApiId", m => m.Asc(s => s.ApiId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 APIDocExample");

            BigEntityTableEngine.LocalEngine.CreateTable<ProxyServer>(p => p.Id, null);
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 ProxyServer");
            BigEntityTableEngine.LocalEngine.CreateTable<ApiUrlSetting>(p => p.Id, b => b.AddIndex("ApiId", c => c.Asc(d => d.ApiId)));
            LJC.FrameWorkV3.LogManager.LogHelper.Instance.Debug("创建表 ApiUrlSetting");

            //模拟服务器
            BigEntityTableEngine.LocalEngine.CreateTable<SimulateServerConfig>(p => p.Id, null);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
            
        }
    }
}
