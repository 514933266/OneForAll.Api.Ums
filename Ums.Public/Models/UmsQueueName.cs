using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Public.Models
{
    /// <summary>
    /// 队列名
    /// </summary>
    public static class UmsQueueName
    {
        /// <summary>
        /// 系统通知
        /// </summary>
        public const string System = "system.ums.queue";

        /// <summary>
        /// 邮件
        /// </summary>
        public const string Email = "email.ums.queue";

        /// <summary>
        /// 短信
        /// </summary>
        public const string Sms = "sms.ums.queue";

        /// <summary>
        /// 微信公众号-模版消息
        /// </summary>
        public const string WxgzhTemplate = "template.wxgzh.ums.queue";

        /// <summary>
        /// 微信公众号-长期订阅
        /// </summary>
        public const string WxgzhSubscribe = "subscribe.wxgzh.ums.queue";

        /// <summary>
        /// 小程序-订阅模板消息
        /// </summary>
        public const string WxmpSubscribeTemplate = "subscribe.wxmp.ums.queue";

        /// <summary>
        /// 企业微信
        /// </summary>
        public const string WxqyRobotText = "robottext.wxqy.ums.queue";

        /// <summary>
        /// 企业微信
        /// </summary>
        public const string WxqyRobotMarkdown = "robottext.wxqy.ums.queue";

        /// <summary>
        /// 钉钉机器人Text
        /// </summary>
        public const string DingTalkRobotText = "robottext.dingtalk.ums.queue";

        /// <summary>
        /// 钉钉机器人Markdown
        /// </summary>
        public const string DingTalkRobotMarkdown = "robotmarkdown.dingtalk.ums.queue";

        /// <summary>
        /// 腾讯云短信
        /// </summary>
        public const string TxCloudSms = "sms.txcloud.ums.queue";
    }
}

