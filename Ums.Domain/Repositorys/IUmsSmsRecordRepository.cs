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
    /// 短信发送记录
    /// </summary>
    public interface IUmsSmsRecordRepository : IEFCoreRepository<UmsSmsRecord>
    {
        /// <summary>
		/// 查询分页
		/// </summary>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <param name="platformName">所属平台</param>
		///  <returns>分页列表</returns>
		Task<PageList<UmsSmsRecord>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string platformName);
    }
}
