using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Authorization;
using OneForAll.Core;
using OneForAll.Core.Extension;
using Ums.HttpService.Interfaces;
using Ums.Host.Models;
using OneForAll.Core.Security;

namespace Ums.Host.Filters
{
    /// <summary>
    /// 过滤器：全局授权
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly AuthConfig _config;
        private readonly ISysPermissionCheckHttpService _httpPermService;
        public AuthorizationFilter(AuthConfig config, ISysPermissionCheckHttpService httpPermService)
        {
            _config = config;
            _httpPermService = httpPermService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter) ||
                !(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return;
            }
            var unChecked = context.HttpContext.Request.Headers["Unchecked"];
            if (!unChecked.IsNull())
            {
                // 不检查权限
                var signs = GetListSign();
                if (signs.Any(w => w == unChecked.ToString())) return;
            };

            var attrs = new List<object>();
            attrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            attrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            var checkPermAttrs = attrs.OfType<CheckPermissionAttribute>().ToList();
            var claims = context.HttpContext.User.Claims;
            if (checkPermAttrs.Count > 0)
            {
                var controller = context.ActionDescriptor.RouteValues["controller"];
                var action = context.ActionDescriptor.RouteValues["action"];
                if (!checkPermAttrs.First().Controller.IsNullOrEmpty()) controller = checkPermAttrs.First().Controller;
                if (!checkPermAttrs.First().Action.IsNullOrEmpty()) action = checkPermAttrs.First().Action;
                var msg = _httpPermService.ValidateAuthorization(controller, action).Result;
                if (msg.ErrType.Equals(BaseErrType.Success))
                {
                    return;
                }
                else
                {
                    context.Result = new JsonResult(msg);
                }
            }
        }

        // 获取签名
        private HashSet<string> GetListSign()
        {
            int seed = 5;
            var result = new HashSet<string>();
            for (int i = 0; i < seed; i++)
            {
                var sign = "clientId={0}&clientSecret={1}&apiName={2}&tt={3}".Fmt(_config.ClientId, _config.ClientSecret, _config.ApiName, DateTime.Now.AddMinutes(-1).ToString("yyyyMMddhhmm")).ToMd5();
                result.Add(sign);
            }
            return result;
        }
    }

    /// <summary>
    /// 权限检测
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }

    }
}
