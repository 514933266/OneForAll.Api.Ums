using AutoMapper;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using OneForAll.Core;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Domain.ValueObjects;
using OneForAll.Core.Extension;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Enums;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using OneForAll.EFCore;
using Azure;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace Ums.Domain
{
    /// <summary>
    /// 企业微信机器人消息通知
    /// </summary>
    public class WechatQyRootManager : UmsBaseManager, IWechatQyRootManager
    {
        private readonly ConnectionFactory _mqFactory;
        private readonly string _exchangeName = "wechatqy.robot.direct.exchange";

        private readonly IWechatQyRobotHttpService _httpService;
        private readonly IUmsMessageRecordRepository _repository;

        public WechatQyRootManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ConnectionFactory mqFactory,
            IUmsMessageRecordRepository repository,
            IWechatQyRobotHttpService httpService) : base(mapper, httpContextAccessor)
        {
            _mqFactory = mqFactory;
            _httpService = httpService;
            _repository = repository;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WechatQyRobotTextForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = WechatQyRootQueueName.Text,
                RouteKey = WechatQyRootQueueName.Text
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendAsync(WechatQyRootQueueName.Text, data.ToJson());
            }
            else
            {
                return BaseErrType.ServerError;
            }
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownAsync(WechatQyRobotTextForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _exchangeName,
                QueueName = WechatQyRootQueueName.Markdown,
                RouteKey = WechatQyRootQueueName.Markdown
            };
            return await SendAsync(WechatQyRootQueueName.Markdown, data.ToJson());
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
        /// 接收Text消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveTextAsync(IModel channel)
        {
            channel.QueueDeclare(WechatQyRootQueueName.Text, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WechatQyRobotTextForm>();
                    var request = _mapper.Map<WechatQyRobotTextRequest>(msg);
                    var response = _httpService.SendTextAsync(request, msg.WebhookUrl).Result;
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
            channel.BasicConsume(WechatQyRootQueueName.Text, true, consumer);
        }

        /// <summary>
        /// 接收Markdown消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveMarkdownAsync(IModel channel)
        {
            channel.QueueDeclare(WechatQyRootQueueName.Markdown, true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WechatQyRobotTextForm>();
                    var request = _mapper.Map<WechatQyRobotMarkdownRequest>(msg);
                    var response = _httpService.SendMarkdownAsync(request, msg.WebhookUrl).Result;
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
            channel.BasicConsume(WechatQyRootQueueName.Markdown, true, consumer);
        }
    }
}

