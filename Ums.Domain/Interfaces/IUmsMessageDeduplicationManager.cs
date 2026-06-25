using System;
using System.Threading.Tasks;
using Ums.Domain.Enums;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 消息去重管理器
    /// </summary>
    public interface IUmsMessageDeduplicationManager
    {
        /// <summary>
        /// 检查并处理消息去重
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>true=需要去重(不发送), false=不需要去重(可以发送)</returns>
        Task<bool> CheckAsync(string title, string content, UmsMessageTypeEnum messageType);

        /// <summary>
        /// 生成去重key
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="matchRule">匹配规则</param>
        /// <returns>去重key</returns>
        string GenerateHashKey(string title, string content, UmsMessageTypeEnum messageType, DeduplicationMatchRuleEnum matchRule);
    }
}
