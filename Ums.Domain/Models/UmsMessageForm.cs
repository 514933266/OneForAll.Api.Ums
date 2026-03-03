using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Enums;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class UmsMessageForm
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public UmsMessageTypeEnum Type { get; set; }

        /// <summary>
        /// 接收账号id
        /// </summary>
        [Required]
        public Guid ToAccountId { get; set; }

    }
}
