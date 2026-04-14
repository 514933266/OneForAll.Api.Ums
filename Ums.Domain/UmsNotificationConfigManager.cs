using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;

namespace Ums.Domain
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public class UmsNotificationConfigManager : BaseManager, IUmsNotificationConfigManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsNotificationConfigRepository _repository;

        public UmsNotificationConfigManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsNotificationConfigRepository repository) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// 根据客户端id、代码和通知类型查询配置
        /// </summary>
        /// <param name="clientId">客户端id</param>
        /// <param name="code">配置代码</param>
        /// <param name="type">通知类型</param>
        /// <returns>配置实体</returns>
        public async Task<UmsNotificationConfig> GetAsync(string clientId, string code, UmsMessageTypeEnum type)
        {
            return await _repository.GetAsync(clientId, code, type);
        }

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="clientId">客户端id</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsNotificationConfig>> GetPageAsync(int pageIndex, int pageSize, string clientId)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            return await _repository.GetPageAsync(pageIndex, pageSize, clientId);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(UmsNotificationConfigForm form)
        {
            var data = _mapper.Map<UmsNotificationConfigForm, UmsNotificationConfig>(form);
            return await ResultAsync(() => _repository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">实体id</param>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(Guid id, UmsNotificationConfigForm form)
        {
            var data = await _repository.FindAsync(id);
            if (data == null)
                return BaseErrType.DataNotFound;

            _mapper.Map(form, data);
            return await ResultAsync(() => _repository.UpdateAsync(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            var data = await _repository.FindAsync(id);
            if (data == null)
                return BaseErrType.DataNotFound;

            return await ResultAsync(() => _repository.DeleteAsync(data));
        }
    }
}
