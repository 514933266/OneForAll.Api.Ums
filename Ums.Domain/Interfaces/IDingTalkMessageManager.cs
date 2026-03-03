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
    /// 钉钉机器人
    /// </summary>
    public interface IDingTalkMessageManager
    {
        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTextAsync(DingTalkRobotMessageForm form);

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendMarkdownAsync(DingTalkRobotMessageForm form);

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
