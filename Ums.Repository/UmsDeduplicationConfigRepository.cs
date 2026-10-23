using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Repositorys;

namespace Ums.Repository
{
    /// <summary>
    /// 消息去重配置仓储
    /// </summary>
    public class UmsDeduplicationConfigRepository : Repository<UmsDeduplicationConfig>, IUmsDeduplicationConfigRepository
    {
        public UmsDeduplicationConfigRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询指定消息类型的已启用配置
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <returns>配置列表</returns>
        public async Task<List<UmsDeduplicationConfig>> GetEnabledListByTypeAsync(UmsMessageTypeEnum messageType)
        {
            return await DbSet.Where(w => w.MessageType == messageType && w.IsEnabled).ToListAsync();
        }
    }
}
