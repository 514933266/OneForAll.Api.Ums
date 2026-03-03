using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Common.Profile;
using TencentCloud.Common;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;
using Microsoft.Extensions.Configuration;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using Microsoft.IdentityModel.Tokens;
using OneForAll.Core.Extension;
using OneForAll.EFCore;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Ums.Domain.Entities;
using Ums.Public.Models;
using Ums.Domain.Repositorys;
using System.Text;
using Ums.Domain.Enums;

namespace Ums.Domain
{
    /// <summary>
    /// 腾讯云-短信发送
    /// </summary>
    public class TxCloudSmsManager : UmsBaseMQManager, ITxCloudSmsManager
    {
        private readonly IConfiguration _config;
        private readonly IUmsMessageRecordRepository _repository;
        private readonly IUmsSmsRecordRepository _smdRepository;

        public TxCloudSmsManager(
            ConnectionFactory mqFactory,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRecordRepository repository,
            IUmsSmsRecordRepository smdRepository,
            IConfiguration config) : base(mqFactory, mapper, httpContextAccessor)
        {
            _config = config;
            _repository = repository;
            _smdRepository = smdRepository;
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(TxCloudSmsForm form)
        {
            var data = new UmsMessageRecord()
            {
                MessageId = Guid.NewGuid(),
                RequestUrl = _httpContextAccessor.HttpContext.Request.Path,
                OriginalMessage = form.ToJson(),
                ExChangeName = _directExchangeName,
                QueueName = UmsQueueName.TxCloudSms,
                RouteKey = UmsQueueName.TxCloudSms
            };
            var errType = await ResultAsync(() => _repository.AddAsync(data));
            if (errType == BaseErrType.Success)
            {
                return await SendDirectAsync(UmsQueueName.TxCloudSms, UmsQueueName.TxCloudSms, data.ToJson());
            }
            else
            {
                return BaseErrType.ServerError;
            }
        }

        /// <summary>
        /// 接收Text消息
        /// </summary>
        /// <param name="channel">信道</param>
        /// <returns></returns>
        public async Task ReceiveAsync(IChannel channel)
        {
            await channel.QueueDeclareAsync(UmsQueueName.TxCloudSms, true, false, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, e) =>
            {
                var msgStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var record = msgStr.FromJson<UmsMessageRecord>();

                try
                {
                    var request = record.OriginalMessage.FromJson<TxCloudSmsForm>();
                    var response = await SendToTxCloudAsync(request);
                    if (response.Status)
                    {
                        record.Status = UmsMessageStatusEnum.Success;
                        record.Result = "发送成功";
                        // 写入短信发送记录
                        await _smdRepository.AddAsync(new UmsSmsRecord()
                        {
                            ErrMsg = response.Message,
                            PlatformName = "腾讯云",
                            Content = request.Content,
                            PhoneNumber = request.PhoneNumber,
                            MoudleCode = request.MoudleCode,
                            MoudleName = request.MoudleName,
                            SignName = request.SignName,
                            Status = response.Status ? UmsSmsSendStatusEnum.Success : UmsSmsSendStatusEnum.Error,
                        });
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
            await channel.BasicConsumeAsync(UmsQueueName.TxCloudSms, true, consumer);
        }

        /// <summary>
        /// 发送系统通知消息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseMessage> SendToTxCloudAsync(TxCloudSmsForm form)
        {
            var msg = new BaseMessage();
            var secretId = _config["Sms:TxCloud:SecretId"];
            var secretKey = _config["Sms:TxCloud:SecretKey"];
            var appId = _config["Sms:TxCloud:AppId"];
            var signs = new String[]{
                (form.MoudleCode + DateTime.Now.ToString("yyyyMMddhhmm")).ToMd5(),
                (form.MoudleCode + DateTime.Now.AddMinutes(-1).ToString("yyyyMMddhhmm")).ToMd5()
            };

            if (!signs.Any(w => w == form.Sign))
                return msg.Fail(BaseErrType.TokenInvalid);

            /* 必要步骤：
             * 实例化一个认证对象，入参需要传入腾讯云账户密钥对secretId，secretKey。
             * 这里采用的是从环境变量读取的方式，需要在环境变量中先设置这两个值。
             * 你也可以直接在代码中写死密钥对，但是小心不要将代码复制、上传或者分享给他人，
             * 以免泄露密钥对危及你的财产安全。
             * SecretId、SecretKey 查询: https://console.cloud.tencent.com/cam/capi */
            Credential cred = new Credential
            {
                SecretId = secretId,
                SecretKey = secretKey
            };
            /* 非必要步骤:
             * 实例化一个客户端配置对象，可以指定超时时间等配置 */
            ClientProfile clientProfile = new ClientProfile();
            /* SDK默认用TC3-HMAC-SHA256进行签名
             * 非必要请不要修改这个字段 */
            clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
            /* 非必要步骤
             * 实例化一个客户端配置对象，可以指定超时时间等配置 */
            HttpProfile httpProfile = new HttpProfile();
            /* SDK默认使用POST方法。
             * 如果你一定要使用GET方法，可以在这里设置。GET方法无法处理一些较大的请求 */
            httpProfile.ReqMethod = "GET";
            /* SDK有默认的超时时间，非必要请不要进行调整
             * 如有需要请在代码中查阅以获取最新的默认值 */
            httpProfile.Timeout = 10; // 请求连接超时时间，单位为秒(默认60秒)
            /* 指定接入地域域名，默认就近地域接入域名为 sms.tencentcloudapi.com ，也支持指定地域域名访问，例如广州地域的域名为 sms.ap-guangzhou.tencentcloudapi.com */
            httpProfile.Endpoint = "sms.tencentcloudapi.com";
            // 代理服务器，当你的环境下有代理服务器时设定（无需要直接忽略）
            // httpProfile.WebProxy = Environment.GetEnvironmentVariable("HTTPS_PROXY");

            clientProfile.HttpProfile = httpProfile;
            /* 实例化要请求产品(以sms为例)的client对象
             * 第二个参数是地域信息，可以直接填写字符串ap-guangzhou，支持的地域列表参考 https://cloud.tencent.com/document/api/382/52071#.E5.9C.B0.E5.9F.9F.E5.88.97.E8.A1.A8 */
            SmsClient client = new SmsClient(cred, "ap-guangzhou", clientProfile);
            /* 实例化一个请求对象，根据调用的接口和实际情况，可以进一步设置请求参数
             * 你可以直接查询SDK源码确定SendSmsRequest有哪些属性可以设置
             * 属性可能是基本类型，也可能引用了另一个数据结构
             * 推荐使用IDE进行开发，可以方便的跳转查阅各个接口和数据结构的文档说明 */
            SendSmsRequest req = new SendSmsRequest();
            /* 基本类型的设置:
             * SDK采用的是指针风格指定参数，即使对于基本类型你也需要用指针来对参数赋值。
             * SDK提供对基本类型的指针引用封装函数
             * 帮助链接：
             * 短信控制台: https://console.cloud.tencent.com/smsv2
             * 腾讯云短信小助手: https://cloud.tencent.com/document/product/382/3773#.E6.8A.80.E6.9C.AF.E4.BA.A4.E6.B5.81 */
            /* 短信应用ID: 短信SdkAppId在 [短信控制台] 添加应用后生成的实际SdkAppId，示例如1400006666 */
            // 应用 ID 可前往 [短信控制台](https://console.cloud.tencent.com/smsv2/app-manage) 查看
            req.SmsSdkAppId = appId;
            /* 短信签名内容: 使用 UTF-8 编码，必须填写已审核通过的签名 */
            // 签名信息可前往 [国内短信](https://console.cloud.tencent.com/smsv2/csms-sign) 或 [国际/港澳台短信](https://console.cloud.tencent.com/smsv2/isms-sign) 的签名管理查看
            req.SignName = form.SignName;
            /* 模板 ID: 必须填写已审核通过的模板 ID */
            // 模板 ID 可前往 [国内短信](https://console.cloud.tencent.com/smsv2/csms-template) 或 [国际/港澳台短信](https://console.cloud.tencent.com/smsv2/isms-template) 的正文模板管理查看
            req.TemplateId = form.TemplateId;
            /* 模板参数: 模板参数的个数需要与 TemplateId 对应模板的变量个数保持一致，若无模板参数，则设置为空 */
            req.TemplateParamSet = form.Content.IsNullOrEmpty() ? new string[0] : form.Content.Split(',');
            /* 下发手机号码，采用 E.164 标准，+[国家或地区码][手机号]
             * 示例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号，最多不要超过200个手机号*/
            req.PhoneNumberSet = form.PhoneNumber.Split(',');
            /* 用户的 session 内容（无需要可忽略）: 可以携带用户侧 ID 等上下文信息，server 会原样返回 */
            req.SessionContext = "";
            /* 短信码号扩展号（无需要可忽略）: 默认未开通，如需开通请联系 [腾讯云短信小助手] */
            req.ExtendCode = "";
            /* 国内短信无需填写该项；国际/港澳台短信已申请独立 SenderId 需要填写该字段，默认使用公共 SenderId，无需填写该字段。注：月度使用量达到指定量级可申请独立 SenderId 使用，详情请联系 [腾讯云短信小助手](https://cloud.tencent.com/document/product/382/3773#.E6.8A.80.E6.9C.AF.E4.BA.A4.E6.B5.81)。 */
            req.SenderId = "";

            SendSmsResponse resp = client.SendSmsSync(req);

            // 输出json格式的字符串回包
            var str = AbstractModel.ToJsonString(resp);
            var result = StringHelper.MatchMiddleValue(str, "\"Code\":\"", "\"");
            var error = StringHelper.MatchMiddleValue(str, "\"Message\":\"", "\"");
            var success = result.ToLower() == "ok" ? true : false;

            return success ? msg.Success(error) : msg.Fail(error);
        }
    }
}
