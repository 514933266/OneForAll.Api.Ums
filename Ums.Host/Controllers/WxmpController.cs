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
    public class WxmpController : BaseController
    {
        private readonly IWxmpService _service;
        public WxmpController(IWxmpService service)
        {
            _service = service;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Subscribe")]
        public async Task<BaseMessage> SendSubscribeAsync([FromBody] WxmpSubscribeForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.SendSubscribeAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
