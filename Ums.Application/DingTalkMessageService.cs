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
    /// 钉钉机器人
    /// </summary>
    public class DingTalkMessageService : IDingTalkMessageService
    {
        private readonly IMapper _mapper;
        private readonly IDingTalkMessageManager _manager;
        private readonly IDingTalkDirectMessageManager _directManager;
        private readonly IUmsNotificationConfigManager _configManager;

        public DingTalkMessageService(
            IMapper mapper,
            IDingTalkMessageManager manager,
            IDingTalkDirectMessageManager directManager,
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
        private async Task<(BaseErrType ErrType, List<DingTalkRobotTargetVo> Targets)> FillFromConfigAsync(DingTalkRobotMessageForm form)
        {
            if (!form.ClientId.IsNullOrEmpty() && !form.ConfigCode.IsNullOrEmpty())
            {
                var config = await _configManager.GetAsync(form.ClientId, form.ConfigCode, UmsMessageTypeEnum.DingTalkRobot);
                if (config == null)
                    return (BaseErrType.DataNotMatch, new List<DingTalkRobotTargetVo>());

                var targets = config.TargetJson.FromJson<List<DingTalkRobotTargetVo>>();
                if (targets == null || !targets.Any())
                    return (BaseErrType.DataError, new List<DingTalkRobotTargetVo>());

                return (BaseErrType.Success, targets);
            }

            return (BaseErrType.Success, new List<DingTalkRobotTargetVo>());
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(DingTalkRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) 
                return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    form.Sign = target.Sign;
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
        public async Task<BaseErrType> SendTextDirectAsync(DingTalkRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) 
                return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    form.Sign = target.Sign;
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
        public async Task<BaseErrType> SendMarkdownAsync(DingTalkRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) 
                return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    form.Sign = target.Sign;
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
        public async Task<BaseErrType> SendMarkdownDirectAsync(DingTalkRobotMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) 
                return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.WebhookUrl = target.WebhookUrl;
                    form.Sign = target.Sign;
                    await _directManager.SendMarkdownDirectAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _directManager.SendMarkdownDirectAsync(form);
        }
    }
}
