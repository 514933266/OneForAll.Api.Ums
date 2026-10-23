using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 个人消息
    /// </summary>
    public interface IUmsPersonalMessageManager
    {
        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAllAsync();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync();
    }
}
