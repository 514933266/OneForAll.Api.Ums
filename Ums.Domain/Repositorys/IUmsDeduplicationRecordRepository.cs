using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 消息去重记录仓储
    /// </summary>
    public interface IUmsDeduplicationRecordRepository : IEFCoreRepository<UmsDeduplicationRecord>
    {
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
    }
}
