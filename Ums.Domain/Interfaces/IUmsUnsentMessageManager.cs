using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 未发送消息重发
    /// </summary>
    public interface IUmsUnsentMessageManager
    {
        /// <summary>
        /// 重新投递当天未发送（超过一小时）的队列消息
        /// </summary>
        /// <returns>重发结果</returns>
        Task<UnsentMessageResendResult> ResendExpiredAsync();
    }
}
