using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Application.Interfaces;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;

namespace Ums.Application
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    public class UmsSmsRecordService : IUmsSmsRecordService
    {
        private readonly IMapper _mapper;
        private readonly IUmsSmsRecordManager _manager;
        public UmsSmsRecordService(
            IMapper mapper,
            IUmsSmsRecordManager manager)
        {
            _mapper = mapper;
            _manager = manager;
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
		public async Task<PageList<UmsSmsRecordDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            DateTime? startTime,
            DateTime? endTime,
            string platformName)
        {
            var data = await _manager.GetPgaeAsync(pageIndex, pageSize, startTime, endTime, platformName);
            var items = _mapper.Map<IEnumerable<UmsSmsRecord>, IEnumerable<UmsSmsRecordDto>>(data.Items);
            return new PageList<UmsSmsRecordDto>(data.Total, data.PageIndex, data.PageSize, items);
        }
    }
}

