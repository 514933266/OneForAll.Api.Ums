using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Enums
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public enum UmsMessageTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 链接
        /// </summary>
        Url = 1,

        /// <summary>
        /// 企业微信机器人
        /// </summary>
        WxQtRoot = 2,

        /// <summary>
        /// 钉钉机器人
        /// </summary>
        DingTalkRobot = 3,

        /// <summary>
        /// 邮件通知
        /// </summary>
        Email = 4
    }
}
