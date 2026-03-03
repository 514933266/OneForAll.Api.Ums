using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.HttpService.Models;

namespace Ums.HttpService.Interfaces
{
    /// <summary>
    /// 全局异常日志
    /// </summary>
    public interface ISysGlobalExceptionLogHttpService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task AddAsync(SysGlobalExceptionLogRequest entity);
    }
}
