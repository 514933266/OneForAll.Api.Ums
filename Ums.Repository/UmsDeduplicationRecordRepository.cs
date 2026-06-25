using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Repositorys;

namespace Ums.Repository
{
    /// <summary>
    /// 消息去重记录仓储
    /// </summary>
    public class UmsDeduplicationRecordRepository : Repository<UmsDeduplicationRecord>, IUmsDeduplicationRecordRepository
    {
        public UmsDeduplicationRecordRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 根据去重key获取记录
        /// </summary>
        public async Task<UmsDeduplicationRecord> GetByKeyAsync(string deduplicationKey)
        {
            return await DbSet.FirstOrDefaultAsync(w => w.MessageKey == deduplicationKey);
        }

        /// <summary>
        /// 更新发送次数和最后发送时间
        /// </summary>
        public async Task UpdateSendCountAsync(Guid id, int sendCount, DateTime lastSendTime)
        {
            var record = await DbSet.FindAsync(id);
            if (record != null)
            {
                record.SendCount = sendCount;
                record.LastSendTime = lastSendTime;
                await UpdateAsync(record);
            }
        }

        /// <summary>
        /// 删除过期记录
        /// </summary>
        public async Task DeleteExpiredAsync(DateTime expireTime)
        {
            var expiredRecords = await DbSet
                .Where(w => w.ExpireTime.HasValue && w.ExpireTime.Value < expireTime)
                .ToListAsync();

            if (expiredRecords.Any())
            {
                DbSet.RemoveRange(expiredRecords);
                await SaveChangesAsync();
            }
        }
    }
}
