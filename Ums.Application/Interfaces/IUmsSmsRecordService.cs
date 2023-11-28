using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Dtos;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    public interface IUmsSmsRecordService
    {

        /// <summary>
		/// 查询分页
		/// </summary>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <param name="platformName">平台名称</param>
		///  <returns>分页</returns>
		Task<PageList<UmsSmsRecordDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string platformName);
    }
}
