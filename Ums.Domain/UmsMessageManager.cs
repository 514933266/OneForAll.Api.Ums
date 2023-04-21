using AutoMapper;
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
        private readonly IMongoDatabase _mongoDb;
        private readonly ConnectionFactory _mqFactory;
        private readonly IUmsFailureMessageRecordRepository _repository;

        public UmsMessageManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsFailureMessageRecordRepository repository,
            ConnectionFactory mqFactory,
            IMongoDatabase mongoDb) : base(mapper, httpContextAccessor)
        {
            _mqFactory = mqFactory;
            _repository = repository;
            _mongoDb = mongoDb;
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(UmsMessageForm form)
        {
            using (var conn = _mqFactory.CreateConnection())
            {
                var exchangeName = "ums.topic.exchange";
                var msg = form.ToJson();
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(UmsQueueName.System, true, false, false);
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                    channel.QueueBind(UmsQueueName.System, exchangeName, UmsQueueName.System);

                    channel.ConfirmSelect();
                    channel.BasicNacks += async (e, a) =>
                    {
                        await _repository.AddAsync(new UmsFailureMessageRecord()
                        {
                            Id = Guid.NewGuid(),
                            OriginalMessage = msg,
                            ExChangeName = exchangeName,
                            QueueName = UmsQueueName.System,
                            Type = UmsFailureMessageTypeEnum.UnConfirmed
                        });
                    };

                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;
                    byte[] body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(exchangeName, UmsQueueName.System, basicProperties, body);
                }
            }
            return BaseErrType.Success;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveAsync(IModel channel)
        {
            channel.QueueDeclare(UmsQueueName.System, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var msg = msgStr.FromJson<UmsMessageForm>();
                var collection = _mongoDb.GetCollection<UmsMessage>("Ums_Message");
                var exists = collection.CountDocuments(w => w.Id == msg.Id && w.ToAccountId == msg.ToAccountId) > 0;
                if (!exists)
                {
                    var data = _mapper.Map<UmsMessageForm, UmsMessage>(msg);
                    collection.InsertOne(data);
                }
            };
            channel.BasicConsume(UmsQueueName.System, true, consumer);
        }
    }
}
