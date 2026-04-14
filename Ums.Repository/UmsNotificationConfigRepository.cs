using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Repositorys;

namespace Ums.Repository
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public class UmsNotificationConfigRepository : Repository<UmsNotificationConfig>, IUmsNotificationConfigRepository
    {
        public UmsNotificationConfigRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsNotificationConfig>> GetPageAsync(int pageIndex, int pageSize, string clientId)
        {
            var predicate = PredicateBuilder.Create<UmsNotificationConfig>(w => true);

            if (!string.IsNullOrEmpty(clientId))
                predicate = predicate.And(w => w.ClientId.Contains(clientId));

            var total = await DbSet.AsNoTracking().CountAsync(predicate);

            var items = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(w => w.CreateTime)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PageList<UmsNotificationConfig>(total, pageIndex, pageSize, items);
        }

        /// <summary>
        /// 根据客户端id、代码和通知类型查询配置
        /// </summary>
        /// <param name="clientId">客户端id</param>
        /// <param name="code">配置代码</param>
        /// <param name="type">通知类型</param>
        /// <returns>配置</returns>
        public async Task<UmsNotificationConfig> GetAsync(string clientId, string code, UmsMessageTypeEnum type)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.ClientId == clientId && w.Code == code && w.NotificationType == type && w.IsEnabled);
        }
    }
}
