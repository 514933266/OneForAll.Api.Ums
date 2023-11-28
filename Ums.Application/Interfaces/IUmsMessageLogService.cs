using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Domain.AggregateRoots;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 消息日志
    /// </summary>
    public interface IUmsMessageLogService
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="exChangeName">交换机名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routeKey">路由名</param>
        ///  <returns>分页</returns>
        Task<PageList<UmsMessageRecordDto>> GetPgaeAsync(
             int pageIndex,
             int pageSize,
             DateTime? startTime,
             DateTime? endTime,
             string exChangeName,
             string queueName,
             string routeKey);
    }
}
