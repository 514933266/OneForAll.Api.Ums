using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;

namespace Ums.Application
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    public class TxCloudSmsService : ITxCloudSmsService
    {
        private readonly IMapper _mapper;
        private readonly ITxCloudSmsManager _manager;
        public TxCloudSmsService(
            IMapper mapper,
            ITxCloudSmsManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(TxCloudSmsForm form)
        {
            return await _manager.SendAsync(form);
        }
    }
}