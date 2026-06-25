using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Domain.Enums;

namespace Ums.Domain.Entities
{
    /// <summary>
    /// 消息去重记录（用于阶梯式时间窗口）
    /// </summary>
    public class UmsDeduplicationRecord
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 去重key（由消息类型、标题、内容生成）
        /// </summary>
        [Required]
        [StringLength(100)]
        public string MessageKey { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [Required]
        public UmsMessageTypeEnum MessageType { get; set; }

        /// <summary>
        /// 发送次数（第几次发送）
        /// </summary>
        [Required]
        public int SendCount { get; set; } = 1;

        /// <summary>
        /// 最后发送时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime LastSendTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 过期时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
