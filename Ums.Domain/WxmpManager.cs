using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Domain.ValueObjects;
using Ums.HttpService.Interfaces;
using OneForAll.Core.Extension;
using Ums.Domain.Enums;
using Ums.HttpService.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpManager : UmsBaseManager, IWxmpManager
    {
        private readonly IConnection _mqConn;
        private readonly string _exchangeName = "wxmp.direct.exchange";
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IWxmpHttpService _httpService;
        public WxmpManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConnection mqConn,
            IUmsMessageRecordRepository repository,
            IWxmpHttpService httpService) : base(mapper, httpContextAccessor)
        {
            _mqConn = mqConn;
            _httpService = httpService;
            _repository = repository;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxmpSubscribeForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = WxmpQueueName.Subscribe,
                RouteKey = WxmpQueueName.Subscribe
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendAsync(WxmpQueueName.Subscribe, data.ToJson());
            }
            else
            {
                return BaseErrType.ServerError;
            }
        }

        /// <summary>
        /// 接收模板消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveSubscribeAsync(IModel channel)
        {
            channel.QueueDeclare(WxmpQueueName.Subscribe, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WxmpSubscribeForm>();
                    var request = _mapper.Map<WxmpSubscribeRequest>(msg);
                    var response = _httpService.SendTemplateAsync(request, msg.AccessToken).Result;
                    if (response.Status)
                    {
                        record.Status = UmsMessageStatusEnum.Success;
                        record.Result = "发送成功";
                    }
                    else
                    {
                        record.Status = UmsMessageStatusEnum.Fail;
                        record.Result = "发送失败：".Append(response.Message);
                    }
                }
                catch (Exception ex)
                {
                    record.Status = UmsMessageStatusEnum.Error;
                    record.Result = "发送异常：".Append(ex.Message);
                }
                _repository.UpdateAsync(record);
            };
            channel.BasicConsume(WxmpQueueName.Subscribe, true, consumer);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="msg">消息json</param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(string queueName, string msg)
        {
            using (var channel = _mqConn.CreateModel())
            {
                channel.QueueDeclare(queueName, true, false, false);
                channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);
                channel.QueueBind(queueName, _exchangeName, queueName);

                var basicProperties = channel.CreateBasicProperties();
                basicProperties.DeliveryMode = 2;
                byte[] body = Encoding.UTF8.GetBytes(msg);
                channel.BasicPublish(_exchangeName, queueName, basicProperties, body);
            }
            return BaseErrType.Success;
        }
    }
}
