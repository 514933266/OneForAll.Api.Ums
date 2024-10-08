﻿using NuGet.Protocol;
using OneForAll.Core;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;
using Ums.Domain.ValueObjects;
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

        private readonly IUmsMessageRecordRepository _repository;

        private readonly IUmsMessageManager _sysMsgManager;
        private readonly IWxgzhManager _wxGzhMsgManager;
        private readonly IWxqyManager _wxQyMsgManager;
        private readonly IWxmpManager _wxmpMsgManager;

        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;

        public MonitorUnsentMessageJob(
            AuthConfig config,
            IUmsMessageRecordRepository repository,
            IUmsMessageManager sysMsgManager,
            IWxgzhManager wxGzhMsgManager,
            IWxqyManager wxQyMsgManager,
            IWxmpManager wxmpMsgManager,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _config = config;
            _repository = repository;
            _sysMsgManager = sysMsgManager;
            _wxGzhMsgManager = wxGzhMsgManager;
            _wxQyMsgManager = wxQyMsgManager;
            _wxmpMsgManager = wxmpMsgManager;
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
                    var errType = BaseErrType.Fail;
                    foreach (var item in data)
                    {
                        if (item.QueueName == UmsQueueName.System)
                        {
                            errType = await _sysMsgManager.SendAsync(item.QueueName, item.ToJson());
                        }
                        else if (item.QueueName == WxgzhQueueName.Template)
                        {
                            errType = await _wxGzhMsgManager.SendAsync(item.QueueName, item.ToJson());
                        }
                        else if (item.QueueName == WxgzhQueueName.Subscribe)
                        {
                            errType = await _wxGzhMsgManager.SendAsync(item.QueueName, item.ToJson());
                        }
                        else if (item.QueueName == WxQyRootQueueName.Text)
                        {
                            errType = await _wxQyMsgManager.SendAsync(item.QueueName, item.ToJson());
                        }
                        else if (item.QueueName == WxQyRootQueueName.Markdown)
                        {
                            errType = await _wxQyMsgManager.SendAsync(item.QueueName, item.ToJson());
                        }
                        else if (item.QueueName == WxmpQueueName.Subscribe)
                        {
                            errType = await _wxmpMsgManager.SendAsync(item.QueueName, item.ToJson());
                        }

                        if (errType == BaseErrType.Success) effected++;
                    }
                }
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorUnsentMessageJob).Name, $"监控当天未发送队列消息（超一小时）任务执行完成，共有{data.Count()}条记录重新发送，成功{effected}条");
            }
            catch (Exception ex)
            {
                await _logHttpService.AddAsync(new SysGlobalExceptionLogRequest
                {
                    MoudleName = _config.ClientName,
                    MoudleCode = _config.ClientCode,
                    Name = ex.Message,
                    Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
                });
            }
        }
    }
}
