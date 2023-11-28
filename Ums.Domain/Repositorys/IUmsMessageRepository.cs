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
    /// 系统消息
    /// </summary>
    public interface IUmsMessageRepository : IEFCoreRepository<UmsMessage>
    {
    }
}
