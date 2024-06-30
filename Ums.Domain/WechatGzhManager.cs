using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Domain.ValueObjects;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    public class WechatGzhManager : UmsBaseManager, IWechatGzhManager
    {
        private readonly ConnectionFactory _mqFactory;
        private readonly string _exchangeName = "wechatgzh.template.direct.exchange";
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IWechatGzhHttpService _httpService;
        public WechatGzhManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ConnectionFactory mqFactory,
            IUmsMessageRecordRepository repository,
            IWechatGzhHttpService httpService) : base(mapper, httpContextAccessor)
        {
            _mqFactory = mqFactory;
            _httpService = httpService;
            _repository = repository;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WechatGzhTemplateForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = WechatGzhQueueName.Template,
                RouteKey = WechatGzhQueueName.Template
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendAsync(WechatGzhQueueName.Template, data.ToJson());
            }
            else
            {
                return BaseErrType.ServerError;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="msg">消息json</param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(string queueName, string msg)
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
                    channel.BasicPublish(_exchangeName, queueName, basicProperties, body);
                }
            }
            return BaseErrType.Success;
        }

        /// <summary>
        /// 接收模板消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveTemplateAsync(IModel channel)
        {
            channel.QueueDeclare(WechatGzhQueueName.Template, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WechatGzhTemplateForm>();
                    var request = _mapper.Map<WechatGzhTemplateRequest>(msg);
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
            channel.BasicConsume(WechatGzhQueueName.Template, true, consumer);
        }
    }
}
