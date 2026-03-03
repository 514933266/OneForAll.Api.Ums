using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    public class WxgzhMessageManager : UmsBaseMQManager, IWxgzhMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IWxgzhHttpService _httpService;
        public WxgzhMessageManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxgzhHttpService httpService) : base(mqFactory, mapper, httpContextAccessor)
        {
            _mapper = mapper;
            _httpService = httpService;
            _repository = repository;
        }

        #region 模版消息

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTemplateAsync(WxgzhTemplateMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _directExchangeName,
                QueueName = UmsQueueName.WxgzhTemplate,
                RouteKey = UmsQueueName.WxgzhTemplate
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendDirectAsync(UmsQueueName.WxgzhTemplate, UmsQueueName.WxgzhTemplate, data.ToJson());
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
        public async Task ReceiveTemplateAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(UmsQueueName.WxgzhTemplate, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();
                try
                {
                    var msg = record.OriginalMessage.FromJson<WxgzhTemplateMessageForm>();
                    var request = _mapper.Map<WxgzhTemplateMessageRequest>(msg);
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
                await _repository.UpdateAsync(record);
            };
            await channel.BasicConsumeAsync(UmsQueueName.WxgzhTemplate, true, consumer);
        }

        #endregion

        #region 长期订阅

        /// <summary>
        /// 发送长期订阅消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeAsync(WxgzhSubscribeMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _directExchangeName,
                QueueName = UmsQueueName.WxgzhSubscribe,
                RouteKey = UmsQueueName.WxgzhSubscribe
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendDirectAsync(UmsQueueName.WxgzhSubscribe, UmsQueueName.WxgzhSubscribe, data.ToJson());
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
        public async Task ReceiveSubscribeAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(UmsQueueName.WxgzhSubscribe, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();
                try
                {
                    var msg = record.OriginalMessage.FromJson<WxgzhSubscribeMessageForm>();
                    var request = _mapper.Map<WxgzhSubscribeMessageRequest>(msg);
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
                await _repository.UpdateAsync(record);
            };
            await channel.BasicConsumeAsync(UmsQueueName.WxgzhSubscribe, true, consumer);
        }

        #endregion
    }
}
