using RabbitMQ.Client;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Interfaces;

namespace Ums.Application
{
    /// <summary>
    /// 消息队列消费
    /// </summary>
    public class UmsQueueConsumeService : IUmsQueueConsumeService
    {
        private readonly IUmsMessageManager _messageManager;
        private readonly IDingTalkMessageManager _dingTalkManager;
        private readonly IUmsEmailMessageManager _emailManager;
        private readonly ITxCloudSmsManager _smsManager;
        private readonly IWxgzhMessageManager _wxgzhManager;
        private readonly IWxgzhSubscribeMessageManager _wxgzhSubscribeManager;
        private readonly IWxmpMessageManager _wxmpManager;
        private readonly IWxqyMessageManager _wxqyManager;

        public UmsQueueConsumeService(
            IUmsMessageManager messageManager,
            IDingTalkMessageManager dingTalkManager,
            IUmsEmailMessageManager emailManager,
            ITxCloudSmsManager smsManager,
            IWxgzhMessageManager wxgzhManager,
            IWxgzhSubscribeMessageManager wxgzhSubscribeManager,
            IWxmpMessageManager wxmpManager,
            IWxqyMessageManager wxqyManager)
        {
            _messageManager = messageManager;
            _dingTalkManager = dingTalkManager;
            _emailManager = emailManager;
            _smsManager = smsManager;
            _wxgzhManager = wxgzhManager;
            _wxgzhSubscribeManager = wxgzhSubscribeManager;
            _wxmpManager = wxmpManager;
            _wxqyManager = wxqyManager;
        }

        /// <summary>
        /// 消费系统通知消息
        /// </summary>
        public async Task ReceiveSystemAsync(IChannel channel)
        {
            await _messageManager.ReceiveSystemAsync(channel);
        }

        /// <summary>
        /// 消费钉钉文本消息
        /// </summary>
        public async Task ReceiveDingTalkTextAsync(IChannel channel)
        {
            await _dingTalkManager.ReceiveTextAsync(channel);
        }

        /// <summary>
        /// 消费钉钉Markdown消息
        /// </summary>
        public async Task ReceiveDingTalkMarkdownAsync(IChannel channel)
        {
            await _dingTalkManager.ReceiveMarkdownAsync(channel);
        }

        /// <summary>
        /// 消费邮件消息
        /// </summary>
        public async Task ReceiveEmailAsync(IChannel channel)
        {
            await _emailManager.ReceiveAsync(channel);
        }

        /// <summary>
        /// 消费腾讯云短信消息
        /// </summary>
        public async Task ReceiveTxCloudSmsAsync(IChannel channel)
        {
            await _smsManager.ReceiveAsync(channel);
        }

        /// <summary>
        /// 消费公众号模板消息
        /// </summary>
        public async Task ReceiveWxgzhTemplateAsync(IChannel channel)
        {
            await _wxgzhManager.ReceiveTemplateAsync(channel);
        }

        /// <summary>
        /// 消费公众号订阅消息
        /// </summary>
        public async Task ReceiveWxgzhSubscribeAsync(IChannel channel)
        {
            await _wxgzhSubscribeManager.ReceiveSubscribeAsync(channel);
        }

        /// <summary>
        /// 消费小程序订阅消息
        /// </summary>
        public async Task ReceiveWxmpSubscribeTemplateAsync(IChannel channel)
        {
            await _wxmpManager.ReceiveSubscribeTemplateAsync(channel);
        }

        /// <summary>
        /// 消费企业微信文本消息
        /// </summary>
        public async Task ReceiveWxqyTextAsync(IChannel channel)
        {
            await _wxqyManager.ReceiveTextAsync(channel);
        }

        /// <summary>
        /// 消费企业微信Markdown消息
        /// </summary>
        public async Task ReceiveWxqyMarkdownAsync(IChannel channel)
        {
            await _wxqyManager.ReceiveMarkdownAsync(channel);
        }
    }
}
