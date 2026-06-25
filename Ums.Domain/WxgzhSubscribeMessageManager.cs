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
    /// 微信公众号订阅消息推送
    /// </summary>
    public class WxgzhSubscribeMessageManager : UmsBaseMQManager, IWxgzhSubscribeMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IWxgzhHttpService _httpService;

        public override string QueueName => UmsQueueName.WxgzhSubscribe;

        public override string RouteKey => UmsQueueName.WxgzhSubscribe;

        public WxgzhSubscribeMessageManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxgzhHttpService httpService) : base(mqFactory, httpContextAccessor, repository)
        {
            _mapper = mapper;
            _httpService = httpService;
        }

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
        /// 接收长期订阅消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveSubscribeAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(QueueName, true, false, false);

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
            await channel.BasicConsumeAsync(QueueName, true, consumer);
        }
    }
}
