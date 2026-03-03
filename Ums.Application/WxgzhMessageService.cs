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
    /// 微信公众号推送
    /// </summary>
    public class WxgzhMessageService : IWxgzhMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWxgzhMessageManager _manager;
        private readonly IWxgzhDirectMessageManager _directManager;
        public WxgzhMessageService(
            IMapper mapper,
            IWxgzhMessageManager manager,
            IWxgzhDirectMessageManager directManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WxgzhTemplateMessageForm form)
        {
            return await _manager.SendTemplateAsync(form);
        }

        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateDirectAsync(WxgzhTemplateMessageForm form)
        {
            return await _directManager.SendTemplateDirectAsync(form);
        }

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeMessageForm form)
        {
            return await _manager.SendSubscribeAsync(form);
        }

        /// <summary>
        /// 直接发送长期订阅消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeDirectAsync(WxgzhSubscribeMessageForm form)
        {
            return await _directManager.SendSubscribeDirectAsync(form);
        }
    }
}
