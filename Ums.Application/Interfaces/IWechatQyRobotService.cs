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
    /// 企业微信机器人
    /// </summary>
    public interface IWechatQyRobotService
    {
        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendTextAsync(WechatQyRobotTextForm form);

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseErrType> SendMarkdownAsync(WechatQyRobotTextForm form);
    }
}
