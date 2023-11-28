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
    public class WechatGzhController : BaseController
    {
        private readonly IWechatGzhService _service;
        public WechatGzhController(IWechatGzhService service)
        {
            _service = service;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("Template")]
        public async Task<BaseMessage> SendTemplateAsync([FromBody] WechatGzhTemplateForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.SendTemplateAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
