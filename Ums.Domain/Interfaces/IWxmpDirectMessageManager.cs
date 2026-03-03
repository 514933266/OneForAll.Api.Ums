using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 微信小程序-直接发送
    /// </summary>
    public interface IWxmpDirectMessageManager
    {
        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeTemplateDirectAsync(WxmpSubscribeTemplateMessageForm form);
    }
}
