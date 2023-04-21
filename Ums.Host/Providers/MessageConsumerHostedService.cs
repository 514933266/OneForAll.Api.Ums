using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Ums.Domain.Interfaces;

namespace Ums.Host
{
    /// <summary>
    /// rabbitmq系统通知消费托管
    /// </summary>
    public class MessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IModel _channel;
        private readonly IUmsMessageManager _manager;
        public MessageConsumerHostedService(IUmsMessageManager manager, ConnectionFactory mqFactory)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnection();
            _channel = _conn.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _manager.ReceiveAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Dispose();
            _conn.Dispose();
        }
    }
}
