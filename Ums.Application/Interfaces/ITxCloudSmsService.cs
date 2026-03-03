using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    public interface ITxCloudSmsService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendAsync(TxCloudSmsForm form);

        /// <summary>
        /// 直接发送短信（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendDirectAsync(TxCloudSmsForm form);
    }
}
