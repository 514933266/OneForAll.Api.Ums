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
    /// 系统通知
    /// </summary>
    public interface IUmsMessageService
    {
        /// <summary>
        /// 发送系统通知消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSystemAsync(UmsMessageForm form);

        /// <summary>
        /// 直接发送系统通知消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSystemDirectAsync(UmsMessageForm form);
    }
}
