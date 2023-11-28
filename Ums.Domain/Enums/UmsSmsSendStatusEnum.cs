using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Enums
{
    /// <summary>
    /// 短信发送状态
    /// </summary>
    public enum UmsSmsSendStatusEnum
    {
        /// <summary>
        /// 待发送
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// 发送成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 异常
        /// </summary>
        Error = 99
    }
}
