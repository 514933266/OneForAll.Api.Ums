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
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 微信小程序-直接发送
    /// </summary>
    public class WxmpDirectMessageManager : BaseManager, IWxmpDirectMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IWxmpHttpService _httpService;

        public WxmpDirectMessageManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxmpHttpService httpService) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _httpService = httpService;
            _repository = repository;
        }

        /// <summary>
        /// 直接发送模板消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendSubscribeTemplateDirectAsync(WxmpSubscribeTemplateMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = "",
                QueueName = UmsQueueName.WxmpSubscribeTemplate,
                RouteKey = ""
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType != BaseErrType.Success) return BaseErrType.ServerError;

            try
            {
                var request = _mapper.Map<WxmpSubscribeTemplateMessageRequest>(form);
                var response = await _httpService.SendSubscribTemplateAsync(request, form.AccessToken);
                if (response.Status)
                {
                    data.Status = UmsMessageStatusEnum.Success;
                    data.Result = "发送成功";
                }
                else
                {
                    data.Status = UmsMessageStatusEnum.Fail;
                    data.Result = "发送失败：".Append(response.Message);
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
