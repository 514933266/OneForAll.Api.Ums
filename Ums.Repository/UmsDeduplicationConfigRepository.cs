using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
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
    /// 消息去重配置仓储
    /// </summary>
    public class UmsDeduplicationConfigRepository : Repository<UmsDeduplicationConfig>, IUmsDeduplicationConfigRepository
    {
        public UmsDeduplicationConfigRepository(DbContext context)
            : base(context)
        {

        }
    }
}
