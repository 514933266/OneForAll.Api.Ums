using OneForAll.Core;
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
    /// 失败消息记录
    /// </summary>
    public class UmsMessageRecord
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 当次消息的id
        /// </summary>
        [Required]
        public Guid MessageId { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        [Required]
        public string Result { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ExChangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string QueueName { get; set; }

        /// <summary>
        /// 路由键名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string RouteKey { get; set; }

        /// <summary>
        /// 原始消息
        /// </summary>
        [Required]
        public string OriginalMessage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

    }
}
