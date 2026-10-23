using AutoMapper;
using OneForAll.Core.OAuth;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;
using OneForAll.Core.Extension;

namespace Ums.Domain.Managers
{
    /// <summary>
    /// 个人消息
    /// </summary>
    public class UmsPersonalMessageManager : UmsDomainManager, IUmsPersonalMessageManager
    {
        private readonly IUmsMessageRepository _repository;

        public UmsPersonalMessageManager(
            IMapper mapper,
            ICurrentUser currentUser,
            IUmsMessageRepository repository) : base(currentUser)
        {
            _repository = repository;
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids)
        {
            var data = await _repository.GetListByIdAsync(LoginUser.Id.TryGuid(), ids);
            data.ForEach(e =>
            {
                e.IsRead = true;
            });
            return await ResultAsync(_repository.SaveChangesAsync);
        }

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAllAsync()
        {
            // 后续数据量大，可移除该功能/使用分页/消息队列异步处理
            var data = await _repository.GetAllByAccountAsync(LoginUser.Id.TryGuid());
            data.ForEach(e =>
            {
                e.IsRead = true;
            });
            return await ResultAsync(_repository.SaveChangesAsync);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var effected = 0;
            var data = await _repository.GetListByIdAsync(LoginUser.Id.TryGuid(), ids);
            if (data.Any())
            {
                effected = await _repository.DeleteRangeAsync(data);
            }
            return Result(() => effected);
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync()
        {
            // 后续数据量大，可移除该功能/使用分页/消息队列异步处理
            var effected = 0;
            var data = await _repository.GetAllByAccountAsync(LoginUser.Id.TryGuid());
            if (data.Any())
            {
                effected = await _repository.DeleteRangeAsync(data);
            }
            return Result(() => effected);
        }
    }
}
