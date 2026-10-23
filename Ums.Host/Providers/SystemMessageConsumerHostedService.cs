using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Ums.Application.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// rabbitmq系统通知消费托管
    /// </summary>
    public class SystemMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IUmsQueueConsumeService _consumeService;
        public SystemMessageConsumerHostedService(ConnectionFactory mqFactory, IUmsQueueConsumeService consumeService)
        {
            _consumeService = consumeService;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _consumeService.ReceiveSystemAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.DisposeAsync();
            await _conn.DisposeAsync();
        }
    }
}
