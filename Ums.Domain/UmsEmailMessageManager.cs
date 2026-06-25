using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
    /// 邮件消息
    /// </summary>
    public class UmsEmailMessageManager : UmsBaseMQManager, IUmsEmailMessageManager
    {
        private readonly IConfiguration _config;

        public override string QueueName => UmsQueueName.Email;

        public override string RouteKey => UmsQueueName.Email;

        public UmsEmailMessageManager(
            ConnectionFactory mqFactory,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository) : base(mqFactory, httpContextAccessor, repository)
        {
            _config = config;
        }

        /// <summary>
        /// 发送邮件消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(UmsEmailMessageForm form)
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
        /// 接收邮件消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(QueueName, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var request = record.OriginalMessage.FromJson<UmsEmailMessageForm>();
                    await SendEmailAsync(request);
                    record.Status = UmsMessageStatusEnum.Success;
                    record.Result = "发送成功";
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
