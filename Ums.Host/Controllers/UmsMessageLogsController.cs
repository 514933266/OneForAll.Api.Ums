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
    /// 消息日志
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class UmsMessageLogsController : BaseController
    {
        private readonly IUmsMessageLogService _service;
        public UmsMessageLogsController(IUmsMessageLogService service)
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
		/// <param name="exChangeName">交换机名</param>
		/// <param name="queueName">队列名</param>
        /// <param name="routeKey">路由名</param>
		///  <returns>分页</returns>
		[HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<PageList<UmsMessageRecordDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] DateTime? startTime,
            [FromQuery] DateTime? endTime,
            [FromQuery] string exChangeName,
            [FromQuery] string queueName,
            [FromQuery] string routeKey)
        {
            return await _service.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, exChangeName, queueName, routeKey);
        }
    }
}
