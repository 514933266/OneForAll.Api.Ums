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

namespace Ums.Host.Filters
{
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

            var classAttrs = new List<CheckPermissionAttribute>();
            var methodAttrs = new List<CheckPermissionAttribute>();
            methodAttrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).OfType<CheckPermissionAttribute>().ToList());
            classAttrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<CheckPermissionAttribute>().ToList());

            if (methodAttrs.Count > 0 || classAttrs.Count > 0)
            {
                var msg = new BaseMessage();
                if (methodAttrs.Count > 0)
                {
                    msg = ValidateAuthorization(context, methodAttrs);
                }
                else
                {
                    msg = ValidateAuthorization(context, classAttrs);
                }

                if (msg.ErrType != BaseErrType.Success)
                {
                    context.Result = new JsonResult(new BaseMessage()
                    {
                        Status = false,
                        ErrType = BaseErrType.PermissionNotEnough,
                        Message = "对不起，您没有操作此功能的权限"
                    });
                }
            }
        }

        // 校验功能权限
        private BaseMessage ValidateAuthorization(AuthorizationFilterContext context, List<CheckPermissionAttribute> attrs)
        {
            var msg = new BaseMessage();
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            foreach (var attr in attrs)
            {
                controller = attr.Controller.IsNullOrEmpty() ? controller : attr.Controller;
                action = attr.Action.IsNullOrEmpty() ? action : attr.Action;
                msg = _httpPermService.ValidateAuthorization(controller, action).Result;
                if (msg.ErrType == BaseErrType.Success)
                    break;
            }
            return msg;
        }
    }

    /// <summary>
    /// 权限检测
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }

    }
}
