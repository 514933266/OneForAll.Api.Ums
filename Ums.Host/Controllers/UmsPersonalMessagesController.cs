using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Application.Interfaces;
using Ums.Domain.Enums;
using Ums.Public.Models;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 个人消息中心
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class UmsPersonalMessagesController : BaseController
    {
        private readonly IUmsPersonalMessageService _service;

        public UmsPersonalMessagesController(IUmsPersonalMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="top">前几条</param>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("{top}")]
        public async Task<IEnumerable<UmsPersonalMessageDto>> GetListAsync(int top)
        {
            return await _service.GetListAsync(top);
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="day">近几天</param>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("{day}/UnReads")]
        public async Task<IEnumerable<UmsPersonalMessageDto>> GetListByDayAsync(int day)
        {
            return await _service.GetListByDayAsync(day);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key">关键字</param>
        /// <param name="status"></param>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<UmsPersonalMessageDto>> GetPageAsync(int pageIndex, int pageSize, [FromQuery] string key, [FromQuery] UmsMessageReadStatusEnum status)
        {
            return await _service.GetPageAsync(pageIndex, pageSize, key, status);
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("UnReadCount")]
        public async Task<int> GetUnReadCountAsync()
        {
            return await _service.GetUnReadCountAsync();
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id，为空时将全部已读</param>
        /// <param name="isQuiet">是否不带返回提示</param>
        /// <returns>结果</returns>
        [HttpPatch, HttpPost]
        [Route("Batch/IsRead")]
        public async Task<object> ReadAsync([FromBody] IEnumerable<Guid> ids, [FromQuery] bool isQuiet = false)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.ReadAsync(ids);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success(isQuiet ? "操作成功" : "");
                default: return msg.Fail(isQuiet ? "操作失败" : "");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id，为空时将全部删除</param>
        /// <returns>结果</returns>
        [HttpPatch, HttpPost]
        [Route("Batch/IsDeleted")]
        public async Task<BaseMessage> DeleteAsync([FromBody] IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.DeleteAsync(ids);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("操作成功");
                default: return msg.Fail("操作失败");
            }
        }
    }
}