using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 企业微信-直接发送
    /// </summary>
    public interface IWxqyDirectMessageManager
    {
        /// <summary>
        /// 直接发送Text消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTextDirectAsync(WxqyRobotMessageForm form);

        /// <summary>
        /// 直接发送Markdown消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendMarkdownDirectAsync(WxqyRobotMessageForm form);
    }
}
