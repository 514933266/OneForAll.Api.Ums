using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public interface IUmsMessageRepository : IEFCoreRepository<UmsMessage>
    {
        /// <summary>
        /// 查询用户消息分页列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsMessage>> GetPageAsync(Guid userId, int pageIndex, int pageSize, string key, UmsMessageReadStatusEnum status);

        /// <summary>
        /// 查询用户前x条未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="top">数量</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetListAsync(Guid userId, int top);

        /// <summary>
        /// 查询用户近x天未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="day">近几天</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetListByDayAsync(Guid userId, int day);

        /// <summary>
        /// 查询用户消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="ids">消息id</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetListByIdAsync(Guid userId, IEnumerable<Guid> ids);

        /// <summary>
        /// 查询用户未读数量
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<int> GetUnReadCountAsync(Guid userId);

        /// <summary>
        /// 查询用户未读列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetUnReadListAsync(Guid userId);
    }
}
