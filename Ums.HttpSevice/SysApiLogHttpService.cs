using Microsoft.AspNetCore.Http;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core;

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
        /// <param name="form">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysApiLogRequest form)
        {
            var client = GetHttpClient(_config.SysLog);
            if (client != null && client.BaseAddress != null)
            {
                await client.PostAsync("api/SysApiLogs", form, new JsonMediaTypeFormatter());
            }
        }
    }
}