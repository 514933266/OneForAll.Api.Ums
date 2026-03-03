using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.Entities;
using Ums.Domain.Enums;
using Ums.Domain.Interfaces;
using Ums.Domain.Models;
using Ums.Domain.Repositorys;

namespace Ums.Application
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    public class TxCloudSmsService : ITxCloudSmsService
    {
        private readonly IMapper _mapper;
        private readonly ITxCloudSmsManager _manager;
        private readonly ITxCloudSmsDirectManager _directManager;
        public TxCloudSmsService(
            IMapper mapper,
            ITxCloudSmsManager manager,
            ITxCloudSmsDirectManager directManager)
        {
            _mapper = mapper;
            _manager = manager;
            _directManager = directManager;
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

        /// <summary>
        /// 直接发送短信（不经过MQ）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendDirectAsync(TxCloudSmsForm form)
        {
            return await _directManager.SendDirectAsync(form);
        }
    }
}