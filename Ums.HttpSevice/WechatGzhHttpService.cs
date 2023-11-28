using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;

namespace Ums.HttpService
{
    /// <summary>
    /// 微信公众号推送
    /// </summary>
    public class WechatGzhHttpService : BaseHttpService, IWechatGzhHttpService
    {
        private readonly HttpServiceConfig _config;
        public WechatGzhHttpService(
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
        public async Task<BaseMessage> SendTemplateAsync(WechatGzhTemplateRequest request, string accessToken)
        {
            var msg = new BaseMessage();
            var client = GetHttpClient(_config.WechatGzhTemplate);
            if (client != null && client.BaseAddress != null)
            {
                if (request.MiniProgram.AppId.IsNullOrEmpty())
                    request.MiniProgram = null;
                var url = $"{client.BaseAddress}?access_token={accessToken}";
                var requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new ByteArrayContent(Encoding.UTF8.GetBytes(request.ToJson()))
                };
                var result = await client.SendAsync(requestMessage);
                var data = await result.Content.ReadAsAsync<WechatGzhTemplateResponse>();
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
