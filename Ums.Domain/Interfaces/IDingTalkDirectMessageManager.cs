using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 钉钉机器人-直接发送
    /// </summary>
    public interface IDingTalkDirectMessageManager
    {
        /// <summary>
        /// 直接发送Text消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTextDirectAsync(DingTalkRobotMessageForm form);

        /// <summary>
        /// 直接发送Markdown消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendMarkdownDirectAsync(DingTalkRobotMessageForm form);
    }
}
