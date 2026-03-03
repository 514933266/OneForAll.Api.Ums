using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Domain.Models;
using OneForAll.Core.OAuth;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 站内信
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.Admin)]
    public class UmsTenantMessagesController : BaseController
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] UmsMessageForm form)
        {
            var msg = new BaseMessage();

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("发送成功");
                default: return msg.Fail("发送失败");
            }
        }
    }
}
