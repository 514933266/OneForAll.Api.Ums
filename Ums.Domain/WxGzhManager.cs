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
    public class WxgzhManager : UmsBaseManager, IWxgzhManager
    {
        private readonly IConnection _mqConn;
        private readonly string _exchangeName = "wxgzh.direct.exchange";
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IWxgzhHttpService _httpService;
        public WxgzhManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConnection mqConn,
            IUmsMessageRecordRepository repository,
            IWxgzhHttpService httpService) : base(mapper, httpContextAccessor)
        {
            _mqConn = mqConn;
            _httpService = httpService;
            _repository = repository;
        }

        #region 模版消息

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WxgzhTemplateForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = WxgzhQueueName.Template,
                RouteKey = WxgzhQueueName.Template
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendAsync(WxgzhQueueName.Template, data.ToJson());
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
        public async Task ReceiveTemplateAsync(IModel channel)
        {
            channel.QueueDeclare(WxgzhQueueName.Template, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WxgzhTemplateForm>();
                    var request = _mapper.Map<WxgzhTemplateRequest>(msg);
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
            channel.BasicConsume(WxgzhQueueName.Template, true, consumer);
        }

        #endregion

        #region 长期订阅

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = WxgzhQueueName.Subscribe,
                RouteKey = WxgzhQueueName.Subscribe
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendAsync(WxgzhQueueName.Subscribe, data.ToJson());
            }
            else
            {
                return BaseErrType.ServerError;
            }
        }

        /// <summary>
        /// 接收长期订阅消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveSubscribeAsync(IModel channel)
        {
            channel.QueueDeclare(WxgzhQueueName.Subscribe, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WxgzhSubscribeForm>();
                    var request = _mapper.Map<WxgzhSubscribeRequest>(msg);
                    var response = _httpService.SendSubscribeAsync(request, msg.AccessToken).Result;
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
            channel.BasicConsume(WxgzhQueueName.Subscribe, true, consumer);
        }

        #endregion

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
