using Microsoft.AspNetCore.Http;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using OneForAll.Core;

namespace Ums.HttpService
{
    /// <summary>
    /// 企业微信
    /// </summary>
    public class WxqyMessageHttpService : BaseHttpService, IWxqyHttpService
    {
        private readonly HttpServiceConfig _config;
        public WxqyMessageHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="url">机器人地址</param>
        /// <returns></returns>
        public async Task<BaseMessage> SendRobotTextAsync(WxqyRobotTextMessageRequest request, string url)
        {
            var msg = new BaseMessage();
            var client = new HttpClient();
            if (client != null)
            {
                request.MsgType = "text";
                var res = await client.PostAsync(new Uri(url), request, new JsonMediaTypeFormatter());
                var result = await res.Content.ReadAsAsync<WxqyRobotMessageResponse>();
                if (result.ErrCode == "0")
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
        /// <returns></returns>
        public async Task<BaseMessage> SendRobotMarkdownAsync(WxqyRobotMarkdownMessageRequest request, string url)
        {
            var msg = new BaseMessage();
            var client = new HttpClient();
            if (client != null)
            {
                request.MsgType = "markdown";
                var res = await client.PostAsync(new Uri(url), request, new JsonMediaTypeFormatter());
                var result = await res.Content.ReadAsAsync<WxqyRobotMessageResponse>();
                if (result.ErrCode == "0")
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

