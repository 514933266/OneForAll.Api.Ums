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
