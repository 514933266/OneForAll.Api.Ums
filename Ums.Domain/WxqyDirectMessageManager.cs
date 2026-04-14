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
    /// 企业微信-直接发送
    /// </summary>
    public class WxqyDirectMessageManager : BaseManager, IWxqyDirectMessageManager
    {
        private readonly IMapper _mapper;
        private readonly IWxqyHttpService _httpService;
        private readonly IUmsMessageRecordRepository _repository;

        public WxqyDirectMessageManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IWxqyHttpService httpService) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _httpService = httpService;
            _repository = repository;
        }

        /// <summary>
        /// 直接发送Text消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendTextDirectAsync(WxqyRobotMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = "",
                QueueName = UmsQueueName.WxqyRobot,
                RouteKey = ""
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType != BaseErrType.Success) return BaseErrType.ServerError;

            try
            {
                var request = _mapper.Map<WxqyRobotTextMessageRequest>(form);
                var response = await _httpService.SendRobotTextAsync(request, form.WebhookUrl);
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

        /// <summary>
        /// 直接发送Markdown消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendMarkdownDirectAsync(WxqyRobotMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = "direct",
                QueueName = "direct",
                RouteKey = "direct"
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType != BaseErrType.Success) return BaseErrType.ServerError;

            try
            {
                var request = _mapper.Map<WxqyRobotMarkdownMessageRequest>(form);
                var response = await _httpService.SendRobotMarkdownAsync(request, form.WebhookUrl);
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
