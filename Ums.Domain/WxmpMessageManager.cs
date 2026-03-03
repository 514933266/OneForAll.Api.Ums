using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.HttpService.Interfaces;
using OneForAll.Core.Extension;
using Ums.Domain.Enums;
using Ums.HttpService.Models;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpMessageManager : UmsBaseMQManager, IWxmpMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IWxmpHttpService _httpService;
        public WxmpMessageManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxmpHttpService httpService) : base(mqFactory, mapper, httpContextAccessor)
        {
            _mapper = mapper;
            _httpService = httpService;
            _repository = repository;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeTemplateAsync(WxmpSubscribeTemplateMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _directExchangeName,
                QueueName = UmsQueueName.WxmpSubscribeTemplate,
                RouteKey = UmsQueueName.WxmpSubscribeTemplate
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendDirectAsync(UmsQueueName.WxmpSubscribeTemplate, UmsQueueName.WxmpSubscribeTemplate, data.ToJson());
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
        public async Task ReceiveSubscribeTemplateAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(UmsQueueName.WxmpSubscribeTemplate, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var msg = record.OriginalMessage.FromJson<WxmpSubscribeTemplateMessageForm>();
                    var request = _mapper.Map<WxmpSubscribeTemplateMessageRequest>(msg);
                    var response = await _httpService.SendSubscribTemplateAsync(request, msg.AccessToken);
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
            await channel.BasicConsumeAsync(UmsQueueName.WxmpSubscribeTemplate, true, consumer);
        }
    }
}
