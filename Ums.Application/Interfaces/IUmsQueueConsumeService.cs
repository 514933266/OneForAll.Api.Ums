using RabbitMQ.Client;
using System.Threading.Tasks;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 消息队列消费（供 Host 托管的消费者调用，领域层不感知连接生命周期）
    /// </summary>
    public interface IUmsQueueConsumeService
    {
        /// <summary>
        /// 消费系统通知消息
        /// </summary>
        Task ReceiveSystemAsync(IChannel channel);

        /// <summary>
        /// 消费钉钉文本消息
        /// </summary>
        Task ReceiveDingTalkTextAsync(IChannel channel);

        /// <summary>
        /// 消费钉钉Markdown消息
        /// </summary>
        Task ReceiveDingTalkMarkdownAsync(IChannel channel);

        /// <summary>
        /// 消费邮件消息
        /// </summary>
        Task ReceiveEmailAsync(IChannel channel);

        /// <summary>
        /// 消费腾讯云短信消息
        /// </summary>
        Task ReceiveTxCloudSmsAsync(IChannel channel);

        /// <summary>
        /// 消费公众号模板消息
        /// </summary>
        Task ReceiveWxgzhTemplateAsync(IChannel channel);

        /// <summary>
        /// 消费公众号订阅消息
        /// </summary>
        Task ReceiveWxgzhSubscribeAsync(IChannel channel);

        /// <summary>
        /// 消费小程序订阅消息
        /// </summary>
        Task ReceiveWxmpSubscribeTemplateAsync(IChannel channel);

        /// <summary>
        /// 消费企业微信文本消息
        /// </summary>
        Task ReceiveWxqyTextAsync(IChannel channel);

        /// <summary>
        /// 消费企业微信Markdown消息
        /// </summary>
        Task ReceiveWxqyMarkdownAsync(IChannel channel);
    }
}
