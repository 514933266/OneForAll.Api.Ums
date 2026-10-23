using OneForAll.Core;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 消息去重记录查询服务
    /// </summary>
    public interface IUmsDeduplicationRecordQueryService
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="key">去重key关键字</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>分页</returns>
        Task<PageList<UmsDeduplicationRecord>> GetPageAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string key,
            UmsMessageTypeEnum? messageType);
    }
}
