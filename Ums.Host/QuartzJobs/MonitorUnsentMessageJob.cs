using OneForAll.Core;
using Quartz;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Host.Models;
using Ums.HttpService.Interfaces;
using Ums.Public.Models;

namespace Ums.Host.QuartzJobs
{
    /// <summary>
    /// 监控当天未发送队列消息（超一小时）
    /// </summary>
    [DisallowConcurrentExecution]
    public class MonitorUnsentMessageJob : BaseLockJob
    {
        private readonly IUmsUnsentMessageService _unsentMessageService;

        public MonitorUnsentMessageJob(
            IUmsUnsentMessageService unsentMessageService,
            QuartzScheduleJobConfig jobConfig,
            AuthConfig authConfig,
            JobLockConfig lockConfig,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
            : base(jobConfig, authConfig, lockConfig, jobHttpService, logHttpService)
        {
            _unsentMessageService = unsentMessageService;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        protected override async Task ExecuteInternalAsync(IJobExecutionContext context)
        {
            var result = await _unsentMessageService.ResendExpiredAsync();
            await AddLogAsync(result.Message);
        }
    }
}
