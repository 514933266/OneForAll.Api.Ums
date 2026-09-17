using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
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
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="key">去重key关键字</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsDeduplicationRecord>> GetPageAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string key,
            UmsMessageTypeEnum? messageType)
        {
            var predicate = PredicateBuilder.Create<UmsDeduplicationRecord>(w => true);

            if (!key.IsNullOrEmpty())
                predicate = predicate.And(w => w.MessageKey.Contains(key));

            if (messageType != null)
                predicate = predicate.And(w => w.MessageType == messageType);

            if (startTime != null)
                predicate = predicate.And(w => w.CreateTime >= startTime);

            if (endTime != null)
                predicate = predicate.And(w => w.CreateTime <= endTime);

            var total = await DbSet.AsNoTracking().CountAsync(predicate);

            var items = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(w => w.CreateTime)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PageList<UmsDeduplicationRecord>(total, pageIndex, pageSize, items);
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
