﻿using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.HttpService.Models;

namespace Ums.HttpService.Interfaces
{
    /// <summary>
    /// 企业微信
    /// </summary>
    public interface IWechatQyRobotHttpService
    {
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <returns></returns>
        Task<BaseMessage> SendTextAsync(WechatQyRobotTextRequest request, string url);

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <returns></returns>
        Task<BaseMessage> SendMarkdownAsync(WechatQyRobotMarkdownRequest request, string url);
    }
}
