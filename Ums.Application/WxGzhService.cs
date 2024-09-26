using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.ValueObjects;

namespace Ums.Application
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    public class WxgzhService : IWxgzhService
    {
        private readonly IMapper _mapper;
        private readonly IWxgzhManager _manager;
        public WxgzhService(
            IMapper mapper,
            IWxgzhManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WxgzhTemplateForm form)
        {
            return await _manager.SendTemplateAsync(form);
        }

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeForm form)
        {
            return await _manager.SendSubscribeAsync(form);
        }
    }
}
