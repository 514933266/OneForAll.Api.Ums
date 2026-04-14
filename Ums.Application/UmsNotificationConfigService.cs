using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Application.Interfaces;
using Ums.Domain.Entities;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;

namespace Ums.Application
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public class UmsNotificationConfigService : IUmsNotificationConfigService
    {
        private readonly IMapper _mapper;
        private readonly IUmsNotificationConfigManager _manager;

        public UmsNotificationConfigService(
            IMapper mapper,
            IUmsNotificationConfigManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsNotificationConfigDto>> GetPageAsync(int pageIndex, int pageSize, string clientId)
        {
            var data = await _manager.GetPageAsync(pageIndex, pageSize, clientId);
            var items = _mapper.Map<IEnumerable<UmsNotificationConfig>, IEnumerable<UmsNotificationConfigDto>>(data.Items);
            return new PageList<UmsNotificationConfigDto>(data.Total, data.PageIndex, data.PageSize, items);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(UmsNotificationConfigForm form)
        {
            return await _manager.AddAsync(form);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">实体id</param>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(Guid id, UmsNotificationConfigForm form)
        {
            return await _manager.UpdateAsync(id, form);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            return await _manager.DeleteAsync(id);
        }
    }
}
