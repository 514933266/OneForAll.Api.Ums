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
    /// 企业微信机器人
    /// </summary>
    public class WxqyMessageService : IWxqyMessageService
    {
        private readonly IMapper _mapper;
        private readonly IWxqyMessageManager _manager;
        private readonly IWxqyDirectMessageManager _directManager;
        public WxqyMessageService(
            IMapper mapper,
            IWxqyMessageManager manager,
            IWxqyDirectMessageManager directManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WxqyRobotMessageForm form)
        {
            return await _manager.SendTextAsync(form);
        }

        /// <summary>
        /// 直接发送Text消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextDirectAsync(WxqyRobotMessageForm form)
        {
            return await _directManager.SendTextDirectAsync(form);
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownAsync(WxqyRobotMessageForm form)
        {
            return await _manager.SendMarkdownAsync(form);
        }

        /// <summary>
        /// 直接发送Markdown消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownDirectAsync(WxqyRobotMessageForm form)
        {
            return await _directManager.SendMarkdownDirectAsync(form);
        }
    }
}
