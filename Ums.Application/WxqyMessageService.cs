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
        private readonly IUmsNotificationConfigManager _configManager;

        public WxqyMessageService(
            IMapper mapper,
            IWxqyMessageManager manager,
            IWxqyDirectMessageManager directManager,
            IUmsNotificationConfigManager configManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _configManager = configManager;
        }

        /// <summary>
        /// 根据通知配置获取目标列表
        /// </summary>
        private async Task<(BaseErrType ErrType, List<WxQtRobotTargetVo> Targets)> FillFromConfigAsync(WxqyRobotMessageForm form)
        {
            if (!form.WebhookUrl.IsNullOrEmpty()) return (BaseErrType.Success, new List<WxQtRobotTargetVo>());
            if (form.ClientId.IsNullOrEmpty() || form.ConfigCode.IsNullOrEmpty()) return (BaseErrType.DataError, new List<WxQtRobotTargetVo>());

            var config = await _configManager.GetAsync(form.ClientId, form.ConfigCode, UmsMessageTypeEnum.WxQtRoot);
            if (config == null) return (BaseErrType.DataNotFound, new List<WxQtRobotTargetVo>());

            var targets = config.TargetJson.FromJson<List<WxQtRobotTargetVo>>();
            if (targets == null || !targets.Any()) return (BaseErrType.DataError, new List<WxQtRobotTargetVo>());

            return (BaseErrType.Success, targets);
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WxqyRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    await _manager.SendTextAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _manager.SendTextAsync(form);
        }

        /// <summary>
        /// 直接发送Text消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextDirectAsync(WxqyRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    await _directManager.SendTextDirectAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _directManager.SendTextDirectAsync(form);
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownAsync(WxqyRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    await _manager.SendMarkdownAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _manager.SendMarkdownAsync(form);
        }

        /// <summary>
        /// 直接发送Markdown消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownDirectAsync(WxqyRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    await _directManager.SendMarkdownDirectAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _directManager.SendMarkdownDirectAsync(form);
        }
    }
}
