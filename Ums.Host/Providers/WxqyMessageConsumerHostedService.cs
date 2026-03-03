using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Ums.Domain.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// 企业微信消费者
    /// </summary>
    public class WxqyMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IWxqyMessageManager _manager;
        public WxqyMessageConsumerHostedService(ConnectionFactory mqFactory, IWxqyMessageManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 暂时先用一个信道，后期量上来再拆分
            await _manager.ReceiveTextAsync(_channel);
            await _manager.ReceiveMarkdownAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.DisposeAsync();
            await _conn.DisposeAsync();
        }
    }
}

