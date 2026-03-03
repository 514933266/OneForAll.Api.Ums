using OneForAll.Core;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    public interface IWxgzhMessageManager
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTemplateAsync(WxgzhTemplateMessageForm form);

        /// <summary>
        /// 接收模板消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveTemplateAsync(IChannel channel);

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeMessageForm form);

        /// <summary>
        /// 接收长期订阅消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        Task ReceiveSubscribeAsync(IChannel channel);
    }
}
