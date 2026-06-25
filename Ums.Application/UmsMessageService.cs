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
        private readonly IUmsMessageDirectManager _directManager;
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;
        public UmsMessageService(
            IMapper mapper,
            IUmsMessageManager manager,
            IUmsMessageDirectManager directManager,
            IUmsMessageDeduplicationManager deduplicationManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
            _deduplicationManager = deduplicationManager;
        }

        /// <summary>
        /// 发送系统通知消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemAsync(UmsMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.Title, form.Content, form.Type);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _manager.RecordAsync(form);
            }
            else
            {
                // 2. 通过MQ发送消息
                return await _manager.SendSystemAsync(form);
            }
        }

        /// <summary>
        /// 直接发送系统通知消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemDirectAsync(UmsMessageForm form)
        {
            // 1. 检查去重
            var isDuplicate = await _deduplicationManager.CheckAsync(form.Title, form.Content, form.Type);

            if (isDuplicate)
            {
                // 1. 仅记录消息（不发送）
                return await _directManager.RecordAsync(form);
            }
            else
            {
                // 2. 直接发送消息
                return await _directManager.SendSystemDirectAsync(form);
            }
        }
    }
}
