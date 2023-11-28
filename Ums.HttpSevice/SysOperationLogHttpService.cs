using Microsoft.AspNetCore.Http;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ums.HttpService
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysOperationLogHttpService : BaseHttpService, ISysOperationLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysOperationLogHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysOperationLogRequest entity)
        {
            entity.CreatorId = LoginUser.Id;
            entity.CreatorName = LoginUser.Name;
            entity.TenantId = LoginUser.SysTenantId;

            var client = GetHttpClient(_config.SysExceptionLog);
            if (client != null && client.BaseAddress != null)
            {
                await client.PostAsync(client.BaseAddress, entity, new JsonMediaTypeFormatter());
            }
        }
    }
}
