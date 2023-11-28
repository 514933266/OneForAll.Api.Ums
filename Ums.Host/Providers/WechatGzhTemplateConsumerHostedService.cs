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
    public class WechatGzhTemplateConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IModel _channel;
        private readonly IWechatGzhManager _manager;
        public WechatGzhTemplateConsumerHostedService(
            ConnectionFactory mqFactory,
            IWechatGzhManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnection();
            _channel = _conn.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _manager.ReceiveTemplateAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Dispose();
            _conn.Dispose();
        }
    }
}

