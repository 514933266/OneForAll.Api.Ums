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
    /// 邮件消息
    /// </summary>
    public class UmsEmailMessageService : IUmsEmailMessageService
    {
        private readonly IMapper _mapper;
        private readonly IUmsEmailMessageManager _manager;
        private readonly IUmsEmailDirectMessageManager _directManager;
        public UmsEmailMessageService(
            IMapper mapper,
            IUmsEmailMessageManager manager,
            IUmsEmailDirectMessageManager directManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
        }

        /// <summary>
        /// 发送邮件消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(UmsEmailMessageForm form)
        {
            return await _manager.SendAsync(form);
        }

        /// <summary>
        /// 直接发送邮件消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(UmsEmailMessageForm form)
        {
            return await _directManager.SendDirectAsync(form);
        }
    }
}
