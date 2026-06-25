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
    /// 微信公众号推送
    /// </summary>
    public class WxgzhMessageService : IWxgzhMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWxgzhMessageManager _manager;
        private readonly IWxgzhDirectMessageManager _directManager;
        private readonly IWxgzhSubscribeMessageManager _subscribeManager;
        private readonly IWxgzhSubscribeDirectMessageManager _subscribeDirectManager;
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;
        
        public WxgzhMessageService(
            IMapper mapper,
            IWxgzhMessageManager manager,
            IWxgzhDirectMessageManager directManager,
            IWxgzhSubscribeMessageManager subscribeManager,
            IWxgzhSubscribeDirectMessageManager subscribeDirectManager,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _subscribeManager = subscribeManager;
            _subscribeDirectManager = subscribeDirectManager;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WxgzhTemplateMessageForm form)
        {
            // 1. 检查去重（使用TemplateId + ToUser作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.ToUser}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Data?.ToString(), UmsMessageTypeEnum.WxgzhTemplate);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _manager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _manager.SendTemplateAsync(form);
            }
        }

        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateDirectAsync(WxgzhTemplateMessageForm form)
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
                return await _directManager.SendTemplateDirectAsync(form);
            }
        }

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeMessageForm form)
        {
            // 1. 检查去重（使用TemplateId + ToUser作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.ToUser}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Data?.ToString(), UmsMessageTypeEnum.Default);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _subscribeManager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _subscribeManager.SendSubscribeAsync(form);
            }
        }

        /// <summary>
        /// 直接发送长期订阅消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeDirectAsync(WxgzhSubscribeMessageForm form)
        {
            // 1. 检查去重（使用TemplateId + ToUser作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.ToUser}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Data?.ToString(), UmsMessageTypeEnum.Default);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _subscribeDirectManager.RecordAsync(form);
            }
            else
            {
                // 2. 直接发送消息
                return await _subscribeDirectManager.SendSubscribeDirectAsync(form);
            }
        }
    }
}
