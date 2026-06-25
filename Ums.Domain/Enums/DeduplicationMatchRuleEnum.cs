using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Enums
{
    /// <summary>
    /// 消息去重匹配规则
    /// </summary>
    public enum DeduplicationMatchRuleEnum
    {
        /// <summary>
        /// 标题
        /// </summary>
        Title = 0,

        /// <summary>
        /// 标题+类型
        /// </summary>
        TitleAndType = 1,

        /// <summary>
        /// 类型
        /// </summary>
        Type = 2,

        /// <summary>
        /// 内容
        /// </summary>
        Content = 3,

        /// <summary>
        /// 内容+类型
        /// </summary>
        ContentAndType = 4,
    }
}
