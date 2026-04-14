using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 邮件消息
    /// </summary>
    public class UmsEmailMessageForm
    {
        /// <summary>
        /// 客户端id（与ConfigCode配合使用，从通知配置中获取邮件发送配置）
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 配置代码（与ClientId配合使用，从通知配置中获取邮件发送配置）
        /// </summary>
        public string ConfigCode { get; set; }

        /// <summary>
        /// 收件人邮箱（多个以,号隔开）
        /// </summary>
        [Required]
        public string To { get; set; }

        /// <summary>
        /// 抄送邮箱（多个以,号隔开）
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        /// <summary>
        /// 邮件正文
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// 是否为Html内容
        /// </summary>
        public bool IsHtml { get; set; } = true;
    }
}
