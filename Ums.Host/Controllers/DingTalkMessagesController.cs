using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Domain.Models;
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
        public DingTalkMessagesController(IDingTalkMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 发送Text消息（isSync=true时同步发送，不经过MQ）
        /// </summary>
        /// <param name="form">实体</param>
        /// <param name="isSync">是否同步发送</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Robot/Text")]
        public async Task<BaseMessage> SendTextAsync([FromBody] DingTalkRobotMessageForm form, [FromQuery] bool isSync = false)
        {
            var msg = new BaseMessage();
            msg.ErrType = isSync
                ? await _service.SendTextDirectAsync(form)
                : await _service.SendTextAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }

        /// <summary>
        /// 发送Markdown消息（isSync=true时同步发送，不经过MQ）
        /// </summary>
        /// <param name="form">实体</param>
        /// <param name="isSync">是否同步发送</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Robot/Markdown")]
        public async Task<BaseMessage> SendMarkdownAsync([FromBody] DingTalkRobotMessageForm form, [FromQuery] bool isSync = false)
        {
            var msg = new BaseMessage();
            msg.ErrType = isSync
                ? await _service.SendMarkdownDirectAsync(form)
                : await _service.SendMarkdownAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
