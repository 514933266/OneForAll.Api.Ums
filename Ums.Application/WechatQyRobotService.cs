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
    public class WechatQyRobotService : IWechatQyRobotService
    {
        private readonly IMapper _mapper;
        private readonly IWechatQyRootManager _manager;
        public WechatQyRobotService(
            IMapper mapper,
            IWechatQyRootManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WechatQyRobotTextForm form)
        {
            return await _manager.SendTextAsync(form);
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownAsync(WechatQyRobotTextForm form)
        {
            return await _manager.SendMarkdownAsync(form);
        }
    }
}
