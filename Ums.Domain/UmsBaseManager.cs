using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;

namespace Ums.Domain
{
    /// <summary>
    /// 消息管理器基类
    /// </summary>
    public abstract class UmsBaseManager : BaseManager, IUmsBaseManager
    {
        protected readonly IUmsMessageRecordRepository _repository;

        /// <summary>
        /// RabbitMQ交换机名称
        /// </summary>
        public abstract string ExChangeName { get; }

        /// <summary>
        /// RabbitMQ队列名称
        /// </summary>
        public abstract string QueueName { get; }

        /// <summary>
        /// RabbitMQ路由键
        /// </summary>
        public abstract string RouteKey { get; }

        protected UmsBaseManager(
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository) : base(httpContextAccessor)
        {
            _repository = repository;
        }

        /// <summary>
        /// 仅记录消息数据（不发送）
        /// </summary>
        /// <param name="form">消息表单</param>
        /// <returns></returns>
        public async Task<BaseErrType> RecordAsync<T>(T form)
        {
            var record = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext?.Request?.Path ?? "/",
                OriginalMessage = form.ToJson(),
                ExChangeName = ExChangeName,
                QueueName = QueueName,
                RouteKey = RouteKey,
                Status = UmsMessageStatusEnum.Success,
                Result = "消息已去重，仅记录不发送"
            };
            await _repository.AddAsync(record);
            return BaseErrType.Success;
        }
    }
}
