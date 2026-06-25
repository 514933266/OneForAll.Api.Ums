using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 站内信-直接发送
    /// </summary>
    public class UmsMessageDirectManager : UmsBaseManager, IUmsMessageDirectManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageRepository _umsRepository;

        public override string ExChangeName => "direct";

        public override string QueueName => UmsQueueName.System;

        public override string RouteKey => "direct";

        public UmsMessageDirectManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IUmsMessageRepository umsRepository) : base(httpContextAccessor, repository)
        {
            _mapper = mapper;
            _umsRepository = umsRepository;
        }

        /// <summary>
        /// 直接发送系统通知消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSystemDirectAsync(UmsMessageForm form)
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
            if (errType != BaseErrType.Success) return BaseErrType.ServerError;

            try
            {
                var exists = await _umsRepository.CountAsync(w => w.Id == form.Id && w.ToAccountId == form.ToAccountId) > 0;
                if (!exists)
                {
                    var item = _mapper.Map<UmsMessageForm, UmsMessage>(form);
                    await _umsRepository.AddAsync(item);
                    data.Status = UmsMessageStatusEnum.Success;
                    data.Result = "发送成功";
                }
                else
                {
                    data.Status = UmsMessageStatusEnum.Fail;
                    data.Result = "发送失败：数据不存在";
                }
            }
            catch (Exception ex)
            {
                data.Status = UmsMessageStatusEnum.Error;
                data.Result = "发送异常：".Append(ex.Message);
            }
            await _repository.UpdateAsync(data);
            return data.Status == UmsMessageStatusEnum.Success ? BaseErrType.Success : BaseErrType.Fail;
        }
    }
}
