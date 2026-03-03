using NuGet.Protocol;
using OneForAll.Core;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;
using Ums.Host.Models;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;

namespace Ums.Host.QuartzJobs
{
    /// <summary>
    /// 监控当天未发送队列消息（超一小时）
    /// </summary>
    [DisallowConcurrentExecution]
    public class MonitorUnsentMessageJob : IJob
    {
        private readonly AuthConfig _config;
        private readonly UmsBaseMQManager _mqManager;
        private readonly IUmsMessageRecordRepository _repository;

        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;

        public MonitorUnsentMessageJob(
            AuthConfig config,
            UmsBaseMQManager mqManager,
            IUmsMessageRecordRepository repository,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _config = config;
            _mqManager = mqManager;
            _repository = repository;
            _jobHttpService = jobHttpService;
            _logHttpService = logHttpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var effected = 0;
                var data = await _repository.GetListExpiryANTAsync();
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        var errType = await _mqManager.SendDirectAsync(item.QueueName, item.RouteKey, item.ToJson());

                        if (errType == BaseErrType.Success)
                            effected++;
                    }
                }
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorUnsentMessageJob).Name, $"监控当天未发送队列消息（超一小时）任务执行完成，共有{data.Count()}条记录重新发送，成功{effected}条");
            }
            catch (Exception ex)
            {
                await _logHttpService.AddAsync(new SysGlobalExceptionLogRequest
                {
                    ModuleName = _config.ClientName,
                    ModuleCode = _config.ClientCode,
                    Name = ex.Message,
                    Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
                });
            }
        }
    }
}
