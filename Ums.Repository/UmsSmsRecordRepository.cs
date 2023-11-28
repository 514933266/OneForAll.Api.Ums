using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Repositorys;
using OneForAll.Core.Extension;

namespace Ums.Repository
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    public class UmsSmsRecordRepository : Repository<UmsSmsRecord>, IUmsSmsRecordRepository
    {
        public UmsSmsRecordRepository(DbContext context)
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
		/// <param name="platformName">所属平台</param>
		///  <returns>分页列表</returns>
		public async Task<PageList<UmsSmsRecord>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string platformName)
        {
            var predicate = PredicateBuilder.Create<UmsSmsRecord>(w => true);

            if (!platformName.IsNullOrEmpty())
                predicate = predicate.And(w => w.PlatformName.Contains(platformName));

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

            return new PageList<UmsSmsRecord>(total, pageIndex, pageSize, items);
        }
    }
}
