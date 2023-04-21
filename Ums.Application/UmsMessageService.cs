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
    /// 
    /// </summary>
    public class UmsMessageService : IUmsMessageService
    {
        private readonly IMapper _mapper;
        private readonly IUmsMessageManager _manager;
        public UmsMessageService(
            IMapper mapper,
            IUmsMessageManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        public async Task<BaseErrType> SendAsync(UmsMessageForm form)
        {
            return await _manager.SendAsync(form);
        }
    }
}
