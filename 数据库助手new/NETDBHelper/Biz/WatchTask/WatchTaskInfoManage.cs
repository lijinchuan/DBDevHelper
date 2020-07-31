using Entity.WatchTask;
using LJC.FrameWorkV3.Data.EntityDataBase;
using LJC.FrameWorkV3.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biz.WatchTask
{
    public class WatchTaskInfoManage
    {
        internal static bool StopTaskLoop = false;

        public static Action<WatchTaskInfo, object> OnTiggerError = null;

        static WatchTaskInfoManage()
        {
            BigEntityTableEngine.LocalEngine.CreateTable<WatchTaskInfo>(p => p.ID, p => p.AddIndex("Name", q => q.Asc(m => m.Name)));
            BigEntityTableEngine.LocalEngine.CreateTable<WatchTaskLog>(p => p.ID, p => p.AddIndex("TaskId-CDate", q => q.Asc(m => m.TaskId).Asc(m => m.CDate)));
        }

        public WatchTaskInfo Get(int taskid)
        {
            return BigEntityTableEngine.LocalEngine.Find<WatchTaskInfo>(nameof(WatchTaskInfo), taskid);
        }

        public bool AddWatchTask(WatchTaskInfo req)
        {
            if (req.DBServer==null)
            {
                throw new Exception("数据库不能为空");
            }
            if (string.IsNullOrWhiteSpace(req.Sql))
            {
                throw new Exception("SQL不能为空");
            }
            if (string.IsNullOrWhiteSpace(req.Name))
            {
                throw new Exception("name不能为空");
            }
            long total = 0;
            if (BigEntityTableEngine.LocalEngine.Scan<WatchTaskInfo>(nameof(WatchTaskInfo), "Name", new[] { req.Name }, new[] { req.Name }, 1, 1, ref total).FirstOrDefault() != null)
            {
                throw new Exception("name不能重复");
            }

            BigEntityTableEngine.LocalEngine.Insert<WatchTaskInfo>(nameof(WatchTaskInfo), new WatchTaskInfo
            {
                DBServer = req.DBServer,
                ErrorMsg = req.ErrorMsg,
                ErrorResult = req.ErrorResult,
                Sql = req.Sql,
                IsValid = req.IsValid,
                NullError = req.NullError,
                NotNullError = req.NotNullError,
                LastSuccessTime = DateTime.Now,
                Name = req.Name,
                Secs = req.Secs,
                ConnDB=req.ConnDB
            });
            return true;
        }

        public bool UpdateWatchTask(WatchTaskInfo req)
        {
            if (req.ID == 0)
            {
                throw new Exception("ID不能为空");
            }
            if (req.DBServer == null)
            {
                throw new Exception("数据库不能为空");
            }
            if (string.IsNullOrWhiteSpace(req.Sql))
            {
                throw new Exception("SQL不能为空");
            }
            if (string.IsNullOrWhiteSpace(req.Name))
            {
                throw new Exception("name不能为空");
            }

            var old = BigEntityTableEngine.LocalEngine.Find<WatchTaskInfo>(nameof(WatchTaskInfo), req.ID);
            if (old == null)
            {
                throw new Exception("数据不存在");
            }
            if (old.Name != req.Name)
            {
                long total = 0;
                if (BigEntityTableEngine.LocalEngine.Scan<WatchTaskInfo>(nameof(WatchTaskInfo), "Name", new[] { req.Name }, new[] { req.Name }, 1, 1, ref total).FirstOrDefault() != null)
                {
                    throw new Exception("name不能重复");
                }
            }
            if (req.DBServer == old.DBServer && req.NullError == old.NullError && req.Name == old.Name && req.IsValid == old.IsValid && req.ErrorMsg == old.ErrorMsg
                && req.ErrorResult == old.ErrorResult && req.Sql == old.Sql && req.Secs == old.Secs
                && req.NotNullError == old.NotNullError)
            {
                throw new Exception("数据未发生改变");
            }

            return BigEntityTableEngine.LocalEngine.Update<WatchTaskInfo>(nameof(WatchTaskInfo), new WatchTaskInfo
            {
                ID = req.ID,
                DBServer = req.DBServer,
                ErrorMsg = req.ErrorMsg,
                ErrorResult = req.ErrorResult,
                Sql = req.Sql,
                IsValid = req.IsValid,
                NullError = req.NullError,
                NotNullError = req.NotNullError,
                Name = req.Name,
                Secs = req.Secs,
                LastSuccessTime = old.LastSuccessTime,
                ConnDB=req.ConnDB,
                HasTriggerErr=old.HasTriggerErr
            });
        }

        public List<WatchTaskInfo> GetWatchTaskList()
        {
            return BigEntityTableEngine.LocalEngine.List<WatchTaskInfo>(nameof(WatchTaskInfo), 1, int.MaxValue).ToList();
        }

        public bool DelWatchTask(WatchTaskInfo req)
        {
            return BigEntityTableEngine.LocalEngine.Delete<WatchTaskInfo>(nameof(WatchTaskInfo), req.ID);
        }

        public List<WatchTaskLog> GetLogs(int taskid, DateTime start, DateTime end)
        {
            long total = 0;
            var list = BigEntityTableEngine.LocalEngine.ScanDesc<WatchTaskLog>(nameof(WatchTaskLog), "TaskId-CDate", new object[] { taskid, start },
                new object[] { taskid, end }, 1, int.MaxValue, ref total);

            return list;
        }

        public static void LoopTask()
        {
            try
            {
                if (StopTaskLoop)
                {
                    return;
                }
                foreach (var item in BigEntityTableEngine.LocalEngine.List<WatchTaskInfo>(nameof(WatchTaskInfo), 1, int.MaxValue))
                {
                    if (!item.IsValid)
                    {
                        continue;
                    }

                    if (item.LastSuccessTime.AddSeconds(item.Secs) > DateTime.Now)
                    {
                        continue;
                    }



                    try
                    {
                        bool haserror = false;
                        
                        var obj = Biz.Common.Data.SQLHelper.ExecuteScalar(item.DBServer,item.ConnDB, item.Sql);

                        if (obj == null && item.NullError)
                        {
                            haserror = true;
                        }
                        else if (obj != null && item.NotNullError)
                        {
                            haserror = true;
                        }
                        else if (obj != null && obj.ToString() == item.ErrorResult)
                        {
                            haserror = true;
                        }

                        if (haserror)
                        {
                            var content = item.ErrorMsg;
                            BigEntityTableEngine.LocalEngine.Insert<WatchTaskLog>(nameof(WatchTaskLog), new WatchTaskLog
                            {
                                CDate=DateTime.Now,
                                TaskId=item.ID,
                                Content=$"触发监控，值为：{obj}"
                            });
                        }
                        item.HasTriggerErr = haserror;
                        item.LastSuccessTime = DateTime.Now;
                        if (!StopTaskLoop)
                        {
                            BigEntityTableEngine.LocalEngine.Update<WatchTaskInfo>(nameof(WatchTaskInfo), item);

                            if (haserror && OnTiggerError != null)
                            {
                                OnTiggerError.BeginInvoke(item, obj, null, null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BigEntityTableEngine.LocalEngine.Insert<WatchTaskLog>(nameof(WatchTaskLog), new WatchTaskLog
                        {
                            CDate = DateTime.Now,
                            TaskId = item.ID,
                            Content = $"监控出错：{ex.ToString()}"
                        });
                        LogHelper.Instance.Error($"LoopTask innerError:{LJC.FrameWorkV3.Comm.JsonUtil<Object>.Serialize(item)}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error($"LoopTask innerError", ex);
            }
        }
    }
}
