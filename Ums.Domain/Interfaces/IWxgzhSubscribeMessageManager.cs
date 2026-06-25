using OneForAll.Core;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 微信公众号订阅消息推送
    /// </summary>
    public interface IWxgzhSubscribeMessageManager : IUmsBaseManager
    {
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
