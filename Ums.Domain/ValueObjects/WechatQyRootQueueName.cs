using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.ValueObjects
{
    /// <summary>
    /// 企业微信机器人队列
    /// </summary>
    public static class WechatQyRootQueueName
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public const string Text = "wechatqy.robot.text.queue";

        /// <summary>
        /// Markdown消息
        /// </summary>
        public const string Markdown = "wechatqy.robot.markdown.queue";
    }
}
