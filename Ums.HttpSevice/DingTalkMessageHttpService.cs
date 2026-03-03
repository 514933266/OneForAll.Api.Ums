using Microsoft.AspNetCore.Http;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core;

namespace Ums.HttpService
{
    /// <summary>
    /// 钉钉
    /// </summary>
    public class DingTalkMessageHttpService : BaseHttpService, IDingTalkHttpService
    {
        private readonly HttpServiceConfig _config;
        public DingTalkMessageHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 获取签名后的URL
        /// </summary>
        /// <param name="webhookUrl">Webhook地址</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        private string GetSignedUrl(string webhookUrl, string secret)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var stringToSign = $"{timestamp}\n{secret}";
            var encoding = Encoding.UTF8;
            using (var hmac = new HMACSHA256(encoding.GetBytes(secret)))
            {
                var hashBytes = hmac.ComputeHash(encoding.GetBytes(stringToSign));
                var sign = Uri.EscapeDataString(Convert.ToBase64String(hashBytes));
                return $"{webhookUrl}&timestamp={timestamp}&sign={sign}";
            }
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        public async Task<BaseMessage> SendRobotTextAsync(DingTalkRobotTextMessageRequest request, string url, string secret)
        {
            var msg = new BaseMessage();
            var client = new HttpClient();
            if (client != null)
            {
                request.MsgType = "text";
                var signedUrl = GetSignedUrl(url, secret);
                var res = await client.PostAsync(new Uri(signedUrl), request, new JsonMediaTypeFormatter());
                var result = await res.Content.ReadAsAsync<DingTalkRobotMessageResponse>();
                if (result.ErrCode == 0)
                {
                    return msg.Success(result.ErrMsg);
                }
                else
                {
                    return msg.Fail(result.ErrMsg);
                }
            }
            else
            {
                return msg.Fail("请求客户端丢失");
            }
        }

        /// <summary>
        /// 发送Markdown消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        public async Task<BaseMessage> SendRobotMarkdownAsync(DingTalkRobotMarkdownMessageRequest request, string url, string secret)
        {
            var msg = new BaseMessage();
            var client = new HttpClient();
            if (client != null)
            {
                request.MsgType = "markdown";
                var signedUrl = GetSignedUrl(url, secret);
                var res = await client.PostAsync(new Uri(signedUrl), request, new JsonMediaTypeFormatter());
                var result = await res.Content.ReadAsAsync<DingTalkRobotMessageResponse>();
                if (result.ErrCode == 0)
                {
                    return msg.Success(result.ErrMsg);
                }
                else
                {
                    return msg.Fail(result.ErrMsg);
                }
            }
            else
            {
                return msg.Fail("请求客户端丢失");
            }
        }
    }
}
