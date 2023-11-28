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
    /// 消息记录
    /// </summary>
    internal class UmsMessageRecordRepository : Repository<UmsMessageRecord>, IUmsMessageRecordRepository
    {
        public UmsMessageRecordRepository(DbContext context)
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
		/// <param name="exChangeName">交换机名</param>
		/// <param name="queueName">队列名</param>
        /// <param name="routeKey">路由名</param>
		///  <returns>分页列表</returns>
		public async Task<PageList<UmsMessageRecord>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string exChangeName,
            string queueName,
            string routeKey)
        {
            var predicate = PredicateBuilder.Create<UmsMessageRecord>(w => true);

            if (!exChangeName.IsNullOrEmpty())
                predicate = predicate.And(w => w.ExChangeName.Contains(exChangeName));

            if (startTime != null)
                predicate = predicate.And(w => w.CreateTime >= startTime);

            if (endTime != null)
                predicate = predicate.And(w => w.CreateTime <= endTime);

            if (!queueName.IsNullOrEmpty())
                predicate = predicate.And(w => w.QueueName.Contains(queueName));

            if (!routeKey.IsNullOrEmpty())
                predicate = predicate.And(w => w.RouteKey.Contains(routeKey));

            var total = await DbSet.AsNoTracking().CountAsync(predicate);

            var items = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(w => w.CreateTime)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PageList<UmsMessageRecord>(total, pageIndex, pageSize, items);
        }
    }
}