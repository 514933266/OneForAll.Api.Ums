using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Ums.Application.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// rabbitmq公众号消息消费托管
    /// </summary>
    public class WxgzhMessageConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IChannel _channel;
        private readonly IUmsQueueConsumeService _consumeService;
        public WxgzhMessageConsumerHostedService(ConnectionFactory mqFactory, IUmsQueueConsumeService consumeService)
        {
            _consumeService = consumeService;
            _conn = mqFactory.CreateConnectionAsync().Result;
            _channel = _conn.CreateChannelAsync().Result;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _consumeService.ReceiveWxgzhTemplateAsync(_channel);
            await _consumeService.ReceiveWxgzhSubscribeAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.DisposeAsync();
            await _conn.DisposeAsync();
        }
    }
}
