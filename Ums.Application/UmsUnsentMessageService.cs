using System.Threading.Tasks;
using Ums.Application.Dtos;
using Ums.Application.Interfaces;
using Ums.Domain.Interfaces;

namespace Ums.Application
{
    /// <summary>
    /// 未发送消息重发
    /// </summary>
    public class UmsUnsentMessageService : IUmsUnsentMessageService
    {
        private readonly IUmsUnsentMessageManager _manager;

        public UmsUnsentMessageService(IUmsUnsentMessageManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// 重新投递当天未发送（超过一小时）的队列消息
        /// </summary>
        /// <returns>重发结果</returns>
        public async Task<UnsentMessageResendDto> ResendExpiredAsync()
        {
            var result = await _manager.ResendExpiredAsync();
            return new UnsentMessageResendDto
            {
                TotalCount = result.TotalCount,
                SuccessCount = result.SuccessCount,
                Message = $"监控当天未发送队列消息（超一小时）任务执行完成，共有{result.TotalCount}条记录重新发送，成功{result.SuccessCount}条"
            };
        }
    }
}
