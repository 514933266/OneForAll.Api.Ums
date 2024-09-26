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
    /// 企业微信
    /// </summary>
    [Route("api/[controller]")]
    public class WxqyController : BaseController
    {
        private readonly IWxqyService _service;
        public WxqyController(IWxqyService service)
        {
            _service = service;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Robot/Text")]
        public async Task<BaseMessage> SendTextAsync([FromBody] WxqyRobotTextForm form)
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
        [Route("Robot/Markdown")]
        public async Task<BaseMessage> SendMarkdownAsync([FromBody] WxqyRobotTextForm form)
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
