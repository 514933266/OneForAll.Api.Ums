using Microsoft.AspNetCore.Http;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using Ums.Public.Models;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core;

namespace Ums.HttpService
{
    /// <summary>
    /// 企业微信
    /// </summary>
    public class WxqyHttpService : BaseHttpService, IWxqyHttpService
    {
        private readonly HttpServiceConfig _config;
        public WxqyHttpService(
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
        public async Task<BaseMessage> SendRobotTextAsync(WxqyRobotTextRequest request, string url)
        {
            var msg = new BaseMessage();
            var client = new HttpClient();
            if (client != null)
            {
                request.MsgType = "text";
                var res = await client.PostAsync(new Uri(url), request, new JsonMediaTypeFormatter());
                var result = await res.Content.ReadAsAsync<WxqyRobotResponse>();
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
        public async Task<BaseMessage> SendRobotMarkdownAsync(WxqyRobotMarkdownRequest request, string url)
        {
            var msg = new BaseMessage();
            var client = new HttpClient();
            if (client != null)
            {
                request.MsgType = "markdown";
                var res = await client.PostAsync(new Uri(url), request, new JsonMediaTypeFormatter());
                var result = await res.Content.ReadAsAsync<WxqyRobotResponse>();
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

