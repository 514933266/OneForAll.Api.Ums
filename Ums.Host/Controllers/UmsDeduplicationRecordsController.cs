using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Ums.Domain.Models;
using Ums.Domain.Interfaces;
using Ums.Domain.Repositorys;
using Ums.Domain.Entities;
using Ums.Domain.Enums;

namespace Ums.Host.Controllers
{
    /// <summary>
    /// 消息去重记录
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UmsDeduplicationRecordsController : BaseController
    {
        private readonly IUmsMessageDeduplicationManager _deduplicationManager;
        private readonly IUmsDeduplicationRecordRepository _recordRepository;

        public UmsDeduplicationRecordsController(
            IUmsMessageDeduplicationManager deduplicationManager,
            IUmsDeduplicationRecordRepository recordRepository)
        {
            _deduplicationManager = deduplicationManager;
            _recordRepository = recordRepository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="key">去重key关键字</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>分页</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<UmsDeduplicationRecord>> GetPageAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] DateTime? startTime = default,
            [FromQuery] DateTime? endTime = default,
            [FromQuery] string key = default,
            [FromQuery] UmsMessageTypeEnum? messageType = default)
        {
            return await _recordRepository.GetPageAsync(pageIndex, pageSize, startTime, endTime, key, messageType);
        }

        /// <summary>
        /// 添加去重记录
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody] UmsDeduplicationRecordForm form)
        {
            var msg = new BaseMessage();

            try
            {
                // 生成去重key
                var messageKey = _deduplicationManager.GenerateHashKey(
                    form.Title,
                    form.Content,
                    form.MessageType,
                    form.MatchRule);

                // 检查是否已存在
                var existingRecord = await _recordRepository.GetByKeyAsync(messageKey);
                if (existingRecord != null)
                {
                    msg.ErrType = BaseErrType.DataExist;
                    return msg.Fail("该去重记录已存在");
                }

                // 创建新记录
                var record = new UmsDeduplicationRecord
                {
                    Id = Guid.NewGuid(),
                    MessageKey = messageKey,
                    MessageType = form.MessageType,
                    SendCount = 1,
                    LastSendTime = DateTime.UtcNow,
                    CreateTime = DateTime.UtcNow
                };

                var result = await _recordRepository.AddAsync(record);

                if (result > 0)
                {
                    msg.ErrType = BaseErrType.Success;
                    return msg.Success("添加成功");
                }
                else
                {
                    msg.ErrType = BaseErrType.DataError;
                    return msg.Fail("添加失败");
                }
            }
            catch (Exception ex)
            {
                msg.ErrType = BaseErrType.DataError;
                msg.Message = ex.Message;
                return msg.Fail($"添加失败: {ex.Message}");
            }
        }
    }
}
