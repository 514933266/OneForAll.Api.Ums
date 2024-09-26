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
    public class WxQyRobotConsumerHostedService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IModel _channel;
        private readonly IWxqyManager _manager;
        public WxQyRobotConsumerHostedService(
            ConnectionFactory mqFactory,
            IWxqyManager manager)
        {
            _manager = manager;
            _conn = mqFactory.CreateConnection();
            _channel = _conn.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // 暂时先用一个信道，后期量上来再拆分
            await _manager.ReceiveTextAsync(_channel);
            await _manager.ReceiveMarkdownAsync(_channel);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Dispose();
            _conn.Dispose();
        }
    }
}

