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
    public class WxmpSubscribeConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IModel _channel;
        private readonly IWxmpManager _manager;
        public WxmpSubscribeConsumerHostedService(
            ConnectionFactory mqFactory,
            IWxmpManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnection();
            _channel = _conn.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _manager.ReceiveSubscribeAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Dispose();
            _conn.Dispose();
        }
    }
}

