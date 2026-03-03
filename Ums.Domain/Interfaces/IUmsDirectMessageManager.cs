using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 站内信-直接发送
    /// </summary>
    public interface IUmsDirectMessageManager
    {
        /// <summary>
        /// 直接发送系统通知消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSystemDirectAsync(UmsMessageForm form);
    }
}
