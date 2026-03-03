using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Net;
using System.Net.Mail;
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
    /// 邮件消息-直接发送
    /// </summary>
    public class UmsEmailDirectMessageManager : BaseManager, IUmsEmailDirectMessageManager
    {
        private readonly IConfiguration _config;
        private readonly IUmsMessageRecordRepository _repository;

        public UmsEmailDirectMessageManager(
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IConfiguration config) : base(httpContextAccessor)
        {
            _config = config;
            _repository = repository;
        }

        /// <summary>
        /// 直接发送邮件消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(UmsEmailMessageForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = "",
                QueueName = UmsQueueName.Email,
                RouteKey = ""
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType != BaseErrType.Success) return BaseErrType.ServerError;

            try
            {
                await SendEmailAsync(form);
                data.Status = UmsMessageStatusEnum.Success;
                data.Result = "发送成功";
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
        /// 通过SMTP发送邮件
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private async Task SendEmailAsync(UmsEmailMessageForm form)
        {
            var host = _config["Email:SmtpHost"];
            var port = _config["Email:SmtpPort"].TryInt();
            var userName = _config["Email:UserName"];
            var password = _config["Email:Password"];
            var displayName = _config["Email:DisplayName"] ?? "";
            var enableSsl = _config["Email:EnableSsl"].TryBoolean();

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(userName, displayName);
            mailMessage.Subject = form.Subject;
            mailMessage.Body = form.Body;
            mailMessage.IsBodyHtml = form.IsHtml;

            // 收件人
            var toAddresses = form.To.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var to in toAddresses)
            {
                mailMessage.To.Add(to.Trim());
            }

            // 抄送
            if (!form.Cc.IsNullOrEmpty())
            {
                var ccAddresses = form.Cc.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var cc in ccAddresses)
                {
                    mailMessage.CC.Add(cc.Trim());
                }
            }

            using var smtpClient = new SmtpClient(host, port);
            smtpClient.Credentials = new NetworkCredential(userName, password);
            smtpClient.EnableSsl = enableSsl;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
