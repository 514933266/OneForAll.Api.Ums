using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Application.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 消息去重记录
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UmsDeduplicationRecordsController : BaseController
    {
        private readonly IUmsDeduplicationRecordService _service;

        public UmsDeduplicationRecordsController(IUmsDeduplicationRecordService service)
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
        /// <param name="key">去重key关键字</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>分页</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<UmsDeduplicationRecord>> GetPageAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] DateTime? startTime = default,
            [FromQuery] DateTime? endTime = default,
            [FromQuery] string key = default,
            [FromQuery] UmsMessageTypeEnum? messageType = default)
        {
            return await _service.GetPageAsync(pageIndex, pageSize, startTime, endTime, key, messageType);
        }

        /// <summary>
        /// 添加去重记录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] UmsDeduplicationRecordForm form)
        {
            return await _service.AddAsync(form);
        }
    }
}
