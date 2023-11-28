using AutoMapper;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using OneForAll.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;

namespace Ums.Domain
{
    /// <summary>
    /// 消息日志
    /// </summary>
    public class UmsMessageRecordManager : UmsBaseManager, IUmsMessageRecordManager
    {
        private readonly IUmsMessageRecordRepository _repository;

        public UmsMessageRecordManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository) : base(mapper, httpContextAccessor)
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
