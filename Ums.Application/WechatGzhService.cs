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
    public class WechatGzhService : IWechatGzhService
    {
        private readonly IMapper _mapper;
        private readonly IWechatGzhManager _manager;
        public WechatGzhService(
            IMapper mapper,
            IWechatGzhManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WechatGzhTemplateForm form)
        {
            return await _manager.SendTemplateAsync(form);
        }
    }
}
