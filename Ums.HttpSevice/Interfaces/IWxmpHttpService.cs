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
    /// 微信小程序
    /// </summary>
    public interface IWxmpHttpService
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="accessToken">请求凭证</param>
        /// <returns></returns>
        Task<BaseMessage> SendTemplateAsync(WxmpSubscribeRequest request, string accessToken);
    }
}