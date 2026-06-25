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
    /// 企业微信机器人
    /// </summary>
    public class WxqyMessageService : IWxqyMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWxqyMessageManager _manager;
        private readonly IWxqyDirectMessageManager _directManager;
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;

        public WxqyMessageService(
            IMapper mapper,
            IWxqyMessageManager manager,
            IWxqyDirectMessageManager directManager,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WxqyRobotMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.WebhookUrl, form.Content, UmsMessageTypeEnum.WxQtRoot);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _manager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _manager.SendTextAsync(form);
            }
        }

        /// <summary>
        /// 直接发送Text消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextDirectAsync(WxqyRobotMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.WebhookUrl, form.Content, UmsMessageTypeEnum.WxQtRoot);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _directManager.RecordAsync(form);
            }
            else
            {
                // 2. 直接发送消息
                return await _directManager.SendTextDirectAsync(form);
            }
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownAsync(WxqyRobotMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.WebhookUrl, form.Content, UmsMessageTypeEnum.WxQtRoot);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _manager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _manager.SendMarkdownAsync(form);
            }
        }

        /// <summary>
        /// 直接发送Markdown消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownDirectAsync(WxqyRobotMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.WebhookUrl, form.Content, UmsMessageTypeEnum.WxQtRoot);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _directManager.RecordAsync(form);
            }
            else
            {
                // 2. 直接发送消息
                return await _directManager.SendMarkdownDirectAsync(form);
            }
        }
    }
}
