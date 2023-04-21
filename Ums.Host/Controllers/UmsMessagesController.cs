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
    /// 系统站内信
    /// </summary>
    [Route("api/[controller]")]
    //[Authorize(Roles = UserRoleType.RULER)]
    public class UmsMessagesController : BaseController
    {
        private readonly IUmsMessageService _service;
        public UmsMessagesController(IUmsMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] UmsMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.SendAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
