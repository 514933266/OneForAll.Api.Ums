using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 邮件消息-直接发送
    /// </summary>
    public interface IUmsEmailDirectMessageManager
    {
        /// <summary>
        /// 直接发送邮件消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendDirectAsync(UmsEmailMessageForm form);
    }
}
