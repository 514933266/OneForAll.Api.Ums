using System;
using System.ComponentModel.DataAnnotations;
using Ums.Domain.Enums;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 消息通知配置表单
    /// </summary>
    public class UmsNotificationConfigForm
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        [Required]
        public string ClientId { get; set; }

        /// <summary>
        /// 代码（唯一）
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        [Required]
        public UmsMessageTypeEnum NotificationType { get; set; }

        /// <summary>
        /// 目标值：[]
        /// </summary>
        [Required]
        public string TargetJson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }
    }
}
