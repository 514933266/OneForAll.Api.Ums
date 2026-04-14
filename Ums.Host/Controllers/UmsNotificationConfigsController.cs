using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using OneForAll.Core.OAuth;
using Ums.Application.Dtos;
using Ums.Application.Interfaces;
using Ums.Domain.Models;
using Ums.Host.Filters;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.Admin)]
    public class UmsNotificationConfigsController : BaseController
    {
        private readonly IUmsNotificationConfigService _service;

        public UmsNotificationConfigsController(IUmsNotificationConfigService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>分页</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<PageList<UmsNotificationConfigDto>> GetPageAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] string clientId = default)
        {
            return await _service.GetPageAsync(pageIndex, pageSize, clientId);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPost]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> AddAsync([FromBody] UmsNotificationConfigForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">实体id</param>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPut]
        [Route("{id}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> UpdateAsync(Guid id, [FromBody] UmsNotificationConfigForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.UpdateAsync(id, form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                case BaseErrType.DataNotFound: return msg.Fail("数据不存在");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        [HttpDelete]
        [Route("{id}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> DeleteAsync(Guid id)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.DeleteAsync(id);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                case BaseErrType.DataNotFound: return msg.Fail("数据不存在");
                default: return msg.Fail("删除失败");
            }
        }
    }
}
