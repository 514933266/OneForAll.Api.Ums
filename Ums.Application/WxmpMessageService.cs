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
    /// 微信小程序
    /// </summary>
    public class WxmpMessageService : IWxmpMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWxmpMessageManager _manager;
        private readonly IWxmpDirectMessageManager _directManager;
        public WxmpMessageService(
            IMapper mapper,
            IWxmpMessageManager manager,
            IWxmpDirectMessageManager directManager)
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
        public async Task<BaseErrType> SendSubscribeTemplateAsync(WxmpSubscribeTemplateMessageForm form)
        {
            return await _manager.SendSubscribeTemplateAsync(form);
        }

        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeTemplateDirectAsync(WxmpSubscribeTemplateMessageForm form)
        {
            return await _directManager.SendSubscribeTemplateDirectAsync(form);
        }
    }
}

