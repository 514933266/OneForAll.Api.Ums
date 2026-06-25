using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneForAll.Core.Extension;
using OneForAll.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;
using Ums.HttpService.Interfaces;
using Ums.HttpService.Models;
using Ums.Public.Models;

namespace Ums.Domain
{
    /// <summary>
    /// 消息去重管理器实现
    /// </summary>
    public class UmsMessageDeduplicationManager : IUmsMessageDeduplicationManager
    {
        private readonly AuthConfig _authConfig;
        private readonly IUmsDeduplicationConfigRepository _configRepository;
        private readonly IUmsDeduplicationRecordRepository _recordRepository;
        private readonly ISysGlobalExceptionLogHttpService _exceptionLogService;


        public UmsMessageDeduplicationManager(
            AuthConfig authConfig,
            IConfiguration configuration,
            IUmsDeduplicationConfigRepository configRepository,
            IUmsDeduplicationRecordRepository recordRepository,
            ISysGlobalExceptionLogHttpService exceptionLogService)
        {
            _authConfig = authConfig;
            _configRepository = configRepository;
            _recordRepository = recordRepository;
            _exceptionLogService = exceptionLogService;
        }

        /// <summary>
        /// 检查并处理消息去重
        /// </summary>
        public async Task<bool> CheckAsync(string title, string content, UmsMessageTypeEnum messageType)
        {
            try
            {
                // 1. 根据消息类型获取所有启用的配置（按优先级排序）
                var configs = await _configRepository.GetListAsync(w => w.MessageType == messageType && w.IsEnabled);

                // 如果没有配置，直接返回不需要去重
                if (configs == null || !configs.Any())
                    return false;

                // 2. 一次性查询该消息类型的所有去重记录（避免N+1查询问题）
                var allRecords = await _recordRepository.GetListAsync(w => w.MessageType == messageType);
                var recordDict = allRecords.ToDictionary(r => r.MessageKey, r => r);

                // 标记是否需要创建新记录
                var recordsToCreate = new List<UmsDeduplicationRecord>();

                // 3. 循环遍历每个配置进行检查
                foreach (var config in configs)
                {
                    // 4. 根据 MatchRule 生成对应的 Key
                    var dedupKey = GenerateHashKey(title, content, messageType, config.MatchRule);

                    // 5. 在内存中检查该 Key 是否存在且未过期
                    if (recordDict.TryGetValue(dedupKey, out var record))
                    {
                        bool isExpired = !record.ExpireTime.HasValue || DateTime.UtcNow >= record.ExpireTime.Value;
                        if (!isExpired)
                        {
                            // 未过期，需要去重
                            return true;
                        }
                        else
                        {
                            // 已过期，更新记录（允许发送）
                            await UpdateRecordAsync(record, config);
                        }
                    }
                    else
                    {
                        // 没有找到Record，创建新记录（允许发送）
                        await AddRecordAsync(dedupKey, messageType, config);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录全局异常日志
                await _exceptionLogService.AddAsync(new SysGlobalExceptionLogRequest
                {
                    ModuleName = _authConfig?.ClientName ?? "Ums",
                    ModuleCode = _authConfig?.ClientCode ?? "ums",
                    Name = $"消息去重检查异常: {ex.Message}",
                    Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
                });
            }

            return false;
        }

        /// <summary>
        /// 更新记录（用于已存在但已过期的记录）
        /// </summary>
        private async Task UpdateRecordAsync(UmsDeduplicationRecord record, UmsDeduplicationConfig config)
        {
            var now = DateTime.UtcNow;
            record.LastSendTime = now;
            record.SendCount++;

            // 根据策略设置过期时间
            if (config.Strategy == DeduplicationStrategyEnum.TimeWindow)
            {
                record.ExpireTime = now.AddMinutes(config.TimeWindowMinutes);
            }
            else
            {
                record.ExpireTime = now.AddDays(30);
            }

            await _recordRepository.UpdateAsync(record);
        }

        /// <summary>
        /// 创建新记录（用于首次发送的消息）
        /// </summary>
        private async Task AddRecordAsync(string dedupKey, UmsMessageTypeEnum messageType, UmsDeduplicationConfig config)
        {
            var now = DateTime.UtcNow;
            var record = new UmsDeduplicationRecord
            {
                MessageKey = dedupKey,
                MessageType = messageType,
                SendCount = 1,
                LastSendTime = now,
                CreateTime = now
            };

            // 根据策略设置过期时间
            if (config.Strategy == DeduplicationStrategyEnum.TimeWindow)
            {
                record.ExpireTime = now.AddMinutes(config.TimeWindowMinutes);
            }
            else
            {
                record.ExpireTime = now.AddDays(30);
            }

            await _recordRepository.AddAsync(record);
        }

        /// <summary>
        /// 生成去重key
        /// Key格式: dedup:{messageType}:{matchRule}:{contentHash}
        /// </summary>
        public string GenerateHashKey(string title, string content, UmsMessageTypeEnum messageType, DeduplicationMatchRuleEnum matchRule)
        {
            StringBuilder keyBuilder = new StringBuilder();

            // 添加messageType
            keyBuilder.Append(messageType.ToString());
            keyBuilder.Append(":");

            // 添加matchRule
            keyBuilder.Append((int)matchRule);
            keyBuilder.Append(":");

            // 根据匹配规则生成内容哈希
            string contentToHash = GetContentForMatching(title, content, messageType, matchRule);
            string contentHash = GenerateHash(contentToHash);

            keyBuilder.Append(contentHash);

            return keyBuilder.ToString();
        }

        /// <summary>
        /// 根据匹配规则获取用于哈希的内容
        /// </summary>
        private string GetContentForMatching(string title, string content, UmsMessageTypeEnum messageType, DeduplicationMatchRuleEnum matchRule)
        {
            switch (matchRule)
            {
                case DeduplicationMatchRuleEnum.Title:
                    return title;
                case DeduplicationMatchRuleEnum.TitleAndType:
                    return $"{title}|{messageType}";
                case DeduplicationMatchRuleEnum.Type:
                    return $"{messageType}";
                case DeduplicationMatchRuleEnum.Content:
                    return content;
                case DeduplicationMatchRuleEnum.ContentAndType:
                    return $"{content}|{messageType}";
                default:
                    return $"{title}|{content}|{messageType}";
            }
        }

        /// <summary>
        /// 生成SHA256哈希值（取前16位）
        /// </summary>
        private string GenerateHash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "empty";
            }

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                // 取前16个字符（8字节的十六进制表示）
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 8; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
