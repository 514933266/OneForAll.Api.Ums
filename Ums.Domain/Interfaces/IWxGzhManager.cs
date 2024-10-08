﻿using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Models;
using Ums.Domain.ValueObjects;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    public interface IWxgzhManager
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTemplateAsync(WxgzhTemplateForm form);

        /// <summary>
        /// 接收模板消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveTemplateAsync(IModel channel);

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeForm form);

        /// <summary>
        /// 接收长期订阅消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveSubscribeAsync(IModel channel);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="msg">消息json</param>
        /// <returns></returns>
        Task<BaseErrType> SendAsync(string queueName, string msg);
    }
}
