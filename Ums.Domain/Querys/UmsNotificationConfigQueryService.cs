using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;

namespace Ums.Domain.Querys
{
    /// <summary>
    /// 消息通知配置查询服务
    /// </summary>
    public class UmsNotificationConfigQueryService : IUmsNotificationConfigQueryService
    {
        private readonly IUmsNotificationConfigRepository _repository;

        public UmsNotificationConfigQueryService(
            IUmsNotificationConfigRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 根据客户端id、代码和通知类型查询配置
        /// </summary>
        /// <param name="clientId">客户端id</param>
        /// <param name="code">配置代码</param>
        /// <param name="type">通知类型</param>
        /// <returns>配置实体</returns>
        public async Task<UmsNotificationConfig> GetAsync(string clientId, string code, UmsMessageTypeEnum type)
        {
            return await _repository.GetAsync(clientId, code, type);
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
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            return await _repository.GetPageAsync(pageIndex, pageSize, clientId);
        }
    }
}
