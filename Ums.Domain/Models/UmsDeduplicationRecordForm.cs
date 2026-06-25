using System.ComponentModel.DataAnnotations;
using Ums.Domain.Enums;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 消息去重记录表单
    /// </summary>
    public class UmsDeduplicationRecordForm
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(200, ErrorMessage = "标题长度不能超过200个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        [StringLength(2000, ErrorMessage = "内容长度不能超过2000个字符")]
        public string Content { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [Required(ErrorMessage = "消息类型不能为空")]
        public UmsMessageTypeEnum MessageType { get; set; }

        /// <summary>
        /// 匹配规则
        /// </summary>
        [Required(ErrorMessage = "匹配规则不能为空")]
        public DeduplicationMatchRuleEnum MatchRule { get; set; }
    }
}
