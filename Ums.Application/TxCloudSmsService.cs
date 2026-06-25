using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;

namespace Ums.Application
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    public class TxCloudSmsService : ITxCloudSmsService
    {
        private readonly IMapper _mapper;
        private readonly ITxCloudSmsManager _manager;
        private readonly ITxCloudSmsDirectManager _directManager;
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;
        
        public TxCloudSmsService(
            IMapper mapper,
            ITxCloudSmsManager manager,
            ITxCloudSmsDirectManager directManager,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(TxCloudSmsForm form)
        {
            // 1. 检查去重（使用TemplateId + PhoneNumber作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.PhoneNumber}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Content, UmsMessageTypeEnum.Default);

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
        /// 直接发送短信（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(TxCloudSmsForm form)
        {
            // 1. 检查去重（使用TemplateId + PhoneNumber作为去重key）
            var dedupKey = $"{form.TemplateId}_{form.PhoneNumber}";
            var isDuplicate = await _deduplicationManager.CheckAsync(dedupKey, form.Content, UmsMessageTypeEnum.Default);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _directManager.RecordAsync(form);
            }
            else
            {
                // 2. 直接发送消息
                return await _directManager.SendDirectAsync(form);
            }
        }
    }
}