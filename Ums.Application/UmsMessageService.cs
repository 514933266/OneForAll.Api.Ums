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
        private readonly IUmsDirectMessageManager _directManager;
        public UmsMessageService(
            IMapper mapper,
            IUmsMessageManager manager,
            IUmsDirectMessageManager directManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
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

        /// <summary>
        /// 直接发送系统通知消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemDirectAsync(UmsMessageForm form)
        {
            return await _directManager.SendSystemDirectAsync(form);
        }
    }
}
