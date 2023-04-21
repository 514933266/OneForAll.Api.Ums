using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;

namespace Ums.Domain.Repositorys
{
    /// <summary>
    /// 失败消息记录
    /// </summary>
    public interface IUmsFailureMessageRecordRepository : IEFCoreRepository<UmsFailureMessageRecord>
    {
    }
}
