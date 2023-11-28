using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Application.Dtos
{
    /// <summary>
    /// 消息记录
    /// </summary>
    public class UmsMessageRecordDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExChangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 路由键名称
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 原始消息
        /// </summary>
        public string OriginalMessage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
