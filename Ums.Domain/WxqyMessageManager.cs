using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using OneForAll.Core.Extension;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 企业微信机器人消息通知
    /// </summary>
    public class WxqyMessageManager : UmsBaseMQManager, IWxqyMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IWxqyHttpService _httpService;

        public override string QueueName => UmsQueueName.WxqyRobot;

        public override string RouteKey => UmsQueueName.WxqyRobot;

        public WxqyMessageManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxqyHttpService httpService) : base(mqFactory, httpContextAccessor, repository)
        {
            _mapper = mapper;
            _httpService = httpService;
        }

        /// <summary>
        /// 发送Text消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextAsync(WxqyRobotMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = ExChangeName,
                QueueName = QueueName,
                RouteKey = RouteKey
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendToRabbitMQAsync(QueueName, RouteKey, data.ToJson());
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
        public async Task<BaseErrType> SendMarkdownAsync(WxqyRobotMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = ExChangeName,
                QueueName = QueueName,
                RouteKey = RouteKey
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendToRabbitMQAsync(QueueName, RouteKey, data.ToJson());
            }
            else
            {
                return BaseErrType.ServerError;
            }
        }

        /// <summary>
        /// 接收Text消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveTextAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(QueueName, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WxqyRobotMessageForm>();
                    var request = _mapper.Map<WxqyRobotTextMessageRequest>(msg);
                    var response = await _httpService.SendRobotTextAsync(request, msg.WebhookUrl);
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
               await _repository.UpdateAsync(record);
            };
            await channel.BasicConsumeAsync(QueueName, true, consumer);
        }

        /// <summary>
        /// 接收Markdown消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveMarkdownAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(QueueName, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WxqyRobotMessageForm>();
                    var request = _mapper.Map<WxqyRobotMarkdownMessageRequest>(msg);
                    var response = await _httpService.SendRobotMarkdownAsync(request, msg.WebhookUrl);
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
                await _repository.UpdateAsync(record);
            };
            await channel.BasicConsumeAsync(QueueName, true, consumer);
        }
    }
}
