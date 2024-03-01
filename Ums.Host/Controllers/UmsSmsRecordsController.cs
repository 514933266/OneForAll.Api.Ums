using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Public.Models;
using Ums.Host.Controllers;
using Ums.Domain.Models;
using Ums.Application.Interfaces;
using Ums.Host.Filters;
using Ums.Application.Dtos;
using OneForAll.Core.OAuth;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
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
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<PageList<UmsSmsRecordDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] DateTime? startTime,
            [FromQuery] DateTime? endTime,
            [FromQuery] string platformName)
        {
            return await _service.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, platformName);
        }
    }
}
