using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Enums
{
    /// <summary>
    /// Mq消息状态
    /// </summary>
    public enum UmsMessageStatusEnum
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 等待
        /// </summary>
        Pending = 2,
        /// <summary>
        /// 异常
        /// </summary>
        Error = 99
    }
}
