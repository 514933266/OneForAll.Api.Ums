using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.ValueObjects
{
    /// <summary>
    /// 队列名
    /// </summary>
    public static class UmsQueueName
    {
        /// <summary>
        /// 系统通知
        /// </summary>
        public const string System = "ums.system.queue";

        /// <summary>
        /// 邮件
        /// </summary>
        public const string Email = "ums.email.queue";
    }
}
