using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Public.Models;
using Ums.Domain.Models;
using Ums.Application.Interfaces;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 企业微信机器人
    /// </summary>
    [Route("api/[controller]")]
    public class WechatQyRobotController : BaseController
    {
        private readonly IWechatQyRobotService _service;
        public WechatQyRobotController(IWechatQyRobotService service)
        {
            _service = service;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Text")]
        public async Task<BaseMessage> SendTextAsync([FromBody] WechatQyRobotTextForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.SendTextAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Markdown")]
        public async Task<BaseMessage> SendMarkdownAsync([FromBody] WechatQyRobotTextForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.SendMarkdownAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
