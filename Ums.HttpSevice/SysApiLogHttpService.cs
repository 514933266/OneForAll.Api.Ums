using Microsoft.AspNetCore.Http;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using Ums.Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Ums.HttpService
{
    /// <summary>
    /// Api日志
    /// </summary>
    public class SysApiLogHttpService : BaseHttpService, ISysApiLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysApiLogHttpService(
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
        public async Task AddAsync(SysApiLogForm entity)
        {
            try
            {
                entity.CreatorId = LoginUser.Id;
                entity.CreatorName = LoginUser.Name;
                entity.TenantId = LoginUser.TenantId;

                var client = GetHttpClient(_config.SysApiLog);
                if (client != null)
                {
                    var res = await client.PostAsync(client.BaseAddress, entity, new JsonMediaTypeFormatter());
                    var b = res.Content.ReadAsStringAsync();
                }
            }
            catch
            {

            }
        }
    }
}

