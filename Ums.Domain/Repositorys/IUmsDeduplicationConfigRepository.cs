using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 查询指定消息类型的已启用配置
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <returns>配置列表</returns>
        Task<List<UmsDeduplicationConfig>> GetEnabledListByTypeAsync(UmsMessageTypeEnum messageType);
    }
}
