using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.ValueObjects
{
    /// <summary>
    /// 微信公众号消息队列名
    /// </summary>
    public class WxgzhQueueName
    {
        /// <summary>
        /// 模板消息
        /// </summary>
        public const string Template = "wxgzh.template.queue";

        /// <summary>
        /// 长期订阅消息
        /// </summary>
        public const string Subscribe = "wxgzh.subscribe.queue";
    }
}
