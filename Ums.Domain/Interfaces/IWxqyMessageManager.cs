using OneForAll.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 企业微信
    /// </summary>
    public interface IWxqyMessageManager
    {
        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTextAsync(WxqyRobotMessageForm form);

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendMarkdownAsync(WxqyRobotMessageForm form);

        /// <summary>
        /// 接收Text消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveTextAsync(IChannel channel);

        /// <summary>
        /// 接收Markdown消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveMarkdownAsync(IChannel channel);
    }
}
