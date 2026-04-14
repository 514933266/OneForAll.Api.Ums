using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 微信机器人
    /// </summary>
    public class WxqyRobotMessageForm
    {
        /// <summary>
        /// 客户端id（与ConfigCode配合使用，从通知配置中获取机器人地址）
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 配置代码（与ClientId配合使用，从通知配置中获取机器人地址）
        /// </summary>
        public string ConfigCode { get; set; }

        /// <summary>
        /// 机器人地址
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 接收消息的用户id
        /// </summary>
        public List<string> UserIds { get; set; } = new List<string>();

        /// <summary>
        /// 接收消息的用户手机列表
        /// </summary>
        public List<string> Mobiles { get; set; } = new List<string>();
    }
}
