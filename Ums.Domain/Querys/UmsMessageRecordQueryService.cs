using OneForAll.Core;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;

namespace Ums.Domain.Querys
{
    /// <summary>
    /// 消息日志查询服务
    /// </summary>
    public class UmsMessageRecordQueryService : IUmsMessageRecordQueryService
    {
        private readonly IUmsMessageRecordRepository _repository;

        public UmsMessageRecordQueryService(
            IUmsMessageRecordRepository repository)
        {
            _repository = repository;
        }

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
        public async Task<PageList<UmsMessageRecord>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string exChangeName,
            string queueName,
            string routeKey)
        {
            return await _repository.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, exChangeName, queueName, routeKey);
        }
    }
}
