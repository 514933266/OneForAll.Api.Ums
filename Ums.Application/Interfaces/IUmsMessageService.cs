using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUmsMessageService
    {
        Task<BaseErrType> SendAsync(UmsMessageForm form);
    }
}
