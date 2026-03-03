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
    public class WxgzhMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IWxgzhMessageManager _manager;
        public WxgzhMessageConsumerHostedService(ConnectionFactory mqFactory, IWxgzhMessageManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _manager.ReceiveTemplateAsync(_channel);
            await _manager.ReceiveSubscribeAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.DisposeAsync();
            await _conn.DisposeAsync();
        }
    }
}

