using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Ums.Domain.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// 微信小程序消费者
    /// </summary>
    public class WxmpMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IWxmpMessageManager _manager;
        public WxmpMessageConsumerHostedService(ConnectionFactory mqFactory, IWxmpMessageManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _manager.ReceiveSubscribeTemplateAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.DisposeAsync();
            await _conn.DisposeAsync();
        }
    }
}

