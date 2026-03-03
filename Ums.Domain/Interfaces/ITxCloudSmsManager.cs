using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Models;
using Ums.Public.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 腾讯云-短信发送
    /// </summary>
    public interface ITxCloudSmsManager
    {
        /// <summary>
        /// 发送短信消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendAsync(TxCloudSmsForm form);

        /// <summary>
        /// 接收短信消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveAsync(IChannel channel);
    }
}
