using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Domain.Enums;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 个人消息
    /// </summary>
    public interface IUmsPersonalMessageService
    {
        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="top">数量</param>
        /// <returns>列表</returns>
        Task<IEnumerable<UmsPersonalMessageDto>> GetListAsync(int top);

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="day">近几天</param>
        /// <returns>列表</returns>
        Task<IEnumerable<UmsPersonalMessageDto>> GetListByDayAsync(int day);

        /// <summary>
        /// 获取消息分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsPersonalMessageDto>> GetPageAsync(int pageIndex, int pageSize, string key, UmsMessageReadStatusEnum status);

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>结果</returns>
        Task<int> GetUnReadCountAsync();

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);
    }
}

