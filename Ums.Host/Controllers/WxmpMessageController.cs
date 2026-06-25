using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Application;
using Ums.Application.Interfaces;
using Ums.Domain.Models;
using Ums.Public.Models;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    [Route("api/[controller]")]
    public class WxmpMessageController : BaseController
    {
        private readonly IWxmpMessageService _service;
        private readonly RabbitMqConnectionConfig _rabbitMqConfig;
        public WxmpMessageController(IWxmpMessageService service, RabbitMqConnectionConfig rabbitMqConfig)
        {
            _service = service;
            _rabbitMqConfig = rabbitMqConfig;
        }

        /// <summary>
        /// 发送模板消息（根据RabbitMQ配置决定发送方式）
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Subscribe")]
        public async Task<BaseMessage> SendSubscribeAsync([FromBody] WxmpSubscribeTemplateMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
                ? await _service.SendSubscribeTemplateDirectAsync(form)
                : await _service.SendSubscribeTemplateAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
