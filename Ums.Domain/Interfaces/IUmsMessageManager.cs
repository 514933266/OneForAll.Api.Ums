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
    /// 站内信
    /// </summary>
    public interface IUmsMessageManager
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendAsync(UmsMessageForm form);

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveAsync(IModel channel);
    }
}
