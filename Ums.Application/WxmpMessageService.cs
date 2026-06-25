using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;

namespace Ums.Application
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpMessageService : IWxmpMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWxmpMessageManager _manager;
        private readonly IWxmpDirectMessageManager _directManager;
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;
        
        public WxmpMessageService(
            IMapper mapper,
            IWxmpMessageManager manager,
            IWxmpDirectMessageManager directManager,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeTemplateAsync(WxmpSubscribeTemplateMessageForm form)
        {
            // 1. 检查去重（使用TemplateId + ToUser作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.ToUser}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Data?.ToString(), UmsMessageTypeEnum.WxgzhSubscribe);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _manager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _manager.SendSubscribeTemplateAsync(form);
            }
        }

        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeTemplateDirectAsync(WxmpSubscribeTemplateMessageForm form)
        {
            // 1. 检查去重（使用TemplateId + ToUser作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.ToUser}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Data?.ToString(), UmsMessageTypeEnum.Default);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _directManager.RecordAsync(form);
            }
            else
            {
                // 2. 直接发送消息
                return await _directManager.SendSubscribeTemplateDirectAsync(form);
            }
        }
    }
}

