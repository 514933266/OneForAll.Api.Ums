using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Repositorys;

namespace Ums.Repository
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class UmsMessageRepository : Repository<UmsMessage>, IUmsMessageRepository
    {
        public UmsMessageRepository(DbContext context)
            : base(context)
        {

        }
    }
}
