using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Application.Interfaces;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;

namespace Ums.Application
{
    /// <summary>
    /// 消息日志
    /// </summary>
    public class UmsMessageLogService : IUmsMessageLogService
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageRecordManager _manager;
        public UmsMessageLogService(
            IMapper mapper,
            IUmsMessageRecordManager manager)
        {
            _mapper = mapper;
            _manager = manager;
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
		public async Task<PageList<UmsMessageRecordDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string exChangeName,
            string queueName,
            string routeKey)
        {
            var data = await _manager.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, exChangeName, queueName, routeKey);
            var items = _mapper.Map<IEnumerable<UmsMessageRecord>, IEnumerable<UmsMessageRecordDto>>(data.Items);
            return new PageList<UmsMessageRecordDto>(data.Total, data.PageIndex, data.PageSize, items);
        }
    }
}
