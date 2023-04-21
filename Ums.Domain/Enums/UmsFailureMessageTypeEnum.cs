using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Enums
{
    /// <summary>
    /// 失败消息类型
    /// </summary>
    public enum UmsFailureMessageTypeEnum
    {
        /// <summary>
        /// 未确认发送成功
        /// </summary>
        UnConfirmed = 0,

        /// <summary>
        /// 未确认消费成功
        /// </summary>
        UnAcked = 1
    }
}
