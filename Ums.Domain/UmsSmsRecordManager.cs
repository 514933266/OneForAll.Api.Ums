using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;

namespace Ums.Domain
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    public class UmsSmsRecordManager : UmsBaseManager, IUmsSmsRecordManager
    {
        private readonly IUmsSmsRecordRepository _repository;

        public UmsSmsRecordManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsSmsRecordRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
		/// <param name="pageSize">页数</param>
		/// <param name="startTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <param name="platformName">平台名称</param>
        ///  <returns>分页</returns>
        public async Task<PageList<UmsSmsRecord>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string platformName)
        {
            return await _repository.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, platformName);
        }
    }
}

