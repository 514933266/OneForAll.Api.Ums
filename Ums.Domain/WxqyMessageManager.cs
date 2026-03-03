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
        private readonly IUmsMessageRecordRepository _repository;

        public WxqyMessageManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxqyHttpService httpService) : base(mqFactory, mapper, httpContextAccessor)
        {
            _mapper = mapper;
            _httpService = httpService;
            _repository = repository;
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
                ExChangeName = _directExchangeName,
                QueueName = UmsQueueName.WxqyRobotText,
                RouteKey = UmsQueueName.WxqyRobotText
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendDirectAsync(UmsQueueName.WxqyRobotText, UmsQueueName.WxqyRobotText, data.ToJson());
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
                ExChangeName = _directExchangeName,
                QueueName = UmsQueueName.WxqyRobotMarkdown,
                RouteKey = UmsQueueName.WxqyRobotMarkdown
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendDirectAsync(UmsQueueName.WxqyRobotMarkdown, UmsQueueName.WxqyRobotMarkdown, data.ToJson());
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
            await channel.QueueDeclareAsync(UmsQueueName.WxqyRobotText, true, false, false);

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
            await channel.BasicConsumeAsync(UmsQueueName.WxqyRobotText, true, consumer);
        }

        /// <summary>
        /// 接收Markdown消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveMarkdownAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(UmsQueueName.WxqyRobotText, true, false, false);

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
            await channel.BasicConsumeAsync(UmsQueueName.WxgzhSubscribe, true, consumer);
        }
    }
}

