using AutoMapper;
using OneForAll.Core;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;

namespace Ums.Domain.Managers
{
    /// <summary>
    /// 消息通知配置
    /// </summary>
    public class UmsNotificationConfigManager : UmsDomainManager, IUmsNotificationConfigManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsNotificationConfigRepository _repository;

        public UmsNotificationConfigManager(
            IMapper mapper,
            ICurrentUser currentUser,
            IUmsNotificationConfigRepository repository) : base(currentUser)
        {
            _mapper = mapper;
            _repository = repository;
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
