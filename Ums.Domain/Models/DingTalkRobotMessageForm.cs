using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 钉钉机器人
    /// </summary>
    public class DingTalkRobotMessageForm
    {
        /// <summary>
        /// 客户端id（与ConfigCode配合使用，从通知配置中获取机器人地址和签名密钥）
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 配置代码（与ClientId配合使用，从通知配置中获取机器人地址和签名密钥）
        /// </summary>
        public string ConfigCode { get; set; }

        /// <summary>
        /// 机器人地址
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// 签名密钥
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 标题（Markdown消息使用）
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 被@人的手机号
        /// </summary>
        public List<string> AtMobiles { get; set; } = new List<string>();

        /// <summary>
        /// 被@人的用户id
        /// </summary>
        public List<string> AtUserIds { get; set; } = new List<string>();

        /// <summary>
        /// 是否@所有人
        /// </summary>
        public bool IsAtAll { get; set; }
    }
}
