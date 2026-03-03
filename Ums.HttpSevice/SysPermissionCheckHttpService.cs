using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using OneForAll.Core.Extension;
using Ums.HttpService.Models;
using Ums.HttpService.Interfaces;
using OneForAll.Core.OAuth;

namespace Ums.HttpService
{
    /// <summary>
    /// 功能权限校验服务
    /// </summary>
    public class SysPermissionCheckHttpService : ISysPermissionCheckHttpService
    {
        private readonly string AUTH_KEY = "Authorization";
        private readonly HttpServiceConfig _config;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public SysPermissionCheckHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpContext = httpContext;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 登录token
        /// </summary>
        private string Token
        {
            get
            {
                var context = _httpContext.HttpContext;
                if (context != null)
                {
                    return context.Request.Headers
                      .FirstOrDefault(w => w.Key.Equals(AUTH_KEY))
                      .Value.TryString();
                }
                return "";
            }
        }

        /// <summary>
        /// 验证功能权限
        /// </summary>
        /// <returns>返回消息</returns>
        public async Task<BaseMessage> ValidateAuthorization(string controller, string action)
        {
            if (!Token.IsNullOrEmpty())
            {
                var claims = _httpContext.HttpContext.User.Claims;
                var uid = claims.FirstOrDefault(e => e.Type == UserClaimType.UserId).Value;

                var client = _httpClientFactory.CreateClient(_config.SysBase);
                client.DefaultRequestHeaders.Add(AUTH_KEY, Token);
                var postData = new SysPermissionCheckRequest()
                {
                    SysUserId = new Guid(uid),
                    Controller = controller,
                    Action = action
                };
                var url = $"{client.BaseAddress}api/SysPermissionCheck";
                var result = await client.PostAsync(url, postData, new JsonMediaTypeFormatter());
                return await result.Content.ReadAsAsync<BaseMessage>();
            }
            return new BaseMessage()
            {
                Status = false,
                ErrType = BaseErrType.TokenInvalid,
                Message = "登录已失效，权限验证失败"
            };
        }
    }
}
