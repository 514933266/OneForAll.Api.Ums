using AutoMapper;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.ValueObjects;

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
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;

        public UmsEmailMessageService(
            IMapper mapper,
            IUmsEmailMessageManager manager,
            IUmsEmailDirectMessageManager directManager,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 发送邮件消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(UmsEmailMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.Subject, form.Body, UmsMessageTypeEnum.Email);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _manager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _manager.SendAsync(form);
            }
        }

        /// <summary>
        /// 直接发送邮件消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(UmsEmailMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.Subject, form.Body, UmsMessageTypeEnum.Email);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _directManager.RecordAsync(form);
            }
            else
            {
                return await _directManager.SendDirectAsync(form);
            }
        }
    }
}
