using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;

namespace Ums.Application
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class UmsMessageService : IUmsMessageService
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageManager _manager;
        public UmsMessageService(
            IMapper mapper,
            IUmsMessageManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送系统通知消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemAsync(UmsMessageForm form)
        {
            return await _manager.SendSystemAsync(form);
        }
    }
}
