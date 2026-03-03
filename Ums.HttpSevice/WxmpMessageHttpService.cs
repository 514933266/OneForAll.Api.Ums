using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;

namespace Ums.HttpService
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WxmpMessageHttpService : BaseHttpService, IWxmpHttpService
    {
        private readonly HttpServiceConfig _config;
        public WxmpMessageHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="accessToken">请求凭证</param>
        /// <returns></returns>
        public async Task<BaseMessage> SendSubscribTemplateAsync(WxmpSubscribeTemplateMessageRequest request, string accessToken)
        {
            var msg = new BaseMessage();
            var client = GetHttpClient(_config.Weixin);
            if (client != null && client.BaseAddress != null)
            {
                var url = $"cgi-bin/message/subscribe/send?access_token={accessToken}";
                var requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new ByteArrayContent(Encoding.UTF8.GetBytes(request.ToJson()))
                };
                var result = await client.SendAsync(requestMessage);
                var data = await result.Content.ReadAsAsync<WxmpSubscribeTemplateMessageResponse>();
                if (data?.ErrCode == "0")
                {
                    return msg.Success(data.ErrMsg);
                }
                else
                {
                    return msg.Fail(data?.ErrMsg);
                }
            }
            else
            {
                return msg.Fail("请求客户端丢失");
            }
        }
    }
}
