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
    /// 消息去重配置
    /// </summary>
    public class UmsDeduplicationConfig
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 消息类型（为空表示所有类型）
        /// </summary>
        [Required]
        public UmsMessageTypeEnum MessageType { get; set; } = UmsMessageTypeEnum.Default;

        /// <summary>
        /// 降噪策略：TimeWindow(时间窗口), Permanent(永久去重)
        /// </summary>
        [Required]
        public DeduplicationStrategyEnum Strategy { get; set; }

        /// <summary>
        /// 时间窗口（分钟），仅TimeWindow策略有效
        /// </summary>
        [Required]
        public int TimeWindowMinutes { get; set; } = 30;

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 匹配规则
        /// </summary>
        [Required]
        public DeduplicationMatchRuleEnum MatchRule { get; set; } = DeduplicationMatchRuleEnum.Title;

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
        public string Remark { get; set; } = "";
    }
}
