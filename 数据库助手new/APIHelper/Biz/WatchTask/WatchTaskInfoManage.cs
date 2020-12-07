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
        public static Action<WatchTaskInfo> OnErrorDisappear = null;

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
            return true;
        }

        public bool UpdateWatchTask(WatchTaskInfo req)
        {
            if (req.ID == 0)
            {
                throw new Exception("ID不能为空");
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
            if (req.NullError == old.NullError && req.Name == old.Name && req.IsValid == old.IsValid && req.ErrorMsg == old.ErrorMsg
                && req.ErrorResult == old.ErrorResult && req.Sql == old.Sql && req.Secs == old.Secs
                && req.NotNullError == old.NotNullError)
            {
                throw new Exception("数据未发生改变");
            }

            return BigEntityTableEngine.LocalEngine.Update<WatchTaskInfo>(nameof(WatchTaskInfo), new WatchTaskInfo
            {
                ID = req.ID,
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

        public bool FindAndUpdate(int taskid,Func<WatchTaskInfo,bool> update)
        {
            var task = Get(taskid);
            if (task == null)
            {
                throw new Exception("任务不存在");
            }
            if (update(task))
            {
                if (task.ID != taskid)
                {
                    throw new Exception("ID不可修改");
                }
                return BigEntityTableEngine.LocalEngine.Update(nameof(WatchTaskInfo), task);
            }
            else
            {
                return false;
            }
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

                        if (haserror)
                        {
                            var content = item.ErrorMsg;
                            BigEntityTableEngine.LocalEngine.Insert<WatchTaskLog>(nameof(WatchTaskLog), new WatchTaskLog
                            {
                                CDate=DateTime.Now,
                                TaskId=item.ID,
                                Content=$"触发监控，值为"
                            });
                        }
                        var lasthaserror = item.HasTriggerErr;
                        item.HasTriggerErr = haserror;
                        item.LastSuccessTime = DateTime.Now;
                        if (!StopTaskLoop)
                        {
                            BigEntityTableEngine.LocalEngine.Update<WatchTaskInfo>(nameof(WatchTaskInfo), item);

                            if (haserror && OnTiggerError != null)
                            {
                            }
                            else if (lasthaserror && !haserror && OnErrorDisappear != null)
                            {
                                OnErrorDisappear.BeginInvoke(item, null, null);
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
