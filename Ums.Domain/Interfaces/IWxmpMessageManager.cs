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
    /// 微信小程序
    /// </summary>
    public interface IWxmpMessageManager
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeTemplateAsync(WxmpSubscribeTemplateMessageForm form);

        /// <summary>
        /// 接收模板消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveSubscribeTemplateAsync(IChannel channel);
    }
}
