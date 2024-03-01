using AutoMapper;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Application.Interfaces;
using Ums.Domain.AggregateRoots;
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
        private readonly IUmsSmsRecordRepository _repository;
        public TxCloudSmsService(
            IMapper mapper,
            ITxCloudSmsManager manager,
            IUmsSmsRecordRepository repository)
        {
            _mapper = mapper;
            _manager = manager;
            _repository = repository;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<BaseErrType> SendAsync(TxCloudSmsForm form)
        {
            var msg = await _manager.SendAsync(form);
            await _repository.AddAsync(new UmsSmsRecord()
            {
                ErrMsg = msg.Message,
                PlatformName = "腾讯云",
                Content = form.Content,
                PhoneNumber = form.PhoneNumber,
                MoudleCode = form.MoudleCode,
                MoudleName = form.MoudleName,
                SignName = form.SignName,
                Status = msg.Status ? UmsSmsSendStatusEnum.Success : UmsSmsSendStatusEnum.Error,
            });
            return msg.ErrType;
        }
    }
}