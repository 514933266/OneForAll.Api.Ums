using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Application.Interfaces;
using Ums.Application.Dtos;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UmsSmsRecordsController : BaseController
    {
        private readonly IUmsSmsRecordService _service;
        public UmsSmsRecordsController(IUmsSmsRecordService service)
        {
            _service = service;
        }

        /// <summary>
		/// 查询分页
		/// </summary>
		/// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <param name="platformName">所属平台</param>
		///  <returns>分页</returns>
		[HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<UmsSmsRecordDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] DateTime? startTime,
            [FromQuery] DateTime? endTime,
            [FromQuery] string platformName = default)
        {
            return await _service.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, platformName);
        }
    }
}
