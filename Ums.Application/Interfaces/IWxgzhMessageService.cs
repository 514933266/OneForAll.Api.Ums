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
    /// 微信公众号推送
    /// </summary>
    public interface IWxgzhMessageService
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTemplateAsync(WxgzhTemplateMessageForm form);

        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTemplateDirectAsync(WxgzhTemplateMessageForm form);

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeMessageForm form);

        /// <summary>
        /// 直接发送长期订阅消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendSubscribeDirectAsync(WxgzhSubscribeMessageForm form);
    }
}
