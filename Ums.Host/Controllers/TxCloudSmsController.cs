using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Domain.Models;
using Ums.Public.Models;
using Ums.Application.Interfaces;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    [Route("api/[controller]")]
    public class TxCloudSmsController : BaseController
    {
        private readonly ITxCloudSmsService _service;
        private readonly RabbitMqConnectionConfig _rabbitMqConfig;
        public TxCloudSmsController(ITxCloudSmsService service, RabbitMqConnectionConfig rabbitMqConfig)
        {
            _service = service;
            _rabbitMqConfig = rabbitMqConfig;
        }

        /// <summary>
        /// 发送短信（根据RabbitMQ配置决定发送方式）
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> SendAsync([FromBody] TxCloudSmsForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
                ? await _service.SendDirectAsync(form)
                : await _service.SendAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                case BaseErrType.TokenInvalid: return msg.Fail("签名错误");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
