using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Ums.Domain.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// rabbitmq邮件消息消费托管
    /// </summary>
    public class EmailMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IUmsEmailMessageManager _manager;
        public EmailMessageConsumerHostedService(ConnectionFactory mqFactory, IUmsEmailMessageManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _manager.ReceiveAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.DisposeAsync();
            await _conn.DisposeAsync();
        }
    }
}
