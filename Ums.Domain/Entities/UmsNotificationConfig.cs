using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Enums;

namespace Ums.Domain.Entities
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public class UmsNotificationConfig
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        [Required]
        public string ClientId { get; set; }

        /// <summary>
        /// 代码（唯一）
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(50)")]
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
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }
    }
}
