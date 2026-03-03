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
    /// 邮件消息
    /// </summary>
    public interface IUmsEmailMessageService
    {
        /// <summary>
        /// 发送邮件消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendAsync(UmsEmailMessageForm form);

        /// <summary>
        /// 直接发送邮件消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendDirectAsync(UmsEmailMessageForm form);
    }
}
