using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Models;
using Ums.Public.Models;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    [Route("api/[controller]")]
    public class WxgzhMessageController : BaseController
    {
        private readonly IWxgzhMessageService _service;
        private readonly RabbitMqConnectionConfig _rabbitMqConfig;
        public WxgzhMessageController(IWxgzhMessageService service, RabbitMqConnectionConfig rabbitMqConfig)
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
        [Route("Template")]
        public async Task<BaseMessage> SendTemplateAsync([FromBody] WxgzhTemplateMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
                ? await _service.SendTemplateDirectAsync(form)
                : await _service.SendTemplateAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }

        /// <summary>
        /// 发送长期订阅消息（根据RabbitMQ配置决定发送方式）
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Subscribe")]
        public async Task<BaseMessage> SendSubscribeAsync([FromBody] WxgzhSubscribeMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
                ? await _service.SendSubscribeDirectAsync(form)
                : await _service.SendSubscribeAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
