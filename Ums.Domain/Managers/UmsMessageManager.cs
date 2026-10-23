using AutoMapper;
using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Public.Models;

namespace Ums.Domain.Managers
{
    /// <summary>
    /// 站内信
    /// </summary>
    public class UmsMessageManager : UmsBaseMQManager, IUmsMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageRepository _umsRepository;

        public override string QueueName => UmsQueueName.System;

        public override string RouteKey => UmsQueueName.System;

        public UmsMessageManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            ICurrentUser currentUser,
            IUmsMessageRecordRepository repository,
            IUmsMessageRepository umsRepository) : base(mqFactory, currentUser, repository)
        {
            _mapper = mapper;
            _umsRepository = umsRepository;
        }

        /// <summary>
        /// 发送系统通知消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemAsync(UmsMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = RequestPath,
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
        /// 接收系统通知消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveSystemAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(QueueName, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();
                var msg = record.OriginalMessage.FromJson<UmsMessageForm>();

                var exists = await _umsRepository.CountAsync(w => w.Id == msg.Id && w.ToAccountId == msg.ToAccountId) > 0;
                if (!exists)
                {
                    var item = _mapper.Map<UmsMessageForm, UmsMessage>(msg);
                    await _umsRepository.AddAsync(item);
                    record.Status = UmsMessageStatusEnum.Success;
                    record.Result = "发送成功";
                }
                else
                {
                    record.Status = UmsMessageStatusEnum.Fail;
                    record.Result = "发送失败：数据不存在";
                }
                await _repository.UpdateAsync(record);
            };
            await channel.BasicConsumeAsync(QueueName, true, consumer);
        }
    }
}