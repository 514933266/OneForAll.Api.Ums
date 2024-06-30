using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 消息记录
    /// </summary>
    public interface IUmsMessageRecordRepository : IEFCoreRepository<UmsMessageRecord>
    {
        /// <summary>
		/// 查询分页
		/// </summary>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <param name="exChangeName">交换机名</param>
		/// <param name="queueName">队列名</param>
        /// <param name="routeKey">路由名</param>
		///  <returns>分页列表</returns>
		Task<PageList<UmsMessageRecord>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string exChangeName,
            string queueName,
            string routeKey);

        /// <summary>
        /// 当天未发送队列消息（超一小时）
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UmsMessageRecord>> GetListExpiryANTAsync();

        /// <summary>
        /// 查询指定消息id数据
        /// </summary>
        /// <returns></returns>
        Task<UmsMessageRecord> GetByMessageANTAsync(Guid messageId);
    }
}
