using OneForAll.Core;
using OneForAll.Core.Extension;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Managers;
using Ums.Domain.Repositorys;

namespace Ums.Domain.Querys
{
    /// <summary>
    /// 个人消息查询服务
    /// </summary>
    public class UmsPersonalMessageQueryService : UmsDomainManager, IUmsPersonalMessageQueryService
    {
        private readonly IUmsMessageRepository _repository;

        public UmsPersonalMessageQueryService(
            ICurrentUser currentUser,
            IUmsMessageRepository repository) : base(currentUser)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <param name="top">前几条</param>
        /// <returns>分页列表</returns>
        public async Task<IEnumerable<UmsMessage>> GetListAsync(int top)
        {
            if (top > 100) top = 100;
            return await _repository.GetListAsync(LoginUser.Id.TryGuid(), top);
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="day">近几天</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<UmsMessage>> GetListByDayAsync(int day)
        {
            if (day > 30) day = 30;
            return await _repository.GetListByDayAsync(LoginUser.Id.TryGuid(), day);
        }

        /// <summary>
        /// 获取消息分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsMessage>> GetPageAsync(int pageIndex, int pageSize, string key, UmsMessageReadStatusEnum status)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            return await _repository.GetPageAsync(LoginUser.Id.TryGuid(), pageIndex, pageSize, key, status);
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> GetUnReadCountAsync()
        {
            return await _repository.GetUnReadCountAsync(LoginUser.Id.TryGuid());
        }
    }
}
