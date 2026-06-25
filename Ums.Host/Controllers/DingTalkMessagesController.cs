using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using Ums.Domain.Models;
using Ums.Public.Models;
using Ums.Application.Interfaces;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 钉钉机器人
    /// </summary>
    [Route("api/[controller]")]
    public class DingTalkMessagesController : BaseController
    {
        private readonly IDingTalkMessageService _service;
        private readonly RabbitMqConnectionConfig _rabbitMqConfig;
        public DingTalkMessagesController(IDingTalkMessageService service, RabbitMqConnectionConfig rabbitMqConfig)
        {
            _service = service;
            _rabbitMqConfig = rabbitMqConfig;
        }

        /// <summary>
        /// 发送Text消息（根据RabbitMQ配置决定发送方式）
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Robot/Text")]
        public async Task<BaseMessage> SendTextAsync([FromBody] DingTalkRobotMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
                ? await _service.SendTextDirectAsync(form)
                : await _service.SendTextAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success:
                    return msg.Success("发送成功");
                case BaseErrType.DataNotMatch:
                    return msg.Success("未查询对应配置");
                case BaseErrType.DataError:
                    return msg.Success("配置异常");
                default:
                    return msg.Fail("发送失败");
            }
        }

        /// <summary>
        /// 发送Markdown消息（根据RabbitMQ配置决定发送方式）
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Robot/Markdown")]
        public async Task<BaseMessage> SendMarkdownAsync([FromBody] DingTalkRobotMessageForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = !_rabbitMqConfig.IsEnabled
                ? await _service.SendMarkdownDirectAsync(form)
                : await _service.SendMarkdownAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success:
                    return msg.Success("发送成功");
                case BaseErrType.DataNotMatch:
                    return msg.Success("未查询对应配置");
                case BaseErrType.DataError:
                    return msg.Success("配置异常");
                default:
                    return msg.Fail("发送失败");
            }
        }
    }
}
