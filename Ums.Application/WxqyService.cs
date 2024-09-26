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
    public class WxqyService : IWxqyService
    {
        private readonly IMapper _mapper;
        private readonly IWxqyManager _manager;
        public WxqyService(
            IMapper mapper,
            IWxqyManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WxqyRobotTextForm form)
        {
            return await _manager.SendTextAsync(form);
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownAsync(WxqyRobotTextForm form)
        {
            return await _manager.SendMarkdownAsync(form);
        }
    }
}
