using OneForAll.Core;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 腾讯云短信-直接发送
    /// </summary>
    public interface ITxCloudSmsDirectManager
    {
        /// <summary>
        /// 直接发送短信消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendDirectAsync(TxCloudSmsForm form);
    }
}
