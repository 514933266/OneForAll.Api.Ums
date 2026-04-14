using System;
using Ums.Domain.Enums;

namespace Ums.Application.Dtos
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public class UmsNotificationConfigDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 代码（唯一）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public UmsMessageTypeEnum NotificationType { get; set; }

        /// <summary>
        /// 目标值：[]
        /// </summary>
        public string TargetJson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
