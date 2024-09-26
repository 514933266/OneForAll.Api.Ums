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
    public class WxmpService : IWxmpService
    {
        private readonly IMapper _mapper;
        private readonly IWxmpManager _manager;
        public WxmpService(
            IMapper mapper,
            IWxmpManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxmpSubscribeForm form)
        {
            return await _manager.SendSubscribeAsync(form);
        }
    }
}

