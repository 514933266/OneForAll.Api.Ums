using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Enums
{
    /// <summary>
    /// 消息去重策略
    /// </summary>
    public enum DeduplicationStrategyEnum
    {
        /// <summary>
        /// 时间窗口去重
        /// </summary>
        TimeWindow = 0,
        
        /// <summary>
        /// 永久去重
        /// </summary>
        Permanent = 1
    }
}
