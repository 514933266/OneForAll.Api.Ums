using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ums.Domain
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class UmsBaseMQManager : BaseManager
    {
        private static IConnection _mqConn;
        private static readonly SemaphoreSlim _connLock = new SemaphoreSlim(1, 1);
        private readonly ConnectionFactory _mqFactory;
        protected readonly string _directExchangeName = "direct.ums.exchange";

        public UmsBaseMQManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _mqFactory = mqFactory;
        }

        /// <summary>
        /// 获取单例连接
        /// </summary>
        /// <returns></returns>
        private async Task<IConnection> GetConnectionAsync()
        {
            if (_mqConn != null && _mqConn.IsOpen)
                return _mqConn;

            await _connLock.WaitAsync();
            try
            {
                if (_mqConn != null && _mqConn.IsOpen)
                    return _mqConn;

                _mqConn = await _mqFactory.CreateConnectionAsync();
                return _mqConn;
            }
            finally
            {
                _connLock.Release();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="routeKey">路由</param>
        /// <param name="msg">消息json</param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(string queueName, string routeKey, string msg)
        {
            if (_mqFactory == null)
                throw new InvalidOperationException("RabbitMQ未启用");

            var conn = await GetConnectionAsync();
            var channel = await conn.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, true, false, false);
            await channel.ExchangeDeclareAsync(_directExchangeName, ExchangeType.Direct, true);
            await channel.QueueBindAsync(queueName, _directExchangeName, queueName);

            var basicProperties = new BasicProperties();
            basicProperties.DeliveryMode = DeliveryModes.Persistent;
            byte[] body = Encoding.UTF8.GetBytes(msg);

            await channel.BasicPublishAsync(_directExchangeName, routeKey, true, basicProperties, body);
            await channel.DisposeAsync();
            return BaseErrType.Success;
        }
    }
}
