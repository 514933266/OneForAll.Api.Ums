using Microsoft.EntityFrameworkCore;
using NPOI.XWPF.UserModel;
using OneForAll.Core;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Repositorys;

namespace Ums.Repository
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class UmsMessageRepository : Repository<UmsMessage>, IUmsMessageRepository
    {
        public UmsMessageRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询用户消息分页列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsMessage>> GetPageAsync(Guid userId, int pageIndex, int pageSize, string key, UmsMessageReadStatusEnum status)
        {
            var predicate = PredicateBuilder.Create<UmsMessage>(w => w.ToAccountId == userId);
            switch (status)
            {
                case UmsMessageReadStatusEnum.UnRead:
                    predicate = predicate.And(w => !w.IsRead);
                    break;
                case UmsMessageReadStatusEnum.Readed:
                    predicate = predicate.And(w => w.IsRead);
                    break;
            }

            if (!string.IsNullOrEmpty(key))
            {
                predicate = predicate.And(w => w.Title.Contains(key));
            }

            var total = await DbSet
                .CountAsync(predicate);

            var data = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(o => o.IsRead)
                .ThenByDescending(o => o.CreateTime)
                .ToListAsync();

            return new PageList<UmsMessage>(total, pageSize, pageIndex, data);
        }

        /// <summary>
        /// 查询用户前x条未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="top">数量</param>
        /// <returns></returns>
        public async Task<IEnumerable<UmsMessage>> GetListAsync(Guid userId, int top)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => w.ToAccountId == userId && !w.IsRead)
                .Take(top)
                .OrderByDescending(o => o.CreateTime)
                .ToListAsync();
        }

        /// <summary>
        /// 查询用户近x天未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="day">近几天</param>
        /// <returns></returns>
        public async Task<IEnumerable<UmsMessage>> GetListByDayAsync(Guid userId, int day)
        {
            var beginDate = DateTime.Now.AddDays(-day).Date;
            return await DbSet
                .AsNoTracking()
                .Where(w => w.ToAccountId == userId && !w.IsRead && w.CreateTime >= beginDate && w.CreateTime <= DateTime.Now)
                .OrderByDescending(o => o.CreateTime)
                .ToListAsync();
        }

        /// <summary>
        /// 查询用户消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="ids">消息id</param>
        /// <returns></returns>
        public async Task<IEnumerable<UmsMessage>> GetListByIdAsync(Guid userId, IEnumerable<Guid> ids)
        {
            return await DbSet.Where(w => w.ToAccountId == userId && ids.Contains(w.Id)).ToListAsync();
        }

        /// <summary>
        /// 查询用户未读数量
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<int> GetUnReadCountAsync(Guid userId)
        {
            return await DbSet.CountAsync(w => w.ToAccountId == userId && !w.IsRead);
        }

        /// <summary>
        /// 查询用户未读列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<IEnumerable<UmsMessage>> GetUnReadListAsync(Guid userId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => w.ToAccountId == userId && !w.IsRead)
                .OrderByDescending(o => o.CreateTime)
                .ToListAsync();
        }
    }
}

