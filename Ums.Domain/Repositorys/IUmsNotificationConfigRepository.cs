using OneForAll.Core;
using OneForAll.EFCore;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public interface IUmsNotificationConfigRepository : IEFCoreRepository<UmsNotificationConfig>
    {
        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsNotificationConfig>> GetPageAsync(int pageIndex, int pageSize, string clientId);

        /// <summary>
        /// 根据客户端id、代码和通知类型查询配置
        /// </summary>
        /// <param name="clientId">客户端id</param>
        /// <param name="code">配置代码</param>
        /// <param name="type">通知类型</param>
        /// <returns>配置</returns>
        Task<UmsNotificationConfig> GetAsync(string clientId, string code, UmsMessageTypeEnum type);
    }
}
