using AutoMapper;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;
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
    /// 腾讯云短信-直接发送
    /// </summary>
    public class TxCloudSmsDirectManager : UmsBaseManager, ITxCloudSmsDirectManager
    {
        private readonly TxCloudSmsSettingVo _smsSetting;
        private readonly IUmsSmsRecordRepository _smsRepository;


        public override string ExChangeName => "direct";

        public override string QueueName => UmsQueueName.TxCloudSms;

        public override string RouteKey => "direct";

        public TxCloudSmsDirectManager(
            IMapper mapper,
            TxCloudSmsSettingVo smsSetting,
            ICurrentUser currentUser,
            IUmsMessageRecordRepository repository,
            IUmsSmsRecordRepository smsRepository) : base(currentUser, repository)
        {
            _smsSetting = smsSetting;
            _smsRepository = smsRepository;
        }

        /// <summary>
        /// 直接发送短信消息（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(TxCloudSmsForm form)
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
                var response = await SendToTxCloudAsync(form);
                if (response.Status)
                {
                    data.Status = UmsMessageStatusEnum.Success;
                    data.Result = "发送成功";
                    await _smsRepository.AddAsync(new UmsSmsRecord()
                    {
                        ErrMsg = response.Message,
                        PlatformName = "腾讯云",
                        Content = form.Content,
                        PhoneNumber = form.PhoneNumber,
                        MoudleCode = form.ModuleCode,
                        MoudleName = form.ModuleName,
                        SignName = form.SignName,
                        Status = UmsSmsSendStatusEnum.Success,
                    });
                }
                else
                {
                    data.Status = UmsMessageStatusEnum.Fail;
                    data.Result = "发送失败：".Append(response.Message);

                    if (response.ErrType == BaseErrType.TokenInvalid)
                    {
                        await _repository.UpdateAsync(data);
                        return BaseErrType.TokenInvalid;
                    }
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
        /// 发送短信到腾讯云
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private async Task<BaseMessage> SendToTxCloudAsync(TxCloudSmsForm form)
        {
            var msg = new BaseMessage();
            var secretId = _smsSetting?.SecretId;
            var secretKey = _smsSetting?.SecretKey;
            var appId = _smsSetting?.AppId;
            var signs = new String[]{
                (form.ModuleCode + DateTime.Now.ToString("yyyyMMddhhmm")).ToMd5(),
                (form.ModuleCode + DateTime.Now.AddMinutes(-1).ToString("yyyyMMddhhmm")).ToMd5()
            };

            if (!signs.Any(w => w == form.Sign))
                return msg.Fail(BaseErrType.TokenInvalid);

            Credential cred = new Credential
            {
                SecretId = secretId,
                SecretKey = secretKey
            };
            ClientProfile clientProfile = new ClientProfile();
            clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.ReqMethod = "GET";
            httpProfile.Timeout = 10;
            httpProfile.Endpoint = "sms.tencentcloudapi.com";

            clientProfile.HttpProfile = httpProfile;
            SmsClient client = new SmsClient(cred, "ap-guangzhou", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            req.SmsSdkAppId = appId;
            req.SignName = form.SignName;
            req.TemplateId = form.TemplateId;
            req.TemplateParamSet = form.Content.IsNullOrEmpty() ? new string[0] : form.Content.Split(',');
            req.PhoneNumberSet = form.PhoneNumber.Split(',');
            req.SessionContext = "";
            req.ExtendCode = "";
            req.SenderId = "";

            SendSmsResponse resp = client.SendSmsSync(req);

            var str = AbstractModel.ToJsonString(resp);
            var result = StringHelper.MatchMiddleValue(str, "\"Code\":\"", "\"");
            var error = StringHelper.MatchMiddleValue(str, "\"Message\":\"", "\"");
            var success = result.ToLower() == "ok" ? true : false;

            return success ? msg.Success(error) : msg.Fail(error);
        }
    }
}
