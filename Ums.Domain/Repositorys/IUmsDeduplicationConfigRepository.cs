using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 消息去重配置仓储
    /// </summary>
    public interface IUmsDeduplicationConfigRepository : IEFCoreRepository<UmsDeduplicationConfig>
    {
    }
}
