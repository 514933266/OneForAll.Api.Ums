using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 消息去重记录仓储
    /// </summary>
    public interface IUmsDeduplicationRecordRepository : IEFCoreRepository<UmsDeduplicationRecord>
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="key">去重key关键字</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsDeduplicationRecord>> GetPageAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string key,
            UmsMessageTypeEnum? messageType);

        /// <summary>
        /// 根据去重key获取记录
        /// </summary>
        Task<UmsDeduplicationRecord> GetByKeyAsync(string deduplicationKey);

        /// <summary>
        /// 更新发送次数和最后发送时间
        /// </summary>
        Task UpdateSendCountAsync(Guid id, int sendCount, DateTime lastSendTime);

        /// <summary>
        /// 删除过期记录
        /// </summary>
        Task DeleteExpiredAsync(DateTime expireTime);

        /// <summary>
        /// 查询指定消息类型的全部去重记录（跟踪）
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <returns>记录列表</returns>
        Task<List<UmsDeduplicationRecord>> GetListByTypeAsync(UmsMessageTypeEnum messageType);
    }
}
