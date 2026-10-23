using OneForAll.Core;
using OneForAll.Core.Extension;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ums.Domain.Entities;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;

namespace Ums.Domain.Managers
{
    /// <summary>
    /// 未发送消息重发
    /// </summary>
    public class UmsUnsentMessageManager : UmsBaseMQManager, IUmsUnsentMessageManager
    {
        public override string QueueName => "direct.ums.queue";

        public override string RouteKey => "direct.ums.route";

        public UmsUnsentMessageManager(
            ConnectionFactory mqFactory,
            ICurrentUser currentUser,
            IUmsMessageRecordRepository repository) : base(mqFactory, currentUser, repository)
        {
        }

        /// <summary>
        /// 重新投递当天未发送（超过一小时）的队列消息
        /// </summary>
        /// <returns>重发结果</returns>
        public async Task<UnsentMessageResendResult> ResendExpiredAsync()
        {
            var result = new UnsentMessageResendResult();
            var data = await _repository.GetListExpiryANTAsync();
            var list = data == null ? new List<UmsMessageRecord>() : data.ToList();
            result.TotalCount = list.Count;

            foreach (var item in list)
            {
                var errType = await SendToRabbitMQAsync(item.QueueName, item.RouteKey, item.ToJson());
                if (errType == BaseErrType.Success)
                    result.SuccessCount++;
            }
            return result;
        }
    }
}
