using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Models;

namespace Ums.Domain.Interfaces
{
    /// <summary>
    /// 腾讯云-短信发送
    /// </summary>
    public interface ITxCloudSmsManager
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<BaseMessage> SendAsync(TxCloudSmsForm form);
    }
}
