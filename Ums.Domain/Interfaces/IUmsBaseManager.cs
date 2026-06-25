using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 基础管理器接口，定义了所有管理器的公共方法和属性
    /// </summary>
    public interface IUmsBaseManager
    {
        /// <summary>
        /// 仅记录消息数据（不发送）
        /// </summary>
        /// <param name="form">消息表单</param>
        /// <returns></returns>
        Task<BaseErrType> RecordAsync<T>(T form);
    }
}
