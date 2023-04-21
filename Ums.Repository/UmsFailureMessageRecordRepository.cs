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
    /// 失败记录
    /// </summary>
    public class UmsFailureMessageRecordRepository : Repository<UmsFailureMessageRecord>, IUmsFailureMessageRecordRepository
    {
        public UmsFailureMessageRecordRepository(DbContext context)
            : base(context)
        {

        }
    }
}

