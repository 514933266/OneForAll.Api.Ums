using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 微信公众号订阅消息-直接发送
    /// </summary>
    public interface IWxgzhSubscribeDirectMessageManager : IUmsBaseManager
    {
        /// <summary>
        /// 直接发送长期订阅消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeDirectAsync(WxgzhSubscribeMessageForm form);
    }
}
