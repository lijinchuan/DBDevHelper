using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.WatchTask
{
    public class WatchTaskInfo
    {
        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }


        public DBSource DBServer
        {
            get;
            set;
        }

        public string ConnDB
        {
            get;
            set;
        }

        public string Sql
        {
            get;
            set;
        }

        /// <summary>
        /// 错误
        /// </summary>
        public string ErrorResult
        {
            get;
            set;
        }

        /// <summary>
        /// 查询为空报错
        /// </summary>
        public bool NullError
        {
            get;
            set;
        }

        /// <summary>
        /// 查询不为空时报错
        /// </summary>
        public bool NotNullError
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            get;
            set;
        }

        public bool IsValid
        {
            get;
            set;
        }

        /// <summary>
        /// 监控频率，秒
        /// </summary>
        public int Secs
        {
            get;
            set;
        }

        /// <summary>
        /// 是否触发了错误
        /// </summary>
        public bool HasTriggerErr
        {
            get;
            set;
        }

        /// <summary>
        /// 执行成功时间
        /// </summary>
        public DateTime LastSuccessTime
        {
            get;
            set;
        }
    }
}
