using OneForAll.Core;
using System;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;

namespace Ums.Application
{
    /// <summary>
    /// 消息去重记录
    /// </summary>
    public class UmsDeduplicationRecordService : IUmsDeduplicationRecordService
    {
        private readonly IUmsDeduplicationRecordQueryService _queryService;
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;

        public UmsDeduplicationRecordService(
            IUmsDeduplicationRecordQueryService queryService,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _queryService = queryService;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="key">去重key关键字</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>分页</returns>
        public async Task<PageList<UmsDeduplicationRecord>> GetPageAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string key,
            UmsMessageTypeEnum? messageType)
        {
            return await _queryService.GetPageAsync(pageIndex, pageSize, startTime, endTime, key, messageType);
        }

        /// <summary>
        /// 添加去重记录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseMessage> AddAsync(UmsDeduplicationRecordForm form)
        {
            return await _deduplicationManager.AddRecordAsync(form);
        }
    }
}
