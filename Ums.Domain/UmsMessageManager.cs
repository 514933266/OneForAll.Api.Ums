using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Domain.ValueObjects;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 站内信
    /// </summary>
    public class UmsMessageManager : UmsBaseManager, IUmsMessageManager
    {
        private readonly string _exchangeName = "ums.direct.exchange";
        private readonly ConnectionFactory _mqFactory;
        private readonly MongoDbConnectionConfig _mongoDbConfig;

        private readonly IUmsMessageRecordRepository _repository;
        private readonly IUmsMessageRepository _umsRepository;
        private readonly IMongoDatabase _mongoDb;
        public UmsMessageManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IUmsMessageRepository umsRepository,
            ConnectionFactory mqFactory,
            MongoDbConnectionConfig mongoDbConfig,
            IMongoDatabase mongoDb = null) : base(mapper, httpContextAccessor)
        {
            _mqFactory = mqFactory;
            _mongoDbConfig = mongoDbConfig;
            _mongoDb = mongoDb;
            _repository = repository;
            _umsRepository = umsRepository;
        }

        /// <summary>
        /// 发送系统通知消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemAsync(UmsMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = UmsQueueName.System,
                RouteKey = UmsQueueName.System
            };
            return await SendAsync(UmsQueueName.System, data.ToJson());
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="msg">消息json</param>
        /// <returns></returns>
        private async Task<BaseErrType> SendAsync(string queueName, string msg)
        {
            using (var conn = _mqFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false);
                    channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);
                    channel.QueueBind(queueName, _exchangeName, queueName);

                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    byte[] body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(_exchangeName, UmsQueueName.System, basicProperties, body);
                }
            }
            return BaseErrType.Success;
        }

        /// <summary>
        /// 接收系统通知消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveSystemAsync(IModel channel)
        {
            channel.QueueDeclare(UmsQueueName.System, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();
                var msg = record.OriginalMessage.FromJson<UmsMessageForm>();

                if (_mongoDbConfig.IsEnabled)
                {
                    #region MongoDb模式
                    var collection = _mongoDb.GetCollection<UmsMessage>("Ums_Message");
                    var exists = collection.CountDocuments(w => w.Id == msg.Id && w.ToAccountId == msg.ToAccountId) > 0;
                    if (!exists)
                    {
                        var data = _mapper.Map<UmsMessageForm, UmsMessage>(msg);
                        collection.InsertOne(data);
                        record.Result = "Success";
                    }
                    else
                    {
                        record.Result = "Fail";
                    }
                    #endregion
                }
                else
                {
                    #region 数据库模式
                    var exists = _umsRepository.CountAsync(w => w.Id == msg.Id && w.ToAccountId == msg.ToAccountId).Result > 0;
                    if (!exists)
                    {
                        var data = _mapper.Map<UmsMessageForm, UmsMessage>(msg);
                        _umsRepository.AddAsync(data);
                        record.Result = "Success";
                    }
                    else
                    {
                        record.Result = "Fail";
                    }
                    #endregion
                }
                _repository.AddAsync(record);
            };
            channel.BasicConsume(UmsQueueName.System, true, consumer);
        }
    }
}