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
    /// 邮件消息
    /// </summary>
    public interface IUmsEmailMessageManager
    {
        /// <summary>
        /// 发送邮件消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendAsync(UmsEmailMessageForm form);

        /// <summary>
        /// 接收邮件消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveAsync(IChannel channel);
    }
}
