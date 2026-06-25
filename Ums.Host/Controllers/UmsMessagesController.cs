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
        private readonly RabbitMqConnectionConfig _rabbitMqConfig;
        public UmsMessagesController(IUmsMessageService service, RabbitMqConnectionConfig rabbitMqConfig)
        {
            _service = service;
            _rabbitMqConfig = rabbitMqConfig;
        }

        /// <summary>
        /// 添加（根据RabbitMQ配置决定发送方式）
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] UmsMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
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
