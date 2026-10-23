using OneForAll.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 个人消息查询服务
    /// </summary>
    public interface IUmsPersonalMessageQueryService
    {
        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="top">数量</param>
        /// <returns>列表</returns>
        Task<IEnumerable<UmsMessage>> GetListAsync(int top);

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="day">近几天</param>
        /// <returns>列表</returns>
        Task<IEnumerable<UmsMessage>> GetListByDayAsync(int day);

        /// <summary>
        /// 获取消息分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsMessage>> GetPageAsync(int pageIndex, int pageSize, string key, UmsMessageReadStatusEnum status);

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>分页列表</returns>
        Task<int> GetUnReadCountAsync();
    }
}
