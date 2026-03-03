using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.HttpService.Models;

namespace Ums.HttpService.Interfaces
{
    /// <summary>
    /// 钉钉
    /// </summary>
    public interface IDingTalkHttpService
    {
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        Task<BaseMessage> SendRobotTextAsync(DingTalkRobotTextMessageRequest request, string url, string secret);

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        Task<BaseMessage> SendRobotMarkdownAsync(DingTalkRobotMarkdownMessageRequest request, string url, string secret);
    }
}
