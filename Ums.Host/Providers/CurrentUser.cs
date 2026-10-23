using Microsoft.AspNetCore.Http;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;
using System.Linq;
using Ums.Domain.Interfaces;

namespace Ums.Host.Providers
{
    /// <summary>
    /// 当前登录用户端口实现：从 JWT Claims 解析登录用户，并暴露当前请求路径
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public LoginUser LoginUser
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext?.User?.Claims;
                if (claims != null && claims.Any())
                {
                    return new LoginUser
                    {
                        DisplayName = claims.FirstOrDefault(e => e.Type == UserClaimType.DisplayName)?.Value ?? "",
                        UserName = claims.FirstOrDefault(e => e.Type == UserClaimType.UserName)?.Value ?? "",
                        WxAppId = claims.FirstOrDefault(e => e.Type == UserClaimType.WxAppId)?.Value ?? "",
                        WxOpenId = claims.FirstOrDefault(e => e.Type == UserClaimType.WxOpenId)?.Value ?? "",
                        WxUnionId = claims.FirstOrDefault(e => e.Type == UserClaimType.WxUnionId)?.Value ?? "",
                        Id = claims.FirstOrDefault(e => e.Type == UserClaimType.UserId)?.Value ?? "",
                        TenantId = claims.FirstOrDefault(e => e.Type == UserClaimType.TenantId)?.Value ?? "",
                        IsDefault = claims.FirstOrDefault(e => e.Type == UserClaimType.IsDefault)?.Value.TryBoolean() ?? false
                    };
                }
                return new LoginUser();
            }
        }

        /// <summary>
        /// 当前请求路径（非 HTTP 上下文调用时为 "/"）
        /// </summary>
        public string RequestPath
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null || context.Request == null)
                    return "/";

                var path = context.Request.Path;
                return path.HasValue ? path.Value : "/";
            }
        }
    }
}
