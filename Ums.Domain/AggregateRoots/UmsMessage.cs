using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Enums;

namespace Ums.Domain.AggregateRoots
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class UmsMessage : AggregateRoot<Guid>, ICreateTime
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public UmsMessageTypeEnum Type { get; set; }

        /// <summary>
        /// 接收账号id
        /// </summary>
        [Required]
        public Guid ToAccountId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}


