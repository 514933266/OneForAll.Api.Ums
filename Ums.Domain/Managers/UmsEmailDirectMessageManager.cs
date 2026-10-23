using AutoMapper;
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
using Ums.Domain.ValueObjects;
using Ums.Public.Models;

namespace Ums.Domain.Managers
{
    /// <summary>
    /// 邮件消息-直接发送
    /// </summary>
    public class UmsEmailDirectMessageManager : UmsBaseManager, IUmsEmailDirectMessageManager
    {
        private readonly EmailSmtpSettingVo _emailSetting;

        public override string ExChangeName => "direct";

        public override string QueueName => UmsQueueName.Email;

        public override string RouteKey => "direct";

        public UmsEmailDirectMessageManager(
            EmailSmtpSettingVo emailSetting,
            ICurrentUser currentUser,
            IUmsMessageRecordRepository repository) : base(currentUser, repository)
        {
            _emailSetting = emailSetting;
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
                RequestUrl = RequestPath,
                OriginalMessage = form.ToJson(),
                ExChangeName = ExChangeName,
                QueueName = QueueName,
                RouteKey = RouteKey
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
            var host = _emailSetting?.SmtpHost;
            var port = _emailSetting?.SmtpPort.TryInt() ?? 0;
            var userName = _emailSetting?.UserName;
            var password = _emailSetting?.Password;
            var displayName = _emailSetting?.DisplayName ?? "";
            var enableSsl = _emailSetting?.EnableSsl.TryBoolean() ?? false;

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
