using OneForAll.Core;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public interface IUmsNotificationConfigManager
    {
        /// <summary>
        /// 根据客户端id、代码和通知类型查询配置
        /// </summary>
        /// <param name="clientId">客户端id</param>
        /// <param name="code">配置代码</param>
        /// <param name="type">通知类型</param>
        /// <returns>配置实体</returns>
        Task<UmsNotificationConfig> GetAsync(string clientId, string code, UmsMessageTypeEnum type);

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsNotificationConfig>> GetPageAsync(int pageIndex, int pageSize, string clientId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(UmsNotificationConfigForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">实体id</param>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(Guid id, UmsNotificationConfigForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(Guid id);

    }
}
