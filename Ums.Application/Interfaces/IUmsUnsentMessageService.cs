using System.Threading.Tasks;
using Ums.Application.Dtos;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 未发送消息重发
    /// </summary>
    public interface IUmsUnsentMessageService
    {
        /// <summary>
        /// 重新投递当天未发送（超过一小时）的队列消息
        /// </summary>
        /// <returns>重发结果</returns>
        Task<UnsentMessageResendDto> ResendExpiredAsync();
    }
}
