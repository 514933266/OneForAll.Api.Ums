using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Application;
using Ums.Application.Interfaces;
using Ums.Domain.Models;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    [Route("api/[controller]")]
    public class WxmpMessageController : BaseController
    {
        private readonly IWxmpMessageService _service;
        public WxmpMessageController(IWxmpMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 发送模板消息（isSync=true时同步发送，不经过MQ）
        /// </summary>
        /// <param name="form">实体</param>
        /// <param name="isSync">是否同步发送</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Subscribe")]
        public async Task<BaseMessage> SendSubscribeAsync([FromBody] WxmpSubscribeTemplateMessageForm form, [FromQuery] bool isSync = false)
        {
            var msg = new BaseMessage();
            msg.ErrType = isSync
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
