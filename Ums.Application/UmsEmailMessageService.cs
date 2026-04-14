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
        private readonly IUmsNotificationConfigManager _configManager;

        public UmsEmailMessageService(
            IMapper mapper,
            IUmsEmailMessageManager manager,
            IUmsEmailDirectMessageManager directManager,
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
        private async Task<(BaseErrType ErrType, List<EmailTargetVo> Targets)> FillFromConfigAsync(UmsEmailMessageForm form)
        {
            if (form.ClientId.IsNullOrEmpty() || form.ConfigCode.IsNullOrEmpty()) return (BaseErrType.Success, new List<EmailTargetVo>());

            var config = await _configManager.GetAsync(form.ClientId, form.ConfigCode, UmsMessageTypeEnum.Email);
            if (config == null) return (BaseErrType.DataNotFound, new List<EmailTargetVo>());

            var targets = config.TargetJson.FromJson<List<EmailTargetVo>>();
            if (targets == null || !targets.Any()) return (BaseErrType.DataError, new List<EmailTargetVo>());

            return (BaseErrType.Success, targets);
        }

        /// <summary>
        /// 发送邮件消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(UmsEmailMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.To = target.To;
                    form.Cc = target.Cc;
                    await _manager.SendAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _manager.SendAsync(form);
        }

        /// <summary>
        /// 直接发送邮件消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(UmsEmailMessageForm form)
        {
            var (errType, targets) = await FillFromConfigAsync(form);
            if (errType != BaseErrType.Success) return errType;

            if (targets.Any())
            {
                foreach (var target in targets)
                {
                    form.To = target.To;
                    form.Cc = target.Cc;
                    await _directManager.SendDirectAsync(form);
                }
                return BaseErrType.Success;
            }

            return await _directManager.SendDirectAsync(form);
        }
    }
}
