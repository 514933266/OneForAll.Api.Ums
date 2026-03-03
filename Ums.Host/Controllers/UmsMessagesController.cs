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

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 系统通知
    /// </summary>
    [Route("api/[controller]")]
    public class UmsMessagesController : BaseController
    {
        private readonly IUmsMessageService _service;
        public UmsMessagesController(IUmsMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 添加（isSync=true时同步发送，不经过MQ）
        /// </summary>
        /// <param name="form">实体</param>
        /// <param name="isSync">是否同步发送</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] UmsMessageForm form, [FromQuery] bool isSync = false)
        {
            var msg = new BaseMessage();
            msg.ErrType = isSync
                ? await _service.SendSystemDirectAsync(form)
                : await _service.SendSystemAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
