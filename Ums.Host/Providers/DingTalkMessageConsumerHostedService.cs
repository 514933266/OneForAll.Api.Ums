using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Ums.Domain.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// 钉钉消费者
    /// </summary>
    public class DingTalkMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IDingTalkMessageManager _manager;
        public DingTalkMessageConsumerHostedService(ConnectionFactory mqFactory, IDingTalkMessageManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
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
